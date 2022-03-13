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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System.Collections.Generic;
using System.Numerics;
using MiNET.Items;
using MiNET.Net;
using MiNET.Utils.Vectors;

namespace MiNET.Utils
{
	public class ItemStackRequests : List<ItemStackActionList>
	{
	}

	public class ItemStackActionList : List<ItemStackAction>
	{
		public int RequestId { get; set; }
	}

	public abstract class ItemStackAction
	{
	}

	public class StackRequestSlotInfo 
	{
		public byte ContainerId { get; set; }
		public byte Slot { get; set; }
		public int StackNetworkId  { get; set; }
	}

	public class TakeAction : ItemStackAction
	{
		public byte Count { get; set; }
		public StackRequestSlotInfo Source { get; set; }
		public StackRequestSlotInfo Destination { get; set; }
	}

	public class PlaceAction : ItemStackAction
	{
		public byte Count { get; set; }
		public StackRequestSlotInfo Source { get; set; }
		public StackRequestSlotInfo Destination { get; set; }
	}

	public class SwapAction : ItemStackAction
	{
		public StackRequestSlotInfo Source { get; set; }
		public StackRequestSlotInfo Destination { get; set; }
	}

	public class DropAction : ItemStackAction
	{
		public byte Count { get; set; }
		public StackRequestSlotInfo Source { get; set; }
		public bool Randomly { get; set; }
	}

	public class DestroyAction : ItemStackAction
	{
		public byte Count { get; set; }
		public StackRequestSlotInfo Source { get; set; }
	}

	public class ConsumeAction : ItemStackAction
	{
		public byte Count { get; set; }
		public StackRequestSlotInfo Source { get; set; }
	}

	public class CreateAction : ItemStackAction
	{
		public byte ResultSlot { get; set; }
	}

	public class LabTableCombineAction : ItemStackAction
	{
	}

	public class BeaconPaymentAction : ItemStackAction
	{
		public int PrimaryEffect { get; set; }
		public int SecondaryEffect { get; set; }
	}

	public class CraftAction : ItemStackAction
	{
		public uint RecipeNetworkId { get; set; }
	}

	public class CraftAutoAction : ItemStackAction
	{
		public uint RecipeNetworkId { get; set; }
	}

	public class CraftCreativeAction : ItemStackAction
	{
		public uint CreativeItemNetworkId { get; set; }
	}

	public class CraftRecipeOptionalAction : ItemStackAction
	{
		public uint RecipeNetworkId { get; set; }
		public int FilteredStringIndex { get; set; }
	}

	public class GrindstoneStackRequestAction : ItemStackAction
	{
		public uint RecipeNetworkId { get; set; }
		public int RepairCost { get; set; }
	}

	public class LoomStackRequestAction : ItemStackAction
	{
		public string PatternId { get; set; }
	}

	public class PlaceIntoBundleAction : ItemStackAction
	{
		
	}
	
	public class TakeFromBundleAction : ItemStackAction
	{
		
	}

	public class CraftNotImplementedDeprecatedAction : ItemStackAction
	{
		// nothing
	}

	public class CraftResultDeprecatedAction : ItemStackAction
	{
		public ItemStacks ResultItems { get; set; } = new ItemStacks();
		public byte TimesCrafted { get; set; }
	}

	public class ItemStackResponses : List<ItemStackResponse>
	{
	}

	public class ItemStackResponse
	{
		public int RequestId { get; set; }
		public StackResponseStatus Result { get; set; } = StackResponseStatus.Ok;
		public List<StackResponseContainerInfo> ResponseContainerInfos { get; set; } = new List<StackResponseContainerInfo>();
	}

	public enum StackResponseStatus
	{
		Ok = 0x00,
		Error = 0x01
	}

	public class StackResponseContainerInfo
	{
		public byte ContainerId { get; set; }
		public List<StackResponseSlotInfo> Slots { get; set; } = new List<StackResponseSlotInfo>();
	}

	public class StackResponseSlotInfo
	{
		public byte Slot           { get; set; }
		public byte HotbarSlot     { get; set; }
		public byte Count          { get; set; }
		public int  StackNetworkId { get; set; }
		public string  CustomName     { get; set; }
		public int DurabilityCorrection { get; set; }
	}


	/// <summary>
	/// Old transactions
	/// </summary>

	public abstract class Transaction
	{
		public bool                    HasNetworkIds   { get; set; } = false;
		
		public int                     RequestId          { get; set; }
		public List<RequestRecord>     RequestRecords     { get; set; } = new List<RequestRecord>();
		public List<TransactionRecord> TransactionRecords { get; set; } = new List<TransactionRecord>();
	}

	public class RequestRecord
	{
		public byte ContainerId { get; set; }
		public List<byte> Slots { get; set; } = new List<byte>();
	}

	public class NormalTransaction : Transaction
	{
	}
	public class InventoryMismatchTransaction : Transaction
	{
	}
	public class ItemUseTransaction : Transaction
	{
		public McpeInventoryTransaction.ItemUseAction ActionType { get; set; }
		public BlockCoordinates Position { get; set; }
		public int Face { get; set; }
		public int Slot { get; set; }
		public Item Item { get; set; }
		public Vector3 FromPosition { get; set; }
		public Vector3 ClickPosition { get; set; }
		public uint BlockRuntimeId { get; set; }
	}
	public class ItemUseOnEntityTransaction : Transaction
	{
		public long EntityId { get; set; }
		public McpeInventoryTransaction.ItemUseOnEntityAction ActionType { get; set; }
		public int Slot { get; set; }
		public Item Item { get; set; }
		public Vector3 FromPosition { get; set; }
		public Vector3 ClickPosition { get; set; }
	}
	public class ItemReleaseTransaction : Transaction
	{
		public McpeInventoryTransaction.ItemReleaseAction ActionType { get; set; }
		public int Slot { get; set; }
		public Item Item { get; set; }
		public Vector3 FromPosition { get; set; }
	}

	public abstract class TransactionRecord
	{
		public int  StackNetworkId { get; set; }
		
		public int  Slot      { get;  set; }
		public Item OldItem   { get;  set; }
		public Item NewItem   { get;  set; }
	}

	public class ContainerTransactionRecord : TransactionRecord
	{
		public int InventoryId { get; set; }
	}

	public class GlobalTransactionRecord : TransactionRecord
	{
	}

	public class WorldInteractionTransactionRecord : TransactionRecord
	{
		public int Flags { get; set; } // NoFlag = 0 WorldInteractionRandom = 1
	}

	public class CreativeTransactionRecord : TransactionRecord
	{
		public int InventoryId { get; set; } = 0x79; // Creative
	}

	public class CraftTransactionRecord : TransactionRecord
	{
		public McpeInventoryTransaction.CraftingAction Action { get; set; }
	}
}