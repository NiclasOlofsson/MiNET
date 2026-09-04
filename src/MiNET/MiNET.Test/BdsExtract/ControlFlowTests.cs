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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2026 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiNET.BdsExtract.Binary;

namespace MiNET.Test.BdsExtract
{
	[TestClass]
	public class ControlFlowTests
	{
		private const ulong ImageBase = 0x140000000;
		private const uint TextRva = 0x1000;
		private const uint RdataRva = 0x2000;

		/// <summary>.text at RVA 0x1000 and .rdata at RVA 0x2000, each backed by its own raw range of one buffer.</summary>
		private static PeImage Fake(byte[] text, PeFunction[] functions, byte[] rdata = null, IReadOnlyDictionary<uint, string> imports = null, uint guardDispatchSlot = 0)
		{
			rdata ??= new byte[0];
			var data = new byte[0x200 + 0x1000 + rdata.Length];
			text.CopyTo(data, 0x200);
			rdata.CopyTo(data, 0x200 + 0x1000);
			var sections = new[]
			{
				new PeSection(".text", TextRva, (uint) text.Length, 0x200, text.Length),
				new PeSection(".rdata", RdataRva, (uint) rdata.Length, 0x200 + 0x1000, rdata.Length),
			};
			return PeImage.FromSections(ImageBase, sections, functions, data, imports: imports, guardDispatchSlot: guardDispatchSlot);
		}

		[TestMethod]
		public void A_jump_through_the_cfg_dispatcher_is_an_indirect_tail_call_not_an_unresolved_jump()
		{
			byte[] text =
			{
				// A [0x1000, 0x1010): the guarded tail call shape, dispatcher loaded into a register first
				0x48, 0x8B, 0x15, 0xF9, 0x0F, 0x00, 0x00, // 1000 mov rdx,[rip+0xFF9] = [0x2000], the dispatch slot
				0x48, 0x8B, 0xC8, // 1007 mov rcx,rax
				0xFF, 0xE2, // 100A jmp rdx
				0xCC, 0xCC, 0xCC, 0xCC,
				// B [0x1010, 0x1020): the same through memory
				0xFF, 0x25, 0xEA, 0x0F, 0x00, 0x00, // 1010 jmp [rip+0xFEA] = [0x2000]
				0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC,
				// D, the dispatcher the slot points at: __guard_dispatch_icall_nop is literally jmp rax
				0xFF, 0xE0, // 1020 jmp rax
			};
			var rdata = new byte[0x10];
			BitConverter.GetBytes(ImageBase + 0x1020).CopyTo(rdata, 0);
			var flow = new ControlFlow(Fake(text, new[] { new PeFunction(0x1000, 0x1010), new PeFunction(0x1010, 0x1020) }, rdata, guardDispatchSlot: 0x2000));

			Assert.AreEqual(0, flow.UnresolvedIndirectJumpsOf(0x1000).Count);
			Assert.AreEqual(1, flow.IndirectTailCallsOf(0x1000));
			Assert.AreEqual(0, flow.UnresolvedIndirectJumpsOf(0x1010).Count);
			Assert.AreEqual(1, flow.IndirectTailCallsOf(0x1010));
			Assert.IsFalse(flow.IsNoReturn(0x1000), "an indirect tail call may return");
			// The dispatcher's own jmp rax is loaded from nowhere and stays what it is: unresolved.
			DiscoveredFunction dispatcher = flow.Discovered.Single();
			Assert.AreEqual(0x1020u, dispatcher.Rva);
			Assert.AreEqual(FunctionOrigin.Pointer, dispatcher.Origin);
			CollectionAssert.AreEqual(new[] { 0x1020u }, flow.UnresolvedIndirectJumpsOf(0x1020).ToArray());
		}

		[TestMethod]
		public void A_pdata_range_that_ends_right_after_a_call_proves_the_callee_never_returns()
		{
			byte[] text =
			{
				// C [0x1000, 0x1009): the compiler emitted nothing after the call, so Z cannot return
				0x48, 0x83, 0xEC, 0x28, // 1000 sub rsp,28h
				0xE8, 0x07, 0x00, 0x00, 0x00, // 1004 call 1010 (Z)
				0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC,
				// Z [0x1010, 0x1020): ends in a guarded tail call, so nothing about Z itself says whether it returns
				0xFF, 0x25, 0xEA, 0x0F, 0x00, 0x00, // 1010 jmp [0x2000]
				0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC,
				// W [0x1020, 0x1030): calls C, then dead bytes
				0xE8, 0xDB, 0xFF, 0xFF, 0xFF, // 1020 call 1000 (C)
				0x48, 0x8B, 0x41, 0x68, 0xC3, // 1025 dead
				0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC,
				0xFF, 0xE0, // 1030 D: jmp rax
			};
			var rdata = new byte[0x10];
			BitConverter.GetBytes(ImageBase + 0x1030).CopyTo(rdata, 0);
			var flow = new ControlFlow(Fake(text, new[] { new PeFunction(0x1000, 0x1009), new PeFunction(0x1010, 0x1020), new PeFunction(0x1020, 0x1030) }, rdata, guardDispatchSlot: 0x2000));

			Assert.IsNull(flow.Functions.Single(f => f.Rva == 0x1000).Unverified, "falling off the range end is the compiler's statement, not a fault");
			Assert.IsTrue(flow.IsNoReturn(0x1010), "Z, proven by C's range end");
			Assert.IsTrue(flow.IsNoReturn(0x1000), "C");
			Assert.IsTrue(flow.IsNoReturn(0x1020), "W");
			Assert.IsFalse(flow.IsCode(0x1025));
		}

		[TestMethod]
		public void Calls_and_tail_jumps_out_of_pdata_discover_functions_while_branches_inside_make_blocks()
		{
			byte[] text =
			{
				// A, the one .pdata function, [0x1000, 0x1010)
				0x85, 0xC9, // 1000 test ecx,ecx
				0x74, 0x05, // 1002 je 1009
				0xE8, 0x0B, 0x00, 0x00, 0x00, // 1004 call 1014 (B)
				0xEB, 0x0F, // 1009 jmp 101A (C): a tail call, it leaves A's range
				0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, // 100B padding
				// B, a leaf getter no .pdata entry covers
				0x48, 0x8B, 0x41, 0x68, // 1014 mov rax,[rcx+0x68]
				0xC3, // 1018 ret
				0xCC, // 1019
				// C
				0x31, 0xC0, // 101A xor eax,eax
				0xC3, // 101C ret
			};
			var flow = new ControlFlow(Fake(text, new[] { new PeFunction(0x1000, 0x1010) }));

			CollectionAssert.AreEqual(new[] { 0x1014u, 0x101Au }, flow.Discovered.Select(f => f.Rva).ToArray());
			DiscoveredFunction b = flow.Discovered[0];
			Assert.AreEqual(0x1019u, b.End);
			Assert.AreEqual(FunctionOrigin.Call, b.Origin);
			Assert.AreEqual(0x1004u, b.Witness);
			Assert.IsNull(b.Unverified);
			DiscoveredFunction c = flow.Discovered[1];
			Assert.AreEqual(0x101Du, c.End);
			Assert.AreEqual(FunctionOrigin.TailCall, c.Origin);
			Assert.AreEqual(0x1009u, c.Witness);

			CollectionAssert.AreEqual(new[] { 0x1000u, 0x1004u, 0x1009u }, flow.BlocksOf(0x1000).ToArray());
			CollectionAssert.AreEqual(new[] { 0x1014u, 0x101Au }, flow.CalleesOf(0x1000).ToArray());
			CollectionAssert.AreEqual(new[] { 0x1000u }, flow.CallersOf(0x1014).ToArray());
			Assert.IsTrue(flow.IsCode(0x1004));
			Assert.IsFalse(flow.IsCode(0x100B), "padding is not code");
			Assert.AreEqual(3, flow.Functions.Count);
			Assert.AreEqual(FunctionOrigin.Pdata, flow.Functions[0].Origin);
		}

		[TestMethod]
		public void A_no_return_import_stops_flow_and_propagates_up_through_every_caller()
		{
			byte[] text =
			{
				// A [0x1000, 0x1010): calls the _exit thunk, then bytes that flow must never reach
				0xE8, 0x1B, 0x00, 0x00, 0x00, // 1000 call 1020 (T)
				0x48, 0x8B, 0x41, 0x68, 0xC3, // 1005 mov rax,[rcx+0x68]; ret: dead, only reached if the call is thought to return
				0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC,
				// A2 [0x1010, 0x1020): calls A, so it is no-return one level removed
				0xE8, 0xEB, 0xFF, 0xFF, 0xFF, // 1010 call 1000 (A)
				0x48, 0x8B, 0x41, 0x68, 0xC3, // 1015 dead the same way
				0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC,
				// T, an import thunk no .pdata entry covers
				0xFF, 0x25, 0xDA, 0x0F, 0x00, 0x00, // 1020 jmp [rip+0xFDA] = [0x2000], the IAT slot of _exit
				0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC,
				// R [0x1030, 0x1036): calls a returning function and returns; the control
				0xE8, 0x0B, 0x00, 0x00, 0x00, // 1030 call 1040 (C)
				0xC3, // 1035 ret
				0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC,
				// C
				0x31, 0xC0, 0xC3, // 1040 xor eax,eax; ret
			};
			var imports = new Dictionary<uint, string> { [0x2000] = "api-ms-win-crt-runtime-l1-1-0.dll!_exit" };
			var flow = new ControlFlow(Fake(text, new[] { new PeFunction(0x1000, 0x1010), new PeFunction(0x1010, 0x1020), new PeFunction(0x1030, 0x1036) }, new byte[8], imports));

			Assert.IsTrue(flow.IsNoReturn(0x1020), "the thunk");
			Assert.IsTrue(flow.IsNoReturn(0x1000), "the direct caller");
			Assert.IsTrue(flow.IsNoReturn(0x1010), "the caller's caller");
			Assert.IsFalse(flow.IsNoReturn(0x1030), "the control returns");
			Assert.IsFalse(flow.IsCode(0x1005), "bytes after a no-return call are not flow");
			Assert.IsFalse(flow.IsCode(0x1015));
			Assert.IsTrue(flow.IsCode(0x1035));
			CollectionAssert.AreEqual(new[] { 0x1010u }, flow.BlocksOf(0x1010).ToArray());
			DiscoveredFunction thunk = flow.Discovered.Single(f => f.Rva == 0x1020);
			Assert.AreEqual(0x1026u, thunk.End);
			Assert.IsNull(thunk.Unverified);
		}

		private static byte[] Table(params uint[] rvas)
		{
			var bytes = new byte[0x20];
			for (int i = 0; i < rvas.Length; i++) System.Buffers.Binary.BinaryPrimitives.WriteUInt32LittleEndian(bytes.AsSpan(4 * i), rvas[i]);
			return bytes;
		}

		[TestMethod]
		public void A_switch_bounded_by_its_cmp_reads_exactly_that_many_cases()
		{
			byte[] text =
			{
				0x48, 0x8D, 0x15, 0xF9, 0xEF, 0xFF, 0xFF, // 1000 lea rdx,[rip-0x1007] = __ImageBase
				0x83, 0xF9, 0x02, // 1007 cmp ecx,2
				0x77, 0x12, // 100A ja 101E (default)
				0x8B, 0xC1, // 100C mov eax,ecx
				0x8B, 0x84, 0x82, 0x00, 0x20, 0x00, 0x00, // 100E mov eax,[rdx+rax*4+0x2000]
				0x48, 0x03, 0xC2, // 1015 add rax,rdx
				0xFF, 0xE0, // 1018 jmp rax
				0xCC, 0xCC, 0xCC, 0xCC, // 101A
				0x31, 0xC0, 0xC3, // 101E default: xor eax,eax; ret
				0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3, // 1021 case 0
				0xB8, 0x02, 0x00, 0x00, 0x00, 0xC3, // 1027 case 1
				0xB8, 0x03, 0x00, 0x00, 0x00, 0xC3, // 102D case 2
				0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, // 1033
			};
			// A fourth entry sits in the table and lies inside the function; the cmp says three, so it must not be read.
			byte[] rdata = Table(0x1021, 0x1027, 0x102D, 0x101E);
			var flow = new ControlFlow(Fake(text, new[] { new PeFunction(0x1000, 0x1040) }, rdata));

			CollectionAssert.AreEqual(new[] { 0x1000u, 0x100Cu, 0x101Eu, 0x1021u, 0x1027u, 0x102Du }, flow.BlocksOf(0x1000).ToArray());
			Assert.AreEqual(0, flow.UnresolvedIndirectJumpsOf(0x1000).Count);
			CollectionAssert.AreEqual(new[] { new JumpTable(0x1018, 0x2000, 3, JumpTableBound.Compare) }, flow.JumpTablesOf(0x1000).ToArray());
			Assert.IsTrue(flow.IsCode(0x102D));
		}

		[TestMethod]
		public void A_switch_without_a_cmp_reads_until_an_entry_leaves_the_function()
		{
			byte[] text =
			{
				0x48, 0x8D, 0x15, 0xF9, 0xEF, 0xFF, 0xFF, // 1000 lea rdx,[rip-0x1007]
				0x8B, 0xC1, // 1007 mov eax,ecx
				0x8B, 0x84, 0x82, 0x00, 0x20, 0x00, 0x00, // 1009 mov eax,[rdx+rax*4+0x2000]
				0x48, 0x03, 0xC2, // 1010 add rax,rdx
				0xFF, 0xE0, // 1013 jmp rax
				0xCC, 0xCC, 0xCC, // 1015
				0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3, // 1018 case 0
				0xB8, 0x02, 0x00, 0x00, 0x00, 0xC3, // 101E case 1
				0xB8, 0x03, 0x00, 0x00, 0x00, 0xC3, // 1024 case 2
				0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, // 102A
			};
			byte[] rdata = Table(0x1018, 0x101E, 0x1024, 0x5000);
			var flow = new ControlFlow(Fake(text, new[] { new PeFunction(0x1000, 0x1030) }, rdata));

			CollectionAssert.AreEqual(new[] { 0x1000u, 0x1018u, 0x101Eu, 0x1024u }, flow.BlocksOf(0x1000).ToArray());
			CollectionAssert.AreEqual(new[] { new JumpTable(0x1013, 0x2000, 3, JumpTableBound.Range) }, flow.JumpTablesOf(0x1000).ToArray());
		}

		[TestMethod]
		public void A_switch_whose_entries_are_relative_to_the_table_itself_resolves_through_the_lea()
		{
			byte[] text =
			{
				0x48, 0x8D, 0x05, 0xF9, 0x0F, 0x00, 0x00, // 1000 lea rax,[rip+0xFF9] = the table at 0x2000
				0x48, 0x83, 0xFB, 0x02, // 1007 cmp rbx,2
				0x77, 0x0B, // 100B ja 1018 (default)
				0x48, 0x63, 0x0C, 0x98, // 100D movsxd rcx,[rax+rbx*4]
				0x48, 0x03, 0xC8, // 1011 add rcx,rax
				0xFF, 0xE1, // 1014 jmp rcx
				0xCC, 0xCC, // 1016
				0x31, 0xC0, 0xC3, // 1018 default
				0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3, // 101B case 0
				0xB8, 0x02, 0x00, 0x00, 0x00, 0xC3, // 1021 case 1
				0xB8, 0x03, 0x00, 0x00, 0x00, 0xC3, // 1027 case 2
				0xCC, 0xCC, 0xCC, // 102D
			};
			byte[] rdata = Table(unchecked((uint) (0x101B - 0x2000)), unchecked((uint) (0x1021 - 0x2000)), unchecked((uint) (0x1027 - 0x2000)));
			var flow = new ControlFlow(Fake(text, new[] { new PeFunction(0x1000, 0x1030) }, rdata));

			CollectionAssert.AreEqual(new[] { 0x1000u, 0x100Du, 0x1018u, 0x101Bu, 0x1021u, 0x1027u }, flow.BlocksOf(0x1000).ToArray());
			CollectionAssert.AreEqual(new[] { new JumpTable(0x1014, 0x2000, 3, JumpTableBound.Compare) }, flow.JumpTablesOf(0x1000).ToArray());
			Assert.AreEqual(0, flow.UnresolvedIndirectJumpsOf(0x1000).Count);
		}

		[TestMethod]
		public void A_two_level_switch_indexes_the_rva_table_through_a_byte_table()
		{
			byte[] text =
			{
				0x48, 0x8D, 0x15, 0xF9, 0xEF, 0xFF, 0xFF, // 1000 lea rdx,[rip-0x1007]
				0x83, 0xF9, 0x05, // 1007 cmp ecx,5: bounds the byte table, not the rva table
				0x77, 0x18, // 100A ja 1024
				0x8B, 0xC1, // 100C mov eax,ecx
				0x0F, 0xB6, 0x84, 0x02, 0x10, 0x20, 0x00, 0x00, // 100E movzx eax,byte ptr [rdx+rax+0x2010]
				0x8B, 0x84, 0x82, 0x00, 0x20, 0x00, 0x00, // 1016 mov eax,[rdx+rax*4+0x2000]
				0x48, 0x03, 0xC2, // 101D add rax,rdx
				0xFF, 0xE0, // 1020 jmp rax
				0xCC, 0xCC, // 1022
				0x31, 0xC0, 0xC3, // 1024 default
				0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3, // 1027 case 0
				0xB8, 0x02, 0x00, 0x00, 0x00, 0xC3, // 102D case 1
				0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, // 1033
			};
			byte[] rdata = Table(0x1027, 0x102D, 0x5000);
			new byte[] { 0, 1, 1, 0, 1, 0 }.CopyTo(rdata, 0x10);
			var flow = new ControlFlow(Fake(text, new[] { new PeFunction(0x1000, 0x1040) }, rdata));

			CollectionAssert.AreEqual(new[] { 0x1000u, 0x100Cu, 0x1024u, 0x1027u, 0x102Du }, flow.BlocksOf(0x1000).ToArray());
			// The cmp bounds the byte table at 6 entries; their largest value plus one is the rva table's exact size.
			CollectionAssert.AreEqual(new[] { new JumpTable(0x1020, 0x2000, 2, JumpTableBound.Compare) }, flow.JumpTablesOf(0x1000).ToArray());
		}

		private static byte[] Nops(int count) => Enumerable.Repeat((byte) 0x90, count).ToArray();

		/// <summary>The table-relative switch with its bound check swapped in: lea; check; ja/jae default; movsxd; add; jmp; default; three cases.</summary>
		private static byte[] SwitchWithCheck(byte[] check, byte branch)
		{
			return new byte[] { 0x48, 0x8D, 0x05, 0xF9, 0x0F, 0x00, 0x00 } // 1000 lea rax,[0x2000]
				.Concat(check) // 1007, four bytes
				.Concat(new byte[]
				{
					branch, 0x0B, // 100B ja/jae 1018 (default)
					0x48, 0x63, 0x0C, 0x98, // 100D movsxd rcx,[rax+rbx*4]
					0x48, 0x03, 0xC8, // 1011 add rcx,rax
					0xFF, 0xE1, // 1014 jmp rcx
					0xCC, 0xCC, // 1016
					0x31, 0xC0, 0xC3, // 1018 default
					0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3, // 101B case 0
					0xB8, 0x02, 0x00, 0x00, 0x00, 0xC3, // 1021 case 1
					0xB8, 0x03, 0x00, 0x00, 0x00, 0xC3, // 1027 case 2
					0xCC, 0xCC, 0xCC, // 102D
				}).ToArray();
		}

		[TestMethod]
		public void A_bound_compared_in_a_16_bit_register_still_counts()
		{
			// cmp bx,2: the index is rbx in the load, bx in the check.
			byte[] text = SwitchWithCheck(new byte[] { 0x66, 0x83, 0xFB, 0x02 }, 0x77);
			// A fourth entry lands inside the function; only the cmp says it is not a case.
			byte[] rdata = Table(unchecked((uint) (0x101B - 0x2000)), unchecked((uint) (0x1021 - 0x2000)), unchecked((uint) (0x1027 - 0x2000)), unchecked((uint) (0x1018 - 0x2000)));
			var flow = new ControlFlow(Fake(text, new[] { new PeFunction(0x1000, 0x1030) }, rdata));

			CollectionAssert.AreEqual(new[] { new JumpTable(0x1014, 0x2000, 3, JumpTableBound.Compare) }, flow.JumpTablesOf(0x1000).ToArray());
		}

		[TestMethod]
		public void A_bound_that_defaults_on_jae_has_one_case_fewer_than_one_that_defaults_on_ja()
		{
			// cmp rbx,3; jae default: indexes 0..2 dispatch, so three cases, not four.
			byte[] text = SwitchWithCheck(new byte[] { 0x48, 0x83, 0xFB, 0x03 }, 0x73);
			byte[] rdata = Table(unchecked((uint) (0x101B - 0x2000)), unchecked((uint) (0x1021 - 0x2000)), unchecked((uint) (0x1027 - 0x2000)), unchecked((uint) (0x1018 - 0x2000)));
			var flow = new ControlFlow(Fake(text, new[] { new PeFunction(0x1000, 0x1030) }, rdata));

			CollectionAssert.AreEqual(new[] { new JumpTable(0x1014, 0x2000, 3, JumpTableBound.Compare) }, flow.JumpTablesOf(0x1000).ToArray());
		}

		[TestMethod]
		public void A_bound_checked_on_a_register_the_index_was_copied_from_in_the_block_before_still_counts()
		{
			byte[] text =
			{
				0x48, 0x8D, 0x05, 0xF9, 0x0F, 0x00, 0x00, // 1000 lea rax,[0x2000]
				0x8B, 0xCA, // 1007 mov ecx,edx: the index is a copy of edx from here on
				0xEB, 0x10, // 1009 jmp 101B
				0x31, 0xC0, 0xC3, // 100B unreachable filler
				0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, // 100E
				0x83, 0xFA, 0x02, // 101B cmp edx,2: the check is on the original
				0x77, 0x0B, // 101E ja 102B (default)
				0x48, 0x63, 0x0C, 0x88, // 1020 movsxd rcx,[rax+rcx*4]: the load indexes by the copy
				0x48, 0x03, 0xC8, // 1024 add rcx,rax
				0xFF, 0xE1, // 1027 jmp rcx
				0xCC, 0xCC, // 1029
				0x31, 0xC0, 0xC3, // 102B default
				0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3, // 102E case 0
				0xB8, 0x02, 0x00, 0x00, 0x00, 0xC3, // 1034 case 1
				0xB8, 0x03, 0x00, 0x00, 0x00, 0xC3, // 103A case 2
				0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, // 1040
			};
			byte[] rdata = Table(unchecked((uint) (0x102E - 0x2000)), unchecked((uint) (0x1034 - 0x2000)), unchecked((uint) (0x103A - 0x2000)), unchecked((uint) (0x102B - 0x2000)));
			var flow = new ControlFlow(Fake(text, new[] { new PeFunction(0x1000, 0x1050) }, rdata));

			CollectionAssert.AreEqual(new[] { new JumpTable(0x1027, 0x2000, 3, JumpTableBound.Compare) }, flow.JumpTablesOf(0x1000).ToArray());
		}

		[TestMethod]
		public void A_trap_reached_by_flow_records_whether_a_call_led_into_it()
		{
			byte[] text =
			{
				// A [0x1000, 0x1006): call, then int3: the compiler's mark that the callee never returns
				0xE8, 0x0B, 0x00, 0x00, 0x00, // 1000 call 1010 (R)
				0xCC, // 1005 int3
				0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC,
				// R [0x1010, 0x1013): returns
				0x31, 0xC0, 0xC3, // 1010 xor eax,eax; ret
				0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC,
				// B [0x1020, 0x1026): a trap on a branch, a __debugbreak, not a no-return mark
				0x85, 0xC9, // 1020 test ecx,ecx
				0x75, 0x01, // 1022 jne 1025
				0xCC, // 1024 int3
				0xC3, // 1025 ret
			};
			var flow = new ControlFlow(Fake(text, new[] { new PeFunction(0x1000, 0x1006), new PeFunction(0x1010, 0x1013), new PeFunction(0x1020, 0x1026) }));

			// The trap after the call was reached in the first walk and marked R; in the final walk
			// flow stops at the call, so the trap is no longer reached and only its consequence shows.
			Assert.AreEqual(0, flow.TrapsOf(0x1000).Count);
			Assert.IsTrue(flow.IsNoReturn(0x1010), "R returns by its own bytes, but the trap after the call is the compiler's statement that it never does");
			Assert.IsTrue(flow.IsNoReturn(0x1000), "every path ends in a no-return call");
			Assert.IsFalse(flow.IsCode(0x1005));
			CollectionAssert.AreEqual(new[] { (0x1024u, false) }, flow.TrapsOf(0x1020).ToArray());
			Assert.IsFalse(flow.IsNoReturn(0x1020));
		}

		[TestMethod]
		public void A_bound_set_by_masking_the_index_counts_the_mask_plus_one()
		{
			byte[] text =
			{
				0x48, 0x8D, 0x05, 0xF9, 0x0F, 0x00, 0x00, // 1000 lea rax,[0x2000]
				0x83, 0xE3, 0x03, // 1007 and ebx,3: no cmp, no default, four cases
				0x48, 0x63, 0x0C, 0x98, // 100A movsxd rcx,[rax+rbx*4]
				0x48, 0x03, 0xC8, // 100E add rcx,rax
				0xFF, 0xE1, // 1011 jmp rcx
				0xCC, // 1013
				0xB8, 0x00, 0x00, 0x00, 0x00, 0xC3, // 1014 case 0
				0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3, // 101A case 1
				0xB8, 0x02, 0x00, 0x00, 0x00, 0xC3, // 1020 case 2
				0xB8, 0x03, 0x00, 0x00, 0x00, 0xC3, // 1026 case 3
				0xCC, 0xCC, 0xCC, 0xCC, // 102C
			};
			byte[] rdata = Table(unchecked((uint) (0x1014 - 0x2000)), unchecked((uint) (0x101A - 0x2000)), unchecked((uint) (0x1020 - 0x2000)), unchecked((uint) (0x1026 - 0x2000)), unchecked((uint) (0x1014 - 0x2000)));
			var flow = new ControlFlow(Fake(text, new[] { new PeFunction(0x1000, 0x1030) }, rdata));

			CollectionAssert.AreEqual(new[] { new JumpTable(0x1011, 0x2000, 4, JumpTableBound.Compare) }, flow.JumpTablesOf(0x1000).ToArray());
		}

		[TestMethod]
		public void A_switch_whose_lea_sits_far_above_in_the_entry_block_still_resolves()
		{
			// lea at entry, then more instructions than a backscan reaches, then the dispatch.
			byte[] text = new byte[] { 0x48, 0x8D, 0x05, 0xF9, 0x0F, 0x00, 0x00 } // 1000 lea rax,[0x2000]
				.Concat(Nops(100)) // 1007..106B
				.Concat(new byte[]
				{
					0x48, 0x83, 0xFB, 0x02, // 106B cmp rbx,2
					0x77, 0x0B, // 106F ja 107C
					0x48, 0x63, 0x0C, 0x98, // 1071 movsxd rcx,[rax+rbx*4]
					0x48, 0x03, 0xC8, // 1075 add rcx,rax
					0xFF, 0xE1, // 1078 jmp rcx
					0xCC, 0xCC, // 107A
					0x31, 0xC0, 0xC3, // 107C default
					0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3, // 107F case 0
					0xB8, 0x02, 0x00, 0x00, 0x00, 0xC3, // 1085 case 1
					0xB8, 0x03, 0x00, 0x00, 0x00, 0xC3, // 108B case 2
					0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, // 1091
				}).ToArray();
			byte[] rdata = Table(unchecked((uint) (0x107F - 0x2000)), unchecked((uint) (0x1085 - 0x2000)), unchecked((uint) (0x108B - 0x2000)));
			var flow = new ControlFlow(Fake(text, new[] { new PeFunction(0x1000, 0x10A0) }, rdata));

			CollectionAssert.AreEqual(new[] { new JumpTable(0x1078, 0x2000, 3, JumpTableBound.Compare) }, flow.JumpTablesOf(0x1000).ToArray());
			Assert.AreEqual(0, flow.UnresolvedIndirectJumpsOf(0x1000).Count);
		}

		[TestMethod]
		public void A_dispatcher_loaded_at_entry_and_copied_between_registers_still_marks_the_tail_call()
		{
			byte[] text = new byte[] { 0x4C, 0x8B, 0x35, 0xF9, 0x0F, 0x00, 0x00 } // 1000 mov r14,[0x2000], the dispatch slot
				.Concat(Nops(100)) // 1007..106B
				.Concat(new byte[]
				{
					0x4D, 0x8B, 0xC6, // 106B mov r8,r14
					0x41, 0xFF, 0xE0, // 106E jmp r8
					0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, // 1071
					0xFF, 0xE0, // 1080 D: jmp rax
				}).ToArray();
			var rdata = new byte[0x10];
			BitConverter.GetBytes(ImageBase + 0x1080).CopyTo(rdata, 0);
			var flow = new ControlFlow(Fake(text, new[] { new PeFunction(0x1000, 0x1080) }, rdata, guardDispatchSlot: 0x2000));

			Assert.AreEqual(1, flow.IndirectTailCallsOf(0x1000));
			Assert.AreEqual(0, flow.UnresolvedIndirectJumpsOf(0x1000).Count);
		}

		[TestMethod]
		public void A_discovered_function_that_faults_is_listed_with_the_fault_not_dropped()
		{
			byte[] text =
			{
				// A [0x1000, 0x1010) calls X and Y, neither in .pdata
				0xE8, 0x0B, 0x00, 0x00, 0x00, // 1000 call 1010 (X)
				0xE8, 0x16, 0x00, 0x00, 0x00, // 1005 call 1020 (Y)
				0xC3, // 100A ret
				0xCC, 0xCC, 0xCC, 0xCC, 0xCC,
				// X: one real instruction, then bytes no instruction decodes from
				0x48, 0x8B, 0x41, 0x68, // 1010 mov rax,[rcx+0x68]
				0x0F, 0x04, // 1014 invalid
				0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC,
				// Y: falls straight into B, a .pdata function
				0x31, 0xC0, // 1020 xor eax,eax
				// B [0x1022, 0x1025)
				0x33, 0xC0, 0xC3, // 1022 xor eax,eax; ret
			};
			var flow = new ControlFlow(Fake(text, new[] { new PeFunction(0x1000, 0x1010), new PeFunction(0x1022, 0x1025) }));

			Assert.AreEqual(2, flow.Discovered.Count);
			DiscoveredFunction x = flow.Discovered[0];
			Assert.AreEqual(0x1010u, x.Rva);
			Assert.AreEqual(0x1014u, x.End);
			Assert.AreEqual("invalid instruction at 0x1014", x.Unverified);
			DiscoveredFunction y = flow.Discovered[1];
			Assert.AreEqual(0x1020u, y.Rva);
			Assert.AreEqual(0x1022u, y.End);
			Assert.AreEqual("flow reaches 0x1022, outside the function", y.Unverified);
			Assert.IsTrue(flow.IsCode(0x1010), "what did decode is code, fault or not");
		}

		[TestMethod]
		public void Pointers_from_data_seed_functions_and_an_import_thunk_carries_the_import_name()
		{
			byte[] text =
			{
				0x31, 0xC0, 0xC3, // 1000 A [0x1000, 0x1003), the only .pdata function, calls nothing
				0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC,
				0x48, 0x8B, 0x41, 0x68, 0xC3, // 1010 L: a leaf getter only a vtable slot points at
				0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC,
				0xFF, 0x25, 0xEA, 0x0F, 0x00, 0x00, // 1020 T: jmp [rip+0xFEA] = [0x2010], the IAT slot of GetTickCount
			};
			var rdata = new byte[0x20];
			BitConverter.GetBytes(0x1234UL).CopyTo(rdata, 0x00); // not a code pointer
			BitConverter.GetBytes(ImageBase + 0x1010).CopyTo(rdata, 0x08); // a vtable slot -> L
			BitConverter.GetBytes(ImageBase + 0x1020).CopyTo(rdata, 0x18); // a function table entry -> T
			var imports = new Dictionary<uint, string> { [0x2010] = "KERNEL32.dll!GetTickCount" };
			var flow = new ControlFlow(Fake(text, new[] { new PeFunction(0x1000, 0x1003) }, rdata, imports));

			CollectionAssert.AreEqual(new[] { 0x1010u, 0x1020u }, flow.Discovered.Select(f => f.Rva).ToArray());
			DiscoveredFunction leaf = flow.Discovered[0];
			Assert.AreEqual(FunctionOrigin.Pointer, leaf.Origin);
			Assert.AreEqual(0x2008u, leaf.Witness);
			Assert.AreEqual(0x1015u, leaf.End);
			Assert.IsNull(leaf.Unverified);
			Assert.IsNull(leaf.Name);
			DiscoveredFunction thunk = flow.Discovered[1];
			Assert.AreEqual(FunctionOrigin.Pointer, thunk.Origin);
			Assert.AreEqual(0x2018u, thunk.Witness);
			Assert.AreEqual("KERNEL32.dll!GetTickCount", thunk.Name);
		}

		[TestMethod]
		public void A_discovered_function_owns_only_its_contiguous_run_so_a_function_between_its_blocks_is_still_found()
		{
			byte[] text =
			{
				// A [0x1000, 0x1006) calls L
				0xE8, 0x0B, 0x00, 0x00, 0x00, // 1000 call 1010 (L)
				0xC3, // 1005 ret
				0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC,
				// L: two blocks with a gap between them
				0x85, 0xC9, // 1010 test ecx,ecx
				0x75, 0x2C, // 1012 jne 1040
				0xC3, // 1014 ret
				0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC,
				// T: a leaf only a pointer names, sitting in L's gap
				0x48, 0x8B, 0x41, 0x68, 0xC3, // 1020 mov rax,[rcx+0x68]; ret
				0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC,
				0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC,
				// L's second block
				0x31, 0xC0, 0xC3, // 1040 xor eax,eax; ret
			};
			var rdata = new byte[0x10];
			BitConverter.GetBytes(ImageBase + 0x1020).CopyTo(rdata, 0);
			var flow = new ControlFlow(Fake(text, new[] { new PeFunction(0x1000, 0x1006) }, rdata));

			CollectionAssert.AreEqual(new[] { 0x1010u, 0x1020u }, flow.Discovered.Select(f => f.Rva).ToArray());
			Assert.AreEqual(0x1015u, flow.Discovered[0].End, "L's contiguous run");
			CollectionAssert.AreEqual(new[] { 0x1010u, 0x1014u, 0x1040u }, flow.BlocksOf(0x1010).ToArray());
			Assert.IsTrue(flow.IsCode(0x1040));
			Assert.AreEqual(FunctionOrigin.Pointer, flow.Discovered[1].Origin);
		}

		[TestMethod]
		public void The_real_image_walks_and_every_discovered_function_sits_outside_pdata_inside_text()
		{
			var image = PeImage.Open(PeImageTests.ExePath());
			var watch = System.Diagnostics.Stopwatch.StartNew();
			var flow = new ControlFlow(image);
			watch.Stop();

			uint textStart = image.Text.VirtualAddress;
			uint textEnd = textStart + (uint) image.Text.RawSize;
			var pdataRanges = image.Functions.Select(f => (f.Begin, f.End)).OrderBy(r => r.Begin).ToList();
			bool InPdata(uint rva)
			{
				int lo = 0, hi = pdataRanges.Count - 1;
				while (lo <= hi)
				{
					int mid = (lo + hi) / 2;
					if (pdataRanges[mid].Begin <= rva) lo = mid + 1;
					else hi = mid - 1;
				}
				return hi >= 0 && rva < pdataRanges[hi].End;
			}

			int primaries = image.Functions.Count(f => f.Parent == 0);
			var pdata = flow.Functions.Where(f => f.Origin == FunctionOrigin.Pdata).ToList();
			Assert.AreEqual(primaries, pdata.Count, "one flow function per .pdata primary");
			Assert.IsTrue(flow.Discovered.All(f => f.Rva >= textStart && f.Rva < textEnd), "a discovered function outside .text");
			Assert.IsTrue(flow.Discovered.All(f => !InPdata(f.Rva)), "a discovered function inside a .pdata range");
			Assert.IsTrue(flow.Discovered.Where(f => f.Unverified == null).All(f => f.End > f.Rva), "a verified function with no bytes");

			var report = new System.Text.StringBuilder();
			report.AppendLine($"walk {watch.Elapsed.TotalSeconds:F2}s, .text {image.Text.RawSize / 1048576.0:F0} MB, .pdata entries {image.Functions.Count}, primaries {primaries}");
			report.AppendLine($"functions {flow.Functions.Count}: pdata {pdata.Count}, discovered {flow.Discovered.Count} (verified {flow.Discovered.Count(f => f.Unverified == null)}, unverified {flow.Discovered.Count(f => f.Unverified != null)}), named thunks {flow.Discovered.Count(f => f.Name != null)}");
			foreach (var group in flow.Discovered.GroupBy(f => f.Origin).OrderBy(g => g.Key))
			{
				report.AppendLine($"  discovered by {group.Key}: {group.Count()} (verified {group.Count(f => f.Unverified == null)})");
			}
			report.AppendLine($"no-return functions {flow.Functions.Count(f => flow.IsNoReturn(f.Rva))}");
			var index = new CodeIndex(image);
			var formatter = new Iced.Intel.NasmFormatter();
			var output = new Iced.Intel.StringOutput();
			string Format(Iced.Intel.Instruction ins)
			{
				formatter.Format(ins, output);
				return $"{ins.IP - image.ImageBase:X}: {output.ToStringAndReset()}";
			}
			IEnumerable<Iced.Intel.Instruction> Linear(DiscoveredFunction f) => index.Decode(new PeFunction(f.Rva, f.End));

			var unresolved = flow.Functions.SelectMany(f => flow.UnresolvedIndirectJumpsOf(f.Rva).Select(at => (Function: f, At: at))).ToList();
			var tables = flow.Functions.SelectMany(f => flow.JumpTablesOf(f.Rva)).ToList();
			report.AppendLine($"jump tables {tables.Count} (compare-bounded {tables.Count(t => t.Bound == JumpTableBound.Compare)}, range-bounded {tables.Count(t => t.Bound == JumpTableBound.Range)}, cases {tables.Sum(t => t.Cases)}), unresolved indirect jumps {unresolved.Count}");
			var unresolvedByShape = new Dictionary<string, (int Count, (DiscoveredFunction Function, uint At) Example)>();
			foreach (var site in unresolved)
			{
				Iced.Intel.Instruction jmp = Linear(site.Function).FirstOrDefault(i => i.IP - image.ImageBase == site.At);
				string shape = jmp.IsIPRelativeMemoryOperand ? $"jmp [0x{jmp.IPRelativeMemoryAddress - image.ImageBase:X}]" : jmp.Op0Kind == Iced.Intel.OpKind.Register ? "jmp register" : $"jmp {jmp.Op0Kind}";
				unresolvedByShape[shape] = (unresolvedByShape.TryGetValue(shape, out var have) ? have.Count + 1 : 1, site);
			}
			foreach (var shape in unresolvedByShape.OrderByDescending(s => s.Value.Count).Take(8))
			{
				report.AppendLine($"  unresolved {shape.Key}: {shape.Value.Count}, e.g. in 0x{shape.Value.Example.Function.Rva:X} at 0x{shape.Value.Example.At:X}");
			}
			foreach (var site in unresolved.Where(s => Linear(s.Function).Any(i => i.IP - image.ImageBase == s.At && i.Op0Kind == Iced.Intel.OpKind.Register)).Take(4))
			{
				var instructions = Linear(site.Function).ToList();
				int at = instructions.FindIndex(i => i.IP - image.ImageBase == site.At);
				report.AppendLine($"  register jump in 0x{site.Function.Rva:X}:");
				foreach (Iced.Intel.Instruction ins in instructions.Skip(Math.Max(0, at - 8)).Take(9)) report.AppendLine($"    {Format(ins)}");
			}

			var noReturn = pdata.Where(f => flow.IsNoReturn(f.Rva)).ToList();
			report.AppendLine($"no-return pdata functions {noReturn.Count}, indirect tail calls {flow.Functions.Sum(f => flow.IndirectTailCallsOf(f.Rva))}");
			var noReturnCallees = noReturn.SelectMany(f => flow.CalleesOf(f.Rva).Where(flow.IsNoReturn)).GroupBy(c => c).OrderByDescending(g => g.Count()).Take(6).ToList();
			foreach (var callee in noReturnCallees)
			{
				DiscoveredFunction f = flow.Functions.First(x => x.Rva == callee.Key);
				report.AppendLine($"  no-return callee 0x{callee.Key:X} called by {callee.Count()} no-return functions, {f.Origin}, {f.End - f.Rva} bytes{(f.Name != null ? ", " + f.Name : "")}:");
				foreach (Iced.Intel.Instruction ins in Linear(f).Take(8)) report.AppendLine($"    {Format(ins)}");
			}
			report.AppendLine($"  no-return functions with no callee at all {noReturn.Count(f => flow.CalleesOf(f.Rva).Count == 0)}");
			var traps = flow.Functions.SelectMany(f => flow.TrapsOf(f.Rva).Select(t => (Function: f, t.At, t.AfterCall))).ToList();
			report.AppendLine($"traps reached by flow {traps.Count}: after a call {traps.Count(t => t.AfterCall)}, after something else {traps.Count(t => !t.AfterCall)}");
			foreach (var trap in traps.Where(t => !t.AfterCall).Take(4))
			{
				var instructions = Linear(trap.Function).ToList();
				int at = instructions.FindIndex(i => i.IP - image.ImageBase == trap.At);
				report.AppendLine($"  trap not after a call in 0x{trap.Function.Rva:X}:");
				foreach (Iced.Intel.Instruction ins in instructions.Skip(Math.Max(0, at - 4)).Take(6)) report.AppendLine($"    {Format(ins)}");
			}
			var byReason = noReturn.GroupBy(f => flow.CalleesOf(f.Rva).Any(flow.IsNoReturn) ? "calls a no-return function" : flow.ImportCallsOf(f.Rva).Count > 0 ? "calls an import" : "neither").ToDictionary(g => g.Key, g => g.ToList());
			foreach (var reason in byReason) report.AppendLine($"  no-return because it {reason.Key}: {reason.Value.Count}");
			var unprovenCallees = noReturn.SelectMany(f => flow.CalleesOf(f.Rva).Where(c => !flow.IsNoReturn(c))).GroupBy(c => c).OrderByDescending(g => g.Count()).Take(3).ToList();
			foreach (var callee in unprovenCallees)
			{
				DiscoveredFunction f = flow.Functions.FirstOrDefault(x => x.Rva == callee.Key);
				if (f == null)
				{
					report.AppendLine($"  unproven callee 0x{callee.Key:X} called by {callee.Count()} no-return functions is not a function start ({(flow.TryFunctionOf(callee.Key, out uint inside) ? $"inside 0x{inside:X}" : "inside nothing")})");
					continue;
				}
				report.AppendLine($"  unproven callee 0x{callee.Key:X} called by {callee.Count()} no-return functions, {f.Origin}, {f.End - f.Rva} bytes, callees {string.Join(",", flow.CalleesOf(f.Rva).Select(c => $"0x{c:X}{(flow.IsNoReturn(c) ? "!" : "")}"))}, imports {string.Join(",", flow.ImportCallsOf(f.Rva).Select(c => c.Import))}, indirect tails {flow.IndirectTailCallsOf(f.Rva)}, unresolved {flow.UnresolvedIndirectJumpsOf(f.Rva).Count}, fault {f.Unverified ?? "none"}:");
				foreach (Iced.Intel.Instruction ins in Linear(f).Take(14)) report.AppendLine($"    {Format(ins)}");
			}
			foreach (DiscoveredFunction f in byReason.SelectMany(r => r.Value.Take(2)))
			{
				report.AppendLine($"  no-return 0x{f.Rva:X} ({f.End - f.Rva} bytes), callees {string.Join(",", flow.CalleesOf(f.Rva).Select(c => $"0x{c:X}{(flow.IsNoReturn(c) ? "!" : "")}"))}, imports {string.Join(",", flow.ImportCallsOf(f.Rva).Select(c => c.Import))}:");
				foreach (Iced.Intel.Instruction ins in Linear(f).Take(14)) report.AppendLine($"    {Format(ins)}");
			}
			var pdataFaults = pdata.Where(f => f.Unverified != null).ToList();
			report.AppendLine($"pdata functions whose walk faulted {pdataFaults.Count}");
			foreach (var group in pdataFaults.GroupBy(f => f.Unverified.Split(' ')[0] + " " + f.Unverified.Split(' ')[1]).OrderByDescending(g => g.Count()).Take(6))
			{
				report.AppendLine($"  {group.Key}: {group.Count()}, e.g. 0x{group.First().Rva:X}: {group.First().Unverified}");
			}
			foreach (DiscoveredFunction f in pdataFaults.Take(8))
			{
				// The instruction that ends at the fault address is the one flow fell through from.
				uint fault = Convert.ToUInt32(f.Unverified.Split(' ').First(w => w.StartsWith("0x")).TrimEnd(','), 16);
				Iced.Intel.Instruction last = Linear(f).FirstOrDefault(i => i.IP - image.ImageBase + (ulong) i.Length == fault);
				PeFunction entryAt = image.Functions.FirstOrDefault(e => e.Begin == fault);
				string owner = flow.TryFunctionOf(fault, out uint ownerRva) ? $"owned by 0x{ownerRva:X}" : "owned by nobody";
				report.AppendLine($"  0x{f.Rva:X}: {f.Unverified}; last instruction {Format(last)}; {owner}; pdata entry at fault: {(entryAt.End == 0 ? "none" : $"[0x{entryAt.Begin:X},0x{entryAt.End:X}) parent 0x{entryAt.Parent:X}")}; tables {string.Join(" ", flow.JumpTablesOf(f.Rva).Select(t => $"{t.Bound}@0x{t.TableRva:X}x{t.Cases}"))}");
			}
			foreach (DiscoveredFunction f in pdataFaults.Where(f => f.Unverified.StartsWith("invalid")).Take(2))
			{
				var instructions = Linear(f).ToList();
				foreach (JumpTable t in flow.JumpTablesOf(f.Rva))
				{
					report.AppendLine($"  0x{f.Rva:X} table {t.Bound}@0x{t.TableRva:X}x{t.Cases}, the dispatch:");
					int at = instructions.FindIndex(i => i.IP - image.ImageBase == t.At);
					foreach (Iced.Intel.Instruction ins in instructions.Skip(Math.Max(0, at - 10)).Take(11)) report.AppendLine($"    {Format(ins)}");
				}
			}
			foreach (DiscoveredFunction f in flow.Discovered.Where(f => f.Unverified != null).Take(6))
			{
				uint fault = Convert.ToUInt32(f.Unverified.Split(' ').First(w => w.StartsWith("0x")).TrimEnd(','), 16);
				string owner = flow.TryFunctionOf(fault, out uint ownerRva) ? $"owned by 0x{ownerRva:X} ({flow.Functions.First(x => x.Rva == ownerRva).Origin})" : "owned by nobody";
				report.AppendLine($"  discovered 0x{f.Rva:X} by {f.Origin} from 0x{f.Witness:X}: {f.Unverified}; {owner}");
				foreach (Iced.Intel.Instruction ins in Linear(f).Take(6)) report.AppendLine($"    {Format(ins)}");
			}
			foreach (var group in flow.Discovered.Where(f => f.Unverified != null).GroupBy(f => f.Unverified.Split(' ')[0] + " " + f.Unverified.Split(' ')[1]).OrderByDescending(g => g.Count()).Take(6))
			{
				report.AppendLine($"  discovered {group.Key}: {group.Count()}, e.g. 0x{group.First().Rva:X} by {group.First().Origin} from 0x{group.First().Witness:X}: {group.First().Unverified}");
			}
			var groups = image.Functions.GroupBy(f => f.Parent == 0 ? f.Begin : f.Parent).Where(g => g.Count() > 1).OrderByDescending(g => g.Count()).ToList();
			report.AppendLine($"chain groups with more than one range {groups.Count}; biggest: {string.Join(" ", groups.Take(5).Select(g => $"0x{g.Key:X}={g.Count()} spanning 0x{g.Min(f => f.Begin):X}..0x{g.Max(f => f.End):X}"))}");
			PeFunction chainedCallee = image.Functions.FirstOrDefault(f => f.Begin == 0xB9D22A0);
			report.AppendLine($"entry at 0xB9D22A0: {(chainedCallee.End == 0 ? "none" : $"[0x{chainedCallee.Begin:X},0x{chainedCallee.End:X}) parent 0x{chainedCallee.Parent:X}")}");
			var chainedBegins = new HashSet<uint>(image.Functions.Where(e => e.Parent != 0).Select(e => e.Begin));
			var chainedCallTargets = flow.Functions.SelectMany(f => flow.CalleesOf(f.Rva)).Distinct().Where(chainedBegins.Contains).ToList();
			report.AppendLine($"distinct call targets that are chained range begins {chainedCallTargets.Count}: {string.Join(" ", chainedCallTargets.Take(8).Select(c => $"0x{c:X}"))}");
			DiscoveredFunction claimant = flow.Functions.FirstOrDefault(f => f.Rva == 0xB9AAD60);
			if (claimant != null) report.AppendLine($"0xB9AAD60: {claimant.Origin} from 0x{claimant.Witness:X}, end 0x{claimant.End:X}, {flow.BlocksOf(claimant.Rva).Count} blocks, fault {claimant.Unverified ?? "none"}, tables {string.Join(" ", flow.JumpTablesOf(claimant.Rva).Select(t => $"{t.Bound}@0x{t.TableRva:X}x{t.Cases}"))}, unresolved {flow.UnresolvedIndirectJumpsOf(claimant.Rva).Count}");
			report.AppendLine($"IAT 0xD209F50 = {(image.Imports.TryGetValue(0xD209F50, out string slotName) ? slotName : "not an import")}");
			foreach (uint target in new[] { 0xB9D22A0u, 0xB9D2210u })
			{
				var covering = image.Functions.Where(e => e.Begin <= target && target < e.End).ToList();
				report.AppendLine($"0x{target:X}: covered by {string.Join(" ", covering.Select(e => $"[0x{e.Begin:X},0x{e.End:X}) parent 0x{e.Parent:X}"))}; flow owner {(flow.TryFunctionOf(target, out uint o) ? $"0x{o:X}" : "none")}; IsCode {flow.IsCode(target)}");
				foreach (Iced.Intel.Instruction ins in index.Decode(new PeFunction(target, target + 48)).Take(12)) report.AppendLine($"    {Format(ins)}");
			}
			long codeBytes = 0;
			for (uint rva = textStart; rva < textEnd; rva++) if (flow.IsCode(rva)) codeBytes++;
			report.AppendLine($"instruction starts reached {codeBytes}");
			string text = report.ToString();
			Console.WriteLine(text);
			File.WriteAllText(@"C:\Development\github\MiNET\temp_auto\controlflow-report.txt", text);
		}
	}
}
