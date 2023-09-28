using System;
using System.Collections.Generic;
using System.Numerics;
using MiNET.Items;
using MiNET.Net.Crafting;
using MiNET.Utils;
using MiNET.Utils.Vectors;

namespace MiNET.Net
{
	public class ItemStackRequests : List<ItemStackActionList>, IPacketDataObject
	{
		public void Write(Packet packet)
		{
			packet.WriteUnsignedVarInt((uint) Count);

			foreach (var request in this)
				packet.Write(request);
		}

		public static ItemStackRequests Read(Packet packet)
		{
			var request = new ItemStackRequests();

			var count = packet.ReadUnsignedVarInt();
			for (var i = 0; i < count; i++)
				request.Add(ItemStackActionList.Read(packet));

			return request;
		}
	}

	public class ItemStackActionList : List<ItemStackAction>, IPacketDataObject
	{
		public int RequestId { get; set; }

		public List<string> FilterStrings { get; set; } = new List<string>();

		public int FilterStringCause { get; set; }

		public void Write(Packet packet)
		{
			packet.WriteVarInt(RequestId);

			packet.WriteUnsignedVarInt((uint) Count);
			foreach (var action in this)
				packet.Write(action);

			packet.WriteUnsignedVarInt((uint) FilterStrings.Count);
			foreach (var filter in FilterStrings)
				packet.Write(filter);

			packet.Write(FilterStringCause);
		}

		public static ItemStackActionList Read(Packet packet)
		{
			var actions = new ItemStackActionList()
			{
				RequestId = packet.ReadVarInt()
			};

			var actionsCount = packet.ReadUnsignedVarInt();
			for (var i = 0; i < actionsCount; i++)
				actions.Add(ItemStackAction.Read(packet));

			var filtersCount = packet.ReadUnsignedVarInt();
			for (var i = 0; i < filtersCount; i++)
				actions.FilterStrings.Add(packet.ReadString());

			actions.FilterStringCause = packet.ReadInt();

			return actions;
		}
	}

	public abstract class ItemStackAction : IPacketDataObject
	{
		public abstract McpeItemStackRequest.ActionType Type { get; }

		public void Write(Packet packet)
		{
			packet.Write((byte) Type);
			WriteData(packet);
		}

		protected virtual void WriteData(Packet packet) { }

		public static ItemStackAction Read(Packet packet)
		{
			var type = (McpeItemStackRequest.ActionType) packet.ReadByte();

			return type switch
			{
				McpeItemStackRequest.ActionType.Take => TakeAction.ReadData(packet),
				McpeItemStackRequest.ActionType.Place => PlaceAction.ReadData(packet),
				McpeItemStackRequest.ActionType.Swap => SwapAction.ReadData(packet),
				McpeItemStackRequest.ActionType.Drop => DropAction.ReadData(packet),
				McpeItemStackRequest.ActionType.Destroy => DestroyAction.ReadData(packet),
				McpeItemStackRequest.ActionType.Consume => ConsumeAction.ReadData(packet),
				McpeItemStackRequest.ActionType.Create => CreateAction.ReadData(packet),
				McpeItemStackRequest.ActionType.PlaceIntoBundle => PlaceIntoBundleAction.ReadData(packet),
				McpeItemStackRequest.ActionType.TakeFromBundle => TakeFromBundleAction.ReadData(packet),
				//McpeItemStackRequest.ActionType.LabTableCombine => LabTableCombineAction.ReadData(packet), // nothing
				McpeItemStackRequest.ActionType.BeaconPayment => BeaconPaymentAction.ReadData(packet),
				McpeItemStackRequest.ActionType.MineBlock => MineBlockAction.ReadData(packet),
				McpeItemStackRequest.ActionType.CraftRecipe => CraftAction.ReadData(packet),
				McpeItemStackRequest.ActionType.CraftRecipeAuto => CraftAutoAction.ReadData(packet),
				McpeItemStackRequest.ActionType.CraftCreative => CraftCreativeAction.ReadData(packet),
				McpeItemStackRequest.ActionType.CraftRecipeOptional => CraftRecipeOptionalAction.ReadData(packet),
				McpeItemStackRequest.ActionType.CraftGrindstone => GrindstoneStackRequestAction.ReadData(packet),
				McpeItemStackRequest.ActionType.CraftLoom => LoomStackRequestAction.ReadData(packet),
				//McpeItemStackRequest.ActionType.CraftNotImplementedDeprecated => CraftNotImplementedDeprecatedAction.ReadData(packet), // nothing
				McpeItemStackRequest.ActionType.CraftResultsDeprecated => CraftResultDeprecatedAction.ReadData(packet),
				_ => throw new ArgumentException($"Unexpected action type [{type}]")
			};
		}
	}

	public class StackRequestSlotInfo : IPacketDataObject
	{
		public byte ContainerId { get; set; }
		public byte Slot { get; set; }
		public int StackNetworkId { get; set; }

		public void Write(Packet packet)
		{
			packet.Write(ContainerId);
			packet.Write(Slot);
			packet.WriteVarInt(StackNetworkId);
		}

		public static StackRequestSlotInfo Read(Packet packet)
		{
			return new StackRequestSlotInfo()
			{
				ContainerId = packet.ReadByte(),
				Slot = packet.ReadByte(),
				StackNetworkId = packet.ReadVarInt()
			};
		}
	}

	public abstract class TakeOrPlaceAction : ItemStackAction
	{
		public byte Count { get; set; }
		public StackRequestSlotInfo Source { get; set; }
		public StackRequestSlotInfo Destination { get; set; }

		protected override void WriteData(Packet packet)
		{
			packet.Write(Count);
			packet.Write(Source);
			packet.Write(Destination);
		}

		internal static ItemStackAction ReadData(Packet packet, TakeOrPlaceAction action)
		{
			action.Count = packet.ReadByte();
			action.Source = StackRequestSlotInfo.Read(packet);
			action.Destination = StackRequestSlotInfo.Read(packet);

			return action;
		}
	}

	public class TakeAction : TakeOrPlaceAction
	{
		public override McpeItemStackRequest.ActionType Type => McpeItemStackRequest.ActionType.Take;

		internal static ItemStackAction ReadData(Packet packet) => ReadData(packet, new TakeAction());
	}

	public class PlaceAction : TakeOrPlaceAction
	{
		public override McpeItemStackRequest.ActionType Type => McpeItemStackRequest.ActionType.Place;

		internal static ItemStackAction ReadData(Packet packet) => ReadData(packet, new PlaceAction());
	}

	public class SwapAction : ItemStackAction
	{
		public override McpeItemStackRequest.ActionType Type => McpeItemStackRequest.ActionType.Swap;

		public StackRequestSlotInfo Source { get; set; }
		public StackRequestSlotInfo Destination { get; set; }

		protected override void WriteData(Packet packet)
		{
			packet.Write(Source);
			packet.Write(Destination);
		}

		internal static ItemStackAction ReadData(Packet packet)
		{
			return new SwapAction()
			{
				Source = StackRequestSlotInfo.Read(packet),
				Destination = StackRequestSlotInfo.Read(packet)
			};
		}
	}

	public class DropAction : ItemStackAction
	{
		public override McpeItemStackRequest.ActionType Type => McpeItemStackRequest.ActionType.Drop;

		public byte Count { get; set; }
		public StackRequestSlotInfo Source { get; set; }
		public bool Randomly { get; set; }

		protected override void WriteData(Packet packet)
		{
			packet.Write(Count);
			packet.Write(Source);
			packet.Write(Randomly);
		}

		internal static ItemStackAction ReadData(Packet packet)
		{
			return new DropAction()
			{
				Count = packet.ReadByte(),
				Source = StackRequestSlotInfo.Read(packet),
				Randomly = packet.ReadBool()
			};
		}
	}

	public abstract class DisappearStackRequestAction : ItemStackAction
	{
		public byte Count { get; set; }
		public StackRequestSlotInfo Source { get; set; }

		protected override void WriteData(Packet packet)
		{
			packet.Write(Count);
			packet.Write(Source);
		}

		internal static ItemStackAction ReadData(Packet packet, DisappearStackRequestAction action)
		{
			action.Count = packet.ReadByte();
			action.Source = StackRequestSlotInfo.Read(packet);

			return action;
		}
	}

	public class DestroyAction : DisappearStackRequestAction
	{
		public override McpeItemStackRequest.ActionType Type => McpeItemStackRequest.ActionType.Destroy;

		internal static ItemStackAction ReadData(Packet packet) => ReadData(packet, new DestroyAction());
	}

	public class ConsumeAction : DisappearStackRequestAction
	{
		public override McpeItemStackRequest.ActionType Type => McpeItemStackRequest.ActionType.Consume;

		internal static ItemStackAction ReadData(Packet packet) => ReadData(packet, new ConsumeAction());
	}

	public class CreateAction : ItemStackAction
	{
		public override McpeItemStackRequest.ActionType Type => McpeItemStackRequest.ActionType.Create;

		public byte ResultSlot { get; set; }

		protected override void WriteData(Packet packet)
		{
			packet.Write(ResultSlot);
		}

		internal static ItemStackAction ReadData(Packet packet)
		{
			return new CreateAction()
			{
				ResultSlot = packet.ReadByte()
			};
		}
	}

	public class LabTableCombineAction : ItemStackAction
	{
		public override McpeItemStackRequest.ActionType Type => McpeItemStackRequest.ActionType.LabTableCombine;
	}

	public class MineBlockAction : ItemStackAction
	{
		public override McpeItemStackRequest.ActionType Type => McpeItemStackRequest.ActionType.MineBlock;

		public int HotbarSlot { get; set; }
		public int PredictedDurability { get; set; }
		public int StackNetworkId { get; set; }

		protected override void WriteData(Packet packet)
		{
			packet.WriteVarInt(HotbarSlot);
			packet.WriteVarInt(PredictedDurability);
			packet.WriteVarInt(StackNetworkId);
		}

		internal static ItemStackAction ReadData(Packet packet)
		{
			return new MineBlockAction()
			{
				HotbarSlot = packet.ReadVarInt(),
				PredictedDurability = packet.ReadVarInt(),
				StackNetworkId = packet.ReadVarInt()
			};
		}
	}

	public class BeaconPaymentAction : ItemStackAction
	{
		public override McpeItemStackRequest.ActionType Type => McpeItemStackRequest.ActionType.BeaconPayment;

		public int PrimaryEffect { get; set; }
		public int SecondaryEffect { get; set; }

		protected override void WriteData(Packet packet)
		{
			packet.WriteVarInt(PrimaryEffect);
			packet.WriteVarInt(SecondaryEffect);
		}

		internal static ItemStackAction ReadData(Packet packet)
		{
			return new BeaconPaymentAction()
			{
				PrimaryEffect = packet.ReadVarInt(),
				SecondaryEffect = packet.ReadVarInt()
			};
		}
	}

	public class CraftAction : ItemStackAction
	{
		public override McpeItemStackRequest.ActionType Type => McpeItemStackRequest.ActionType.CraftRecipe;

		public uint RecipeNetworkId { get; set; }

		protected override void WriteData(Packet packet)
		{
			packet.WriteUnsignedVarInt(RecipeNetworkId);
		}

		internal static ItemStackAction ReadData(Packet packet)
		{
			return new CraftAction()
			{
				RecipeNetworkId = packet.ReadUnsignedVarInt()
			};
		}
	}

	public class CraftAutoAction : ItemStackAction
	{
		public override McpeItemStackRequest.ActionType Type => McpeItemStackRequest.ActionType.CraftRecipeAuto;

		public uint RecipeNetworkId { get; set; }
		public byte Repetitions { get; set; }
		public List<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();

		protected override void WriteData(Packet packet)
		{
			packet.Write(RecipeNetworkId);
			packet.Write(Repetitions);

			packet.Write((byte) RecipeIngredients.Count);
			foreach (var ingredient in RecipeIngredients)
				packet.Write(ingredient);
		}

		internal static ItemStackAction ReadData(Packet packet)
		{
			var action = new CraftAutoAction()
			{
				RecipeNetworkId = packet.ReadUnsignedVarInt(),
				Repetitions = packet.ReadByte()
			};

			var count = packet.ReadByte();
			for (var i = 0; i < count; i++)
				action.RecipeIngredients.Add(RecipeIngredient.Read(packet));

			return action;
		}
	}

	public class CraftCreativeAction : ItemStackAction
	{
		public override McpeItemStackRequest.ActionType Type => McpeItemStackRequest.ActionType.CraftCreative;

		public uint CreativeItemNetworkId { get; set; }

		protected override void WriteData(Packet packet)
		{
			packet.WriteUnsignedVarInt(CreativeItemNetworkId);
		}

		internal static ItemStackAction ReadData(Packet packet)
		{
			return new CraftCreativeAction()
			{
				CreativeItemNetworkId = packet.ReadUnsignedVarInt()
			};
		}
	}

	public class CraftRecipeOptionalAction : ItemStackAction
	{
		public override McpeItemStackRequest.ActionType Type => McpeItemStackRequest.ActionType.CraftRecipeOptional;

		public uint RecipeNetworkId { get; set; }
		public int FilteredStringIndex { get; set; }

		protected override void WriteData(Packet packet)
		{
			packet.WriteUnsignedVarInt(RecipeNetworkId);
			packet.Write(FilteredStringIndex);
		}

		internal static ItemStackAction ReadData(Packet packet)
		{
			return new CraftRecipeOptionalAction()
			{
				RecipeNetworkId = packet.ReadUnsignedVarInt(),
				FilteredStringIndex = packet.ReadInt()
			};
		}
	}

	public class GrindstoneStackRequestAction : ItemStackAction
	{
		public override McpeItemStackRequest.ActionType Type => McpeItemStackRequest.ActionType.CraftGrindstone;

		public uint RecipeNetworkId { get; set; }
		public int RepairCost { get; set; }

		protected override void WriteData(Packet packet)
		{
			packet.WriteUnsignedVarInt(RecipeNetworkId);
			packet.WriteVarInt(RepairCost);
		}

		internal static ItemStackAction ReadData(Packet packet)
		{
			return new GrindstoneStackRequestAction()
			{
				RecipeNetworkId = packet.ReadUnsignedVarInt(),
				RepairCost = packet.ReadVarInt()
			};
		}
	}

	public class LoomStackRequestAction : ItemStackAction
	{
		public override McpeItemStackRequest.ActionType Type => McpeItemStackRequest.ActionType.CraftLoom;

		public string PatternId { get; set; }

		protected override void WriteData(Packet packet)
		{
			packet.Write(PatternId);
		}

		internal static ItemStackAction ReadData(Packet packet)
		{
			return new LoomStackRequestAction()
			{
				PatternId = packet.ReadString()
			};
		}
	}

	public class PlaceIntoBundleAction : TakeOrPlaceAction
	{
		public override McpeItemStackRequest.ActionType Type => McpeItemStackRequest.ActionType.PlaceIntoBundle;

		internal static ItemStackAction ReadData(Packet packet) => ReadData(packet, new PlaceIntoBundleAction());
	}

	public class TakeFromBundleAction : TakeOrPlaceAction
	{
		public override McpeItemStackRequest.ActionType Type => McpeItemStackRequest.ActionType.TakeFromBundle;

		internal static ItemStackAction ReadData(Packet packet) => ReadData(packet, new TakeFromBundleAction());
	}

	public class CraftNotImplementedDeprecatedAction : ItemStackAction
	{
		public override McpeItemStackRequest.ActionType Type => McpeItemStackRequest.ActionType.CraftNotImplementedDeprecated;

		// nothing
	}

	public class CraftResultDeprecatedAction : ItemStackAction
	{
		public override McpeItemStackRequest.ActionType Type => McpeItemStackRequest.ActionType.CraftResultsDeprecated;

		public ItemStacks ResultItems { get; set; } = new ItemStacks();
		public byte TimesCrafted { get; set; }

		protected override void WriteData(Packet packet)
		{
			packet.WriteUnsignedVarInt((uint) ResultItems.Count);
			foreach (var item in ResultItems)
				packet.Write(item, false);

			packet.Write(TimesCrafted);
		}

		internal static ItemStackAction ReadData(Packet packet)
		{
			var action = new CraftResultDeprecatedAction();

			var count = packet.ReadUnsignedVarInt();
			for (var i = 0; i < count; i++)
				action.ResultItems.Add(packet.ReadItem(false));

			action.TimesCrafted = packet.ReadByte();

			return action;
		}
	}

	public class ItemStackResponses : List<ItemStackResponse>, IPacketDataObject
	{
		public void Write(Packet packet)
		{
			packet.WriteUnsignedVarInt((uint) Count);
			foreach (var response in this)
				packet.Write(response);
		}

		public static ItemStackResponses Read(Packet packet)
		{
			var responses = new ItemStackResponses();

			var count = packet.ReadUnsignedVarInt();
			for (var i = 0; i < count; i++)
				responses.Add(ItemStackResponse.Read(packet));

			return responses;
		}
	}

	public class ItemStackResponse : IPacketDataObject
	{
		public int RequestId { get; set; }
		public StackResponseStatus Result { get; set; } = StackResponseStatus.Ok;
		public List<StackResponseContainerInfo> ResponseContainerInfos { get; set; } = new List<StackResponseContainerInfo>();

		public void Write(Packet packet)
		{
			packet.Write((byte) Result);
			packet.WriteVarInt(RequestId);

			if (Result != StackResponseStatus.Ok)
				return;

			packet.WriteUnsignedVarInt((uint) ResponseContainerInfos.Count);
			foreach (var response in ResponseContainerInfos)
				packet.Write(response);
		}

		public static ItemStackResponse Read(Packet packet)
		{
			var response = new ItemStackResponse()
			{
				Result = (StackResponseStatus) packet.ReadByte(),
				RequestId = packet.ReadVarInt()
			};

			if (response.Result != StackResponseStatus.Ok)
				return response;

			var count = packet.ReadUnsignedVarInt();
			for (var i = 0; i < count; i++)
				response.ResponseContainerInfos.Add(StackResponseContainerInfo.Read(packet));

			return response;
		}
	}

	public enum StackResponseStatus
	{
		Ok = 0x00,
		Error = 0x01
	}

	public class StackResponseContainerInfo : IPacketDataObject
	{
		public byte ContainerId { get; set; }
		public List<StackResponseSlotInfo> Slots { get; set; } = new List<StackResponseSlotInfo>();

		public void Write(Packet packet)
		{
			packet.Write(ContainerId);

			packet.WriteUnsignedVarInt((uint) Slots.Count);
			foreach (var slot in Slots)
				packet.Write(slot);
		}

		public static StackResponseContainerInfo Read(Packet packet)
		{
			var response = new StackResponseContainerInfo()
			{
				ContainerId = packet.ReadByte()
			};

			var count = packet.ReadUnsignedVarInt();
			for (var i = 0; i < count; i++)
				response.Slots.Add(StackResponseSlotInfo.Read(packet));

			return response;
		}
	}

	public class StackResponseSlotInfo : IPacketDataObject
	{
		public byte Slot { get; set; }
		public byte HotbarSlot { get; set; }
		public byte Count { get; set; }
		public int StackNetworkId { get; set; }
		public string CustomName { get; set; }
		public int DurabilityCorrection { get; set; }

		public void Write(Packet packet)
		{
			packet.Write(Slot);
			packet.Write(HotbarSlot);
			packet.Write(Count);
			packet.WriteSignedVarInt(StackNetworkId);
			packet.Write(CustomName);
			packet.WriteSignedVarInt(DurabilityCorrection);
		}

		public static StackResponseSlotInfo Read(Packet packet)
		{
			return new StackResponseSlotInfo()
			{
				Slot = packet.ReadByte(),
				HotbarSlot = packet.ReadByte(),
				Count = packet.ReadByte(),
				StackNetworkId = packet.ReadSignedVarInt(),
				CustomName = packet.ReadString(),
				DurabilityCorrection = packet.ReadSignedVarInt()
			};
		}
	}


	/// <summary>
	/// Old transactions
	/// </summary>

	public abstract class Transaction
	{
		public bool HasNetworkIds { get; set; } = false;

		public int RequestId { get; set; }
		public List<RequestRecord> RequestRecords { get; set; } = new List<RequestRecord>();
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
		public int StackNetworkId { get; set; }

		public int Slot { get; set; }
		public Item OldItem { get; set; }
		public Item NewItem { get; set; }
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