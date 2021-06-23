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

using System;
using System.Numerics;
using fNbt;
using log4net;
using MiNET.Entities.Behaviors;
using MiNET.Items;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Utils.Metadata;
using MiNET.Utils.Nbt;
using MiNET.Worlds;

namespace MiNET.Entities.Passive
{
	public class Horse : PassiveMob, IRideable
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Horse));

		public int Markings { get; set; }
		public bool IsEating { get; set; }
		public double JumpStrength { get; set; }
		public int Temper { get; set; }
		public Entity Rider { get; set; }
		public HorseInventory Inventory { get; set; }

		public Horse(Level level, bool isDonkey = false, Random rnd = null) : base(isDonkey ? EntityType.Donkey : EntityType.Horse, level)
		{
			Width = Length = 1.4;
			Height = 1.6;

			var random = rnd ?? new Random();
			Variant = random.Next(7);
			Markings = random.Next(5);
			Speed = (0.45 + random.NextDouble() * 0.3D + random.NextDouble() * 0.3D + random.NextDouble() * 0.3) * 0.25D;
			JumpStrength = 0.4 + random.NextDouble() * 0.2 + random.NextDouble() * 0.2 + random.NextDouble() * 0.2;

			IsAffectedByGravity = true;
			//IsBreathing = true; // ??
			HasCollision = true;

			Behaviors.Add(new HorseRiddenBehavior(this));
			Behaviors.Add(new PanicBehavior(this, 60, Speed, 1.2));
			Behaviors.Add(new HorseEatBlockBehavior(this, 100));
			Behaviors.Add(new WanderBehavior(this, 0.7));
			Behaviors.Add(new LookAtPlayerBehavior(this));
			Behaviors.Add(new RandomLookaroundBehavior(this));

			Inventory = new HorseInventory(this);
		}

		public override MetadataDictionary GetMetadata()
		{
			EatingHaystack = IsEating ? 32 : 0;
			Scale = IsBaby ? 0.5582917f : 1.0;
			var metadata = base.GetMetadata();
			metadata[(int) MetadataFlags.Variant] = new MetadataInt(Variant);
			metadata[(int) MetadataFlags.Markings] = new MetadataInt(Markings);
			metadata[(int) MetadataFlags.EatingHaystack] = new MetadataInt(EatingHaystack);
			if (IsTamed)
			{
				metadata[45] = new MetadataByte(12);
				metadata[46] = new MetadataInt(2);
			}

			Log.Debug($"Horse: {metadata}");
			return metadata;
		}

		public override EntityAttributes GetEntityAttributes()
		{
			var attributes = base.GetEntityAttributes();
			attributes["minecraft:horse.jump_strength"] = new EntityAttribute
			{
				Name = "minecraft:horse.jump_strength",
				MinValue = 0,
				MaxValue = float.MaxValue,
				Value = (float) JumpStrength
			};

			return attributes;
		}

		public override void DoInteraction(int actionId, Player player)
		{
			if (player.IsSneaking)
			{
				Inventory.Open(player);
				return;
			}

			var inHand = player.Inventory.GetItemInHand();
			if (inHand is ItemSugar || inHand is ItemWheat || inHand is ItemApple || inHand is ItemGoldenCarrot || inHand is ItemGoldenApple || inHand.Id == 170)
			{
				// Feeding

				// Increase temper
				if (inHand is ItemSugar)
				{
					Temper += 3;
					HealthManager.Regen(1);
				}
				else if (inHand is ItemWheat)
				{
					Temper += 3;
					HealthManager.Regen(2);
				}
				else if (inHand is ItemApple)
				{
					Temper += 3;
					HealthManager.Regen(3);
				}
				else if (inHand is ItemGoldenCarrot)
				{
					Temper += 5;
					HealthManager.Regen(4);
				}
				else if (inHand is ItemGoldenApple)
				{
					Temper += 10;
					HealthManager.Regen(10);
				}
				else if (inHand.Id == 170)
				{
					//Temper += 3;
					HealthManager.Regen(20);
				}
			}
			else if (IsTamed && !IsSaddled && inHand is ItemSaddle)
			{
				// Saddle horse

				if (!IsSaddled)
				{
					Inventory.SetSlot(0, inHand);
					player.Inventory.RemoveItems(inHand.Id, 1); // Wrong. Should really be item in hand
				}
			}
			else
			{
				// Riding

				if (!IsTamed)
				{
					Random random = new Random();
					if (random.Next(100) < Temper || player.GameMode == GameMode.Creative)
					{
						// Tamed
						Temper = 100;
						IsTamed = true;
						BroadcastSetEntityData();

						McpeEntityEvent entityEvent = McpeEntityEvent.CreateObject();
						entityEvent.runtimeEntityId = EntityId;
						entityEvent.eventId = 7;
						entityEvent.data = 0;
						Level.RelayBroadcast(entityEvent);
					}
					else
					{
						Temper += 5;
					}
				}

				Mount(player);
			}
		}

		public void SaddleHorse(bool saddle)
		{
			IsSaddled = saddle;
			IsWasdControlled = saddle;
			CanPowerJump = saddle;

			BroadcastSetEntityData();
		}

		public override void Mount(Entity rider)
		{
			if (rider is Player player)
			{
				Rider = player;
				IsRidden = true;

				player.Vehicle = EntityId;

				McpeSetEntityLink link = McpeSetEntityLink.CreateObject();
				link.linkType = (byte) McpeSetEntityLink.LinkActions.Ride;
				link.riderId = player.EntityId;
				link.riddenId = EntityId;
				Level.RelayBroadcast(link);

				SendSetEntityData(player);
			}
		}

		public override void Unmount(Entity rider)
		{
			if (rider is Player player)
			{
				// Unmount ridden entity
				IsRiding = false;

				McpeSetEntityLink link = McpeSetEntityLink.CreateObject();
				link.linkType = (byte) McpeSetEntityLink.LinkActions.Remove;
				link.riderId = player.EntityId;
				link.riddenId = EntityId;
				Level.RelayBroadcast(link);

				IsRidden = false;
				IsRearing = false;
				BroadcastSetEntityData();

				player.Vehicle = 0;
				Rider = null;

				player.BroadcastSetEntityData();
			}
		}

		public void SendSetEntityData(Player player)
		{
			player.IsRiding = true;
			player.RiderSeatPosition = new Vector3(0, 2.32001f, -0.2f);
			player.RiderRotationLocked = false;
			player.RiderMaxRotation = 181;
			player.RiderMinRotation = 0;
			player.BroadcastSetEntityData();
		}
	}

	public class HorseRiddenBehavior : BehaviorBase
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(HorseRiddenBehavior));

		private readonly Horse _horse;
		private long _rideTime = 0;

		public HorseRiddenBehavior(Horse horse)
		{
			_horse = horse;
		}

		public override bool ShouldStart()
		{
			return _horse.IsRidden;
		}

		public override void OnTick(Entity[] entities)
		{
			if (!_horse.IsTamed)
			{
				//Log.Debug($"Riding untamed horse {_rideTime}");

				if (_rideTime > 100 && !_horse.IsRearing)
				{
					_horse.IsRearing = true;
					_horse.BroadcastSetEntityData();
				}
				else if (_rideTime > 120)
				{
					McpeEntityEvent entityEvent = McpeEntityEvent.CreateObject();
					entityEvent.runtimeEntityId = _horse.EntityId;
					entityEvent.eventId = 6;
					entityEvent.data = 0;
					_horse.Level.RelayBroadcast(entityEvent);
					_horse.Unmount(_horse.Rider);
				}

				_rideTime++;
			}
			else
			{
				if (_horse.IsRearing && _horse.IsOnGround)
				{
					_horse.IsRearing = false;
					_horse.BroadcastSetEntityData();
				}
			}
		}

		public override void OnEnd()
		{
			_rideTime = 0;

			if (_horse.IsRearing)
			{
				_horse.IsRearing = false;
				_horse.BroadcastSetEntityData();
			}
		}
	}

	public class HorseInventory : IInventory
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(HorseInventory));

		private readonly Horse _horse;

		public Item Slot0 { get; private set; } = new ItemAir();
		public Item Slot1 { get; private set; } = new ItemAir();

		public HorseInventory(Horse horse)
		{
			_horse = horse;
		}

		public void Open(Player player)
		{
			if (!_horse.IsTamed) return;

			player.SetOpenInventory(this);

			McpeUpdateEquipment equ = McpeUpdateEquipment.CreateObject();
			equ.entityId = _horse.EntityId;
			equ.windowId = 2;
			equ.windowType = 12;

			Nbt nbt = new Nbt
			{
				NbtFile = new NbtFile
				{
					BigEndian = false,
					UseVarInt = true,
					RootTag = GetNbt()
				}
			};
			equ.namedtag = nbt;

			player.SendPacket(equ);

			McpeInventoryContent containerSetContent = McpeInventoryContent.CreateObject();
			containerSetContent.inventoryId = 2;
			containerSetContent.input = new ItemStacks()
			{
				Slot0,
				Slot1
			};
			player.SendPacket(containerSetContent);
		}

		public Item GetSlot(int slot)
		{
			Item item = null;
			if (slot == 0)
				item = Slot0;
			else if (slot == 1)
				item = Slot1;

			return item ?? new ItemAir();
		}

		public void SetSlot(int slot, Item item)
		{
			if (item == null) item = new ItemAir();

			if (slot == 0)
			{
				Slot0 = item;

				_horse.SaddleHorse(item is ItemSaddle);
			}
			else if (slot == 1)
			{
				Slot1 = item;
				_horse.Chest = item;
				_horse.BroadcastArmor();
			}
		}

		public NbtCompound GetNbt()
		{
			NbtCompound root = new NbtCompound("");
			root.Add(
				new NbtList("slots")
				{
					new NbtCompound()
					{
						new NbtList("acceptedItems")
						{
							new NbtCompound()
							{
								new NbtCompound("slotItem")
								{
									new NbtByte("Count", 1),
									new NbtShort("Damage", 0),
									new NbtShort("id", 329),
								},
							}
						},
						new NbtCompound("item")
						{
							new NbtByte("Count", Slot0.Count),
							new NbtShort("Damage", Slot0.Metadata),
							new NbtShort("id", Slot0.Id),
						},
						new NbtInt("slotNumber", 0)
					},
					new NbtCompound()
					{
						new NbtList("acceptedItems")
						{
							new NbtCompound()
							{
								new NbtCompound("slotItem")
								{
									new NbtByte("Count", 1),
									new NbtShort("Damage", 0),
									new NbtShort("id", 416),
								},
							},
							new NbtCompound()
							{
								new NbtCompound("slotItem")
								{
									new NbtByte("Count", 1),
									new NbtShort("Damage", 0),
									new NbtShort("id", 417),
								},
							},
							new NbtCompound()
							{
								new NbtCompound("slotItem")
								{
									new NbtByte("Count", 1),
									new NbtShort("Damage", 0),
									new NbtShort("id", 418),
								},
							},
							new NbtCompound()
							{
								new NbtCompound("slotItem")
								{
									new NbtByte("Count", 1),
									new NbtShort("Damage", 0),
									new NbtShort("id", 419),
								},
							},
						},
						new NbtCompound("item")
						{
							new NbtByte("Count", Slot1.Count),
							new NbtShort("Damage", Slot1.Metadata),
							new NbtShort("id", Slot1.Id),
						},
						new NbtInt("slotNumber", 1)
					},
				});

			return root;
		}
	}
}