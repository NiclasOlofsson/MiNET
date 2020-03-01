#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/MiNET/blob/master/LICENSE.
// The License is based on the Mozilla Public License Version 1.1, but Sections 14
// and 15 have been added to cover use of software over a computer network and
// provide for limited attribution for the Original Developer. In addition, Exhibit A has
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is MiNET.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.IO;
using System.Threading;

namespace MiNET.Test.Streaming
{
	public sealed class RecyclableMemoryStreamManager
	{
		public const int DefaultBlockSize = 131072;
		public const int DefaultLargeBufferMultiple = 1048576;
		public const int DefaultMaximumBufferSize = 134217728;
		private readonly long[] largeBufferFreeSize;
		private readonly long[] largeBufferInUseSize;
		private readonly ConcurrentStack<byte[]>[] largePools;
		private readonly ConcurrentStack<byte[]> smallPool;
		private long smallPoolFreeSize;
		private long smallPoolInUseSize;

		public RecyclableMemoryStreamManager()
			: this(131072, 1048576, 134217728, false)
		{
		}

		public RecyclableMemoryStreamManager(
			int blockSize,
			int largeBufferMultiple,
			int maximumBufferSize)
			: this(blockSize, largeBufferMultiple, maximumBufferSize, false)
		{
		}

		public RecyclableMemoryStreamManager(
			int blockSize,
			int largeBufferMultiple,
			int maximumBufferSize,
			bool useExponentialLargeBuffer)
		{
			if (blockSize <= 0)
				throw new ArgumentOutOfRangeException(nameof(blockSize), (object) blockSize, "blockSize must be a positive number");
			if (largeBufferMultiple <= 0)
				throw new ArgumentOutOfRangeException(nameof(largeBufferMultiple), "largeBufferMultiple must be a positive number");
			if (maximumBufferSize < blockSize)
				throw new ArgumentOutOfRangeException(nameof(maximumBufferSize), "maximumBufferSize must be at least blockSize");
			this.BlockSize = blockSize;
			this.LargeBufferMultiple = largeBufferMultiple;
			this.MaximumBufferSize = maximumBufferSize;
			this.UseExponentialLargeBuffer = useExponentialLargeBuffer;
			if (!this.IsLargeBufferSize(maximumBufferSize))
				throw new ArgumentException(string.Format("maximumBufferSize is not {0} of largeBufferMultiple", this.UseExponentialLargeBuffer ? (object) "an exponential" : (object) "a multiple"), nameof(maximumBufferSize));
			this.smallPool = new ConcurrentStack<byte[]>();
			int length = useExponentialLargeBuffer ? (int) Math.Log((double) (maximumBufferSize / largeBufferMultiple), 2.0) + 1 : maximumBufferSize / largeBufferMultiple;
			this.largeBufferInUseSize = new long[length + 1];
			this.largeBufferFreeSize = new long[length];
			this.largePools = new ConcurrentStack<byte[]>[length];
			for (int index = 0; index < this.largePools.Length; ++index)
				this.largePools[index] = new ConcurrentStack<byte[]>();
			Events.Writer.MemoryStreamManagerInitialized(blockSize, largeBufferMultiple, maximumBufferSize);
		}

		public int BlockSize { get; }

		public int LargeBufferMultiple { get; }

		public bool UseMultipleLargeBuffer
		{
			get
			{
				return !this.UseExponentialLargeBuffer;
			}
		}

		public bool UseExponentialLargeBuffer { get; }

		public int MaximumBufferSize { get; }

		public long SmallPoolFreeSize
		{
			get
			{
				return this.smallPoolFreeSize;
			}
		}

		public long SmallPoolInUseSize
		{
			get
			{
				return this.smallPoolInUseSize;
			}
		}

		public long LargePoolFreeSize
		{
			get
			{
				long num1 = 0;
				foreach (long num2 in this.largeBufferFreeSize)
					num1 += num2;
				return num1;
			}
		}

		public long LargePoolInUseSize
		{
			get
			{
				long num1 = 0;
				foreach (long num2 in this.largeBufferInUseSize)
					num1 += num2;
				return num1;
			}
		}

		public long SmallBlocksFree
		{
			get
			{
				return (long) this.smallPool.Count;
			}
		}

		public long LargeBuffersFree
		{
			get
			{
				long num = 0;
				foreach (ConcurrentStack<byte[]> largePool in this.largePools)
					num += (long) largePool.Count;
				return num;
			}
		}

		public long MaximumFreeSmallPoolBytes { get; set; }

		public long MaximumFreeLargePoolBytes { get; set; }

		public long MaximumStreamCapacity { get; set; }

		public bool GenerateCallStacks { get; set; }

		public bool AggressiveBufferReturn { get; set; }

		internal byte[] GetBlock()
		{
			byte[] result;
			if (!this.smallPool.TryPop(out result))
			{
				result = new byte[this.BlockSize];
				Events.Writer.MemoryStreamNewBlockCreated(this.smallPoolInUseSize);
				this.ReportBlockCreated();
			}
			else
				Interlocked.Add(ref this.smallPoolFreeSize, (long) -this.BlockSize);
			Interlocked.Add(ref this.smallPoolInUseSize, (long) this.BlockSize);
			return result;
		}

		internal byte[] GetLargeBuffer(int requiredSize, string tag)
		{
			requiredSize = this.RoundToLargeBufferSize(requiredSize);
			int index = this.GetPoolIndex(requiredSize);
			byte[] result;
			if (index < this.largePools.Length)
			{
				if (!this.largePools[index].TryPop(out result))
				{
					result = new byte[requiredSize];
					Events.Writer.MemoryStreamNewLargeBufferCreated(requiredSize, this.LargePoolInUseSize);
					this.ReportLargeBufferCreated();
				}
				else
					Interlocked.Add(ref this.largeBufferFreeSize[index], (long) -result.Length);
			}
			else
			{
				index = this.largeBufferInUseSize.Length - 1;
				result = new byte[requiredSize];
				string allocationStack = (string) null;
				if (this.GenerateCallStacks)
					allocationStack = Environment.StackTrace;
				Events.Writer.MemoryStreamNonPooledLargeBufferCreated(requiredSize, tag, allocationStack);
				this.ReportLargeBufferCreated();
			}
			Interlocked.Add(ref this.largeBufferInUseSize[index], (long) result.Length);
			return result;
		}

		private int RoundToLargeBufferSize(int requiredSize)
		{
			if (!this.UseExponentialLargeBuffer)
				return (requiredSize + this.LargeBufferMultiple - 1) / this.LargeBufferMultiple * this.LargeBufferMultiple;
			int num = 1;
			while (this.LargeBufferMultiple * num < requiredSize)
				num <<= 1;
			return this.LargeBufferMultiple * num;
		}

		private bool IsLargeBufferSize(int value)
		{
			if (value == 0)
				return false;
			return !this.UseExponentialLargeBuffer ? value % this.LargeBufferMultiple == 0 : value == this.RoundToLargeBufferSize(value);
		}

		private int GetPoolIndex(int length)
		{
			if (!this.UseExponentialLargeBuffer)
				return length / this.LargeBufferMultiple - 1;
			int num = 0;
			while (this.LargeBufferMultiple << num < length)
				++num;
			return num;
		}

		internal void ReturnLargeBuffer(byte[] buffer, string tag)
		{
			if (buffer == null)
				throw new ArgumentNullException(nameof(buffer));
			if (!this.IsLargeBufferSize(buffer.Length))
				throw new ArgumentException(string.Format("buffer did not originate from this memory manager. The size is not {0} of ", this.UseExponentialLargeBuffer ? (object) "an exponential" : (object) "a multiple") + (object) this.LargeBufferMultiple);
			int index = this.GetPoolIndex(buffer.Length);
			if (index < this.largePools.Length)
			{
				if ((long) ((this.largePools[index].Count + 1) * buffer.Length) <= this.MaximumFreeLargePoolBytes || this.MaximumFreeLargePoolBytes == 0L)
				{
					this.largePools[index].Push(buffer);
					Interlocked.Add(ref this.largeBufferFreeSize[index], (long) buffer.Length);
				}
				else
				{
					Events.Writer.MemoryStreamDiscardBuffer(Events.MemoryStreamBufferType.Large, tag, Events.MemoryStreamDiscardReason.EnoughFree);
					this.ReportLargeBufferDiscarded(Events.MemoryStreamDiscardReason.EnoughFree);
				}
			}
			else
			{
				index = this.largeBufferInUseSize.Length - 1;
				Events.Writer.MemoryStreamDiscardBuffer(Events.MemoryStreamBufferType.Large, tag, Events.MemoryStreamDiscardReason.TooLarge);
				this.ReportLargeBufferDiscarded(Events.MemoryStreamDiscardReason.TooLarge);
			}
			Interlocked.Add(ref this.largeBufferInUseSize[index], (long) -buffer.Length);
			this.ReportUsageReport(this.smallPoolInUseSize, this.smallPoolFreeSize, this.LargePoolInUseSize, this.LargePoolFreeSize);
		}

		internal void ReturnBlocks(ICollection<byte[]> blocks, string tag)
		{
			if (blocks == null)
				throw new ArgumentNullException(nameof(blocks));
			Interlocked.Add(ref this.smallPoolInUseSize, (long) -(blocks.Count * this.BlockSize));
			foreach (byte[] block in (IEnumerable<byte[]>) blocks)
			{
				if (block == null || block.Length != this.BlockSize)
					throw new ArgumentException("blocks contains buffers that are not BlockSize in length");
			}
			foreach (byte[] block in (IEnumerable<byte[]>) blocks)
			{
				if (this.MaximumFreeSmallPoolBytes == 0L || this.SmallPoolFreeSize < this.MaximumFreeSmallPoolBytes)
				{
					Interlocked.Add(ref this.smallPoolFreeSize, (long) this.BlockSize);
					this.smallPool.Push(block);
				}
				else
				{
					Events.Writer.MemoryStreamDiscardBuffer(Events.MemoryStreamBufferType.Small, tag, Events.MemoryStreamDiscardReason.EnoughFree);
					this.ReportBlockDiscarded();
					break;
				}
			}
			//this.ReportUsageReport(this.smallPoolInUseSize, this.smallPoolFreeSize, 0, 0);
			this.ReportUsageReport(this.smallPoolInUseSize, this.smallPoolFreeSize, this.LargePoolInUseSize, this.LargePoolFreeSize);
		}

		internal void ReportBlockCreated()
		{
			EventHandler blockCreated = this.BlockCreated;
			if (blockCreated == null)
				return;
			blockCreated();
		}

		internal void ReportBlockDiscarded()
		{
			EventHandler blockDiscarded = this.BlockDiscarded;
			if (blockDiscarded == null)
				return;
			blockDiscarded();
		}

		internal void ReportLargeBufferCreated()
		{
			EventHandler largeBufferCreated = this.LargeBufferCreated;
			if (largeBufferCreated == null)
				return;
			largeBufferCreated();
		}

		internal void ReportLargeBufferDiscarded(
			Events.MemoryStreamDiscardReason reason)
		{
			LargeBufferDiscardedEventHandler largeBufferDiscarded = this.LargeBufferDiscarded;
			if (largeBufferDiscarded == null)
				return;
			largeBufferDiscarded(reason);
		}

		internal void ReportStreamCreated()
		{
			EventHandler streamCreated = this.StreamCreated;
			if (streamCreated == null)
				return;
			streamCreated();
		}

		internal void ReportStreamDisposed()
		{
			EventHandler streamDisposed = this.StreamDisposed;
			if (streamDisposed == null)
				return;
			streamDisposed();
		}

		internal void ReportStreamFinalized()
		{
			EventHandler streamFinalized = this.StreamFinalized;
			if (streamFinalized == null)
				return;
			streamFinalized();
		}

		internal void ReportStreamLength(long bytes)
		{
			StreamLengthReportHandler streamLength = this.StreamLength;
			if (streamLength == null)
				return;
			streamLength(bytes);
		}

		internal void ReportStreamToArray()
		{
			EventHandler convertedToArray = this.StreamConvertedToArray;
			if (convertedToArray == null)
				return;
			convertedToArray();
		}

		internal void ReportUsageReport(
			long smallPoolInUseBytes,
			long smallPoolFreeBytes,
			long largePoolInUseBytes,
			long largePoolFreeBytes)
		{
			UsageReportEventHandler usageReport = this.UsageReport;
			if (usageReport == null)
				return;
			usageReport(smallPoolInUseBytes, smallPoolFreeBytes, largePoolInUseBytes, largePoolFreeBytes);
		}

		public MemoryStream GetStream()
		{
			return (MemoryStream) new RecyclableMemoryStream(this);
		}

		public MemoryStream GetStream(Guid id)
		{
			return (MemoryStream) new RecyclableMemoryStream(this, id);
		}

		public MemoryStream GetStream(string tag)
		{
			return (MemoryStream) new RecyclableMemoryStream(this, tag);
		}

		public MemoryStream GetStream(Guid id, string tag)
		{
			return (MemoryStream) new RecyclableMemoryStream(this, id, tag);
		}

		public MemoryStream GetStream(string tag, int requiredSize)
		{
			return (MemoryStream) new RecyclableMemoryStream(this, tag, requiredSize);
		}

		public MemoryStream GetStream(Guid id, string tag, int requiredSize)
		{
			return (MemoryStream) new RecyclableMemoryStream(this, id, tag, requiredSize);
		}

		public MemoryStream GetStream(
			Guid id,
			string tag,
			int requiredSize,
			bool asContiguousBuffer)
		{
			return !asContiguousBuffer || requiredSize <= this.BlockSize ? this.GetStream(id, tag, requiredSize) : (MemoryStream) new RecyclableMemoryStream(this, id, tag, requiredSize, this.GetLargeBuffer(requiredSize, tag));
		}

		public MemoryStream GetStream(
			string tag,
			int requiredSize,
			bool asContiguousBuffer)
		{
			return this.GetStream(Guid.NewGuid(), tag, requiredSize, asContiguousBuffer);
		}

		public MemoryStream GetStream(
			Guid id,
			string tag,
			byte[] buffer,
			int offset,
			int count)
		{
			RecyclableMemoryStream recyclableMemoryStream = (RecyclableMemoryStream) null;
			try
			{
				recyclableMemoryStream = new RecyclableMemoryStream(this, id, tag, count);
				recyclableMemoryStream.Write(buffer, offset, count);
				recyclableMemoryStream.Position = 0L;
				return (MemoryStream) recyclableMemoryStream;
			}
			catch
			{
				recyclableMemoryStream?.Dispose();
				throw;
			}
		}

		public MemoryStream GetStream(string tag, byte[] buffer, int offset, int count)
		{
			return this.GetStream(Guid.NewGuid(), tag, buffer, offset, count);
		}

		public event EventHandler BlockCreated;

		public event EventHandler BlockDiscarded;

		public event EventHandler LargeBufferCreated;

		public event EventHandler StreamCreated;

		public event EventHandler StreamDisposed;

		public event EventHandler StreamFinalized;

		public event StreamLengthReportHandler StreamLength;

		public event EventHandler StreamConvertedToArray;

		public event LargeBufferDiscardedEventHandler LargeBufferDiscarded;

		public event UsageReportEventHandler UsageReport;

		[EventSource(Guid = "{B80CD4E4-890E-468D-9CBA-90EB7C82DFC7}", Name = "Microsoft-IO-RecyclableMemoryStream")]
		public sealed class Events : EventSource
		{
			public static Events Writer = new Events();

			[Event(1, Level = EventLevel.Verbose)]
			public void MemoryStreamCreated(Guid guid, string tag, int requestedSize)
			{
				if (!this.IsEnabled(EventLevel.Verbose, EventKeywords.None))
					return;
				this.WriteEvent(1, (object) guid, (object) (tag ?? string.Empty), (object) requestedSize);
			}

			[Event(2, Level = EventLevel.Verbose)]
			public void MemoryStreamDisposed(Guid guid, string tag)
			{
				if (!this.IsEnabled(EventLevel.Verbose, EventKeywords.None))
					return;
				this.WriteEvent(2, (object) guid, (object) (tag ?? string.Empty));
			}

			[Event(3, Level = EventLevel.Critical)]
			public void MemoryStreamDoubleDispose(
				Guid guid,
				string tag,
				string allocationStack,
				string disposeStack1,
				string disposeStack2)
			{
				if (!this.IsEnabled())
					return;
				this.WriteEvent(3, (object) guid, (object) (tag ?? string.Empty), (object) (allocationStack ?? string.Empty), (object) (disposeStack1 ?? string.Empty), (object) (disposeStack2 ?? string.Empty));
			}

			[Event(4, Level = EventLevel.Error)]
			public void MemoryStreamFinalized(Guid guid, string tag, string allocationStack)
			{
				if (!this.IsEnabled())
					return;
				this.WriteEvent(4, (object) guid, (object) (tag ?? string.Empty), (object) (allocationStack ?? string.Empty));
			}

			[Event(5, Level = EventLevel.Verbose)]
			public void MemoryStreamToArray(Guid guid, string tag, string stack, int size)
			{
				if (!this.IsEnabled(EventLevel.Verbose, EventKeywords.None))
					return;
				this.WriteEvent(5, (object) guid, (object) (tag ?? string.Empty), (object) (stack ?? string.Empty), (object) size);
			}

			[Event(6, Level = EventLevel.Informational)]
			public void MemoryStreamManagerInitialized(
				int blockSize,
				int largeBufferMultiple,
				int maximumBufferSize)
			{
				if (!this.IsEnabled())
					return;
				this.WriteEvent(6, blockSize, largeBufferMultiple, maximumBufferSize);
			}

			[Event(7, Level = EventLevel.Verbose)]
			public void MemoryStreamNewBlockCreated(long smallPoolInUseBytes)
			{
				if (!this.IsEnabled(EventLevel.Verbose, EventKeywords.None))
					return;
				this.WriteEvent(7, smallPoolInUseBytes);
			}

			[Event(8, Level = EventLevel.Verbose)]
			public void MemoryStreamNewLargeBufferCreated(int requiredSize, long largePoolInUseBytes)
			{
				if (!this.IsEnabled(EventLevel.Verbose, EventKeywords.None))
					return;
				this.WriteEvent(8, (long) requiredSize, largePoolInUseBytes);
			}

			[Event(9, Level = EventLevel.Verbose)]
			public void MemoryStreamNonPooledLargeBufferCreated(
				int requiredSize,
				string tag,
				string allocationStack)
			{
				if (!this.IsEnabled(EventLevel.Verbose, EventKeywords.None))
					return;
				this.WriteEvent(9, (object) requiredSize, (object) (tag ?? string.Empty), (object) (allocationStack ?? string.Empty));
			}

			[Event(10, Level = EventLevel.Warning)]
			public void MemoryStreamDiscardBuffer(
				MemoryStreamBufferType bufferType,
				string tag,
				MemoryStreamDiscardReason reason)
			{
				if (!this.IsEnabled())
					return;
				this.WriteEvent(10, (object) bufferType, (object) (tag ?? string.Empty), (object) reason);
			}

			[Event(11, Level = EventLevel.Error)]
			public void MemoryStreamOverCapacity(
				int requestedCapacity,
				long maxCapacity,
				string tag,
				string allocationStack)
			{
				if (!this.IsEnabled())
					return;
				this.WriteEvent(11, (object) requestedCapacity, (object) maxCapacity, (object) (tag ?? string.Empty), (object) (allocationStack ?? string.Empty));
			}

			public enum MemoryStreamBufferType
			{
				Small,
				Large,
			}

			public enum MemoryStreamDiscardReason
			{
				TooLarge,
				EnoughFree,
			}
		}

		public delegate void EventHandler();

		public delegate void LargeBufferDiscardedEventHandler(
			Events.MemoryStreamDiscardReason reason);

		public delegate void StreamLengthReportHandler(long bytes);

		public delegate void UsageReportEventHandler(
			long smallPoolInUseBytes,
			long smallPoolFreeBytes,
			long largePoolInUseBytes,
			long largePoolFreeBytes);
	}
}