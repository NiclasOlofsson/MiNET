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
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace MiNET.Test.Streaming
{
	public sealed class RecyclableMemoryStream : MemoryStream
	{
		private static readonly byte[] emptyArray = new byte[0];
		private readonly List<byte[]> blocks = new List<byte[]>(1);
		private readonly byte[] byteBuffer = new byte[1];
		private const long MaxStreamLength = 2147483647;
		private readonly Guid id;
		private readonly RecyclableMemoryStreamManager memoryManager;
		private readonly string tag;
		private List<byte[]> dirtyBuffers;
		private long disposedState;
		private byte[] largeBuffer;
		private int length;
		private int position;

		internal Guid Id
		{
			get
			{
				this.CheckDisposed();
				return this.id;
			}
		}

		internal string Tag
		{
			get
			{
				this.CheckDisposed();
				return this.tag;
			}
		}

		internal RecyclableMemoryStreamManager MemoryManager
		{
			get
			{
				this.CheckDisposed();
				return this.memoryManager;
			}
		}

		internal string AllocationStack { get; }

		internal string DisposeStack { get; private set; }

		public RecyclableMemoryStream(RecyclableMemoryStreamManager memoryManager)
			: this(memoryManager, Guid.NewGuid(), (string) null, 0, (byte[]) null)
		{
		}

		public RecyclableMemoryStream(RecyclableMemoryStreamManager memoryManager, Guid id)
			: this(memoryManager, id, (string) null, 0, (byte[]) null)
		{
		}

		public RecyclableMemoryStream(RecyclableMemoryStreamManager memoryManager, string tag)
			: this(memoryManager, Guid.NewGuid(), tag, 0, (byte[]) null)
		{
		}

		public RecyclableMemoryStream(RecyclableMemoryStreamManager memoryManager, Guid id, string tag)
			: this(memoryManager, id, tag, 0, (byte[]) null)
		{
		}

		public RecyclableMemoryStream(
			RecyclableMemoryStreamManager memoryManager,
			string tag,
			int requestedSize)
			: this(memoryManager, Guid.NewGuid(), tag, requestedSize, (byte[]) null)
		{
		}

		public RecyclableMemoryStream(
			RecyclableMemoryStreamManager memoryManager,
			Guid id,
			string tag,
			int requestedSize)
			: this(memoryManager, id, tag, requestedSize, (byte[]) null)
		{
		}

		internal RecyclableMemoryStream(
			RecyclableMemoryStreamManager memoryManager,
			Guid id,
			string tag,
			int requestedSize,
			byte[] initialLargeBuffer)
			: base(emptyArray)
		{
			this.memoryManager = memoryManager;
			this.id = id;
			this.tag = tag;
			//if (requestedSize < memoryManager.BlockSize)
			//	requestedSize = memoryManager.BlockSize;
			//if (initialLargeBuffer == null)
			//	this.EnsureCapacity(requestedSize);
			//else
			//	this.largeBuffer = initialLargeBuffer;
			//if (this.memoryManager.GenerateCallStacks)
			//	this.AllocationStack = Environment.StackTrace;
			//RecyclableMemoryStreamManager.Events.Writer.MemoryStreamCreated(this.id, this.tag, requestedSize);
			//this.memoryManager.ReportStreamCreated();
		}

		//~RecyclableMemoryStream()
		//{
		//	this.Dispose(false);
		//}

		protected override void Dispose(bool disposing)
		{
			if (Interlocked.CompareExchange(ref this.disposedState, 1L, 0L) != 0L)
			{
				//string disposeStack2 = (string) null;
				//if (this.memoryManager.GenerateCallStacks)
				//	disposeStack2 = Environment.StackTrace;
				//RecyclableMemoryStreamManager.Events.Writer.MemoryStreamDoubleDispose(this.id, this.tag, this.AllocationStack, this.DisposeStack, disposeStack2);
			}
			else
			{
				//RecyclableMemoryStreamManager.Events.Writer.MemoryStreamDisposed(this.id, this.tag);
				//if (this.memoryManager.GenerateCallStacks)
				//	this.DisposeStack = Environment.StackTrace;
				if (disposing)
				{
					//this.memoryManager.ReportStreamDisposed();
					//GC.SuppressFinalize((object) this);
				}
				else
				{
					//RecyclableMemoryStreamManager.Events.Writer.MemoryStreamFinalized(this.id, this.tag, this.AllocationStack);
					//if (AppDomain.CurrentDomain.IsFinalizingForUnload())
					//{
					//	base.Dispose(disposing);
					//	return;
					//}
					//this.memoryManager.ReportStreamFinalized();
				}
				//this.memoryManager.ReportStreamLength((long) this.length);
				//if (this.largeBuffer != null)
				//	this.memoryManager.ReturnLargeBuffer(this.largeBuffer, this.tag);
				//if (this.dirtyBuffers != null)
				//{
				//	foreach (byte[] dirtyBuffer in this.dirtyBuffers)
				//		this.memoryManager.ReturnLargeBuffer(dirtyBuffer, this.tag);
				//}
				this.memoryManager.ReturnBlocks((ICollection<byte[]>) this.blocks, this.tag);
				this.blocks.Clear();
				base.Dispose(disposing);
			}
		}

		public override void Close()
		{
			this.Dispose(true);
		}

		public override int Capacity
		{
			get
			{
				this.CheckDisposed();
				return this.largeBuffer != null ? this.largeBuffer.Length : (int) Math.Min((long) int.MaxValue, (long) this.blocks.Count * (long) this.memoryManager.BlockSize);
			}
			set
			{
				this.CheckDisposed();
				this.EnsureCapacity(value);
			}
		}

		public override long Length
		{
			get
			{
				this.CheckDisposed();
				return (long) this.length;
			}
		}

		public override long Position
		{
			get
			{
				this.CheckDisposed();
				return (long) this.position;
			}
			set
			{
				this.CheckDisposed();
				if (value < 0L)
					throw new ArgumentOutOfRangeException(nameof(value), "value must be non-negative");
				if (value > (long) int.MaxValue)
					throw new ArgumentOutOfRangeException(nameof(value), "value cannot be more than " + (object) (long) int.MaxValue);
				this.position = (int) value;
			}
		}

		public override bool CanRead
		{
			get
			{
				return !this.Disposed;
			}
		}

		public override bool CanSeek
		{
			get
			{
				return !this.Disposed;
			}
		}

		public override bool CanTimeout
		{
			get
			{
				return false;
			}
		}

		public override bool CanWrite
		{
			get
			{
				return !this.Disposed;
			}
		}

		public override byte[] GetBuffer()
		{
			this.CheckDisposed();
			if (this.largeBuffer != null)
				return this.largeBuffer;
			if (this.blocks.Count == 1)
				return this.blocks[0];
			byte[] largeBuffer = this.memoryManager.GetLargeBuffer(this.Capacity, this.tag);
			this.InternalRead(largeBuffer, 0, this.length, 0);
			this.largeBuffer = largeBuffer;
			if (this.blocks.Count > 0 && this.memoryManager.AggressiveBufferReturn)
			{
				this.memoryManager.ReturnBlocks((ICollection<byte[]>) this.blocks, this.tag);
				this.blocks.Clear();
			}
			return this.largeBuffer;
		}

		public override bool TryGetBuffer(out ArraySegment<byte> buffer)
		{
			this.CheckDisposed();
			buffer = new ArraySegment<byte>(this.GetBuffer(), 0, (int) this.Length);
			return true;
		}

		[Obsolete("This method has degraded performance vs. GetBuffer and should be avoided.")]
		public override byte[] ToArray()
		{
			this.CheckDisposed();
			byte[] buffer = new byte[this.Length];
			this.InternalRead(buffer, 0, this.length, 0);
			string stack = this.memoryManager.GenerateCallStacks ? Environment.StackTrace : (string) null;
			RecyclableMemoryStreamManager.Events.Writer.MemoryStreamToArray(this.id, this.tag, stack, 0);
			this.memoryManager.ReportStreamToArray();
			return buffer;
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			return this.SafeRead(buffer, offset, count, ref this.position);
		}

		public int SafeRead(byte[] buffer, int offset, int count, ref int streamPosition)
		{
			this.CheckDisposed();
			if (buffer == null)
				throw new ArgumentNullException(nameof(buffer));
			if (offset < 0)
				throw new ArgumentOutOfRangeException(nameof(offset), "offset cannot be negative");
			if (count < 0)
				throw new ArgumentOutOfRangeException(nameof(count), "count cannot be negative");
			if (offset + count > buffer.Length)
				throw new ArgumentException("buffer length must be at least offset + count");
			int num = this.InternalRead(buffer, offset, count, streamPosition);
			streamPosition += num;
			return num;
		}

		public override int Read(Span<byte> buffer)
		{
			return this.SafeRead(buffer, ref this.position);
		}

		public int SafeRead(Span<byte> buffer, ref int streamPosition)
		{
			this.CheckDisposed();
			int num = this.InternalRead(buffer, streamPosition);
			streamPosition += num;
			return num;
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			this.CheckDisposed();
			if (buffer == null)
				throw new ArgumentNullException(nameof(buffer));
			if (offset < 0)
				throw new ArgumentOutOfRangeException(nameof(offset), (object) offset, "Offset must be in the range of 0 - buffer.Length-1");
			if (count < 0)
				throw new ArgumentOutOfRangeException(nameof(count), (object) count, "count must be non-negative");
			if (count + offset > buffer.Length)
				throw new ArgumentException("count must be greater than buffer.Length - offset");
			int blockSize = this.memoryManager.BlockSize;
			long num1 = (long) this.position + (long) count;
			if (num1 > (long) int.MaxValue)
				throw new IOException("Maximum capacity exceeded");
			this.EnsureCapacity((int) num1);
			if (this.largeBuffer == null)
			{
				int val2 = count;
				int num2 = 0;
				BlockAndOffset andRelativeOffset = this.GetBlockAndRelativeOffset(this.position);
				while (val2 > 0)
				{
					byte[] block = this.blocks[andRelativeOffset.Block];
					int count1 = Math.Min(blockSize - andRelativeOffset.Offset, val2);
					Buffer.BlockCopy((Array) buffer, offset + num2, (Array) block, andRelativeOffset.Offset, count1);
					val2 -= count1;
					num2 += count1;
					++andRelativeOffset.Block;
					andRelativeOffset.Offset = 0;
				}
			}
			else
				Buffer.BlockCopy((Array) buffer, offset, (Array) this.largeBuffer, this.position, count);
			this.position = (int) num1;
			this.length = Math.Max(this.position, this.length);
		}

		public override void Write(ReadOnlySpan<byte> source)
		{
			this.CheckDisposed();
			int blockSize = this.memoryManager.BlockSize;
			long num1 = (long) this.position + (long) source.Length;
			if (num1 > (long) int.MaxValue)
				throw new IOException("Maximum capacity exceeded");
			this.EnsureCapacity((int) num1);
			if (this.largeBuffer == null)
			{
				BlockAndOffset andRelativeOffset = this.GetBlockAndRelativeOffset(this.position);
				while (source.Length > 0)
				{
					byte[] block = this.blocks[andRelativeOffset.Block];
					int num2 = Math.Min(blockSize - andRelativeOffset.Offset, source.Length);
					source.Slice(0, num2).CopyTo(block.AsSpan<byte>(andRelativeOffset.Offset));
					source = source.Slice(num2);
					++andRelativeOffset.Block;
					andRelativeOffset.Offset = 0;
				}
			}
			else
				source.CopyTo(this.largeBuffer.AsSpan<byte>(this.position));
			this.position = (int) num1;
			this.length = Math.Max(this.position, this.length);
		}

		public override string ToString()
		{
			return string.Format("Id = {0}, Tag = {1}, Length = {2:N0} bytes", (object) this.Id, (object) this.Tag, (object) this.Length);
		}

		public override void WriteByte(byte value)
		{
			this.CheckDisposed();
			this.byteBuffer[0] = value;
			this.Write(this.byteBuffer, 0, 1);
		}

		public override int ReadByte()
		{
			return this.SafeReadByte(ref this.position);
		}

		public int SafeReadByte(ref int streamPosition)
		{
			this.CheckDisposed();
			if (streamPosition == this.length)
				return -1;
			byte num;
			if (this.largeBuffer == null)
			{
				BlockAndOffset andRelativeOffset = this.GetBlockAndRelativeOffset(streamPosition);
				num = this.blocks[andRelativeOffset.Block][andRelativeOffset.Offset];
			}
			else
				num = this.largeBuffer[streamPosition];
			++streamPosition;
			return (int) num;
		}

		public override void SetLength(long value)
		{
			this.CheckDisposed();
			if (value < 0L || value > (long) int.MaxValue)
				throw new ArgumentOutOfRangeException(nameof(value), "value must be non-negative and at most " + (object) (long) int.MaxValue);
			this.EnsureCapacity((int) value);
			this.length = (int) value;
			if ((long) this.position <= value)
				return;
			this.position = (int) value;
		}

		public override long Seek(long offset, SeekOrigin loc)
		{
			this.CheckDisposed();
			if (offset > (long) int.MaxValue)
				throw new ArgumentOutOfRangeException(nameof(offset), "offset cannot be larger than " + (object) (long) int.MaxValue);
			int num;
			switch (loc)
			{
				case SeekOrigin.Begin:
					num = (int) offset;
					break;
				case SeekOrigin.Current:
					num = (int) offset + this.position;
					break;
				case SeekOrigin.End:
					num = (int) offset + this.length;
					break;
				default:
					throw new ArgumentException("Invalid seek origin", nameof(loc));
			}
			if (num < 0)
				throw new IOException("Seek before beginning");
			this.position = num;
			return (long) this.position;
		}

		public override void WriteTo(Stream stream)
		{
			this.CheckDisposed();
			if (stream == null)
				throw new ArgumentNullException(nameof(stream));
			if (this.largeBuffer == null)
			{
				int index = 0;
				int length = this.length;
				while (length > 0)
				{
					int count = Math.Min(this.blocks[index].Length, length);
					stream.Write(this.blocks[index], 0, count);
					length -= count;
					++index;
				}
			}
			else
				stream.Write(this.largeBuffer, 0, this.length);
		}

		private bool Disposed
		{
			get
			{
				return (ulong) Interlocked.Read(ref this.disposedState) > 0UL;
			}
		}

		private void CheckDisposed()
		{
			if (this.Disposed)
				throw new ObjectDisposedException(string.Format("The stream with Id {0} and Tag {1} is disposed.", (object) this.id, (object) this.tag));
		}

		private int InternalRead(byte[] buffer, int offset, int count, int fromPosition)
		{
			if (this.length - fromPosition <= 0)
				return 0;
			if (this.largeBuffer == null)
			{
				BlockAndOffset andRelativeOffset = this.GetBlockAndRelativeOffset(fromPosition);
				int num = 0;
				int val2 = Math.Min(count, this.length - fromPosition);
				while (val2 > 0)
				{
					int count1 = Math.Min(this.blocks[andRelativeOffset.Block].Length - andRelativeOffset.Offset, val2);
					Buffer.BlockCopy((Array) this.blocks[andRelativeOffset.Block], andRelativeOffset.Offset, (Array) buffer, num + offset, count1);
					num += count1;
					val2 -= count1;
					++andRelativeOffset.Block;
					andRelativeOffset.Offset = 0;
				}
				return num;
			}
			int count2 = Math.Min(count, this.length - fromPosition);
			Buffer.BlockCopy((Array) this.largeBuffer, fromPosition, (Array) buffer, offset, count2);
			return count2;
		}

		private int InternalRead(Span<byte> buffer, int fromPosition)
		{
			if (this.length - fromPosition <= 0)
				return 0;
			if (this.largeBuffer == null)
			{
				BlockAndOffset andRelativeOffset = this.GetBlockAndRelativeOffset(fromPosition);
				int start = 0;
				int val2 = Math.Min(buffer.Length, this.length - fromPosition);
				while (val2 > 0)
				{
					int length = Math.Min(this.blocks[andRelativeOffset.Block].Length - andRelativeOffset.Offset, val2);
					this.blocks[andRelativeOffset.Block].AsSpan<byte>(andRelativeOffset.Offset, length).CopyTo(buffer.Slice(start));
					start += length;
					val2 -= length;
					++andRelativeOffset.Block;
					andRelativeOffset.Offset = 0;
				}
				return start;
			}
			int length1 = Math.Min(buffer.Length, this.length - fromPosition);
			this.largeBuffer.AsSpan<byte>(fromPosition, length1).CopyTo(buffer);
			return length1;
		}

		private BlockAndOffset GetBlockAndRelativeOffset(
			int offset)
		{
			int blockSize = this.memoryManager.BlockSize;
			return new BlockAndOffset(offset / blockSize, offset % blockSize);
		}

		private void EnsureCapacity(int newCapacity)
		{
			if ((long) newCapacity > this.memoryManager.MaximumStreamCapacity && this.memoryManager.MaximumStreamCapacity > 0L)
			{
				RecyclableMemoryStreamManager.Events.Writer.MemoryStreamOverCapacity(newCapacity, this.memoryManager.MaximumStreamCapacity, this.tag, this.AllocationStack);
				throw new InvalidOperationException("Requested capacity is too large: " + (object) newCapacity + ". Limit is " + (object) this.memoryManager.MaximumStreamCapacity);
			}
			if (this.largeBuffer != null)
			{
				if (newCapacity <= this.largeBuffer.Length)
					return;
				byte[] largeBuffer = this.memoryManager.GetLargeBuffer(newCapacity, this.tag);
				this.InternalRead(largeBuffer, 0, this.length, 0);
				this.ReleaseLargeBuffer();
				this.largeBuffer = largeBuffer;
			}
			else
			{
				while (this.Capacity < newCapacity)
					this.blocks.Add(this.memoryManager.GetBlock());
			}
		}

		private void ReleaseLargeBuffer()
		{
			if (this.memoryManager.AggressiveBufferReturn)
			{
				this.memoryManager.ReturnLargeBuffer(this.largeBuffer, this.tag);
			}
			else
			{
				if (this.dirtyBuffers == null)
					this.dirtyBuffers = new List<byte[]>(1);
				this.dirtyBuffers.Add(this.largeBuffer);
			}
			this.largeBuffer = (byte[]) null;
		}

		private struct BlockAndOffset
		{
			public int Block;
			public int Offset;

			public BlockAndOffset(int block, int offset)
			{
				this.Block = block;
				this.Offset = offset;
			}
		}
	}
}