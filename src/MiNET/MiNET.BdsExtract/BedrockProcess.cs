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

using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MiNET.BdsExtract;

/// <summary>
///     A read-only window onto a running Bedrock Dedicated Server.
///     Nothing is injected and nothing is written. The process is opened without write rights at
///     all, so the operating system refuses a write even if this code asked for one. That matters
///     because the server being read is usually one someone is using.
/// </summary>
public sealed partial class BedrockProcess : IDisposable
{
	private const uint ProcessQueryInformation = 0x0400;
	private const uint ProcessVmRead = 0x0010;
	private const uint MemCommit = 0x1000;
	private const uint PageGuard = 0x100;
	private const uint ReadableProtections = 0x66; // R, RW, RX, RWX and their write-copy variants

	private readonly IntPtr _handle;
	private readonly List<Region> _regions;

	/// <summary>A committed, readable stretch of the target's address space.</summary>
	public readonly record struct Region(ulong Base, ulong Size)
	{
		public ulong End => Base + Size;
	}

	public int Id { get; }
	public string ExecutablePath { get; }
	public IReadOnlyList<Region> Regions => _regions;

	private BedrockProcess(Process process)
	{
		Id = process.Id;
		ExecutablePath = process.MainModule?.FileName ?? "";
		_handle = OpenProcess(ProcessQueryInformation | ProcessVmRead, false, process.Id);
		if (_handle == IntPtr.Zero)
		{
			throw new InvalidOperationException($"cannot open pid {process.Id}: error {Marshal.GetLastWin32Error()}");
		}
		_regions = ReadRegions();
	}

	/// <summary>
	///     Attaches to a running server. When several are running, <paramref name="pathFilter" />
	///     picks between them by executable path, so a machine hosting more than one server does
	///     not silently get read from the wrong one.
	/// </summary>
	public static BedrockProcess Attach(string pathFilter = null)
	{
		var candidates = new List<Process>();
		foreach (var process in Process.GetProcessesByName("bedrock_server"))
		{
			try
			{
				string path = process.MainModule?.FileName ?? "";
				if (pathFilter is null || path.Replace('/', '\\').Contains(pathFilter, StringComparison.OrdinalIgnoreCase))
				{
					candidates.Add(process);
				}
			}
			catch (Exception)
			{
				// A process we cannot inspect is a process we cannot read; skip it.
			}
		}

		if (candidates.Count == 0)
		{
			throw new InvalidOperationException(pathFilter is null
				? "no bedrock_server.exe is running"
				: $"no bedrock_server.exe running from a path containing \"{pathFilter}\"");
		}
		if (candidates.Count > 1)
		{
			string paths = string.Join(", ", candidates.Select(c => $"{c.Id}"));
			throw new InvalidOperationException(
				$"{candidates.Count} servers are running (pids {paths}); pass a path filter to choose one");
		}
		return new BedrockProcess(candidates[0]);
	}

	public bool IsMapped(ulong address)
	{
		// Regions are ordered, so a binary search settles this without walking them all.
		int low = 0, high = _regions.Count - 1;
		while (low <= high)
		{
			int mid = (low + high) / 2;
			var region = _regions[mid];
			if (address < region.Base) high = mid - 1;
			else if (address >= region.End) low = mid + 1;
			else return true;
		}
		return false;
	}

	public bool TryRead(ulong address, byte[] buffer, int length)
	{
		return ReadProcessMemory(_handle, (IntPtr) address, buffer, (nuint) length, out nuint read) && (int) read == length;
	}

	public ulong ReadUInt64(ulong address, byte[] scratch)
	{
		return TryRead(address, scratch, 8) ? BitConverter.ToUInt64(scratch, 0) : 0;
	}

	private List<Region> ReadRegions()
	{
		var regions = new List<Region>();
		ulong address = 0;
		int size = Marshal.SizeOf<MemoryBasicInformation>();
		while (address < 0x7FFFFFFF0000)
		{
			if (VirtualQueryEx(_handle, (IntPtr) address, out var info, (nuint) size) == 0) break;
			ulong length = (ulong) info.RegionSize;
			if (length == 0) break;

			bool readable = (info.Protect & ReadableProtections) != 0 && (info.Protect & PageGuard) == 0;
			if (info.State == MemCommit && readable) regions.Add(new Region(address, length));
			address += length;
		}
		return regions;
	}

	public void Dispose()
	{
		if (_handle != IntPtr.Zero) CloseHandle(_handle);
	}

	[LibraryImport("kernel32.dll", SetLastError = true)]
	private static partial IntPtr OpenProcess(uint access, [MarshalAs(UnmanagedType.Bool)] bool inherit, int pid);

	[LibraryImport("kernel32.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static partial bool CloseHandle(IntPtr handle);

	[LibraryImport("kernel32.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static partial bool ReadProcessMemory(IntPtr process, IntPtr address, byte[] buffer, nuint size, out nuint read);

	[LibraryImport("kernel32.dll", SetLastError = true)]
	private static partial nuint VirtualQueryEx(IntPtr process, IntPtr address, out MemoryBasicInformation info, nuint length);

	[StructLayout(LayoutKind.Sequential)]
	private struct MemoryBasicInformation
	{
		public IntPtr BaseAddress;
		public IntPtr AllocationBase;
		public uint AllocationProtect;
		public uint Alignment1;
		public IntPtr RegionSize;
		public uint State;
		public uint Protect;
		public uint Type;
		public uint Alignment2;
	}
}
