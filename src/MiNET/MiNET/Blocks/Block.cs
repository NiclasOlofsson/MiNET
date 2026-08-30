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
using System.Linq;
using System.Numerics;
using log4net;
using MiNET.Items;
using MiNET.Particles;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class Block : ICloneable
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Block));

		private BlockDefinition _definition;

		public bool IsGenerated { get; protected set; } = false;

		public BlockCoordinates Coordinates { get; set; }

		public virtual string Name { get; protected set; }

		[Obsolete("Use block states instead.")]
		public byte Metadata { get; set; }

		// Values below are supplied per block by PartialBlocks.cs, generated from CloudburstMC
		// block_properties.json. They are answers about a block, never state the server writes, so
		// they are get-only: a block that needs a different value overrides the property, and a
		// value that depends on the block's own state (a candle's light) is generated as a switch
		// over that state rather than flattened to whichever state came first in the palette.
		public virtual float Hardness => 0;
		public virtual float BlastResistance => 0;
		public short FuelEfficiency { get; protected set; } = 0;
		public virtual float FrictionFactor => 0.6f;
		public virtual int LightLevel => 0;

		/// <summary>How much light this block removes as it passes through, 0 to 15.</summary>
		public virtual int LightDampening => 15;

		/// <summary>0 opaque through 1 fully see-through. Not the same question as IsSolid.</summary>
		public virtual float Translucency => 0f;

		/// <summary>Chance fire consumes this block, and chance fire spreads from it. 0 means neither.</summary>
		public virtual int BurnOdds => 0;
		public virtual int FlameOdds => 0;

		/// <summary>Whether a wrong tool still drops the block, or just breaks it.</summary>
		public virtual bool RequiresCorrectToolForDrops => false;

		/// <summary>
		///     What StartGame sends the client for a data-driven block. Null means the client already
		///     has the block built in and is told nothing about it.
		/// </summary>
		public virtual BlockDefinition CreateDefinition() => null;

		/// <summary>The definition, built once per block.</summary>
		public BlockDefinition Definition => _definition ??= CreateDefinition();

		/// <summary>Whether water can occupy the same space, which is what waterlogging means.</summary>
		public virtual bool CanContainLiquidSource => false;

		public bool IsReplaceable { get; protected set; } = false;
		public virtual bool IsSolid => true;
		public bool IsBuildable { get; protected set; } = true;

		// Derived, not stored. These used to be set by hand on each block, which is why most blocks
		// had them wrong or unset; they are now the obvious reading of the real values above.
		public bool IsTransparent => Translucency > 0f;
		public bool IsFlammable => BurnOdds > 0 || FlameOdds > 0;
		public bool IsBlockingSkylight => LightDampening >= 15;

		/// <summary>
		///     Negative hardness is how vanilla says "cannot be broken", for bedrock, barriers, the
		///     light blocks and 27 others. It is a sentinel, not a small number: multiply it into a
		///     break time and you get a negative one, which reads as instant.
		/// </summary>
		public bool IsUnbreakable => Hardness < 0;

		public byte BlockLight { get; set; }
		public byte SkyLight { get; set; }

		public byte BiomeId { get; set; }

		/// <summary>
		///     A block is its name. Every block in the palette gets a generated partial that overrides
		///     <see cref="Name" /> with its own identity, so nothing is passed in here; this exists for
		///     the handful of classes that are not in the palette and set Name themselves.
		/// </summary>
		public Block()
		{
		}

		// State flows as the states LIST: typed (generated) classes map it onto their state
		// properties in SetState(List) and rebuild it in GetState(). The palette container is
		// looked up from the current state, never carried. A block type without a generated
		// state mapping fails loudly here - the fix is running the generator, not a fallback.
		public virtual void SetState(BlockStateContainer blockstate)
		{
			SetState(blockstate.States);
		}

		public virtual void SetState(List<IBlockState> states)
		{
		}

		public virtual BlockStateContainer GetState()
		{
			Log.Warn($"Block {GetType().Name} ({Name}) has no generated state mapping (GetState not overridden)");
			return null;
		}

		public virtual BlockStateContainer GetGlobalState()
		{
			BlockStateContainer currentState = GetState();
			if (currentState == null || !BlockFactory.BlockStates.TryGetValue(currentState, out var blockstate))
			{
				Log.Warn($"Did not find block state for {this}, {currentState}");
				return null;
			}

			return blockstate;
		}

		/// <summary>
		///     Overridden by the generated partial with closed-form arithmetic, because a block's
		///     position in the palette is a mixed-radix encoding of its state values and is known
		///     when the code is generated. This lookup is the fallback for classes that write their
		///     own GetState by hand.
		/// </summary>
		public virtual int GetRuntimeId()
		{
			BlockStateContainer currentState = GetState();
			if (currentState == null || !BlockFactory.BlockStates.TryGetValue(currentState, out var blockstate))
			{
				Log.Warn($"Did not find block state for {this}, {currentState}");
				return -1;
			}

			return blockstate.RuntimeId;
		}

		/// <summary>
		///     The item this block is picked up as. Post-flattening a block and its item share one
		///     registry name, so this is a lookup, not a search. It used to be a hundred lines of
		///     hunting the palette for a state whose legacy (id, meta) pair carried an ItemInstance,
		///     which is what the pre-flattening world needed.
		///     A block whose item is a different identity (redstone wire dropping redstone dust)
		///     overrides this or GetDrops.
		/// </summary>
		public virtual Item GetItem()
		{
			return ItemFactory.GetItemByName(Name);
		}

		public bool CanPlace(Level world, Player player, BlockCoordinates targetCoordinates, BlockFace face)
		{
			return CanPlace(world, player, Coordinates, targetCoordinates, face);
		}

		protected virtual bool CanPlace(Level world, Player player, BlockCoordinates blockCoordinates, BlockCoordinates targetCoordinates, BlockFace face)
		{
			var playerBbox = (player.GetBoundingBox() - 0.01f);
			var blockBbox = GetBoundingBox();
			if (playerBbox.Intersects(blockBbox))
			{
				Log.Debug($"Player bbox={playerBbox}, block bbox={blockBbox}, intersects={playerBbox.Intersects(blockBbox)}");
				Log.Debug($"Can't build where you are standing");
				return false;
			}

			return world.GetBlock(blockCoordinates).IsReplaceable;
		}

		public virtual void BreakBlock(Level world, BlockFace face, bool silent = false)
		{
			world.SetAir(Coordinates);

			if (!silent)
			{
				var particle = new DestroyBlockParticle(world, this);
				particle.Spawn();
			}

			UpdateBlocks(world);
			world.BroadcastSound(Coordinates, LevelSoundEventType.BreakBlock, GetRuntimeId());
		}

		protected void UpdateBlocks(Level world)
		{
			world.GetBlock(Coordinates.BlockUp()).BlockUpdate(world, Coordinates);
			world.GetBlock(Coordinates.BlockDown()).BlockUpdate(world, Coordinates);
			world.GetBlock(Coordinates.BlockWest()).BlockUpdate(world, Coordinates);
			world.GetBlock(Coordinates.BlockEast()).BlockUpdate(world, Coordinates);
			world.GetBlock(Coordinates.BlockSouth()).BlockUpdate(world, Coordinates);
			world.GetBlock(Coordinates.BlockNorth()).BlockUpdate(world, Coordinates);
		}

		public virtual bool PlaceBlock(Level world, Player player, BlockCoordinates targetCoordinates, BlockFace face, Vector3 faceCoords)
		{
			// No default placement. Return unhandled.
			return false;
		}

		public virtual void BlockAdded(Level level)
		{
		}

		public virtual bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
		{
			// No default interaction. Return unhandled.
			return false;
		}

		public virtual void OnTick(Level level, bool isRandom)
		{
		}

		public virtual void BlockUpdate(Level level, BlockCoordinates blockCoordinates)
		{
		}

		public float GetHardness()
		{
			return Hardness / 5.0F;
		}

		//public double GetMineTime(Item miningTool)
		//{
		//	int multiplier = (int) miningTool.ItemMaterial;
		//	return Hardness*(1.5*multiplier);
		//}

		protected BlockCoordinates GetNewCoordinatesFromFace(BlockCoordinates target, BlockFace face)
		{
			switch (face)
			{
				case BlockFace.Down:
					return target + Level.Down;
				case BlockFace.Up:
					return target + Level.Up;
				case BlockFace.North:
					return target + Level.North;
				case BlockFace.South:
					return target + Level.South;
				case BlockFace.West:
					return target + Level.West;
				case BlockFace.East:
					return target + Level.East;
				default:
					return target;
			}
		}

		public virtual Item[] GetDrops(Item tool)
		{
			var item = GetItem();
			if (item == null) return new Item[0];

			item.Count = 1;

			return new[] {item};
		}

		public virtual Item GetSmelt()
		{
			return null;
		}

		public virtual float GetExperiencePoints()
		{
			return 0;
		}

		public virtual void DoPhysics(Level level)
		{
		}

		public virtual BoundingBox GetBoundingBox()
		{
			return new BoundingBox(Coordinates, Coordinates + 1);
		}


		public object Clone()
		{
			return MemberwiseClone();
		}

		public override string ToString()
		{
			return $"Name: {Name}, State: {GetState()}, Coordinates: {Coordinates}";
		}
	}

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
	public class StateAttribute : Attribute
	{
	}

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
	public class StateBitAttribute : StateAttribute
	{
	}


	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
	public class StateRangeAttribute : StateAttribute
	{
		public int Minimum { get; }
		public int Maximum { get; }

		public StateRangeAttribute(int minimum, int maximum)
		{
			Minimum = minimum;
			Maximum = maximum;
		}
	}

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
	public class StateEnumAttribute : StateAttribute
	{
		public StateEnumAttribute(params string[] validValues)
		{
		}
	}
}