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
// The Original Code is Niclas Olofsson.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2017 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System;
using System.Numerics;
using MiNET.Items;
using MiNET.Particles;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	/// <summary>
	///     Blocks are the basic units of structure in Minecraft. Together, they build up the in-game environment and can be
	///     mined and utilized in various fashions.
	/// </summary>
	public class Block : ICloneable
	{
		public BlockCoordinates Coordinates { get; set; }
		public byte Id { get; protected set; }
		public byte Metadata { get; set; }

		public float Hardness { get; protected set; }
		public float BlastResistance { get; protected set; }
		public short FuelEfficiency { get; protected set; }
		public float FrictionFactor { get; protected set; }
		public int LightLevel { get; set; }

		public bool IsReplacible { get; protected set; }
		public bool IsSolid { get; protected set; }
		public bool IsBuildable { get; protected set; }
		public bool IsTransparent { get; protected set; }
		public bool IsFlammable { get; protected set; }
		public bool IsBlockingSkylight { get; protected set; } = true;

		public byte BlockLight { get; set; }
		public byte SkyLight { get; set; }

		public byte BiomeId { get; set; }

		public Block(byte id)
		{
			Id = id;

			IsSolid = true;
			IsBuildable = true;
			IsReplacible = false;
			IsTransparent = false;
			IsFlammable = false;

			Hardness = 0;
			BlastResistance = 0;
			FuelEfficiency = 0;
			FrictionFactor = 0.6f;
			LightLevel = 0;
		}

		public bool CanPlace(Level world, BlockCoordinates targetCoordinates, BlockFace face)
		{
			return CanPlace(world, Coordinates, targetCoordinates, face);
		}

		protected virtual bool CanPlace(Level world, BlockCoordinates blockCoordinates, BlockCoordinates targetCoordinates, BlockFace face)
		{
			return world.GetBlock(blockCoordinates).IsReplacible;
		}

		public virtual void BreakBlock(Level world, bool silent = false)
		{
			world.SetAir(Coordinates);

			if (!silent)
			{
				DestroyBlockParticle particle = new DestroyBlockParticle(world, this);
				particle.Spawn();
			}

			UpdateBlocks(world);
		}

		protected void UpdateBlocks(Level world)
		{
			world.GetBlock(Coordinates + BlockCoordinates.Up).BlockUpdate(world, Coordinates);
			world.GetBlock(Coordinates + BlockCoordinates.Down).BlockUpdate(world, Coordinates);
			world.GetBlock(Coordinates + BlockCoordinates.West).BlockUpdate(world, Coordinates);
			world.GetBlock(Coordinates + BlockCoordinates.East).BlockUpdate(world, Coordinates);
			world.GetBlock(Coordinates + BlockCoordinates.South).BlockUpdate(world, Coordinates);
			world.GetBlock(Coordinates + BlockCoordinates.North).BlockUpdate(world, Coordinates);
		}

		public virtual bool PlaceBlock(Level world, Player player, BlockCoordinates targetCoordinates, BlockFace face, Vector3 faceCoords)
		{
			// No default placement. Return unhandled.
			return false;
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
			return Hardness/5.0F;
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
				case BlockFace.East:
					return target + Level.East;
				case BlockFace.West:
					return target + Level.West;
				case BlockFace.North:
					return target + Level.North;
				case BlockFace.South:
					return target + Level.South;
				default:
					return target;
			}
		}

		public virtual Item[] GetDrops(Item tool)
		{
			return new Item[] {new ItemBlock(this, Metadata) {Count = 1}};
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
			return $"Id: {Id}, Metadata: {Metadata}, Coordinates: {Coordinates}";
		}
	}
}