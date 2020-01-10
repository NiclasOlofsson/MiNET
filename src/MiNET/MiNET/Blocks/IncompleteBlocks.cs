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
using System.ComponentModel.DataAnnotations;

namespace MiNET.Blocks
{
	public partial class AcaciaButton : Block // 0 typeof=Block
	{
		[Range(0, 1)] public byte ButtonPressedBit { get; set; }
		[Range(0, 5)] public int FacingDirection { get; set; }

		public AcaciaButton() : base(395)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					ButtonPressedBit = 0;
					FacingDirection = 5;
					break;
				case 1:
					ButtonPressedBit = 0;
					FacingDirection = 2;
					break;
				case 2:
					ButtonPressedBit = 1;
					FacingDirection = 5;
					break;
				case 3:
					ButtonPressedBit = 0;
					FacingDirection = 4;
					break;
				case 4:
					ButtonPressedBit = 0;
					FacingDirection = 1;
					break;
				case 5:
					ButtonPressedBit = 1;
					FacingDirection = 0;
					break;
				case 6:
					ButtonPressedBit = 1;
					FacingDirection = 1;
					break;
				case 7:
					ButtonPressedBit = 0;
					FacingDirection = 3;
					break;
				case 8:
					ButtonPressedBit = 1;
					FacingDirection = 3;
					break;
				case 9:
					ButtonPressedBit = 1;
					FacingDirection = 2;
					break;
				case 10:
					ButtonPressedBit = 0;
					FacingDirection = 0;
					break;
				case 11:
					ButtonPressedBit = 1;
					FacingDirection = 4;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.ButtonPressedBit == 0 && b.FacingDirection == 5:
					return 0;
				case { } b when true && b.ButtonPressedBit == 0 && b.FacingDirection == 2:
					return 1;
				case { } b when true && b.ButtonPressedBit == 1 && b.FacingDirection == 5:
					return 2;
				case { } b when true && b.ButtonPressedBit == 0 && b.FacingDirection == 4:
					return 3;
				case { } b when true && b.ButtonPressedBit == 0 && b.FacingDirection == 1:
					return 4;
				case { } b when true && b.ButtonPressedBit == 1 && b.FacingDirection == 0:
					return 5;
				case { } b when true && b.ButtonPressedBit == 1 && b.FacingDirection == 1:
					return 6;
				case { } b when true && b.ButtonPressedBit == 0 && b.FacingDirection == 3:
					return 7;
				case { } b when true && b.ButtonPressedBit == 1 && b.FacingDirection == 3:
					return 8;
				case { } b when true && b.ButtonPressedBit == 1 && b.FacingDirection == 2:
					return 9;
				case { } b when true && b.ButtonPressedBit == 0 && b.FacingDirection == 0:
					return 10;
				case { } b when true && b.ButtonPressedBit == 1 && b.FacingDirection == 4:
					return 11;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class AcaciaDoor // 196 typeof=AcaciaDoor
	{
		[Range(0, 3)] public int Direction { get; set; }
		[Range(0, 1)] public byte DoorHingeBit { get; set; }
		[Range(0, 1)] public byte OpenBit { get; set; }
		[Range(0, 1)] public byte UpperBlockBit { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Direction = 1;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 1:
					Direction = 2;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 2:
					Direction = 3;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 3:
					Direction = 3;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 4:
					Direction = 3;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 5:
					Direction = 0;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 6:
					Direction = 0;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 7:
					Direction = 0;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 8:
					Direction = 1;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 9:
					Direction = 1;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 10:
					Direction = 0;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 11:
					Direction = 3;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 12:
					Direction = 0;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 13:
					Direction = 1;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 14:
					Direction = 2;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 15:
					Direction = 2;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 16:
					Direction = 1;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 17:
					Direction = 2;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 18:
					Direction = 3;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 19:
					Direction = 2;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 20:
					Direction = 3;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 21:
					Direction = 1;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 22:
					Direction = 0;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 23:
					Direction = 2;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 24:
					Direction = 2;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 25:
					Direction = 1;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 26:
					Direction = 2;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 27:
					Direction = 1;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 28:
					Direction = 0;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 29:
					Direction = 3;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 30:
					Direction = 3;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 31:
					Direction = 0;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 0;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 1;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 2;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 3;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 4;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 5;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 6;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 7;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 8;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 9;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 10;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 11;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 12;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 13;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 14;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 15;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 16;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 17;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 18;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 19;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 20;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 21;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 22;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 23;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 24;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 25;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 26;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 27;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 28;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 29;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 30;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 31;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class AcaciaFenceGate // 187 typeof=AcaciaFenceGate
	{
		[Range(0, 3)] public int Direction { get; set; }
		[Range(0, 1)] public byte InWallBit { get; set; }
		[Range(0, 1)] public byte OpenBit { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Direction = 2;
					InWallBit = 0;
					OpenBit = 1;
					break;
				case 1:
					Direction = 3;
					InWallBit = 0;
					OpenBit = 0;
					break;
				case 2:
					Direction = 1;
					InWallBit = 1;
					OpenBit = 1;
					break;
				case 3:
					Direction = 0;
					InWallBit = 1;
					OpenBit = 1;
					break;
				case 4:
					Direction = 3;
					InWallBit = 1;
					OpenBit = 1;
					break;
				case 5:
					Direction = 1;
					InWallBit = 0;
					OpenBit = 0;
					break;
				case 6:
					Direction = 1;
					InWallBit = 1;
					OpenBit = 0;
					break;
				case 7:
					Direction = 0;
					InWallBit = 1;
					OpenBit = 0;
					break;
				case 8:
					Direction = 0;
					InWallBit = 0;
					OpenBit = 1;
					break;
				case 9:
					Direction = 3;
					InWallBit = 0;
					OpenBit = 1;
					break;
				case 10:
					Direction = 1;
					InWallBit = 0;
					OpenBit = 1;
					break;
				case 11:
					Direction = 2;
					InWallBit = 1;
					OpenBit = 1;
					break;
				case 12:
					Direction = 0;
					InWallBit = 0;
					OpenBit = 0;
					break;
				case 13:
					Direction = 3;
					InWallBit = 1;
					OpenBit = 0;
					break;
				case 14:
					Direction = 2;
					InWallBit = 1;
					OpenBit = 0;
					break;
				case 15:
					Direction = 2;
					InWallBit = 0;
					OpenBit = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Direction == 2 && b.InWallBit == 0 && b.OpenBit == 1:
					return 0;
				case { } b when true && b.Direction == 3 && b.InWallBit == 0 && b.OpenBit == 0:
					return 1;
				case { } b when true && b.Direction == 1 && b.InWallBit == 1 && b.OpenBit == 1:
					return 2;
				case { } b when true && b.Direction == 0 && b.InWallBit == 1 && b.OpenBit == 1:
					return 3;
				case { } b when true && b.Direction == 3 && b.InWallBit == 1 && b.OpenBit == 1:
					return 4;
				case { } b when true && b.Direction == 1 && b.InWallBit == 0 && b.OpenBit == 0:
					return 5;
				case { } b when true && b.Direction == 1 && b.InWallBit == 1 && b.OpenBit == 0:
					return 6;
				case { } b when true && b.Direction == 0 && b.InWallBit == 1 && b.OpenBit == 0:
					return 7;
				case { } b when true && b.Direction == 0 && b.InWallBit == 0 && b.OpenBit == 1:
					return 8;
				case { } b when true && b.Direction == 3 && b.InWallBit == 0 && b.OpenBit == 1:
					return 9;
				case { } b when true && b.Direction == 1 && b.InWallBit == 0 && b.OpenBit == 1:
					return 10;
				case { } b when true && b.Direction == 2 && b.InWallBit == 1 && b.OpenBit == 1:
					return 11;
				case { } b when true && b.Direction == 0 && b.InWallBit == 0 && b.OpenBit == 0:
					return 12;
				case { } b when true && b.Direction == 3 && b.InWallBit == 1 && b.OpenBit == 0:
					return 13;
				case { } b when true && b.Direction == 2 && b.InWallBit == 1 && b.OpenBit == 0:
					return 14;
				case { } b when true && b.Direction == 2 && b.InWallBit == 0 && b.OpenBit == 0:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class AcaciaPressurePlate : Block // 0 typeof=Block
	{
		[Range(0, 9)] public int RedstoneSignal { get; set; }

		public AcaciaPressurePlate() : base(405)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					RedstoneSignal = 3;
					break;
				case 1:
					RedstoneSignal = 7;
					break;
				case 2:
					RedstoneSignal = 1;
					break;
				case 3:
					RedstoneSignal = 0;
					break;
				case 4:
					RedstoneSignal = 8;
					break;
				case 5:
					RedstoneSignal = 10;
					break;
				case 6:
					RedstoneSignal = 2;
					break;
				case 7:
					RedstoneSignal = 13;
					break;
				case 8:
					RedstoneSignal = 6;
					break;
				case 9:
					RedstoneSignal = 9;
					break;
				case 10:
					RedstoneSignal = 15;
					break;
				case 11:
					RedstoneSignal = 11;
					break;
				case 12:
					RedstoneSignal = 12;
					break;
				case 13:
					RedstoneSignal = 5;
					break;
				case 14:
					RedstoneSignal = 4;
					break;
				case 15:
					RedstoneSignal = 14;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.RedstoneSignal == 3:
					return 0;
				case { } b when true && b.RedstoneSignal == 7:
					return 1;
				case { } b when true && b.RedstoneSignal == 1:
					return 2;
				case { } b when true && b.RedstoneSignal == 0:
					return 3;
				case { } b when true && b.RedstoneSignal == 8:
					return 4;
				case { } b when true && b.RedstoneSignal == 10:
					return 5;
				case { } b when true && b.RedstoneSignal == 2:
					return 6;
				case { } b when true && b.RedstoneSignal == 13:
					return 7;
				case { } b when true && b.RedstoneSignal == 6:
					return 8;
				case { } b when true && b.RedstoneSignal == 9:
					return 9;
				case { } b when true && b.RedstoneSignal == 15:
					return 10;
				case { } b when true && b.RedstoneSignal == 11:
					return 11;
				case { } b when true && b.RedstoneSignal == 12:
					return 12;
				case { } b when true && b.RedstoneSignal == 5:
					return 13;
				case { } b when true && b.RedstoneSignal == 4:
					return 14;
				case { } b when true && b.RedstoneSignal == 14:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class AcaciaStairs // 163 typeof=AcaciaStairs
	{
		[Range(0, 1)] public byte UpsideDownBit { get; set; }
		[Range(0, 3)] public int WeirdoDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					UpsideDownBit = 1;
					WeirdoDirection = 2;
					break;
				case 1:
					UpsideDownBit = 0;
					WeirdoDirection = 0;
					break;
				case 2:
					UpsideDownBit = 0;
					WeirdoDirection = 2;
					break;
				case 3:
					UpsideDownBit = 0;
					WeirdoDirection = 3;
					break;
				case 4:
					UpsideDownBit = 1;
					WeirdoDirection = 3;
					break;
				case 5:
					UpsideDownBit = 1;
					WeirdoDirection = 0;
					break;
				case 6:
					UpsideDownBit = 0;
					WeirdoDirection = 1;
					break;
				case 7:
					UpsideDownBit = 1;
					WeirdoDirection = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 2:
					return 0;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 0:
					return 1;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 2:
					return 2;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 3:
					return 3;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 3:
					return 4;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 0:
					return 5;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 1:
					return 6;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 1:
					return 7;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class AcaciaStandingSign : Block // 0 typeof=Block
	{
		[Range(0, 9)] public int GroundSignDirection { get; set; }

		public AcaciaStandingSign() : base(445)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					GroundSignDirection = 10;
					break;
				case 1:
					GroundSignDirection = 5;
					break;
				case 2:
					GroundSignDirection = 7;
					break;
				case 3:
					GroundSignDirection = 8;
					break;
				case 4:
					GroundSignDirection = 1;
					break;
				case 5:
					GroundSignDirection = 11;
					break;
				case 6:
					GroundSignDirection = 4;
					break;
				case 7:
					GroundSignDirection = 15;
					break;
				case 8:
					GroundSignDirection = 13;
					break;
				case 9:
					GroundSignDirection = 12;
					break;
				case 10:
					GroundSignDirection = 3;
					break;
				case 11:
					GroundSignDirection = 2;
					break;
				case 12:
					GroundSignDirection = 9;
					break;
				case 13:
					GroundSignDirection = 6;
					break;
				case 14:
					GroundSignDirection = 0;
					break;
				case 15:
					GroundSignDirection = 14;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.GroundSignDirection == 10:
					return 0;
				case { } b when true && b.GroundSignDirection == 5:
					return 1;
				case { } b when true && b.GroundSignDirection == 7:
					return 2;
				case { } b when true && b.GroundSignDirection == 8:
					return 3;
				case { } b when true && b.GroundSignDirection == 1:
					return 4;
				case { } b when true && b.GroundSignDirection == 11:
					return 5;
				case { } b when true && b.GroundSignDirection == 4:
					return 6;
				case { } b when true && b.GroundSignDirection == 15:
					return 7;
				case { } b when true && b.GroundSignDirection == 13:
					return 8;
				case { } b when true && b.GroundSignDirection == 12:
					return 9;
				case { } b when true && b.GroundSignDirection == 3:
					return 10;
				case { } b when true && b.GroundSignDirection == 2:
					return 11;
				case { } b when true && b.GroundSignDirection == 9:
					return 12;
				case { } b when true && b.GroundSignDirection == 6:
					return 13;
				case { } b when true && b.GroundSignDirection == 0:
					return 14;
				case { } b when true && b.GroundSignDirection == 14:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class AcaciaTrapdoor : Block // 0 typeof=Block
	{
		[Range(0, 3)] public int Direction { get; set; }
		[Range(0, 1)] public byte OpenBit { get; set; }
		[Range(0, 1)] public byte UpsideDownBit { get; set; }

		public AcaciaTrapdoor() : base(400)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Direction = 1;
					OpenBit = 1;
					UpsideDownBit = 1;
					break;
				case 1:
					Direction = 1;
					OpenBit = 1;
					UpsideDownBit = 0;
					break;
				case 2:
					Direction = 3;
					OpenBit = 0;
					UpsideDownBit = 0;
					break;
				case 3:
					Direction = 0;
					OpenBit = 0;
					UpsideDownBit = 1;
					break;
				case 4:
					Direction = 1;
					OpenBit = 0;
					UpsideDownBit = 1;
					break;
				case 5:
					Direction = 3;
					OpenBit = 0;
					UpsideDownBit = 1;
					break;
				case 6:
					Direction = 2;
					OpenBit = 0;
					UpsideDownBit = 1;
					break;
				case 7:
					Direction = 1;
					OpenBit = 0;
					UpsideDownBit = 0;
					break;
				case 8:
					Direction = 3;
					OpenBit = 1;
					UpsideDownBit = 1;
					break;
				case 9:
					Direction = 3;
					OpenBit = 1;
					UpsideDownBit = 0;
					break;
				case 10:
					Direction = 2;
					OpenBit = 1;
					UpsideDownBit = 1;
					break;
				case 11:
					Direction = 0;
					OpenBit = 1;
					UpsideDownBit = 1;
					break;
				case 12:
					Direction = 2;
					OpenBit = 0;
					UpsideDownBit = 0;
					break;
				case 13:
					Direction = 0;
					OpenBit = 1;
					UpsideDownBit = 0;
					break;
				case 14:
					Direction = 2;
					OpenBit = 1;
					UpsideDownBit = 0;
					break;
				case 15:
					Direction = 0;
					OpenBit = 0;
					UpsideDownBit = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Direction == 1 && b.OpenBit == 1 && b.UpsideDownBit == 1:
					return 0;
				case { } b when true && b.Direction == 1 && b.OpenBit == 1 && b.UpsideDownBit == 0:
					return 1;
				case { } b when true && b.Direction == 3 && b.OpenBit == 0 && b.UpsideDownBit == 0:
					return 2;
				case { } b when true && b.Direction == 0 && b.OpenBit == 0 && b.UpsideDownBit == 1:
					return 3;
				case { } b when true && b.Direction == 1 && b.OpenBit == 0 && b.UpsideDownBit == 1:
					return 4;
				case { } b when true && b.Direction == 3 && b.OpenBit == 0 && b.UpsideDownBit == 1:
					return 5;
				case { } b when true && b.Direction == 2 && b.OpenBit == 0 && b.UpsideDownBit == 1:
					return 6;
				case { } b when true && b.Direction == 1 && b.OpenBit == 0 && b.UpsideDownBit == 0:
					return 7;
				case { } b when true && b.Direction == 3 && b.OpenBit == 1 && b.UpsideDownBit == 1:
					return 8;
				case { } b when true && b.Direction == 3 && b.OpenBit == 1 && b.UpsideDownBit == 0:
					return 9;
				case { } b when true && b.Direction == 2 && b.OpenBit == 1 && b.UpsideDownBit == 1:
					return 10;
				case { } b when true && b.Direction == 0 && b.OpenBit == 1 && b.UpsideDownBit == 1:
					return 11;
				case { } b when true && b.Direction == 2 && b.OpenBit == 0 && b.UpsideDownBit == 0:
					return 12;
				case { } b when true && b.Direction == 0 && b.OpenBit == 1 && b.UpsideDownBit == 0:
					return 13;
				case { } b when true && b.Direction == 2 && b.OpenBit == 1 && b.UpsideDownBit == 0:
					return 14;
				case { } b when true && b.Direction == 0 && b.OpenBit == 0 && b.UpsideDownBit == 0:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class AcaciaWallSign : Block // 0 typeof=Block
	{
		[Range(0, 5)] public int FacingDirection { get; set; }

		public AcaciaWallSign() : base(446)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					FacingDirection = 2;
					break;
				case 1:
					FacingDirection = 3;
					break;
				case 2:
					FacingDirection = 4;
					break;
				case 3:
					FacingDirection = 0;
					break;
				case 4:
					FacingDirection = 5;
					break;
				case 5:
					FacingDirection = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.FacingDirection == 2:
					return 0;
				case { } b when true && b.FacingDirection == 3:
					return 1;
				case { } b when true && b.FacingDirection == 4:
					return 2;
				case { } b when true && b.FacingDirection == 0:
					return 3;
				case { } b when true && b.FacingDirection == 5:
					return 4;
				case { } b when true && b.FacingDirection == 1:
					return 5;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class ActivatorRail // 126 typeof=ActivatorRail
	{
		[Range(0, 1)] public byte RailDataBit { get; set; }
		[Range(0, 5)] public int RailDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					RailDataBit = 1;
					RailDirection = 3;
					break;
				case 1:
					RailDataBit = 1;
					RailDirection = 0;
					break;
				case 2:
					RailDataBit = 1;
					RailDirection = 2;
					break;
				case 3:
					RailDataBit = 1;
					RailDirection = 1;
					break;
				case 4:
					RailDataBit = 1;
					RailDirection = 5;
					break;
				case 5:
					RailDataBit = 0;
					RailDirection = 0;
					break;
				case 6:
					RailDataBit = 0;
					RailDirection = 3;
					break;
				case 7:
					RailDataBit = 1;
					RailDirection = 4;
					break;
				case 8:
					RailDataBit = 0;
					RailDirection = 4;
					break;
				case 9:
					RailDataBit = 0;
					RailDirection = 1;
					break;
				case 10:
					RailDataBit = 0;
					RailDirection = 5;
					break;
				case 11:
					RailDataBit = 0;
					RailDirection = 2;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.RailDataBit == 1 && b.RailDirection == 3:
					return 0;
				case { } b when true && b.RailDataBit == 1 && b.RailDirection == 0:
					return 1;
				case { } b when true && b.RailDataBit == 1 && b.RailDirection == 2:
					return 2;
				case { } b when true && b.RailDataBit == 1 && b.RailDirection == 1:
					return 3;
				case { } b when true && b.RailDataBit == 1 && b.RailDirection == 5:
					return 4;
				case { } b when true && b.RailDataBit == 0 && b.RailDirection == 0:
					return 5;
				case { } b when true && b.RailDataBit == 0 && b.RailDirection == 3:
					return 6;
				case { } b when true && b.RailDataBit == 1 && b.RailDirection == 4:
					return 7;
				case { } b when true && b.RailDataBit == 0 && b.RailDirection == 4:
					return 8;
				case { } b when true && b.RailDataBit == 0 && b.RailDirection == 1:
					return 9;
				case { } b when true && b.RailDataBit == 0 && b.RailDirection == 5:
					return 10;
				case { } b when true && b.RailDataBit == 0 && b.RailDirection == 2:
					return 11;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class AndesiteStairs : Block // 0 typeof=Block
	{
		[Range(0, 1)] public byte UpsideDownBit { get; set; }
		[Range(0, 3)] public int WeirdoDirection { get; set; }

		public AndesiteStairs() : base(426)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					UpsideDownBit = 0;
					WeirdoDirection = 2;
					break;
				case 1:
					UpsideDownBit = 1;
					WeirdoDirection = 3;
					break;
				case 2:
					UpsideDownBit = 1;
					WeirdoDirection = 2;
					break;
				case 3:
					UpsideDownBit = 0;
					WeirdoDirection = 3;
					break;
				case 4:
					UpsideDownBit = 0;
					WeirdoDirection = 0;
					break;
				case 5:
					UpsideDownBit = 1;
					WeirdoDirection = 1;
					break;
				case 6:
					UpsideDownBit = 0;
					WeirdoDirection = 1;
					break;
				case 7:
					UpsideDownBit = 1;
					WeirdoDirection = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 2:
					return 0;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 3:
					return 1;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 2:
					return 2;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 3:
					return 3;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 0:
					return 4;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 1:
					return 5;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 1:
					return 6;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 0:
					return 7;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Anvil // 145 typeof=Anvil
	{
		// Convert this attribute to enum
		//[Enum("broken","slightly_damaged","undamaged","very_damaged"]
		public string Damage { get; set; }
		[Range(0, 3)] public int Direction { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Damage = "very_damaged";
					Direction = 0;
					break;
				case 1:
					Damage = "very_damaged";
					Direction = 3;
					break;
				case 2:
					Damage = "undamaged";
					Direction = 1;
					break;
				case 3:
					Damage = "undamaged";
					Direction = 0;
					break;
				case 4:
					Damage = "undamaged";
					Direction = 2;
					break;
				case 5:
					Damage = "slightly_damaged";
					Direction = 0;
					break;
				case 6:
					Damage = "broken";
					Direction = 1;
					break;
				case 7:
					Damage = "very_damaged";
					Direction = 2;
					break;
				case 8:
					Damage = "undamaged";
					Direction = 3;
					break;
				case 9:
					Damage = "broken";
					Direction = 0;
					break;
				case 10:
					Damage = "slightly_damaged";
					Direction = 2;
					break;
				case 11:
					Damage = "very_damaged";
					Direction = 1;
					break;
				case 12:
					Damage = "broken";
					Direction = 2;
					break;
				case 13:
					Damage = "slightly_damaged";
					Direction = 3;
					break;
				case 14:
					Damage = "slightly_damaged";
					Direction = 1;
					break;
				case 15:
					Damage = "broken";
					Direction = 3;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Damage == "very_damaged" && b.Direction == 0:
					return 0;
				case { } b when true && b.Damage == "very_damaged" && b.Direction == 3:
					return 1;
				case { } b when true && b.Damage == "undamaged" && b.Direction == 1:
					return 2;
				case { } b when true && b.Damage == "undamaged" && b.Direction == 0:
					return 3;
				case { } b when true && b.Damage == "undamaged" && b.Direction == 2:
					return 4;
				case { } b when true && b.Damage == "slightly_damaged" && b.Direction == 0:
					return 5;
				case { } b when true && b.Damage == "broken" && b.Direction == 1:
					return 6;
				case { } b when true && b.Damage == "very_damaged" && b.Direction == 2:
					return 7;
				case { } b when true && b.Damage == "undamaged" && b.Direction == 3:
					return 8;
				case { } b when true && b.Damage == "broken" && b.Direction == 0:
					return 9;
				case { } b when true && b.Damage == "slightly_damaged" && b.Direction == 2:
					return 10;
				case { } b when true && b.Damage == "very_damaged" && b.Direction == 1:
					return 11;
				case { } b when true && b.Damage == "broken" && b.Direction == 2:
					return 12;
				case { } b when true && b.Damage == "slightly_damaged" && b.Direction == 3:
					return 13;
				case { } b when true && b.Damage == "slightly_damaged" && b.Direction == 1:
					return 14;
				case { } b when true && b.Damage == "broken" && b.Direction == 3:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Bamboo : Block // 0 typeof=Block
	{
		[Range(0, 1)] public byte AgeBit { get; set; }

		// Convert this attribute to enum
		//[Enum("large_leaves","no_leaves","small_leaves"]
		public string BambooLeafSize { get; set; }

		// Convert this attribute to enum
		//[Enum("thick","thin"]
		public string BambooStalkThickness { get; set; }

		public Bamboo() : base(418)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					AgeBit = 1;
					BambooLeafSize = "large_leaves";
					BambooStalkThickness = "thin";
					break;
				case 1:
					AgeBit = 0;
					BambooLeafSize = "no_leaves";
					BambooStalkThickness = "thin";
					break;
				case 2:
					AgeBit = 0;
					BambooLeafSize = "small_leaves";
					BambooStalkThickness = "thick";
					break;
				case 3:
					AgeBit = 1;
					BambooLeafSize = "no_leaves";
					BambooStalkThickness = "thick";
					break;
				case 4:
					AgeBit = 1;
					BambooLeafSize = "small_leaves";
					BambooStalkThickness = "thick";
					break;
				case 5:
					AgeBit = 1;
					BambooLeafSize = "small_leaves";
					BambooStalkThickness = "thin";
					break;
				case 6:
					AgeBit = 0;
					BambooLeafSize = "small_leaves";
					BambooStalkThickness = "thin";
					break;
				case 7:
					AgeBit = 1;
					BambooLeafSize = "large_leaves";
					BambooStalkThickness = "thick";
					break;
				case 8:
					AgeBit = 1;
					BambooLeafSize = "no_leaves";
					BambooStalkThickness = "thin";
					break;
				case 9:
					AgeBit = 0;
					BambooLeafSize = "large_leaves";
					BambooStalkThickness = "thin";
					break;
				case 10:
					AgeBit = 0;
					BambooLeafSize = "no_leaves";
					BambooStalkThickness = "thick";
					break;
				case 11:
					AgeBit = 0;
					BambooLeafSize = "large_leaves";
					BambooStalkThickness = "thick";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.AgeBit == 1 && b.BambooLeafSize == "large_leaves" && b.BambooStalkThickness == "thin":
					return 0;
				case { } b when true && b.AgeBit == 0 && b.BambooLeafSize == "no_leaves" && b.BambooStalkThickness == "thin":
					return 1;
				case { } b when true && b.AgeBit == 0 && b.BambooLeafSize == "small_leaves" && b.BambooStalkThickness == "thick":
					return 2;
				case { } b when true && b.AgeBit == 1 && b.BambooLeafSize == "no_leaves" && b.BambooStalkThickness == "thick":
					return 3;
				case { } b when true && b.AgeBit == 1 && b.BambooLeafSize == "small_leaves" && b.BambooStalkThickness == "thick":
					return 4;
				case { } b when true && b.AgeBit == 1 && b.BambooLeafSize == "small_leaves" && b.BambooStalkThickness == "thin":
					return 5;
				case { } b when true && b.AgeBit == 0 && b.BambooLeafSize == "small_leaves" && b.BambooStalkThickness == "thin":
					return 6;
				case { } b when true && b.AgeBit == 1 && b.BambooLeafSize == "large_leaves" && b.BambooStalkThickness == "thick":
					return 7;
				case { } b when true && b.AgeBit == 1 && b.BambooLeafSize == "no_leaves" && b.BambooStalkThickness == "thin":
					return 8;
				case { } b when true && b.AgeBit == 0 && b.BambooLeafSize == "large_leaves" && b.BambooStalkThickness == "thin":
					return 9;
				case { } b when true && b.AgeBit == 0 && b.BambooLeafSize == "no_leaves" && b.BambooStalkThickness == "thick":
					return 10;
				case { } b when true && b.AgeBit == 0 && b.BambooLeafSize == "large_leaves" && b.BambooStalkThickness == "thick":
					return 11;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class BambooSapling : Block // 0 typeof=Block
	{
		[Range(0, 1)] public byte AgeBit { get; set; }

		// Convert this attribute to enum
		//[Enum("acacia","birch","dark_oak","jungle","oak","spruce"]
		public string SaplingType { get; set; }

		public BambooSapling() : base(419)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					AgeBit = 0;
					SaplingType = "birch";
					break;
				case 1:
					AgeBit = 0;
					SaplingType = "acacia";
					break;
				case 2:
					AgeBit = 0;
					SaplingType = "spruce";
					break;
				case 3:
					AgeBit = 0;
					SaplingType = "oak";
					break;
				case 4:
					AgeBit = 0;
					SaplingType = "dark_oak";
					break;
				case 5:
					AgeBit = 1;
					SaplingType = "birch";
					break;
				case 6:
					AgeBit = 1;
					SaplingType = "jungle";
					break;
				case 7:
					AgeBit = 1;
					SaplingType = "acacia";
					break;
				case 8:
					AgeBit = 1;
					SaplingType = "spruce";
					break;
				case 9:
					AgeBit = 1;
					SaplingType = "oak";
					break;
				case 10:
					AgeBit = 1;
					SaplingType = "dark_oak";
					break;
				case 11:
					AgeBit = 0;
					SaplingType = "jungle";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.AgeBit == 0 && b.SaplingType == "birch":
					return 0;
				case { } b when true && b.AgeBit == 0 && b.SaplingType == "acacia":
					return 1;
				case { } b when true && b.AgeBit == 0 && b.SaplingType == "spruce":
					return 2;
				case { } b when true && b.AgeBit == 0 && b.SaplingType == "oak":
					return 3;
				case { } b when true && b.AgeBit == 0 && b.SaplingType == "dark_oak":
					return 4;
				case { } b when true && b.AgeBit == 1 && b.SaplingType == "birch":
					return 5;
				case { } b when true && b.AgeBit == 1 && b.SaplingType == "jungle":
					return 6;
				case { } b when true && b.AgeBit == 1 && b.SaplingType == "acacia":
					return 7;
				case { } b when true && b.AgeBit == 1 && b.SaplingType == "spruce":
					return 8;
				case { } b when true && b.AgeBit == 1 && b.SaplingType == "oak":
					return 9;
				case { } b when true && b.AgeBit == 1 && b.SaplingType == "dark_oak":
					return 10;
				case { } b when true && b.AgeBit == 0 && b.SaplingType == "jungle":
					return 11;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Barrel : Block // 0 typeof=Block
	{
		[Range(0, 5)] public int FacingDirection { get; set; }
		[Range(0, 1)] public byte OpenBit { get; set; }

		public Barrel() : base(458)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					FacingDirection = 2;
					OpenBit = 0;
					break;
				case 1:
					FacingDirection = 4;
					OpenBit = 0;
					break;
				case 2:
					FacingDirection = 2;
					OpenBit = 1;
					break;
				case 3:
					FacingDirection = 0;
					OpenBit = 0;
					break;
				case 4:
					FacingDirection = 3;
					OpenBit = 0;
					break;
				case 5:
					FacingDirection = 5;
					OpenBit = 0;
					break;
				case 6:
					FacingDirection = 1;
					OpenBit = 0;
					break;
				case 7:
					FacingDirection = 3;
					OpenBit = 1;
					break;
				case 8:
					FacingDirection = 0;
					OpenBit = 1;
					break;
				case 9:
					FacingDirection = 5;
					OpenBit = 1;
					break;
				case 10:
					FacingDirection = 4;
					OpenBit = 1;
					break;
				case 11:
					FacingDirection = 1;
					OpenBit = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.FacingDirection == 2 && b.OpenBit == 0:
					return 0;
				case { } b when true && b.FacingDirection == 4 && b.OpenBit == 0:
					return 1;
				case { } b when true && b.FacingDirection == 2 && b.OpenBit == 1:
					return 2;
				case { } b when true && b.FacingDirection == 0 && b.OpenBit == 0:
					return 3;
				case { } b when true && b.FacingDirection == 3 && b.OpenBit == 0:
					return 4;
				case { } b when true && b.FacingDirection == 5 && b.OpenBit == 0:
					return 5;
				case { } b when true && b.FacingDirection == 1 && b.OpenBit == 0:
					return 6;
				case { } b when true && b.FacingDirection == 3 && b.OpenBit == 1:
					return 7;
				case { } b when true && b.FacingDirection == 0 && b.OpenBit == 1:
					return 8;
				case { } b when true && b.FacingDirection == 5 && b.OpenBit == 1:
					return 9;
				case { } b when true && b.FacingDirection == 4 && b.OpenBit == 1:
					return 10;
				case { } b when true && b.FacingDirection == 1 && b.OpenBit == 1:
					return 11;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Barrier : Block // 0 typeof=Block
	{
		public Barrier() : base(416)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Beacon // 138 typeof=Beacon
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Bed // 26 typeof=Bed
	{
		[Range(0, 3)] public int Direction { get; set; }
		[Range(0, 1)] public byte HeadPieceBit { get; set; }
		[Range(0, 1)] public byte OccupiedBit { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Direction = 3;
					HeadPieceBit = 1;
					OccupiedBit = 1;
					break;
				case 1:
					Direction = 0;
					HeadPieceBit = 1;
					OccupiedBit = 0;
					break;
				case 2:
					Direction = 1;
					HeadPieceBit = 1;
					OccupiedBit = 1;
					break;
				case 3:
					Direction = 2;
					HeadPieceBit = 0;
					OccupiedBit = 0;
					break;
				case 4:
					Direction = 3;
					HeadPieceBit = 0;
					OccupiedBit = 1;
					break;
				case 5:
					Direction = 2;
					HeadPieceBit = 1;
					OccupiedBit = 0;
					break;
				case 6:
					Direction = 0;
					HeadPieceBit = 1;
					OccupiedBit = 1;
					break;
				case 7:
					Direction = 2;
					HeadPieceBit = 0;
					OccupiedBit = 1;
					break;
				case 8:
					Direction = 1;
					HeadPieceBit = 0;
					OccupiedBit = 0;
					break;
				case 9:
					Direction = 0;
					HeadPieceBit = 0;
					OccupiedBit = 0;
					break;
				case 10:
					Direction = 2;
					HeadPieceBit = 1;
					OccupiedBit = 1;
					break;
				case 11:
					Direction = 3;
					HeadPieceBit = 1;
					OccupiedBit = 0;
					break;
				case 12:
					Direction = 1;
					HeadPieceBit = 0;
					OccupiedBit = 1;
					break;
				case 13:
					Direction = 1;
					HeadPieceBit = 1;
					OccupiedBit = 0;
					break;
				case 14:
					Direction = 0;
					HeadPieceBit = 0;
					OccupiedBit = 1;
					break;
				case 15:
					Direction = 3;
					HeadPieceBit = 0;
					OccupiedBit = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Direction == 3 && b.HeadPieceBit == 1 && b.OccupiedBit == 1:
					return 0;
				case { } b when true && b.Direction == 0 && b.HeadPieceBit == 1 && b.OccupiedBit == 0:
					return 1;
				case { } b when true && b.Direction == 1 && b.HeadPieceBit == 1 && b.OccupiedBit == 1:
					return 2;
				case { } b when true && b.Direction == 2 && b.HeadPieceBit == 0 && b.OccupiedBit == 0:
					return 3;
				case { } b when true && b.Direction == 3 && b.HeadPieceBit == 0 && b.OccupiedBit == 1:
					return 4;
				case { } b when true && b.Direction == 2 && b.HeadPieceBit == 1 && b.OccupiedBit == 0:
					return 5;
				case { } b when true && b.Direction == 0 && b.HeadPieceBit == 1 && b.OccupiedBit == 1:
					return 6;
				case { } b when true && b.Direction == 2 && b.HeadPieceBit == 0 && b.OccupiedBit == 1:
					return 7;
				case { } b when true && b.Direction == 1 && b.HeadPieceBit == 0 && b.OccupiedBit == 0:
					return 8;
				case { } b when true && b.Direction == 0 && b.HeadPieceBit == 0 && b.OccupiedBit == 0:
					return 9;
				case { } b when true && b.Direction == 2 && b.HeadPieceBit == 1 && b.OccupiedBit == 1:
					return 10;
				case { } b when true && b.Direction == 3 && b.HeadPieceBit == 1 && b.OccupiedBit == 0:
					return 11;
				case { } b when true && b.Direction == 1 && b.HeadPieceBit == 0 && b.OccupiedBit == 1:
					return 12;
				case { } b when true && b.Direction == 1 && b.HeadPieceBit == 1 && b.OccupiedBit == 0:
					return 13;
				case { } b when true && b.Direction == 0 && b.HeadPieceBit == 0 && b.OccupiedBit == 1:
					return 14;
				case { } b when true && b.Direction == 3 && b.HeadPieceBit == 0 && b.OccupiedBit == 0:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Bedrock // 7 typeof=Bedrock
	{
		[Range(0, 1)] public byte InfiniburnBit { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					InfiniburnBit = 0;
					break;
				case 1:
					InfiniburnBit = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.InfiniburnBit == 0:
					return 0;
				case { } b when true && b.InfiniburnBit == 1:
					return 1;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class BeeNest : Block // 0 typeof=Block
	{
		[Range(0, 5)] public int FacingDirection { get; set; }
		[Range(0, 5)] public int HoneyLevel { get; set; }

		public BeeNest() : base(473)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					FacingDirection = 1;
					HoneyLevel = 2;
					break;
				case 1:
					FacingDirection = 0;
					HoneyLevel = 4;
					break;
				case 2:
					FacingDirection = 3;
					HoneyLevel = 1;
					break;
				case 3:
					FacingDirection = 1;
					HoneyLevel = 5;
					break;
				case 4:
					FacingDirection = 0;
					HoneyLevel = 3;
					break;
				case 5:
					FacingDirection = 3;
					HoneyLevel = 5;
					break;
				case 6:
					FacingDirection = 4;
					HoneyLevel = 1;
					break;
				case 7:
					FacingDirection = 1;
					HoneyLevel = 3;
					break;
				case 8:
					FacingDirection = 3;
					HoneyLevel = 0;
					break;
				case 9:
					FacingDirection = 4;
					HoneyLevel = 0;
					break;
				case 10:
					FacingDirection = 2;
					HoneyLevel = 0;
					break;
				case 11:
					FacingDirection = 0;
					HoneyLevel = 2;
					break;
				case 12:
					FacingDirection = 0;
					HoneyLevel = 5;
					break;
				case 13:
					FacingDirection = 1;
					HoneyLevel = 1;
					break;
				case 14:
					FacingDirection = 4;
					HoneyLevel = 2;
					break;
				case 15:
					FacingDirection = 1;
					HoneyLevel = 4;
					break;
				case 16:
					FacingDirection = 0;
					HoneyLevel = 0;
					break;
				case 17:
					FacingDirection = 5;
					HoneyLevel = 1;
					break;
				case 18:
					FacingDirection = 3;
					HoneyLevel = 2;
					break;
				case 19:
					FacingDirection = 1;
					HoneyLevel = 0;
					break;
				case 20:
					FacingDirection = 5;
					HoneyLevel = 0;
					break;
				case 21:
					FacingDirection = 2;
					HoneyLevel = 2;
					break;
				case 22:
					FacingDirection = 5;
					HoneyLevel = 3;
					break;
				case 23:
					FacingDirection = 4;
					HoneyLevel = 5;
					break;
				case 24:
					FacingDirection = 5;
					HoneyLevel = 5;
					break;
				case 25:
					FacingDirection = 4;
					HoneyLevel = 3;
					break;
				case 26:
					FacingDirection = 2;
					HoneyLevel = 4;
					break;
				case 27:
					FacingDirection = 2;
					HoneyLevel = 1;
					break;
				case 28:
					FacingDirection = 3;
					HoneyLevel = 4;
					break;
				case 29:
					FacingDirection = 2;
					HoneyLevel = 5;
					break;
				case 30:
					FacingDirection = 5;
					HoneyLevel = 2;
					break;
				case 31:
					FacingDirection = 3;
					HoneyLevel = 3;
					break;
				case 32:
					FacingDirection = 0;
					HoneyLevel = 1;
					break;
				case 33:
					FacingDirection = 4;
					HoneyLevel = 4;
					break;
				case 34:
					FacingDirection = 2;
					HoneyLevel = 3;
					break;
				case 35:
					FacingDirection = 5;
					HoneyLevel = 4;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.FacingDirection == 1 && b.HoneyLevel == 2:
					return 0;
				case { } b when true && b.FacingDirection == 0 && b.HoneyLevel == 4:
					return 1;
				case { } b when true && b.FacingDirection == 3 && b.HoneyLevel == 1:
					return 2;
				case { } b when true && b.FacingDirection == 1 && b.HoneyLevel == 5:
					return 3;
				case { } b when true && b.FacingDirection == 0 && b.HoneyLevel == 3:
					return 4;
				case { } b when true && b.FacingDirection == 3 && b.HoneyLevel == 5:
					return 5;
				case { } b when true && b.FacingDirection == 4 && b.HoneyLevel == 1:
					return 6;
				case { } b when true && b.FacingDirection == 1 && b.HoneyLevel == 3:
					return 7;
				case { } b when true && b.FacingDirection == 3 && b.HoneyLevel == 0:
					return 8;
				case { } b when true && b.FacingDirection == 4 && b.HoneyLevel == 0:
					return 9;
				case { } b when true && b.FacingDirection == 2 && b.HoneyLevel == 0:
					return 10;
				case { } b when true && b.FacingDirection == 0 && b.HoneyLevel == 2:
					return 11;
				case { } b when true && b.FacingDirection == 0 && b.HoneyLevel == 5:
					return 12;
				case { } b when true && b.FacingDirection == 1 && b.HoneyLevel == 1:
					return 13;
				case { } b when true && b.FacingDirection == 4 && b.HoneyLevel == 2:
					return 14;
				case { } b when true && b.FacingDirection == 1 && b.HoneyLevel == 4:
					return 15;
				case { } b when true && b.FacingDirection == 0 && b.HoneyLevel == 0:
					return 16;
				case { } b when true && b.FacingDirection == 5 && b.HoneyLevel == 1:
					return 17;
				case { } b when true && b.FacingDirection == 3 && b.HoneyLevel == 2:
					return 18;
				case { } b when true && b.FacingDirection == 1 && b.HoneyLevel == 0:
					return 19;
				case { } b when true && b.FacingDirection == 5 && b.HoneyLevel == 0:
					return 20;
				case { } b when true && b.FacingDirection == 2 && b.HoneyLevel == 2:
					return 21;
				case { } b when true && b.FacingDirection == 5 && b.HoneyLevel == 3:
					return 22;
				case { } b when true && b.FacingDirection == 4 && b.HoneyLevel == 5:
					return 23;
				case { } b when true && b.FacingDirection == 5 && b.HoneyLevel == 5:
					return 24;
				case { } b when true && b.FacingDirection == 4 && b.HoneyLevel == 3:
					return 25;
				case { } b when true && b.FacingDirection == 2 && b.HoneyLevel == 4:
					return 26;
				case { } b when true && b.FacingDirection == 2 && b.HoneyLevel == 1:
					return 27;
				case { } b when true && b.FacingDirection == 3 && b.HoneyLevel == 4:
					return 28;
				case { } b when true && b.FacingDirection == 2 && b.HoneyLevel == 5:
					return 29;
				case { } b when true && b.FacingDirection == 5 && b.HoneyLevel == 2:
					return 30;
				case { } b when true && b.FacingDirection == 3 && b.HoneyLevel == 3:
					return 31;
				case { } b when true && b.FacingDirection == 0 && b.HoneyLevel == 1:
					return 32;
				case { } b when true && b.FacingDirection == 4 && b.HoneyLevel == 4:
					return 33;
				case { } b when true && b.FacingDirection == 2 && b.HoneyLevel == 3:
					return 34;
				case { } b when true && b.FacingDirection == 5 && b.HoneyLevel == 4:
					return 35;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Beehive : Block // 0 typeof=Block
	{
		[Range(0, 5)] public int FacingDirection { get; set; }
		[Range(0, 5)] public int HoneyLevel { get; set; }

		public Beehive() : base(474)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					FacingDirection = 2;
					HoneyLevel = 1;
					break;
				case 1:
					FacingDirection = 3;
					HoneyLevel = 0;
					break;
				case 2:
					FacingDirection = 1;
					HoneyLevel = 3;
					break;
				case 3:
					FacingDirection = 5;
					HoneyLevel = 1;
					break;
				case 4:
					FacingDirection = 3;
					HoneyLevel = 4;
					break;
				case 5:
					FacingDirection = 4;
					HoneyLevel = 0;
					break;
				case 6:
					FacingDirection = 1;
					HoneyLevel = 4;
					break;
				case 7:
					FacingDirection = 0;
					HoneyLevel = 4;
					break;
				case 8:
					FacingDirection = 5;
					HoneyLevel = 5;
					break;
				case 9:
					FacingDirection = 5;
					HoneyLevel = 3;
					break;
				case 10:
					FacingDirection = 2;
					HoneyLevel = 0;
					break;
				case 11:
					FacingDirection = 4;
					HoneyLevel = 2;
					break;
				case 12:
					FacingDirection = 1;
					HoneyLevel = 0;
					break;
				case 13:
					FacingDirection = 3;
					HoneyLevel = 2;
					break;
				case 14:
					FacingDirection = 3;
					HoneyLevel = 3;
					break;
				case 15:
					FacingDirection = 1;
					HoneyLevel = 1;
					break;
				case 16:
					FacingDirection = 0;
					HoneyLevel = 2;
					break;
				case 17:
					FacingDirection = 5;
					HoneyLevel = 4;
					break;
				case 18:
					FacingDirection = 4;
					HoneyLevel = 3;
					break;
				case 19:
					FacingDirection = 0;
					HoneyLevel = 5;
					break;
				case 20:
					FacingDirection = 2;
					HoneyLevel = 4;
					break;
				case 21:
					FacingDirection = 4;
					HoneyLevel = 4;
					break;
				case 22:
					FacingDirection = 3;
					HoneyLevel = 1;
					break;
				case 23:
					FacingDirection = 1;
					HoneyLevel = 5;
					break;
				case 24:
					FacingDirection = 0;
					HoneyLevel = 1;
					break;
				case 25:
					FacingDirection = 0;
					HoneyLevel = 3;
					break;
				case 26:
					FacingDirection = 1;
					HoneyLevel = 2;
					break;
				case 27:
					FacingDirection = 5;
					HoneyLevel = 2;
					break;
				case 28:
					FacingDirection = 5;
					HoneyLevel = 0;
					break;
				case 29:
					FacingDirection = 2;
					HoneyLevel = 3;
					break;
				case 30:
					FacingDirection = 4;
					HoneyLevel = 5;
					break;
				case 31:
					FacingDirection = 0;
					HoneyLevel = 0;
					break;
				case 32:
					FacingDirection = 2;
					HoneyLevel = 2;
					break;
				case 33:
					FacingDirection = 2;
					HoneyLevel = 5;
					break;
				case 34:
					FacingDirection = 4;
					HoneyLevel = 1;
					break;
				case 35:
					FacingDirection = 3;
					HoneyLevel = 5;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.FacingDirection == 2 && b.HoneyLevel == 1:
					return 0;
				case { } b when true && b.FacingDirection == 3 && b.HoneyLevel == 0:
					return 1;
				case { } b when true && b.FacingDirection == 1 && b.HoneyLevel == 3:
					return 2;
				case { } b when true && b.FacingDirection == 5 && b.HoneyLevel == 1:
					return 3;
				case { } b when true && b.FacingDirection == 3 && b.HoneyLevel == 4:
					return 4;
				case { } b when true && b.FacingDirection == 4 && b.HoneyLevel == 0:
					return 5;
				case { } b when true && b.FacingDirection == 1 && b.HoneyLevel == 4:
					return 6;
				case { } b when true && b.FacingDirection == 0 && b.HoneyLevel == 4:
					return 7;
				case { } b when true && b.FacingDirection == 5 && b.HoneyLevel == 5:
					return 8;
				case { } b when true && b.FacingDirection == 5 && b.HoneyLevel == 3:
					return 9;
				case { } b when true && b.FacingDirection == 2 && b.HoneyLevel == 0:
					return 10;
				case { } b when true && b.FacingDirection == 4 && b.HoneyLevel == 2:
					return 11;
				case { } b when true && b.FacingDirection == 1 && b.HoneyLevel == 0:
					return 12;
				case { } b when true && b.FacingDirection == 3 && b.HoneyLevel == 2:
					return 13;
				case { } b when true && b.FacingDirection == 3 && b.HoneyLevel == 3:
					return 14;
				case { } b when true && b.FacingDirection == 1 && b.HoneyLevel == 1:
					return 15;
				case { } b when true && b.FacingDirection == 0 && b.HoneyLevel == 2:
					return 16;
				case { } b when true && b.FacingDirection == 5 && b.HoneyLevel == 4:
					return 17;
				case { } b when true && b.FacingDirection == 4 && b.HoneyLevel == 3:
					return 18;
				case { } b when true && b.FacingDirection == 0 && b.HoneyLevel == 5:
					return 19;
				case { } b when true && b.FacingDirection == 2 && b.HoneyLevel == 4:
					return 20;
				case { } b when true && b.FacingDirection == 4 && b.HoneyLevel == 4:
					return 21;
				case { } b when true && b.FacingDirection == 3 && b.HoneyLevel == 1:
					return 22;
				case { } b when true && b.FacingDirection == 1 && b.HoneyLevel == 5:
					return 23;
				case { } b when true && b.FacingDirection == 0 && b.HoneyLevel == 1:
					return 24;
				case { } b when true && b.FacingDirection == 0 && b.HoneyLevel == 3:
					return 25;
				case { } b when true && b.FacingDirection == 1 && b.HoneyLevel == 2:
					return 26;
				case { } b when true && b.FacingDirection == 5 && b.HoneyLevel == 2:
					return 27;
				case { } b when true && b.FacingDirection == 5 && b.HoneyLevel == 0:
					return 28;
				case { } b when true && b.FacingDirection == 2 && b.HoneyLevel == 3:
					return 29;
				case { } b when true && b.FacingDirection == 4 && b.HoneyLevel == 5:
					return 30;
				case { } b when true && b.FacingDirection == 0 && b.HoneyLevel == 0:
					return 31;
				case { } b when true && b.FacingDirection == 2 && b.HoneyLevel == 2:
					return 32;
				case { } b when true && b.FacingDirection == 2 && b.HoneyLevel == 5:
					return 33;
				case { } b when true && b.FacingDirection == 4 && b.HoneyLevel == 1:
					return 34;
				case { } b when true && b.FacingDirection == 3 && b.HoneyLevel == 5:
					return 35;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Beetroot // 244 typeof=Beetroot
	{
		[Range(0, 7)] public int Growth { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Growth = 1;
					break;
				case 1:
					Growth = 3;
					break;
				case 2:
					Growth = 0;
					break;
				case 3:
					Growth = 5;
					break;
				case 4:
					Growth = 4;
					break;
				case 5:
					Growth = 2;
					break;
				case 6:
					Growth = 6;
					break;
				case 7:
					Growth = 7;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Growth == 1:
					return 0;
				case { } b when true && b.Growth == 3:
					return 1;
				case { } b when true && b.Growth == 0:
					return 2;
				case { } b when true && b.Growth == 5:
					return 3;
				case { } b when true && b.Growth == 4:
					return 4;
				case { } b when true && b.Growth == 2:
					return 5;
				case { } b when true && b.Growth == 6:
					return 6;
				case { } b when true && b.Growth == 7:
					return 7;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Bell : Block // 0 typeof=Block
	{
		// Convert this attribute to enum
		//[Enum("hanging","multiple","side","standing"]
		public string Attachment { get; set; }
		[Range(0, 3)] public int Direction { get; set; }
		[Range(0, 1)] public byte ToggleBit { get; set; }

		public Bell() : base(461)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Attachment = "standing";
					Direction = 0;
					ToggleBit = 0;
					break;
				case 1:
					Attachment = "hanging";
					Direction = 1;
					ToggleBit = 1;
					break;
				case 2:
					Attachment = "multiple";
					Direction = 2;
					ToggleBit = 1;
					break;
				case 3:
					Attachment = "side";
					Direction = 2;
					ToggleBit = 0;
					break;
				case 4:
					Attachment = "standing";
					Direction = 3;
					ToggleBit = 1;
					break;
				case 5:
					Attachment = "hanging";
					Direction = 2;
					ToggleBit = 1;
					break;
				case 6:
					Attachment = "multiple";
					Direction = 0;
					ToggleBit = 1;
					break;
				case 7:
					Attachment = "standing";
					Direction = 3;
					ToggleBit = 0;
					break;
				case 8:
					Attachment = "side";
					Direction = 1;
					ToggleBit = 0;
					break;
				case 9:
					Attachment = "hanging";
					Direction = 2;
					ToggleBit = 0;
					break;
				case 10:
					Attachment = "side";
					Direction = 2;
					ToggleBit = 1;
					break;
				case 11:
					Attachment = "side";
					Direction = 3;
					ToggleBit = 1;
					break;
				case 12:
					Attachment = "multiple";
					Direction = 2;
					ToggleBit = 0;
					break;
				case 13:
					Attachment = "multiple";
					Direction = 1;
					ToggleBit = 0;
					break;
				case 14:
					Attachment = "standing";
					Direction = 1;
					ToggleBit = 0;
					break;
				case 15:
					Attachment = "side";
					Direction = 0;
					ToggleBit = 1;
					break;
				case 16:
					Attachment = "multiple";
					Direction = 1;
					ToggleBit = 1;
					break;
				case 17:
					Attachment = "hanging";
					Direction = 3;
					ToggleBit = 0;
					break;
				case 18:
					Attachment = "hanging";
					Direction = 0;
					ToggleBit = 1;
					break;
				case 19:
					Attachment = "side";
					Direction = 3;
					ToggleBit = 0;
					break;
				case 20:
					Attachment = "hanging";
					Direction = 3;
					ToggleBit = 1;
					break;
				case 21:
					Attachment = "hanging";
					Direction = 0;
					ToggleBit = 0;
					break;
				case 22:
					Attachment = "standing";
					Direction = 2;
					ToggleBit = 0;
					break;
				case 23:
					Attachment = "hanging";
					Direction = 1;
					ToggleBit = 0;
					break;
				case 24:
					Attachment = "standing";
					Direction = 1;
					ToggleBit = 1;
					break;
				case 25:
					Attachment = "standing";
					Direction = 2;
					ToggleBit = 1;
					break;
				case 26:
					Attachment = "standing";
					Direction = 0;
					ToggleBit = 1;
					break;
				case 27:
					Attachment = "side";
					Direction = 0;
					ToggleBit = 0;
					break;
				case 28:
					Attachment = "side";
					Direction = 1;
					ToggleBit = 1;
					break;
				case 29:
					Attachment = "multiple";
					Direction = 3;
					ToggleBit = 1;
					break;
				case 30:
					Attachment = "multiple";
					Direction = 3;
					ToggleBit = 0;
					break;
				case 31:
					Attachment = "multiple";
					Direction = 0;
					ToggleBit = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Attachment == "standing" && b.Direction == 0 && b.ToggleBit == 0:
					return 0;
				case { } b when true && b.Attachment == "hanging" && b.Direction == 1 && b.ToggleBit == 1:
					return 1;
				case { } b when true && b.Attachment == "multiple" && b.Direction == 2 && b.ToggleBit == 1:
					return 2;
				case { } b when true && b.Attachment == "side" && b.Direction == 2 && b.ToggleBit == 0:
					return 3;
				case { } b when true && b.Attachment == "standing" && b.Direction == 3 && b.ToggleBit == 1:
					return 4;
				case { } b when true && b.Attachment == "hanging" && b.Direction == 2 && b.ToggleBit == 1:
					return 5;
				case { } b when true && b.Attachment == "multiple" && b.Direction == 0 && b.ToggleBit == 1:
					return 6;
				case { } b when true && b.Attachment == "standing" && b.Direction == 3 && b.ToggleBit == 0:
					return 7;
				case { } b when true && b.Attachment == "side" && b.Direction == 1 && b.ToggleBit == 0:
					return 8;
				case { } b when true && b.Attachment == "hanging" && b.Direction == 2 && b.ToggleBit == 0:
					return 9;
				case { } b when true && b.Attachment == "side" && b.Direction == 2 && b.ToggleBit == 1:
					return 10;
				case { } b when true && b.Attachment == "side" && b.Direction == 3 && b.ToggleBit == 1:
					return 11;
				case { } b when true && b.Attachment == "multiple" && b.Direction == 2 && b.ToggleBit == 0:
					return 12;
				case { } b when true && b.Attachment == "multiple" && b.Direction == 1 && b.ToggleBit == 0:
					return 13;
				case { } b when true && b.Attachment == "standing" && b.Direction == 1 && b.ToggleBit == 0:
					return 14;
				case { } b when true && b.Attachment == "side" && b.Direction == 0 && b.ToggleBit == 1:
					return 15;
				case { } b when true && b.Attachment == "multiple" && b.Direction == 1 && b.ToggleBit == 1:
					return 16;
				case { } b when true && b.Attachment == "hanging" && b.Direction == 3 && b.ToggleBit == 0:
					return 17;
				case { } b when true && b.Attachment == "hanging" && b.Direction == 0 && b.ToggleBit == 1:
					return 18;
				case { } b when true && b.Attachment == "side" && b.Direction == 3 && b.ToggleBit == 0:
					return 19;
				case { } b when true && b.Attachment == "hanging" && b.Direction == 3 && b.ToggleBit == 1:
					return 20;
				case { } b when true && b.Attachment == "hanging" && b.Direction == 0 && b.ToggleBit == 0:
					return 21;
				case { } b when true && b.Attachment == "standing" && b.Direction == 2 && b.ToggleBit == 0:
					return 22;
				case { } b when true && b.Attachment == "hanging" && b.Direction == 1 && b.ToggleBit == 0:
					return 23;
				case { } b when true && b.Attachment == "standing" && b.Direction == 1 && b.ToggleBit == 1:
					return 24;
				case { } b when true && b.Attachment == "standing" && b.Direction == 2 && b.ToggleBit == 1:
					return 25;
				case { } b when true && b.Attachment == "standing" && b.Direction == 0 && b.ToggleBit == 1:
					return 26;
				case { } b when true && b.Attachment == "side" && b.Direction == 0 && b.ToggleBit == 0:
					return 27;
				case { } b when true && b.Attachment == "side" && b.Direction == 1 && b.ToggleBit == 1:
					return 28;
				case { } b when true && b.Attachment == "multiple" && b.Direction == 3 && b.ToggleBit == 1:
					return 29;
				case { } b when true && b.Attachment == "multiple" && b.Direction == 3 && b.ToggleBit == 0:
					return 30;
				case { } b when true && b.Attachment == "multiple" && b.Direction == 0 && b.ToggleBit == 0:
					return 31;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class BirchButton : Block // 0 typeof=Block
	{
		[Range(0, 1)] public byte ButtonPressedBit { get; set; }
		[Range(0, 5)] public int FacingDirection { get; set; }

		public BirchButton() : base(396)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					ButtonPressedBit = 0;
					FacingDirection = 0;
					break;
				case 1:
					ButtonPressedBit = 1;
					FacingDirection = 4;
					break;
				case 2:
					ButtonPressedBit = 1;
					FacingDirection = 2;
					break;
				case 3:
					ButtonPressedBit = 1;
					FacingDirection = 0;
					break;
				case 4:
					ButtonPressedBit = 0;
					FacingDirection = 5;
					break;
				case 5:
					ButtonPressedBit = 1;
					FacingDirection = 5;
					break;
				case 6:
					ButtonPressedBit = 1;
					FacingDirection = 3;
					break;
				case 7:
					ButtonPressedBit = 0;
					FacingDirection = 1;
					break;
				case 8:
					ButtonPressedBit = 1;
					FacingDirection = 1;
					break;
				case 9:
					ButtonPressedBit = 0;
					FacingDirection = 2;
					break;
				case 10:
					ButtonPressedBit = 0;
					FacingDirection = 3;
					break;
				case 11:
					ButtonPressedBit = 0;
					FacingDirection = 4;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.ButtonPressedBit == 0 && b.FacingDirection == 0:
					return 0;
				case { } b when true && b.ButtonPressedBit == 1 && b.FacingDirection == 4:
					return 1;
				case { } b when true && b.ButtonPressedBit == 1 && b.FacingDirection == 2:
					return 2;
				case { } b when true && b.ButtonPressedBit == 1 && b.FacingDirection == 0:
					return 3;
				case { } b when true && b.ButtonPressedBit == 0 && b.FacingDirection == 5:
					return 4;
				case { } b when true && b.ButtonPressedBit == 1 && b.FacingDirection == 5:
					return 5;
				case { } b when true && b.ButtonPressedBit == 1 && b.FacingDirection == 3:
					return 6;
				case { } b when true && b.ButtonPressedBit == 0 && b.FacingDirection == 1:
					return 7;
				case { } b when true && b.ButtonPressedBit == 1 && b.FacingDirection == 1:
					return 8;
				case { } b when true && b.ButtonPressedBit == 0 && b.FacingDirection == 2:
					return 9;
				case { } b when true && b.ButtonPressedBit == 0 && b.FacingDirection == 3:
					return 10;
				case { } b when true && b.ButtonPressedBit == 0 && b.FacingDirection == 4:
					return 11;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class BirchDoor // 194 typeof=BirchDoor
	{
		[Range(0, 3)] public int Direction { get; set; }
		[Range(0, 1)] public byte DoorHingeBit { get; set; }
		[Range(0, 1)] public byte OpenBit { get; set; }
		[Range(0, 1)] public byte UpperBlockBit { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Direction = 2;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 1:
					Direction = 3;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 2:
					Direction = 0;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 3:
					Direction = 0;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 4:
					Direction = 0;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 5:
					Direction = 0;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 6:
					Direction = 3;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 7:
					Direction = 2;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 8:
					Direction = 1;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 9:
					Direction = 2;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 10:
					Direction = 2;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 11:
					Direction = 3;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 12:
					Direction = 3;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 13:
					Direction = 0;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 14:
					Direction = 1;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 15:
					Direction = 0;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 16:
					Direction = 3;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 17:
					Direction = 2;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 18:
					Direction = 1;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 19:
					Direction = 3;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 20:
					Direction = 3;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 21:
					Direction = 3;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 22:
					Direction = 0;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 23:
					Direction = 1;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 24:
					Direction = 1;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 25:
					Direction = 2;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 26:
					Direction = 2;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 27:
					Direction = 1;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 28:
					Direction = 0;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 29:
					Direction = 1;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 30:
					Direction = 1;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 31:
					Direction = 2;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 0;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 1;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 2;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 3;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 4;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 5;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 6;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 7;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 8;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 9;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 10;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 11;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 12;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 13;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 14;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 15;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 16;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 17;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 18;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 19;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 20;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 21;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 22;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 23;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 24;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 25;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 26;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 27;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 28;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 29;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 30;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 31;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class BirchFenceGate // 184 typeof=BirchFenceGate
	{
		[Range(0, 3)] public int Direction { get; set; }
		[Range(0, 1)] public byte InWallBit { get; set; }
		[Range(0, 1)] public byte OpenBit { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Direction = 1;
					InWallBit = 0;
					OpenBit = 1;
					break;
				case 1:
					Direction = 3;
					InWallBit = 0;
					OpenBit = 0;
					break;
				case 2:
					Direction = 3;
					InWallBit = 1;
					OpenBit = 0;
					break;
				case 3:
					Direction = 1;
					InWallBit = 1;
					OpenBit = 1;
					break;
				case 4:
					Direction = 0;
					InWallBit = 0;
					OpenBit = 0;
					break;
				case 5:
					Direction = 2;
					InWallBit = 1;
					OpenBit = 1;
					break;
				case 6:
					Direction = 2;
					InWallBit = 1;
					OpenBit = 0;
					break;
				case 7:
					Direction = 1;
					InWallBit = 0;
					OpenBit = 0;
					break;
				case 8:
					Direction = 0;
					InWallBit = 1;
					OpenBit = 1;
					break;
				case 9:
					Direction = 2;
					InWallBit = 0;
					OpenBit = 1;
					break;
				case 10:
					Direction = 0;
					InWallBit = 0;
					OpenBit = 1;
					break;
				case 11:
					Direction = 0;
					InWallBit = 1;
					OpenBit = 0;
					break;
				case 12:
					Direction = 1;
					InWallBit = 1;
					OpenBit = 0;
					break;
				case 13:
					Direction = 2;
					InWallBit = 0;
					OpenBit = 0;
					break;
				case 14:
					Direction = 3;
					InWallBit = 0;
					OpenBit = 1;
					break;
				case 15:
					Direction = 3;
					InWallBit = 1;
					OpenBit = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Direction == 1 && b.InWallBit == 0 && b.OpenBit == 1:
					return 0;
				case { } b when true && b.Direction == 3 && b.InWallBit == 0 && b.OpenBit == 0:
					return 1;
				case { } b when true && b.Direction == 3 && b.InWallBit == 1 && b.OpenBit == 0:
					return 2;
				case { } b when true && b.Direction == 1 && b.InWallBit == 1 && b.OpenBit == 1:
					return 3;
				case { } b when true && b.Direction == 0 && b.InWallBit == 0 && b.OpenBit == 0:
					return 4;
				case { } b when true && b.Direction == 2 && b.InWallBit == 1 && b.OpenBit == 1:
					return 5;
				case { } b when true && b.Direction == 2 && b.InWallBit == 1 && b.OpenBit == 0:
					return 6;
				case { } b when true && b.Direction == 1 && b.InWallBit == 0 && b.OpenBit == 0:
					return 7;
				case { } b when true && b.Direction == 0 && b.InWallBit == 1 && b.OpenBit == 1:
					return 8;
				case { } b when true && b.Direction == 2 && b.InWallBit == 0 && b.OpenBit == 1:
					return 9;
				case { } b when true && b.Direction == 0 && b.InWallBit == 0 && b.OpenBit == 1:
					return 10;
				case { } b when true && b.Direction == 0 && b.InWallBit == 1 && b.OpenBit == 0:
					return 11;
				case { } b when true && b.Direction == 1 && b.InWallBit == 1 && b.OpenBit == 0:
					return 12;
				case { } b when true && b.Direction == 2 && b.InWallBit == 0 && b.OpenBit == 0:
					return 13;
				case { } b when true && b.Direction == 3 && b.InWallBit == 0 && b.OpenBit == 1:
					return 14;
				case { } b when true && b.Direction == 3 && b.InWallBit == 1 && b.OpenBit == 1:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class BirchPressurePlate : Block // 0 typeof=Block
	{
		[Range(0, 9)] public int RedstoneSignal { get; set; }

		public BirchPressurePlate() : base(406)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					RedstoneSignal = 7;
					break;
				case 1:
					RedstoneSignal = 5;
					break;
				case 2:
					RedstoneSignal = 3;
					break;
				case 3:
					RedstoneSignal = 1;
					break;
				case 4:
					RedstoneSignal = 12;
					break;
				case 5:
					RedstoneSignal = 9;
					break;
				case 6:
					RedstoneSignal = 8;
					break;
				case 7:
					RedstoneSignal = 0;
					break;
				case 8:
					RedstoneSignal = 4;
					break;
				case 9:
					RedstoneSignal = 10;
					break;
				case 10:
					RedstoneSignal = 2;
					break;
				case 11:
					RedstoneSignal = 11;
					break;
				case 12:
					RedstoneSignal = 13;
					break;
				case 13:
					RedstoneSignal = 6;
					break;
				case 14:
					RedstoneSignal = 14;
					break;
				case 15:
					RedstoneSignal = 15;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.RedstoneSignal == 7:
					return 0;
				case { } b when true && b.RedstoneSignal == 5:
					return 1;
				case { } b when true && b.RedstoneSignal == 3:
					return 2;
				case { } b when true && b.RedstoneSignal == 1:
					return 3;
				case { } b when true && b.RedstoneSignal == 12:
					return 4;
				case { } b when true && b.RedstoneSignal == 9:
					return 5;
				case { } b when true && b.RedstoneSignal == 8:
					return 6;
				case { } b when true && b.RedstoneSignal == 0:
					return 7;
				case { } b when true && b.RedstoneSignal == 4:
					return 8;
				case { } b when true && b.RedstoneSignal == 10:
					return 9;
				case { } b when true && b.RedstoneSignal == 2:
					return 10;
				case { } b when true && b.RedstoneSignal == 11:
					return 11;
				case { } b when true && b.RedstoneSignal == 13:
					return 12;
				case { } b when true && b.RedstoneSignal == 6:
					return 13;
				case { } b when true && b.RedstoneSignal == 14:
					return 14;
				case { } b when true && b.RedstoneSignal == 15:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class BirchStairs // 135 typeof=BirchStairs
	{
		[Range(0, 1)] public byte UpsideDownBit { get; set; }
		[Range(0, 3)] public int WeirdoDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					UpsideDownBit = 1;
					WeirdoDirection = 1;
					break;
				case 1:
					UpsideDownBit = 1;
					WeirdoDirection = 2;
					break;
				case 2:
					UpsideDownBit = 0;
					WeirdoDirection = 1;
					break;
				case 3:
					UpsideDownBit = 0;
					WeirdoDirection = 2;
					break;
				case 4:
					UpsideDownBit = 0;
					WeirdoDirection = 0;
					break;
				case 5:
					UpsideDownBit = 1;
					WeirdoDirection = 0;
					break;
				case 6:
					UpsideDownBit = 1;
					WeirdoDirection = 3;
					break;
				case 7:
					UpsideDownBit = 0;
					WeirdoDirection = 3;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 1:
					return 0;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 2:
					return 1;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 1:
					return 2;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 2:
					return 3;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 0:
					return 4;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 0:
					return 5;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 3:
					return 6;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 3:
					return 7;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class BirchStandingSign : Block // 0 typeof=Block
	{
		[Range(0, 9)] public int GroundSignDirection { get; set; }

		public BirchStandingSign() : base(441)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					GroundSignDirection = 1;
					break;
				case 1:
					GroundSignDirection = 4;
					break;
				case 2:
					GroundSignDirection = 13;
					break;
				case 3:
					GroundSignDirection = 11;
					break;
				case 4:
					GroundSignDirection = 12;
					break;
				case 5:
					GroundSignDirection = 7;
					break;
				case 6:
					GroundSignDirection = 0;
					break;
				case 7:
					GroundSignDirection = 9;
					break;
				case 8:
					GroundSignDirection = 2;
					break;
				case 9:
					GroundSignDirection = 14;
					break;
				case 10:
					GroundSignDirection = 6;
					break;
				case 11:
					GroundSignDirection = 15;
					break;
				case 12:
					GroundSignDirection = 3;
					break;
				case 13:
					GroundSignDirection = 10;
					break;
				case 14:
					GroundSignDirection = 5;
					break;
				case 15:
					GroundSignDirection = 8;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.GroundSignDirection == 1:
					return 0;
				case { } b when true && b.GroundSignDirection == 4:
					return 1;
				case { } b when true && b.GroundSignDirection == 13:
					return 2;
				case { } b when true && b.GroundSignDirection == 11:
					return 3;
				case { } b when true && b.GroundSignDirection == 12:
					return 4;
				case { } b when true && b.GroundSignDirection == 7:
					return 5;
				case { } b when true && b.GroundSignDirection == 0:
					return 6;
				case { } b when true && b.GroundSignDirection == 9:
					return 7;
				case { } b when true && b.GroundSignDirection == 2:
					return 8;
				case { } b when true && b.GroundSignDirection == 14:
					return 9;
				case { } b when true && b.GroundSignDirection == 6:
					return 10;
				case { } b when true && b.GroundSignDirection == 15:
					return 11;
				case { } b when true && b.GroundSignDirection == 3:
					return 12;
				case { } b when true && b.GroundSignDirection == 10:
					return 13;
				case { } b when true && b.GroundSignDirection == 5:
					return 14;
				case { } b when true && b.GroundSignDirection == 8:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class BirchTrapdoor : Block // 0 typeof=Block
	{
		[Range(0, 3)] public int Direction { get; set; }
		[Range(0, 1)] public byte OpenBit { get; set; }
		[Range(0, 1)] public byte UpsideDownBit { get; set; }

		public BirchTrapdoor() : base(401)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Direction = 0;
					OpenBit = 0;
					UpsideDownBit = 0;
					break;
				case 1:
					Direction = 1;
					OpenBit = 0;
					UpsideDownBit = 0;
					break;
				case 2:
					Direction = 1;
					OpenBit = 0;
					UpsideDownBit = 1;
					break;
				case 3:
					Direction = 1;
					OpenBit = 1;
					UpsideDownBit = 0;
					break;
				case 4:
					Direction = 3;
					OpenBit = 0;
					UpsideDownBit = 1;
					break;
				case 5:
					Direction = 0;
					OpenBit = 0;
					UpsideDownBit = 1;
					break;
				case 6:
					Direction = 2;
					OpenBit = 1;
					UpsideDownBit = 1;
					break;
				case 7:
					Direction = 0;
					OpenBit = 1;
					UpsideDownBit = 0;
					break;
				case 8:
					Direction = 2;
					OpenBit = 0;
					UpsideDownBit = 1;
					break;
				case 9:
					Direction = 2;
					OpenBit = 0;
					UpsideDownBit = 0;
					break;
				case 10:
					Direction = 3;
					OpenBit = 1;
					UpsideDownBit = 0;
					break;
				case 11:
					Direction = 3;
					OpenBit = 0;
					UpsideDownBit = 0;
					break;
				case 12:
					Direction = 2;
					OpenBit = 1;
					UpsideDownBit = 0;
					break;
				case 13:
					Direction = 1;
					OpenBit = 1;
					UpsideDownBit = 1;
					break;
				case 14:
					Direction = 3;
					OpenBit = 1;
					UpsideDownBit = 1;
					break;
				case 15:
					Direction = 0;
					OpenBit = 1;
					UpsideDownBit = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Direction == 0 && b.OpenBit == 0 && b.UpsideDownBit == 0:
					return 0;
				case { } b when true && b.Direction == 1 && b.OpenBit == 0 && b.UpsideDownBit == 0:
					return 1;
				case { } b when true && b.Direction == 1 && b.OpenBit == 0 && b.UpsideDownBit == 1:
					return 2;
				case { } b when true && b.Direction == 1 && b.OpenBit == 1 && b.UpsideDownBit == 0:
					return 3;
				case { } b when true && b.Direction == 3 && b.OpenBit == 0 && b.UpsideDownBit == 1:
					return 4;
				case { } b when true && b.Direction == 0 && b.OpenBit == 0 && b.UpsideDownBit == 1:
					return 5;
				case { } b when true && b.Direction == 2 && b.OpenBit == 1 && b.UpsideDownBit == 1:
					return 6;
				case { } b when true && b.Direction == 0 && b.OpenBit == 1 && b.UpsideDownBit == 0:
					return 7;
				case { } b when true && b.Direction == 2 && b.OpenBit == 0 && b.UpsideDownBit == 1:
					return 8;
				case { } b when true && b.Direction == 2 && b.OpenBit == 0 && b.UpsideDownBit == 0:
					return 9;
				case { } b when true && b.Direction == 3 && b.OpenBit == 1 && b.UpsideDownBit == 0:
					return 10;
				case { } b when true && b.Direction == 3 && b.OpenBit == 0 && b.UpsideDownBit == 0:
					return 11;
				case { } b when true && b.Direction == 2 && b.OpenBit == 1 && b.UpsideDownBit == 0:
					return 12;
				case { } b when true && b.Direction == 1 && b.OpenBit == 1 && b.UpsideDownBit == 1:
					return 13;
				case { } b when true && b.Direction == 3 && b.OpenBit == 1 && b.UpsideDownBit == 1:
					return 14;
				case { } b when true && b.Direction == 0 && b.OpenBit == 1 && b.UpsideDownBit == 1:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class BirchWallSign : Block // 0 typeof=Block
	{
		[Range(0, 5)] public int FacingDirection { get; set; }

		public BirchWallSign() : base(442)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					FacingDirection = 3;
					break;
				case 1:
					FacingDirection = 5;
					break;
				case 2:
					FacingDirection = 1;
					break;
				case 3:
					FacingDirection = 0;
					break;
				case 4:
					FacingDirection = 4;
					break;
				case 5:
					FacingDirection = 2;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.FacingDirection == 3:
					return 0;
				case { } b when true && b.FacingDirection == 5:
					return 1;
				case { } b when true && b.FacingDirection == 1:
					return 2;
				case { } b when true && b.FacingDirection == 0:
					return 3;
				case { } b when true && b.FacingDirection == 4:
					return 4;
				case { } b when true && b.FacingDirection == 2:
					return 5;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class BlackGlazedTerracotta // 235 typeof=BlackGlazedTerracotta
	{
		[Range(0, 5)] public int FacingDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					FacingDirection = 5;
					break;
				case 1:
					FacingDirection = 2;
					break;
				case 2:
					FacingDirection = 3;
					break;
				case 3:
					FacingDirection = 1;
					break;
				case 4:
					FacingDirection = 4;
					break;
				case 5:
					FacingDirection = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.FacingDirection == 5:
					return 0;
				case { } b when true && b.FacingDirection == 2:
					return 1;
				case { } b when true && b.FacingDirection == 3:
					return 2;
				case { } b when true && b.FacingDirection == 1:
					return 3;
				case { } b when true && b.FacingDirection == 4:
					return 4;
				case { } b when true && b.FacingDirection == 0:
					return 5;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class BlastFurnace : Block // 0 typeof=Block
	{
		[Range(0, 5)] public int FacingDirection { get; set; }

		public BlastFurnace() : base(451)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					FacingDirection = 0;
					break;
				case 1:
					FacingDirection = 1;
					break;
				case 2:
					FacingDirection = 4;
					break;
				case 3:
					FacingDirection = 3;
					break;
				case 4:
					FacingDirection = 2;
					break;
				case 5:
					FacingDirection = 5;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.FacingDirection == 0:
					return 0;
				case { } b when true && b.FacingDirection == 1:
					return 1;
				case { } b when true && b.FacingDirection == 4:
					return 2;
				case { } b when true && b.FacingDirection == 3:
					return 3;
				case { } b when true && b.FacingDirection == 2:
					return 4;
				case { } b when true && b.FacingDirection == 5:
					return 5;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class BlueGlazedTerracotta // 231 typeof=BlueGlazedTerracotta
	{
		[Range(0, 5)] public int FacingDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					FacingDirection = 5;
					break;
				case 1:
					FacingDirection = 2;
					break;
				case 2:
					FacingDirection = 0;
					break;
				case 3:
					FacingDirection = 1;
					break;
				case 4:
					FacingDirection = 3;
					break;
				case 5:
					FacingDirection = 4;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.FacingDirection == 5:
					return 0;
				case { } b when true && b.FacingDirection == 2:
					return 1;
				case { } b when true && b.FacingDirection == 0:
					return 2;
				case { } b when true && b.FacingDirection == 1:
					return 3;
				case { } b when true && b.FacingDirection == 3:
					return 4;
				case { } b when true && b.FacingDirection == 4:
					return 5;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class BlueIce : Block // 0 typeof=Block
	{
		public BlueIce() : base(266)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class BoneBlock : Block // 0 typeof=Block
	{
		[Range(0, 3)] public int Deprecated { get; set; }

		// Convert this attribute to enum
		//[Enum("x","y","z"]
		public string PillarAxis { get; set; }

		public BoneBlock() : base(216)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Deprecated = 3;
					PillarAxis = "x";
					break;
				case 1:
					Deprecated = 0;
					PillarAxis = "z";
					break;
				case 2:
					Deprecated = 0;
					PillarAxis = "y";
					break;
				case 3:
					Deprecated = 2;
					PillarAxis = "y";
					break;
				case 4:
					Deprecated = 1;
					PillarAxis = "y";
					break;
				case 5:
					Deprecated = 1;
					PillarAxis = "z";
					break;
				case 6:
					Deprecated = 2;
					PillarAxis = "x";
					break;
				case 7:
					Deprecated = 2;
					PillarAxis = "z";
					break;
				case 8:
					Deprecated = 0;
					PillarAxis = "x";
					break;
				case 9:
					Deprecated = 3;
					PillarAxis = "y";
					break;
				case 10:
					Deprecated = 1;
					PillarAxis = "x";
					break;
				case 11:
					Deprecated = 3;
					PillarAxis = "z";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Deprecated == 3 && b.PillarAxis == "x":
					return 0;
				case { } b when true && b.Deprecated == 0 && b.PillarAxis == "z":
					return 1;
				case { } b when true && b.Deprecated == 0 && b.PillarAxis == "y":
					return 2;
				case { } b when true && b.Deprecated == 2 && b.PillarAxis == "y":
					return 3;
				case { } b when true && b.Deprecated == 1 && b.PillarAxis == "y":
					return 4;
				case { } b when true && b.Deprecated == 1 && b.PillarAxis == "z":
					return 5;
				case { } b when true && b.Deprecated == 2 && b.PillarAxis == "x":
					return 6;
				case { } b when true && b.Deprecated == 2 && b.PillarAxis == "z":
					return 7;
				case { } b when true && b.Deprecated == 0 && b.PillarAxis == "x":
					return 8;
				case { } b when true && b.Deprecated == 3 && b.PillarAxis == "y":
					return 9;
				case { } b when true && b.Deprecated == 1 && b.PillarAxis == "x":
					return 10;
				case { } b when true && b.Deprecated == 3 && b.PillarAxis == "z":
					return 11;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Bookshelf // 47 typeof=Bookshelf
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class BrewingStand // 117 typeof=BrewingStand
	{
		[Range(0, 1)] public byte BrewingStandSlotABit { get; set; }
		[Range(0, 1)] public byte BrewingStandSlotBBit { get; set; }
		[Range(0, 1)] public byte BrewingStandSlotCBit { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					BrewingStandSlotABit = 1;
					BrewingStandSlotBBit = 0;
					BrewingStandSlotCBit = 0;
					break;
				case 1:
					BrewingStandSlotABit = 0;
					BrewingStandSlotBBit = 0;
					BrewingStandSlotCBit = 1;
					break;
				case 2:
					BrewingStandSlotABit = 0;
					BrewingStandSlotBBit = 1;
					BrewingStandSlotCBit = 1;
					break;
				case 3:
					BrewingStandSlotABit = 1;
					BrewingStandSlotBBit = 0;
					BrewingStandSlotCBit = 1;
					break;
				case 4:
					BrewingStandSlotABit = 1;
					BrewingStandSlotBBit = 1;
					BrewingStandSlotCBit = 1;
					break;
				case 5:
					BrewingStandSlotABit = 1;
					BrewingStandSlotBBit = 1;
					BrewingStandSlotCBit = 0;
					break;
				case 6:
					BrewingStandSlotABit = 0;
					BrewingStandSlotBBit = 0;
					BrewingStandSlotCBit = 0;
					break;
				case 7:
					BrewingStandSlotABit = 0;
					BrewingStandSlotBBit = 1;
					BrewingStandSlotCBit = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.BrewingStandSlotABit == 1 && b.BrewingStandSlotBBit == 0 && b.BrewingStandSlotCBit == 0:
					return 0;
				case { } b when true && b.BrewingStandSlotABit == 0 && b.BrewingStandSlotBBit == 0 && b.BrewingStandSlotCBit == 1:
					return 1;
				case { } b when true && b.BrewingStandSlotABit == 0 && b.BrewingStandSlotBBit == 1 && b.BrewingStandSlotCBit == 1:
					return 2;
				case { } b when true && b.BrewingStandSlotABit == 1 && b.BrewingStandSlotBBit == 0 && b.BrewingStandSlotCBit == 1:
					return 3;
				case { } b when true && b.BrewingStandSlotABit == 1 && b.BrewingStandSlotBBit == 1 && b.BrewingStandSlotCBit == 1:
					return 4;
				case { } b when true && b.BrewingStandSlotABit == 1 && b.BrewingStandSlotBBit == 1 && b.BrewingStandSlotCBit == 0:
					return 5;
				case { } b when true && b.BrewingStandSlotABit == 0 && b.BrewingStandSlotBBit == 0 && b.BrewingStandSlotCBit == 0:
					return 6;
				case { } b when true && b.BrewingStandSlotABit == 0 && b.BrewingStandSlotBBit == 1 && b.BrewingStandSlotCBit == 0:
					return 7;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class BrickBlock // 45 typeof=BrickBlock
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class BrickStairs // 108 typeof=BrickStairs
	{
		[Range(0, 1)] public byte UpsideDownBit { get; set; }
		[Range(0, 3)] public int WeirdoDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					UpsideDownBit = 1;
					WeirdoDirection = 1;
					break;
				case 1:
					UpsideDownBit = 0;
					WeirdoDirection = 2;
					break;
				case 2:
					UpsideDownBit = 0;
					WeirdoDirection = 3;
					break;
				case 3:
					UpsideDownBit = 0;
					WeirdoDirection = 1;
					break;
				case 4:
					UpsideDownBit = 1;
					WeirdoDirection = 3;
					break;
				case 5:
					UpsideDownBit = 1;
					WeirdoDirection = 0;
					break;
				case 6:
					UpsideDownBit = 1;
					WeirdoDirection = 2;
					break;
				case 7:
					UpsideDownBit = 0;
					WeirdoDirection = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 1:
					return 0;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 2:
					return 1;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 3:
					return 2;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 1:
					return 3;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 3:
					return 4;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 0:
					return 5;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 2:
					return 6;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 0:
					return 7;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class BrownGlazedTerracotta // 232 typeof=BrownGlazedTerracotta
	{
		[Range(0, 5)] public int FacingDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					FacingDirection = 5;
					break;
				case 1:
					FacingDirection = 1;
					break;
				case 2:
					FacingDirection = 4;
					break;
				case 3:
					FacingDirection = 0;
					break;
				case 4:
					FacingDirection = 2;
					break;
				case 5:
					FacingDirection = 3;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.FacingDirection == 5:
					return 0;
				case { } b when true && b.FacingDirection == 1:
					return 1;
				case { } b when true && b.FacingDirection == 4:
					return 2;
				case { } b when true && b.FacingDirection == 0:
					return 3;
				case { } b when true && b.FacingDirection == 2:
					return 4;
				case { } b when true && b.FacingDirection == 3:
					return 5;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class BrownMushroom // 39 typeof=BrownMushroom
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class BrownMushroomBlock // 99 typeof=BrownMushroomBlock
	{
		[Range(0, 9)] public int HugeMushroomBits { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					HugeMushroomBits = 14;
					break;
				case 1:
					HugeMushroomBits = 6;
					break;
				case 2:
					HugeMushroomBits = 7;
					break;
				case 3:
					HugeMushroomBits = 12;
					break;
				case 4:
					HugeMushroomBits = 13;
					break;
				case 5:
					HugeMushroomBits = 10;
					break;
				case 6:
					HugeMushroomBits = 11;
					break;
				case 7:
					HugeMushroomBits = 0;
					break;
				case 8:
					HugeMushroomBits = 9;
					break;
				case 9:
					HugeMushroomBits = 1;
					break;
				case 10:
					HugeMushroomBits = 8;
					break;
				case 11:
					HugeMushroomBits = 2;
					break;
				case 12:
					HugeMushroomBits = 4;
					break;
				case 13:
					HugeMushroomBits = 15;
					break;
				case 14:
					HugeMushroomBits = 5;
					break;
				case 15:
					HugeMushroomBits = 3;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.HugeMushroomBits == 14:
					return 0;
				case { } b when true && b.HugeMushroomBits == 6:
					return 1;
				case { } b when true && b.HugeMushroomBits == 7:
					return 2;
				case { } b when true && b.HugeMushroomBits == 12:
					return 3;
				case { } b when true && b.HugeMushroomBits == 13:
					return 4;
				case { } b when true && b.HugeMushroomBits == 10:
					return 5;
				case { } b when true && b.HugeMushroomBits == 11:
					return 6;
				case { } b when true && b.HugeMushroomBits == 0:
					return 7;
				case { } b when true && b.HugeMushroomBits == 9:
					return 8;
				case { } b when true && b.HugeMushroomBits == 1:
					return 9;
				case { } b when true && b.HugeMushroomBits == 8:
					return 10;
				case { } b when true && b.HugeMushroomBits == 2:
					return 11;
				case { } b when true && b.HugeMushroomBits == 4:
					return 12;
				case { } b when true && b.HugeMushroomBits == 15:
					return 13;
				case { } b when true && b.HugeMushroomBits == 5:
					return 14;
				case { } b when true && b.HugeMushroomBits == 3:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class BubbleColumn : Block // 0 typeof=Block
	{
		[Range(0, 1)] public byte DragDown { get; set; }

		public BubbleColumn() : base(415)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					DragDown = 1;
					break;
				case 1:
					DragDown = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.DragDown == 1:
					return 0;
				case { } b when true && b.DragDown == 0:
					return 1;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Cactus // 81 typeof=Cactus
	{
		[Range(0, 9)] public int Age { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Age = 3;
					break;
				case 1:
					Age = 11;
					break;
				case 2:
					Age = 10;
					break;
				case 3:
					Age = 8;
					break;
				case 4:
					Age = 1;
					break;
				case 5:
					Age = 6;
					break;
				case 6:
					Age = 9;
					break;
				case 7:
					Age = 4;
					break;
				case 8:
					Age = 0;
					break;
				case 9:
					Age = 15;
					break;
				case 10:
					Age = 2;
					break;
				case 11:
					Age = 12;
					break;
				case 12:
					Age = 13;
					break;
				case 13:
					Age = 14;
					break;
				case 14:
					Age = 5;
					break;
				case 15:
					Age = 7;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Age == 3:
					return 0;
				case { } b when true && b.Age == 11:
					return 1;
				case { } b when true && b.Age == 10:
					return 2;
				case { } b when true && b.Age == 8:
					return 3;
				case { } b when true && b.Age == 1:
					return 4;
				case { } b when true && b.Age == 6:
					return 5;
				case { } b when true && b.Age == 9:
					return 6;
				case { } b when true && b.Age == 4:
					return 7;
				case { } b when true && b.Age == 0:
					return 8;
				case { } b when true && b.Age == 15:
					return 9;
				case { } b when true && b.Age == 2:
					return 10;
				case { } b when true && b.Age == 12:
					return 11;
				case { } b when true && b.Age == 13:
					return 12;
				case { } b when true && b.Age == 14:
					return 13;
				case { } b when true && b.Age == 5:
					return 14;
				case { } b when true && b.Age == 7:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Cake // 92 typeof=Cake
	{
		[Range(0, 6)] public int BiteCounter { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					BiteCounter = 4;
					break;
				case 1:
					BiteCounter = 3;
					break;
				case 2:
					BiteCounter = 6;
					break;
				case 3:
					BiteCounter = 5;
					break;
				case 4:
					BiteCounter = 2;
					break;
				case 5:
					BiteCounter = 1;
					break;
				case 6:
					BiteCounter = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.BiteCounter == 4:
					return 0;
				case { } b when true && b.BiteCounter == 3:
					return 1;
				case { } b when true && b.BiteCounter == 6:
					return 2;
				case { } b when true && b.BiteCounter == 5:
					return 3;
				case { } b when true && b.BiteCounter == 2:
					return 4;
				case { } b when true && b.BiteCounter == 1:
					return 5;
				case { } b when true && b.BiteCounter == 0:
					return 6;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Camera : Block // 0 typeof=Block
	{
		public Camera() : base(242)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Campfire : Block // 0 typeof=Block
	{
		[Range(0, 3)] public int Direction { get; set; }
		[Range(0, 1)] public byte Extinguished { get; set; }

		public Campfire() : base(464)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Direction = 0;
					Extinguished = 1;
					break;
				case 1:
					Direction = 0;
					Extinguished = 0;
					break;
				case 2:
					Direction = 1;
					Extinguished = 1;
					break;
				case 3:
					Direction = 3;
					Extinguished = 1;
					break;
				case 4:
					Direction = 2;
					Extinguished = 1;
					break;
				case 5:
					Direction = 2;
					Extinguished = 0;
					break;
				case 6:
					Direction = 1;
					Extinguished = 0;
					break;
				case 7:
					Direction = 3;
					Extinguished = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Direction == 0 && b.Extinguished == 1:
					return 0;
				case { } b when true && b.Direction == 0 && b.Extinguished == 0:
					return 1;
				case { } b when true && b.Direction == 1 && b.Extinguished == 1:
					return 2;
				case { } b when true && b.Direction == 3 && b.Extinguished == 1:
					return 3;
				case { } b when true && b.Direction == 2 && b.Extinguished == 1:
					return 4;
				case { } b when true && b.Direction == 2 && b.Extinguished == 0:
					return 5;
				case { } b when true && b.Direction == 1 && b.Extinguished == 0:
					return 6;
				case { } b when true && b.Direction == 3 && b.Extinguished == 0:
					return 7;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Carpet // 171 typeof=Carpet
	{
		// Convert this attribute to enum
		//[Enum("black","blue","brown","cyan","gray","green","light_blue","lime","magenta","orange","pink","purple","red","silver","white","yellow"]
		public string Color { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Color = "yellow";
					break;
				case 1:
					Color = "lime";
					break;
				case 2:
					Color = "cyan";
					break;
				case 3:
					Color = "brown";
					break;
				case 4:
					Color = "blue";
					break;
				case 5:
					Color = "black";
					break;
				case 6:
					Color = "orange";
					break;
				case 7:
					Color = "gray";
					break;
				case 8:
					Color = "pink";
					break;
				case 9:
					Color = "purple";
					break;
				case 10:
					Color = "red";
					break;
				case 11:
					Color = "green";
					break;
				case 12:
					Color = "white";
					break;
				case 13:
					Color = "light_blue";
					break;
				case 14:
					Color = "silver";
					break;
				case 15:
					Color = "magenta";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Color == "yellow":
					return 0;
				case { } b when true && b.Color == "lime":
					return 1;
				case { } b when true && b.Color == "cyan":
					return 2;
				case { } b when true && b.Color == "brown":
					return 3;
				case { } b when true && b.Color == "blue":
					return 4;
				case { } b when true && b.Color == "black":
					return 5;
				case { } b when true && b.Color == "orange":
					return 6;
				case { } b when true && b.Color == "gray":
					return 7;
				case { } b when true && b.Color == "pink":
					return 8;
				case { } b when true && b.Color == "purple":
					return 9;
				case { } b when true && b.Color == "red":
					return 10;
				case { } b when true && b.Color == "green":
					return 11;
				case { } b when true && b.Color == "white":
					return 12;
				case { } b when true && b.Color == "light_blue":
					return 13;
				case { } b when true && b.Color == "silver":
					return 14;
				case { } b when true && b.Color == "magenta":
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Carrots // 141 typeof=Carrots
	{
		[Range(0, 7)] public int Growth { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Growth = 4;
					break;
				case 1:
					Growth = 7;
					break;
				case 2:
					Growth = 5;
					break;
				case 3:
					Growth = 0;
					break;
				case 4:
					Growth = 1;
					break;
				case 5:
					Growth = 6;
					break;
				case 6:
					Growth = 2;
					break;
				case 7:
					Growth = 3;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Growth == 4:
					return 0;
				case { } b when true && b.Growth == 7:
					return 1;
				case { } b when true && b.Growth == 5:
					return 2;
				case { } b when true && b.Growth == 0:
					return 3;
				case { } b when true && b.Growth == 1:
					return 4;
				case { } b when true && b.Growth == 6:
					return 5;
				case { } b when true && b.Growth == 2:
					return 6;
				case { } b when true && b.Growth == 3:
					return 7;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class CartographyTable : Block // 0 typeof=Block
	{
		public CartographyTable() : base(455)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class CarvedPumpkin : Block // 0 typeof=Block
	{
		[Range(0, 3)] public int Direction { get; set; }

		public CarvedPumpkin() : base(410)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Direction = 1;
					break;
				case 1:
					Direction = 3;
					break;
				case 2:
					Direction = 2;
					break;
				case 3:
					Direction = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Direction == 1:
					return 0;
				case { } b when true && b.Direction == 3:
					return 1;
				case { } b when true && b.Direction == 2:
					return 2;
				case { } b when true && b.Direction == 0:
					return 3;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Cauldron // 118 typeof=Cauldron
	{
		// Convert this attribute to enum
		//[Enum("lava","water"]
		public string CauldronLiquid { get; set; }
		[Range(0, 6)] public int FillLevel { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					CauldronLiquid = "water";
					FillLevel = 3;
					break;
				case 1:
					CauldronLiquid = "water";
					FillLevel = 2;
					break;
				case 2:
					CauldronLiquid = "lava";
					FillLevel = 6;
					break;
				case 3:
					CauldronLiquid = "lava";
					FillLevel = 3;
					break;
				case 4:
					CauldronLiquid = "water";
					FillLevel = 5;
					break;
				case 5:
					CauldronLiquid = "lava";
					FillLevel = 1;
					break;
				case 6:
					CauldronLiquid = "lava";
					FillLevel = 4;
					break;
				case 7:
					CauldronLiquid = "water";
					FillLevel = 0;
					break;
				case 8:
					CauldronLiquid = "lava";
					FillLevel = 0;
					break;
				case 9:
					CauldronLiquid = "water";
					FillLevel = 4;
					break;
				case 10:
					CauldronLiquid = "water";
					FillLevel = 1;
					break;
				case 11:
					CauldronLiquid = "lava";
					FillLevel = 5;
					break;
				case 12:
					CauldronLiquid = "water";
					FillLevel = 6;
					break;
				case 13:
					CauldronLiquid = "lava";
					FillLevel = 2;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.CauldronLiquid == "water" && b.FillLevel == 3:
					return 0;
				case { } b when true && b.CauldronLiquid == "water" && b.FillLevel == 2:
					return 1;
				case { } b when true && b.CauldronLiquid == "lava" && b.FillLevel == 6:
					return 2;
				case { } b when true && b.CauldronLiquid == "lava" && b.FillLevel == 3:
					return 3;
				case { } b when true && b.CauldronLiquid == "water" && b.FillLevel == 5:
					return 4;
				case { } b when true && b.CauldronLiquid == "lava" && b.FillLevel == 1:
					return 5;
				case { } b when true && b.CauldronLiquid == "lava" && b.FillLevel == 4:
					return 6;
				case { } b when true && b.CauldronLiquid == "water" && b.FillLevel == 0:
					return 7;
				case { } b when true && b.CauldronLiquid == "lava" && b.FillLevel == 0:
					return 8;
				case { } b when true && b.CauldronLiquid == "water" && b.FillLevel == 4:
					return 9;
				case { } b when true && b.CauldronLiquid == "water" && b.FillLevel == 1:
					return 10;
				case { } b when true && b.CauldronLiquid == "lava" && b.FillLevel == 5:
					return 11;
				case { } b when true && b.CauldronLiquid == "water" && b.FillLevel == 6:
					return 12;
				case { } b when true && b.CauldronLiquid == "lava" && b.FillLevel == 2:
					return 13;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class ChainCommandBlock : Block // 0 typeof=Block
	{
		[Range(0, 1)] public byte ConditionalBit { get; set; }
		[Range(0, 5)] public int FacingDirection { get; set; }

		public ChainCommandBlock() : base(189)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					ConditionalBit = 1;
					FacingDirection = 0;
					break;
				case 1:
					ConditionalBit = 0;
					FacingDirection = 5;
					break;
				case 2:
					ConditionalBit = 0;
					FacingDirection = 0;
					break;
				case 3:
					ConditionalBit = 1;
					FacingDirection = 4;
					break;
				case 4:
					ConditionalBit = 1;
					FacingDirection = 3;
					break;
				case 5:
					ConditionalBit = 1;
					FacingDirection = 5;
					break;
				case 6:
					ConditionalBit = 0;
					FacingDirection = 2;
					break;
				case 7:
					ConditionalBit = 1;
					FacingDirection = 2;
					break;
				case 8:
					ConditionalBit = 0;
					FacingDirection = 4;
					break;
				case 9:
					ConditionalBit = 1;
					FacingDirection = 1;
					break;
				case 10:
					ConditionalBit = 0;
					FacingDirection = 3;
					break;
				case 11:
					ConditionalBit = 0;
					FacingDirection = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.ConditionalBit == 1 && b.FacingDirection == 0:
					return 0;
				case { } b when true && b.ConditionalBit == 0 && b.FacingDirection == 5:
					return 1;
				case { } b when true && b.ConditionalBit == 0 && b.FacingDirection == 0:
					return 2;
				case { } b when true && b.ConditionalBit == 1 && b.FacingDirection == 4:
					return 3;
				case { } b when true && b.ConditionalBit == 1 && b.FacingDirection == 3:
					return 4;
				case { } b when true && b.ConditionalBit == 1 && b.FacingDirection == 5:
					return 5;
				case { } b when true && b.ConditionalBit == 0 && b.FacingDirection == 2:
					return 6;
				case { } b when true && b.ConditionalBit == 1 && b.FacingDirection == 2:
					return 7;
				case { } b when true && b.ConditionalBit == 0 && b.FacingDirection == 4:
					return 8;
				case { } b when true && b.ConditionalBit == 1 && b.FacingDirection == 1:
					return 9;
				case { } b when true && b.ConditionalBit == 0 && b.FacingDirection == 3:
					return 10;
				case { } b when true && b.ConditionalBit == 0 && b.FacingDirection == 1:
					return 11;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class ChemicalHeat : Block // 0 typeof=Block
	{
		public ChemicalHeat() : base(192)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class ChemistryTable : Block // 0 typeof=Block
	{
		// Convert this attribute to enum
		//[Enum("compound_creator","element_constructor","lab_table","material_reducer"]
		public string ChemistryTableType { get; set; }
		[Range(0, 3)] public int Direction { get; set; }

		public ChemistryTable() : base(238)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					ChemistryTableType = "material_reducer";
					Direction = 1;
					break;
				case 1:
					ChemistryTableType = "lab_table";
					Direction = 3;
					break;
				case 2:
					ChemistryTableType = "material_reducer";
					Direction = 0;
					break;
				case 3:
					ChemistryTableType = "element_constructor";
					Direction = 0;
					break;
				case 4:
					ChemistryTableType = "element_constructor";
					Direction = 3;
					break;
				case 5:
					ChemistryTableType = "material_reducer";
					Direction = 2;
					break;
				case 6:
					ChemistryTableType = "material_reducer";
					Direction = 3;
					break;
				case 7:
					ChemistryTableType = "compound_creator";
					Direction = 0;
					break;
				case 8:
					ChemistryTableType = "element_constructor";
					Direction = 1;
					break;
				case 9:
					ChemistryTableType = "element_constructor";
					Direction = 2;
					break;
				case 10:
					ChemistryTableType = "lab_table";
					Direction = 2;
					break;
				case 11:
					ChemistryTableType = "lab_table";
					Direction = 1;
					break;
				case 12:
					ChemistryTableType = "compound_creator";
					Direction = 3;
					break;
				case 13:
					ChemistryTableType = "compound_creator";
					Direction = 2;
					break;
				case 14:
					ChemistryTableType = "lab_table";
					Direction = 0;
					break;
				case 15:
					ChemistryTableType = "compound_creator";
					Direction = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.ChemistryTableType == "material_reducer" && b.Direction == 1:
					return 0;
				case { } b when true && b.ChemistryTableType == "lab_table" && b.Direction == 3:
					return 1;
				case { } b when true && b.ChemistryTableType == "material_reducer" && b.Direction == 0:
					return 2;
				case { } b when true && b.ChemistryTableType == "element_constructor" && b.Direction == 0:
					return 3;
				case { } b when true && b.ChemistryTableType == "element_constructor" && b.Direction == 3:
					return 4;
				case { } b when true && b.ChemistryTableType == "material_reducer" && b.Direction == 2:
					return 5;
				case { } b when true && b.ChemistryTableType == "material_reducer" && b.Direction == 3:
					return 6;
				case { } b when true && b.ChemistryTableType == "compound_creator" && b.Direction == 0:
					return 7;
				case { } b when true && b.ChemistryTableType == "element_constructor" && b.Direction == 1:
					return 8;
				case { } b when true && b.ChemistryTableType == "element_constructor" && b.Direction == 2:
					return 9;
				case { } b when true && b.ChemistryTableType == "lab_table" && b.Direction == 2:
					return 10;
				case { } b when true && b.ChemistryTableType == "lab_table" && b.Direction == 1:
					return 11;
				case { } b when true && b.ChemistryTableType == "compound_creator" && b.Direction == 3:
					return 12;
				case { } b when true && b.ChemistryTableType == "compound_creator" && b.Direction == 2:
					return 13;
				case { } b when true && b.ChemistryTableType == "lab_table" && b.Direction == 0:
					return 14;
				case { } b when true && b.ChemistryTableType == "compound_creator" && b.Direction == 1:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Chest // 54 typeof=Chest
	{
		[Range(0, 5)] public int FacingDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					FacingDirection = 1;
					break;
				case 1:
					FacingDirection = 3;
					break;
				case 2:
					FacingDirection = 0;
					break;
				case 3:
					FacingDirection = 4;
					break;
				case 4:
					FacingDirection = 2;
					break;
				case 5:
					FacingDirection = 5;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.FacingDirection == 1:
					return 0;
				case { } b when true && b.FacingDirection == 3:
					return 1;
				case { } b when true && b.FacingDirection == 0:
					return 2;
				case { } b when true && b.FacingDirection == 4:
					return 3;
				case { } b when true && b.FacingDirection == 2:
					return 4;
				case { } b when true && b.FacingDirection == 5:
					return 5;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class ChorusFlower // 200 typeof=ChorusFlower
	{
		[Range(0, 5)] public int Age { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Age = 5;
					break;
				case 1:
					Age = 2;
					break;
				case 2:
					Age = 0;
					break;
				case 3:
					Age = 1;
					break;
				case 4:
					Age = 4;
					break;
				case 5:
					Age = 3;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Age == 5:
					return 0;
				case { } b when true && b.Age == 2:
					return 1;
				case { } b when true && b.Age == 0:
					return 2;
				case { } b when true && b.Age == 1:
					return 3;
				case { } b when true && b.Age == 4:
					return 4;
				case { } b when true && b.Age == 3:
					return 5;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class ChorusPlant // 240 typeof=ChorusPlant
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Clay // 82 typeof=Clay
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class CoalBlock // 173 typeof=CoalBlock
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class CoalOre // 16 typeof=CoalOre
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Cobblestone // 4 typeof=Cobblestone
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class CobblestoneWall // 139 typeof=CobblestoneWall
	{
		// Convert this attribute to enum
		//[Enum("andesite","brick","cobblestone","diorite","end_brick","granite","mossy_cobblestone","mossy_stone_brick","nether_brick","prismarine","red_nether_brick","red_sandstone","sandstone","stone_brick"]
		public string WallBlockType { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					WallBlockType = "brick";
					break;
				case 1:
					WallBlockType = "red_sandstone";
					break;
				case 2:
					WallBlockType = "mossy_cobblestone";
					break;
				case 3:
					WallBlockType = "andesite";
					break;
				case 4:
					WallBlockType = "prismarine";
					break;
				case 5:
					WallBlockType = "granite";
					break;
				case 6:
					WallBlockType = "nether_brick";
					break;
				case 7:
					WallBlockType = "red_nether_brick";
					break;
				case 8:
					WallBlockType = "diorite";
					break;
				case 9:
					WallBlockType = "sandstone";
					break;
				case 10:
					WallBlockType = "cobblestone";
					break;
				case 11:
					WallBlockType = "stone_brick";
					break;
				case 12:
					WallBlockType = "mossy_stone_brick";
					break;
				case 13:
					WallBlockType = "end_brick";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.WallBlockType == "brick":
					return 0;
				case { } b when true && b.WallBlockType == "red_sandstone":
					return 1;
				case { } b when true && b.WallBlockType == "mossy_cobblestone":
					return 2;
				case { } b when true && b.WallBlockType == "andesite":
					return 3;
				case { } b when true && b.WallBlockType == "prismarine":
					return 4;
				case { } b when true && b.WallBlockType == "granite":
					return 5;
				case { } b when true && b.WallBlockType == "nether_brick":
					return 6;
				case { } b when true && b.WallBlockType == "red_nether_brick":
					return 7;
				case { } b when true && b.WallBlockType == "diorite":
					return 8;
				case { } b when true && b.WallBlockType == "sandstone":
					return 9;
				case { } b when true && b.WallBlockType == "cobblestone":
					return 10;
				case { } b when true && b.WallBlockType == "stone_brick":
					return 11;
				case { } b when true && b.WallBlockType == "mossy_stone_brick":
					return 12;
				case { } b when true && b.WallBlockType == "end_brick":
					return 13;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Cocoa // 127 typeof=Cocoa
	{
		[Range(0, 2)] public int Age { get; set; }
		[Range(0, 3)] public int Direction { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Age = 0;
					Direction = 3;
					break;
				case 1:
					Age = 1;
					Direction = 0;
					break;
				case 2:
					Age = 2;
					Direction = 3;
					break;
				case 3:
					Age = 2;
					Direction = 0;
					break;
				case 4:
					Age = 1;
					Direction = 2;
					break;
				case 5:
					Age = 2;
					Direction = 1;
					break;
				case 6:
					Age = 0;
					Direction = 1;
					break;
				case 7:
					Age = 1;
					Direction = 1;
					break;
				case 8:
					Age = 0;
					Direction = 2;
					break;
				case 9:
					Age = 1;
					Direction = 3;
					break;
				case 10:
					Age = 0;
					Direction = 0;
					break;
				case 11:
					Age = 2;
					Direction = 2;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Age == 0 && b.Direction == 3:
					return 0;
				case { } b when true && b.Age == 1 && b.Direction == 0:
					return 1;
				case { } b when true && b.Age == 2 && b.Direction == 3:
					return 2;
				case { } b when true && b.Age == 2 && b.Direction == 0:
					return 3;
				case { } b when true && b.Age == 1 && b.Direction == 2:
					return 4;
				case { } b when true && b.Age == 2 && b.Direction == 1:
					return 5;
				case { } b when true && b.Age == 0 && b.Direction == 1:
					return 6;
				case { } b when true && b.Age == 1 && b.Direction == 1:
					return 7;
				case { } b when true && b.Age == 0 && b.Direction == 2:
					return 8;
				case { } b when true && b.Age == 1 && b.Direction == 3:
					return 9;
				case { } b when true && b.Age == 0 && b.Direction == 0:
					return 10;
				case { } b when true && b.Age == 2 && b.Direction == 2:
					return 11;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class ColoredTorchBp : Block // 0 typeof=Block
	{
		[Range(0, 1)] public byte ColorBit { get; set; }

		// Convert this attribute to enum
		//[Enum("east","north","south","top","unknown","west"]
		public string TorchFacingDirection { get; set; }

		public ColoredTorchBp() : base(204)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					ColorBit = 0;
					TorchFacingDirection = "top";
					break;
				case 1:
					ColorBit = 1;
					TorchFacingDirection = "west";
					break;
				case 2:
					ColorBit = 0;
					TorchFacingDirection = "west";
					break;
				case 3:
					ColorBit = 1;
					TorchFacingDirection = "north";
					break;
				case 4:
					ColorBit = 1;
					TorchFacingDirection = "south";
					break;
				case 5:
					ColorBit = 1;
					TorchFacingDirection = "top";
					break;
				case 6:
					ColorBit = 0;
					TorchFacingDirection = "east";
					break;
				case 7:
					ColorBit = 0;
					TorchFacingDirection = "north";
					break;
				case 8:
					ColorBit = 1;
					TorchFacingDirection = "east";
					break;
				case 9:
					ColorBit = 0;
					TorchFacingDirection = "south";
					break;
				case 10:
					ColorBit = 0;
					TorchFacingDirection = "unknown";
					break;
				case 11:
					ColorBit = 1;
					TorchFacingDirection = "unknown";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.ColorBit == 0 && b.TorchFacingDirection == "top":
					return 0;
				case { } b when true && b.ColorBit == 1 && b.TorchFacingDirection == "west":
					return 1;
				case { } b when true && b.ColorBit == 0 && b.TorchFacingDirection == "west":
					return 2;
				case { } b when true && b.ColorBit == 1 && b.TorchFacingDirection == "north":
					return 3;
				case { } b when true && b.ColorBit == 1 && b.TorchFacingDirection == "south":
					return 4;
				case { } b when true && b.ColorBit == 1 && b.TorchFacingDirection == "top":
					return 5;
				case { } b when true && b.ColorBit == 0 && b.TorchFacingDirection == "east":
					return 6;
				case { } b when true && b.ColorBit == 0 && b.TorchFacingDirection == "north":
					return 7;
				case { } b when true && b.ColorBit == 1 && b.TorchFacingDirection == "east":
					return 8;
				case { } b when true && b.ColorBit == 0 && b.TorchFacingDirection == "south":
					return 9;
				case { } b when true && b.ColorBit == 0 && b.TorchFacingDirection == "unknown":
					return 10;
				case { } b when true && b.ColorBit == 1 && b.TorchFacingDirection == "unknown":
					return 11;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class ColoredTorchRg : Block // 0 typeof=Block
	{
		[Range(0, 1)] public byte ColorBit { get; set; }

		// Convert this attribute to enum
		//[Enum("east","north","south","top","unknown","west"]
		public string TorchFacingDirection { get; set; }

		public ColoredTorchRg() : base(202)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					ColorBit = 0;
					TorchFacingDirection = "north";
					break;
				case 1:
					ColorBit = 1;
					TorchFacingDirection = "east";
					break;
				case 2:
					ColorBit = 0;
					TorchFacingDirection = "top";
					break;
				case 3:
					ColorBit = 1;
					TorchFacingDirection = "south";
					break;
				case 4:
					ColorBit = 0;
					TorchFacingDirection = "west";
					break;
				case 5:
					ColorBit = 1;
					TorchFacingDirection = "north";
					break;
				case 6:
					ColorBit = 1;
					TorchFacingDirection = "west";
					break;
				case 7:
					ColorBit = 1;
					TorchFacingDirection = "unknown";
					break;
				case 8:
					ColorBit = 0;
					TorchFacingDirection = "south";
					break;
				case 9:
					ColorBit = 1;
					TorchFacingDirection = "top";
					break;
				case 10:
					ColorBit = 0;
					TorchFacingDirection = "east";
					break;
				case 11:
					ColorBit = 0;
					TorchFacingDirection = "unknown";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.ColorBit == 0 && b.TorchFacingDirection == "north":
					return 0;
				case { } b when true && b.ColorBit == 1 && b.TorchFacingDirection == "east":
					return 1;
				case { } b when true && b.ColorBit == 0 && b.TorchFacingDirection == "top":
					return 2;
				case { } b when true && b.ColorBit == 1 && b.TorchFacingDirection == "south":
					return 3;
				case { } b when true && b.ColorBit == 0 && b.TorchFacingDirection == "west":
					return 4;
				case { } b when true && b.ColorBit == 1 && b.TorchFacingDirection == "north":
					return 5;
				case { } b when true && b.ColorBit == 1 && b.TorchFacingDirection == "west":
					return 6;
				case { } b when true && b.ColorBit == 1 && b.TorchFacingDirection == "unknown":
					return 7;
				case { } b when true && b.ColorBit == 0 && b.TorchFacingDirection == "south":
					return 8;
				case { } b when true && b.ColorBit == 1 && b.TorchFacingDirection == "top":
					return 9;
				case { } b when true && b.ColorBit == 0 && b.TorchFacingDirection == "east":
					return 10;
				case { } b when true && b.ColorBit == 0 && b.TorchFacingDirection == "unknown":
					return 11;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class CommandBlock : Block // 0 typeof=Block
	{
		[Range(0, 1)] public byte ConditionalBit { get; set; }
		[Range(0, 5)] public int FacingDirection { get; set; }

		public CommandBlock() : base(137)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					ConditionalBit = 1;
					FacingDirection = 5;
					break;
				case 1:
					ConditionalBit = 1;
					FacingDirection = 0;
					break;
				case 2:
					ConditionalBit = 0;
					FacingDirection = 2;
					break;
				case 3:
					ConditionalBit = 1;
					FacingDirection = 4;
					break;
				case 4:
					ConditionalBit = 1;
					FacingDirection = 1;
					break;
				case 5:
					ConditionalBit = 0;
					FacingDirection = 4;
					break;
				case 6:
					ConditionalBit = 0;
					FacingDirection = 0;
					break;
				case 7:
					ConditionalBit = 1;
					FacingDirection = 3;
					break;
				case 8:
					ConditionalBit = 0;
					FacingDirection = 1;
					break;
				case 9:
					ConditionalBit = 1;
					FacingDirection = 2;
					break;
				case 10:
					ConditionalBit = 0;
					FacingDirection = 3;
					break;
				case 11:
					ConditionalBit = 0;
					FacingDirection = 5;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.ConditionalBit == 1 && b.FacingDirection == 5:
					return 0;
				case { } b when true && b.ConditionalBit == 1 && b.FacingDirection == 0:
					return 1;
				case { } b when true && b.ConditionalBit == 0 && b.FacingDirection == 2:
					return 2;
				case { } b when true && b.ConditionalBit == 1 && b.FacingDirection == 4:
					return 3;
				case { } b when true && b.ConditionalBit == 1 && b.FacingDirection == 1:
					return 4;
				case { } b when true && b.ConditionalBit == 0 && b.FacingDirection == 4:
					return 5;
				case { } b when true && b.ConditionalBit == 0 && b.FacingDirection == 0:
					return 6;
				case { } b when true && b.ConditionalBit == 1 && b.FacingDirection == 3:
					return 7;
				case { } b when true && b.ConditionalBit == 0 && b.FacingDirection == 1:
					return 8;
				case { } b when true && b.ConditionalBit == 1 && b.FacingDirection == 2:
					return 9;
				case { } b when true && b.ConditionalBit == 0 && b.FacingDirection == 3:
					return 10;
				case { } b when true && b.ConditionalBit == 0 && b.FacingDirection == 5:
					return 11;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Composter : Block // 0 typeof=Block
	{
		[Range(0, 8)] public int ComposterFillLevel { get; set; }

		public Composter() : base(468)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					ComposterFillLevel = 3;
					break;
				case 1:
					ComposterFillLevel = 7;
					break;
				case 2:
					ComposterFillLevel = 6;
					break;
				case 3:
					ComposterFillLevel = 5;
					break;
				case 4:
					ComposterFillLevel = 1;
					break;
				case 5:
					ComposterFillLevel = 8;
					break;
				case 6:
					ComposterFillLevel = 2;
					break;
				case 7:
					ComposterFillLevel = 4;
					break;
				case 8:
					ComposterFillLevel = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.ComposterFillLevel == 3:
					return 0;
				case { } b when true && b.ComposterFillLevel == 7:
					return 1;
				case { } b when true && b.ComposterFillLevel == 6:
					return 2;
				case { } b when true && b.ComposterFillLevel == 5:
					return 3;
				case { } b when true && b.ComposterFillLevel == 1:
					return 4;
				case { } b when true && b.ComposterFillLevel == 8:
					return 5;
				case { } b when true && b.ComposterFillLevel == 2:
					return 6;
				case { } b when true && b.ComposterFillLevel == 4:
					return 7;
				case { } b when true && b.ComposterFillLevel == 0:
					return 8;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Concrete // 236 typeof=Concrete
	{
		// Convert this attribute to enum
		//[Enum("black","blue","brown","cyan","gray","green","light_blue","lime","magenta","orange","pink","purple","red","silver","white","yellow"]
		public string Color { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Color = "cyan";
					break;
				case 1:
					Color = "orange";
					break;
				case 2:
					Color = "purple";
					break;
				case 3:
					Color = "gray";
					break;
				case 4:
					Color = "red";
					break;
				case 5:
					Color = "lime";
					break;
				case 6:
					Color = "blue";
					break;
				case 7:
					Color = "brown";
					break;
				case 8:
					Color = "white";
					break;
				case 9:
					Color = "black";
					break;
				case 10:
					Color = "yellow";
					break;
				case 11:
					Color = "magenta";
					break;
				case 12:
					Color = "pink";
					break;
				case 13:
					Color = "light_blue";
					break;
				case 14:
					Color = "green";
					break;
				case 15:
					Color = "silver";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Color == "cyan":
					return 0;
				case { } b when true && b.Color == "orange":
					return 1;
				case { } b when true && b.Color == "purple":
					return 2;
				case { } b when true && b.Color == "gray":
					return 3;
				case { } b when true && b.Color == "red":
					return 4;
				case { } b when true && b.Color == "lime":
					return 5;
				case { } b when true && b.Color == "blue":
					return 6;
				case { } b when true && b.Color == "brown":
					return 7;
				case { } b when true && b.Color == "white":
					return 8;
				case { } b when true && b.Color == "black":
					return 9;
				case { } b when true && b.Color == "yellow":
					return 10;
				case { } b when true && b.Color == "magenta":
					return 11;
				case { } b when true && b.Color == "pink":
					return 12;
				case { } b when true && b.Color == "light_blue":
					return 13;
				case { } b when true && b.Color == "green":
					return 14;
				case { } b when true && b.Color == "silver":
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class ConcretePowder // 237 typeof=ConcretePowder
	{
		// Convert this attribute to enum
		//[Enum("black","blue","brown","cyan","gray","green","light_blue","lime","magenta","orange","pink","purple","red","silver","white","yellow"]
		public string Color { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Color = "lime";
					break;
				case 1:
					Color = "gray";
					break;
				case 2:
					Color = "orange";
					break;
				case 3:
					Color = "purple";
					break;
				case 4:
					Color = "brown";
					break;
				case 5:
					Color = "pink";
					break;
				case 6:
					Color = "light_blue";
					break;
				case 7:
					Color = "red";
					break;
				case 8:
					Color = "green";
					break;
				case 9:
					Color = "blue";
					break;
				case 10:
					Color = "white";
					break;
				case 11:
					Color = "black";
					break;
				case 12:
					Color = "magenta";
					break;
				case 13:
					Color = "cyan";
					break;
				case 14:
					Color = "silver";
					break;
				case 15:
					Color = "yellow";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Color == "lime":
					return 0;
				case { } b when true && b.Color == "gray":
					return 1;
				case { } b when true && b.Color == "orange":
					return 2;
				case { } b when true && b.Color == "purple":
					return 3;
				case { } b when true && b.Color == "brown":
					return 4;
				case { } b when true && b.Color == "pink":
					return 5;
				case { } b when true && b.Color == "light_blue":
					return 6;
				case { } b when true && b.Color == "red":
					return 7;
				case { } b when true && b.Color == "green":
					return 8;
				case { } b when true && b.Color == "blue":
					return 9;
				case { } b when true && b.Color == "white":
					return 10;
				case { } b when true && b.Color == "black":
					return 11;
				case { } b when true && b.Color == "magenta":
					return 12;
				case { } b when true && b.Color == "cyan":
					return 13;
				case { } b when true && b.Color == "silver":
					return 14;
				case { } b when true && b.Color == "yellow":
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Conduit : Block // 0 typeof=Block
	{
		public Conduit() : base(412)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Coral : Block // 0 typeof=Block
	{
		// Convert this attribute to enum
		//[Enum("blue","pink","purple","red","yellow"]
		public string CoralColor { get; set; }
		[Range(0, 1)] public byte DeadBit { get; set; }

		public Coral() : base(386)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					CoralColor = "yellow";
					DeadBit = 0;
					break;
				case 1:
					CoralColor = "blue";
					DeadBit = 1;
					break;
				case 2:
					CoralColor = "yellow";
					DeadBit = 1;
					break;
				case 3:
					CoralColor = "red";
					DeadBit = 1;
					break;
				case 4:
					CoralColor = "red";
					DeadBit = 0;
					break;
				case 5:
					CoralColor = "purple";
					DeadBit = 0;
					break;
				case 6:
					CoralColor = "pink";
					DeadBit = 1;
					break;
				case 7:
					CoralColor = "pink";
					DeadBit = 0;
					break;
				case 8:
					CoralColor = "purple";
					DeadBit = 1;
					break;
				case 9:
					CoralColor = "blue";
					DeadBit = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.CoralColor == "yellow" && b.DeadBit == 0:
					return 0;
				case { } b when true && b.CoralColor == "blue" && b.DeadBit == 1:
					return 1;
				case { } b when true && b.CoralColor == "yellow" && b.DeadBit == 1:
					return 2;
				case { } b when true && b.CoralColor == "red" && b.DeadBit == 1:
					return 3;
				case { } b when true && b.CoralColor == "red" && b.DeadBit == 0:
					return 4;
				case { } b when true && b.CoralColor == "purple" && b.DeadBit == 0:
					return 5;
				case { } b when true && b.CoralColor == "pink" && b.DeadBit == 1:
					return 6;
				case { } b when true && b.CoralColor == "pink" && b.DeadBit == 0:
					return 7;
				case { } b when true && b.CoralColor == "purple" && b.DeadBit == 1:
					return 8;
				case { } b when true && b.CoralColor == "blue" && b.DeadBit == 0:
					return 9;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class CoralBlock : Block // 0 typeof=Block
	{
		// Convert this attribute to enum
		//[Enum("blue","pink","purple","red","yellow"]
		public string CoralColor { get; set; }
		[Range(0, 1)] public byte DeadBit { get; set; }

		public CoralBlock() : base(387)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					CoralColor = "purple";
					DeadBit = 1;
					break;
				case 1:
					CoralColor = "yellow";
					DeadBit = 1;
					break;
				case 2:
					CoralColor = "pink";
					DeadBit = 0;
					break;
				case 3:
					CoralColor = "red";
					DeadBit = 0;
					break;
				case 4:
					CoralColor = "pink";
					DeadBit = 1;
					break;
				case 5:
					CoralColor = "purple";
					DeadBit = 0;
					break;
				case 6:
					CoralColor = "blue";
					DeadBit = 1;
					break;
				case 7:
					CoralColor = "blue";
					DeadBit = 0;
					break;
				case 8:
					CoralColor = "red";
					DeadBit = 1;
					break;
				case 9:
					CoralColor = "yellow";
					DeadBit = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.CoralColor == "purple" && b.DeadBit == 1:
					return 0;
				case { } b when true && b.CoralColor == "yellow" && b.DeadBit == 1:
					return 1;
				case { } b when true && b.CoralColor == "pink" && b.DeadBit == 0:
					return 2;
				case { } b when true && b.CoralColor == "red" && b.DeadBit == 0:
					return 3;
				case { } b when true && b.CoralColor == "pink" && b.DeadBit == 1:
					return 4;
				case { } b when true && b.CoralColor == "purple" && b.DeadBit == 0:
					return 5;
				case { } b when true && b.CoralColor == "blue" && b.DeadBit == 1:
					return 6;
				case { } b when true && b.CoralColor == "blue" && b.DeadBit == 0:
					return 7;
				case { } b when true && b.CoralColor == "red" && b.DeadBit == 1:
					return 8;
				case { } b when true && b.CoralColor == "yellow" && b.DeadBit == 0:
					return 9;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class CoralFan : Block // 0 typeof=Block
	{
		// Convert this attribute to enum
		//[Enum("blue","pink","purple","red","yellow"]
		public string CoralColor { get; set; }
		[Range(0, 1)] public int CoralFanDirection { get; set; }

		public CoralFan() : base(388)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					CoralColor = "yellow";
					CoralFanDirection = 0;
					break;
				case 1:
					CoralColor = "red";
					CoralFanDirection = 1;
					break;
				case 2:
					CoralColor = "pink";
					CoralFanDirection = 0;
					break;
				case 3:
					CoralColor = "yellow";
					CoralFanDirection = 1;
					break;
				case 4:
					CoralColor = "purple";
					CoralFanDirection = 1;
					break;
				case 5:
					CoralColor = "red";
					CoralFanDirection = 0;
					break;
				case 6:
					CoralColor = "purple";
					CoralFanDirection = 0;
					break;
				case 7:
					CoralColor = "pink";
					CoralFanDirection = 1;
					break;
				case 8:
					CoralColor = "blue";
					CoralFanDirection = 0;
					break;
				case 9:
					CoralColor = "blue";
					CoralFanDirection = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.CoralColor == "yellow" && b.CoralFanDirection == 0:
					return 0;
				case { } b when true && b.CoralColor == "red" && b.CoralFanDirection == 1:
					return 1;
				case { } b when true && b.CoralColor == "pink" && b.CoralFanDirection == 0:
					return 2;
				case { } b when true && b.CoralColor == "yellow" && b.CoralFanDirection == 1:
					return 3;
				case { } b when true && b.CoralColor == "purple" && b.CoralFanDirection == 1:
					return 4;
				case { } b when true && b.CoralColor == "red" && b.CoralFanDirection == 0:
					return 5;
				case { } b when true && b.CoralColor == "purple" && b.CoralFanDirection == 0:
					return 6;
				case { } b when true && b.CoralColor == "pink" && b.CoralFanDirection == 1:
					return 7;
				case { } b when true && b.CoralColor == "blue" && b.CoralFanDirection == 0:
					return 8;
				case { } b when true && b.CoralColor == "blue" && b.CoralFanDirection == 1:
					return 9;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class CoralFanDead : Block // 0 typeof=Block
	{
		// Convert this attribute to enum
		//[Enum("blue","pink","purple","red","yellow"]
		public string CoralColor { get; set; }
		[Range(0, 1)] public int CoralFanDirection { get; set; }

		public CoralFanDead() : base(389)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					CoralColor = "pink";
					CoralFanDirection = 0;
					break;
				case 1:
					CoralColor = "purple";
					CoralFanDirection = 1;
					break;
				case 2:
					CoralColor = "red";
					CoralFanDirection = 1;
					break;
				case 3:
					CoralColor = "red";
					CoralFanDirection = 0;
					break;
				case 4:
					CoralColor = "blue";
					CoralFanDirection = 0;
					break;
				case 5:
					CoralColor = "blue";
					CoralFanDirection = 1;
					break;
				case 6:
					CoralColor = "pink";
					CoralFanDirection = 1;
					break;
				case 7:
					CoralColor = "yellow";
					CoralFanDirection = 0;
					break;
				case 8:
					CoralColor = "purple";
					CoralFanDirection = 0;
					break;
				case 9:
					CoralColor = "yellow";
					CoralFanDirection = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.CoralColor == "pink" && b.CoralFanDirection == 0:
					return 0;
				case { } b when true && b.CoralColor == "purple" && b.CoralFanDirection == 1:
					return 1;
				case { } b when true && b.CoralColor == "red" && b.CoralFanDirection == 1:
					return 2;
				case { } b when true && b.CoralColor == "red" && b.CoralFanDirection == 0:
					return 3;
				case { } b when true && b.CoralColor == "blue" && b.CoralFanDirection == 0:
					return 4;
				case { } b when true && b.CoralColor == "blue" && b.CoralFanDirection == 1:
					return 5;
				case { } b when true && b.CoralColor == "pink" && b.CoralFanDirection == 1:
					return 6;
				case { } b when true && b.CoralColor == "yellow" && b.CoralFanDirection == 0:
					return 7;
				case { } b when true && b.CoralColor == "purple" && b.CoralFanDirection == 0:
					return 8;
				case { } b when true && b.CoralColor == "yellow" && b.CoralFanDirection == 1:
					return 9;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class CoralFanHang : Block // 0 typeof=Block
	{
		[Range(0, 3)] public int CoralDirection { get; set; }
		[Range(0, 1)] public byte CoralHangTypeBit { get; set; }
		[Range(0, 1)] public byte DeadBit { get; set; }

		public CoralFanHang() : base(390)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					CoralDirection = 1;
					CoralHangTypeBit = 0;
					DeadBit = 1;
					break;
				case 1:
					CoralDirection = 0;
					CoralHangTypeBit = 1;
					DeadBit = 0;
					break;
				case 2:
					CoralDirection = 2;
					CoralHangTypeBit = 1;
					DeadBit = 0;
					break;
				case 3:
					CoralDirection = 1;
					CoralHangTypeBit = 1;
					DeadBit = 1;
					break;
				case 4:
					CoralDirection = 1;
					CoralHangTypeBit = 1;
					DeadBit = 0;
					break;
				case 5:
					CoralDirection = 2;
					CoralHangTypeBit = 0;
					DeadBit = 1;
					break;
				case 6:
					CoralDirection = 3;
					CoralHangTypeBit = 0;
					DeadBit = 1;
					break;
				case 7:
					CoralDirection = 2;
					CoralHangTypeBit = 1;
					DeadBit = 1;
					break;
				case 8:
					CoralDirection = 3;
					CoralHangTypeBit = 1;
					DeadBit = 1;
					break;
				case 9:
					CoralDirection = 2;
					CoralHangTypeBit = 0;
					DeadBit = 0;
					break;
				case 10:
					CoralDirection = 0;
					CoralHangTypeBit = 1;
					DeadBit = 1;
					break;
				case 11:
					CoralDirection = 3;
					CoralHangTypeBit = 1;
					DeadBit = 0;
					break;
				case 12:
					CoralDirection = 3;
					CoralHangTypeBit = 0;
					DeadBit = 0;
					break;
				case 13:
					CoralDirection = 1;
					CoralHangTypeBit = 0;
					DeadBit = 0;
					break;
				case 14:
					CoralDirection = 0;
					CoralHangTypeBit = 0;
					DeadBit = 0;
					break;
				case 15:
					CoralDirection = 0;
					CoralHangTypeBit = 0;
					DeadBit = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.CoralDirection == 1 && b.CoralHangTypeBit == 0 && b.DeadBit == 1:
					return 0;
				case { } b when true && b.CoralDirection == 0 && b.CoralHangTypeBit == 1 && b.DeadBit == 0:
					return 1;
				case { } b when true && b.CoralDirection == 2 && b.CoralHangTypeBit == 1 && b.DeadBit == 0:
					return 2;
				case { } b when true && b.CoralDirection == 1 && b.CoralHangTypeBit == 1 && b.DeadBit == 1:
					return 3;
				case { } b when true && b.CoralDirection == 1 && b.CoralHangTypeBit == 1 && b.DeadBit == 0:
					return 4;
				case { } b when true && b.CoralDirection == 2 && b.CoralHangTypeBit == 0 && b.DeadBit == 1:
					return 5;
				case { } b when true && b.CoralDirection == 3 && b.CoralHangTypeBit == 0 && b.DeadBit == 1:
					return 6;
				case { } b when true && b.CoralDirection == 2 && b.CoralHangTypeBit == 1 && b.DeadBit == 1:
					return 7;
				case { } b when true && b.CoralDirection == 3 && b.CoralHangTypeBit == 1 && b.DeadBit == 1:
					return 8;
				case { } b when true && b.CoralDirection == 2 && b.CoralHangTypeBit == 0 && b.DeadBit == 0:
					return 9;
				case { } b when true && b.CoralDirection == 0 && b.CoralHangTypeBit == 1 && b.DeadBit == 1:
					return 10;
				case { } b when true && b.CoralDirection == 3 && b.CoralHangTypeBit == 1 && b.DeadBit == 0:
					return 11;
				case { } b when true && b.CoralDirection == 3 && b.CoralHangTypeBit == 0 && b.DeadBit == 0:
					return 12;
				case { } b when true && b.CoralDirection == 1 && b.CoralHangTypeBit == 0 && b.DeadBit == 0:
					return 13;
				case { } b when true && b.CoralDirection == 0 && b.CoralHangTypeBit == 0 && b.DeadBit == 0:
					return 14;
				case { } b when true && b.CoralDirection == 0 && b.CoralHangTypeBit == 0 && b.DeadBit == 1:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class CoralFanHang2 : Block // 0 typeof=Block
	{
		[Range(0, 3)] public int CoralDirection { get; set; }
		[Range(0, 1)] public byte CoralHangTypeBit { get; set; }
		[Range(0, 1)] public byte DeadBit { get; set; }

		public CoralFanHang2() : base(391)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					CoralDirection = 2;
					CoralHangTypeBit = 0;
					DeadBit = 1;
					break;
				case 1:
					CoralDirection = 2;
					CoralHangTypeBit = 0;
					DeadBit = 0;
					break;
				case 2:
					CoralDirection = 0;
					CoralHangTypeBit = 1;
					DeadBit = 0;
					break;
				case 3:
					CoralDirection = 3;
					CoralHangTypeBit = 1;
					DeadBit = 0;
					break;
				case 4:
					CoralDirection = 1;
					CoralHangTypeBit = 0;
					DeadBit = 0;
					break;
				case 5:
					CoralDirection = 1;
					CoralHangTypeBit = 0;
					DeadBit = 1;
					break;
				case 6:
					CoralDirection = 1;
					CoralHangTypeBit = 1;
					DeadBit = 1;
					break;
				case 7:
					CoralDirection = 0;
					CoralHangTypeBit = 0;
					DeadBit = 1;
					break;
				case 8:
					CoralDirection = 3;
					CoralHangTypeBit = 0;
					DeadBit = 0;
					break;
				case 9:
					CoralDirection = 2;
					CoralHangTypeBit = 1;
					DeadBit = 1;
					break;
				case 10:
					CoralDirection = 3;
					CoralHangTypeBit = 1;
					DeadBit = 1;
					break;
				case 11:
					CoralDirection = 0;
					CoralHangTypeBit = 1;
					DeadBit = 1;
					break;
				case 12:
					CoralDirection = 1;
					CoralHangTypeBit = 1;
					DeadBit = 0;
					break;
				case 13:
					CoralDirection = 2;
					CoralHangTypeBit = 1;
					DeadBit = 0;
					break;
				case 14:
					CoralDirection = 3;
					CoralHangTypeBit = 0;
					DeadBit = 1;
					break;
				case 15:
					CoralDirection = 0;
					CoralHangTypeBit = 0;
					DeadBit = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.CoralDirection == 2 && b.CoralHangTypeBit == 0 && b.DeadBit == 1:
					return 0;
				case { } b when true && b.CoralDirection == 2 && b.CoralHangTypeBit == 0 && b.DeadBit == 0:
					return 1;
				case { } b when true && b.CoralDirection == 0 && b.CoralHangTypeBit == 1 && b.DeadBit == 0:
					return 2;
				case { } b when true && b.CoralDirection == 3 && b.CoralHangTypeBit == 1 && b.DeadBit == 0:
					return 3;
				case { } b when true && b.CoralDirection == 1 && b.CoralHangTypeBit == 0 && b.DeadBit == 0:
					return 4;
				case { } b when true && b.CoralDirection == 1 && b.CoralHangTypeBit == 0 && b.DeadBit == 1:
					return 5;
				case { } b when true && b.CoralDirection == 1 && b.CoralHangTypeBit == 1 && b.DeadBit == 1:
					return 6;
				case { } b when true && b.CoralDirection == 0 && b.CoralHangTypeBit == 0 && b.DeadBit == 1:
					return 7;
				case { } b when true && b.CoralDirection == 3 && b.CoralHangTypeBit == 0 && b.DeadBit == 0:
					return 8;
				case { } b when true && b.CoralDirection == 2 && b.CoralHangTypeBit == 1 && b.DeadBit == 1:
					return 9;
				case { } b when true && b.CoralDirection == 3 && b.CoralHangTypeBit == 1 && b.DeadBit == 1:
					return 10;
				case { } b when true && b.CoralDirection == 0 && b.CoralHangTypeBit == 1 && b.DeadBit == 1:
					return 11;
				case { } b when true && b.CoralDirection == 1 && b.CoralHangTypeBit == 1 && b.DeadBit == 0:
					return 12;
				case { } b when true && b.CoralDirection == 2 && b.CoralHangTypeBit == 1 && b.DeadBit == 0:
					return 13;
				case { } b when true && b.CoralDirection == 3 && b.CoralHangTypeBit == 0 && b.DeadBit == 1:
					return 14;
				case { } b when true && b.CoralDirection == 0 && b.CoralHangTypeBit == 0 && b.DeadBit == 0:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class CoralFanHang3 : Block // 0 typeof=Block
	{
		[Range(0, 3)] public int CoralDirection { get; set; }
		[Range(0, 1)] public byte CoralHangTypeBit { get; set; }
		[Range(0, 1)] public byte DeadBit { get; set; }

		public CoralFanHang3() : base(392)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					CoralDirection = 2;
					CoralHangTypeBit = 1;
					DeadBit = 1;
					break;
				case 1:
					CoralDirection = 3;
					CoralHangTypeBit = 0;
					DeadBit = 0;
					break;
				case 2:
					CoralDirection = 3;
					CoralHangTypeBit = 1;
					DeadBit = 1;
					break;
				case 3:
					CoralDirection = 1;
					CoralHangTypeBit = 0;
					DeadBit = 1;
					break;
				case 4:
					CoralDirection = 0;
					CoralHangTypeBit = 0;
					DeadBit = 1;
					break;
				case 5:
					CoralDirection = 3;
					CoralHangTypeBit = 1;
					DeadBit = 0;
					break;
				case 6:
					CoralDirection = 1;
					CoralHangTypeBit = 1;
					DeadBit = 1;
					break;
				case 7:
					CoralDirection = 0;
					CoralHangTypeBit = 1;
					DeadBit = 0;
					break;
				case 8:
					CoralDirection = 1;
					CoralHangTypeBit = 1;
					DeadBit = 0;
					break;
				case 9:
					CoralDirection = 3;
					CoralHangTypeBit = 0;
					DeadBit = 1;
					break;
				case 10:
					CoralDirection = 1;
					CoralHangTypeBit = 0;
					DeadBit = 0;
					break;
				case 11:
					CoralDirection = 2;
					CoralHangTypeBit = 1;
					DeadBit = 0;
					break;
				case 12:
					CoralDirection = 0;
					CoralHangTypeBit = 1;
					DeadBit = 1;
					break;
				case 13:
					CoralDirection = 2;
					CoralHangTypeBit = 0;
					DeadBit = 1;
					break;
				case 14:
					CoralDirection = 2;
					CoralHangTypeBit = 0;
					DeadBit = 0;
					break;
				case 15:
					CoralDirection = 0;
					CoralHangTypeBit = 0;
					DeadBit = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.CoralDirection == 2 && b.CoralHangTypeBit == 1 && b.DeadBit == 1:
					return 0;
				case { } b when true && b.CoralDirection == 3 && b.CoralHangTypeBit == 0 && b.DeadBit == 0:
					return 1;
				case { } b when true && b.CoralDirection == 3 && b.CoralHangTypeBit == 1 && b.DeadBit == 1:
					return 2;
				case { } b when true && b.CoralDirection == 1 && b.CoralHangTypeBit == 0 && b.DeadBit == 1:
					return 3;
				case { } b when true && b.CoralDirection == 0 && b.CoralHangTypeBit == 0 && b.DeadBit == 1:
					return 4;
				case { } b when true && b.CoralDirection == 3 && b.CoralHangTypeBit == 1 && b.DeadBit == 0:
					return 5;
				case { } b when true && b.CoralDirection == 1 && b.CoralHangTypeBit == 1 && b.DeadBit == 1:
					return 6;
				case { } b when true && b.CoralDirection == 0 && b.CoralHangTypeBit == 1 && b.DeadBit == 0:
					return 7;
				case { } b when true && b.CoralDirection == 1 && b.CoralHangTypeBit == 1 && b.DeadBit == 0:
					return 8;
				case { } b when true && b.CoralDirection == 3 && b.CoralHangTypeBit == 0 && b.DeadBit == 1:
					return 9;
				case { } b when true && b.CoralDirection == 1 && b.CoralHangTypeBit == 0 && b.DeadBit == 0:
					return 10;
				case { } b when true && b.CoralDirection == 2 && b.CoralHangTypeBit == 1 && b.DeadBit == 0:
					return 11;
				case { } b when true && b.CoralDirection == 0 && b.CoralHangTypeBit == 1 && b.DeadBit == 1:
					return 12;
				case { } b when true && b.CoralDirection == 2 && b.CoralHangTypeBit == 0 && b.DeadBit == 1:
					return 13;
				case { } b when true && b.CoralDirection == 2 && b.CoralHangTypeBit == 0 && b.DeadBit == 0:
					return 14;
				case { } b when true && b.CoralDirection == 0 && b.CoralHangTypeBit == 0 && b.DeadBit == 0:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class CraftingTable // 58 typeof=CraftingTable
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class CyanGlazedTerracotta // 229 typeof=CyanGlazedTerracotta
	{
		[Range(0, 5)] public int FacingDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					FacingDirection = 0;
					break;
				case 1:
					FacingDirection = 4;
					break;
				case 2:
					FacingDirection = 1;
					break;
				case 3:
					FacingDirection = 3;
					break;
				case 4:
					FacingDirection = 2;
					break;
				case 5:
					FacingDirection = 5;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.FacingDirection == 0:
					return 0;
				case { } b when true && b.FacingDirection == 4:
					return 1;
				case { } b when true && b.FacingDirection == 1:
					return 2;
				case { } b when true && b.FacingDirection == 3:
					return 3;
				case { } b when true && b.FacingDirection == 2:
					return 4;
				case { } b when true && b.FacingDirection == 5:
					return 5;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class DarkOakButton : Block // 0 typeof=Block
	{
		[Range(0, 1)] public byte ButtonPressedBit { get; set; }
		[Range(0, 5)] public int FacingDirection { get; set; }

		public DarkOakButton() : base(397)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					ButtonPressedBit = 0;
					FacingDirection = 4;
					break;
				case 1:
					ButtonPressedBit = 0;
					FacingDirection = 0;
					break;
				case 2:
					ButtonPressedBit = 1;
					FacingDirection = 4;
					break;
				case 3:
					ButtonPressedBit = 0;
					FacingDirection = 2;
					break;
				case 4:
					ButtonPressedBit = 1;
					FacingDirection = 2;
					break;
				case 5:
					ButtonPressedBit = 1;
					FacingDirection = 1;
					break;
				case 6:
					ButtonPressedBit = 0;
					FacingDirection = 3;
					break;
				case 7:
					ButtonPressedBit = 1;
					FacingDirection = 0;
					break;
				case 8:
					ButtonPressedBit = 1;
					FacingDirection = 5;
					break;
				case 9:
					ButtonPressedBit = 0;
					FacingDirection = 1;
					break;
				case 10:
					ButtonPressedBit = 1;
					FacingDirection = 3;
					break;
				case 11:
					ButtonPressedBit = 0;
					FacingDirection = 5;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.ButtonPressedBit == 0 && b.FacingDirection == 4:
					return 0;
				case { } b when true && b.ButtonPressedBit == 0 && b.FacingDirection == 0:
					return 1;
				case { } b when true && b.ButtonPressedBit == 1 && b.FacingDirection == 4:
					return 2;
				case { } b when true && b.ButtonPressedBit == 0 && b.FacingDirection == 2:
					return 3;
				case { } b when true && b.ButtonPressedBit == 1 && b.FacingDirection == 2:
					return 4;
				case { } b when true && b.ButtonPressedBit == 1 && b.FacingDirection == 1:
					return 5;
				case { } b when true && b.ButtonPressedBit == 0 && b.FacingDirection == 3:
					return 6;
				case { } b when true && b.ButtonPressedBit == 1 && b.FacingDirection == 0:
					return 7;
				case { } b when true && b.ButtonPressedBit == 1 && b.FacingDirection == 5:
					return 8;
				case { } b when true && b.ButtonPressedBit == 0 && b.FacingDirection == 1:
					return 9;
				case { } b when true && b.ButtonPressedBit == 1 && b.FacingDirection == 3:
					return 10;
				case { } b when true && b.ButtonPressedBit == 0 && b.FacingDirection == 5:
					return 11;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class DarkOakDoor // 197 typeof=DarkOakDoor
	{
		[Range(0, 3)] public int Direction { get; set; }
		[Range(0, 1)] public byte DoorHingeBit { get; set; }
		[Range(0, 1)] public byte OpenBit { get; set; }
		[Range(0, 1)] public byte UpperBlockBit { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Direction = 2;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 1:
					Direction = 1;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 2:
					Direction = 3;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 3:
					Direction = 2;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 4:
					Direction = 1;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 5:
					Direction = 3;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 6:
					Direction = 0;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 7:
					Direction = 0;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 8:
					Direction = 1;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 9:
					Direction = 2;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 10:
					Direction = 3;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 11:
					Direction = 0;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 12:
					Direction = 1;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 13:
					Direction = 1;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 14:
					Direction = 3;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 15:
					Direction = 0;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 16:
					Direction = 2;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 17:
					Direction = 0;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 18:
					Direction = 3;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 19:
					Direction = 2;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 20:
					Direction = 3;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 21:
					Direction = 1;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 22:
					Direction = 2;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 23:
					Direction = 3;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 24:
					Direction = 0;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 25:
					Direction = 0;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 26:
					Direction = 0;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 27:
					Direction = 2;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 28:
					Direction = 3;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 29:
					Direction = 1;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 30:
					Direction = 1;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 31:
					Direction = 2;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 0;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 1;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 2;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 3;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 4;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 5;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 6;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 7;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 8;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 9;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 10;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 11;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 12;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 13;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 14;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 15;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 16;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 17;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 18;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 19;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 20;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 21;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 22;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 23;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 24;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 25;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 26;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 27;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 28;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 29;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 30;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 31;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class DarkOakFenceGate // 186 typeof=DarkOakFenceGate
	{
		[Range(0, 3)] public int Direction { get; set; }
		[Range(0, 1)] public byte InWallBit { get; set; }
		[Range(0, 1)] public byte OpenBit { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Direction = 2;
					InWallBit = 1;
					OpenBit = 0;
					break;
				case 1:
					Direction = 3;
					InWallBit = 0;
					OpenBit = 0;
					break;
				case 2:
					Direction = 1;
					InWallBit = 0;
					OpenBit = 1;
					break;
				case 3:
					Direction = 1;
					InWallBit = 1;
					OpenBit = 0;
					break;
				case 4:
					Direction = 3;
					InWallBit = 1;
					OpenBit = 1;
					break;
				case 5:
					Direction = 2;
					InWallBit = 0;
					OpenBit = 0;
					break;
				case 6:
					Direction = 0;
					InWallBit = 0;
					OpenBit = 1;
					break;
				case 7:
					Direction = 3;
					InWallBit = 1;
					OpenBit = 0;
					break;
				case 8:
					Direction = 0;
					InWallBit = 0;
					OpenBit = 0;
					break;
				case 9:
					Direction = 0;
					InWallBit = 1;
					OpenBit = 1;
					break;
				case 10:
					Direction = 1;
					InWallBit = 0;
					OpenBit = 0;
					break;
				case 11:
					Direction = 0;
					InWallBit = 1;
					OpenBit = 0;
					break;
				case 12:
					Direction = 2;
					InWallBit = 1;
					OpenBit = 1;
					break;
				case 13:
					Direction = 2;
					InWallBit = 0;
					OpenBit = 1;
					break;
				case 14:
					Direction = 3;
					InWallBit = 0;
					OpenBit = 1;
					break;
				case 15:
					Direction = 1;
					InWallBit = 1;
					OpenBit = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Direction == 2 && b.InWallBit == 1 && b.OpenBit == 0:
					return 0;
				case { } b when true && b.Direction == 3 && b.InWallBit == 0 && b.OpenBit == 0:
					return 1;
				case { } b when true && b.Direction == 1 && b.InWallBit == 0 && b.OpenBit == 1:
					return 2;
				case { } b when true && b.Direction == 1 && b.InWallBit == 1 && b.OpenBit == 0:
					return 3;
				case { } b when true && b.Direction == 3 && b.InWallBit == 1 && b.OpenBit == 1:
					return 4;
				case { } b when true && b.Direction == 2 && b.InWallBit == 0 && b.OpenBit == 0:
					return 5;
				case { } b when true && b.Direction == 0 && b.InWallBit == 0 && b.OpenBit == 1:
					return 6;
				case { } b when true && b.Direction == 3 && b.InWallBit == 1 && b.OpenBit == 0:
					return 7;
				case { } b when true && b.Direction == 0 && b.InWallBit == 0 && b.OpenBit == 0:
					return 8;
				case { } b when true && b.Direction == 0 && b.InWallBit == 1 && b.OpenBit == 1:
					return 9;
				case { } b when true && b.Direction == 1 && b.InWallBit == 0 && b.OpenBit == 0:
					return 10;
				case { } b when true && b.Direction == 0 && b.InWallBit == 1 && b.OpenBit == 0:
					return 11;
				case { } b when true && b.Direction == 2 && b.InWallBit == 1 && b.OpenBit == 1:
					return 12;
				case { } b when true && b.Direction == 2 && b.InWallBit == 0 && b.OpenBit == 1:
					return 13;
				case { } b when true && b.Direction == 3 && b.InWallBit == 0 && b.OpenBit == 1:
					return 14;
				case { } b when true && b.Direction == 1 && b.InWallBit == 1 && b.OpenBit == 1:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class DarkOakPressurePlate : Block // 0 typeof=Block
	{
		[Range(0, 9)] public int RedstoneSignal { get; set; }

		public DarkOakPressurePlate() : base(407)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					RedstoneSignal = 0;
					break;
				case 1:
					RedstoneSignal = 1;
					break;
				case 2:
					RedstoneSignal = 12;
					break;
				case 3:
					RedstoneSignal = 4;
					break;
				case 4:
					RedstoneSignal = 5;
					break;
				case 5:
					RedstoneSignal = 2;
					break;
				case 6:
					RedstoneSignal = 11;
					break;
				case 7:
					RedstoneSignal = 3;
					break;
				case 8:
					RedstoneSignal = 13;
					break;
				case 9:
					RedstoneSignal = 8;
					break;
				case 10:
					RedstoneSignal = 6;
					break;
				case 11:
					RedstoneSignal = 10;
					break;
				case 12:
					RedstoneSignal = 14;
					break;
				case 13:
					RedstoneSignal = 9;
					break;
				case 14:
					RedstoneSignal = 7;
					break;
				case 15:
					RedstoneSignal = 15;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.RedstoneSignal == 0:
					return 0;
				case { } b when true && b.RedstoneSignal == 1:
					return 1;
				case { } b when true && b.RedstoneSignal == 12:
					return 2;
				case { } b when true && b.RedstoneSignal == 4:
					return 3;
				case { } b when true && b.RedstoneSignal == 5:
					return 4;
				case { } b when true && b.RedstoneSignal == 2:
					return 5;
				case { } b when true && b.RedstoneSignal == 11:
					return 6;
				case { } b when true && b.RedstoneSignal == 3:
					return 7;
				case { } b when true && b.RedstoneSignal == 13:
					return 8;
				case { } b when true && b.RedstoneSignal == 8:
					return 9;
				case { } b when true && b.RedstoneSignal == 6:
					return 10;
				case { } b when true && b.RedstoneSignal == 10:
					return 11;
				case { } b when true && b.RedstoneSignal == 14:
					return 12;
				case { } b when true && b.RedstoneSignal == 9:
					return 13;
				case { } b when true && b.RedstoneSignal == 7:
					return 14;
				case { } b when true && b.RedstoneSignal == 15:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class DarkOakStairs // 164 typeof=DarkOakStairs
	{
		[Range(0, 1)] public byte UpsideDownBit { get; set; }
		[Range(0, 3)] public int WeirdoDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					UpsideDownBit = 1;
					WeirdoDirection = 3;
					break;
				case 1:
					UpsideDownBit = 0;
					WeirdoDirection = 1;
					break;
				case 2:
					UpsideDownBit = 0;
					WeirdoDirection = 0;
					break;
				case 3:
					UpsideDownBit = 0;
					WeirdoDirection = 3;
					break;
				case 4:
					UpsideDownBit = 1;
					WeirdoDirection = 1;
					break;
				case 5:
					UpsideDownBit = 1;
					WeirdoDirection = 0;
					break;
				case 6:
					UpsideDownBit = 0;
					WeirdoDirection = 2;
					break;
				case 7:
					UpsideDownBit = 1;
					WeirdoDirection = 2;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 3:
					return 0;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 1:
					return 1;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 0:
					return 2;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 3:
					return 3;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 1:
					return 4;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 0:
					return 5;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 2:
					return 6;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 2:
					return 7;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class DarkOakTrapdoor : Block // 0 typeof=Block
	{
		[Range(0, 3)] public int Direction { get; set; }
		[Range(0, 1)] public byte OpenBit { get; set; }
		[Range(0, 1)] public byte UpsideDownBit { get; set; }

		public DarkOakTrapdoor() : base(402)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Direction = 3;
					OpenBit = 0;
					UpsideDownBit = 0;
					break;
				case 1:
					Direction = 2;
					OpenBit = 0;
					UpsideDownBit = 0;
					break;
				case 2:
					Direction = 1;
					OpenBit = 0;
					UpsideDownBit = 0;
					break;
				case 3:
					Direction = 3;
					OpenBit = 1;
					UpsideDownBit = 1;
					break;
				case 4:
					Direction = 3;
					OpenBit = 1;
					UpsideDownBit = 0;
					break;
				case 5:
					Direction = 2;
					OpenBit = 0;
					UpsideDownBit = 1;
					break;
				case 6:
					Direction = 1;
					OpenBit = 0;
					UpsideDownBit = 1;
					break;
				case 7:
					Direction = 0;
					OpenBit = 1;
					UpsideDownBit = 0;
					break;
				case 8:
					Direction = 2;
					OpenBit = 1;
					UpsideDownBit = 0;
					break;
				case 9:
					Direction = 0;
					OpenBit = 0;
					UpsideDownBit = 1;
					break;
				case 10:
					Direction = 0;
					OpenBit = 1;
					UpsideDownBit = 1;
					break;
				case 11:
					Direction = 2;
					OpenBit = 1;
					UpsideDownBit = 1;
					break;
				case 12:
					Direction = 1;
					OpenBit = 1;
					UpsideDownBit = 1;
					break;
				case 13:
					Direction = 1;
					OpenBit = 1;
					UpsideDownBit = 0;
					break;
				case 14:
					Direction = 3;
					OpenBit = 0;
					UpsideDownBit = 1;
					break;
				case 15:
					Direction = 0;
					OpenBit = 0;
					UpsideDownBit = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Direction == 3 && b.OpenBit == 0 && b.UpsideDownBit == 0:
					return 0;
				case { } b when true && b.Direction == 2 && b.OpenBit == 0 && b.UpsideDownBit == 0:
					return 1;
				case { } b when true && b.Direction == 1 && b.OpenBit == 0 && b.UpsideDownBit == 0:
					return 2;
				case { } b when true && b.Direction == 3 && b.OpenBit == 1 && b.UpsideDownBit == 1:
					return 3;
				case { } b when true && b.Direction == 3 && b.OpenBit == 1 && b.UpsideDownBit == 0:
					return 4;
				case { } b when true && b.Direction == 2 && b.OpenBit == 0 && b.UpsideDownBit == 1:
					return 5;
				case { } b when true && b.Direction == 1 && b.OpenBit == 0 && b.UpsideDownBit == 1:
					return 6;
				case { } b when true && b.Direction == 0 && b.OpenBit == 1 && b.UpsideDownBit == 0:
					return 7;
				case { } b when true && b.Direction == 2 && b.OpenBit == 1 && b.UpsideDownBit == 0:
					return 8;
				case { } b when true && b.Direction == 0 && b.OpenBit == 0 && b.UpsideDownBit == 1:
					return 9;
				case { } b when true && b.Direction == 0 && b.OpenBit == 1 && b.UpsideDownBit == 1:
					return 10;
				case { } b when true && b.Direction == 2 && b.OpenBit == 1 && b.UpsideDownBit == 1:
					return 11;
				case { } b when true && b.Direction == 1 && b.OpenBit == 1 && b.UpsideDownBit == 1:
					return 12;
				case { } b when true && b.Direction == 1 && b.OpenBit == 1 && b.UpsideDownBit == 0:
					return 13;
				case { } b when true && b.Direction == 3 && b.OpenBit == 0 && b.UpsideDownBit == 1:
					return 14;
				case { } b when true && b.Direction == 0 && b.OpenBit == 0 && b.UpsideDownBit == 0:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class DarkPrismarineStairs : Block // 0 typeof=Block
	{
		[Range(0, 1)] public byte UpsideDownBit { get; set; }
		[Range(0, 3)] public int WeirdoDirection { get; set; }

		public DarkPrismarineStairs() : base(258)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					UpsideDownBit = 0;
					WeirdoDirection = 1;
					break;
				case 1:
					UpsideDownBit = 0;
					WeirdoDirection = 3;
					break;
				case 2:
					UpsideDownBit = 1;
					WeirdoDirection = 1;
					break;
				case 3:
					UpsideDownBit = 1;
					WeirdoDirection = 2;
					break;
				case 4:
					UpsideDownBit = 0;
					WeirdoDirection = 2;
					break;
				case 5:
					UpsideDownBit = 0;
					WeirdoDirection = 0;
					break;
				case 6:
					UpsideDownBit = 1;
					WeirdoDirection = 3;
					break;
				case 7:
					UpsideDownBit = 1;
					WeirdoDirection = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 1:
					return 0;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 3:
					return 1;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 1:
					return 2;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 2:
					return 3;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 2:
					return 4;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 0:
					return 5;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 3:
					return 6;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 0:
					return 7;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class DarkoakStandingSign : Block // 0 typeof=Block
	{
		[Range(0, 9)] public int GroundSignDirection { get; set; }

		public DarkoakStandingSign() : base(447)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					GroundSignDirection = 11;
					break;
				case 1:
					GroundSignDirection = 9;
					break;
				case 2:
					GroundSignDirection = 0;
					break;
				case 3:
					GroundSignDirection = 2;
					break;
				case 4:
					GroundSignDirection = 5;
					break;
				case 5:
					GroundSignDirection = 10;
					break;
				case 6:
					GroundSignDirection = 14;
					break;
				case 7:
					GroundSignDirection = 7;
					break;
				case 8:
					GroundSignDirection = 6;
					break;
				case 9:
					GroundSignDirection = 15;
					break;
				case 10:
					GroundSignDirection = 1;
					break;
				case 11:
					GroundSignDirection = 4;
					break;
				case 12:
					GroundSignDirection = 13;
					break;
				case 13:
					GroundSignDirection = 3;
					break;
				case 14:
					GroundSignDirection = 12;
					break;
				case 15:
					GroundSignDirection = 8;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.GroundSignDirection == 11:
					return 0;
				case { } b when true && b.GroundSignDirection == 9:
					return 1;
				case { } b when true && b.GroundSignDirection == 0:
					return 2;
				case { } b when true && b.GroundSignDirection == 2:
					return 3;
				case { } b when true && b.GroundSignDirection == 5:
					return 4;
				case { } b when true && b.GroundSignDirection == 10:
					return 5;
				case { } b when true && b.GroundSignDirection == 14:
					return 6;
				case { } b when true && b.GroundSignDirection == 7:
					return 7;
				case { } b when true && b.GroundSignDirection == 6:
					return 8;
				case { } b when true && b.GroundSignDirection == 15:
					return 9;
				case { } b when true && b.GroundSignDirection == 1:
					return 10;
				case { } b when true && b.GroundSignDirection == 4:
					return 11;
				case { } b when true && b.GroundSignDirection == 13:
					return 12;
				case { } b when true && b.GroundSignDirection == 3:
					return 13;
				case { } b when true && b.GroundSignDirection == 12:
					return 14;
				case { } b when true && b.GroundSignDirection == 8:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class DarkoakWallSign : Block // 0 typeof=Block
	{
		[Range(0, 5)] public int FacingDirection { get; set; }

		public DarkoakWallSign() : base(448)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					FacingDirection = 4;
					break;
				case 1:
					FacingDirection = 3;
					break;
				case 2:
					FacingDirection = 5;
					break;
				case 3:
					FacingDirection = 1;
					break;
				case 4:
					FacingDirection = 2;
					break;
				case 5:
					FacingDirection = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.FacingDirection == 4:
					return 0;
				case { } b when true && b.FacingDirection == 3:
					return 1;
				case { } b when true && b.FacingDirection == 5:
					return 2;
				case { } b when true && b.FacingDirection == 1:
					return 3;
				case { } b when true && b.FacingDirection == 2:
					return 4;
				case { } b when true && b.FacingDirection == 0:
					return 5;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class DaylightDetector // 151 typeof=DaylightDetector
	{
		[Range(0, 9)] public int RedstoneSignal { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					RedstoneSignal = 15;
					break;
				case 1:
					RedstoneSignal = 4;
					break;
				case 2:
					RedstoneSignal = 7;
					break;
				case 3:
					RedstoneSignal = 12;
					break;
				case 4:
					RedstoneSignal = 2;
					break;
				case 5:
					RedstoneSignal = 11;
					break;
				case 6:
					RedstoneSignal = 1;
					break;
				case 7:
					RedstoneSignal = 10;
					break;
				case 8:
					RedstoneSignal = 0;
					break;
				case 9:
					RedstoneSignal = 3;
					break;
				case 10:
					RedstoneSignal = 9;
					break;
				case 11:
					RedstoneSignal = 13;
					break;
				case 12:
					RedstoneSignal = 5;
					break;
				case 13:
					RedstoneSignal = 14;
					break;
				case 14:
					RedstoneSignal = 8;
					break;
				case 15:
					RedstoneSignal = 6;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.RedstoneSignal == 15:
					return 0;
				case { } b when true && b.RedstoneSignal == 4:
					return 1;
				case { } b when true && b.RedstoneSignal == 7:
					return 2;
				case { } b when true && b.RedstoneSignal == 12:
					return 3;
				case { } b when true && b.RedstoneSignal == 2:
					return 4;
				case { } b when true && b.RedstoneSignal == 11:
					return 5;
				case { } b when true && b.RedstoneSignal == 1:
					return 6;
				case { } b when true && b.RedstoneSignal == 10:
					return 7;
				case { } b when true && b.RedstoneSignal == 0:
					return 8;
				case { } b when true && b.RedstoneSignal == 3:
					return 9;
				case { } b when true && b.RedstoneSignal == 9:
					return 10;
				case { } b when true && b.RedstoneSignal == 13:
					return 11;
				case { } b when true && b.RedstoneSignal == 5:
					return 12;
				case { } b when true && b.RedstoneSignal == 14:
					return 13;
				case { } b when true && b.RedstoneSignal == 8:
					return 14;
				case { } b when true && b.RedstoneSignal == 6:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class DaylightDetectorInverted // 178 typeof=DaylightDetectorInverted
	{
		[Range(0, 9)] public int RedstoneSignal { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					RedstoneSignal = 13;
					break;
				case 1:
					RedstoneSignal = 10;
					break;
				case 2:
					RedstoneSignal = 2;
					break;
				case 3:
					RedstoneSignal = 11;
					break;
				case 4:
					RedstoneSignal = 6;
					break;
				case 5:
					RedstoneSignal = 12;
					break;
				case 6:
					RedstoneSignal = 8;
					break;
				case 7:
					RedstoneSignal = 9;
					break;
				case 8:
					RedstoneSignal = 7;
					break;
				case 9:
					RedstoneSignal = 15;
					break;
				case 10:
					RedstoneSignal = 0;
					break;
				case 11:
					RedstoneSignal = 14;
					break;
				case 12:
					RedstoneSignal = 5;
					break;
				case 13:
					RedstoneSignal = 4;
					break;
				case 14:
					RedstoneSignal = 1;
					break;
				case 15:
					RedstoneSignal = 3;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.RedstoneSignal == 13:
					return 0;
				case { } b when true && b.RedstoneSignal == 10:
					return 1;
				case { } b when true && b.RedstoneSignal == 2:
					return 2;
				case { } b when true && b.RedstoneSignal == 11:
					return 3;
				case { } b when true && b.RedstoneSignal == 6:
					return 4;
				case { } b when true && b.RedstoneSignal == 12:
					return 5;
				case { } b when true && b.RedstoneSignal == 8:
					return 6;
				case { } b when true && b.RedstoneSignal == 9:
					return 7;
				case { } b when true && b.RedstoneSignal == 7:
					return 8;
				case { } b when true && b.RedstoneSignal == 15:
					return 9;
				case { } b when true && b.RedstoneSignal == 0:
					return 10;
				case { } b when true && b.RedstoneSignal == 14:
					return 11;
				case { } b when true && b.RedstoneSignal == 5:
					return 12;
				case { } b when true && b.RedstoneSignal == 4:
					return 13;
				case { } b when true && b.RedstoneSignal == 1:
					return 14;
				case { } b when true && b.RedstoneSignal == 3:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Deadbush // 32 typeof=DeadBush
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class DetectorRail // 28 typeof=DetectorRail
	{
		[Range(0, 1)] public byte RailDataBit { get; set; }
		[Range(0, 5)] public int RailDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					RailDataBit = 1;
					RailDirection = 4;
					break;
				case 1:
					RailDataBit = 0;
					RailDirection = 0;
					break;
				case 2:
					RailDataBit = 0;
					RailDirection = 3;
					break;
				case 3:
					RailDataBit = 0;
					RailDirection = 5;
					break;
				case 4:
					RailDataBit = 1;
					RailDirection = 2;
					break;
				case 5:
					RailDataBit = 1;
					RailDirection = 0;
					break;
				case 6:
					RailDataBit = 0;
					RailDirection = 2;
					break;
				case 7:
					RailDataBit = 1;
					RailDirection = 5;
					break;
				case 8:
					RailDataBit = 1;
					RailDirection = 3;
					break;
				case 9:
					RailDataBit = 0;
					RailDirection = 4;
					break;
				case 10:
					RailDataBit = 0;
					RailDirection = 1;
					break;
				case 11:
					RailDataBit = 1;
					RailDirection = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.RailDataBit == 1 && b.RailDirection == 4:
					return 0;
				case { } b when true && b.RailDataBit == 0 && b.RailDirection == 0:
					return 1;
				case { } b when true && b.RailDataBit == 0 && b.RailDirection == 3:
					return 2;
				case { } b when true && b.RailDataBit == 0 && b.RailDirection == 5:
					return 3;
				case { } b when true && b.RailDataBit == 1 && b.RailDirection == 2:
					return 4;
				case { } b when true && b.RailDataBit == 1 && b.RailDirection == 0:
					return 5;
				case { } b when true && b.RailDataBit == 0 && b.RailDirection == 2:
					return 6;
				case { } b when true && b.RailDataBit == 1 && b.RailDirection == 5:
					return 7;
				case { } b when true && b.RailDataBit == 1 && b.RailDirection == 3:
					return 8;
				case { } b when true && b.RailDataBit == 0 && b.RailDirection == 4:
					return 9;
				case { } b when true && b.RailDataBit == 0 && b.RailDirection == 1:
					return 10;
				case { } b when true && b.RailDataBit == 1 && b.RailDirection == 1:
					return 11;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class DiamondBlock // 57 typeof=DiamondBlock
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class DiamondOre // 56 typeof=DiamondOre
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class DioriteStairs : Block // 0 typeof=Block
	{
		[Range(0, 1)] public byte UpsideDownBit { get; set; }
		[Range(0, 3)] public int WeirdoDirection { get; set; }

		public DioriteStairs() : base(425)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					UpsideDownBit = 1;
					WeirdoDirection = 1;
					break;
				case 1:
					UpsideDownBit = 1;
					WeirdoDirection = 0;
					break;
				case 2:
					UpsideDownBit = 1;
					WeirdoDirection = 3;
					break;
				case 3:
					UpsideDownBit = 0;
					WeirdoDirection = 0;
					break;
				case 4:
					UpsideDownBit = 0;
					WeirdoDirection = 3;
					break;
				case 5:
					UpsideDownBit = 1;
					WeirdoDirection = 2;
					break;
				case 6:
					UpsideDownBit = 0;
					WeirdoDirection = 2;
					break;
				case 7:
					UpsideDownBit = 0;
					WeirdoDirection = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 1:
					return 0;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 0:
					return 1;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 3:
					return 2;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 0:
					return 3;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 3:
					return 4;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 2:
					return 5;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 2:
					return 6;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 1:
					return 7;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Dirt // 3 typeof=Dirt
	{
		// Convert this attribute to enum
		//[Enum("coarse","normal"]
		public string DirtType { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					DirtType = "coarse";
					break;
				case 1:
					DirtType = "normal";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.DirtType == "coarse":
					return 0;
				case { } b when true && b.DirtType == "normal":
					return 1;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Dispenser // 23 typeof=Dispenser
	{
		[Range(0, 5)] public int FacingDirection { get; set; }
		[Range(0, 1)] public byte TriggeredBit { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					FacingDirection = 1;
					TriggeredBit = 1;
					break;
				case 1:
					FacingDirection = 0;
					TriggeredBit = 0;
					break;
				case 2:
					FacingDirection = 2;
					TriggeredBit = 0;
					break;
				case 3:
					FacingDirection = 1;
					TriggeredBit = 0;
					break;
				case 4:
					FacingDirection = 0;
					TriggeredBit = 1;
					break;
				case 5:
					FacingDirection = 4;
					TriggeredBit = 0;
					break;
				case 6:
					FacingDirection = 5;
					TriggeredBit = 0;
					break;
				case 7:
					FacingDirection = 2;
					TriggeredBit = 1;
					break;
				case 8:
					FacingDirection = 4;
					TriggeredBit = 1;
					break;
				case 9:
					FacingDirection = 5;
					TriggeredBit = 1;
					break;
				case 10:
					FacingDirection = 3;
					TriggeredBit = 0;
					break;
				case 11:
					FacingDirection = 3;
					TriggeredBit = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.FacingDirection == 1 && b.TriggeredBit == 1:
					return 0;
				case { } b when true && b.FacingDirection == 0 && b.TriggeredBit == 0:
					return 1;
				case { } b when true && b.FacingDirection == 2 && b.TriggeredBit == 0:
					return 2;
				case { } b when true && b.FacingDirection == 1 && b.TriggeredBit == 0:
					return 3;
				case { } b when true && b.FacingDirection == 0 && b.TriggeredBit == 1:
					return 4;
				case { } b when true && b.FacingDirection == 4 && b.TriggeredBit == 0:
					return 5;
				case { } b when true && b.FacingDirection == 5 && b.TriggeredBit == 0:
					return 6;
				case { } b when true && b.FacingDirection == 2 && b.TriggeredBit == 1:
					return 7;
				case { } b when true && b.FacingDirection == 4 && b.TriggeredBit == 1:
					return 8;
				case { } b when true && b.FacingDirection == 5 && b.TriggeredBit == 1:
					return 9;
				case { } b when true && b.FacingDirection == 3 && b.TriggeredBit == 0:
					return 10;
				case { } b when true && b.FacingDirection == 3 && b.TriggeredBit == 1:
					return 11;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class DoublePlant // 175 typeof=DoublePlant
	{
		// Convert this attribute to enum
		//[Enum("fern","grass","paeonia","rose","sunflower","syringa"]
		public string DoublePlantType { get; set; }
		[Range(0, 1)] public byte UpperBlockBit { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					DoublePlantType = "sunflower";
					UpperBlockBit = 0;
					break;
				case 1:
					DoublePlantType = "rose";
					UpperBlockBit = 0;
					break;
				case 2:
					DoublePlantType = "syringa";
					UpperBlockBit = 0;
					break;
				case 3:
					DoublePlantType = "grass";
					UpperBlockBit = 0;
					break;
				case 4:
					DoublePlantType = "fern";
					UpperBlockBit = 1;
					break;
				case 5:
					DoublePlantType = "grass";
					UpperBlockBit = 1;
					break;
				case 6:
					DoublePlantType = "paeonia";
					UpperBlockBit = 1;
					break;
				case 7:
					DoublePlantType = "rose";
					UpperBlockBit = 1;
					break;
				case 8:
					DoublePlantType = "paeonia";
					UpperBlockBit = 0;
					break;
				case 9:
					DoublePlantType = "fern";
					UpperBlockBit = 0;
					break;
				case 10:
					DoublePlantType = "sunflower";
					UpperBlockBit = 1;
					break;
				case 11:
					DoublePlantType = "syringa";
					UpperBlockBit = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.DoublePlantType == "sunflower" && b.UpperBlockBit == 0:
					return 0;
				case { } b when true && b.DoublePlantType == "rose" && b.UpperBlockBit == 0:
					return 1;
				case { } b when true && b.DoublePlantType == "syringa" && b.UpperBlockBit == 0:
					return 2;
				case { } b when true && b.DoublePlantType == "grass" && b.UpperBlockBit == 0:
					return 3;
				case { } b when true && b.DoublePlantType == "fern" && b.UpperBlockBit == 1:
					return 4;
				case { } b when true && b.DoublePlantType == "grass" && b.UpperBlockBit == 1:
					return 5;
				case { } b when true && b.DoublePlantType == "paeonia" && b.UpperBlockBit == 1:
					return 6;
				case { } b when true && b.DoublePlantType == "rose" && b.UpperBlockBit == 1:
					return 7;
				case { } b when true && b.DoublePlantType == "paeonia" && b.UpperBlockBit == 0:
					return 8;
				case { } b when true && b.DoublePlantType == "fern" && b.UpperBlockBit == 0:
					return 9;
				case { } b when true && b.DoublePlantType == "sunflower" && b.UpperBlockBit == 1:
					return 10;
				case { } b when true && b.DoublePlantType == "syringa" && b.UpperBlockBit == 1:
					return 11;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class DoubleStoneSlab // 43 typeof=DoubleStoneSlab
	{
		// Convert this attribute to enum
		//[Enum("brick","cobblestone","nether_brick","quartz","sandstone","smooth_stone","stone_brick","wood"]
		public string StoneSlabType { get; set; }
		[Range(0, 1)] public byte TopSlotBit { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					StoneSlabType = "smooth_stone";
					TopSlotBit = 0;
					break;
				case 1:
					StoneSlabType = "cobblestone";
					TopSlotBit = 1;
					break;
				case 2:
					StoneSlabType = "stone_brick";
					TopSlotBit = 1;
					break;
				case 3:
					StoneSlabType = "quartz";
					TopSlotBit = 0;
					break;
				case 4:
					StoneSlabType = "quartz";
					TopSlotBit = 1;
					break;
				case 5:
					StoneSlabType = "nether_brick";
					TopSlotBit = 1;
					break;
				case 6:
					StoneSlabType = "wood";
					TopSlotBit = 1;
					break;
				case 7:
					StoneSlabType = "wood";
					TopSlotBit = 0;
					break;
				case 8:
					StoneSlabType = "sandstone";
					TopSlotBit = 0;
					break;
				case 9:
					StoneSlabType = "smooth_stone";
					TopSlotBit = 1;
					break;
				case 10:
					StoneSlabType = "brick";
					TopSlotBit = 0;
					break;
				case 11:
					StoneSlabType = "stone_brick";
					TopSlotBit = 0;
					break;
				case 12:
					StoneSlabType = "nether_brick";
					TopSlotBit = 0;
					break;
				case 13:
					StoneSlabType = "brick";
					TopSlotBit = 1;
					break;
				case 14:
					StoneSlabType = "cobblestone";
					TopSlotBit = 0;
					break;
				case 15:
					StoneSlabType = "sandstone";
					TopSlotBit = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.StoneSlabType == "smooth_stone" && b.TopSlotBit == 0:
					return 0;
				case { } b when true && b.StoneSlabType == "cobblestone" && b.TopSlotBit == 1:
					return 1;
				case { } b when true && b.StoneSlabType == "stone_brick" && b.TopSlotBit == 1:
					return 2;
				case { } b when true && b.StoneSlabType == "quartz" && b.TopSlotBit == 0:
					return 3;
				case { } b when true && b.StoneSlabType == "quartz" && b.TopSlotBit == 1:
					return 4;
				case { } b when true && b.StoneSlabType == "nether_brick" && b.TopSlotBit == 1:
					return 5;
				case { } b when true && b.StoneSlabType == "wood" && b.TopSlotBit == 1:
					return 6;
				case { } b when true && b.StoneSlabType == "wood" && b.TopSlotBit == 0:
					return 7;
				case { } b when true && b.StoneSlabType == "sandstone" && b.TopSlotBit == 0:
					return 8;
				case { } b when true && b.StoneSlabType == "smooth_stone" && b.TopSlotBit == 1:
					return 9;
				case { } b when true && b.StoneSlabType == "brick" && b.TopSlotBit == 0:
					return 10;
				case { } b when true && b.StoneSlabType == "stone_brick" && b.TopSlotBit == 0:
					return 11;
				case { } b when true && b.StoneSlabType == "nether_brick" && b.TopSlotBit == 0:
					return 12;
				case { } b when true && b.StoneSlabType == "brick" && b.TopSlotBit == 1:
					return 13;
				case { } b when true && b.StoneSlabType == "cobblestone" && b.TopSlotBit == 0:
					return 14;
				case { } b when true && b.StoneSlabType == "sandstone" && b.TopSlotBit == 1:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class DoubleStoneSlab2 // 181 typeof=DoubleStoneSlab2
	{
		// Convert this attribute to enum
		//[Enum("mossy_cobblestone","prismarine_brick","prismarine_dark","prismarine_rough","purpur","red_nether_brick","red_sandstone","smooth_sandstone"]
		public string StoneSlabType2 { get; set; }
		[Range(0, 1)] public byte TopSlotBit { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					StoneSlabType2 = "prismarine_brick";
					TopSlotBit = 1;
					break;
				case 1:
					StoneSlabType2 = "red_sandstone";
					TopSlotBit = 0;
					break;
				case 2:
					StoneSlabType2 = "red_sandstone";
					TopSlotBit = 1;
					break;
				case 3:
					StoneSlabType2 = "purpur";
					TopSlotBit = 0;
					break;
				case 4:
					StoneSlabType2 = "red_nether_brick";
					TopSlotBit = 0;
					break;
				case 5:
					StoneSlabType2 = "prismarine_rough";
					TopSlotBit = 1;
					break;
				case 6:
					StoneSlabType2 = "prismarine_rough";
					TopSlotBit = 0;
					break;
				case 7:
					StoneSlabType2 = "mossy_cobblestone";
					TopSlotBit = 0;
					break;
				case 8:
					StoneSlabType2 = "prismarine_dark";
					TopSlotBit = 1;
					break;
				case 9:
					StoneSlabType2 = "mossy_cobblestone";
					TopSlotBit = 1;
					break;
				case 10:
					StoneSlabType2 = "red_nether_brick";
					TopSlotBit = 1;
					break;
				case 11:
					StoneSlabType2 = "prismarine_dark";
					TopSlotBit = 0;
					break;
				case 12:
					StoneSlabType2 = "purpur";
					TopSlotBit = 1;
					break;
				case 13:
					StoneSlabType2 = "smooth_sandstone";
					TopSlotBit = 0;
					break;
				case 14:
					StoneSlabType2 = "prismarine_brick";
					TopSlotBit = 0;
					break;
				case 15:
					StoneSlabType2 = "smooth_sandstone";
					TopSlotBit = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.StoneSlabType2 == "prismarine_brick" && b.TopSlotBit == 1:
					return 0;
				case { } b when true && b.StoneSlabType2 == "red_sandstone" && b.TopSlotBit == 0:
					return 1;
				case { } b when true && b.StoneSlabType2 == "red_sandstone" && b.TopSlotBit == 1:
					return 2;
				case { } b when true && b.StoneSlabType2 == "purpur" && b.TopSlotBit == 0:
					return 3;
				case { } b when true && b.StoneSlabType2 == "red_nether_brick" && b.TopSlotBit == 0:
					return 4;
				case { } b when true && b.StoneSlabType2 == "prismarine_rough" && b.TopSlotBit == 1:
					return 5;
				case { } b when true && b.StoneSlabType2 == "prismarine_rough" && b.TopSlotBit == 0:
					return 6;
				case { } b when true && b.StoneSlabType2 == "mossy_cobblestone" && b.TopSlotBit == 0:
					return 7;
				case { } b when true && b.StoneSlabType2 == "prismarine_dark" && b.TopSlotBit == 1:
					return 8;
				case { } b when true && b.StoneSlabType2 == "mossy_cobblestone" && b.TopSlotBit == 1:
					return 9;
				case { } b when true && b.StoneSlabType2 == "red_nether_brick" && b.TopSlotBit == 1:
					return 10;
				case { } b when true && b.StoneSlabType2 == "prismarine_dark" && b.TopSlotBit == 0:
					return 11;
				case { } b when true && b.StoneSlabType2 == "purpur" && b.TopSlotBit == 1:
					return 12;
				case { } b when true && b.StoneSlabType2 == "smooth_sandstone" && b.TopSlotBit == 0:
					return 13;
				case { } b when true && b.StoneSlabType2 == "prismarine_brick" && b.TopSlotBit == 0:
					return 14;
				case { } b when true && b.StoneSlabType2 == "smooth_sandstone" && b.TopSlotBit == 1:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class DoubleStoneSlab3 : Block // 0 typeof=Block
	{
		// Convert this attribute to enum
		//[Enum("andesite","diorite","end_stone_brick","granite","polished_andesite","polished_diorite","polished_granite","smooth_red_sandstone"]
		public string StoneSlabType3 { get; set; }
		[Range(0, 1)] public byte TopSlotBit { get; set; }

		public DoubleStoneSlab3() : base(422)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					StoneSlabType3 = "diorite";
					TopSlotBit = 1;
					break;
				case 1:
					StoneSlabType3 = "polished_andesite";
					TopSlotBit = 0;
					break;
				case 2:
					StoneSlabType3 = "polished_andesite";
					TopSlotBit = 1;
					break;
				case 3:
					StoneSlabType3 = "andesite";
					TopSlotBit = 1;
					break;
				case 4:
					StoneSlabType3 = "diorite";
					TopSlotBit = 0;
					break;
				case 5:
					StoneSlabType3 = "polished_diorite";
					TopSlotBit = 1;
					break;
				case 6:
					StoneSlabType3 = "granite";
					TopSlotBit = 0;
					break;
				case 7:
					StoneSlabType3 = "smooth_red_sandstone";
					TopSlotBit = 0;
					break;
				case 8:
					StoneSlabType3 = "polished_granite";
					TopSlotBit = 1;
					break;
				case 9:
					StoneSlabType3 = "polished_granite";
					TopSlotBit = 0;
					break;
				case 10:
					StoneSlabType3 = "andesite";
					TopSlotBit = 0;
					break;
				case 11:
					StoneSlabType3 = "end_stone_brick";
					TopSlotBit = 1;
					break;
				case 12:
					StoneSlabType3 = "end_stone_brick";
					TopSlotBit = 0;
					break;
				case 13:
					StoneSlabType3 = "polished_diorite";
					TopSlotBit = 0;
					break;
				case 14:
					StoneSlabType3 = "granite";
					TopSlotBit = 1;
					break;
				case 15:
					StoneSlabType3 = "smooth_red_sandstone";
					TopSlotBit = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.StoneSlabType3 == "diorite" && b.TopSlotBit == 1:
					return 0;
				case { } b when true && b.StoneSlabType3 == "polished_andesite" && b.TopSlotBit == 0:
					return 1;
				case { } b when true && b.StoneSlabType3 == "polished_andesite" && b.TopSlotBit == 1:
					return 2;
				case { } b when true && b.StoneSlabType3 == "andesite" && b.TopSlotBit == 1:
					return 3;
				case { } b when true && b.StoneSlabType3 == "diorite" && b.TopSlotBit == 0:
					return 4;
				case { } b when true && b.StoneSlabType3 == "polished_diorite" && b.TopSlotBit == 1:
					return 5;
				case { } b when true && b.StoneSlabType3 == "granite" && b.TopSlotBit == 0:
					return 6;
				case { } b when true && b.StoneSlabType3 == "smooth_red_sandstone" && b.TopSlotBit == 0:
					return 7;
				case { } b when true && b.StoneSlabType3 == "polished_granite" && b.TopSlotBit == 1:
					return 8;
				case { } b when true && b.StoneSlabType3 == "polished_granite" && b.TopSlotBit == 0:
					return 9;
				case { } b when true && b.StoneSlabType3 == "andesite" && b.TopSlotBit == 0:
					return 10;
				case { } b when true && b.StoneSlabType3 == "end_stone_brick" && b.TopSlotBit == 1:
					return 11;
				case { } b when true && b.StoneSlabType3 == "end_stone_brick" && b.TopSlotBit == 0:
					return 12;
				case { } b when true && b.StoneSlabType3 == "polished_diorite" && b.TopSlotBit == 0:
					return 13;
				case { } b when true && b.StoneSlabType3 == "granite" && b.TopSlotBit == 1:
					return 14;
				case { } b when true && b.StoneSlabType3 == "smooth_red_sandstone" && b.TopSlotBit == 1:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class DoubleStoneSlab4 : Block // 0 typeof=Block
	{
		// Convert this attribute to enum
		//[Enum("cut_red_sandstone","cut_sandstone","mossy_stone_brick","smooth_quartz","stone"]
		public string StoneSlabType4 { get; set; }
		[Range(0, 1)] public byte TopSlotBit { get; set; }

		public DoubleStoneSlab4() : base(423)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					StoneSlabType4 = "cut_red_sandstone";
					TopSlotBit = 0;
					break;
				case 1:
					StoneSlabType4 = "mossy_stone_brick";
					TopSlotBit = 1;
					break;
				case 2:
					StoneSlabType4 = "cut_sandstone";
					TopSlotBit = 1;
					break;
				case 3:
					StoneSlabType4 = "cut_sandstone";
					TopSlotBit = 0;
					break;
				case 4:
					StoneSlabType4 = "stone";
					TopSlotBit = 0;
					break;
				case 5:
					StoneSlabType4 = "smooth_quartz";
					TopSlotBit = 0;
					break;
				case 6:
					StoneSlabType4 = "cut_red_sandstone";
					TopSlotBit = 1;
					break;
				case 7:
					StoneSlabType4 = "smooth_quartz";
					TopSlotBit = 1;
					break;
				case 8:
					StoneSlabType4 = "mossy_stone_brick";
					TopSlotBit = 0;
					break;
				case 9:
					StoneSlabType4 = "stone";
					TopSlotBit = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.StoneSlabType4 == "cut_red_sandstone" && b.TopSlotBit == 0:
					return 0;
				case { } b when true && b.StoneSlabType4 == "mossy_stone_brick" && b.TopSlotBit == 1:
					return 1;
				case { } b when true && b.StoneSlabType4 == "cut_sandstone" && b.TopSlotBit == 1:
					return 2;
				case { } b when true && b.StoneSlabType4 == "cut_sandstone" && b.TopSlotBit == 0:
					return 3;
				case { } b when true && b.StoneSlabType4 == "stone" && b.TopSlotBit == 0:
					return 4;
				case { } b when true && b.StoneSlabType4 == "smooth_quartz" && b.TopSlotBit == 0:
					return 5;
				case { } b when true && b.StoneSlabType4 == "cut_red_sandstone" && b.TopSlotBit == 1:
					return 6;
				case { } b when true && b.StoneSlabType4 == "smooth_quartz" && b.TopSlotBit == 1:
					return 7;
				case { } b when true && b.StoneSlabType4 == "mossy_stone_brick" && b.TopSlotBit == 0:
					return 8;
				case { } b when true && b.StoneSlabType4 == "stone" && b.TopSlotBit == 1:
					return 9;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class DoubleWoodenSlab // 157 typeof=DoubleWoodenSlab
	{
		[Range(0, 1)] public byte TopSlotBit { get; set; }

		// Convert this attribute to enum
		//[Enum("acacia","birch","dark_oak","jungle","oak","spruce"]
		public string WoodType { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					TopSlotBit = 0;
					WoodType = "oak";
					break;
				case 1:
					TopSlotBit = 0;
					WoodType = "acacia";
					break;
				case 2:
					TopSlotBit = 0;
					WoodType = "jungle";
					break;
				case 3:
					TopSlotBit = 0;
					WoodType = "birch";
					break;
				case 4:
					TopSlotBit = 1;
					WoodType = "acacia";
					break;
				case 5:
					TopSlotBit = 0;
					WoodType = "dark_oak";
					break;
				case 6:
					TopSlotBit = 1;
					WoodType = "oak";
					break;
				case 7:
					TopSlotBit = 1;
					WoodType = "spruce";
					break;
				case 8:
					TopSlotBit = 1;
					WoodType = "birch";
					break;
				case 9:
					TopSlotBit = 1;
					WoodType = "dark_oak";
					break;
				case 10:
					TopSlotBit = 0;
					WoodType = "spruce";
					break;
				case 11:
					TopSlotBit = 1;
					WoodType = "jungle";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.TopSlotBit == 0 && b.WoodType == "oak":
					return 0;
				case { } b when true && b.TopSlotBit == 0 && b.WoodType == "acacia":
					return 1;
				case { } b when true && b.TopSlotBit == 0 && b.WoodType == "jungle":
					return 2;
				case { } b when true && b.TopSlotBit == 0 && b.WoodType == "birch":
					return 3;
				case { } b when true && b.TopSlotBit == 1 && b.WoodType == "acacia":
					return 4;
				case { } b when true && b.TopSlotBit == 0 && b.WoodType == "dark_oak":
					return 5;
				case { } b when true && b.TopSlotBit == 1 && b.WoodType == "oak":
					return 6;
				case { } b when true && b.TopSlotBit == 1 && b.WoodType == "spruce":
					return 7;
				case { } b when true && b.TopSlotBit == 1 && b.WoodType == "birch":
					return 8;
				case { } b when true && b.TopSlotBit == 1 && b.WoodType == "dark_oak":
					return 9;
				case { } b when true && b.TopSlotBit == 0 && b.WoodType == "spruce":
					return 10;
				case { } b when true && b.TopSlotBit == 1 && b.WoodType == "jungle":
					return 11;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class DragonEgg // 122 typeof=DragonEgg
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class DriedKelpBlock : Block // 0 typeof=Block
	{
		public DriedKelpBlock() : base(394)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Dropper // 125 typeof=Dropper
	{
		[Range(0, 5)] public int FacingDirection { get; set; }
		[Range(0, 1)] public byte TriggeredBit { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					FacingDirection = 2;
					TriggeredBit = 0;
					break;
				case 1:
					FacingDirection = 3;
					TriggeredBit = 0;
					break;
				case 2:
					FacingDirection = 1;
					TriggeredBit = 0;
					break;
				case 3:
					FacingDirection = 0;
					TriggeredBit = 1;
					break;
				case 4:
					FacingDirection = 4;
					TriggeredBit = 0;
					break;
				case 5:
					FacingDirection = 2;
					TriggeredBit = 1;
					break;
				case 6:
					FacingDirection = 1;
					TriggeredBit = 1;
					break;
				case 7:
					FacingDirection = 3;
					TriggeredBit = 1;
					break;
				case 8:
					FacingDirection = 4;
					TriggeredBit = 1;
					break;
				case 9:
					FacingDirection = 5;
					TriggeredBit = 0;
					break;
				case 10:
					FacingDirection = 0;
					TriggeredBit = 0;
					break;
				case 11:
					FacingDirection = 5;
					TriggeredBit = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.FacingDirection == 2 && b.TriggeredBit == 0:
					return 0;
				case { } b when true && b.FacingDirection == 3 && b.TriggeredBit == 0:
					return 1;
				case { } b when true && b.FacingDirection == 1 && b.TriggeredBit == 0:
					return 2;
				case { } b when true && b.FacingDirection == 0 && b.TriggeredBit == 1:
					return 3;
				case { } b when true && b.FacingDirection == 4 && b.TriggeredBit == 0:
					return 4;
				case { } b when true && b.FacingDirection == 2 && b.TriggeredBit == 1:
					return 5;
				case { } b when true && b.FacingDirection == 1 && b.TriggeredBit == 1:
					return 6;
				case { } b when true && b.FacingDirection == 3 && b.TriggeredBit == 1:
					return 7;
				case { } b when true && b.FacingDirection == 4 && b.TriggeredBit == 1:
					return 8;
				case { } b when true && b.FacingDirection == 5 && b.TriggeredBit == 0:
					return 9;
				case { } b when true && b.FacingDirection == 0 && b.TriggeredBit == 0:
					return 10;
				case { } b when true && b.FacingDirection == 5 && b.TriggeredBit == 1:
					return 11;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element0 : Block // 0 typeof=Block
	{
		public Element0() : base(36)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element1 : Block // 0 typeof=Block
	{
		public Element1() : base(267)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element10 : Block // 0 typeof=Block
	{
		public Element10() : base(276)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element100 : Block // 0 typeof=Block
	{
		public Element100() : base(366)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element101 : Block // 0 typeof=Block
	{
		public Element101() : base(367)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element102 : Block // 0 typeof=Block
	{
		public Element102() : base(368)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element103 : Block // 0 typeof=Block
	{
		public Element103() : base(369)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element104 : Block // 0 typeof=Block
	{
		public Element104() : base(370)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element105 : Block // 0 typeof=Block
	{
		public Element105() : base(371)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element106 : Block // 0 typeof=Block
	{
		public Element106() : base(372)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element107 : Block // 0 typeof=Block
	{
		public Element107() : base(373)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element108 : Block // 0 typeof=Block
	{
		public Element108() : base(374)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element109 : Block // 0 typeof=Block
	{
		public Element109() : base(375)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element11 : Block // 0 typeof=Block
	{
		public Element11() : base(277)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element110 : Block // 0 typeof=Block
	{
		public Element110() : base(376)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element111 : Block // 0 typeof=Block
	{
		public Element111() : base(377)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element112 : Block // 0 typeof=Block
	{
		public Element112() : base(378)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element113 : Block // 0 typeof=Block
	{
		public Element113() : base(379)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element114 : Block // 0 typeof=Block
	{
		public Element114() : base(380)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element115 : Block // 0 typeof=Block
	{
		public Element115() : base(381)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element116 : Block // 0 typeof=Block
	{
		public Element116() : base(382)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element117 : Block // 0 typeof=Block
	{
		public Element117() : base(383)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element118 : Block // 0 typeof=Block
	{
		public Element118() : base(384)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element12 : Block // 0 typeof=Block
	{
		public Element12() : base(278)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element13 : Block // 0 typeof=Block
	{
		public Element13() : base(279)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element14 : Block // 0 typeof=Block
	{
		public Element14() : base(280)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element15 : Block // 0 typeof=Block
	{
		public Element15() : base(281)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element16 : Block // 0 typeof=Block
	{
		public Element16() : base(282)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element17 : Block // 0 typeof=Block
	{
		public Element17() : base(283)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element18 : Block // 0 typeof=Block
	{
		public Element18() : base(284)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element19 : Block // 0 typeof=Block
	{
		public Element19() : base(285)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element2 : Block // 0 typeof=Block
	{
		public Element2() : base(268)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element20 : Block // 0 typeof=Block
	{
		public Element20() : base(286)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element21 : Block // 0 typeof=Block
	{
		public Element21() : base(287)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element22 : Block // 0 typeof=Block
	{
		public Element22() : base(288)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element23 : Block // 0 typeof=Block
	{
		public Element23() : base(289)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element24 : Block // 0 typeof=Block
	{
		public Element24() : base(290)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element25 : Block // 0 typeof=Block
	{
		public Element25() : base(291)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element26 : Block // 0 typeof=Block
	{
		public Element26() : base(292)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element27 : Block // 0 typeof=Block
	{
		public Element27() : base(293)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element28 : Block // 0 typeof=Block
	{
		public Element28() : base(294)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element29 : Block // 0 typeof=Block
	{
		public Element29() : base(295)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element3 : Block // 0 typeof=Block
	{
		public Element3() : base(269)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element30 : Block // 0 typeof=Block
	{
		public Element30() : base(296)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element31 : Block // 0 typeof=Block
	{
		public Element31() : base(297)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element32 : Block // 0 typeof=Block
	{
		public Element32() : base(298)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element33 : Block // 0 typeof=Block
	{
		public Element33() : base(299)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element34 : Block // 0 typeof=Block
	{
		public Element34() : base(300)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element35 : Block // 0 typeof=Block
	{
		public Element35() : base(301)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element36 : Block // 0 typeof=Block
	{
		public Element36() : base(302)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element37 : Block // 0 typeof=Block
	{
		public Element37() : base(303)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element38 : Block // 0 typeof=Block
	{
		public Element38() : base(304)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element39 : Block // 0 typeof=Block
	{
		public Element39() : base(305)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element4 : Block // 0 typeof=Block
	{
		public Element4() : base(270)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element40 : Block // 0 typeof=Block
	{
		public Element40() : base(306)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element41 : Block // 0 typeof=Block
	{
		public Element41() : base(307)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element42 : Block // 0 typeof=Block
	{
		public Element42() : base(308)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element43 : Block // 0 typeof=Block
	{
		public Element43() : base(309)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element44 : Block // 0 typeof=Block
	{
		public Element44() : base(310)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element45 : Block // 0 typeof=Block
	{
		public Element45() : base(311)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element46 : Block // 0 typeof=Block
	{
		public Element46() : base(312)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element47 : Block // 0 typeof=Block
	{
		public Element47() : base(313)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element48 : Block // 0 typeof=Block
	{
		public Element48() : base(314)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element49 : Block // 0 typeof=Block
	{
		public Element49() : base(315)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element5 : Block // 0 typeof=Block
	{
		public Element5() : base(271)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element50 : Block // 0 typeof=Block
	{
		public Element50() : base(316)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element51 : Block // 0 typeof=Block
	{
		public Element51() : base(317)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element52 : Block // 0 typeof=Block
	{
		public Element52() : base(318)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element53 : Block // 0 typeof=Block
	{
		public Element53() : base(319)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element54 : Block // 0 typeof=Block
	{
		public Element54() : base(320)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element55 : Block // 0 typeof=Block
	{
		public Element55() : base(321)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element56 : Block // 0 typeof=Block
	{
		public Element56() : base(322)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element57 : Block // 0 typeof=Block
	{
		public Element57() : base(323)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element58 : Block // 0 typeof=Block
	{
		public Element58() : base(324)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element59 : Block // 0 typeof=Block
	{
		public Element59() : base(325)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element6 : Block // 0 typeof=Block
	{
		public Element6() : base(272)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element60 : Block // 0 typeof=Block
	{
		public Element60() : base(326)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element61 : Block // 0 typeof=Block
	{
		public Element61() : base(327)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element62 : Block // 0 typeof=Block
	{
		public Element62() : base(328)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element63 : Block // 0 typeof=Block
	{
		public Element63() : base(329)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element64 : Block // 0 typeof=Block
	{
		public Element64() : base(330)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element65 : Block // 0 typeof=Block
	{
		public Element65() : base(331)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element66 : Block // 0 typeof=Block
	{
		public Element66() : base(332)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element67 : Block // 0 typeof=Block
	{
		public Element67() : base(333)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element68 : Block // 0 typeof=Block
	{
		public Element68() : base(334)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element69 : Block // 0 typeof=Block
	{
		public Element69() : base(335)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element7 : Block // 0 typeof=Block
	{
		public Element7() : base(273)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element70 : Block // 0 typeof=Block
	{
		public Element70() : base(336)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element71 : Block // 0 typeof=Block
	{
		public Element71() : base(337)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element72 : Block // 0 typeof=Block
	{
		public Element72() : base(338)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element73 : Block // 0 typeof=Block
	{
		public Element73() : base(339)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element74 : Block // 0 typeof=Block
	{
		public Element74() : base(340)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element75 : Block // 0 typeof=Block
	{
		public Element75() : base(341)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element76 : Block // 0 typeof=Block
	{
		public Element76() : base(342)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element77 : Block // 0 typeof=Block
	{
		public Element77() : base(343)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element78 : Block // 0 typeof=Block
	{
		public Element78() : base(344)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element79 : Block // 0 typeof=Block
	{
		public Element79() : base(345)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element8 : Block // 0 typeof=Block
	{
		public Element8() : base(274)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element80 : Block // 0 typeof=Block
	{
		public Element80() : base(346)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element81 : Block // 0 typeof=Block
	{
		public Element81() : base(347)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element82 : Block // 0 typeof=Block
	{
		public Element82() : base(348)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element83 : Block // 0 typeof=Block
	{
		public Element83() : base(349)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element84 : Block // 0 typeof=Block
	{
		public Element84() : base(350)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element85 : Block // 0 typeof=Block
	{
		public Element85() : base(351)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element86 : Block // 0 typeof=Block
	{
		public Element86() : base(352)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element87 : Block // 0 typeof=Block
	{
		public Element87() : base(353)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element88 : Block // 0 typeof=Block
	{
		public Element88() : base(354)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element89 : Block // 0 typeof=Block
	{
		public Element89() : base(355)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element9 : Block // 0 typeof=Block
	{
		public Element9() : base(275)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element90 : Block // 0 typeof=Block
	{
		public Element90() : base(356)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element91 : Block // 0 typeof=Block
	{
		public Element91() : base(357)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element92 : Block // 0 typeof=Block
	{
		public Element92() : base(358)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element93 : Block // 0 typeof=Block
	{
		public Element93() : base(359)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element94 : Block // 0 typeof=Block
	{
		public Element94() : base(360)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element95 : Block // 0 typeof=Block
	{
		public Element95() : base(361)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element96 : Block // 0 typeof=Block
	{
		public Element96() : base(362)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element97 : Block // 0 typeof=Block
	{
		public Element97() : base(363)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element98 : Block // 0 typeof=Block
	{
		public Element98() : base(364)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Element99 : Block // 0 typeof=Block
	{
		public Element99() : base(365)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class EmeraldBlock // 133 typeof=EmeraldBlock
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class EmeraldOre // 129 typeof=EmeraldOre
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class EnchantingTable // 116 typeof=EnchantingTable
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class EndBrickStairs : Block // 0 typeof=Block
	{
		[Range(0, 1)] public byte UpsideDownBit { get; set; }
		[Range(0, 3)] public int WeirdoDirection { get; set; }

		public EndBrickStairs() : base(433)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					UpsideDownBit = 0;
					WeirdoDirection = 1;
					break;
				case 1:
					UpsideDownBit = 1;
					WeirdoDirection = 0;
					break;
				case 2:
					UpsideDownBit = 0;
					WeirdoDirection = 0;
					break;
				case 3:
					UpsideDownBit = 1;
					WeirdoDirection = 2;
					break;
				case 4:
					UpsideDownBit = 1;
					WeirdoDirection = 1;
					break;
				case 5:
					UpsideDownBit = 0;
					WeirdoDirection = 2;
					break;
				case 6:
					UpsideDownBit = 0;
					WeirdoDirection = 3;
					break;
				case 7:
					UpsideDownBit = 1;
					WeirdoDirection = 3;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 1:
					return 0;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 0:
					return 1;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 0:
					return 2;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 2:
					return 3;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 1:
					return 4;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 2:
					return 5;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 3:
					return 6;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 3:
					return 7;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class EndBricks // 206 typeof=EndBricks
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class EndGateway // 209 typeof=EndGateway
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class EndPortal // 119 typeof=EndPortal
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class EndPortalFrame // 120 typeof=EndPortalFrame
	{
		[Range(0, 3)] public int Direction { get; set; }
		[Range(0, 1)] public byte EndPortalEyeBit { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Direction = 1;
					EndPortalEyeBit = 1;
					break;
				case 1:
					Direction = 0;
					EndPortalEyeBit = 1;
					break;
				case 2:
					Direction = 2;
					EndPortalEyeBit = 1;
					break;
				case 3:
					Direction = 3;
					EndPortalEyeBit = 1;
					break;
				case 4:
					Direction = 2;
					EndPortalEyeBit = 0;
					break;
				case 5:
					Direction = 1;
					EndPortalEyeBit = 0;
					break;
				case 6:
					Direction = 3;
					EndPortalEyeBit = 0;
					break;
				case 7:
					Direction = 0;
					EndPortalEyeBit = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Direction == 1 && b.EndPortalEyeBit == 1:
					return 0;
				case { } b when true && b.Direction == 0 && b.EndPortalEyeBit == 1:
					return 1;
				case { } b when true && b.Direction == 2 && b.EndPortalEyeBit == 1:
					return 2;
				case { } b when true && b.Direction == 3 && b.EndPortalEyeBit == 1:
					return 3;
				case { } b when true && b.Direction == 2 && b.EndPortalEyeBit == 0:
					return 4;
				case { } b when true && b.Direction == 1 && b.EndPortalEyeBit == 0:
					return 5;
				case { } b when true && b.Direction == 3 && b.EndPortalEyeBit == 0:
					return 6;
				case { } b when true && b.Direction == 0 && b.EndPortalEyeBit == 0:
					return 7;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class EndRod // 208 typeof=EndRod
	{
		[Range(0, 5)] public int FacingDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					FacingDirection = 1;
					break;
				case 1:
					FacingDirection = 0;
					break;
				case 2:
					FacingDirection = 5;
					break;
				case 3:
					FacingDirection = 4;
					break;
				case 4:
					FacingDirection = 3;
					break;
				case 5:
					FacingDirection = 2;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.FacingDirection == 1:
					return 0;
				case { } b when true && b.FacingDirection == 0:
					return 1;
				case { } b when true && b.FacingDirection == 5:
					return 2;
				case { } b when true && b.FacingDirection == 4:
					return 3;
				case { } b when true && b.FacingDirection == 3:
					return 4;
				case { } b when true && b.FacingDirection == 2:
					return 5;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class EndStone // 121 typeof=EndStone
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class EnderChest // 130 typeof=EnderChest
	{
		[Range(0, 5)] public int FacingDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					FacingDirection = 2;
					break;
				case 1:
					FacingDirection = 1;
					break;
				case 2:
					FacingDirection = 5;
					break;
				case 3:
					FacingDirection = 4;
					break;
				case 4:
					FacingDirection = 0;
					break;
				case 5:
					FacingDirection = 3;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.FacingDirection == 2:
					return 0;
				case { } b when true && b.FacingDirection == 1:
					return 1;
				case { } b when true && b.FacingDirection == 5:
					return 2;
				case { } b when true && b.FacingDirection == 4:
					return 3;
				case { } b when true && b.FacingDirection == 0:
					return 4;
				case { } b when true && b.FacingDirection == 3:
					return 5;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Farmland // 60 typeof=Farmland
	{
		[Range(0, 7)] public int MoisturizedAmount { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					MoisturizedAmount = 0;
					break;
				case 1:
					MoisturizedAmount = 6;
					break;
				case 2:
					MoisturizedAmount = 1;
					break;
				case 3:
					MoisturizedAmount = 7;
					break;
				case 4:
					MoisturizedAmount = 2;
					break;
				case 5:
					MoisturizedAmount = 4;
					break;
				case 6:
					MoisturizedAmount = 3;
					break;
				case 7:
					MoisturizedAmount = 5;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.MoisturizedAmount == 0:
					return 0;
				case { } b when true && b.MoisturizedAmount == 6:
					return 1;
				case { } b when true && b.MoisturizedAmount == 1:
					return 2;
				case { } b when true && b.MoisturizedAmount == 7:
					return 3;
				case { } b when true && b.MoisturizedAmount == 2:
					return 4;
				case { } b when true && b.MoisturizedAmount == 4:
					return 5;
				case { } b when true && b.MoisturizedAmount == 3:
					return 6;
				case { } b when true && b.MoisturizedAmount == 5:
					return 7;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Fence // 85 typeof=Fence
	{
		// Convert this attribute to enum
		//[Enum("acacia","birch","dark_oak","jungle","oak","spruce"]
		public string WoodType { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					WoodType = "spruce";
					break;
				case 1:
					WoodType = "acacia";
					break;
				case 2:
					WoodType = "dark_oak";
					break;
				case 3:
					WoodType = "jungle";
					break;
				case 4:
					WoodType = "birch";
					break;
				case 5:
					WoodType = "oak";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.WoodType == "spruce":
					return 0;
				case { } b when true && b.WoodType == "acacia":
					return 1;
				case { } b when true && b.WoodType == "dark_oak":
					return 2;
				case { } b when true && b.WoodType == "jungle":
					return 3;
				case { } b when true && b.WoodType == "birch":
					return 4;
				case { } b when true && b.WoodType == "oak":
					return 5;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class FenceGate // 107 typeof=FenceGate
	{
		[Range(0, 3)] public int Direction { get; set; }
		[Range(0, 1)] public byte InWallBit { get; set; }
		[Range(0, 1)] public byte OpenBit { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Direction = 2;
					InWallBit = 1;
					OpenBit = 1;
					break;
				case 1:
					Direction = 0;
					InWallBit = 1;
					OpenBit = 1;
					break;
				case 2:
					Direction = 3;
					InWallBit = 0;
					OpenBit = 0;
					break;
				case 3:
					Direction = 2;
					InWallBit = 0;
					OpenBit = 0;
					break;
				case 4:
					Direction = 0;
					InWallBit = 0;
					OpenBit = 1;
					break;
				case 5:
					Direction = 3;
					InWallBit = 0;
					OpenBit = 1;
					break;
				case 6:
					Direction = 2;
					InWallBit = 0;
					OpenBit = 1;
					break;
				case 7:
					Direction = 0;
					InWallBit = 0;
					OpenBit = 0;
					break;
				case 8:
					Direction = 1;
					InWallBit = 0;
					OpenBit = 0;
					break;
				case 9:
					Direction = 1;
					InWallBit = 1;
					OpenBit = 1;
					break;
				case 10:
					Direction = 0;
					InWallBit = 1;
					OpenBit = 0;
					break;
				case 11:
					Direction = 1;
					InWallBit = 0;
					OpenBit = 1;
					break;
				case 12:
					Direction = 3;
					InWallBit = 1;
					OpenBit = 1;
					break;
				case 13:
					Direction = 3;
					InWallBit = 1;
					OpenBit = 0;
					break;
				case 14:
					Direction = 2;
					InWallBit = 1;
					OpenBit = 0;
					break;
				case 15:
					Direction = 1;
					InWallBit = 1;
					OpenBit = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Direction == 2 && b.InWallBit == 1 && b.OpenBit == 1:
					return 0;
				case { } b when true && b.Direction == 0 && b.InWallBit == 1 && b.OpenBit == 1:
					return 1;
				case { } b when true && b.Direction == 3 && b.InWallBit == 0 && b.OpenBit == 0:
					return 2;
				case { } b when true && b.Direction == 2 && b.InWallBit == 0 && b.OpenBit == 0:
					return 3;
				case { } b when true && b.Direction == 0 && b.InWallBit == 0 && b.OpenBit == 1:
					return 4;
				case { } b when true && b.Direction == 3 && b.InWallBit == 0 && b.OpenBit == 1:
					return 5;
				case { } b when true && b.Direction == 2 && b.InWallBit == 0 && b.OpenBit == 1:
					return 6;
				case { } b when true && b.Direction == 0 && b.InWallBit == 0 && b.OpenBit == 0:
					return 7;
				case { } b when true && b.Direction == 1 && b.InWallBit == 0 && b.OpenBit == 0:
					return 8;
				case { } b when true && b.Direction == 1 && b.InWallBit == 1 && b.OpenBit == 1:
					return 9;
				case { } b when true && b.Direction == 0 && b.InWallBit == 1 && b.OpenBit == 0:
					return 10;
				case { } b when true && b.Direction == 1 && b.InWallBit == 0 && b.OpenBit == 1:
					return 11;
				case { } b when true && b.Direction == 3 && b.InWallBit == 1 && b.OpenBit == 1:
					return 12;
				case { } b when true && b.Direction == 3 && b.InWallBit == 1 && b.OpenBit == 0:
					return 13;
				case { } b when true && b.Direction == 2 && b.InWallBit == 1 && b.OpenBit == 0:
					return 14;
				case { } b when true && b.Direction == 1 && b.InWallBit == 1 && b.OpenBit == 0:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Fire // 51 typeof=Fire
	{
		[Range(0, 9)] public int Age { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Age = 1;
					break;
				case 1:
					Age = 3;
					break;
				case 2:
					Age = 12;
					break;
				case 3:
					Age = 8;
					break;
				case 4:
					Age = 6;
					break;
				case 5:
					Age = 14;
					break;
				case 6:
					Age = 0;
					break;
				case 7:
					Age = 5;
					break;
				case 8:
					Age = 10;
					break;
				case 9:
					Age = 4;
					break;
				case 10:
					Age = 13;
					break;
				case 11:
					Age = 7;
					break;
				case 12:
					Age = 15;
					break;
				case 13:
					Age = 11;
					break;
				case 14:
					Age = 9;
					break;
				case 15:
					Age = 2;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Age == 1:
					return 0;
				case { } b when true && b.Age == 3:
					return 1;
				case { } b when true && b.Age == 12:
					return 2;
				case { } b when true && b.Age == 8:
					return 3;
				case { } b when true && b.Age == 6:
					return 4;
				case { } b when true && b.Age == 14:
					return 5;
				case { } b when true && b.Age == 0:
					return 6;
				case { } b when true && b.Age == 5:
					return 7;
				case { } b when true && b.Age == 10:
					return 8;
				case { } b when true && b.Age == 4:
					return 9;
				case { } b when true && b.Age == 13:
					return 10;
				case { } b when true && b.Age == 7:
					return 11;
				case { } b when true && b.Age == 15:
					return 12;
				case { } b when true && b.Age == 11:
					return 13;
				case { } b when true && b.Age == 9:
					return 14;
				case { } b when true && b.Age == 2:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class FletchingTable : Block // 0 typeof=Block
	{
		public FletchingTable() : base(456)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class FlowerPot // 140 typeof=FlowerPot
	{
		[Range(0, 1)] public byte UpdateBit { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					UpdateBit = 1;
					break;
				case 1:
					UpdateBit = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.UpdateBit == 1:
					return 0;
				case { } b when true && b.UpdateBit == 0:
					return 1;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class FlowingLava // 10 typeof=FlowingLava
	{
		[Range(0, 9)] public int LiquidDepth { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					LiquidDepth = 10;
					break;
				case 1:
					LiquidDepth = 2;
					break;
				case 2:
					LiquidDepth = 1;
					break;
				case 3:
					LiquidDepth = 13;
					break;
				case 4:
					LiquidDepth = 0;
					break;
				case 5:
					LiquidDepth = 14;
					break;
				case 6:
					LiquidDepth = 6;
					break;
				case 7:
					LiquidDepth = 4;
					break;
				case 8:
					LiquidDepth = 8;
					break;
				case 9:
					LiquidDepth = 12;
					break;
				case 10:
					LiquidDepth = 3;
					break;
				case 11:
					LiquidDepth = 9;
					break;
				case 12:
					LiquidDepth = 5;
					break;
				case 13:
					LiquidDepth = 7;
					break;
				case 14:
					LiquidDepth = 15;
					break;
				case 15:
					LiquidDepth = 11;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.LiquidDepth == 10:
					return 0;
				case { } b when true && b.LiquidDepth == 2:
					return 1;
				case { } b when true && b.LiquidDepth == 1:
					return 2;
				case { } b when true && b.LiquidDepth == 13:
					return 3;
				case { } b when true && b.LiquidDepth == 0:
					return 4;
				case { } b when true && b.LiquidDepth == 14:
					return 5;
				case { } b when true && b.LiquidDepth == 6:
					return 6;
				case { } b when true && b.LiquidDepth == 4:
					return 7;
				case { } b when true && b.LiquidDepth == 8:
					return 8;
				case { } b when true && b.LiquidDepth == 12:
					return 9;
				case { } b when true && b.LiquidDepth == 3:
					return 10;
				case { } b when true && b.LiquidDepth == 9:
					return 11;
				case { } b when true && b.LiquidDepth == 5:
					return 12;
				case { } b when true && b.LiquidDepth == 7:
					return 13;
				case { } b when true && b.LiquidDepth == 15:
					return 14;
				case { } b when true && b.LiquidDepth == 11:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class FlowingWater // 8 typeof=FlowingWater
	{
		[Range(0, 9)] public int LiquidDepth { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					LiquidDepth = 14;
					break;
				case 1:
					LiquidDepth = 11;
					break;
				case 2:
					LiquidDepth = 13;
					break;
				case 3:
					LiquidDepth = 1;
					break;
				case 4:
					LiquidDepth = 15;
					break;
				case 5:
					LiquidDepth = 12;
					break;
				case 6:
					LiquidDepth = 2;
					break;
				case 7:
					LiquidDepth = 6;
					break;
				case 8:
					LiquidDepth = 4;
					break;
				case 9:
					LiquidDepth = 9;
					break;
				case 10:
					LiquidDepth = 10;
					break;
				case 11:
					LiquidDepth = 0;
					break;
				case 12:
					LiquidDepth = 3;
					break;
				case 13:
					LiquidDepth = 7;
					break;
				case 14:
					LiquidDepth = 5;
					break;
				case 15:
					LiquidDepth = 8;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.LiquidDepth == 14:
					return 0;
				case { } b when true && b.LiquidDepth == 11:
					return 1;
				case { } b when true && b.LiquidDepth == 13:
					return 2;
				case { } b when true && b.LiquidDepth == 1:
					return 3;
				case { } b when true && b.LiquidDepth == 15:
					return 4;
				case { } b when true && b.LiquidDepth == 12:
					return 5;
				case { } b when true && b.LiquidDepth == 2:
					return 6;
				case { } b when true && b.LiquidDepth == 6:
					return 7;
				case { } b when true && b.LiquidDepth == 4:
					return 8;
				case { } b when true && b.LiquidDepth == 9:
					return 9;
				case { } b when true && b.LiquidDepth == 10:
					return 10;
				case { } b when true && b.LiquidDepth == 0:
					return 11;
				case { } b when true && b.LiquidDepth == 3:
					return 12;
				case { } b when true && b.LiquidDepth == 7:
					return 13;
				case { } b when true && b.LiquidDepth == 5:
					return 14;
				case { } b when true && b.LiquidDepth == 8:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Frame // 199 typeof=Frame
	{
		[Range(0, 5)] public int FacingDirection { get; set; }
		[Range(0, 1)] public byte ItemFrameMapBit { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					FacingDirection = 5;
					ItemFrameMapBit = 1;
					break;
				case 1:
					FacingDirection = 3;
					ItemFrameMapBit = 0;
					break;
				case 2:
					FacingDirection = 1;
					ItemFrameMapBit = 1;
					break;
				case 3:
					FacingDirection = 5;
					ItemFrameMapBit = 0;
					break;
				case 4:
					FacingDirection = 2;
					ItemFrameMapBit = 0;
					break;
				case 5:
					FacingDirection = 0;
					ItemFrameMapBit = 0;
					break;
				case 6:
					FacingDirection = 0;
					ItemFrameMapBit = 1;
					break;
				case 7:
					FacingDirection = 3;
					ItemFrameMapBit = 1;
					break;
				case 8:
					FacingDirection = 4;
					ItemFrameMapBit = 1;
					break;
				case 9:
					FacingDirection = 4;
					ItemFrameMapBit = 0;
					break;
				case 10:
					FacingDirection = 2;
					ItemFrameMapBit = 1;
					break;
				case 11:
					FacingDirection = 1;
					ItemFrameMapBit = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.FacingDirection == 5 && b.ItemFrameMapBit == 1:
					return 0;
				case { } b when true && b.FacingDirection == 3 && b.ItemFrameMapBit == 0:
					return 1;
				case { } b when true && b.FacingDirection == 1 && b.ItemFrameMapBit == 1:
					return 2;
				case { } b when true && b.FacingDirection == 5 && b.ItemFrameMapBit == 0:
					return 3;
				case { } b when true && b.FacingDirection == 2 && b.ItemFrameMapBit == 0:
					return 4;
				case { } b when true && b.FacingDirection == 0 && b.ItemFrameMapBit == 0:
					return 5;
				case { } b when true && b.FacingDirection == 0 && b.ItemFrameMapBit == 1:
					return 6;
				case { } b when true && b.FacingDirection == 3 && b.ItemFrameMapBit == 1:
					return 7;
				case { } b when true && b.FacingDirection == 4 && b.ItemFrameMapBit == 1:
					return 8;
				case { } b when true && b.FacingDirection == 4 && b.ItemFrameMapBit == 0:
					return 9;
				case { } b when true && b.FacingDirection == 2 && b.ItemFrameMapBit == 1:
					return 10;
				case { } b when true && b.FacingDirection == 1 && b.ItemFrameMapBit == 0:
					return 11;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class FrostedIce // 207 typeof=FrostedIce
	{
		[Range(0, 3)] public int Age { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Age = 1;
					break;
				case 1:
					Age = 2;
					break;
				case 2:
					Age = 3;
					break;
				case 3:
					Age = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Age == 1:
					return 0;
				case { } b when true && b.Age == 2:
					return 1;
				case { } b when true && b.Age == 3:
					return 2;
				case { } b when true && b.Age == 0:
					return 3;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Furnace // 61 typeof=Furnace
	{
		[Range(0, 5)] public int FacingDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					FacingDirection = 5;
					break;
				case 1:
					FacingDirection = 2;
					break;
				case 2:
					FacingDirection = 4;
					break;
				case 3:
					FacingDirection = 0;
					break;
				case 4:
					FacingDirection = 1;
					break;
				case 5:
					FacingDirection = 3;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.FacingDirection == 5:
					return 0;
				case { } b when true && b.FacingDirection == 2:
					return 1;
				case { } b when true && b.FacingDirection == 4:
					return 2;
				case { } b when true && b.FacingDirection == 0:
					return 3;
				case { } b when true && b.FacingDirection == 1:
					return 4;
				case { } b when true && b.FacingDirection == 3:
					return 5;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Glass // 20 typeof=Glass
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class GlassPane // 102 typeof=GlassPane
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Glowingobsidian // 246 typeof=GlowingObsidian
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Glowstone // 89 typeof=Glowstone
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class GoldBlock // 41 typeof=GoldBlock
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class GoldOre // 14 typeof=GoldOre
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class GoldenRail // 27 typeof=GoldenRail
	{
		[Range(0, 1)] public byte RailDataBit { get; set; }
		[Range(0, 5)] public int RailDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					RailDataBit = 0;
					RailDirection = 0;
					break;
				case 1:
					RailDataBit = 1;
					RailDirection = 4;
					break;
				case 2:
					RailDataBit = 1;
					RailDirection = 1;
					break;
				case 3:
					RailDataBit = 1;
					RailDirection = 0;
					break;
				case 4:
					RailDataBit = 1;
					RailDirection = 2;
					break;
				case 5:
					RailDataBit = 0;
					RailDirection = 5;
					break;
				case 6:
					RailDataBit = 0;
					RailDirection = 2;
					break;
				case 7:
					RailDataBit = 0;
					RailDirection = 3;
					break;
				case 8:
					RailDataBit = 1;
					RailDirection = 3;
					break;
				case 9:
					RailDataBit = 1;
					RailDirection = 5;
					break;
				case 10:
					RailDataBit = 0;
					RailDirection = 4;
					break;
				case 11:
					RailDataBit = 0;
					RailDirection = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.RailDataBit == 0 && b.RailDirection == 0:
					return 0;
				case { } b when true && b.RailDataBit == 1 && b.RailDirection == 4:
					return 1;
				case { } b when true && b.RailDataBit == 1 && b.RailDirection == 1:
					return 2;
				case { } b when true && b.RailDataBit == 1 && b.RailDirection == 0:
					return 3;
				case { } b when true && b.RailDataBit == 1 && b.RailDirection == 2:
					return 4;
				case { } b when true && b.RailDataBit == 0 && b.RailDirection == 5:
					return 5;
				case { } b when true && b.RailDataBit == 0 && b.RailDirection == 2:
					return 6;
				case { } b when true && b.RailDataBit == 0 && b.RailDirection == 3:
					return 7;
				case { } b when true && b.RailDataBit == 1 && b.RailDirection == 3:
					return 8;
				case { } b when true && b.RailDataBit == 1 && b.RailDirection == 5:
					return 9;
				case { } b when true && b.RailDataBit == 0 && b.RailDirection == 4:
					return 10;
				case { } b when true && b.RailDataBit == 0 && b.RailDirection == 1:
					return 11;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class GraniteStairs : Block // 0 typeof=Block
	{
		[Range(0, 1)] public byte UpsideDownBit { get; set; }
		[Range(0, 3)] public int WeirdoDirection { get; set; }

		public GraniteStairs() : base(424)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					UpsideDownBit = 1;
					WeirdoDirection = 2;
					break;
				case 1:
					UpsideDownBit = 1;
					WeirdoDirection = 3;
					break;
				case 2:
					UpsideDownBit = 0;
					WeirdoDirection = 2;
					break;
				case 3:
					UpsideDownBit = 0;
					WeirdoDirection = 3;
					break;
				case 4:
					UpsideDownBit = 0;
					WeirdoDirection = 1;
					break;
				case 5:
					UpsideDownBit = 1;
					WeirdoDirection = 0;
					break;
				case 6:
					UpsideDownBit = 0;
					WeirdoDirection = 0;
					break;
				case 7:
					UpsideDownBit = 1;
					WeirdoDirection = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 2:
					return 0;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 3:
					return 1;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 2:
					return 2;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 3:
					return 3;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 1:
					return 4;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 0:
					return 5;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 0:
					return 6;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 1:
					return 7;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Grass // 2 typeof=Grass
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class GrassPath // 198 typeof=GrassPath
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Gravel // 13 typeof=Gravel
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class GrayGlazedTerracotta // 227 typeof=GrayGlazedTerracotta
	{
		[Range(0, 5)] public int FacingDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					FacingDirection = 4;
					break;
				case 1:
					FacingDirection = 1;
					break;
				case 2:
					FacingDirection = 5;
					break;
				case 3:
					FacingDirection = 3;
					break;
				case 4:
					FacingDirection = 2;
					break;
				case 5:
					FacingDirection = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.FacingDirection == 4:
					return 0;
				case { } b when true && b.FacingDirection == 1:
					return 1;
				case { } b when true && b.FacingDirection == 5:
					return 2;
				case { } b when true && b.FacingDirection == 3:
					return 3;
				case { } b when true && b.FacingDirection == 2:
					return 4;
				case { } b when true && b.FacingDirection == 0:
					return 5;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class GreenGlazedTerracotta // 233 typeof=GreenGlazedTerracotta
	{
		[Range(0, 5)] public int FacingDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					FacingDirection = 1;
					break;
				case 1:
					FacingDirection = 5;
					break;
				case 2:
					FacingDirection = 4;
					break;
				case 3:
					FacingDirection = 2;
					break;
				case 4:
					FacingDirection = 0;
					break;
				case 5:
					FacingDirection = 3;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.FacingDirection == 1:
					return 0;
				case { } b when true && b.FacingDirection == 5:
					return 1;
				case { } b when true && b.FacingDirection == 4:
					return 2;
				case { } b when true && b.FacingDirection == 2:
					return 3;
				case { } b when true && b.FacingDirection == 0:
					return 4;
				case { } b when true && b.FacingDirection == 3:
					return 5;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Grindstone : Block // 0 typeof=Block
	{
		// Convert this attribute to enum
		//[Enum("hanging","multiple","side","standing"]
		public string Attachment { get; set; }
		[Range(0, 3)] public int Direction { get; set; }

		public Grindstone() : base(450)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Attachment = "standing";
					Direction = 3;
					break;
				case 1:
					Attachment = "standing";
					Direction = 1;
					break;
				case 2:
					Attachment = "multiple";
					Direction = 2;
					break;
				case 3:
					Attachment = "hanging";
					Direction = 0;
					break;
				case 4:
					Attachment = "multiple";
					Direction = 1;
					break;
				case 5:
					Attachment = "multiple";
					Direction = 0;
					break;
				case 6:
					Attachment = "side";
					Direction = 2;
					break;
				case 7:
					Attachment = "side";
					Direction = 3;
					break;
				case 8:
					Attachment = "hanging";
					Direction = 3;
					break;
				case 9:
					Attachment = "standing";
					Direction = 0;
					break;
				case 10:
					Attachment = "side";
					Direction = 0;
					break;
				case 11:
					Attachment = "standing";
					Direction = 2;
					break;
				case 12:
					Attachment = "hanging";
					Direction = 1;
					break;
				case 13:
					Attachment = "side";
					Direction = 1;
					break;
				case 14:
					Attachment = "multiple";
					Direction = 3;
					break;
				case 15:
					Attachment = "hanging";
					Direction = 2;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Attachment == "standing" && b.Direction == 3:
					return 0;
				case { } b when true && b.Attachment == "standing" && b.Direction == 1:
					return 1;
				case { } b when true && b.Attachment == "multiple" && b.Direction == 2:
					return 2;
				case { } b when true && b.Attachment == "hanging" && b.Direction == 0:
					return 3;
				case { } b when true && b.Attachment == "multiple" && b.Direction == 1:
					return 4;
				case { } b when true && b.Attachment == "multiple" && b.Direction == 0:
					return 5;
				case { } b when true && b.Attachment == "side" && b.Direction == 2:
					return 6;
				case { } b when true && b.Attachment == "side" && b.Direction == 3:
					return 7;
				case { } b when true && b.Attachment == "hanging" && b.Direction == 3:
					return 8;
				case { } b when true && b.Attachment == "standing" && b.Direction == 0:
					return 9;
				case { } b when true && b.Attachment == "side" && b.Direction == 0:
					return 10;
				case { } b when true && b.Attachment == "standing" && b.Direction == 2:
					return 11;
				case { } b when true && b.Attachment == "hanging" && b.Direction == 1:
					return 12;
				case { } b when true && b.Attachment == "side" && b.Direction == 1:
					return 13;
				case { } b when true && b.Attachment == "multiple" && b.Direction == 3:
					return 14;
				case { } b when true && b.Attachment == "hanging" && b.Direction == 2:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class HardGlass : Block // 0 typeof=Block
	{
		public HardGlass() : base(253)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class HardGlassPane : Block // 0 typeof=Block
	{
		public HardGlassPane() : base(190)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class HardStainedGlass : Block // 0 typeof=Block
	{
		// Convert this attribute to enum
		//[Enum("black","blue","brown","cyan","gray","green","light_blue","lime","magenta","orange","pink","purple","red","silver","white","yellow"]
		public string Color { get; set; }

		public HardStainedGlass() : base(254)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Color = "gray";
					break;
				case 1:
					Color = "cyan";
					break;
				case 2:
					Color = "brown";
					break;
				case 3:
					Color = "light_blue";
					break;
				case 4:
					Color = "white";
					break;
				case 5:
					Color = "black";
					break;
				case 6:
					Color = "orange";
					break;
				case 7:
					Color = "silver";
					break;
				case 8:
					Color = "yellow";
					break;
				case 9:
					Color = "purple";
					break;
				case 10:
					Color = "pink";
					break;
				case 11:
					Color = "lime";
					break;
				case 12:
					Color = "blue";
					break;
				case 13:
					Color = "red";
					break;
				case 14:
					Color = "green";
					break;
				case 15:
					Color = "magenta";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Color == "gray":
					return 0;
				case { } b when true && b.Color == "cyan":
					return 1;
				case { } b when true && b.Color == "brown":
					return 2;
				case { } b when true && b.Color == "light_blue":
					return 3;
				case { } b when true && b.Color == "white":
					return 4;
				case { } b when true && b.Color == "black":
					return 5;
				case { } b when true && b.Color == "orange":
					return 6;
				case { } b when true && b.Color == "silver":
					return 7;
				case { } b when true && b.Color == "yellow":
					return 8;
				case { } b when true && b.Color == "purple":
					return 9;
				case { } b when true && b.Color == "pink":
					return 10;
				case { } b when true && b.Color == "lime":
					return 11;
				case { } b when true && b.Color == "blue":
					return 12;
				case { } b when true && b.Color == "red":
					return 13;
				case { } b when true && b.Color == "green":
					return 14;
				case { } b when true && b.Color == "magenta":
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class HardStainedGlassPane : Block // 0 typeof=Block
	{
		// Convert this attribute to enum
		//[Enum("black","blue","brown","cyan","gray","green","light_blue","lime","magenta","orange","pink","purple","red","silver","white","yellow"]
		public string Color { get; set; }

		public HardStainedGlassPane() : base(191)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Color = "lime";
					break;
				case 1:
					Color = "brown";
					break;
				case 2:
					Color = "red";
					break;
				case 3:
					Color = "magenta";
					break;
				case 4:
					Color = "orange";
					break;
				case 5:
					Color = "silver";
					break;
				case 6:
					Color = "cyan";
					break;
				case 7:
					Color = "blue";
					break;
				case 8:
					Color = "black";
					break;
				case 9:
					Color = "pink";
					break;
				case 10:
					Color = "gray";
					break;
				case 11:
					Color = "green";
					break;
				case 12:
					Color = "yellow";
					break;
				case 13:
					Color = "purple";
					break;
				case 14:
					Color = "light_blue";
					break;
				case 15:
					Color = "white";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Color == "lime":
					return 0;
				case { } b when true && b.Color == "brown":
					return 1;
				case { } b when true && b.Color == "red":
					return 2;
				case { } b when true && b.Color == "magenta":
					return 3;
				case { } b when true && b.Color == "orange":
					return 4;
				case { } b when true && b.Color == "silver":
					return 5;
				case { } b when true && b.Color == "cyan":
					return 6;
				case { } b when true && b.Color == "blue":
					return 7;
				case { } b when true && b.Color == "black":
					return 8;
				case { } b when true && b.Color == "pink":
					return 9;
				case { } b when true && b.Color == "gray":
					return 10;
				case { } b when true && b.Color == "green":
					return 11;
				case { } b when true && b.Color == "yellow":
					return 12;
				case { } b when true && b.Color == "purple":
					return 13;
				case { } b when true && b.Color == "light_blue":
					return 14;
				case { } b when true && b.Color == "white":
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class HardenedClay // 172 typeof=HardenedClay
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class HayBlock // 170 typeof=HayBlock
	{
		[Range(0, 3)] public int Deprecated { get; set; }

		// Convert this attribute to enum
		//[Enum("x","y","z"]
		public string PillarAxis { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Deprecated = 1;
					PillarAxis = "y";
					break;
				case 1:
					Deprecated = 2;
					PillarAxis = "z";
					break;
				case 2:
					Deprecated = 0;
					PillarAxis = "x";
					break;
				case 3:
					Deprecated = 3;
					PillarAxis = "y";
					break;
				case 4:
					Deprecated = 2;
					PillarAxis = "x";
					break;
				case 5:
					Deprecated = 3;
					PillarAxis = "z";
					break;
				case 6:
					Deprecated = 0;
					PillarAxis = "z";
					break;
				case 7:
					Deprecated = 0;
					PillarAxis = "y";
					break;
				case 8:
					Deprecated = 2;
					PillarAxis = "y";
					break;
				case 9:
					Deprecated = 1;
					PillarAxis = "z";
					break;
				case 10:
					Deprecated = 1;
					PillarAxis = "x";
					break;
				case 11:
					Deprecated = 3;
					PillarAxis = "x";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Deprecated == 1 && b.PillarAxis == "y":
					return 0;
				case { } b when true && b.Deprecated == 2 && b.PillarAxis == "z":
					return 1;
				case { } b when true && b.Deprecated == 0 && b.PillarAxis == "x":
					return 2;
				case { } b when true && b.Deprecated == 3 && b.PillarAxis == "y":
					return 3;
				case { } b when true && b.Deprecated == 2 && b.PillarAxis == "x":
					return 4;
				case { } b when true && b.Deprecated == 3 && b.PillarAxis == "z":
					return 5;
				case { } b when true && b.Deprecated == 0 && b.PillarAxis == "z":
					return 6;
				case { } b when true && b.Deprecated == 0 && b.PillarAxis == "y":
					return 7;
				case { } b when true && b.Deprecated == 2 && b.PillarAxis == "y":
					return 8;
				case { } b when true && b.Deprecated == 1 && b.PillarAxis == "z":
					return 9;
				case { } b when true && b.Deprecated == 1 && b.PillarAxis == "x":
					return 10;
				case { } b when true && b.Deprecated == 3 && b.PillarAxis == "x":
					return 11;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class HeavyWeightedPressurePlate // 148 typeof=HeavyWeightedPressurePlate
	{
		[Range(0, 9)] public int RedstoneSignal { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					RedstoneSignal = 2;
					break;
				case 1:
					RedstoneSignal = 14;
					break;
				case 2:
					RedstoneSignal = 11;
					break;
				case 3:
					RedstoneSignal = 15;
					break;
				case 4:
					RedstoneSignal = 8;
					break;
				case 5:
					RedstoneSignal = 9;
					break;
				case 6:
					RedstoneSignal = 12;
					break;
				case 7:
					RedstoneSignal = 6;
					break;
				case 8:
					RedstoneSignal = 3;
					break;
				case 9:
					RedstoneSignal = 10;
					break;
				case 10:
					RedstoneSignal = 0;
					break;
				case 11:
					RedstoneSignal = 7;
					break;
				case 12:
					RedstoneSignal = 13;
					break;
				case 13:
					RedstoneSignal = 5;
					break;
				case 14:
					RedstoneSignal = 1;
					break;
				case 15:
					RedstoneSignal = 4;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.RedstoneSignal == 2:
					return 0;
				case { } b when true && b.RedstoneSignal == 14:
					return 1;
				case { } b when true && b.RedstoneSignal == 11:
					return 2;
				case { } b when true && b.RedstoneSignal == 15:
					return 3;
				case { } b when true && b.RedstoneSignal == 8:
					return 4;
				case { } b when true && b.RedstoneSignal == 9:
					return 5;
				case { } b when true && b.RedstoneSignal == 12:
					return 6;
				case { } b when true && b.RedstoneSignal == 6:
					return 7;
				case { } b when true && b.RedstoneSignal == 3:
					return 8;
				case { } b when true && b.RedstoneSignal == 10:
					return 9;
				case { } b when true && b.RedstoneSignal == 0:
					return 10;
				case { } b when true && b.RedstoneSignal == 7:
					return 11;
				case { } b when true && b.RedstoneSignal == 13:
					return 12;
				case { } b when true && b.RedstoneSignal == 5:
					return 13;
				case { } b when true && b.RedstoneSignal == 1:
					return 14;
				case { } b when true && b.RedstoneSignal == 4:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class HoneyBlock : Block // 0 typeof=Block
	{
		public HoneyBlock() : base(475)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class HoneycombBlock : Block // 0 typeof=Block
	{
		public HoneycombBlock() : base(476)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Hopper // 154 typeof=Hopper
	{
		[Range(0, 5)] public int FacingDirection { get; set; }
		[Range(0, 1)] public byte ToggleBit { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					FacingDirection = 0;
					ToggleBit = 0;
					break;
				case 1:
					FacingDirection = 5;
					ToggleBit = 0;
					break;
				case 2:
					FacingDirection = 4;
					ToggleBit = 1;
					break;
				case 3:
					FacingDirection = 5;
					ToggleBit = 1;
					break;
				case 4:
					FacingDirection = 3;
					ToggleBit = 0;
					break;
				case 5:
					FacingDirection = 0;
					ToggleBit = 1;
					break;
				case 6:
					FacingDirection = 1;
					ToggleBit = 0;
					break;
				case 7:
					FacingDirection = 4;
					ToggleBit = 0;
					break;
				case 8:
					FacingDirection = 2;
					ToggleBit = 1;
					break;
				case 9:
					FacingDirection = 3;
					ToggleBit = 1;
					break;
				case 10:
					FacingDirection = 2;
					ToggleBit = 0;
					break;
				case 11:
					FacingDirection = 1;
					ToggleBit = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.FacingDirection == 0 && b.ToggleBit == 0:
					return 0;
				case { } b when true && b.FacingDirection == 5 && b.ToggleBit == 0:
					return 1;
				case { } b when true && b.FacingDirection == 4 && b.ToggleBit == 1:
					return 2;
				case { } b when true && b.FacingDirection == 5 && b.ToggleBit == 1:
					return 3;
				case { } b when true && b.FacingDirection == 3 && b.ToggleBit == 0:
					return 4;
				case { } b when true && b.FacingDirection == 0 && b.ToggleBit == 1:
					return 5;
				case { } b when true && b.FacingDirection == 1 && b.ToggleBit == 0:
					return 6;
				case { } b when true && b.FacingDirection == 4 && b.ToggleBit == 0:
					return 7;
				case { } b when true && b.FacingDirection == 2 && b.ToggleBit == 1:
					return 8;
				case { } b when true && b.FacingDirection == 3 && b.ToggleBit == 1:
					return 9;
				case { } b when true && b.FacingDirection == 2 && b.ToggleBit == 0:
					return 10;
				case { } b when true && b.FacingDirection == 1 && b.ToggleBit == 1:
					return 11;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Ice // 79 typeof=Ice
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class InfoUpdate : Block // 0 typeof=Block
	{
		public InfoUpdate() : base(248)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class InfoUpdate2 : Block // 0 typeof=Block
	{
		public InfoUpdate2() : base(249)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class InvisibleBedrock // 95 typeof=InvisibleBedrock
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class IronBars // 101 typeof=IronBars
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class IronBlock // 42 typeof=IronBlock
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class IronDoor // 71 typeof=IronDoor
	{
		[Range(0, 3)] public int Direction { get; set; }
		[Range(0, 1)] public byte DoorHingeBit { get; set; }
		[Range(0, 1)] public byte OpenBit { get; set; }
		[Range(0, 1)] public byte UpperBlockBit { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Direction = 1;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 1:
					Direction = 2;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 2:
					Direction = 3;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 3:
					Direction = 0;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 4:
					Direction = 2;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 5:
					Direction = 3;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 6:
					Direction = 3;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 7:
					Direction = 0;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 8:
					Direction = 1;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 9:
					Direction = 3;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 10:
					Direction = 2;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 11:
					Direction = 1;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 12:
					Direction = 2;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 13:
					Direction = 1;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 14:
					Direction = 3;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 15:
					Direction = 1;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 16:
					Direction = 3;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 17:
					Direction = 0;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 18:
					Direction = 1;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 19:
					Direction = 0;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 20:
					Direction = 0;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 21:
					Direction = 2;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 22:
					Direction = 0;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 23:
					Direction = 0;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 24:
					Direction = 2;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 25:
					Direction = 0;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 26:
					Direction = 2;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 27:
					Direction = 2;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 28:
					Direction = 1;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 29:
					Direction = 3;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 30:
					Direction = 3;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 31:
					Direction = 1;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 0;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 1;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 2;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 3;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 4;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 5;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 6;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 7;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 8;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 9;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 10;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 11;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 12;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 13;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 14;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 15;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 16;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 17;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 18;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 19;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 20;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 21;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 22;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 23;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 24;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 25;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 26;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 27;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 28;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 29;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 30;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 31;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class IronOre // 15 typeof=IronOre
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class IronTrapdoor // 167 typeof=IronTrapdoor
	{
		[Range(0, 3)] public int Direction { get; set; }
		[Range(0, 1)] public byte OpenBit { get; set; }
		[Range(0, 1)] public byte UpsideDownBit { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Direction = 1;
					OpenBit = 0;
					UpsideDownBit = 1;
					break;
				case 1:
					Direction = 2;
					OpenBit = 1;
					UpsideDownBit = 1;
					break;
				case 2:
					Direction = 2;
					OpenBit = 0;
					UpsideDownBit = 1;
					break;
				case 3:
					Direction = 3;
					OpenBit = 0;
					UpsideDownBit = 1;
					break;
				case 4:
					Direction = 2;
					OpenBit = 1;
					UpsideDownBit = 0;
					break;
				case 5:
					Direction = 0;
					OpenBit = 1;
					UpsideDownBit = 0;
					break;
				case 6:
					Direction = 0;
					OpenBit = 0;
					UpsideDownBit = 0;
					break;
				case 7:
					Direction = 3;
					OpenBit = 1;
					UpsideDownBit = 0;
					break;
				case 8:
					Direction = 1;
					OpenBit = 0;
					UpsideDownBit = 0;
					break;
				case 9:
					Direction = 0;
					OpenBit = 1;
					UpsideDownBit = 1;
					break;
				case 10:
					Direction = 3;
					OpenBit = 0;
					UpsideDownBit = 0;
					break;
				case 11:
					Direction = 1;
					OpenBit = 1;
					UpsideDownBit = 1;
					break;
				case 12:
					Direction = 3;
					OpenBit = 1;
					UpsideDownBit = 1;
					break;
				case 13:
					Direction = 0;
					OpenBit = 0;
					UpsideDownBit = 1;
					break;
				case 14:
					Direction = 2;
					OpenBit = 0;
					UpsideDownBit = 0;
					break;
				case 15:
					Direction = 1;
					OpenBit = 1;
					UpsideDownBit = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Direction == 1 && b.OpenBit == 0 && b.UpsideDownBit == 1:
					return 0;
				case { } b when true && b.Direction == 2 && b.OpenBit == 1 && b.UpsideDownBit == 1:
					return 1;
				case { } b when true && b.Direction == 2 && b.OpenBit == 0 && b.UpsideDownBit == 1:
					return 2;
				case { } b when true && b.Direction == 3 && b.OpenBit == 0 && b.UpsideDownBit == 1:
					return 3;
				case { } b when true && b.Direction == 2 && b.OpenBit == 1 && b.UpsideDownBit == 0:
					return 4;
				case { } b when true && b.Direction == 0 && b.OpenBit == 1 && b.UpsideDownBit == 0:
					return 5;
				case { } b when true && b.Direction == 0 && b.OpenBit == 0 && b.UpsideDownBit == 0:
					return 6;
				case { } b when true && b.Direction == 3 && b.OpenBit == 1 && b.UpsideDownBit == 0:
					return 7;
				case { } b when true && b.Direction == 1 && b.OpenBit == 0 && b.UpsideDownBit == 0:
					return 8;
				case { } b when true && b.Direction == 0 && b.OpenBit == 1 && b.UpsideDownBit == 1:
					return 9;
				case { } b when true && b.Direction == 3 && b.OpenBit == 0 && b.UpsideDownBit == 0:
					return 10;
				case { } b when true && b.Direction == 1 && b.OpenBit == 1 && b.UpsideDownBit == 1:
					return 11;
				case { } b when true && b.Direction == 3 && b.OpenBit == 1 && b.UpsideDownBit == 1:
					return 12;
				case { } b when true && b.Direction == 0 && b.OpenBit == 0 && b.UpsideDownBit == 1:
					return 13;
				case { } b when true && b.Direction == 2 && b.OpenBit == 0 && b.UpsideDownBit == 0:
					return 14;
				case { } b when true && b.Direction == 1 && b.OpenBit == 1 && b.UpsideDownBit == 0:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Jigsaw : Block // 0 typeof=Block
	{
		[Range(0, 5)] public int FacingDirection { get; set; }

		public Jigsaw() : base(466)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					FacingDirection = 2;
					break;
				case 1:
					FacingDirection = 3;
					break;
				case 2:
					FacingDirection = 0;
					break;
				case 3:
					FacingDirection = 4;
					break;
				case 4:
					FacingDirection = 5;
					break;
				case 5:
					FacingDirection = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.FacingDirection == 2:
					return 0;
				case { } b when true && b.FacingDirection == 3:
					return 1;
				case { } b when true && b.FacingDirection == 0:
					return 2;
				case { } b when true && b.FacingDirection == 4:
					return 3;
				case { } b when true && b.FacingDirection == 5:
					return 4;
				case { } b when true && b.FacingDirection == 1:
					return 5;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Jukebox // 84 typeof=Jukebox
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class JungleButton : Block // 0 typeof=Block
	{
		[Range(0, 1)] public byte ButtonPressedBit { get; set; }
		[Range(0, 5)] public int FacingDirection { get; set; }

		public JungleButton() : base(398)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					ButtonPressedBit = 0;
					FacingDirection = 5;
					break;
				case 1:
					ButtonPressedBit = 1;
					FacingDirection = 5;
					break;
				case 2:
					ButtonPressedBit = 1;
					FacingDirection = 0;
					break;
				case 3:
					ButtonPressedBit = 1;
					FacingDirection = 1;
					break;
				case 4:
					ButtonPressedBit = 0;
					FacingDirection = 1;
					break;
				case 5:
					ButtonPressedBit = 1;
					FacingDirection = 4;
					break;
				case 6:
					ButtonPressedBit = 0;
					FacingDirection = 2;
					break;
				case 7:
					ButtonPressedBit = 0;
					FacingDirection = 4;
					break;
				case 8:
					ButtonPressedBit = 1;
					FacingDirection = 2;
					break;
				case 9:
					ButtonPressedBit = 0;
					FacingDirection = 0;
					break;
				case 10:
					ButtonPressedBit = 0;
					FacingDirection = 3;
					break;
				case 11:
					ButtonPressedBit = 1;
					FacingDirection = 3;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.ButtonPressedBit == 0 && b.FacingDirection == 5:
					return 0;
				case { } b when true && b.ButtonPressedBit == 1 && b.FacingDirection == 5:
					return 1;
				case { } b when true && b.ButtonPressedBit == 1 && b.FacingDirection == 0:
					return 2;
				case { } b when true && b.ButtonPressedBit == 1 && b.FacingDirection == 1:
					return 3;
				case { } b when true && b.ButtonPressedBit == 0 && b.FacingDirection == 1:
					return 4;
				case { } b when true && b.ButtonPressedBit == 1 && b.FacingDirection == 4:
					return 5;
				case { } b when true && b.ButtonPressedBit == 0 && b.FacingDirection == 2:
					return 6;
				case { } b when true && b.ButtonPressedBit == 0 && b.FacingDirection == 4:
					return 7;
				case { } b when true && b.ButtonPressedBit == 1 && b.FacingDirection == 2:
					return 8;
				case { } b when true && b.ButtonPressedBit == 0 && b.FacingDirection == 0:
					return 9;
				case { } b when true && b.ButtonPressedBit == 0 && b.FacingDirection == 3:
					return 10;
				case { } b when true && b.ButtonPressedBit == 1 && b.FacingDirection == 3:
					return 11;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class JungleDoor // 195 typeof=JungleDoor
	{
		[Range(0, 3)] public int Direction { get; set; }
		[Range(0, 1)] public byte DoorHingeBit { get; set; }
		[Range(0, 1)] public byte OpenBit { get; set; }
		[Range(0, 1)] public byte UpperBlockBit { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Direction = 1;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 1:
					Direction = 1;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 2:
					Direction = 2;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 3:
					Direction = 3;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 4:
					Direction = 3;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 5:
					Direction = 0;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 6:
					Direction = 2;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 7:
					Direction = 2;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 8:
					Direction = 2;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 9:
					Direction = 1;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 10:
					Direction = 3;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 11:
					Direction = 3;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 12:
					Direction = 0;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 13:
					Direction = 0;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 14:
					Direction = 1;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 15:
					Direction = 3;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 16:
					Direction = 0;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 17:
					Direction = 3;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 18:
					Direction = 0;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 19:
					Direction = 2;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 20:
					Direction = 1;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 21:
					Direction = 2;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 22:
					Direction = 2;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 23:
					Direction = 0;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 24:
					Direction = 0;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 25:
					Direction = 1;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 26:
					Direction = 3;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 27:
					Direction = 3;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 28:
					Direction = 1;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 29:
					Direction = 1;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 30:
					Direction = 2;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 31:
					Direction = 0;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 0;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 1;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 2;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 3;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 4;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 5;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 6;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 7;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 8;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 9;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 10;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 11;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 12;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 13;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 14;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 15;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 16;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 17;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 18;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 19;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 20;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 21;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 22;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 23;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 24;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 25;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 26;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 27;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 28;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 29;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 30;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 31;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class JungleFenceGate // 185 typeof=JungleFenceGate
	{
		[Range(0, 3)] public int Direction { get; set; }
		[Range(0, 1)] public byte InWallBit { get; set; }
		[Range(0, 1)] public byte OpenBit { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Direction = 2;
					InWallBit = 1;
					OpenBit = 1;
					break;
				case 1:
					Direction = 2;
					InWallBit = 0;
					OpenBit = 1;
					break;
				case 2:
					Direction = 1;
					InWallBit = 1;
					OpenBit = 0;
					break;
				case 3:
					Direction = 3;
					InWallBit = 0;
					OpenBit = 1;
					break;
				case 4:
					Direction = 3;
					InWallBit = 1;
					OpenBit = 0;
					break;
				case 5:
					Direction = 1;
					InWallBit = 0;
					OpenBit = 0;
					break;
				case 6:
					Direction = 0;
					InWallBit = 1;
					OpenBit = 0;
					break;
				case 7:
					Direction = 3;
					InWallBit = 1;
					OpenBit = 1;
					break;
				case 8:
					Direction = 0;
					InWallBit = 0;
					OpenBit = 1;
					break;
				case 9:
					Direction = 2;
					InWallBit = 1;
					OpenBit = 0;
					break;
				case 10:
					Direction = 3;
					InWallBit = 0;
					OpenBit = 0;
					break;
				case 11:
					Direction = 0;
					InWallBit = 1;
					OpenBit = 1;
					break;
				case 12:
					Direction = 0;
					InWallBit = 0;
					OpenBit = 0;
					break;
				case 13:
					Direction = 2;
					InWallBit = 0;
					OpenBit = 0;
					break;
				case 14:
					Direction = 1;
					InWallBit = 1;
					OpenBit = 1;
					break;
				case 15:
					Direction = 1;
					InWallBit = 0;
					OpenBit = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Direction == 2 && b.InWallBit == 1 && b.OpenBit == 1:
					return 0;
				case { } b when true && b.Direction == 2 && b.InWallBit == 0 && b.OpenBit == 1:
					return 1;
				case { } b when true && b.Direction == 1 && b.InWallBit == 1 && b.OpenBit == 0:
					return 2;
				case { } b when true && b.Direction == 3 && b.InWallBit == 0 && b.OpenBit == 1:
					return 3;
				case { } b when true && b.Direction == 3 && b.InWallBit == 1 && b.OpenBit == 0:
					return 4;
				case { } b when true && b.Direction == 1 && b.InWallBit == 0 && b.OpenBit == 0:
					return 5;
				case { } b when true && b.Direction == 0 && b.InWallBit == 1 && b.OpenBit == 0:
					return 6;
				case { } b when true && b.Direction == 3 && b.InWallBit == 1 && b.OpenBit == 1:
					return 7;
				case { } b when true && b.Direction == 0 && b.InWallBit == 0 && b.OpenBit == 1:
					return 8;
				case { } b when true && b.Direction == 2 && b.InWallBit == 1 && b.OpenBit == 0:
					return 9;
				case { } b when true && b.Direction == 3 && b.InWallBit == 0 && b.OpenBit == 0:
					return 10;
				case { } b when true && b.Direction == 0 && b.InWallBit == 1 && b.OpenBit == 1:
					return 11;
				case { } b when true && b.Direction == 0 && b.InWallBit == 0 && b.OpenBit == 0:
					return 12;
				case { } b when true && b.Direction == 2 && b.InWallBit == 0 && b.OpenBit == 0:
					return 13;
				case { } b when true && b.Direction == 1 && b.InWallBit == 1 && b.OpenBit == 1:
					return 14;
				case { } b when true && b.Direction == 1 && b.InWallBit == 0 && b.OpenBit == 1:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class JunglePressurePlate : Block // 0 typeof=Block
	{
		[Range(0, 9)] public int RedstoneSignal { get; set; }

		public JunglePressurePlate() : base(408)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					RedstoneSignal = 11;
					break;
				case 1:
					RedstoneSignal = 9;
					break;
				case 2:
					RedstoneSignal = 3;
					break;
				case 3:
					RedstoneSignal = 7;
					break;
				case 4:
					RedstoneSignal = 14;
					break;
				case 5:
					RedstoneSignal = 13;
					break;
				case 6:
					RedstoneSignal = 0;
					break;
				case 7:
					RedstoneSignal = 2;
					break;
				case 8:
					RedstoneSignal = 8;
					break;
				case 9:
					RedstoneSignal = 15;
					break;
				case 10:
					RedstoneSignal = 5;
					break;
				case 11:
					RedstoneSignal = 12;
					break;
				case 12:
					RedstoneSignal = 6;
					break;
				case 13:
					RedstoneSignal = 4;
					break;
				case 14:
					RedstoneSignal = 1;
					break;
				case 15:
					RedstoneSignal = 10;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.RedstoneSignal == 11:
					return 0;
				case { } b when true && b.RedstoneSignal == 9:
					return 1;
				case { } b when true && b.RedstoneSignal == 3:
					return 2;
				case { } b when true && b.RedstoneSignal == 7:
					return 3;
				case { } b when true && b.RedstoneSignal == 14:
					return 4;
				case { } b when true && b.RedstoneSignal == 13:
					return 5;
				case { } b when true && b.RedstoneSignal == 0:
					return 6;
				case { } b when true && b.RedstoneSignal == 2:
					return 7;
				case { } b when true && b.RedstoneSignal == 8:
					return 8;
				case { } b when true && b.RedstoneSignal == 15:
					return 9;
				case { } b when true && b.RedstoneSignal == 5:
					return 10;
				case { } b when true && b.RedstoneSignal == 12:
					return 11;
				case { } b when true && b.RedstoneSignal == 6:
					return 12;
				case { } b when true && b.RedstoneSignal == 4:
					return 13;
				case { } b when true && b.RedstoneSignal == 1:
					return 14;
				case { } b when true && b.RedstoneSignal == 10:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class JungleStairs // 136 typeof=JungleStairs
	{
		[Range(0, 1)] public byte UpsideDownBit { get; set; }
		[Range(0, 3)] public int WeirdoDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					UpsideDownBit = 1;
					WeirdoDirection = 0;
					break;
				case 1:
					UpsideDownBit = 1;
					WeirdoDirection = 3;
					break;
				case 2:
					UpsideDownBit = 1;
					WeirdoDirection = 2;
					break;
				case 3:
					UpsideDownBit = 0;
					WeirdoDirection = 1;
					break;
				case 4:
					UpsideDownBit = 1;
					WeirdoDirection = 1;
					break;
				case 5:
					UpsideDownBit = 0;
					WeirdoDirection = 2;
					break;
				case 6:
					UpsideDownBit = 0;
					WeirdoDirection = 0;
					break;
				case 7:
					UpsideDownBit = 0;
					WeirdoDirection = 3;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 0:
					return 0;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 3:
					return 1;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 2:
					return 2;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 1:
					return 3;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 1:
					return 4;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 2:
					return 5;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 0:
					return 6;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 3:
					return 7;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class JungleStandingSign : Block // 0 typeof=Block
	{
		[Range(0, 9)] public int GroundSignDirection { get; set; }

		public JungleStandingSign() : base(443)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					GroundSignDirection = 8;
					break;
				case 1:
					GroundSignDirection = 1;
					break;
				case 2:
					GroundSignDirection = 13;
					break;
				case 3:
					GroundSignDirection = 11;
					break;
				case 4:
					GroundSignDirection = 4;
					break;
				case 5:
					GroundSignDirection = 5;
					break;
				case 6:
					GroundSignDirection = 10;
					break;
				case 7:
					GroundSignDirection = 12;
					break;
				case 8:
					GroundSignDirection = 9;
					break;
				case 9:
					GroundSignDirection = 2;
					break;
				case 10:
					GroundSignDirection = 6;
					break;
				case 11:
					GroundSignDirection = 14;
					break;
				case 12:
					GroundSignDirection = 3;
					break;
				case 13:
					GroundSignDirection = 0;
					break;
				case 14:
					GroundSignDirection = 7;
					break;
				case 15:
					GroundSignDirection = 15;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.GroundSignDirection == 8:
					return 0;
				case { } b when true && b.GroundSignDirection == 1:
					return 1;
				case { } b when true && b.GroundSignDirection == 13:
					return 2;
				case { } b when true && b.GroundSignDirection == 11:
					return 3;
				case { } b when true && b.GroundSignDirection == 4:
					return 4;
				case { } b when true && b.GroundSignDirection == 5:
					return 5;
				case { } b when true && b.GroundSignDirection == 10:
					return 6;
				case { } b when true && b.GroundSignDirection == 12:
					return 7;
				case { } b when true && b.GroundSignDirection == 9:
					return 8;
				case { } b when true && b.GroundSignDirection == 2:
					return 9;
				case { } b when true && b.GroundSignDirection == 6:
					return 10;
				case { } b when true && b.GroundSignDirection == 14:
					return 11;
				case { } b when true && b.GroundSignDirection == 3:
					return 12;
				case { } b when true && b.GroundSignDirection == 0:
					return 13;
				case { } b when true && b.GroundSignDirection == 7:
					return 14;
				case { } b when true && b.GroundSignDirection == 15:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class JungleTrapdoor : Block // 0 typeof=Block
	{
		[Range(0, 3)] public int Direction { get; set; }
		[Range(0, 1)] public byte OpenBit { get; set; }
		[Range(0, 1)] public byte UpsideDownBit { get; set; }

		public JungleTrapdoor() : base(403)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Direction = 2;
					OpenBit = 0;
					UpsideDownBit = 1;
					break;
				case 1:
					Direction = 3;
					OpenBit = 1;
					UpsideDownBit = 1;
					break;
				case 2:
					Direction = 0;
					OpenBit = 1;
					UpsideDownBit = 0;
					break;
				case 3:
					Direction = 1;
					OpenBit = 0;
					UpsideDownBit = 1;
					break;
				case 4:
					Direction = 3;
					OpenBit = 0;
					UpsideDownBit = 1;
					break;
				case 5:
					Direction = 3;
					OpenBit = 1;
					UpsideDownBit = 0;
					break;
				case 6:
					Direction = 2;
					OpenBit = 0;
					UpsideDownBit = 0;
					break;
				case 7:
					Direction = 1;
					OpenBit = 1;
					UpsideDownBit = 1;
					break;
				case 8:
					Direction = 2;
					OpenBit = 1;
					UpsideDownBit = 1;
					break;
				case 9:
					Direction = 2;
					OpenBit = 1;
					UpsideDownBit = 0;
					break;
				case 10:
					Direction = 3;
					OpenBit = 0;
					UpsideDownBit = 0;
					break;
				case 11:
					Direction = 0;
					OpenBit = 0;
					UpsideDownBit = 1;
					break;
				case 12:
					Direction = 1;
					OpenBit = 1;
					UpsideDownBit = 0;
					break;
				case 13:
					Direction = 1;
					OpenBit = 0;
					UpsideDownBit = 0;
					break;
				case 14:
					Direction = 0;
					OpenBit = 1;
					UpsideDownBit = 1;
					break;
				case 15:
					Direction = 0;
					OpenBit = 0;
					UpsideDownBit = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Direction == 2 && b.OpenBit == 0 && b.UpsideDownBit == 1:
					return 0;
				case { } b when true && b.Direction == 3 && b.OpenBit == 1 && b.UpsideDownBit == 1:
					return 1;
				case { } b when true && b.Direction == 0 && b.OpenBit == 1 && b.UpsideDownBit == 0:
					return 2;
				case { } b when true && b.Direction == 1 && b.OpenBit == 0 && b.UpsideDownBit == 1:
					return 3;
				case { } b when true && b.Direction == 3 && b.OpenBit == 0 && b.UpsideDownBit == 1:
					return 4;
				case { } b when true && b.Direction == 3 && b.OpenBit == 1 && b.UpsideDownBit == 0:
					return 5;
				case { } b when true && b.Direction == 2 && b.OpenBit == 0 && b.UpsideDownBit == 0:
					return 6;
				case { } b when true && b.Direction == 1 && b.OpenBit == 1 && b.UpsideDownBit == 1:
					return 7;
				case { } b when true && b.Direction == 2 && b.OpenBit == 1 && b.UpsideDownBit == 1:
					return 8;
				case { } b when true && b.Direction == 2 && b.OpenBit == 1 && b.UpsideDownBit == 0:
					return 9;
				case { } b when true && b.Direction == 3 && b.OpenBit == 0 && b.UpsideDownBit == 0:
					return 10;
				case { } b when true && b.Direction == 0 && b.OpenBit == 0 && b.UpsideDownBit == 1:
					return 11;
				case { } b when true && b.Direction == 1 && b.OpenBit == 1 && b.UpsideDownBit == 0:
					return 12;
				case { } b when true && b.Direction == 1 && b.OpenBit == 0 && b.UpsideDownBit == 0:
					return 13;
				case { } b when true && b.Direction == 0 && b.OpenBit == 1 && b.UpsideDownBit == 1:
					return 14;
				case { } b when true && b.Direction == 0 && b.OpenBit == 0 && b.UpsideDownBit == 0:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class JungleWallSign : Block // 0 typeof=Block
	{
		[Range(0, 5)] public int FacingDirection { get; set; }

		public JungleWallSign() : base(444)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					FacingDirection = 2;
					break;
				case 1:
					FacingDirection = 0;
					break;
				case 2:
					FacingDirection = 4;
					break;
				case 3:
					FacingDirection = 5;
					break;
				case 4:
					FacingDirection = 3;
					break;
				case 5:
					FacingDirection = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.FacingDirection == 2:
					return 0;
				case { } b when true && b.FacingDirection == 0:
					return 1;
				case { } b when true && b.FacingDirection == 4:
					return 2;
				case { } b when true && b.FacingDirection == 5:
					return 3;
				case { } b when true && b.FacingDirection == 3:
					return 4;
				case { } b when true && b.FacingDirection == 1:
					return 5;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Kelp : Block // 0 typeof=Block
	{
		[Range(0, 9)] public int KelpAge { get; set; }

		public Kelp() : base(393)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					KelpAge = 11;
					break;
				case 1:
					KelpAge = 19;
					break;
				case 2:
					KelpAge = 17;
					break;
				case 3:
					KelpAge = 22;
					break;
				case 4:
					KelpAge = 6;
					break;
				case 5:
					KelpAge = 14;
					break;
				case 6:
					KelpAge = 0;
					break;
				case 7:
					KelpAge = 18;
					break;
				case 8:
					KelpAge = 8;
					break;
				case 9:
					KelpAge = 25;
					break;
				case 10:
					KelpAge = 2;
					break;
				case 11:
					KelpAge = 13;
					break;
				case 12:
					KelpAge = 4;
					break;
				case 13:
					KelpAge = 10;
					break;
				case 14:
					KelpAge = 15;
					break;
				case 15:
					KelpAge = 1;
					break;
				case 16:
					KelpAge = 23;
					break;
				case 17:
					KelpAge = 20;
					break;
				case 18:
					KelpAge = 9;
					break;
				case 19:
					KelpAge = 21;
					break;
				case 20:
					KelpAge = 24;
					break;
				case 21:
					KelpAge = 16;
					break;
				case 22:
					KelpAge = 3;
					break;
				case 23:
					KelpAge = 5;
					break;
				case 24:
					KelpAge = 7;
					break;
				case 25:
					KelpAge = 12;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.KelpAge == 11:
					return 0;
				case { } b when true && b.KelpAge == 19:
					return 1;
				case { } b when true && b.KelpAge == 17:
					return 2;
				case { } b when true && b.KelpAge == 22:
					return 3;
				case { } b when true && b.KelpAge == 6:
					return 4;
				case { } b when true && b.KelpAge == 14:
					return 5;
				case { } b when true && b.KelpAge == 0:
					return 6;
				case { } b when true && b.KelpAge == 18:
					return 7;
				case { } b when true && b.KelpAge == 8:
					return 8;
				case { } b when true && b.KelpAge == 25:
					return 9;
				case { } b when true && b.KelpAge == 2:
					return 10;
				case { } b when true && b.KelpAge == 13:
					return 11;
				case { } b when true && b.KelpAge == 4:
					return 12;
				case { } b when true && b.KelpAge == 10:
					return 13;
				case { } b when true && b.KelpAge == 15:
					return 14;
				case { } b when true && b.KelpAge == 1:
					return 15;
				case { } b when true && b.KelpAge == 23:
					return 16;
				case { } b when true && b.KelpAge == 20:
					return 17;
				case { } b when true && b.KelpAge == 9:
					return 18;
				case { } b when true && b.KelpAge == 21:
					return 19;
				case { } b when true && b.KelpAge == 24:
					return 20;
				case { } b when true && b.KelpAge == 16:
					return 21;
				case { } b when true && b.KelpAge == 3:
					return 22;
				case { } b when true && b.KelpAge == 5:
					return 23;
				case { } b when true && b.KelpAge == 7:
					return 24;
				case { } b when true && b.KelpAge == 12:
					return 25;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Ladder // 65 typeof=Ladder
	{
		[Range(0, 5)] public int FacingDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					FacingDirection = 4;
					break;
				case 1:
					FacingDirection = 1;
					break;
				case 2:
					FacingDirection = 3;
					break;
				case 3:
					FacingDirection = 0;
					break;
				case 4:
					FacingDirection = 2;
					break;
				case 5:
					FacingDirection = 5;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.FacingDirection == 4:
					return 0;
				case { } b when true && b.FacingDirection == 1:
					return 1;
				case { } b when true && b.FacingDirection == 3:
					return 2;
				case { } b when true && b.FacingDirection == 0:
					return 3;
				case { } b when true && b.FacingDirection == 2:
					return 4;
				case { } b when true && b.FacingDirection == 5:
					return 5;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Lantern : Block // 0 typeof=Block
	{
		[Range(0, 1)] public byte Hanging { get; set; }

		public Lantern() : base(463)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Hanging = 0;
					break;
				case 1:
					Hanging = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Hanging == 0:
					return 0;
				case { } b when true && b.Hanging == 1:
					return 1;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class LapisBlock // 22 typeof=LapisBlock
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class LapisOre // 21 typeof=LapisOre
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Lava // 11 typeof=Lava
	{
		[Range(0, 9)] public int LiquidDepth { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					LiquidDepth = 14;
					break;
				case 1:
					LiquidDepth = 15;
					break;
				case 2:
					LiquidDepth = 4;
					break;
				case 3:
					LiquidDepth = 5;
					break;
				case 4:
					LiquidDepth = 1;
					break;
				case 5:
					LiquidDepth = 6;
					break;
				case 6:
					LiquidDepth = 7;
					break;
				case 7:
					LiquidDepth = 2;
					break;
				case 8:
					LiquidDepth = 10;
					break;
				case 9:
					LiquidDepth = 3;
					break;
				case 10:
					LiquidDepth = 0;
					break;
				case 11:
					LiquidDepth = 9;
					break;
				case 12:
					LiquidDepth = 12;
					break;
				case 13:
					LiquidDepth = 13;
					break;
				case 14:
					LiquidDepth = 11;
					break;
				case 15:
					LiquidDepth = 8;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.LiquidDepth == 14:
					return 0;
				case { } b when true && b.LiquidDepth == 15:
					return 1;
				case { } b when true && b.LiquidDepth == 4:
					return 2;
				case { } b when true && b.LiquidDepth == 5:
					return 3;
				case { } b when true && b.LiquidDepth == 1:
					return 4;
				case { } b when true && b.LiquidDepth == 6:
					return 5;
				case { } b when true && b.LiquidDepth == 7:
					return 6;
				case { } b when true && b.LiquidDepth == 2:
					return 7;
				case { } b when true && b.LiquidDepth == 10:
					return 8;
				case { } b when true && b.LiquidDepth == 3:
					return 9;
				case { } b when true && b.LiquidDepth == 0:
					return 10;
				case { } b when true && b.LiquidDepth == 9:
					return 11;
				case { } b when true && b.LiquidDepth == 12:
					return 12;
				case { } b when true && b.LiquidDepth == 13:
					return 13;
				case { } b when true && b.LiquidDepth == 11:
					return 14;
				case { } b when true && b.LiquidDepth == 8:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class LavaCauldron : Block // 0 typeof=Block
	{
		// Convert this attribute to enum
		//[Enum("lava","water"]
		public string CauldronLiquid { get; set; }
		[Range(0, 6)] public int FillLevel { get; set; }

		public LavaCauldron() : base(465)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					CauldronLiquid = "lava";
					FillLevel = 4;
					break;
				case 1:
					CauldronLiquid = "lava";
					FillLevel = 3;
					break;
				case 2:
					CauldronLiquid = "water";
					FillLevel = 2;
					break;
				case 3:
					CauldronLiquid = "water";
					FillLevel = 5;
					break;
				case 4:
					CauldronLiquid = "water";
					FillLevel = 3;
					break;
				case 5:
					CauldronLiquid = "water";
					FillLevel = 6;
					break;
				case 6:
					CauldronLiquid = "lava";
					FillLevel = 1;
					break;
				case 7:
					CauldronLiquid = "lava";
					FillLevel = 5;
					break;
				case 8:
					CauldronLiquid = "water";
					FillLevel = 4;
					break;
				case 9:
					CauldronLiquid = "lava";
					FillLevel = 6;
					break;
				case 10:
					CauldronLiquid = "water";
					FillLevel = 1;
					break;
				case 11:
					CauldronLiquid = "water";
					FillLevel = 0;
					break;
				case 12:
					CauldronLiquid = "lava";
					FillLevel = 2;
					break;
				case 13:
					CauldronLiquid = "lava";
					FillLevel = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.CauldronLiquid == "lava" && b.FillLevel == 4:
					return 0;
				case { } b when true && b.CauldronLiquid == "lava" && b.FillLevel == 3:
					return 1;
				case { } b when true && b.CauldronLiquid == "water" && b.FillLevel == 2:
					return 2;
				case { } b when true && b.CauldronLiquid == "water" && b.FillLevel == 5:
					return 3;
				case { } b when true && b.CauldronLiquid == "water" && b.FillLevel == 3:
					return 4;
				case { } b when true && b.CauldronLiquid == "water" && b.FillLevel == 6:
					return 5;
				case { } b when true && b.CauldronLiquid == "lava" && b.FillLevel == 1:
					return 6;
				case { } b when true && b.CauldronLiquid == "lava" && b.FillLevel == 5:
					return 7;
				case { } b when true && b.CauldronLiquid == "water" && b.FillLevel == 4:
					return 8;
				case { } b when true && b.CauldronLiquid == "lava" && b.FillLevel == 6:
					return 9;
				case { } b when true && b.CauldronLiquid == "water" && b.FillLevel == 1:
					return 10;
				case { } b when true && b.CauldronLiquid == "water" && b.FillLevel == 0:
					return 11;
				case { } b when true && b.CauldronLiquid == "lava" && b.FillLevel == 2:
					return 12;
				case { } b when true && b.CauldronLiquid == "lava" && b.FillLevel == 0:
					return 13;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Leaves // 18 typeof=Leaves
	{
		// Convert this attribute to enum
		//[Enum("birch","jungle","oak","spruce"]
		public string OldLeafType { get; set; }
		[Range(0, 1)] public byte PersistentBit { get; set; }
		[Range(0, 1)] public byte UpdateBit { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					OldLeafType = "birch";
					PersistentBit = 0;
					UpdateBit = 1;
					break;
				case 1:
					OldLeafType = "oak";
					PersistentBit = 1;
					UpdateBit = 1;
					break;
				case 2:
					OldLeafType = "birch";
					PersistentBit = 1;
					UpdateBit = 1;
					break;
				case 3:
					OldLeafType = "jungle";
					PersistentBit = 0;
					UpdateBit = 0;
					break;
				case 4:
					OldLeafType = "jungle";
					PersistentBit = 0;
					UpdateBit = 1;
					break;
				case 5:
					OldLeafType = "oak";
					PersistentBit = 0;
					UpdateBit = 0;
					break;
				case 6:
					OldLeafType = "jungle";
					PersistentBit = 1;
					UpdateBit = 1;
					break;
				case 7:
					OldLeafType = "oak";
					PersistentBit = 0;
					UpdateBit = 1;
					break;
				case 8:
					OldLeafType = "spruce";
					PersistentBit = 0;
					UpdateBit = 0;
					break;
				case 9:
					OldLeafType = "spruce";
					PersistentBit = 1;
					UpdateBit = 1;
					break;
				case 10:
					OldLeafType = "birch";
					PersistentBit = 0;
					UpdateBit = 0;
					break;
				case 11:
					OldLeafType = "oak";
					PersistentBit = 1;
					UpdateBit = 0;
					break;
				case 12:
					OldLeafType = "jungle";
					PersistentBit = 1;
					UpdateBit = 0;
					break;
				case 13:
					OldLeafType = "birch";
					PersistentBit = 1;
					UpdateBit = 0;
					break;
				case 14:
					OldLeafType = "spruce";
					PersistentBit = 0;
					UpdateBit = 1;
					break;
				case 15:
					OldLeafType = "spruce";
					PersistentBit = 1;
					UpdateBit = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.OldLeafType == "birch" && b.PersistentBit == 0 && b.UpdateBit == 1:
					return 0;
				case { } b when true && b.OldLeafType == "oak" && b.PersistentBit == 1 && b.UpdateBit == 1:
					return 1;
				case { } b when true && b.OldLeafType == "birch" && b.PersistentBit == 1 && b.UpdateBit == 1:
					return 2;
				case { } b when true && b.OldLeafType == "jungle" && b.PersistentBit == 0 && b.UpdateBit == 0:
					return 3;
				case { } b when true && b.OldLeafType == "jungle" && b.PersistentBit == 0 && b.UpdateBit == 1:
					return 4;
				case { } b when true && b.OldLeafType == "oak" && b.PersistentBit == 0 && b.UpdateBit == 0:
					return 5;
				case { } b when true && b.OldLeafType == "jungle" && b.PersistentBit == 1 && b.UpdateBit == 1:
					return 6;
				case { } b when true && b.OldLeafType == "oak" && b.PersistentBit == 0 && b.UpdateBit == 1:
					return 7;
				case { } b when true && b.OldLeafType == "spruce" && b.PersistentBit == 0 && b.UpdateBit == 0:
					return 8;
				case { } b when true && b.OldLeafType == "spruce" && b.PersistentBit == 1 && b.UpdateBit == 1:
					return 9;
				case { } b when true && b.OldLeafType == "birch" && b.PersistentBit == 0 && b.UpdateBit == 0:
					return 10;
				case { } b when true && b.OldLeafType == "oak" && b.PersistentBit == 1 && b.UpdateBit == 0:
					return 11;
				case { } b when true && b.OldLeafType == "jungle" && b.PersistentBit == 1 && b.UpdateBit == 0:
					return 12;
				case { } b when true && b.OldLeafType == "birch" && b.PersistentBit == 1 && b.UpdateBit == 0:
					return 13;
				case { } b when true && b.OldLeafType == "spruce" && b.PersistentBit == 0 && b.UpdateBit == 1:
					return 14;
				case { } b when true && b.OldLeafType == "spruce" && b.PersistentBit == 1 && b.UpdateBit == 0:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Leaves2 // 161 typeof=Leaves2
	{
		// Convert this attribute to enum
		//[Enum("acacia","dark_oak"]
		public string NewLeafType { get; set; }
		[Range(0, 1)] public byte PersistentBit { get; set; }
		[Range(0, 1)] public byte UpdateBit { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					NewLeafType = "dark_oak";
					PersistentBit = 0;
					UpdateBit = 0;
					break;
				case 1:
					NewLeafType = "dark_oak";
					PersistentBit = 1;
					UpdateBit = 0;
					break;
				case 2:
					NewLeafType = "dark_oak";
					PersistentBit = 0;
					UpdateBit = 1;
					break;
				case 3:
					NewLeafType = "acacia";
					PersistentBit = 1;
					UpdateBit = 0;
					break;
				case 4:
					NewLeafType = "acacia";
					PersistentBit = 1;
					UpdateBit = 1;
					break;
				case 5:
					NewLeafType = "acacia";
					PersistentBit = 0;
					UpdateBit = 1;
					break;
				case 6:
					NewLeafType = "dark_oak";
					PersistentBit = 1;
					UpdateBit = 1;
					break;
				case 7:
					NewLeafType = "acacia";
					PersistentBit = 0;
					UpdateBit = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.NewLeafType == "dark_oak" && b.PersistentBit == 0 && b.UpdateBit == 0:
					return 0;
				case { } b when true && b.NewLeafType == "dark_oak" && b.PersistentBit == 1 && b.UpdateBit == 0:
					return 1;
				case { } b when true && b.NewLeafType == "dark_oak" && b.PersistentBit == 0 && b.UpdateBit == 1:
					return 2;
				case { } b when true && b.NewLeafType == "acacia" && b.PersistentBit == 1 && b.UpdateBit == 0:
					return 3;
				case { } b when true && b.NewLeafType == "acacia" && b.PersistentBit == 1 && b.UpdateBit == 1:
					return 4;
				case { } b when true && b.NewLeafType == "acacia" && b.PersistentBit == 0 && b.UpdateBit == 1:
					return 5;
				case { } b when true && b.NewLeafType == "dark_oak" && b.PersistentBit == 1 && b.UpdateBit == 1:
					return 6;
				case { } b when true && b.NewLeafType == "acacia" && b.PersistentBit == 0 && b.UpdateBit == 0:
					return 7;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Lectern : Block // 0 typeof=Block
	{
		[Range(0, 3)] public int Direction { get; set; }
		[Range(0, 1)] public byte PoweredBit { get; set; }

		public Lectern() : base(449)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Direction = 3;
					PoweredBit = 0;
					break;
				case 1:
					Direction = 2;
					PoweredBit = 1;
					break;
				case 2:
					Direction = 0;
					PoweredBit = 0;
					break;
				case 3:
					Direction = 1;
					PoweredBit = 0;
					break;
				case 4:
					Direction = 0;
					PoweredBit = 1;
					break;
				case 5:
					Direction = 3;
					PoweredBit = 1;
					break;
				case 6:
					Direction = 2;
					PoweredBit = 0;
					break;
				case 7:
					Direction = 1;
					PoweredBit = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Direction == 3 && b.PoweredBit == 0:
					return 0;
				case { } b when true && b.Direction == 2 && b.PoweredBit == 1:
					return 1;
				case { } b when true && b.Direction == 0 && b.PoweredBit == 0:
					return 2;
				case { } b when true && b.Direction == 1 && b.PoweredBit == 0:
					return 3;
				case { } b when true && b.Direction == 0 && b.PoweredBit == 1:
					return 4;
				case { } b when true && b.Direction == 3 && b.PoweredBit == 1:
					return 5;
				case { } b when true && b.Direction == 2 && b.PoweredBit == 0:
					return 6;
				case { } b when true && b.Direction == 1 && b.PoweredBit == 1:
					return 7;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Lever // 69 typeof=Lever
	{
		// Convert this attribute to enum
		//[Enum("down_east_west","down_north_south","east","north","south","up_east_west","up_north_south","west"]
		public string LeverDirection { get; set; }
		[Range(0, 1)] public byte OpenBit { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					LeverDirection = "up_north_south";
					OpenBit = 0;
					break;
				case 1:
					LeverDirection = "south";
					OpenBit = 1;
					break;
				case 2:
					LeverDirection = "south";
					OpenBit = 0;
					break;
				case 3:
					LeverDirection = "down_east_west";
					OpenBit = 1;
					break;
				case 4:
					LeverDirection = "up_east_west";
					OpenBit = 0;
					break;
				case 5:
					LeverDirection = "down_east_west";
					OpenBit = 0;
					break;
				case 6:
					LeverDirection = "down_north_south";
					OpenBit = 0;
					break;
				case 7:
					LeverDirection = "east";
					OpenBit = 0;
					break;
				case 8:
					LeverDirection = "up_east_west";
					OpenBit = 1;
					break;
				case 9:
					LeverDirection = "up_north_south";
					OpenBit = 1;
					break;
				case 10:
					LeverDirection = "north";
					OpenBit = 0;
					break;
				case 11:
					LeverDirection = "west";
					OpenBit = 1;
					break;
				case 12:
					LeverDirection = "north";
					OpenBit = 1;
					break;
				case 13:
					LeverDirection = "west";
					OpenBit = 0;
					break;
				case 14:
					LeverDirection = "east";
					OpenBit = 1;
					break;
				case 15:
					LeverDirection = "down_north_south";
					OpenBit = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.LeverDirection == "up_north_south" && b.OpenBit == 0:
					return 0;
				case { } b when true && b.LeverDirection == "south" && b.OpenBit == 1:
					return 1;
				case { } b when true && b.LeverDirection == "south" && b.OpenBit == 0:
					return 2;
				case { } b when true && b.LeverDirection == "down_east_west" && b.OpenBit == 1:
					return 3;
				case { } b when true && b.LeverDirection == "up_east_west" && b.OpenBit == 0:
					return 4;
				case { } b when true && b.LeverDirection == "down_east_west" && b.OpenBit == 0:
					return 5;
				case { } b when true && b.LeverDirection == "down_north_south" && b.OpenBit == 0:
					return 6;
				case { } b when true && b.LeverDirection == "east" && b.OpenBit == 0:
					return 7;
				case { } b when true && b.LeverDirection == "up_east_west" && b.OpenBit == 1:
					return 8;
				case { } b when true && b.LeverDirection == "up_north_south" && b.OpenBit == 1:
					return 9;
				case { } b when true && b.LeverDirection == "north" && b.OpenBit == 0:
					return 10;
				case { } b when true && b.LeverDirection == "west" && b.OpenBit == 1:
					return 11;
				case { } b when true && b.LeverDirection == "north" && b.OpenBit == 1:
					return 12;
				case { } b when true && b.LeverDirection == "west" && b.OpenBit == 0:
					return 13;
				case { } b when true && b.LeverDirection == "east" && b.OpenBit == 1:
					return 14;
				case { } b when true && b.LeverDirection == "down_north_south" && b.OpenBit == 1:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class LightBlock : Block // 0 typeof=Block
	{
		[Range(0, 9)] public int BlockLightLevel { get; set; }

		public LightBlock() : base(470)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					BlockLightLevel = 0;
					break;
				case 1:
					BlockLightLevel = 9;
					break;
				case 2:
					BlockLightLevel = 14;
					break;
				case 3:
					BlockLightLevel = 2;
					break;
				case 4:
					BlockLightLevel = 5;
					break;
				case 5:
					BlockLightLevel = 4;
					break;
				case 6:
					BlockLightLevel = 12;
					break;
				case 7:
					BlockLightLevel = 11;
					break;
				case 8:
					BlockLightLevel = 3;
					break;
				case 9:
					BlockLightLevel = 1;
					break;
				case 10:
					BlockLightLevel = 7;
					break;
				case 11:
					BlockLightLevel = 6;
					break;
				case 12:
					BlockLightLevel = 8;
					break;
				case 13:
					BlockLightLevel = 10;
					break;
				case 14:
					BlockLightLevel = 15;
					break;
				case 15:
					BlockLightLevel = 13;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.BlockLightLevel == 0:
					return 0;
				case { } b when true && b.BlockLightLevel == 9:
					return 1;
				case { } b when true && b.BlockLightLevel == 14:
					return 2;
				case { } b when true && b.BlockLightLevel == 2:
					return 3;
				case { } b when true && b.BlockLightLevel == 5:
					return 4;
				case { } b when true && b.BlockLightLevel == 4:
					return 5;
				case { } b when true && b.BlockLightLevel == 12:
					return 6;
				case { } b when true && b.BlockLightLevel == 11:
					return 7;
				case { } b when true && b.BlockLightLevel == 3:
					return 8;
				case { } b when true && b.BlockLightLevel == 1:
					return 9;
				case { } b when true && b.BlockLightLevel == 7:
					return 10;
				case { } b when true && b.BlockLightLevel == 6:
					return 11;
				case { } b when true && b.BlockLightLevel == 8:
					return 12;
				case { } b when true && b.BlockLightLevel == 10:
					return 13;
				case { } b when true && b.BlockLightLevel == 15:
					return 14;
				case { } b when true && b.BlockLightLevel == 13:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class LightBlueGlazedTerracotta // 223 typeof=LightBlueGlazedTerracotta
	{
		[Range(0, 5)] public int FacingDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					FacingDirection = 3;
					break;
				case 1:
					FacingDirection = 1;
					break;
				case 2:
					FacingDirection = 0;
					break;
				case 3:
					FacingDirection = 5;
					break;
				case 4:
					FacingDirection = 4;
					break;
				case 5:
					FacingDirection = 2;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.FacingDirection == 3:
					return 0;
				case { } b when true && b.FacingDirection == 1:
					return 1;
				case { } b when true && b.FacingDirection == 0:
					return 2;
				case { } b when true && b.FacingDirection == 5:
					return 3;
				case { } b when true && b.FacingDirection == 4:
					return 4;
				case { } b when true && b.FacingDirection == 2:
					return 5;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class LightWeightedPressurePlate // 147 typeof=LightWeightedPressurePlate
	{
		[Range(0, 9)] public int RedstoneSignal { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					RedstoneSignal = 12;
					break;
				case 1:
					RedstoneSignal = 7;
					break;
				case 2:
					RedstoneSignal = 8;
					break;
				case 3:
					RedstoneSignal = 10;
					break;
				case 4:
					RedstoneSignal = 14;
					break;
				case 5:
					RedstoneSignal = 0;
					break;
				case 6:
					RedstoneSignal = 13;
					break;
				case 7:
					RedstoneSignal = 3;
					break;
				case 8:
					RedstoneSignal = 5;
					break;
				case 9:
					RedstoneSignal = 15;
					break;
				case 10:
					RedstoneSignal = 1;
					break;
				case 11:
					RedstoneSignal = 2;
					break;
				case 12:
					RedstoneSignal = 4;
					break;
				case 13:
					RedstoneSignal = 6;
					break;
				case 14:
					RedstoneSignal = 11;
					break;
				case 15:
					RedstoneSignal = 9;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.RedstoneSignal == 12:
					return 0;
				case { } b when true && b.RedstoneSignal == 7:
					return 1;
				case { } b when true && b.RedstoneSignal == 8:
					return 2;
				case { } b when true && b.RedstoneSignal == 10:
					return 3;
				case { } b when true && b.RedstoneSignal == 14:
					return 4;
				case { } b when true && b.RedstoneSignal == 0:
					return 5;
				case { } b when true && b.RedstoneSignal == 13:
					return 6;
				case { } b when true && b.RedstoneSignal == 3:
					return 7;
				case { } b when true && b.RedstoneSignal == 5:
					return 8;
				case { } b when true && b.RedstoneSignal == 15:
					return 9;
				case { } b when true && b.RedstoneSignal == 1:
					return 10;
				case { } b when true && b.RedstoneSignal == 2:
					return 11;
				case { } b when true && b.RedstoneSignal == 4:
					return 12;
				case { } b when true && b.RedstoneSignal == 6:
					return 13;
				case { } b when true && b.RedstoneSignal == 11:
					return 14;
				case { } b when true && b.RedstoneSignal == 9:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class LimeGlazedTerracotta // 225 typeof=LimeGlazedTerracotta
	{
		[Range(0, 5)] public int FacingDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					FacingDirection = 2;
					break;
				case 1:
					FacingDirection = 3;
					break;
				case 2:
					FacingDirection = 1;
					break;
				case 3:
					FacingDirection = 0;
					break;
				case 4:
					FacingDirection = 4;
					break;
				case 5:
					FacingDirection = 5;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.FacingDirection == 2:
					return 0;
				case { } b when true && b.FacingDirection == 3:
					return 1;
				case { } b when true && b.FacingDirection == 1:
					return 2;
				case { } b when true && b.FacingDirection == 0:
					return 3;
				case { } b when true && b.FacingDirection == 4:
					return 4;
				case { } b when true && b.FacingDirection == 5:
					return 5;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class LitBlastFurnace : Block // 0 typeof=Block
	{
		[Range(0, 5)] public int FacingDirection { get; set; }

		public LitBlastFurnace() : base(469)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					FacingDirection = 5;
					break;
				case 1:
					FacingDirection = 3;
					break;
				case 2:
					FacingDirection = 2;
					break;
				case 3:
					FacingDirection = 4;
					break;
				case 4:
					FacingDirection = 0;
					break;
				case 5:
					FacingDirection = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.FacingDirection == 5:
					return 0;
				case { } b when true && b.FacingDirection == 3:
					return 1;
				case { } b when true && b.FacingDirection == 2:
					return 2;
				case { } b when true && b.FacingDirection == 4:
					return 3;
				case { } b when true && b.FacingDirection == 0:
					return 4;
				case { } b when true && b.FacingDirection == 1:
					return 5;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class LitFurnace // 62 typeof=LitFurnace
	{
		[Range(0, 5)] public int FacingDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					FacingDirection = 4;
					break;
				case 1:
					FacingDirection = 3;
					break;
				case 2:
					FacingDirection = 2;
					break;
				case 3:
					FacingDirection = 0;
					break;
				case 4:
					FacingDirection = 5;
					break;
				case 5:
					FacingDirection = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.FacingDirection == 4:
					return 0;
				case { } b when true && b.FacingDirection == 3:
					return 1;
				case { } b when true && b.FacingDirection == 2:
					return 2;
				case { } b when true && b.FacingDirection == 0:
					return 3;
				case { } b when true && b.FacingDirection == 5:
					return 4;
				case { } b when true && b.FacingDirection == 1:
					return 5;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class LitPumpkin // 91 typeof=LitPumpkin
	{
		[Range(0, 3)] public int Direction { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Direction = 0;
					break;
				case 1:
					Direction = 1;
					break;
				case 2:
					Direction = 2;
					break;
				case 3:
					Direction = 3;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Direction == 0:
					return 0;
				case { } b when true && b.Direction == 1:
					return 1;
				case { } b when true && b.Direction == 2:
					return 2;
				case { } b when true && b.Direction == 3:
					return 3;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class LitRedstoneLamp // 124 typeof=LitRedstoneLamp
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class LitRedstoneOre // 74 typeof=LitRedstoneOre
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class LitSmoker : Block // 0 typeof=Block
	{
		[Range(0, 5)] public int FacingDirection { get; set; }

		public LitSmoker() : base(454)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					FacingDirection = 0;
					break;
				case 1:
					FacingDirection = 2;
					break;
				case 2:
					FacingDirection = 1;
					break;
				case 3:
					FacingDirection = 4;
					break;
				case 4:
					FacingDirection = 3;
					break;
				case 5:
					FacingDirection = 5;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.FacingDirection == 0:
					return 0;
				case { } b when true && b.FacingDirection == 2:
					return 1;
				case { } b when true && b.FacingDirection == 1:
					return 2;
				case { } b when true && b.FacingDirection == 4:
					return 3;
				case { } b when true && b.FacingDirection == 3:
					return 4;
				case { } b when true && b.FacingDirection == 5:
					return 5;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Log // 17 typeof=Log
	{
		// Convert this attribute to enum
		//[Enum("birch","jungle","oak","spruce"]
		public string OldLogType { get; set; }

		// Convert this attribute to enum
		//[Enum("x","y","z"]
		public string PillarAxis { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					OldLogType = "jungle";
					PillarAxis = "x";
					break;
				case 1:
					OldLogType = "birch";
					PillarAxis = "z";
					break;
				case 2:
					OldLogType = "spruce";
					PillarAxis = "y";
					break;
				case 3:
					OldLogType = "spruce";
					PillarAxis = "x";
					break;
				case 4:
					OldLogType = "oak";
					PillarAxis = "z";
					break;
				case 5:
					OldLogType = "spruce";
					PillarAxis = "z";
					break;
				case 6:
					OldLogType = "oak";
					PillarAxis = "x";
					break;
				case 7:
					OldLogType = "jungle";
					PillarAxis = "y";
					break;
				case 8:
					OldLogType = "jungle";
					PillarAxis = "z";
					break;
				case 9:
					OldLogType = "oak";
					PillarAxis = "y";
					break;
				case 10:
					OldLogType = "birch";
					PillarAxis = "x";
					break;
				case 11:
					OldLogType = "birch";
					PillarAxis = "y";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.OldLogType == "jungle" && b.PillarAxis == "x":
					return 0;
				case { } b when true && b.OldLogType == "birch" && b.PillarAxis == "z":
					return 1;
				case { } b when true && b.OldLogType == "spruce" && b.PillarAxis == "y":
					return 2;
				case { } b when true && b.OldLogType == "spruce" && b.PillarAxis == "x":
					return 3;
				case { } b when true && b.OldLogType == "oak" && b.PillarAxis == "z":
					return 4;
				case { } b when true && b.OldLogType == "spruce" && b.PillarAxis == "z":
					return 5;
				case { } b when true && b.OldLogType == "oak" && b.PillarAxis == "x":
					return 6;
				case { } b when true && b.OldLogType == "jungle" && b.PillarAxis == "y":
					return 7;
				case { } b when true && b.OldLogType == "jungle" && b.PillarAxis == "z":
					return 8;
				case { } b when true && b.OldLogType == "oak" && b.PillarAxis == "y":
					return 9;
				case { } b when true && b.OldLogType == "birch" && b.PillarAxis == "x":
					return 10;
				case { } b when true && b.OldLogType == "birch" && b.PillarAxis == "y":
					return 11;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Log2 // 162 typeof=Log2
	{
		// Convert this attribute to enum
		//[Enum("acacia","dark_oak"]
		public string NewLogType { get; set; }

		// Convert this attribute to enum
		//[Enum("x","y","z"]
		public string PillarAxis { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					NewLogType = "dark_oak";
					PillarAxis = "y";
					break;
				case 1:
					NewLogType = "acacia";
					PillarAxis = "x";
					break;
				case 2:
					NewLogType = "dark_oak";
					PillarAxis = "x";
					break;
				case 3:
					NewLogType = "acacia";
					PillarAxis = "z";
					break;
				case 4:
					NewLogType = "acacia";
					PillarAxis = "y";
					break;
				case 5:
					NewLogType = "dark_oak";
					PillarAxis = "z";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.NewLogType == "dark_oak" && b.PillarAxis == "y":
					return 0;
				case { } b when true && b.NewLogType == "acacia" && b.PillarAxis == "x":
					return 1;
				case { } b when true && b.NewLogType == "dark_oak" && b.PillarAxis == "x":
					return 2;
				case { } b when true && b.NewLogType == "acacia" && b.PillarAxis == "z":
					return 3;
				case { } b when true && b.NewLogType == "acacia" && b.PillarAxis == "y":
					return 4;
				case { } b when true && b.NewLogType == "dark_oak" && b.PillarAxis == "z":
					return 5;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Loom : Block // 0 typeof=Block
	{
		[Range(0, 3)] public int Direction { get; set; }

		public Loom() : base(459)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Direction = 1;
					break;
				case 1:
					Direction = 2;
					break;
				case 2:
					Direction = 3;
					break;
				case 3:
					Direction = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Direction == 1:
					return 0;
				case { } b when true && b.Direction == 2:
					return 1;
				case { } b when true && b.Direction == 3:
					return 2;
				case { } b when true && b.Direction == 0:
					return 3;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class MagentaGlazedTerracotta // 222 typeof=MagentaGlazedTerracotta
	{
		[Range(0, 5)] public int FacingDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					FacingDirection = 2;
					break;
				case 1:
					FacingDirection = 1;
					break;
				case 2:
					FacingDirection = 5;
					break;
				case 3:
					FacingDirection = 3;
					break;
				case 4:
					FacingDirection = 4;
					break;
				case 5:
					FacingDirection = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.FacingDirection == 2:
					return 0;
				case { } b when true && b.FacingDirection == 1:
					return 1;
				case { } b when true && b.FacingDirection == 5:
					return 2;
				case { } b when true && b.FacingDirection == 3:
					return 3;
				case { } b when true && b.FacingDirection == 4:
					return 4;
				case { } b when true && b.FacingDirection == 0:
					return 5;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Magma : Block // 0 typeof=Block
	{
		public Magma() : base(213)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class MelonBlock // 103 typeof=MelonBlock
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class MelonStem // 105 typeof=MelonStem
	{
		[Range(0, 7)] public int Growth { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Growth = 4;
					break;
				case 1:
					Growth = 1;
					break;
				case 2:
					Growth = 7;
					break;
				case 3:
					Growth = 5;
					break;
				case 4:
					Growth = 2;
					break;
				case 5:
					Growth = 6;
					break;
				case 6:
					Growth = 0;
					break;
				case 7:
					Growth = 3;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Growth == 4:
					return 0;
				case { } b when true && b.Growth == 1:
					return 1;
				case { } b when true && b.Growth == 7:
					return 2;
				case { } b when true && b.Growth == 5:
					return 3;
				case { } b when true && b.Growth == 2:
					return 4;
				case { } b when true && b.Growth == 6:
					return 5;
				case { } b when true && b.Growth == 0:
					return 6;
				case { } b when true && b.Growth == 3:
					return 7;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class MobSpawner // 52 typeof=MobSpawner
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class MonsterEgg // 97 typeof=MonsterEgg
	{
		// Convert this attribute to enum
		//[Enum("chiseled_stone_brick","cobblestone","cracked_stone_brick","mossy_stone_brick","stone","stone_brick"]
		public string MonsterEggStoneType { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					MonsterEggStoneType = "stone_brick";
					break;
				case 1:
					MonsterEggStoneType = "stone";
					break;
				case 2:
					MonsterEggStoneType = "cobblestone";
					break;
				case 3:
					MonsterEggStoneType = "mossy_stone_brick";
					break;
				case 4:
					MonsterEggStoneType = "chiseled_stone_brick";
					break;
				case 5:
					MonsterEggStoneType = "cracked_stone_brick";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.MonsterEggStoneType == "stone_brick":
					return 0;
				case { } b when true && b.MonsterEggStoneType == "stone":
					return 1;
				case { } b when true && b.MonsterEggStoneType == "cobblestone":
					return 2;
				case { } b when true && b.MonsterEggStoneType == "mossy_stone_brick":
					return 3;
				case { } b when true && b.MonsterEggStoneType == "chiseled_stone_brick":
					return 4;
				case { } b when true && b.MonsterEggStoneType == "cracked_stone_brick":
					return 5;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class MossyCobblestone // 48 typeof=MossyCobblestone
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class MossyCobblestoneStairs : Block // 0 typeof=Block
	{
		[Range(0, 1)] public byte UpsideDownBit { get; set; }
		[Range(0, 3)] public int WeirdoDirection { get; set; }

		public MossyCobblestoneStairs() : base(434)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					UpsideDownBit = 1;
					WeirdoDirection = 1;
					break;
				case 1:
					UpsideDownBit = 0;
					WeirdoDirection = 3;
					break;
				case 2:
					UpsideDownBit = 0;
					WeirdoDirection = 0;
					break;
				case 3:
					UpsideDownBit = 1;
					WeirdoDirection = 3;
					break;
				case 4:
					UpsideDownBit = 1;
					WeirdoDirection = 2;
					break;
				case 5:
					UpsideDownBit = 0;
					WeirdoDirection = 1;
					break;
				case 6:
					UpsideDownBit = 0;
					WeirdoDirection = 2;
					break;
				case 7:
					UpsideDownBit = 1;
					WeirdoDirection = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 1:
					return 0;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 3:
					return 1;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 0:
					return 2;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 3:
					return 3;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 2:
					return 4;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 1:
					return 5;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 2:
					return 6;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 0:
					return 7;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class MossyStoneBrickStairs : Block // 0 typeof=Block
	{
		[Range(0, 1)] public byte UpsideDownBit { get; set; }
		[Range(0, 3)] public int WeirdoDirection { get; set; }

		public MossyStoneBrickStairs() : base(430)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					UpsideDownBit = 0;
					WeirdoDirection = 2;
					break;
				case 1:
					UpsideDownBit = 1;
					WeirdoDirection = 3;
					break;
				case 2:
					UpsideDownBit = 0;
					WeirdoDirection = 3;
					break;
				case 3:
					UpsideDownBit = 0;
					WeirdoDirection = 1;
					break;
				case 4:
					UpsideDownBit = 1;
					WeirdoDirection = 0;
					break;
				case 5:
					UpsideDownBit = 1;
					WeirdoDirection = 1;
					break;
				case 6:
					UpsideDownBit = 1;
					WeirdoDirection = 2;
					break;
				case 7:
					UpsideDownBit = 0;
					WeirdoDirection = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 2:
					return 0;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 3:
					return 1;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 3:
					return 2;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 1:
					return 3;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 0:
					return 4;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 1:
					return 5;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 2:
					return 6;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 0:
					return 7;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class MovingBlock : Block // 0 typeof=Block
	{
		public MovingBlock() : base(250)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Mycelium // 110 typeof=Mycelium
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class NetherBrick // 112 typeof=NetherBrick
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class NetherBrickFence // 113 typeof=NetherBrickFence
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class NetherBrickStairs // 114 typeof=NetherBrickStairs
	{
		[Range(0, 1)] public byte UpsideDownBit { get; set; }
		[Range(0, 3)] public int WeirdoDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					UpsideDownBit = 0;
					WeirdoDirection = 0;
					break;
				case 1:
					UpsideDownBit = 1;
					WeirdoDirection = 0;
					break;
				case 2:
					UpsideDownBit = 0;
					WeirdoDirection = 3;
					break;
				case 3:
					UpsideDownBit = 1;
					WeirdoDirection = 2;
					break;
				case 4:
					UpsideDownBit = 1;
					WeirdoDirection = 1;
					break;
				case 5:
					UpsideDownBit = 1;
					WeirdoDirection = 3;
					break;
				case 6:
					UpsideDownBit = 0;
					WeirdoDirection = 2;
					break;
				case 7:
					UpsideDownBit = 0;
					WeirdoDirection = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 0:
					return 0;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 0:
					return 1;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 3:
					return 2;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 2:
					return 3;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 1:
					return 4;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 3:
					return 5;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 2:
					return 6;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 1:
					return 7;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class NetherWart // 115 typeof=NetherWart
	{
		[Range(0, 3)] public int Age { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Age = 0;
					break;
				case 1:
					Age = 3;
					break;
				case 2:
					Age = 2;
					break;
				case 3:
					Age = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Age == 0:
					return 0;
				case { } b when true && b.Age == 3:
					return 1;
				case { } b when true && b.Age == 2:
					return 2;
				case { } b when true && b.Age == 1:
					return 3;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class NetherWartBlock : Block // 0 typeof=Block
	{
		public NetherWartBlock() : base(214)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Netherrack // 87 typeof=Netherrack
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Netherreactor // 247 typeof=Netherreactor
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class NormalStoneStairs : Block // 0 typeof=Block
	{
		[Range(0, 1)] public byte UpsideDownBit { get; set; }
		[Range(0, 3)] public int WeirdoDirection { get; set; }

		public NormalStoneStairs() : base(435)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					UpsideDownBit = 1;
					WeirdoDirection = 2;
					break;
				case 1:
					UpsideDownBit = 0;
					WeirdoDirection = 1;
					break;
				case 2:
					UpsideDownBit = 1;
					WeirdoDirection = 0;
					break;
				case 3:
					UpsideDownBit = 1;
					WeirdoDirection = 1;
					break;
				case 4:
					UpsideDownBit = 0;
					WeirdoDirection = 3;
					break;
				case 5:
					UpsideDownBit = 0;
					WeirdoDirection = 2;
					break;
				case 6:
					UpsideDownBit = 1;
					WeirdoDirection = 3;
					break;
				case 7:
					UpsideDownBit = 0;
					WeirdoDirection = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 2:
					return 0;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 1:
					return 1;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 0:
					return 2;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 1:
					return 3;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 3:
					return 4;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 2:
					return 5;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 3:
					return 6;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 0:
					return 7;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Noteblock // 25 typeof=NoteBlock
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class OakStairs // 53 typeof=OakStairs
	{
		[Range(0, 1)] public byte UpsideDownBit { get; set; }
		[Range(0, 3)] public int WeirdoDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					UpsideDownBit = 1;
					WeirdoDirection = 1;
					break;
				case 1:
					UpsideDownBit = 0;
					WeirdoDirection = 2;
					break;
				case 2:
					UpsideDownBit = 1;
					WeirdoDirection = 0;
					break;
				case 3:
					UpsideDownBit = 1;
					WeirdoDirection = 3;
					break;
				case 4:
					UpsideDownBit = 0;
					WeirdoDirection = 0;
					break;
				case 5:
					UpsideDownBit = 1;
					WeirdoDirection = 2;
					break;
				case 6:
					UpsideDownBit = 0;
					WeirdoDirection = 1;
					break;
				case 7:
					UpsideDownBit = 0;
					WeirdoDirection = 3;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 1:
					return 0;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 2:
					return 1;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 0:
					return 2;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 3:
					return 3;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 0:
					return 4;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 2:
					return 5;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 1:
					return 6;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 3:
					return 7;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Observer // 251 typeof=Observer
	{
		[Range(0, 5)] public int FacingDirection { get; set; }
		[Range(0, 1)] public byte PoweredBit { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					FacingDirection = 0;
					PoweredBit = 0;
					break;
				case 1:
					FacingDirection = 1;
					PoweredBit = 1;
					break;
				case 2:
					FacingDirection = 5;
					PoweredBit = 1;
					break;
				case 3:
					FacingDirection = 2;
					PoweredBit = 1;
					break;
				case 4:
					FacingDirection = 5;
					PoweredBit = 0;
					break;
				case 5:
					FacingDirection = 2;
					PoweredBit = 0;
					break;
				case 6:
					FacingDirection = 3;
					PoweredBit = 1;
					break;
				case 7:
					FacingDirection = 0;
					PoweredBit = 1;
					break;
				case 8:
					FacingDirection = 1;
					PoweredBit = 0;
					break;
				case 9:
					FacingDirection = 4;
					PoweredBit = 1;
					break;
				case 10:
					FacingDirection = 4;
					PoweredBit = 0;
					break;
				case 11:
					FacingDirection = 3;
					PoweredBit = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.FacingDirection == 0 && b.PoweredBit == 0:
					return 0;
				case { } b when true && b.FacingDirection == 1 && b.PoweredBit == 1:
					return 1;
				case { } b when true && b.FacingDirection == 5 && b.PoweredBit == 1:
					return 2;
				case { } b when true && b.FacingDirection == 2 && b.PoweredBit == 1:
					return 3;
				case { } b when true && b.FacingDirection == 5 && b.PoweredBit == 0:
					return 4;
				case { } b when true && b.FacingDirection == 2 && b.PoweredBit == 0:
					return 5;
				case { } b when true && b.FacingDirection == 3 && b.PoweredBit == 1:
					return 6;
				case { } b when true && b.FacingDirection == 0 && b.PoweredBit == 1:
					return 7;
				case { } b when true && b.FacingDirection == 1 && b.PoweredBit == 0:
					return 8;
				case { } b when true && b.FacingDirection == 4 && b.PoweredBit == 1:
					return 9;
				case { } b when true && b.FacingDirection == 4 && b.PoweredBit == 0:
					return 10;
				case { } b when true && b.FacingDirection == 3 && b.PoweredBit == 0:
					return 11;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Obsidian // 49 typeof=Obsidian
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class OrangeGlazedTerracotta // 221 typeof=OrangeGlazedTerracotta
	{
		[Range(0, 5)] public int FacingDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					FacingDirection = 3;
					break;
				case 1:
					FacingDirection = 2;
					break;
				case 2:
					FacingDirection = 4;
					break;
				case 3:
					FacingDirection = 0;
					break;
				case 4:
					FacingDirection = 1;
					break;
				case 5:
					FacingDirection = 5;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.FacingDirection == 3:
					return 0;
				case { } b when true && b.FacingDirection == 2:
					return 1;
				case { } b when true && b.FacingDirection == 4:
					return 2;
				case { } b when true && b.FacingDirection == 0:
					return 3;
				case { } b when true && b.FacingDirection == 1:
					return 4;
				case { } b when true && b.FacingDirection == 5:
					return 5;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class PackedIce // 174 typeof=PackedIce
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class PinkGlazedTerracotta // 226 typeof=PinkGlazedTerracotta
	{
		[Range(0, 5)] public int FacingDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					FacingDirection = 1;
					break;
				case 1:
					FacingDirection = 5;
					break;
				case 2:
					FacingDirection = 2;
					break;
				case 3:
					FacingDirection = 4;
					break;
				case 4:
					FacingDirection = 3;
					break;
				case 5:
					FacingDirection = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.FacingDirection == 1:
					return 0;
				case { } b when true && b.FacingDirection == 5:
					return 1;
				case { } b when true && b.FacingDirection == 2:
					return 2;
				case { } b when true && b.FacingDirection == 4:
					return 3;
				case { } b when true && b.FacingDirection == 3:
					return 4;
				case { } b when true && b.FacingDirection == 0:
					return 5;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Piston // 33 typeof=Piston
	{
		[Range(0, 5)] public int FacingDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					FacingDirection = 1;
					break;
				case 1:
					FacingDirection = 4;
					break;
				case 2:
					FacingDirection = 3;
					break;
				case 3:
					FacingDirection = 5;
					break;
				case 4:
					FacingDirection = 0;
					break;
				case 5:
					FacingDirection = 2;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.FacingDirection == 1:
					return 0;
				case { } b when true && b.FacingDirection == 4:
					return 1;
				case { } b when true && b.FacingDirection == 3:
					return 2;
				case { } b when true && b.FacingDirection == 5:
					return 3;
				case { } b when true && b.FacingDirection == 0:
					return 4;
				case { } b when true && b.FacingDirection == 2:
					return 5;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class PistonArmCollision // 34 typeof=PistonArmCollision
	{
		[Range(0, 5)] public int FacingDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					FacingDirection = 5;
					break;
				case 1:
					FacingDirection = 1;
					break;
				case 2:
					FacingDirection = 2;
					break;
				case 3:
					FacingDirection = 0;
					break;
				case 4:
					FacingDirection = 3;
					break;
				case 5:
					FacingDirection = 4;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.FacingDirection == 5:
					return 0;
				case { } b when true && b.FacingDirection == 1:
					return 1;
				case { } b when true && b.FacingDirection == 2:
					return 2;
				case { } b when true && b.FacingDirection == 0:
					return 3;
				case { } b when true && b.FacingDirection == 3:
					return 4;
				case { } b when true && b.FacingDirection == 4:
					return 5;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Planks // 5 typeof=Planks
	{
		// Convert this attribute to enum
		//[Enum("acacia","birch","dark_oak","jungle","oak","spruce"]
		public string WoodType { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					WoodType = "acacia";
					break;
				case 1:
					WoodType = "dark_oak";
					break;
				case 2:
					WoodType = "spruce";
					break;
				case 3:
					WoodType = "oak";
					break;
				case 4:
					WoodType = "jungle";
					break;
				case 5:
					WoodType = "birch";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.WoodType == "acacia":
					return 0;
				case { } b when true && b.WoodType == "dark_oak":
					return 1;
				case { } b when true && b.WoodType == "spruce":
					return 2;
				case { } b when true && b.WoodType == "oak":
					return 3;
				case { } b when true && b.WoodType == "jungle":
					return 4;
				case { } b when true && b.WoodType == "birch":
					return 5;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Podzol // 243 typeof=Podzol
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class PolishedAndesiteStairs : Block // 0 typeof=Block
	{
		[Range(0, 1)] public byte UpsideDownBit { get; set; }
		[Range(0, 3)] public int WeirdoDirection { get; set; }

		public PolishedAndesiteStairs() : base(429)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					UpsideDownBit = 1;
					WeirdoDirection = 0;
					break;
				case 1:
					UpsideDownBit = 0;
					WeirdoDirection = 2;
					break;
				case 2:
					UpsideDownBit = 1;
					WeirdoDirection = 3;
					break;
				case 3:
					UpsideDownBit = 0;
					WeirdoDirection = 3;
					break;
				case 4:
					UpsideDownBit = 1;
					WeirdoDirection = 1;
					break;
				case 5:
					UpsideDownBit = 0;
					WeirdoDirection = 1;
					break;
				case 6:
					UpsideDownBit = 1;
					WeirdoDirection = 2;
					break;
				case 7:
					UpsideDownBit = 0;
					WeirdoDirection = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 0:
					return 0;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 2:
					return 1;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 3:
					return 2;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 3:
					return 3;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 1:
					return 4;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 1:
					return 5;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 2:
					return 6;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 0:
					return 7;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class PolishedDioriteStairs : Block // 0 typeof=Block
	{
		[Range(0, 1)] public byte UpsideDownBit { get; set; }
		[Range(0, 3)] public int WeirdoDirection { get; set; }

		public PolishedDioriteStairs() : base(428)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					UpsideDownBit = 1;
					WeirdoDirection = 1;
					break;
				case 1:
					UpsideDownBit = 1;
					WeirdoDirection = 0;
					break;
				case 2:
					UpsideDownBit = 0;
					WeirdoDirection = 0;
					break;
				case 3:
					UpsideDownBit = 1;
					WeirdoDirection = 3;
					break;
				case 4:
					UpsideDownBit = 0;
					WeirdoDirection = 2;
					break;
				case 5:
					UpsideDownBit = 1;
					WeirdoDirection = 2;
					break;
				case 6:
					UpsideDownBit = 0;
					WeirdoDirection = 3;
					break;
				case 7:
					UpsideDownBit = 0;
					WeirdoDirection = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 1:
					return 0;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 0:
					return 1;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 0:
					return 2;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 3:
					return 3;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 2:
					return 4;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 2:
					return 5;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 3:
					return 6;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 1:
					return 7;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class PolishedGraniteStairs : Block // 0 typeof=Block
	{
		[Range(0, 1)] public byte UpsideDownBit { get; set; }
		[Range(0, 3)] public int WeirdoDirection { get; set; }

		public PolishedGraniteStairs() : base(427)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					UpsideDownBit = 1;
					WeirdoDirection = 3;
					break;
				case 1:
					UpsideDownBit = 0;
					WeirdoDirection = 0;
					break;
				case 2:
					UpsideDownBit = 1;
					WeirdoDirection = 1;
					break;
				case 3:
					UpsideDownBit = 1;
					WeirdoDirection = 0;
					break;
				case 4:
					UpsideDownBit = 0;
					WeirdoDirection = 2;
					break;
				case 5:
					UpsideDownBit = 0;
					WeirdoDirection = 1;
					break;
				case 6:
					UpsideDownBit = 1;
					WeirdoDirection = 2;
					break;
				case 7:
					UpsideDownBit = 0;
					WeirdoDirection = 3;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 3:
					return 0;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 0:
					return 1;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 1:
					return 2;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 0:
					return 3;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 2:
					return 4;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 1:
					return 5;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 2:
					return 6;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 3:
					return 7;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Portal // 90 typeof=Portal
	{
		// Convert this attribute to enum
		//[Enum("unknown","x","z"]
		public string PortalAxis { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					PortalAxis = "unknown";
					break;
				case 1:
					PortalAxis = "z";
					break;
				case 2:
					PortalAxis = "x";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.PortalAxis == "unknown":
					return 0;
				case { } b when true && b.PortalAxis == "z":
					return 1;
				case { } b when true && b.PortalAxis == "x":
					return 2;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Potatoes // 142 typeof=Potatoes
	{
		[Range(0, 7)] public int Growth { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Growth = 7;
					break;
				case 1:
					Growth = 3;
					break;
				case 2:
					Growth = 5;
					break;
				case 3:
					Growth = 2;
					break;
				case 4:
					Growth = 1;
					break;
				case 5:
					Growth = 4;
					break;
				case 6:
					Growth = 0;
					break;
				case 7:
					Growth = 6;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Growth == 7:
					return 0;
				case { } b when true && b.Growth == 3:
					return 1;
				case { } b when true && b.Growth == 5:
					return 2;
				case { } b when true && b.Growth == 2:
					return 3;
				case { } b when true && b.Growth == 1:
					return 4;
				case { } b when true && b.Growth == 4:
					return 5;
				case { } b when true && b.Growth == 0:
					return 6;
				case { } b when true && b.Growth == 6:
					return 7;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class PoweredComparator // 150 typeof=PoweredComparator
	{
		[Range(0, 3)] public int Direction { get; set; }
		[Range(0, 1)] public byte OutputLitBit { get; set; }
		[Range(0, 1)] public byte OutputSubtractBit { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Direction = 3;
					OutputLitBit = 0;
					OutputSubtractBit = 0;
					break;
				case 1:
					Direction = 0;
					OutputLitBit = 0;
					OutputSubtractBit = 0;
					break;
				case 2:
					Direction = 0;
					OutputLitBit = 1;
					OutputSubtractBit = 1;
					break;
				case 3:
					Direction = 3;
					OutputLitBit = 0;
					OutputSubtractBit = 1;
					break;
				case 4:
					Direction = 0;
					OutputLitBit = 0;
					OutputSubtractBit = 1;
					break;
				case 5:
					Direction = 2;
					OutputLitBit = 0;
					OutputSubtractBit = 1;
					break;
				case 6:
					Direction = 2;
					OutputLitBit = 0;
					OutputSubtractBit = 0;
					break;
				case 7:
					Direction = 1;
					OutputLitBit = 0;
					OutputSubtractBit = 0;
					break;
				case 8:
					Direction = 3;
					OutputLitBit = 1;
					OutputSubtractBit = 0;
					break;
				case 9:
					Direction = 0;
					OutputLitBit = 1;
					OutputSubtractBit = 0;
					break;
				case 10:
					Direction = 1;
					OutputLitBit = 0;
					OutputSubtractBit = 1;
					break;
				case 11:
					Direction = 2;
					OutputLitBit = 1;
					OutputSubtractBit = 0;
					break;
				case 12:
					Direction = 1;
					OutputLitBit = 1;
					OutputSubtractBit = 1;
					break;
				case 13:
					Direction = 2;
					OutputLitBit = 1;
					OutputSubtractBit = 1;
					break;
				case 14:
					Direction = 1;
					OutputLitBit = 1;
					OutputSubtractBit = 0;
					break;
				case 15:
					Direction = 3;
					OutputLitBit = 1;
					OutputSubtractBit = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Direction == 3 && b.OutputLitBit == 0 && b.OutputSubtractBit == 0:
					return 0;
				case { } b when true && b.Direction == 0 && b.OutputLitBit == 0 && b.OutputSubtractBit == 0:
					return 1;
				case { } b when true && b.Direction == 0 && b.OutputLitBit == 1 && b.OutputSubtractBit == 1:
					return 2;
				case { } b when true && b.Direction == 3 && b.OutputLitBit == 0 && b.OutputSubtractBit == 1:
					return 3;
				case { } b when true && b.Direction == 0 && b.OutputLitBit == 0 && b.OutputSubtractBit == 1:
					return 4;
				case { } b when true && b.Direction == 2 && b.OutputLitBit == 0 && b.OutputSubtractBit == 1:
					return 5;
				case { } b when true && b.Direction == 2 && b.OutputLitBit == 0 && b.OutputSubtractBit == 0:
					return 6;
				case { } b when true && b.Direction == 1 && b.OutputLitBit == 0 && b.OutputSubtractBit == 0:
					return 7;
				case { } b when true && b.Direction == 3 && b.OutputLitBit == 1 && b.OutputSubtractBit == 0:
					return 8;
				case { } b when true && b.Direction == 0 && b.OutputLitBit == 1 && b.OutputSubtractBit == 0:
					return 9;
				case { } b when true && b.Direction == 1 && b.OutputLitBit == 0 && b.OutputSubtractBit == 1:
					return 10;
				case { } b when true && b.Direction == 2 && b.OutputLitBit == 1 && b.OutputSubtractBit == 0:
					return 11;
				case { } b when true && b.Direction == 1 && b.OutputLitBit == 1 && b.OutputSubtractBit == 1:
					return 12;
				case { } b when true && b.Direction == 2 && b.OutputLitBit == 1 && b.OutputSubtractBit == 1:
					return 13;
				case { } b when true && b.Direction == 1 && b.OutputLitBit == 1 && b.OutputSubtractBit == 0:
					return 14;
				case { } b when true && b.Direction == 3 && b.OutputLitBit == 1 && b.OutputSubtractBit == 1:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class PoweredRepeater // 94 typeof=PoweredRepeater
	{
		[Range(0, 3)] public int Direction { get; set; }
		[Range(0, 3)] public int RepeaterDelay { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Direction = 0;
					RepeaterDelay = 0;
					break;
				case 1:
					Direction = 3;
					RepeaterDelay = 1;
					break;
				case 2:
					Direction = 0;
					RepeaterDelay = 2;
					break;
				case 3:
					Direction = 2;
					RepeaterDelay = 2;
					break;
				case 4:
					Direction = 3;
					RepeaterDelay = 2;
					break;
				case 5:
					Direction = 3;
					RepeaterDelay = 0;
					break;
				case 6:
					Direction = 1;
					RepeaterDelay = 2;
					break;
				case 7:
					Direction = 1;
					RepeaterDelay = 3;
					break;
				case 8:
					Direction = 0;
					RepeaterDelay = 3;
					break;
				case 9:
					Direction = 2;
					RepeaterDelay = 0;
					break;
				case 10:
					Direction = 3;
					RepeaterDelay = 3;
					break;
				case 11:
					Direction = 1;
					RepeaterDelay = 1;
					break;
				case 12:
					Direction = 2;
					RepeaterDelay = 1;
					break;
				case 13:
					Direction = 1;
					RepeaterDelay = 0;
					break;
				case 14:
					Direction = 2;
					RepeaterDelay = 3;
					break;
				case 15:
					Direction = 0;
					RepeaterDelay = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Direction == 0 && b.RepeaterDelay == 0:
					return 0;
				case { } b when true && b.Direction == 3 && b.RepeaterDelay == 1:
					return 1;
				case { } b when true && b.Direction == 0 && b.RepeaterDelay == 2:
					return 2;
				case { } b when true && b.Direction == 2 && b.RepeaterDelay == 2:
					return 3;
				case { } b when true && b.Direction == 3 && b.RepeaterDelay == 2:
					return 4;
				case { } b when true && b.Direction == 3 && b.RepeaterDelay == 0:
					return 5;
				case { } b when true && b.Direction == 1 && b.RepeaterDelay == 2:
					return 6;
				case { } b when true && b.Direction == 1 && b.RepeaterDelay == 3:
					return 7;
				case { } b when true && b.Direction == 0 && b.RepeaterDelay == 3:
					return 8;
				case { } b when true && b.Direction == 2 && b.RepeaterDelay == 0:
					return 9;
				case { } b when true && b.Direction == 3 && b.RepeaterDelay == 3:
					return 10;
				case { } b when true && b.Direction == 1 && b.RepeaterDelay == 1:
					return 11;
				case { } b when true && b.Direction == 2 && b.RepeaterDelay == 1:
					return 12;
				case { } b when true && b.Direction == 1 && b.RepeaterDelay == 0:
					return 13;
				case { } b when true && b.Direction == 2 && b.RepeaterDelay == 3:
					return 14;
				case { } b when true && b.Direction == 0 && b.RepeaterDelay == 1:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Prismarine // 168 typeof=Prismarine
	{
		// Convert this attribute to enum
		//[Enum("bricks","dark","default"]
		public string PrismarineBlockType { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					PrismarineBlockType = "dark";
					break;
				case 1:
					PrismarineBlockType = "bricks";
					break;
				case 2:
					PrismarineBlockType = "default";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.PrismarineBlockType == "dark":
					return 0;
				case { } b when true && b.PrismarineBlockType == "bricks":
					return 1;
				case { } b when true && b.PrismarineBlockType == "default":
					return 2;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class PrismarineBricksStairs : Block // 0 typeof=Block
	{
		[Range(0, 1)] public byte UpsideDownBit { get; set; }
		[Range(0, 3)] public int WeirdoDirection { get; set; }

		public PrismarineBricksStairs() : base(259)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					UpsideDownBit = 0;
					WeirdoDirection = 2;
					break;
				case 1:
					UpsideDownBit = 1;
					WeirdoDirection = 3;
					break;
				case 2:
					UpsideDownBit = 1;
					WeirdoDirection = 0;
					break;
				case 3:
					UpsideDownBit = 0;
					WeirdoDirection = 1;
					break;
				case 4:
					UpsideDownBit = 1;
					WeirdoDirection = 1;
					break;
				case 5:
					UpsideDownBit = 1;
					WeirdoDirection = 2;
					break;
				case 6:
					UpsideDownBit = 0;
					WeirdoDirection = 3;
					break;
				case 7:
					UpsideDownBit = 0;
					WeirdoDirection = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 2:
					return 0;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 3:
					return 1;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 0:
					return 2;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 1:
					return 3;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 1:
					return 4;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 2:
					return 5;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 3:
					return 6;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 0:
					return 7;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class PrismarineStairs : Block // 0 typeof=Block
	{
		[Range(0, 1)] public byte UpsideDownBit { get; set; }
		[Range(0, 3)] public int WeirdoDirection { get; set; }

		public PrismarineStairs() : base(257)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					UpsideDownBit = 0;
					WeirdoDirection = 0;
					break;
				case 1:
					UpsideDownBit = 1;
					WeirdoDirection = 0;
					break;
				case 2:
					UpsideDownBit = 1;
					WeirdoDirection = 1;
					break;
				case 3:
					UpsideDownBit = 0;
					WeirdoDirection = 2;
					break;
				case 4:
					UpsideDownBit = 1;
					WeirdoDirection = 2;
					break;
				case 5:
					UpsideDownBit = 1;
					WeirdoDirection = 3;
					break;
				case 6:
					UpsideDownBit = 0;
					WeirdoDirection = 1;
					break;
				case 7:
					UpsideDownBit = 0;
					WeirdoDirection = 3;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 0:
					return 0;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 0:
					return 1;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 1:
					return 2;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 2:
					return 3;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 2:
					return 4;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 3:
					return 5;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 1:
					return 6;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 3:
					return 7;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Pumpkin // 86 typeof=Pumpkin
	{
		[Range(0, 3)] public int Direction { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Direction = 0;
					break;
				case 1:
					Direction = 3;
					break;
				case 2:
					Direction = 1;
					break;
				case 3:
					Direction = 2;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Direction == 0:
					return 0;
				case { } b when true && b.Direction == 3:
					return 1;
				case { } b when true && b.Direction == 1:
					return 2;
				case { } b when true && b.Direction == 2:
					return 3;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class PumpkinStem // 104 typeof=PumpkinStem
	{
		[Range(0, 7)] public int Growth { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Growth = 3;
					break;
				case 1:
					Growth = 5;
					break;
				case 2:
					Growth = 2;
					break;
				case 3:
					Growth = 6;
					break;
				case 4:
					Growth = 7;
					break;
				case 5:
					Growth = 0;
					break;
				case 6:
					Growth = 4;
					break;
				case 7:
					Growth = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Growth == 3:
					return 0;
				case { } b when true && b.Growth == 5:
					return 1;
				case { } b when true && b.Growth == 2:
					return 2;
				case { } b when true && b.Growth == 6:
					return 3;
				case { } b when true && b.Growth == 7:
					return 4;
				case { } b when true && b.Growth == 0:
					return 5;
				case { } b when true && b.Growth == 4:
					return 6;
				case { } b when true && b.Growth == 1:
					return 7;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class PurpleGlazedTerracotta // 219 typeof=PurpleGlazedTerracotta
	{
		[Range(0, 5)] public int FacingDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					FacingDirection = 4;
					break;
				case 1:
					FacingDirection = 2;
					break;
				case 2:
					FacingDirection = 3;
					break;
				case 3:
					FacingDirection = 0;
					break;
				case 4:
					FacingDirection = 5;
					break;
				case 5:
					FacingDirection = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.FacingDirection == 4:
					return 0;
				case { } b when true && b.FacingDirection == 2:
					return 1;
				case { } b when true && b.FacingDirection == 3:
					return 2;
				case { } b when true && b.FacingDirection == 0:
					return 3;
				case { } b when true && b.FacingDirection == 5:
					return 4;
				case { } b when true && b.FacingDirection == 1:
					return 5;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class PurpurBlock // 201 typeof=PurpurBlock
	{
		// Convert this attribute to enum
		//[Enum("chiseled","default","lines","smooth"]
		public string ChiselType { get; set; }

		// Convert this attribute to enum
		//[Enum("x","y","z"]
		public string PillarAxis { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					ChiselType = "default";
					PillarAxis = "z";
					break;
				case 1:
					ChiselType = "smooth";
					PillarAxis = "y";
					break;
				case 2:
					ChiselType = "default";
					PillarAxis = "x";
					break;
				case 3:
					ChiselType = "lines";
					PillarAxis = "z";
					break;
				case 4:
					ChiselType = "chiseled";
					PillarAxis = "y";
					break;
				case 5:
					ChiselType = "default";
					PillarAxis = "y";
					break;
				case 6:
					ChiselType = "lines";
					PillarAxis = "x";
					break;
				case 7:
					ChiselType = "smooth";
					PillarAxis = "x";
					break;
				case 8:
					ChiselType = "chiseled";
					PillarAxis = "z";
					break;
				case 9:
					ChiselType = "lines";
					PillarAxis = "y";
					break;
				case 10:
					ChiselType = "smooth";
					PillarAxis = "z";
					break;
				case 11:
					ChiselType = "chiseled";
					PillarAxis = "x";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.ChiselType == "default" && b.PillarAxis == "z":
					return 0;
				case { } b when true && b.ChiselType == "smooth" && b.PillarAxis == "y":
					return 1;
				case { } b when true && b.ChiselType == "default" && b.PillarAxis == "x":
					return 2;
				case { } b when true && b.ChiselType == "lines" && b.PillarAxis == "z":
					return 3;
				case { } b when true && b.ChiselType == "chiseled" && b.PillarAxis == "y":
					return 4;
				case { } b when true && b.ChiselType == "default" && b.PillarAxis == "y":
					return 5;
				case { } b when true && b.ChiselType == "lines" && b.PillarAxis == "x":
					return 6;
				case { } b when true && b.ChiselType == "smooth" && b.PillarAxis == "x":
					return 7;
				case { } b when true && b.ChiselType == "chiseled" && b.PillarAxis == "z":
					return 8;
				case { } b when true && b.ChiselType == "lines" && b.PillarAxis == "y":
					return 9;
				case { } b when true && b.ChiselType == "smooth" && b.PillarAxis == "z":
					return 10;
				case { } b when true && b.ChiselType == "chiseled" && b.PillarAxis == "x":
					return 11;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class PurpurStairs // 203 typeof=PurpurStairs
	{
		[Range(0, 1)] public byte UpsideDownBit { get; set; }
		[Range(0, 3)] public int WeirdoDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					UpsideDownBit = 0;
					WeirdoDirection = 1;
					break;
				case 1:
					UpsideDownBit = 1;
					WeirdoDirection = 3;
					break;
				case 2:
					UpsideDownBit = 1;
					WeirdoDirection = 2;
					break;
				case 3:
					UpsideDownBit = 0;
					WeirdoDirection = 2;
					break;
				case 4:
					UpsideDownBit = 0;
					WeirdoDirection = 0;
					break;
				case 5:
					UpsideDownBit = 1;
					WeirdoDirection = 0;
					break;
				case 6:
					UpsideDownBit = 1;
					WeirdoDirection = 1;
					break;
				case 7:
					UpsideDownBit = 0;
					WeirdoDirection = 3;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 1:
					return 0;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 3:
					return 1;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 2:
					return 2;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 2:
					return 3;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 0:
					return 4;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 0:
					return 5;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 1:
					return 6;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 3:
					return 7;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class QuartzBlock // 155 typeof=QuartzBlock
	{
		// Convert this attribute to enum
		//[Enum("chiseled","default","lines","smooth"]
		public string ChiselType { get; set; }

		// Convert this attribute to enum
		//[Enum("x","y","z"]
		public string PillarAxis { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					ChiselType = "chiseled";
					PillarAxis = "z";
					break;
				case 1:
					ChiselType = "default";
					PillarAxis = "x";
					break;
				case 2:
					ChiselType = "chiseled";
					PillarAxis = "x";
					break;
				case 3:
					ChiselType = "default";
					PillarAxis = "y";
					break;
				case 4:
					ChiselType = "lines";
					PillarAxis = "x";
					break;
				case 5:
					ChiselType = "default";
					PillarAxis = "z";
					break;
				case 6:
					ChiselType = "lines";
					PillarAxis = "z";
					break;
				case 7:
					ChiselType = "lines";
					PillarAxis = "y";
					break;
				case 8:
					ChiselType = "smooth";
					PillarAxis = "z";
					break;
				case 9:
					ChiselType = "smooth";
					PillarAxis = "y";
					break;
				case 10:
					ChiselType = "smooth";
					PillarAxis = "x";
					break;
				case 11:
					ChiselType = "chiseled";
					PillarAxis = "y";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.ChiselType == "chiseled" && b.PillarAxis == "z":
					return 0;
				case { } b when true && b.ChiselType == "default" && b.PillarAxis == "x":
					return 1;
				case { } b when true && b.ChiselType == "chiseled" && b.PillarAxis == "x":
					return 2;
				case { } b when true && b.ChiselType == "default" && b.PillarAxis == "y":
					return 3;
				case { } b when true && b.ChiselType == "lines" && b.PillarAxis == "x":
					return 4;
				case { } b when true && b.ChiselType == "default" && b.PillarAxis == "z":
					return 5;
				case { } b when true && b.ChiselType == "lines" && b.PillarAxis == "z":
					return 6;
				case { } b when true && b.ChiselType == "lines" && b.PillarAxis == "y":
					return 7;
				case { } b when true && b.ChiselType == "smooth" && b.PillarAxis == "z":
					return 8;
				case { } b when true && b.ChiselType == "smooth" && b.PillarAxis == "y":
					return 9;
				case { } b when true && b.ChiselType == "smooth" && b.PillarAxis == "x":
					return 10;
				case { } b when true && b.ChiselType == "chiseled" && b.PillarAxis == "y":
					return 11;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class QuartzOre // 153 typeof=QuartzOre
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class QuartzStairs // 156 typeof=QuartzStairs
	{
		[Range(0, 1)] public byte UpsideDownBit { get; set; }
		[Range(0, 3)] public int WeirdoDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					UpsideDownBit = 1;
					WeirdoDirection = 0;
					break;
				case 1:
					UpsideDownBit = 0;
					WeirdoDirection = 3;
					break;
				case 2:
					UpsideDownBit = 1;
					WeirdoDirection = 2;
					break;
				case 3:
					UpsideDownBit = 1;
					WeirdoDirection = 1;
					break;
				case 4:
					UpsideDownBit = 1;
					WeirdoDirection = 3;
					break;
				case 5:
					UpsideDownBit = 0;
					WeirdoDirection = 2;
					break;
				case 6:
					UpsideDownBit = 0;
					WeirdoDirection = 0;
					break;
				case 7:
					UpsideDownBit = 0;
					WeirdoDirection = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 0:
					return 0;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 3:
					return 1;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 2:
					return 2;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 1:
					return 3;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 3:
					return 4;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 2:
					return 5;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 0:
					return 6;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 1:
					return 7;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Rail // 66 typeof=Rail
	{
		[Range(0, 9)] public int RailDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					RailDirection = 7;
					break;
				case 1:
					RailDirection = 6;
					break;
				case 2:
					RailDirection = 5;
					break;
				case 3:
					RailDirection = 9;
					break;
				case 4:
					RailDirection = 1;
					break;
				case 5:
					RailDirection = 0;
					break;
				case 6:
					RailDirection = 2;
					break;
				case 7:
					RailDirection = 3;
					break;
				case 8:
					RailDirection = 8;
					break;
				case 9:
					RailDirection = 4;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.RailDirection == 7:
					return 0;
				case { } b when true && b.RailDirection == 6:
					return 1;
				case { } b when true && b.RailDirection == 5:
					return 2;
				case { } b when true && b.RailDirection == 9:
					return 3;
				case { } b when true && b.RailDirection == 1:
					return 4;
				case { } b when true && b.RailDirection == 0:
					return 5;
				case { } b when true && b.RailDirection == 2:
					return 6;
				case { } b when true && b.RailDirection == 3:
					return 7;
				case { } b when true && b.RailDirection == 8:
					return 8;
				case { } b when true && b.RailDirection == 4:
					return 9;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class RedFlower // 38 typeof=RedFlower
	{
		// Convert this attribute to enum
		//[Enum("allium","cornflower","houstonia","lily_of_the_valley","orchid","oxeye","poppy","tulip_orange","tulip_pink","tulip_red","tulip_white"]
		public string FlowerType { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					FlowerType = "tulip_pink";
					break;
				case 1:
					FlowerType = "poppy";
					break;
				case 2:
					FlowerType = "allium";
					break;
				case 3:
					FlowerType = "orchid";
					break;
				case 4:
					FlowerType = "lily_of_the_valley";
					break;
				case 5:
					FlowerType = "houstonia";
					break;
				case 6:
					FlowerType = "tulip_white";
					break;
				case 7:
					FlowerType = "cornflower";
					break;
				case 8:
					FlowerType = "tulip_red";
					break;
				case 9:
					FlowerType = "tulip_orange";
					break;
				case 10:
					FlowerType = "oxeye";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.FlowerType == "tulip_pink":
					return 0;
				case { } b when true && b.FlowerType == "poppy":
					return 1;
				case { } b when true && b.FlowerType == "allium":
					return 2;
				case { } b when true && b.FlowerType == "orchid":
					return 3;
				case { } b when true && b.FlowerType == "lily_of_the_valley":
					return 4;
				case { } b when true && b.FlowerType == "houstonia":
					return 5;
				case { } b when true && b.FlowerType == "tulip_white":
					return 6;
				case { } b when true && b.FlowerType == "cornflower":
					return 7;
				case { } b when true && b.FlowerType == "tulip_red":
					return 8;
				case { } b when true && b.FlowerType == "tulip_orange":
					return 9;
				case { } b when true && b.FlowerType == "oxeye":
					return 10;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class RedGlazedTerracotta // 234 typeof=RedGlazedTerracotta
	{
		[Range(0, 5)] public int FacingDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					FacingDirection = 1;
					break;
				case 1:
					FacingDirection = 3;
					break;
				case 2:
					FacingDirection = 2;
					break;
				case 3:
					FacingDirection = 5;
					break;
				case 4:
					FacingDirection = 0;
					break;
				case 5:
					FacingDirection = 4;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.FacingDirection == 1:
					return 0;
				case { } b when true && b.FacingDirection == 3:
					return 1;
				case { } b when true && b.FacingDirection == 2:
					return 2;
				case { } b when true && b.FacingDirection == 5:
					return 3;
				case { } b when true && b.FacingDirection == 0:
					return 4;
				case { } b when true && b.FacingDirection == 4:
					return 5;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class RedMushroom // 40 typeof=RedMushroom
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class RedMushroomBlock // 100 typeof=RedMushroomBlock
	{
		[Range(0, 9)] public int HugeMushroomBits { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					HugeMushroomBits = 4;
					break;
				case 1:
					HugeMushroomBits = 7;
					break;
				case 2:
					HugeMushroomBits = 2;
					break;
				case 3:
					HugeMushroomBits = 15;
					break;
				case 4:
					HugeMushroomBits = 0;
					break;
				case 5:
					HugeMushroomBits = 6;
					break;
				case 6:
					HugeMushroomBits = 3;
					break;
				case 7:
					HugeMushroomBits = 9;
					break;
				case 8:
					HugeMushroomBits = 11;
					break;
				case 9:
					HugeMushroomBits = 13;
					break;
				case 10:
					HugeMushroomBits = 8;
					break;
				case 11:
					HugeMushroomBits = 5;
					break;
				case 12:
					HugeMushroomBits = 12;
					break;
				case 13:
					HugeMushroomBits = 10;
					break;
				case 14:
					HugeMushroomBits = 1;
					break;
				case 15:
					HugeMushroomBits = 14;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.HugeMushroomBits == 4:
					return 0;
				case { } b when true && b.HugeMushroomBits == 7:
					return 1;
				case { } b when true && b.HugeMushroomBits == 2:
					return 2;
				case { } b when true && b.HugeMushroomBits == 15:
					return 3;
				case { } b when true && b.HugeMushroomBits == 0:
					return 4;
				case { } b when true && b.HugeMushroomBits == 6:
					return 5;
				case { } b when true && b.HugeMushroomBits == 3:
					return 6;
				case { } b when true && b.HugeMushroomBits == 9:
					return 7;
				case { } b when true && b.HugeMushroomBits == 11:
					return 8;
				case { } b when true && b.HugeMushroomBits == 13:
					return 9;
				case { } b when true && b.HugeMushroomBits == 8:
					return 10;
				case { } b when true && b.HugeMushroomBits == 5:
					return 11;
				case { } b when true && b.HugeMushroomBits == 12:
					return 12;
				case { } b when true && b.HugeMushroomBits == 10:
					return 13;
				case { } b when true && b.HugeMushroomBits == 1:
					return 14;
				case { } b when true && b.HugeMushroomBits == 14:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class RedNetherBrick : Block // 0 typeof=Block
	{
		public RedNetherBrick() : base(215)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class RedNetherBrickStairs : Block // 0 typeof=Block
	{
		[Range(0, 1)] public byte UpsideDownBit { get; set; }
		[Range(0, 3)] public int WeirdoDirection { get; set; }

		public RedNetherBrickStairs() : base(439)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					UpsideDownBit = 1;
					WeirdoDirection = 3;
					break;
				case 1:
					UpsideDownBit = 0;
					WeirdoDirection = 1;
					break;
				case 2:
					UpsideDownBit = 1;
					WeirdoDirection = 0;
					break;
				case 3:
					UpsideDownBit = 0;
					WeirdoDirection = 3;
					break;
				case 4:
					UpsideDownBit = 0;
					WeirdoDirection = 0;
					break;
				case 5:
					UpsideDownBit = 1;
					WeirdoDirection = 2;
					break;
				case 6:
					UpsideDownBit = 0;
					WeirdoDirection = 2;
					break;
				case 7:
					UpsideDownBit = 1;
					WeirdoDirection = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 3:
					return 0;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 1:
					return 1;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 0:
					return 2;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 3:
					return 3;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 0:
					return 4;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 2:
					return 5;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 2:
					return 6;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 1:
					return 7;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class RedSandstone // 179 typeof=RedSandstone
	{
		// Convert this attribute to enum
		//[Enum("cut","default","heiroglyphs","smooth"]
		public string SandStoneType { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					SandStoneType = "cut";
					break;
				case 1:
					SandStoneType = "heiroglyphs";
					break;
				case 2:
					SandStoneType = "smooth";
					break;
				case 3:
					SandStoneType = "default";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.SandStoneType == "cut":
					return 0;
				case { } b when true && b.SandStoneType == "heiroglyphs":
					return 1;
				case { } b when true && b.SandStoneType == "smooth":
					return 2;
				case { } b when true && b.SandStoneType == "default":
					return 3;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class RedSandstoneStairs // 180 typeof=RedSandstoneStairs
	{
		[Range(0, 1)] public byte UpsideDownBit { get; set; }
		[Range(0, 3)] public int WeirdoDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					UpsideDownBit = 1;
					WeirdoDirection = 1;
					break;
				case 1:
					UpsideDownBit = 1;
					WeirdoDirection = 3;
					break;
				case 2:
					UpsideDownBit = 1;
					WeirdoDirection = 2;
					break;
				case 3:
					UpsideDownBit = 0;
					WeirdoDirection = 3;
					break;
				case 4:
					UpsideDownBit = 0;
					WeirdoDirection = 2;
					break;
				case 5:
					UpsideDownBit = 0;
					WeirdoDirection = 1;
					break;
				case 6:
					UpsideDownBit = 0;
					WeirdoDirection = 0;
					break;
				case 7:
					UpsideDownBit = 1;
					WeirdoDirection = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 1:
					return 0;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 3:
					return 1;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 2:
					return 2;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 3:
					return 3;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 2:
					return 4;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 1:
					return 5;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 0:
					return 6;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 0:
					return 7;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class RedstoneBlock // 152 typeof=RedstoneBlock
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class RedstoneLamp // 123 typeof=RedstoneLamp
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class RedstoneOre // 73 typeof=RedstoneOre
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class RedstoneTorch // 76 typeof=RedstoneTorch
	{
		// Convert this attribute to enum
		//[Enum("east","north","south","top","unknown","west"]
		public string TorchFacingDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					TorchFacingDirection = "north";
					break;
				case 1:
					TorchFacingDirection = "west";
					break;
				case 2:
					TorchFacingDirection = "south";
					break;
				case 3:
					TorchFacingDirection = "top";
					break;
				case 4:
					TorchFacingDirection = "unknown";
					break;
				case 5:
					TorchFacingDirection = "east";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.TorchFacingDirection == "north":
					return 0;
				case { } b when true && b.TorchFacingDirection == "west":
					return 1;
				case { } b when true && b.TorchFacingDirection == "south":
					return 2;
				case { } b when true && b.TorchFacingDirection == "top":
					return 3;
				case { } b when true && b.TorchFacingDirection == "unknown":
					return 4;
				case { } b when true && b.TorchFacingDirection == "east":
					return 5;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class RedstoneWire // 55 typeof=RedstoneWire
	{
		[Range(0, 9)] public int RedstoneSignal { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					RedstoneSignal = 15;
					break;
				case 1:
					RedstoneSignal = 6;
					break;
				case 2:
					RedstoneSignal = 2;
					break;
				case 3:
					RedstoneSignal = 1;
					break;
				case 4:
					RedstoneSignal = 10;
					break;
				case 5:
					RedstoneSignal = 0;
					break;
				case 6:
					RedstoneSignal = 3;
					break;
				case 7:
					RedstoneSignal = 5;
					break;
				case 8:
					RedstoneSignal = 14;
					break;
				case 9:
					RedstoneSignal = 12;
					break;
				case 10:
					RedstoneSignal = 4;
					break;
				case 11:
					RedstoneSignal = 8;
					break;
				case 12:
					RedstoneSignal = 11;
					break;
				case 13:
					RedstoneSignal = 7;
					break;
				case 14:
					RedstoneSignal = 9;
					break;
				case 15:
					RedstoneSignal = 13;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.RedstoneSignal == 15:
					return 0;
				case { } b when true && b.RedstoneSignal == 6:
					return 1;
				case { } b when true && b.RedstoneSignal == 2:
					return 2;
				case { } b when true && b.RedstoneSignal == 1:
					return 3;
				case { } b when true && b.RedstoneSignal == 10:
					return 4;
				case { } b when true && b.RedstoneSignal == 0:
					return 5;
				case { } b when true && b.RedstoneSignal == 3:
					return 6;
				case { } b when true && b.RedstoneSignal == 5:
					return 7;
				case { } b when true && b.RedstoneSignal == 14:
					return 8;
				case { } b when true && b.RedstoneSignal == 12:
					return 9;
				case { } b when true && b.RedstoneSignal == 4:
					return 10;
				case { } b when true && b.RedstoneSignal == 8:
					return 11;
				case { } b when true && b.RedstoneSignal == 11:
					return 12;
				case { } b when true && b.RedstoneSignal == 7:
					return 13;
				case { } b when true && b.RedstoneSignal == 9:
					return 14;
				case { } b when true && b.RedstoneSignal == 13:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Reeds // 83 typeof=Reeds
	{
		[Range(0, 9)] public int Age { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Age = 11;
					break;
				case 1:
					Age = 5;
					break;
				case 2:
					Age = 13;
					break;
				case 3:
					Age = 6;
					break;
				case 4:
					Age = 8;
					break;
				case 5:
					Age = 1;
					break;
				case 6:
					Age = 7;
					break;
				case 7:
					Age = 12;
					break;
				case 8:
					Age = 2;
					break;
				case 9:
					Age = 3;
					break;
				case 10:
					Age = 10;
					break;
				case 11:
					Age = 0;
					break;
				case 12:
					Age = 9;
					break;
				case 13:
					Age = 15;
					break;
				case 14:
					Age = 4;
					break;
				case 15:
					Age = 14;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Age == 11:
					return 0;
				case { } b when true && b.Age == 5:
					return 1;
				case { } b when true && b.Age == 13:
					return 2;
				case { } b when true && b.Age == 6:
					return 3;
				case { } b when true && b.Age == 8:
					return 4;
				case { } b when true && b.Age == 1:
					return 5;
				case { } b when true && b.Age == 7:
					return 6;
				case { } b when true && b.Age == 12:
					return 7;
				case { } b when true && b.Age == 2:
					return 8;
				case { } b when true && b.Age == 3:
					return 9;
				case { } b when true && b.Age == 10:
					return 10;
				case { } b when true && b.Age == 0:
					return 11;
				case { } b when true && b.Age == 9:
					return 12;
				case { } b when true && b.Age == 15:
					return 13;
				case { } b when true && b.Age == 4:
					return 14;
				case { } b when true && b.Age == 14:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class RepeatingCommandBlock : Block // 0 typeof=Block
	{
		[Range(0, 1)] public byte ConditionalBit { get; set; }
		[Range(0, 5)] public int FacingDirection { get; set; }

		public RepeatingCommandBlock() : base(188)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					ConditionalBit = 0;
					FacingDirection = 4;
					break;
				case 1:
					ConditionalBit = 0;
					FacingDirection = 2;
					break;
				case 2:
					ConditionalBit = 1;
					FacingDirection = 2;
					break;
				case 3:
					ConditionalBit = 0;
					FacingDirection = 1;
					break;
				case 4:
					ConditionalBit = 1;
					FacingDirection = 1;
					break;
				case 5:
					ConditionalBit = 1;
					FacingDirection = 5;
					break;
				case 6:
					ConditionalBit = 1;
					FacingDirection = 3;
					break;
				case 7:
					ConditionalBit = 1;
					FacingDirection = 0;
					break;
				case 8:
					ConditionalBit = 1;
					FacingDirection = 4;
					break;
				case 9:
					ConditionalBit = 0;
					FacingDirection = 3;
					break;
				case 10:
					ConditionalBit = 0;
					FacingDirection = 0;
					break;
				case 11:
					ConditionalBit = 0;
					FacingDirection = 5;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.ConditionalBit == 0 && b.FacingDirection == 4:
					return 0;
				case { } b when true && b.ConditionalBit == 0 && b.FacingDirection == 2:
					return 1;
				case { } b when true && b.ConditionalBit == 1 && b.FacingDirection == 2:
					return 2;
				case { } b when true && b.ConditionalBit == 0 && b.FacingDirection == 1:
					return 3;
				case { } b when true && b.ConditionalBit == 1 && b.FacingDirection == 1:
					return 4;
				case { } b when true && b.ConditionalBit == 1 && b.FacingDirection == 5:
					return 5;
				case { } b when true && b.ConditionalBit == 1 && b.FacingDirection == 3:
					return 6;
				case { } b when true && b.ConditionalBit == 1 && b.FacingDirection == 0:
					return 7;
				case { } b when true && b.ConditionalBit == 1 && b.FacingDirection == 4:
					return 8;
				case { } b when true && b.ConditionalBit == 0 && b.FacingDirection == 3:
					return 9;
				case { } b when true && b.ConditionalBit == 0 && b.FacingDirection == 0:
					return 10;
				case { } b when true && b.ConditionalBit == 0 && b.FacingDirection == 5:
					return 11;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Reserved6 : Block // 0 typeof=Block
	{
		public Reserved6() : base(255)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Sand // 12 typeof=Sand
	{
		// Convert this attribute to enum
		//[Enum("normal","red"]
		public string SandType { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					SandType = "normal";
					break;
				case 1:
					SandType = "red";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.SandType == "normal":
					return 0;
				case { } b when true && b.SandType == "red":
					return 1;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Sandstone // 24 typeof=Sandstone
	{
		// Convert this attribute to enum
		//[Enum("cut","default","heiroglyphs","smooth"]
		public string SandStoneType { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					SandStoneType = "heiroglyphs";
					break;
				case 1:
					SandStoneType = "smooth";
					break;
				case 2:
					SandStoneType = "default";
					break;
				case 3:
					SandStoneType = "cut";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.SandStoneType == "heiroglyphs":
					return 0;
				case { } b when true && b.SandStoneType == "smooth":
					return 1;
				case { } b when true && b.SandStoneType == "default":
					return 2;
				case { } b when true && b.SandStoneType == "cut":
					return 3;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class SandstoneStairs // 128 typeof=SandStoneStairs
	{
		[Range(0, 1)] public byte UpsideDownBit { get; set; }
		[Range(0, 3)] public int WeirdoDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					UpsideDownBit = 1;
					WeirdoDirection = 2;
					break;
				case 1:
					UpsideDownBit = 0;
					WeirdoDirection = 0;
					break;
				case 2:
					UpsideDownBit = 1;
					WeirdoDirection = 1;
					break;
				case 3:
					UpsideDownBit = 0;
					WeirdoDirection = 1;
					break;
				case 4:
					UpsideDownBit = 0;
					WeirdoDirection = 2;
					break;
				case 5:
					UpsideDownBit = 0;
					WeirdoDirection = 3;
					break;
				case 6:
					UpsideDownBit = 1;
					WeirdoDirection = 3;
					break;
				case 7:
					UpsideDownBit = 1;
					WeirdoDirection = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 2:
					return 0;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 0:
					return 1;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 1:
					return 2;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 1:
					return 3;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 2:
					return 4;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 3:
					return 5;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 3:
					return 6;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 0:
					return 7;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Sapling // 6 typeof=Sapling
	{
		[Range(0, 1)] public byte AgeBit { get; set; }

		// Convert this attribute to enum
		//[Enum("acacia","birch","dark_oak","jungle","oak","spruce"]
		public string SaplingType { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					AgeBit = 0;
					SaplingType = "jungle";
					break;
				case 1:
					AgeBit = 1;
					SaplingType = "spruce";
					break;
				case 2:
					AgeBit = 0;
					SaplingType = "birch";
					break;
				case 3:
					AgeBit = 0;
					SaplingType = "spruce";
					break;
				case 4:
					AgeBit = 1;
					SaplingType = "dark_oak";
					break;
				case 5:
					AgeBit = 1;
					SaplingType = "birch";
					break;
				case 6:
					AgeBit = 1;
					SaplingType = "oak";
					break;
				case 7:
					AgeBit = 0;
					SaplingType = "oak";
					break;
				case 8:
					AgeBit = 1;
					SaplingType = "jungle";
					break;
				case 9:
					AgeBit = 1;
					SaplingType = "acacia";
					break;
				case 10:
					AgeBit = 0;
					SaplingType = "acacia";
					break;
				case 11:
					AgeBit = 0;
					SaplingType = "dark_oak";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.AgeBit == 0 && b.SaplingType == "jungle":
					return 0;
				case { } b when true && b.AgeBit == 1 && b.SaplingType == "spruce":
					return 1;
				case { } b when true && b.AgeBit == 0 && b.SaplingType == "birch":
					return 2;
				case { } b when true && b.AgeBit == 0 && b.SaplingType == "spruce":
					return 3;
				case { } b when true && b.AgeBit == 1 && b.SaplingType == "dark_oak":
					return 4;
				case { } b when true && b.AgeBit == 1 && b.SaplingType == "birch":
					return 5;
				case { } b when true && b.AgeBit == 1 && b.SaplingType == "oak":
					return 6;
				case { } b when true && b.AgeBit == 0 && b.SaplingType == "oak":
					return 7;
				case { } b when true && b.AgeBit == 1 && b.SaplingType == "jungle":
					return 8;
				case { } b when true && b.AgeBit == 1 && b.SaplingType == "acacia":
					return 9;
				case { } b when true && b.AgeBit == 0 && b.SaplingType == "acacia":
					return 10;
				case { } b when true && b.AgeBit == 0 && b.SaplingType == "dark_oak":
					return 11;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Scaffolding : Block // 0 typeof=Block
	{
		[Range(0, 7)] public int Stability { get; set; }
		[Range(0, 1)] public byte StabilityCheck { get; set; }

		public Scaffolding() : base(420)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Stability = 1;
					StabilityCheck = 0;
					break;
				case 1:
					Stability = 0;
					StabilityCheck = 0;
					break;
				case 2:
					Stability = 1;
					StabilityCheck = 1;
					break;
				case 3:
					Stability = 7;
					StabilityCheck = 1;
					break;
				case 4:
					Stability = 3;
					StabilityCheck = 1;
					break;
				case 5:
					Stability = 2;
					StabilityCheck = 0;
					break;
				case 6:
					Stability = 6;
					StabilityCheck = 1;
					break;
				case 7:
					Stability = 3;
					StabilityCheck = 0;
					break;
				case 8:
					Stability = 7;
					StabilityCheck = 0;
					break;
				case 9:
					Stability = 2;
					StabilityCheck = 1;
					break;
				case 10:
					Stability = 4;
					StabilityCheck = 1;
					break;
				case 11:
					Stability = 5;
					StabilityCheck = 0;
					break;
				case 12:
					Stability = 0;
					StabilityCheck = 1;
					break;
				case 13:
					Stability = 4;
					StabilityCheck = 0;
					break;
				case 14:
					Stability = 6;
					StabilityCheck = 0;
					break;
				case 15:
					Stability = 5;
					StabilityCheck = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Stability == 1 && b.StabilityCheck == 0:
					return 0;
				case { } b when true && b.Stability == 0 && b.StabilityCheck == 0:
					return 1;
				case { } b when true && b.Stability == 1 && b.StabilityCheck == 1:
					return 2;
				case { } b when true && b.Stability == 7 && b.StabilityCheck == 1:
					return 3;
				case { } b when true && b.Stability == 3 && b.StabilityCheck == 1:
					return 4;
				case { } b when true && b.Stability == 2 && b.StabilityCheck == 0:
					return 5;
				case { } b when true && b.Stability == 6 && b.StabilityCheck == 1:
					return 6;
				case { } b when true && b.Stability == 3 && b.StabilityCheck == 0:
					return 7;
				case { } b when true && b.Stability == 7 && b.StabilityCheck == 0:
					return 8;
				case { } b when true && b.Stability == 2 && b.StabilityCheck == 1:
					return 9;
				case { } b when true && b.Stability == 4 && b.StabilityCheck == 1:
					return 10;
				case { } b when true && b.Stability == 5 && b.StabilityCheck == 0:
					return 11;
				case { } b when true && b.Stability == 0 && b.StabilityCheck == 1:
					return 12;
				case { } b when true && b.Stability == 4 && b.StabilityCheck == 0:
					return 13;
				case { } b when true && b.Stability == 6 && b.StabilityCheck == 0:
					return 14;
				case { } b when true && b.Stability == 5 && b.StabilityCheck == 1:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class SeaPickle : Block // 0 typeof=Block
	{
		[Range(0, 3)] public int ClusterCount { get; set; }
		[Range(0, 1)] public byte DeadBit { get; set; }

		public SeaPickle() : base(411)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					ClusterCount = 3;
					DeadBit = 0;
					break;
				case 1:
					ClusterCount = 2;
					DeadBit = 1;
					break;
				case 2:
					ClusterCount = 2;
					DeadBit = 0;
					break;
				case 3:
					ClusterCount = 1;
					DeadBit = 0;
					break;
				case 4:
					ClusterCount = 1;
					DeadBit = 1;
					break;
				case 5:
					ClusterCount = 0;
					DeadBit = 0;
					break;
				case 6:
					ClusterCount = 0;
					DeadBit = 1;
					break;
				case 7:
					ClusterCount = 3;
					DeadBit = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.ClusterCount == 3 && b.DeadBit == 0:
					return 0;
				case { } b when true && b.ClusterCount == 2 && b.DeadBit == 1:
					return 1;
				case { } b when true && b.ClusterCount == 2 && b.DeadBit == 0:
					return 2;
				case { } b when true && b.ClusterCount == 1 && b.DeadBit == 0:
					return 3;
				case { } b when true && b.ClusterCount == 1 && b.DeadBit == 1:
					return 4;
				case { } b when true && b.ClusterCount == 0 && b.DeadBit == 0:
					return 5;
				case { } b when true && b.ClusterCount == 0 && b.DeadBit == 1:
					return 6;
				case { } b when true && b.ClusterCount == 3 && b.DeadBit == 1:
					return 7;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Seagrass : Block // 0 typeof=Block
	{
		// Convert this attribute to enum
		//[Enum("default","double_bot","double_top"]
		public string SeaGrassType { get; set; }

		public Seagrass() : base(385)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					SeaGrassType = "double_bot";
					break;
				case 1:
					SeaGrassType = "default";
					break;
				case 2:
					SeaGrassType = "double_top";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.SeaGrassType == "double_bot":
					return 0;
				case { } b when true && b.SeaGrassType == "default":
					return 1;
				case { } b when true && b.SeaGrassType == "double_top":
					return 2;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class SeaLantern // 169 typeof=SeaLantern
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class ShulkerBox // 218 typeof=ShulkerBox
	{
		// Convert this attribute to enum
		//[Enum("black","blue","brown","cyan","gray","green","light_blue","lime","magenta","orange","pink","purple","red","silver","white","yellow"]
		public string Color { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Color = "pink";
					break;
				case 1:
					Color = "orange";
					break;
				case 2:
					Color = "cyan";
					break;
				case 3:
					Color = "white";
					break;
				case 4:
					Color = "yellow";
					break;
				case 5:
					Color = "gray";
					break;
				case 6:
					Color = "brown";
					break;
				case 7:
					Color = "red";
					break;
				case 8:
					Color = "blue";
					break;
				case 9:
					Color = "lime";
					break;
				case 10:
					Color = "magenta";
					break;
				case 11:
					Color = "purple";
					break;
				case 12:
					Color = "black";
					break;
				case 13:
					Color = "light_blue";
					break;
				case 14:
					Color = "silver";
					break;
				case 15:
					Color = "green";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Color == "pink":
					return 0;
				case { } b when true && b.Color == "orange":
					return 1;
				case { } b when true && b.Color == "cyan":
					return 2;
				case { } b when true && b.Color == "white":
					return 3;
				case { } b when true && b.Color == "yellow":
					return 4;
				case { } b when true && b.Color == "gray":
					return 5;
				case { } b when true && b.Color == "brown":
					return 6;
				case { } b when true && b.Color == "red":
					return 7;
				case { } b when true && b.Color == "blue":
					return 8;
				case { } b when true && b.Color == "lime":
					return 9;
				case { } b when true && b.Color == "magenta":
					return 10;
				case { } b when true && b.Color == "purple":
					return 11;
				case { } b when true && b.Color == "black":
					return 12;
				case { } b when true && b.Color == "light_blue":
					return 13;
				case { } b when true && b.Color == "silver":
					return 14;
				case { } b when true && b.Color == "green":
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class SilverGlazedTerracotta // 228 typeof=SilverGlazedTerracotta
	{
		[Range(0, 5)] public int FacingDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					FacingDirection = 3;
					break;
				case 1:
					FacingDirection = 5;
					break;
				case 2:
					FacingDirection = 4;
					break;
				case 3:
					FacingDirection = 2;
					break;
				case 4:
					FacingDirection = 1;
					break;
				case 5:
					FacingDirection = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.FacingDirection == 3:
					return 0;
				case { } b when true && b.FacingDirection == 5:
					return 1;
				case { } b when true && b.FacingDirection == 4:
					return 2;
				case { } b when true && b.FacingDirection == 2:
					return 3;
				case { } b when true && b.FacingDirection == 1:
					return 4;
				case { } b when true && b.FacingDirection == 0:
					return 5;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Skull // 144 typeof=Skull
	{
		[Range(0, 5)] public int FacingDirection { get; set; }
		[Range(0, 1)] public byte NoDropBit { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					FacingDirection = 4;
					NoDropBit = 1;
					break;
				case 1:
					FacingDirection = 2;
					NoDropBit = 1;
					break;
				case 2:
					FacingDirection = 0;
					NoDropBit = 0;
					break;
				case 3:
					FacingDirection = 5;
					NoDropBit = 0;
					break;
				case 4:
					FacingDirection = 3;
					NoDropBit = 1;
					break;
				case 5:
					FacingDirection = 1;
					NoDropBit = 0;
					break;
				case 6:
					FacingDirection = 3;
					NoDropBit = 0;
					break;
				case 7:
					FacingDirection = 4;
					NoDropBit = 0;
					break;
				case 8:
					FacingDirection = 2;
					NoDropBit = 0;
					break;
				case 9:
					FacingDirection = 0;
					NoDropBit = 1;
					break;
				case 10:
					FacingDirection = 1;
					NoDropBit = 1;
					break;
				case 11:
					FacingDirection = 5;
					NoDropBit = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.FacingDirection == 4 && b.NoDropBit == 1:
					return 0;
				case { } b when true && b.FacingDirection == 2 && b.NoDropBit == 1:
					return 1;
				case { } b when true && b.FacingDirection == 0 && b.NoDropBit == 0:
					return 2;
				case { } b when true && b.FacingDirection == 5 && b.NoDropBit == 0:
					return 3;
				case { } b when true && b.FacingDirection == 3 && b.NoDropBit == 1:
					return 4;
				case { } b when true && b.FacingDirection == 1 && b.NoDropBit == 0:
					return 5;
				case { } b when true && b.FacingDirection == 3 && b.NoDropBit == 0:
					return 6;
				case { } b when true && b.FacingDirection == 4 && b.NoDropBit == 0:
					return 7;
				case { } b when true && b.FacingDirection == 2 && b.NoDropBit == 0:
					return 8;
				case { } b when true && b.FacingDirection == 0 && b.NoDropBit == 1:
					return 9;
				case { } b when true && b.FacingDirection == 1 && b.NoDropBit == 1:
					return 10;
				case { } b when true && b.FacingDirection == 5 && b.NoDropBit == 1:
					return 11;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Slime // 165 typeof=Slime
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class SmithingTable : Block // 0 typeof=Block
	{
		public SmithingTable() : base(457)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Smoker : Block // 0 typeof=Block
	{
		[Range(0, 5)] public int FacingDirection { get; set; }

		public Smoker() : base(453)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					FacingDirection = 1;
					break;
				case 1:
					FacingDirection = 4;
					break;
				case 2:
					FacingDirection = 5;
					break;
				case 3:
					FacingDirection = 0;
					break;
				case 4:
					FacingDirection = 2;
					break;
				case 5:
					FacingDirection = 3;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.FacingDirection == 1:
					return 0;
				case { } b when true && b.FacingDirection == 4:
					return 1;
				case { } b when true && b.FacingDirection == 5:
					return 2;
				case { } b when true && b.FacingDirection == 0:
					return 3;
				case { } b when true && b.FacingDirection == 2:
					return 4;
				case { } b when true && b.FacingDirection == 3:
					return 5;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class SmoothQuartzStairs : Block // 0 typeof=Block
	{
		[Range(0, 1)] public byte UpsideDownBit { get; set; }
		[Range(0, 3)] public int WeirdoDirection { get; set; }

		public SmoothQuartzStairs() : base(440)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					UpsideDownBit = 0;
					WeirdoDirection = 3;
					break;
				case 1:
					UpsideDownBit = 0;
					WeirdoDirection = 2;
					break;
				case 2:
					UpsideDownBit = 0;
					WeirdoDirection = 1;
					break;
				case 3:
					UpsideDownBit = 0;
					WeirdoDirection = 0;
					break;
				case 4:
					UpsideDownBit = 1;
					WeirdoDirection = 2;
					break;
				case 5:
					UpsideDownBit = 1;
					WeirdoDirection = 3;
					break;
				case 6:
					UpsideDownBit = 1;
					WeirdoDirection = 1;
					break;
				case 7:
					UpsideDownBit = 1;
					WeirdoDirection = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 3:
					return 0;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 2:
					return 1;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 1:
					return 2;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 0:
					return 3;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 2:
					return 4;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 3:
					return 5;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 1:
					return 6;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 0:
					return 7;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class SmoothRedSandstoneStairs : Block // 0 typeof=Block
	{
		[Range(0, 1)] public byte UpsideDownBit { get; set; }
		[Range(0, 3)] public int WeirdoDirection { get; set; }

		public SmoothRedSandstoneStairs() : base(431)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					UpsideDownBit = 1;
					WeirdoDirection = 1;
					break;
				case 1:
					UpsideDownBit = 0;
					WeirdoDirection = 3;
					break;
				case 2:
					UpsideDownBit = 1;
					WeirdoDirection = 3;
					break;
				case 3:
					UpsideDownBit = 0;
					WeirdoDirection = 2;
					break;
				case 4:
					UpsideDownBit = 0;
					WeirdoDirection = 0;
					break;
				case 5:
					UpsideDownBit = 0;
					WeirdoDirection = 1;
					break;
				case 6:
					UpsideDownBit = 1;
					WeirdoDirection = 2;
					break;
				case 7:
					UpsideDownBit = 1;
					WeirdoDirection = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 1:
					return 0;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 3:
					return 1;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 3:
					return 2;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 2:
					return 3;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 0:
					return 4;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 1:
					return 5;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 2:
					return 6;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 0:
					return 7;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class SmoothSandstoneStairs : Block // 0 typeof=Block
	{
		[Range(0, 1)] public byte UpsideDownBit { get; set; }
		[Range(0, 3)] public int WeirdoDirection { get; set; }

		public SmoothSandstoneStairs() : base(432)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					UpsideDownBit = 1;
					WeirdoDirection = 3;
					break;
				case 1:
					UpsideDownBit = 0;
					WeirdoDirection = 3;
					break;
				case 2:
					UpsideDownBit = 1;
					WeirdoDirection = 2;
					break;
				case 3:
					UpsideDownBit = 0;
					WeirdoDirection = 2;
					break;
				case 4:
					UpsideDownBit = 0;
					WeirdoDirection = 0;
					break;
				case 5:
					UpsideDownBit = 1;
					WeirdoDirection = 0;
					break;
				case 6:
					UpsideDownBit = 1;
					WeirdoDirection = 1;
					break;
				case 7:
					UpsideDownBit = 0;
					WeirdoDirection = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 3:
					return 0;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 3:
					return 1;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 2:
					return 2;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 2:
					return 3;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 0:
					return 4;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 0:
					return 5;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 1:
					return 6;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 1:
					return 7;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class SmoothStone : Block // 0 typeof=Block
	{
		public SmoothStone() : base(438)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Snow // 80 typeof=Snow
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class SnowLayer // 78 typeof=SnowLayer
	{
		[Range(0, 1)] public byte CoveredBit { get; set; }
		[Range(0, 7)] public int Height { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					CoveredBit = 1;
					Height = 2;
					break;
				case 1:
					CoveredBit = 1;
					Height = 5;
					break;
				case 2:
					CoveredBit = 1;
					Height = 3;
					break;
				case 3:
					CoveredBit = 0;
					Height = 4;
					break;
				case 4:
					CoveredBit = 1;
					Height = 4;
					break;
				case 5:
					CoveredBit = 0;
					Height = 7;
					break;
				case 6:
					CoveredBit = 0;
					Height = 5;
					break;
				case 7:
					CoveredBit = 0;
					Height = 0;
					break;
				case 8:
					CoveredBit = 1;
					Height = 6;
					break;
				case 9:
					CoveredBit = 1;
					Height = 0;
					break;
				case 10:
					CoveredBit = 0;
					Height = 1;
					break;
				case 11:
					CoveredBit = 1;
					Height = 7;
					break;
				case 12:
					CoveredBit = 0;
					Height = 6;
					break;
				case 13:
					CoveredBit = 0;
					Height = 3;
					break;
				case 14:
					CoveredBit = 0;
					Height = 2;
					break;
				case 15:
					CoveredBit = 1;
					Height = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.CoveredBit == 1 && b.Height == 2:
					return 0;
				case { } b when true && b.CoveredBit == 1 && b.Height == 5:
					return 1;
				case { } b when true && b.CoveredBit == 1 && b.Height == 3:
					return 2;
				case { } b when true && b.CoveredBit == 0 && b.Height == 4:
					return 3;
				case { } b when true && b.CoveredBit == 1 && b.Height == 4:
					return 4;
				case { } b when true && b.CoveredBit == 0 && b.Height == 7:
					return 5;
				case { } b when true && b.CoveredBit == 0 && b.Height == 5:
					return 6;
				case { } b when true && b.CoveredBit == 0 && b.Height == 0:
					return 7;
				case { } b when true && b.CoveredBit == 1 && b.Height == 6:
					return 8;
				case { } b when true && b.CoveredBit == 1 && b.Height == 0:
					return 9;
				case { } b when true && b.CoveredBit == 0 && b.Height == 1:
					return 10;
				case { } b when true && b.CoveredBit == 1 && b.Height == 7:
					return 11;
				case { } b when true && b.CoveredBit == 0 && b.Height == 6:
					return 12;
				case { } b when true && b.CoveredBit == 0 && b.Height == 3:
					return 13;
				case { } b when true && b.CoveredBit == 0 && b.Height == 2:
					return 14;
				case { } b when true && b.CoveredBit == 1 && b.Height == 1:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class SoulSand // 88 typeof=SoulSand
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Sponge // 19 typeof=Sponge
	{
		// Convert this attribute to enum
		//[Enum("dry","wet"]
		public string SpongeType { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					SpongeType = "dry";
					break;
				case 1:
					SpongeType = "wet";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.SpongeType == "dry":
					return 0;
				case { } b when true && b.SpongeType == "wet":
					return 1;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class SpruceButton : Block // 0 typeof=Block
	{
		[Range(0, 1)] public byte ButtonPressedBit { get; set; }
		[Range(0, 5)] public int FacingDirection { get; set; }

		public SpruceButton() : base(399)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					ButtonPressedBit = 0;
					FacingDirection = 0;
					break;
				case 1:
					ButtonPressedBit = 1;
					FacingDirection = 3;
					break;
				case 2:
					ButtonPressedBit = 0;
					FacingDirection = 1;
					break;
				case 3:
					ButtonPressedBit = 0;
					FacingDirection = 3;
					break;
				case 4:
					ButtonPressedBit = 1;
					FacingDirection = 4;
					break;
				case 5:
					ButtonPressedBit = 1;
					FacingDirection = 0;
					break;
				case 6:
					ButtonPressedBit = 0;
					FacingDirection = 5;
					break;
				case 7:
					ButtonPressedBit = 1;
					FacingDirection = 1;
					break;
				case 8:
					ButtonPressedBit = 0;
					FacingDirection = 4;
					break;
				case 9:
					ButtonPressedBit = 0;
					FacingDirection = 2;
					break;
				case 10:
					ButtonPressedBit = 1;
					FacingDirection = 2;
					break;
				case 11:
					ButtonPressedBit = 1;
					FacingDirection = 5;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.ButtonPressedBit == 0 && b.FacingDirection == 0:
					return 0;
				case { } b when true && b.ButtonPressedBit == 1 && b.FacingDirection == 3:
					return 1;
				case { } b when true && b.ButtonPressedBit == 0 && b.FacingDirection == 1:
					return 2;
				case { } b when true && b.ButtonPressedBit == 0 && b.FacingDirection == 3:
					return 3;
				case { } b when true && b.ButtonPressedBit == 1 && b.FacingDirection == 4:
					return 4;
				case { } b when true && b.ButtonPressedBit == 1 && b.FacingDirection == 0:
					return 5;
				case { } b when true && b.ButtonPressedBit == 0 && b.FacingDirection == 5:
					return 6;
				case { } b when true && b.ButtonPressedBit == 1 && b.FacingDirection == 1:
					return 7;
				case { } b when true && b.ButtonPressedBit == 0 && b.FacingDirection == 4:
					return 8;
				case { } b when true && b.ButtonPressedBit == 0 && b.FacingDirection == 2:
					return 9;
				case { } b when true && b.ButtonPressedBit == 1 && b.FacingDirection == 2:
					return 10;
				case { } b when true && b.ButtonPressedBit == 1 && b.FacingDirection == 5:
					return 11;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class SpruceDoor // 193 typeof=SpruceDoor
	{
		[Range(0, 3)] public int Direction { get; set; }
		[Range(0, 1)] public byte DoorHingeBit { get; set; }
		[Range(0, 1)] public byte OpenBit { get; set; }
		[Range(0, 1)] public byte UpperBlockBit { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Direction = 3;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 1:
					Direction = 3;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 2:
					Direction = 2;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 3:
					Direction = 1;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 4:
					Direction = 2;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 5:
					Direction = 3;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 6:
					Direction = 1;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 7:
					Direction = 1;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 8:
					Direction = 2;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 9:
					Direction = 1;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 10:
					Direction = 0;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 11:
					Direction = 0;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 12:
					Direction = 1;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 13:
					Direction = 3;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 14:
					Direction = 3;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 15:
					Direction = 0;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 16:
					Direction = 1;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 17:
					Direction = 2;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 18:
					Direction = 1;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 19:
					Direction = 1;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 20:
					Direction = 2;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 21:
					Direction = 0;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 22:
					Direction = 0;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 23:
					Direction = 2;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 24:
					Direction = 2;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 25:
					Direction = 3;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 26:
					Direction = 3;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 27:
					Direction = 0;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 28:
					Direction = 3;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 29:
					Direction = 0;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 30:
					Direction = 0;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 31:
					Direction = 2;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 0;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 1;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 2;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 3;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 4;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 5;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 6;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 7;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 8;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 9;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 10;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 11;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 12;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 13;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 14;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 15;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 16;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 17;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 18;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 19;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 20;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 21;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 22;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 23;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 24;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 25;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 26;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 27;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 28;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 29;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 30;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 31;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class SpruceFenceGate // 183 typeof=SpruceFenceGate
	{
		[Range(0, 3)] public int Direction { get; set; }
		[Range(0, 1)] public byte InWallBit { get; set; }
		[Range(0, 1)] public byte OpenBit { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Direction = 3;
					InWallBit = 0;
					OpenBit = 1;
					break;
				case 1:
					Direction = 2;
					InWallBit = 1;
					OpenBit = 0;
					break;
				case 2:
					Direction = 1;
					InWallBit = 1;
					OpenBit = 1;
					break;
				case 3:
					Direction = 0;
					InWallBit = 1;
					OpenBit = 1;
					break;
				case 4:
					Direction = 3;
					InWallBit = 1;
					OpenBit = 1;
					break;
				case 5:
					Direction = 0;
					InWallBit = 0;
					OpenBit = 1;
					break;
				case 6:
					Direction = 1;
					InWallBit = 1;
					OpenBit = 0;
					break;
				case 7:
					Direction = 2;
					InWallBit = 0;
					OpenBit = 1;
					break;
				case 8:
					Direction = 1;
					InWallBit = 0;
					OpenBit = 0;
					break;
				case 9:
					Direction = 2;
					InWallBit = 1;
					OpenBit = 1;
					break;
				case 10:
					Direction = 3;
					InWallBit = 1;
					OpenBit = 0;
					break;
				case 11:
					Direction = 2;
					InWallBit = 0;
					OpenBit = 0;
					break;
				case 12:
					Direction = 0;
					InWallBit = 1;
					OpenBit = 0;
					break;
				case 13:
					Direction = 1;
					InWallBit = 0;
					OpenBit = 1;
					break;
				case 14:
					Direction = 3;
					InWallBit = 0;
					OpenBit = 0;
					break;
				case 15:
					Direction = 0;
					InWallBit = 0;
					OpenBit = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Direction == 3 && b.InWallBit == 0 && b.OpenBit == 1:
					return 0;
				case { } b when true && b.Direction == 2 && b.InWallBit == 1 && b.OpenBit == 0:
					return 1;
				case { } b when true && b.Direction == 1 && b.InWallBit == 1 && b.OpenBit == 1:
					return 2;
				case { } b when true && b.Direction == 0 && b.InWallBit == 1 && b.OpenBit == 1:
					return 3;
				case { } b when true && b.Direction == 3 && b.InWallBit == 1 && b.OpenBit == 1:
					return 4;
				case { } b when true && b.Direction == 0 && b.InWallBit == 0 && b.OpenBit == 1:
					return 5;
				case { } b when true && b.Direction == 1 && b.InWallBit == 1 && b.OpenBit == 0:
					return 6;
				case { } b when true && b.Direction == 2 && b.InWallBit == 0 && b.OpenBit == 1:
					return 7;
				case { } b when true && b.Direction == 1 && b.InWallBit == 0 && b.OpenBit == 0:
					return 8;
				case { } b when true && b.Direction == 2 && b.InWallBit == 1 && b.OpenBit == 1:
					return 9;
				case { } b when true && b.Direction == 3 && b.InWallBit == 1 && b.OpenBit == 0:
					return 10;
				case { } b when true && b.Direction == 2 && b.InWallBit == 0 && b.OpenBit == 0:
					return 11;
				case { } b when true && b.Direction == 0 && b.InWallBit == 1 && b.OpenBit == 0:
					return 12;
				case { } b when true && b.Direction == 1 && b.InWallBit == 0 && b.OpenBit == 1:
					return 13;
				case { } b when true && b.Direction == 3 && b.InWallBit == 0 && b.OpenBit == 0:
					return 14;
				case { } b when true && b.Direction == 0 && b.InWallBit == 0 && b.OpenBit == 0:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class SprucePressurePlate : Block // 0 typeof=Block
	{
		[Range(0, 9)] public int RedstoneSignal { get; set; }

		public SprucePressurePlate() : base(409)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					RedstoneSignal = 12;
					break;
				case 1:
					RedstoneSignal = 3;
					break;
				case 2:
					RedstoneSignal = 5;
					break;
				case 3:
					RedstoneSignal = 0;
					break;
				case 4:
					RedstoneSignal = 15;
					break;
				case 5:
					RedstoneSignal = 9;
					break;
				case 6:
					RedstoneSignal = 14;
					break;
				case 7:
					RedstoneSignal = 11;
					break;
				case 8:
					RedstoneSignal = 8;
					break;
				case 9:
					RedstoneSignal = 2;
					break;
				case 10:
					RedstoneSignal = 7;
					break;
				case 11:
					RedstoneSignal = 6;
					break;
				case 12:
					RedstoneSignal = 10;
					break;
				case 13:
					RedstoneSignal = 13;
					break;
				case 14:
					RedstoneSignal = 1;
					break;
				case 15:
					RedstoneSignal = 4;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.RedstoneSignal == 12:
					return 0;
				case { } b when true && b.RedstoneSignal == 3:
					return 1;
				case { } b when true && b.RedstoneSignal == 5:
					return 2;
				case { } b when true && b.RedstoneSignal == 0:
					return 3;
				case { } b when true && b.RedstoneSignal == 15:
					return 4;
				case { } b when true && b.RedstoneSignal == 9:
					return 5;
				case { } b when true && b.RedstoneSignal == 14:
					return 6;
				case { } b when true && b.RedstoneSignal == 11:
					return 7;
				case { } b when true && b.RedstoneSignal == 8:
					return 8;
				case { } b when true && b.RedstoneSignal == 2:
					return 9;
				case { } b when true && b.RedstoneSignal == 7:
					return 10;
				case { } b when true && b.RedstoneSignal == 6:
					return 11;
				case { } b when true && b.RedstoneSignal == 10:
					return 12;
				case { } b when true && b.RedstoneSignal == 13:
					return 13;
				case { } b when true && b.RedstoneSignal == 1:
					return 14;
				case { } b when true && b.RedstoneSignal == 4:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class SpruceStairs // 134 typeof=SpruceStairs
	{
		[Range(0, 1)] public byte UpsideDownBit { get; set; }
		[Range(0, 3)] public int WeirdoDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					UpsideDownBit = 0;
					WeirdoDirection = 3;
					break;
				case 1:
					UpsideDownBit = 0;
					WeirdoDirection = 0;
					break;
				case 2:
					UpsideDownBit = 0;
					WeirdoDirection = 1;
					break;
				case 3:
					UpsideDownBit = 1;
					WeirdoDirection = 3;
					break;
				case 4:
					UpsideDownBit = 1;
					WeirdoDirection = 0;
					break;
				case 5:
					UpsideDownBit = 1;
					WeirdoDirection = 2;
					break;
				case 6:
					UpsideDownBit = 1;
					WeirdoDirection = 1;
					break;
				case 7:
					UpsideDownBit = 0;
					WeirdoDirection = 2;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 3:
					return 0;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 0:
					return 1;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 1:
					return 2;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 3:
					return 3;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 0:
					return 4;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 2:
					return 5;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 1:
					return 6;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 2:
					return 7;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class SpruceStandingSign : Block // 0 typeof=Block
	{
		[Range(0, 9)] public int GroundSignDirection { get; set; }

		public SpruceStandingSign() : base(436)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					GroundSignDirection = 4;
					break;
				case 1:
					GroundSignDirection = 5;
					break;
				case 2:
					GroundSignDirection = 10;
					break;
				case 3:
					GroundSignDirection = 11;
					break;
				case 4:
					GroundSignDirection = 9;
					break;
				case 5:
					GroundSignDirection = 12;
					break;
				case 6:
					GroundSignDirection = 15;
					break;
				case 7:
					GroundSignDirection = 0;
					break;
				case 8:
					GroundSignDirection = 3;
					break;
				case 9:
					GroundSignDirection = 6;
					break;
				case 10:
					GroundSignDirection = 1;
					break;
				case 11:
					GroundSignDirection = 2;
					break;
				case 12:
					GroundSignDirection = 14;
					break;
				case 13:
					GroundSignDirection = 7;
					break;
				case 14:
					GroundSignDirection = 13;
					break;
				case 15:
					GroundSignDirection = 8;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.GroundSignDirection == 4:
					return 0;
				case { } b when true && b.GroundSignDirection == 5:
					return 1;
				case { } b when true && b.GroundSignDirection == 10:
					return 2;
				case { } b when true && b.GroundSignDirection == 11:
					return 3;
				case { } b when true && b.GroundSignDirection == 9:
					return 4;
				case { } b when true && b.GroundSignDirection == 12:
					return 5;
				case { } b when true && b.GroundSignDirection == 15:
					return 6;
				case { } b when true && b.GroundSignDirection == 0:
					return 7;
				case { } b when true && b.GroundSignDirection == 3:
					return 8;
				case { } b when true && b.GroundSignDirection == 6:
					return 9;
				case { } b when true && b.GroundSignDirection == 1:
					return 10;
				case { } b when true && b.GroundSignDirection == 2:
					return 11;
				case { } b when true && b.GroundSignDirection == 14:
					return 12;
				case { } b when true && b.GroundSignDirection == 7:
					return 13;
				case { } b when true && b.GroundSignDirection == 13:
					return 14;
				case { } b when true && b.GroundSignDirection == 8:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class SpruceTrapdoor : Block // 0 typeof=Block
	{
		[Range(0, 3)] public int Direction { get; set; }
		[Range(0, 1)] public byte OpenBit { get; set; }
		[Range(0, 1)] public byte UpsideDownBit { get; set; }

		public SpruceTrapdoor() : base(404)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Direction = 1;
					OpenBit = 1;
					UpsideDownBit = 1;
					break;
				case 1:
					Direction = 1;
					OpenBit = 1;
					UpsideDownBit = 0;
					break;
				case 2:
					Direction = 0;
					OpenBit = 0;
					UpsideDownBit = 1;
					break;
				case 3:
					Direction = 3;
					OpenBit = 1;
					UpsideDownBit = 1;
					break;
				case 4:
					Direction = 0;
					OpenBit = 1;
					UpsideDownBit = 0;
					break;
				case 5:
					Direction = 3;
					OpenBit = 0;
					UpsideDownBit = 0;
					break;
				case 6:
					Direction = 2;
					OpenBit = 0;
					UpsideDownBit = 1;
					break;
				case 7:
					Direction = 3;
					OpenBit = 1;
					UpsideDownBit = 0;
					break;
				case 8:
					Direction = 3;
					OpenBit = 0;
					UpsideDownBit = 1;
					break;
				case 9:
					Direction = 2;
					OpenBit = 1;
					UpsideDownBit = 1;
					break;
				case 10:
					Direction = 2;
					OpenBit = 1;
					UpsideDownBit = 0;
					break;
				case 11:
					Direction = 0;
					OpenBit = 1;
					UpsideDownBit = 1;
					break;
				case 12:
					Direction = 1;
					OpenBit = 0;
					UpsideDownBit = 0;
					break;
				case 13:
					Direction = 0;
					OpenBit = 0;
					UpsideDownBit = 0;
					break;
				case 14:
					Direction = 2;
					OpenBit = 0;
					UpsideDownBit = 0;
					break;
				case 15:
					Direction = 1;
					OpenBit = 0;
					UpsideDownBit = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Direction == 1 && b.OpenBit == 1 && b.UpsideDownBit == 1:
					return 0;
				case { } b when true && b.Direction == 1 && b.OpenBit == 1 && b.UpsideDownBit == 0:
					return 1;
				case { } b when true && b.Direction == 0 && b.OpenBit == 0 && b.UpsideDownBit == 1:
					return 2;
				case { } b when true && b.Direction == 3 && b.OpenBit == 1 && b.UpsideDownBit == 1:
					return 3;
				case { } b when true && b.Direction == 0 && b.OpenBit == 1 && b.UpsideDownBit == 0:
					return 4;
				case { } b when true && b.Direction == 3 && b.OpenBit == 0 && b.UpsideDownBit == 0:
					return 5;
				case { } b when true && b.Direction == 2 && b.OpenBit == 0 && b.UpsideDownBit == 1:
					return 6;
				case { } b when true && b.Direction == 3 && b.OpenBit == 1 && b.UpsideDownBit == 0:
					return 7;
				case { } b when true && b.Direction == 3 && b.OpenBit == 0 && b.UpsideDownBit == 1:
					return 8;
				case { } b when true && b.Direction == 2 && b.OpenBit == 1 && b.UpsideDownBit == 1:
					return 9;
				case { } b when true && b.Direction == 2 && b.OpenBit == 1 && b.UpsideDownBit == 0:
					return 10;
				case { } b when true && b.Direction == 0 && b.OpenBit == 1 && b.UpsideDownBit == 1:
					return 11;
				case { } b when true && b.Direction == 1 && b.OpenBit == 0 && b.UpsideDownBit == 0:
					return 12;
				case { } b when true && b.Direction == 0 && b.OpenBit == 0 && b.UpsideDownBit == 0:
					return 13;
				case { } b when true && b.Direction == 2 && b.OpenBit == 0 && b.UpsideDownBit == 0:
					return 14;
				case { } b when true && b.Direction == 1 && b.OpenBit == 0 && b.UpsideDownBit == 1:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class SpruceWallSign : Block // 0 typeof=Block
	{
		[Range(0, 5)] public int FacingDirection { get; set; }

		public SpruceWallSign() : base(437)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					FacingDirection = 4;
					break;
				case 1:
					FacingDirection = 5;
					break;
				case 2:
					FacingDirection = 1;
					break;
				case 3:
					FacingDirection = 3;
					break;
				case 4:
					FacingDirection = 2;
					break;
				case 5:
					FacingDirection = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.FacingDirection == 4:
					return 0;
				case { } b when true && b.FacingDirection == 5:
					return 1;
				case { } b when true && b.FacingDirection == 1:
					return 2;
				case { } b when true && b.FacingDirection == 3:
					return 3;
				case { } b when true && b.FacingDirection == 2:
					return 4;
				case { } b when true && b.FacingDirection == 0:
					return 5;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class StainedGlass // 241 typeof=StainedGlass
	{
		// Convert this attribute to enum
		//[Enum("black","blue","brown","cyan","gray","green","light_blue","lime","magenta","orange","pink","purple","red","silver","white","yellow"]
		public string Color { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Color = "pink";
					break;
				case 1:
					Color = "yellow";
					break;
				case 2:
					Color = "light_blue";
					break;
				case 3:
					Color = "gray";
					break;
				case 4:
					Color = "lime";
					break;
				case 5:
					Color = "magenta";
					break;
				case 6:
					Color = "black";
					break;
				case 7:
					Color = "blue";
					break;
				case 8:
					Color = "orange";
					break;
				case 9:
					Color = "brown";
					break;
				case 10:
					Color = "red";
					break;
				case 11:
					Color = "green";
					break;
				case 12:
					Color = "silver";
					break;
				case 13:
					Color = "white";
					break;
				case 14:
					Color = "cyan";
					break;
				case 15:
					Color = "purple";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Color == "pink":
					return 0;
				case { } b when true && b.Color == "yellow":
					return 1;
				case { } b when true && b.Color == "light_blue":
					return 2;
				case { } b when true && b.Color == "gray":
					return 3;
				case { } b when true && b.Color == "lime":
					return 4;
				case { } b when true && b.Color == "magenta":
					return 5;
				case { } b when true && b.Color == "black":
					return 6;
				case { } b when true && b.Color == "blue":
					return 7;
				case { } b when true && b.Color == "orange":
					return 8;
				case { } b when true && b.Color == "brown":
					return 9;
				case { } b when true && b.Color == "red":
					return 10;
				case { } b when true && b.Color == "green":
					return 11;
				case { } b when true && b.Color == "silver":
					return 12;
				case { } b when true && b.Color == "white":
					return 13;
				case { } b when true && b.Color == "cyan":
					return 14;
				case { } b when true && b.Color == "purple":
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class StainedGlassPane // 160 typeof=StainedGlassPane
	{
		// Convert this attribute to enum
		//[Enum("black","blue","brown","cyan","gray","green","light_blue","lime","magenta","orange","pink","purple","red","silver","white","yellow"]
		public string Color { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Color = "magenta";
					break;
				case 1:
					Color = "gray";
					break;
				case 2:
					Color = "lime";
					break;
				case 3:
					Color = "pink";
					break;
				case 4:
					Color = "green";
					break;
				case 5:
					Color = "purple";
					break;
				case 6:
					Color = "yellow";
					break;
				case 7:
					Color = "white";
					break;
				case 8:
					Color = "light_blue";
					break;
				case 9:
					Color = "brown";
					break;
				case 10:
					Color = "orange";
					break;
				case 11:
					Color = "silver";
					break;
				case 12:
					Color = "cyan";
					break;
				case 13:
					Color = "black";
					break;
				case 14:
					Color = "red";
					break;
				case 15:
					Color = "blue";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Color == "magenta":
					return 0;
				case { } b when true && b.Color == "gray":
					return 1;
				case { } b when true && b.Color == "lime":
					return 2;
				case { } b when true && b.Color == "pink":
					return 3;
				case { } b when true && b.Color == "green":
					return 4;
				case { } b when true && b.Color == "purple":
					return 5;
				case { } b when true && b.Color == "yellow":
					return 6;
				case { } b when true && b.Color == "white":
					return 7;
				case { } b when true && b.Color == "light_blue":
					return 8;
				case { } b when true && b.Color == "brown":
					return 9;
				case { } b when true && b.Color == "orange":
					return 10;
				case { } b when true && b.Color == "silver":
					return 11;
				case { } b when true && b.Color == "cyan":
					return 12;
				case { } b when true && b.Color == "black":
					return 13;
				case { } b when true && b.Color == "red":
					return 14;
				case { } b when true && b.Color == "blue":
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class StainedHardenedClay // 159 typeof=StainedHardenedClay
	{
		// Convert this attribute to enum
		//[Enum("black","blue","brown","cyan","gray","green","light_blue","lime","magenta","orange","pink","purple","red","silver","white","yellow"]
		public string Color { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Color = "brown";
					break;
				case 1:
					Color = "black";
					break;
				case 2:
					Color = "gray";
					break;
				case 3:
					Color = "green";
					break;
				case 4:
					Color = "orange";
					break;
				case 5:
					Color = "lime";
					break;
				case 6:
					Color = "pink";
					break;
				case 7:
					Color = "white";
					break;
				case 8:
					Color = "light_blue";
					break;
				case 9:
					Color = "silver";
					break;
				case 10:
					Color = "cyan";
					break;
				case 11:
					Color = "red";
					break;
				case 12:
					Color = "magenta";
					break;
				case 13:
					Color = "blue";
					break;
				case 14:
					Color = "purple";
					break;
				case 15:
					Color = "yellow";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Color == "brown":
					return 0;
				case { } b when true && b.Color == "black":
					return 1;
				case { } b when true && b.Color == "gray":
					return 2;
				case { } b when true && b.Color == "green":
					return 3;
				case { } b when true && b.Color == "orange":
					return 4;
				case { } b when true && b.Color == "lime":
					return 5;
				case { } b when true && b.Color == "pink":
					return 6;
				case { } b when true && b.Color == "white":
					return 7;
				case { } b when true && b.Color == "light_blue":
					return 8;
				case { } b when true && b.Color == "silver":
					return 9;
				case { } b when true && b.Color == "cyan":
					return 10;
				case { } b when true && b.Color == "red":
					return 11;
				case { } b when true && b.Color == "magenta":
					return 12;
				case { } b when true && b.Color == "blue":
					return 13;
				case { } b when true && b.Color == "purple":
					return 14;
				case { } b when true && b.Color == "yellow":
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class StandingBanner // 176 typeof=StandingBanner
	{
		[Range(0, 9)] public int GroundSignDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					GroundSignDirection = 15;
					break;
				case 1:
					GroundSignDirection = 6;
					break;
				case 2:
					GroundSignDirection = 8;
					break;
				case 3:
					GroundSignDirection = 4;
					break;
				case 4:
					GroundSignDirection = 11;
					break;
				case 5:
					GroundSignDirection = 10;
					break;
				case 6:
					GroundSignDirection = 3;
					break;
				case 7:
					GroundSignDirection = 5;
					break;
				case 8:
					GroundSignDirection = 1;
					break;
				case 9:
					GroundSignDirection = 0;
					break;
				case 10:
					GroundSignDirection = 12;
					break;
				case 11:
					GroundSignDirection = 14;
					break;
				case 12:
					GroundSignDirection = 13;
					break;
				case 13:
					GroundSignDirection = 7;
					break;
				case 14:
					GroundSignDirection = 2;
					break;
				case 15:
					GroundSignDirection = 9;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.GroundSignDirection == 15:
					return 0;
				case { } b when true && b.GroundSignDirection == 6:
					return 1;
				case { } b when true && b.GroundSignDirection == 8:
					return 2;
				case { } b when true && b.GroundSignDirection == 4:
					return 3;
				case { } b when true && b.GroundSignDirection == 11:
					return 4;
				case { } b when true && b.GroundSignDirection == 10:
					return 5;
				case { } b when true && b.GroundSignDirection == 3:
					return 6;
				case { } b when true && b.GroundSignDirection == 5:
					return 7;
				case { } b when true && b.GroundSignDirection == 1:
					return 8;
				case { } b when true && b.GroundSignDirection == 0:
					return 9;
				case { } b when true && b.GroundSignDirection == 12:
					return 10;
				case { } b when true && b.GroundSignDirection == 14:
					return 11;
				case { } b when true && b.GroundSignDirection == 13:
					return 12;
				case { } b when true && b.GroundSignDirection == 7:
					return 13;
				case { } b when true && b.GroundSignDirection == 2:
					return 14;
				case { } b when true && b.GroundSignDirection == 9:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class StandingSign // 63 typeof=StandingSign
	{
		[Range(0, 9)] public int GroundSignDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					GroundSignDirection = 4;
					break;
				case 1:
					GroundSignDirection = 7;
					break;
				case 2:
					GroundSignDirection = 11;
					break;
				case 3:
					GroundSignDirection = 1;
					break;
				case 4:
					GroundSignDirection = 3;
					break;
				case 5:
					GroundSignDirection = 12;
					break;
				case 6:
					GroundSignDirection = 6;
					break;
				case 7:
					GroundSignDirection = 9;
					break;
				case 8:
					GroundSignDirection = 0;
					break;
				case 9:
					GroundSignDirection = 13;
					break;
				case 10:
					GroundSignDirection = 14;
					break;
				case 11:
					GroundSignDirection = 10;
					break;
				case 12:
					GroundSignDirection = 2;
					break;
				case 13:
					GroundSignDirection = 8;
					break;
				case 14:
					GroundSignDirection = 5;
					break;
				case 15:
					GroundSignDirection = 15;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.GroundSignDirection == 4:
					return 0;
				case { } b when true && b.GroundSignDirection == 7:
					return 1;
				case { } b when true && b.GroundSignDirection == 11:
					return 2;
				case { } b when true && b.GroundSignDirection == 1:
					return 3;
				case { } b when true && b.GroundSignDirection == 3:
					return 4;
				case { } b when true && b.GroundSignDirection == 12:
					return 5;
				case { } b when true && b.GroundSignDirection == 6:
					return 6;
				case { } b when true && b.GroundSignDirection == 9:
					return 7;
				case { } b when true && b.GroundSignDirection == 0:
					return 8;
				case { } b when true && b.GroundSignDirection == 13:
					return 9;
				case { } b when true && b.GroundSignDirection == 14:
					return 10;
				case { } b when true && b.GroundSignDirection == 10:
					return 11;
				case { } b when true && b.GroundSignDirection == 2:
					return 12;
				case { } b when true && b.GroundSignDirection == 8:
					return 13;
				case { } b when true && b.GroundSignDirection == 5:
					return 14;
				case { } b when true && b.GroundSignDirection == 15:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class StickyPiston // 29 typeof=StickyPiston
	{
		[Range(0, 5)] public int FacingDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					FacingDirection = 0;
					break;
				case 1:
					FacingDirection = 3;
					break;
				case 2:
					FacingDirection = 5;
					break;
				case 3:
					FacingDirection = 4;
					break;
				case 4:
					FacingDirection = 1;
					break;
				case 5:
					FacingDirection = 2;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.FacingDirection == 0:
					return 0;
				case { } b when true && b.FacingDirection == 3:
					return 1;
				case { } b when true && b.FacingDirection == 5:
					return 2;
				case { } b when true && b.FacingDirection == 4:
					return 3;
				case { } b when true && b.FacingDirection == 1:
					return 4;
				case { } b when true && b.FacingDirection == 2:
					return 5;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class StickyPistonArmCollision : Block // 0 typeof=Block
	{
		[Range(0, 5)] public int FacingDirection { get; set; }

		public StickyPistonArmCollision() : base(472)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					FacingDirection = 5;
					break;
				case 1:
					FacingDirection = 2;
					break;
				case 2:
					FacingDirection = 3;
					break;
				case 3:
					FacingDirection = 0;
					break;
				case 4:
					FacingDirection = 1;
					break;
				case 5:
					FacingDirection = 4;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.FacingDirection == 5:
					return 0;
				case { } b when true && b.FacingDirection == 2:
					return 1;
				case { } b when true && b.FacingDirection == 3:
					return 2;
				case { } b when true && b.FacingDirection == 0:
					return 3;
				case { } b when true && b.FacingDirection == 1:
					return 4;
				case { } b when true && b.FacingDirection == 4:
					return 5;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Stone // 1 typeof=Stone
	{
		// Convert this attribute to enum
		//[Enum("andesite","andesite_smooth","diorite","diorite_smooth","granite","granite_smooth","stone"]
		public string StoneType { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					StoneType = "granite";
					break;
				case 1:
					StoneType = "diorite";
					break;
				case 2:
					StoneType = "andesite_smooth";
					break;
				case 3:
					StoneType = "stone";
					break;
				case 4:
					StoneType = "andesite";
					break;
				case 5:
					StoneType = "granite_smooth";
					break;
				case 6:
					StoneType = "diorite_smooth";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.StoneType == "granite":
					return 0;
				case { } b when true && b.StoneType == "diorite":
					return 1;
				case { } b when true && b.StoneType == "andesite_smooth":
					return 2;
				case { } b when true && b.StoneType == "stone":
					return 3;
				case { } b when true && b.StoneType == "andesite":
					return 4;
				case { } b when true && b.StoneType == "granite_smooth":
					return 5;
				case { } b when true && b.StoneType == "diorite_smooth":
					return 6;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class StoneBrickStairs // 109 typeof=StoneBrickStairs
	{
		[Range(0, 1)] public byte UpsideDownBit { get; set; }
		[Range(0, 3)] public int WeirdoDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					UpsideDownBit = 1;
					WeirdoDirection = 0;
					break;
				case 1:
					UpsideDownBit = 1;
					WeirdoDirection = 1;
					break;
				case 2:
					UpsideDownBit = 0;
					WeirdoDirection = 3;
					break;
				case 3:
					UpsideDownBit = 0;
					WeirdoDirection = 0;
					break;
				case 4:
					UpsideDownBit = 1;
					WeirdoDirection = 2;
					break;
				case 5:
					UpsideDownBit = 0;
					WeirdoDirection = 2;
					break;
				case 6:
					UpsideDownBit = 1;
					WeirdoDirection = 3;
					break;
				case 7:
					UpsideDownBit = 0;
					WeirdoDirection = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 0:
					return 0;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 1:
					return 1;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 3:
					return 2;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 0:
					return 3;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 2:
					return 4;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 2:
					return 5;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 3:
					return 6;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 1:
					return 7;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class StoneButton // 77 typeof=StoneButton
	{
		[Range(0, 1)] public byte ButtonPressedBit { get; set; }
		[Range(0, 5)] public int FacingDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					ButtonPressedBit = 0;
					FacingDirection = 5;
					break;
				case 1:
					ButtonPressedBit = 1;
					FacingDirection = 4;
					break;
				case 2:
					ButtonPressedBit = 0;
					FacingDirection = 2;
					break;
				case 3:
					ButtonPressedBit = 1;
					FacingDirection = 5;
					break;
				case 4:
					ButtonPressedBit = 1;
					FacingDirection = 2;
					break;
				case 5:
					ButtonPressedBit = 0;
					FacingDirection = 0;
					break;
				case 6:
					ButtonPressedBit = 0;
					FacingDirection = 1;
					break;
				case 7:
					ButtonPressedBit = 1;
					FacingDirection = 0;
					break;
				case 8:
					ButtonPressedBit = 1;
					FacingDirection = 3;
					break;
				case 9:
					ButtonPressedBit = 0;
					FacingDirection = 3;
					break;
				case 10:
					ButtonPressedBit = 0;
					FacingDirection = 4;
					break;
				case 11:
					ButtonPressedBit = 1;
					FacingDirection = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.ButtonPressedBit == 0 && b.FacingDirection == 5:
					return 0;
				case { } b when true && b.ButtonPressedBit == 1 && b.FacingDirection == 4:
					return 1;
				case { } b when true && b.ButtonPressedBit == 0 && b.FacingDirection == 2:
					return 2;
				case { } b when true && b.ButtonPressedBit == 1 && b.FacingDirection == 5:
					return 3;
				case { } b when true && b.ButtonPressedBit == 1 && b.FacingDirection == 2:
					return 4;
				case { } b when true && b.ButtonPressedBit == 0 && b.FacingDirection == 0:
					return 5;
				case { } b when true && b.ButtonPressedBit == 0 && b.FacingDirection == 1:
					return 6;
				case { } b when true && b.ButtonPressedBit == 1 && b.FacingDirection == 0:
					return 7;
				case { } b when true && b.ButtonPressedBit == 1 && b.FacingDirection == 3:
					return 8;
				case { } b when true && b.ButtonPressedBit == 0 && b.FacingDirection == 3:
					return 9;
				case { } b when true && b.ButtonPressedBit == 0 && b.FacingDirection == 4:
					return 10;
				case { } b when true && b.ButtonPressedBit == 1 && b.FacingDirection == 1:
					return 11;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class StonePressurePlate // 70 typeof=StonePressurePlate
	{
		[Range(0, 9)] public int RedstoneSignal { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					RedstoneSignal = 3;
					break;
				case 1:
					RedstoneSignal = 10;
					break;
				case 2:
					RedstoneSignal = 6;
					break;
				case 3:
					RedstoneSignal = 15;
					break;
				case 4:
					RedstoneSignal = 14;
					break;
				case 5:
					RedstoneSignal = 8;
					break;
				case 6:
					RedstoneSignal = 2;
					break;
				case 7:
					RedstoneSignal = 11;
					break;
				case 8:
					RedstoneSignal = 0;
					break;
				case 9:
					RedstoneSignal = 4;
					break;
				case 10:
					RedstoneSignal = 13;
					break;
				case 11:
					RedstoneSignal = 12;
					break;
				case 12:
					RedstoneSignal = 1;
					break;
				case 13:
					RedstoneSignal = 9;
					break;
				case 14:
					RedstoneSignal = 7;
					break;
				case 15:
					RedstoneSignal = 5;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.RedstoneSignal == 3:
					return 0;
				case { } b when true && b.RedstoneSignal == 10:
					return 1;
				case { } b when true && b.RedstoneSignal == 6:
					return 2;
				case { } b when true && b.RedstoneSignal == 15:
					return 3;
				case { } b when true && b.RedstoneSignal == 14:
					return 4;
				case { } b when true && b.RedstoneSignal == 8:
					return 5;
				case { } b when true && b.RedstoneSignal == 2:
					return 6;
				case { } b when true && b.RedstoneSignal == 11:
					return 7;
				case { } b when true && b.RedstoneSignal == 0:
					return 8;
				case { } b when true && b.RedstoneSignal == 4:
					return 9;
				case { } b when true && b.RedstoneSignal == 13:
					return 10;
				case { } b when true && b.RedstoneSignal == 12:
					return 11;
				case { } b when true && b.RedstoneSignal == 1:
					return 12;
				case { } b when true && b.RedstoneSignal == 9:
					return 13;
				case { } b when true && b.RedstoneSignal == 7:
					return 14;
				case { } b when true && b.RedstoneSignal == 5:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class StoneSlab // 44 typeof=StoneSlab
	{
		// Convert this attribute to enum
		//[Enum("brick","cobblestone","nether_brick","quartz","sandstone","smooth_stone","stone_brick","wood"]
		public string StoneSlabType { get; set; }
		[Range(0, 1)] public byte TopSlotBit { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					StoneSlabType = "sandstone";
					TopSlotBit = 1;
					break;
				case 1:
					StoneSlabType = "stone_brick";
					TopSlotBit = 1;
					break;
				case 2:
					StoneSlabType = "nether_brick";
					TopSlotBit = 0;
					break;
				case 3:
					StoneSlabType = "wood";
					TopSlotBit = 1;
					break;
				case 4:
					StoneSlabType = "smooth_stone";
					TopSlotBit = 0;
					break;
				case 5:
					StoneSlabType = "wood";
					TopSlotBit = 0;
					break;
				case 6:
					StoneSlabType = "nether_brick";
					TopSlotBit = 1;
					break;
				case 7:
					StoneSlabType = "smooth_stone";
					TopSlotBit = 1;
					break;
				case 8:
					StoneSlabType = "cobblestone";
					TopSlotBit = 1;
					break;
				case 9:
					StoneSlabType = "sandstone";
					TopSlotBit = 0;
					break;
				case 10:
					StoneSlabType = "brick";
					TopSlotBit = 1;
					break;
				case 11:
					StoneSlabType = "quartz";
					TopSlotBit = 0;
					break;
				case 12:
					StoneSlabType = "stone_brick";
					TopSlotBit = 0;
					break;
				case 13:
					StoneSlabType = "brick";
					TopSlotBit = 0;
					break;
				case 14:
					StoneSlabType = "cobblestone";
					TopSlotBit = 0;
					break;
				case 15:
					StoneSlabType = "quartz";
					TopSlotBit = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.StoneSlabType == "sandstone" && b.TopSlotBit == 1:
					return 0;
				case { } b when true && b.StoneSlabType == "stone_brick" && b.TopSlotBit == 1:
					return 1;
				case { } b when true && b.StoneSlabType == "nether_brick" && b.TopSlotBit == 0:
					return 2;
				case { } b when true && b.StoneSlabType == "wood" && b.TopSlotBit == 1:
					return 3;
				case { } b when true && b.StoneSlabType == "smooth_stone" && b.TopSlotBit == 0:
					return 4;
				case { } b when true && b.StoneSlabType == "wood" && b.TopSlotBit == 0:
					return 5;
				case { } b when true && b.StoneSlabType == "nether_brick" && b.TopSlotBit == 1:
					return 6;
				case { } b when true && b.StoneSlabType == "smooth_stone" && b.TopSlotBit == 1:
					return 7;
				case { } b when true && b.StoneSlabType == "cobblestone" && b.TopSlotBit == 1:
					return 8;
				case { } b when true && b.StoneSlabType == "sandstone" && b.TopSlotBit == 0:
					return 9;
				case { } b when true && b.StoneSlabType == "brick" && b.TopSlotBit == 1:
					return 10;
				case { } b when true && b.StoneSlabType == "quartz" && b.TopSlotBit == 0:
					return 11;
				case { } b when true && b.StoneSlabType == "stone_brick" && b.TopSlotBit == 0:
					return 12;
				case { } b when true && b.StoneSlabType == "brick" && b.TopSlotBit == 0:
					return 13;
				case { } b when true && b.StoneSlabType == "cobblestone" && b.TopSlotBit == 0:
					return 14;
				case { } b when true && b.StoneSlabType == "quartz" && b.TopSlotBit == 1:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class StoneSlab2 // 182 typeof=StoneSlab2
	{
		// Convert this attribute to enum
		//[Enum("mossy_cobblestone","prismarine_brick","prismarine_dark","prismarine_rough","purpur","red_nether_brick","red_sandstone","smooth_sandstone"]
		public string StoneSlabType2 { get; set; }
		[Range(0, 1)] public byte TopSlotBit { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					StoneSlabType2 = "prismarine_rough";
					TopSlotBit = 1;
					break;
				case 1:
					StoneSlabType2 = "purpur";
					TopSlotBit = 0;
					break;
				case 2:
					StoneSlabType2 = "purpur";
					TopSlotBit = 1;
					break;
				case 3:
					StoneSlabType2 = "prismarine_brick";
					TopSlotBit = 1;
					break;
				case 4:
					StoneSlabType2 = "red_sandstone";
					TopSlotBit = 0;
					break;
				case 5:
					StoneSlabType2 = "prismarine_brick";
					TopSlotBit = 0;
					break;
				case 6:
					StoneSlabType2 = "prismarine_dark";
					TopSlotBit = 0;
					break;
				case 7:
					StoneSlabType2 = "smooth_sandstone";
					TopSlotBit = 0;
					break;
				case 8:
					StoneSlabType2 = "red_nether_brick";
					TopSlotBit = 1;
					break;
				case 9:
					StoneSlabType2 = "smooth_sandstone";
					TopSlotBit = 1;
					break;
				case 10:
					StoneSlabType2 = "prismarine_dark";
					TopSlotBit = 1;
					break;
				case 11:
					StoneSlabType2 = "red_sandstone";
					TopSlotBit = 1;
					break;
				case 12:
					StoneSlabType2 = "mossy_cobblestone";
					TopSlotBit = 1;
					break;
				case 13:
					StoneSlabType2 = "prismarine_rough";
					TopSlotBit = 0;
					break;
				case 14:
					StoneSlabType2 = "red_nether_brick";
					TopSlotBit = 0;
					break;
				case 15:
					StoneSlabType2 = "mossy_cobblestone";
					TopSlotBit = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.StoneSlabType2 == "prismarine_rough" && b.TopSlotBit == 1:
					return 0;
				case { } b when true && b.StoneSlabType2 == "purpur" && b.TopSlotBit == 0:
					return 1;
				case { } b when true && b.StoneSlabType2 == "purpur" && b.TopSlotBit == 1:
					return 2;
				case { } b when true && b.StoneSlabType2 == "prismarine_brick" && b.TopSlotBit == 1:
					return 3;
				case { } b when true && b.StoneSlabType2 == "red_sandstone" && b.TopSlotBit == 0:
					return 4;
				case { } b when true && b.StoneSlabType2 == "prismarine_brick" && b.TopSlotBit == 0:
					return 5;
				case { } b when true && b.StoneSlabType2 == "prismarine_dark" && b.TopSlotBit == 0:
					return 6;
				case { } b when true && b.StoneSlabType2 == "smooth_sandstone" && b.TopSlotBit == 0:
					return 7;
				case { } b when true && b.StoneSlabType2 == "red_nether_brick" && b.TopSlotBit == 1:
					return 8;
				case { } b when true && b.StoneSlabType2 == "smooth_sandstone" && b.TopSlotBit == 1:
					return 9;
				case { } b when true && b.StoneSlabType2 == "prismarine_dark" && b.TopSlotBit == 1:
					return 10;
				case { } b when true && b.StoneSlabType2 == "red_sandstone" && b.TopSlotBit == 1:
					return 11;
				case { } b when true && b.StoneSlabType2 == "mossy_cobblestone" && b.TopSlotBit == 1:
					return 12;
				case { } b when true && b.StoneSlabType2 == "prismarine_rough" && b.TopSlotBit == 0:
					return 13;
				case { } b when true && b.StoneSlabType2 == "red_nether_brick" && b.TopSlotBit == 0:
					return 14;
				case { } b when true && b.StoneSlabType2 == "mossy_cobblestone" && b.TopSlotBit == 0:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class StoneSlab3 : Block // 0 typeof=Block
	{
		// Convert this attribute to enum
		//[Enum("andesite","diorite","end_stone_brick","granite","polished_andesite","polished_diorite","polished_granite","smooth_red_sandstone"]
		public string StoneSlabType3 { get; set; }
		[Range(0, 1)] public byte TopSlotBit { get; set; }

		public StoneSlab3() : base(417)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					StoneSlabType3 = "end_stone_brick";
					TopSlotBit = 1;
					break;
				case 1:
					StoneSlabType3 = "granite";
					TopSlotBit = 0;
					break;
				case 2:
					StoneSlabType3 = "smooth_red_sandstone";
					TopSlotBit = 1;
					break;
				case 3:
					StoneSlabType3 = "andesite";
					TopSlotBit = 1;
					break;
				case 4:
					StoneSlabType3 = "diorite";
					TopSlotBit = 0;
					break;
				case 5:
					StoneSlabType3 = "smooth_red_sandstone";
					TopSlotBit = 0;
					break;
				case 6:
					StoneSlabType3 = "end_stone_brick";
					TopSlotBit = 0;
					break;
				case 7:
					StoneSlabType3 = "granite";
					TopSlotBit = 1;
					break;
				case 8:
					StoneSlabType3 = "polished_granite";
					TopSlotBit = 0;
					break;
				case 9:
					StoneSlabType3 = "polished_andesite";
					TopSlotBit = 0;
					break;
				case 10:
					StoneSlabType3 = "polished_diorite";
					TopSlotBit = 1;
					break;
				case 11:
					StoneSlabType3 = "polished_andesite";
					TopSlotBit = 1;
					break;
				case 12:
					StoneSlabType3 = "andesite";
					TopSlotBit = 0;
					break;
				case 13:
					StoneSlabType3 = "polished_granite";
					TopSlotBit = 1;
					break;
				case 14:
					StoneSlabType3 = "diorite";
					TopSlotBit = 1;
					break;
				case 15:
					StoneSlabType3 = "polished_diorite";
					TopSlotBit = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.StoneSlabType3 == "end_stone_brick" && b.TopSlotBit == 1:
					return 0;
				case { } b when true && b.StoneSlabType3 == "granite" && b.TopSlotBit == 0:
					return 1;
				case { } b when true && b.StoneSlabType3 == "smooth_red_sandstone" && b.TopSlotBit == 1:
					return 2;
				case { } b when true && b.StoneSlabType3 == "andesite" && b.TopSlotBit == 1:
					return 3;
				case { } b when true && b.StoneSlabType3 == "diorite" && b.TopSlotBit == 0:
					return 4;
				case { } b when true && b.StoneSlabType3 == "smooth_red_sandstone" && b.TopSlotBit == 0:
					return 5;
				case { } b when true && b.StoneSlabType3 == "end_stone_brick" && b.TopSlotBit == 0:
					return 6;
				case { } b when true && b.StoneSlabType3 == "granite" && b.TopSlotBit == 1:
					return 7;
				case { } b when true && b.StoneSlabType3 == "polished_granite" && b.TopSlotBit == 0:
					return 8;
				case { } b when true && b.StoneSlabType3 == "polished_andesite" && b.TopSlotBit == 0:
					return 9;
				case { } b when true && b.StoneSlabType3 == "polished_diorite" && b.TopSlotBit == 1:
					return 10;
				case { } b when true && b.StoneSlabType3 == "polished_andesite" && b.TopSlotBit == 1:
					return 11;
				case { } b when true && b.StoneSlabType3 == "andesite" && b.TopSlotBit == 0:
					return 12;
				case { } b when true && b.StoneSlabType3 == "polished_granite" && b.TopSlotBit == 1:
					return 13;
				case { } b when true && b.StoneSlabType3 == "diorite" && b.TopSlotBit == 1:
					return 14;
				case { } b when true && b.StoneSlabType3 == "polished_diorite" && b.TopSlotBit == 0:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class StoneSlab4 : Block // 0 typeof=Block
	{
		// Convert this attribute to enum
		//[Enum("cut_red_sandstone","cut_sandstone","mossy_stone_brick","smooth_quartz","stone"]
		public string StoneSlabType4 { get; set; }
		[Range(0, 1)] public byte TopSlotBit { get; set; }

		public StoneSlab4() : base(421)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					StoneSlabType4 = "stone";
					TopSlotBit = 1;
					break;
				case 1:
					StoneSlabType4 = "mossy_stone_brick";
					TopSlotBit = 1;
					break;
				case 2:
					StoneSlabType4 = "cut_red_sandstone";
					TopSlotBit = 1;
					break;
				case 3:
					StoneSlabType4 = "cut_sandstone";
					TopSlotBit = 1;
					break;
				case 4:
					StoneSlabType4 = "cut_red_sandstone";
					TopSlotBit = 0;
					break;
				case 5:
					StoneSlabType4 = "mossy_stone_brick";
					TopSlotBit = 0;
					break;
				case 6:
					StoneSlabType4 = "smooth_quartz";
					TopSlotBit = 1;
					break;
				case 7:
					StoneSlabType4 = "cut_sandstone";
					TopSlotBit = 0;
					break;
				case 8:
					StoneSlabType4 = "smooth_quartz";
					TopSlotBit = 0;
					break;
				case 9:
					StoneSlabType4 = "stone";
					TopSlotBit = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.StoneSlabType4 == "stone" && b.TopSlotBit == 1:
					return 0;
				case { } b when true && b.StoneSlabType4 == "mossy_stone_brick" && b.TopSlotBit == 1:
					return 1;
				case { } b when true && b.StoneSlabType4 == "cut_red_sandstone" && b.TopSlotBit == 1:
					return 2;
				case { } b when true && b.StoneSlabType4 == "cut_sandstone" && b.TopSlotBit == 1:
					return 3;
				case { } b when true && b.StoneSlabType4 == "cut_red_sandstone" && b.TopSlotBit == 0:
					return 4;
				case { } b when true && b.StoneSlabType4 == "mossy_stone_brick" && b.TopSlotBit == 0:
					return 5;
				case { } b when true && b.StoneSlabType4 == "smooth_quartz" && b.TopSlotBit == 1:
					return 6;
				case { } b when true && b.StoneSlabType4 == "cut_sandstone" && b.TopSlotBit == 0:
					return 7;
				case { } b when true && b.StoneSlabType4 == "smooth_quartz" && b.TopSlotBit == 0:
					return 8;
				case { } b when true && b.StoneSlabType4 == "stone" && b.TopSlotBit == 0:
					return 9;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class StoneStairs // 67 typeof=StoneStairs
	{
		[Range(0, 1)] public byte UpsideDownBit { get; set; }
		[Range(0, 3)] public int WeirdoDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					UpsideDownBit = 1;
					WeirdoDirection = 3;
					break;
				case 1:
					UpsideDownBit = 0;
					WeirdoDirection = 3;
					break;
				case 2:
					UpsideDownBit = 0;
					WeirdoDirection = 1;
					break;
				case 3:
					UpsideDownBit = 1;
					WeirdoDirection = 2;
					break;
				case 4:
					UpsideDownBit = 0;
					WeirdoDirection = 2;
					break;
				case 5:
					UpsideDownBit = 0;
					WeirdoDirection = 0;
					break;
				case 6:
					UpsideDownBit = 1;
					WeirdoDirection = 0;
					break;
				case 7:
					UpsideDownBit = 1;
					WeirdoDirection = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 3:
					return 0;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 3:
					return 1;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 1:
					return 2;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 2:
					return 3;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 2:
					return 4;
				case { } b when true && b.UpsideDownBit == 0 && b.WeirdoDirection == 0:
					return 5;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 0:
					return 6;
				case { } b when true && b.UpsideDownBit == 1 && b.WeirdoDirection == 1:
					return 7;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Stonebrick // 98 typeof=StoneBrick
	{
		// Convert this attribute to enum
		//[Enum("chiseled","cracked","default","mossy","smooth"]
		public string StoneBrickType { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					StoneBrickType = "cracked";
					break;
				case 1:
					StoneBrickType = "smooth";
					break;
				case 2:
					StoneBrickType = "mossy";
					break;
				case 3:
					StoneBrickType = "chiseled";
					break;
				case 4:
					StoneBrickType = "default";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.StoneBrickType == "cracked":
					return 0;
				case { } b when true && b.StoneBrickType == "smooth":
					return 1;
				case { } b when true && b.StoneBrickType == "mossy":
					return 2;
				case { } b when true && b.StoneBrickType == "chiseled":
					return 3;
				case { } b when true && b.StoneBrickType == "default":
					return 4;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Stonecutter // 245 typeof=Stonecutter
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class StonecutterBlock : Block // 0 typeof=Block
	{
		[Range(0, 5)] public int FacingDirection { get; set; }

		public StonecutterBlock() : base(452)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					FacingDirection = 0;
					break;
				case 1:
					FacingDirection = 4;
					break;
				case 2:
					FacingDirection = 1;
					break;
				case 3:
					FacingDirection = 3;
					break;
				case 4:
					FacingDirection = 5;
					break;
				case 5:
					FacingDirection = 2;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.FacingDirection == 0:
					return 0;
				case { } b when true && b.FacingDirection == 4:
					return 1;
				case { } b when true && b.FacingDirection == 1:
					return 2;
				case { } b when true && b.FacingDirection == 3:
					return 3;
				case { } b when true && b.FacingDirection == 5:
					return 4;
				case { } b when true && b.FacingDirection == 2:
					return 5;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class StrippedAcaciaLog : Block // 0 typeof=Block
	{
		// Convert this attribute to enum
		//[Enum("x","y","z"]
		public string PillarAxis { get; set; }

		public StrippedAcaciaLog() : base(263)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					PillarAxis = "y";
					break;
				case 1:
					PillarAxis = "z";
					break;
				case 2:
					PillarAxis = "x";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.PillarAxis == "y":
					return 0;
				case { } b when true && b.PillarAxis == "z":
					return 1;
				case { } b when true && b.PillarAxis == "x":
					return 2;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class StrippedBirchLog : Block // 0 typeof=Block
	{
		// Convert this attribute to enum
		//[Enum("x","y","z"]
		public string PillarAxis { get; set; }

		public StrippedBirchLog() : base(261)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					PillarAxis = "x";
					break;
				case 1:
					PillarAxis = "y";
					break;
				case 2:
					PillarAxis = "z";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.PillarAxis == "x":
					return 0;
				case { } b when true && b.PillarAxis == "y":
					return 1;
				case { } b when true && b.PillarAxis == "z":
					return 2;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class StrippedDarkOakLog : Block // 0 typeof=Block
	{
		// Convert this attribute to enum
		//[Enum("x","y","z"]
		public string PillarAxis { get; set; }

		public StrippedDarkOakLog() : base(264)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					PillarAxis = "z";
					break;
				case 1:
					PillarAxis = "x";
					break;
				case 2:
					PillarAxis = "y";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.PillarAxis == "z":
					return 0;
				case { } b when true && b.PillarAxis == "x":
					return 1;
				case { } b when true && b.PillarAxis == "y":
					return 2;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class StrippedJungleLog : Block // 0 typeof=Block
	{
		// Convert this attribute to enum
		//[Enum("x","y","z"]
		public string PillarAxis { get; set; }

		public StrippedJungleLog() : base(262)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					PillarAxis = "z";
					break;
				case 1:
					PillarAxis = "y";
					break;
				case 2:
					PillarAxis = "x";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.PillarAxis == "z":
					return 0;
				case { } b when true && b.PillarAxis == "y":
					return 1;
				case { } b when true && b.PillarAxis == "x":
					return 2;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class StrippedOakLog : Block // 0 typeof=Block
	{
		// Convert this attribute to enum
		//[Enum("x","y","z"]
		public string PillarAxis { get; set; }

		public StrippedOakLog() : base(265)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					PillarAxis = "y";
					break;
				case 1:
					PillarAxis = "z";
					break;
				case 2:
					PillarAxis = "x";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.PillarAxis == "y":
					return 0;
				case { } b when true && b.PillarAxis == "z":
					return 1;
				case { } b when true && b.PillarAxis == "x":
					return 2;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class StrippedSpruceLog : Block // 0 typeof=Block
	{
		// Convert this attribute to enum
		//[Enum("x","y","z"]
		public string PillarAxis { get; set; }

		public StrippedSpruceLog() : base(260)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					PillarAxis = "z";
					break;
				case 1:
					PillarAxis = "x";
					break;
				case 2:
					PillarAxis = "y";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.PillarAxis == "z":
					return 0;
				case { } b when true && b.PillarAxis == "x":
					return 1;
				case { } b when true && b.PillarAxis == "y":
					return 2;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class StructureBlock : Block // 0 typeof=Block
	{
		// Convert this attribute to enum
		//[Enum("corner","data","export","invalid","load","save"]
		public string StructureBlockType { get; set; }

		public StructureBlock() : base(252)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					StructureBlockType = "invalid";
					break;
				case 1:
					StructureBlockType = "save";
					break;
				case 2:
					StructureBlockType = "data";
					break;
				case 3:
					StructureBlockType = "load";
					break;
				case 4:
					StructureBlockType = "export";
					break;
				case 5:
					StructureBlockType = "corner";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.StructureBlockType == "invalid":
					return 0;
				case { } b when true && b.StructureBlockType == "save":
					return 1;
				case { } b when true && b.StructureBlockType == "data":
					return 2;
				case { } b when true && b.StructureBlockType == "load":
					return 3;
				case { } b when true && b.StructureBlockType == "export":
					return 4;
				case { } b when true && b.StructureBlockType == "corner":
					return 5;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class StructureVoid : Block // 0 typeof=Block
	{
		// Convert this attribute to enum
		//[Enum("air","void"]
		public string StructureVoidType { get; set; }

		public StructureVoid() : base(217)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					StructureVoidType = "void";
					break;
				case 1:
					StructureVoidType = "air";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.StructureVoidType == "void":
					return 0;
				case { } b when true && b.StructureVoidType == "air":
					return 1;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class SweetBerryBush : Block // 0 typeof=Block
	{
		[Range(0, 7)] public int Growth { get; set; }

		public SweetBerryBush() : base(462)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Growth = 6;
					break;
				case 1:
					Growth = 7;
					break;
				case 2:
					Growth = 2;
					break;
				case 3:
					Growth = 3;
					break;
				case 4:
					Growth = 0;
					break;
				case 5:
					Growth = 1;
					break;
				case 6:
					Growth = 5;
					break;
				case 7:
					Growth = 4;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Growth == 6:
					return 0;
				case { } b when true && b.Growth == 7:
					return 1;
				case { } b when true && b.Growth == 2:
					return 2;
				case { } b when true && b.Growth == 3:
					return 3;
				case { } b when true && b.Growth == 0:
					return 4;
				case { } b when true && b.Growth == 1:
					return 5;
				case { } b when true && b.Growth == 5:
					return 6;
				case { } b when true && b.Growth == 4:
					return 7;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Tallgrass // 31 typeof=TallGrass
	{
		// Convert this attribute to enum
		//[Enum("default","fern","snow","tall"]
		public string TallGrassType { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					TallGrassType = "fern";
					break;
				case 1:
					TallGrassType = "snow";
					break;
				case 2:
					TallGrassType = "tall";
					break;
				case 3:
					TallGrassType = "default";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.TallGrassType == "fern":
					return 0;
				case { } b when true && b.TallGrassType == "snow":
					return 1;
				case { } b when true && b.TallGrassType == "tall":
					return 2;
				case { } b when true && b.TallGrassType == "default":
					return 3;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Tnt // 46 typeof=Tnt
	{
		[Range(0, 1)] public byte AllowUnderwaterBit { get; set; }
		[Range(0, 1)] public byte ExplodeBit { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					AllowUnderwaterBit = 1;
					ExplodeBit = 0;
					break;
				case 1:
					AllowUnderwaterBit = 0;
					ExplodeBit = 1;
					break;
				case 2:
					AllowUnderwaterBit = 0;
					ExplodeBit = 0;
					break;
				case 3:
					AllowUnderwaterBit = 1;
					ExplodeBit = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.AllowUnderwaterBit == 1 && b.ExplodeBit == 0:
					return 0;
				case { } b when true && b.AllowUnderwaterBit == 0 && b.ExplodeBit == 1:
					return 1;
				case { } b when true && b.AllowUnderwaterBit == 0 && b.ExplodeBit == 0:
					return 2;
				case { } b when true && b.AllowUnderwaterBit == 1 && b.ExplodeBit == 1:
					return 3;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Torch // 50 typeof=Torch
	{
		// Convert this attribute to enum
		//[Enum("east","north","south","top","unknown","west"]
		public string TorchFacingDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					TorchFacingDirection = "north";
					break;
				case 1:
					TorchFacingDirection = "south";
					break;
				case 2:
					TorchFacingDirection = "top";
					break;
				case 3:
					TorchFacingDirection = "unknown";
					break;
				case 4:
					TorchFacingDirection = "east";
					break;
				case 5:
					TorchFacingDirection = "west";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.TorchFacingDirection == "north":
					return 0;
				case { } b when true && b.TorchFacingDirection == "south":
					return 1;
				case { } b when true && b.TorchFacingDirection == "top":
					return 2;
				case { } b when true && b.TorchFacingDirection == "unknown":
					return 3;
				case { } b when true && b.TorchFacingDirection == "east":
					return 4;
				case { } b when true && b.TorchFacingDirection == "west":
					return 5;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Trapdoor // 96 typeof=Trapdoor
	{
		[Range(0, 3)] public int Direction { get; set; }
		[Range(0, 1)] public byte OpenBit { get; set; }
		[Range(0, 1)] public byte UpsideDownBit { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Direction = 1;
					OpenBit = 0;
					UpsideDownBit = 1;
					break;
				case 1:
					Direction = 1;
					OpenBit = 0;
					UpsideDownBit = 0;
					break;
				case 2:
					Direction = 0;
					OpenBit = 1;
					UpsideDownBit = 1;
					break;
				case 3:
					Direction = 0;
					OpenBit = 0;
					UpsideDownBit = 1;
					break;
				case 4:
					Direction = 2;
					OpenBit = 1;
					UpsideDownBit = 1;
					break;
				case 5:
					Direction = 0;
					OpenBit = 1;
					UpsideDownBit = 0;
					break;
				case 6:
					Direction = 2;
					OpenBit = 1;
					UpsideDownBit = 0;
					break;
				case 7:
					Direction = 3;
					OpenBit = 1;
					UpsideDownBit = 0;
					break;
				case 8:
					Direction = 2;
					OpenBit = 0;
					UpsideDownBit = 0;
					break;
				case 9:
					Direction = 3;
					OpenBit = 0;
					UpsideDownBit = 0;
					break;
				case 10:
					Direction = 3;
					OpenBit = 0;
					UpsideDownBit = 1;
					break;
				case 11:
					Direction = 1;
					OpenBit = 1;
					UpsideDownBit = 0;
					break;
				case 12:
					Direction = 2;
					OpenBit = 0;
					UpsideDownBit = 1;
					break;
				case 13:
					Direction = 0;
					OpenBit = 0;
					UpsideDownBit = 0;
					break;
				case 14:
					Direction = 1;
					OpenBit = 1;
					UpsideDownBit = 1;
					break;
				case 15:
					Direction = 3;
					OpenBit = 1;
					UpsideDownBit = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Direction == 1 && b.OpenBit == 0 && b.UpsideDownBit == 1:
					return 0;
				case { } b when true && b.Direction == 1 && b.OpenBit == 0 && b.UpsideDownBit == 0:
					return 1;
				case { } b when true && b.Direction == 0 && b.OpenBit == 1 && b.UpsideDownBit == 1:
					return 2;
				case { } b when true && b.Direction == 0 && b.OpenBit == 0 && b.UpsideDownBit == 1:
					return 3;
				case { } b when true && b.Direction == 2 && b.OpenBit == 1 && b.UpsideDownBit == 1:
					return 4;
				case { } b when true && b.Direction == 0 && b.OpenBit == 1 && b.UpsideDownBit == 0:
					return 5;
				case { } b when true && b.Direction == 2 && b.OpenBit == 1 && b.UpsideDownBit == 0:
					return 6;
				case { } b when true && b.Direction == 3 && b.OpenBit == 1 && b.UpsideDownBit == 0:
					return 7;
				case { } b when true && b.Direction == 2 && b.OpenBit == 0 && b.UpsideDownBit == 0:
					return 8;
				case { } b when true && b.Direction == 3 && b.OpenBit == 0 && b.UpsideDownBit == 0:
					return 9;
				case { } b when true && b.Direction == 3 && b.OpenBit == 0 && b.UpsideDownBit == 1:
					return 10;
				case { } b when true && b.Direction == 1 && b.OpenBit == 1 && b.UpsideDownBit == 0:
					return 11;
				case { } b when true && b.Direction == 2 && b.OpenBit == 0 && b.UpsideDownBit == 1:
					return 12;
				case { } b when true && b.Direction == 0 && b.OpenBit == 0 && b.UpsideDownBit == 0:
					return 13;
				case { } b when true && b.Direction == 1 && b.OpenBit == 1 && b.UpsideDownBit == 1:
					return 14;
				case { } b when true && b.Direction == 3 && b.OpenBit == 1 && b.UpsideDownBit == 1:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class TrappedChest // 146 typeof=TrappedChest
	{
		[Range(0, 5)] public int FacingDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					FacingDirection = 2;
					break;
				case 1:
					FacingDirection = 5;
					break;
				case 2:
					FacingDirection = 1;
					break;
				case 3:
					FacingDirection = 3;
					break;
				case 4:
					FacingDirection = 0;
					break;
				case 5:
					FacingDirection = 4;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.FacingDirection == 2:
					return 0;
				case { } b when true && b.FacingDirection == 5:
					return 1;
				case { } b when true && b.FacingDirection == 1:
					return 2;
				case { } b when true && b.FacingDirection == 3:
					return 3;
				case { } b when true && b.FacingDirection == 0:
					return 4;
				case { } b when true && b.FacingDirection == 4:
					return 5;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class TripWire // 132 typeof=Tripwire
	{
		[Range(0, 1)] public byte AttachedBit { get; set; }
		[Range(0, 1)] public byte DisarmedBit { get; set; }
		[Range(0, 1)] public byte PoweredBit { get; set; }
		[Range(0, 1)] public byte SuspendedBit { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					AttachedBit = 0;
					DisarmedBit = 1;
					PoweredBit = 1;
					SuspendedBit = 1;
					break;
				case 1:
					AttachedBit = 0;
					DisarmedBit = 0;
					PoweredBit = 1;
					SuspendedBit = 1;
					break;
				case 2:
					AttachedBit = 1;
					DisarmedBit = 0;
					PoweredBit = 0;
					SuspendedBit = 1;
					break;
				case 3:
					AttachedBit = 0;
					DisarmedBit = 0;
					PoweredBit = 0;
					SuspendedBit = 1;
					break;
				case 4:
					AttachedBit = 0;
					DisarmedBit = 1;
					PoweredBit = 1;
					SuspendedBit = 0;
					break;
				case 5:
					AttachedBit = 1;
					DisarmedBit = 0;
					PoweredBit = 1;
					SuspendedBit = 0;
					break;
				case 6:
					AttachedBit = 0;
					DisarmedBit = 0;
					PoweredBit = 0;
					SuspendedBit = 0;
					break;
				case 7:
					AttachedBit = 1;
					DisarmedBit = 1;
					PoweredBit = 0;
					SuspendedBit = 0;
					break;
				case 8:
					AttachedBit = 1;
					DisarmedBit = 0;
					PoweredBit = 1;
					SuspendedBit = 1;
					break;
				case 9:
					AttachedBit = 0;
					DisarmedBit = 1;
					PoweredBit = 0;
					SuspendedBit = 1;
					break;
				case 10:
					AttachedBit = 1;
					DisarmedBit = 0;
					PoweredBit = 0;
					SuspendedBit = 0;
					break;
				case 11:
					AttachedBit = 0;
					DisarmedBit = 0;
					PoweredBit = 1;
					SuspendedBit = 0;
					break;
				case 12:
					AttachedBit = 1;
					DisarmedBit = 1;
					PoweredBit = 1;
					SuspendedBit = 0;
					break;
				case 13:
					AttachedBit = 1;
					DisarmedBit = 1;
					PoweredBit = 0;
					SuspendedBit = 1;
					break;
				case 14:
					AttachedBit = 0;
					DisarmedBit = 1;
					PoweredBit = 0;
					SuspendedBit = 0;
					break;
				case 15:
					AttachedBit = 1;
					DisarmedBit = 1;
					PoweredBit = 1;
					SuspendedBit = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.AttachedBit == 0 && b.DisarmedBit == 1 && b.PoweredBit == 1 && b.SuspendedBit == 1:
					return 0;
				case { } b when true && b.AttachedBit == 0 && b.DisarmedBit == 0 && b.PoweredBit == 1 && b.SuspendedBit == 1:
					return 1;
				case { } b when true && b.AttachedBit == 1 && b.DisarmedBit == 0 && b.PoweredBit == 0 && b.SuspendedBit == 1:
					return 2;
				case { } b when true && b.AttachedBit == 0 && b.DisarmedBit == 0 && b.PoweredBit == 0 && b.SuspendedBit == 1:
					return 3;
				case { } b when true && b.AttachedBit == 0 && b.DisarmedBit == 1 && b.PoweredBit == 1 && b.SuspendedBit == 0:
					return 4;
				case { } b when true && b.AttachedBit == 1 && b.DisarmedBit == 0 && b.PoweredBit == 1 && b.SuspendedBit == 0:
					return 5;
				case { } b when true && b.AttachedBit == 0 && b.DisarmedBit == 0 && b.PoweredBit == 0 && b.SuspendedBit == 0:
					return 6;
				case { } b when true && b.AttachedBit == 1 && b.DisarmedBit == 1 && b.PoweredBit == 0 && b.SuspendedBit == 0:
					return 7;
				case { } b when true && b.AttachedBit == 1 && b.DisarmedBit == 0 && b.PoweredBit == 1 && b.SuspendedBit == 1:
					return 8;
				case { } b when true && b.AttachedBit == 0 && b.DisarmedBit == 1 && b.PoweredBit == 0 && b.SuspendedBit == 1:
					return 9;
				case { } b when true && b.AttachedBit == 1 && b.DisarmedBit == 0 && b.PoweredBit == 0 && b.SuspendedBit == 0:
					return 10;
				case { } b when true && b.AttachedBit == 0 && b.DisarmedBit == 0 && b.PoweredBit == 1 && b.SuspendedBit == 0:
					return 11;
				case { } b when true && b.AttachedBit == 1 && b.DisarmedBit == 1 && b.PoweredBit == 1 && b.SuspendedBit == 0:
					return 12;
				case { } b when true && b.AttachedBit == 1 && b.DisarmedBit == 1 && b.PoweredBit == 0 && b.SuspendedBit == 1:
					return 13;
				case { } b when true && b.AttachedBit == 0 && b.DisarmedBit == 1 && b.PoweredBit == 0 && b.SuspendedBit == 0:
					return 14;
				case { } b when true && b.AttachedBit == 1 && b.DisarmedBit == 1 && b.PoweredBit == 1 && b.SuspendedBit == 1:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class TripwireHook // 131 typeof=TripwireHook
	{
		[Range(0, 1)] public byte AttachedBit { get; set; }
		[Range(0, 3)] public int Direction { get; set; }
		[Range(0, 1)] public byte PoweredBit { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					AttachedBit = 1;
					Direction = 2;
					PoweredBit = 0;
					break;
				case 1:
					AttachedBit = 1;
					Direction = 1;
					PoweredBit = 0;
					break;
				case 2:
					AttachedBit = 0;
					Direction = 1;
					PoweredBit = 0;
					break;
				case 3:
					AttachedBit = 0;
					Direction = 0;
					PoweredBit = 1;
					break;
				case 4:
					AttachedBit = 1;
					Direction = 1;
					PoweredBit = 1;
					break;
				case 5:
					AttachedBit = 0;
					Direction = 2;
					PoweredBit = 1;
					break;
				case 6:
					AttachedBit = 1;
					Direction = 3;
					PoweredBit = 1;
					break;
				case 7:
					AttachedBit = 1;
					Direction = 3;
					PoweredBit = 0;
					break;
				case 8:
					AttachedBit = 1;
					Direction = 2;
					PoweredBit = 1;
					break;
				case 9:
					AttachedBit = 0;
					Direction = 2;
					PoweredBit = 0;
					break;
				case 10:
					AttachedBit = 0;
					Direction = 3;
					PoweredBit = 0;
					break;
				case 11:
					AttachedBit = 1;
					Direction = 0;
					PoweredBit = 1;
					break;
				case 12:
					AttachedBit = 1;
					Direction = 0;
					PoweredBit = 0;
					break;
				case 13:
					AttachedBit = 0;
					Direction = 3;
					PoweredBit = 1;
					break;
				case 14:
					AttachedBit = 0;
					Direction = 0;
					PoweredBit = 0;
					break;
				case 15:
					AttachedBit = 0;
					Direction = 1;
					PoweredBit = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.AttachedBit == 1 && b.Direction == 2 && b.PoweredBit == 0:
					return 0;
				case { } b when true && b.AttachedBit == 1 && b.Direction == 1 && b.PoweredBit == 0:
					return 1;
				case { } b when true && b.AttachedBit == 0 && b.Direction == 1 && b.PoweredBit == 0:
					return 2;
				case { } b when true && b.AttachedBit == 0 && b.Direction == 0 && b.PoweredBit == 1:
					return 3;
				case { } b when true && b.AttachedBit == 1 && b.Direction == 1 && b.PoweredBit == 1:
					return 4;
				case { } b when true && b.AttachedBit == 0 && b.Direction == 2 && b.PoweredBit == 1:
					return 5;
				case { } b when true && b.AttachedBit == 1 && b.Direction == 3 && b.PoweredBit == 1:
					return 6;
				case { } b when true && b.AttachedBit == 1 && b.Direction == 3 && b.PoweredBit == 0:
					return 7;
				case { } b when true && b.AttachedBit == 1 && b.Direction == 2 && b.PoweredBit == 1:
					return 8;
				case { } b when true && b.AttachedBit == 0 && b.Direction == 2 && b.PoweredBit == 0:
					return 9;
				case { } b when true && b.AttachedBit == 0 && b.Direction == 3 && b.PoweredBit == 0:
					return 10;
				case { } b when true && b.AttachedBit == 1 && b.Direction == 0 && b.PoweredBit == 1:
					return 11;
				case { } b when true && b.AttachedBit == 1 && b.Direction == 0 && b.PoweredBit == 0:
					return 12;
				case { } b when true && b.AttachedBit == 0 && b.Direction == 3 && b.PoweredBit == 1:
					return 13;
				case { } b when true && b.AttachedBit == 0 && b.Direction == 0 && b.PoweredBit == 0:
					return 14;
				case { } b when true && b.AttachedBit == 0 && b.Direction == 1 && b.PoweredBit == 1:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class TurtleEgg : Block // 0 typeof=Block
	{
		// Convert this attribute to enum
		//[Enum("cracked","max_cracked","no_cracks"]
		public string CrackedState { get; set; }

		// Convert this attribute to enum
		//[Enum("four_egg","one_egg","three_egg","two_egg"]
		public string TurtleEggCount { get; set; }

		public TurtleEgg() : base(414)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					CrackedState = "no_cracks";
					TurtleEggCount = "three_egg";
					break;
				case 1:
					CrackedState = "cracked";
					TurtleEggCount = "three_egg";
					break;
				case 2:
					CrackedState = "no_cracks";
					TurtleEggCount = "four_egg";
					break;
				case 3:
					CrackedState = "max_cracked";
					TurtleEggCount = "one_egg";
					break;
				case 4:
					CrackedState = "max_cracked";
					TurtleEggCount = "four_egg";
					break;
				case 5:
					CrackedState = "max_cracked";
					TurtleEggCount = "two_egg";
					break;
				case 6:
					CrackedState = "cracked";
					TurtleEggCount = "two_egg";
					break;
				case 7:
					CrackedState = "cracked";
					TurtleEggCount = "one_egg";
					break;
				case 8:
					CrackedState = "cracked";
					TurtleEggCount = "four_egg";
					break;
				case 9:
					CrackedState = "max_cracked";
					TurtleEggCount = "three_egg";
					break;
				case 10:
					CrackedState = "no_cracks";
					TurtleEggCount = "two_egg";
					break;
				case 11:
					CrackedState = "no_cracks";
					TurtleEggCount = "one_egg";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.CrackedState == "no_cracks" && b.TurtleEggCount == "three_egg":
					return 0;
				case { } b when true && b.CrackedState == "cracked" && b.TurtleEggCount == "three_egg":
					return 1;
				case { } b when true && b.CrackedState == "no_cracks" && b.TurtleEggCount == "four_egg":
					return 2;
				case { } b when true && b.CrackedState == "max_cracked" && b.TurtleEggCount == "one_egg":
					return 3;
				case { } b when true && b.CrackedState == "max_cracked" && b.TurtleEggCount == "four_egg":
					return 4;
				case { } b when true && b.CrackedState == "max_cracked" && b.TurtleEggCount == "two_egg":
					return 5;
				case { } b when true && b.CrackedState == "cracked" && b.TurtleEggCount == "two_egg":
					return 6;
				case { } b when true && b.CrackedState == "cracked" && b.TurtleEggCount == "one_egg":
					return 7;
				case { } b when true && b.CrackedState == "cracked" && b.TurtleEggCount == "four_egg":
					return 8;
				case { } b when true && b.CrackedState == "max_cracked" && b.TurtleEggCount == "three_egg":
					return 9;
				case { } b when true && b.CrackedState == "no_cracks" && b.TurtleEggCount == "two_egg":
					return 10;
				case { } b when true && b.CrackedState == "no_cracks" && b.TurtleEggCount == "one_egg":
					return 11;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class UnderwaterTorch : Block // 0 typeof=Block
	{
		// Convert this attribute to enum
		//[Enum("east","north","south","top","unknown","west"]
		public string TorchFacingDirection { get; set; }

		public UnderwaterTorch() : base(239)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					TorchFacingDirection = "north";
					break;
				case 1:
					TorchFacingDirection = "south";
					break;
				case 2:
					TorchFacingDirection = "top";
					break;
				case 3:
					TorchFacingDirection = "west";
					break;
				case 4:
					TorchFacingDirection = "unknown";
					break;
				case 5:
					TorchFacingDirection = "east";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.TorchFacingDirection == "north":
					return 0;
				case { } b when true && b.TorchFacingDirection == "south":
					return 1;
				case { } b when true && b.TorchFacingDirection == "top":
					return 2;
				case { } b when true && b.TorchFacingDirection == "west":
					return 3;
				case { } b when true && b.TorchFacingDirection == "unknown":
					return 4;
				case { } b when true && b.TorchFacingDirection == "east":
					return 5;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class UndyedShulkerBox // 205 typeof=UndyedShulkerBox
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class UnlitRedstoneTorch // 75 typeof=UnlitRedstoneTorch
	{
		// Convert this attribute to enum
		//[Enum("east","north","south","top","unknown","west"]
		public string TorchFacingDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					TorchFacingDirection = "west";
					break;
				case 1:
					TorchFacingDirection = "south";
					break;
				case 2:
					TorchFacingDirection = "unknown";
					break;
				case 3:
					TorchFacingDirection = "east";
					break;
				case 4:
					TorchFacingDirection = "top";
					break;
				case 5:
					TorchFacingDirection = "north";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.TorchFacingDirection == "west":
					return 0;
				case { } b when true && b.TorchFacingDirection == "south":
					return 1;
				case { } b when true && b.TorchFacingDirection == "unknown":
					return 2;
				case { } b when true && b.TorchFacingDirection == "east":
					return 3;
				case { } b when true && b.TorchFacingDirection == "top":
					return 4;
				case { } b when true && b.TorchFacingDirection == "north":
					return 5;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class UnpoweredComparator // 149 typeof=UnpoweredComparator
	{
		[Range(0, 3)] public int Direction { get; set; }
		[Range(0, 1)] public byte OutputLitBit { get; set; }
		[Range(0, 1)] public byte OutputSubtractBit { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Direction = 2;
					OutputLitBit = 0;
					OutputSubtractBit = 1;
					break;
				case 1:
					Direction = 3;
					OutputLitBit = 1;
					OutputSubtractBit = 1;
					break;
				case 2:
					Direction = 0;
					OutputLitBit = 0;
					OutputSubtractBit = 0;
					break;
				case 3:
					Direction = 0;
					OutputLitBit = 1;
					OutputSubtractBit = 1;
					break;
				case 4:
					Direction = 1;
					OutputLitBit = 1;
					OutputSubtractBit = 1;
					break;
				case 5:
					Direction = 3;
					OutputLitBit = 0;
					OutputSubtractBit = 1;
					break;
				case 6:
					Direction = 2;
					OutputLitBit = 1;
					OutputSubtractBit = 0;
					break;
				case 7:
					Direction = 3;
					OutputLitBit = 0;
					OutputSubtractBit = 0;
					break;
				case 8:
					Direction = 1;
					OutputLitBit = 1;
					OutputSubtractBit = 0;
					break;
				case 9:
					Direction = 0;
					OutputLitBit = 1;
					OutputSubtractBit = 0;
					break;
				case 10:
					Direction = 1;
					OutputLitBit = 0;
					OutputSubtractBit = 1;
					break;
				case 11:
					Direction = 2;
					OutputLitBit = 1;
					OutputSubtractBit = 1;
					break;
				case 12:
					Direction = 2;
					OutputLitBit = 0;
					OutputSubtractBit = 0;
					break;
				case 13:
					Direction = 3;
					OutputLitBit = 1;
					OutputSubtractBit = 0;
					break;
				case 14:
					Direction = 1;
					OutputLitBit = 0;
					OutputSubtractBit = 0;
					break;
				case 15:
					Direction = 0;
					OutputLitBit = 0;
					OutputSubtractBit = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Direction == 2 && b.OutputLitBit == 0 && b.OutputSubtractBit == 1:
					return 0;
				case { } b when true && b.Direction == 3 && b.OutputLitBit == 1 && b.OutputSubtractBit == 1:
					return 1;
				case { } b when true && b.Direction == 0 && b.OutputLitBit == 0 && b.OutputSubtractBit == 0:
					return 2;
				case { } b when true && b.Direction == 0 && b.OutputLitBit == 1 && b.OutputSubtractBit == 1:
					return 3;
				case { } b when true && b.Direction == 1 && b.OutputLitBit == 1 && b.OutputSubtractBit == 1:
					return 4;
				case { } b when true && b.Direction == 3 && b.OutputLitBit == 0 && b.OutputSubtractBit == 1:
					return 5;
				case { } b when true && b.Direction == 2 && b.OutputLitBit == 1 && b.OutputSubtractBit == 0:
					return 6;
				case { } b when true && b.Direction == 3 && b.OutputLitBit == 0 && b.OutputSubtractBit == 0:
					return 7;
				case { } b when true && b.Direction == 1 && b.OutputLitBit == 1 && b.OutputSubtractBit == 0:
					return 8;
				case { } b when true && b.Direction == 0 && b.OutputLitBit == 1 && b.OutputSubtractBit == 0:
					return 9;
				case { } b when true && b.Direction == 1 && b.OutputLitBit == 0 && b.OutputSubtractBit == 1:
					return 10;
				case { } b when true && b.Direction == 2 && b.OutputLitBit == 1 && b.OutputSubtractBit == 1:
					return 11;
				case { } b when true && b.Direction == 2 && b.OutputLitBit == 0 && b.OutputSubtractBit == 0:
					return 12;
				case { } b when true && b.Direction == 3 && b.OutputLitBit == 1 && b.OutputSubtractBit == 0:
					return 13;
				case { } b when true && b.Direction == 1 && b.OutputLitBit == 0 && b.OutputSubtractBit == 0:
					return 14;
				case { } b when true && b.Direction == 0 && b.OutputLitBit == 0 && b.OutputSubtractBit == 1:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class UnpoweredRepeater // 93 typeof=UnpoweredRepeater
	{
		[Range(0, 3)] public int Direction { get; set; }
		[Range(0, 3)] public int RepeaterDelay { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Direction = 0;
					RepeaterDelay = 3;
					break;
				case 1:
					Direction = 0;
					RepeaterDelay = 2;
					break;
				case 2:
					Direction = 3;
					RepeaterDelay = 2;
					break;
				case 3:
					Direction = 3;
					RepeaterDelay = 1;
					break;
				case 4:
					Direction = 3;
					RepeaterDelay = 3;
					break;
				case 5:
					Direction = 3;
					RepeaterDelay = 0;
					break;
				case 6:
					Direction = 2;
					RepeaterDelay = 2;
					break;
				case 7:
					Direction = 2;
					RepeaterDelay = 1;
					break;
				case 8:
					Direction = 2;
					RepeaterDelay = 0;
					break;
				case 9:
					Direction = 2;
					RepeaterDelay = 3;
					break;
				case 10:
					Direction = 0;
					RepeaterDelay = 0;
					break;
				case 11:
					Direction = 1;
					RepeaterDelay = 3;
					break;
				case 12:
					Direction = 0;
					RepeaterDelay = 1;
					break;
				case 13:
					Direction = 1;
					RepeaterDelay = 2;
					break;
				case 14:
					Direction = 1;
					RepeaterDelay = 0;
					break;
				case 15:
					Direction = 1;
					RepeaterDelay = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Direction == 0 && b.RepeaterDelay == 3:
					return 0;
				case { } b when true && b.Direction == 0 && b.RepeaterDelay == 2:
					return 1;
				case { } b when true && b.Direction == 3 && b.RepeaterDelay == 2:
					return 2;
				case { } b when true && b.Direction == 3 && b.RepeaterDelay == 1:
					return 3;
				case { } b when true && b.Direction == 3 && b.RepeaterDelay == 3:
					return 4;
				case { } b when true && b.Direction == 3 && b.RepeaterDelay == 0:
					return 5;
				case { } b when true && b.Direction == 2 && b.RepeaterDelay == 2:
					return 6;
				case { } b when true && b.Direction == 2 && b.RepeaterDelay == 1:
					return 7;
				case { } b when true && b.Direction == 2 && b.RepeaterDelay == 0:
					return 8;
				case { } b when true && b.Direction == 2 && b.RepeaterDelay == 3:
					return 9;
				case { } b when true && b.Direction == 0 && b.RepeaterDelay == 0:
					return 10;
				case { } b when true && b.Direction == 1 && b.RepeaterDelay == 3:
					return 11;
				case { } b when true && b.Direction == 0 && b.RepeaterDelay == 1:
					return 12;
				case { } b when true && b.Direction == 1 && b.RepeaterDelay == 2:
					return 13;
				case { } b when true && b.Direction == 1 && b.RepeaterDelay == 0:
					return 14;
				case { } b when true && b.Direction == 1 && b.RepeaterDelay == 1:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class WallBanner // 177 typeof=WallBanner
	{
		[Range(0, 5)] public int FacingDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					FacingDirection = 3;
					break;
				case 1:
					FacingDirection = 4;
					break;
				case 2:
					FacingDirection = 0;
					break;
				case 3:
					FacingDirection = 2;
					break;
				case 4:
					FacingDirection = 1;
					break;
				case 5:
					FacingDirection = 5;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.FacingDirection == 3:
					return 0;
				case { } b when true && b.FacingDirection == 4:
					return 1;
				case { } b when true && b.FacingDirection == 0:
					return 2;
				case { } b when true && b.FacingDirection == 2:
					return 3;
				case { } b when true && b.FacingDirection == 1:
					return 4;
				case { } b when true && b.FacingDirection == 5:
					return 5;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class WallSign // 68 typeof=WallSign
	{
		[Range(0, 5)] public int FacingDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					FacingDirection = 2;
					break;
				case 1:
					FacingDirection = 0;
					break;
				case 2:
					FacingDirection = 1;
					break;
				case 3:
					FacingDirection = 4;
					break;
				case 4:
					FacingDirection = 3;
					break;
				case 5:
					FacingDirection = 5;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.FacingDirection == 2:
					return 0;
				case { } b when true && b.FacingDirection == 0:
					return 1;
				case { } b when true && b.FacingDirection == 1:
					return 2;
				case { } b when true && b.FacingDirection == 4:
					return 3;
				case { } b when true && b.FacingDirection == 3:
					return 4;
				case { } b when true && b.FacingDirection == 5:
					return 5;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Water // 9 typeof=Water
	{
		[Range(0, 9)] public int LiquidDepth { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					LiquidDepth = 0;
					break;
				case 1:
					LiquidDepth = 12;
					break;
				case 2:
					LiquidDepth = 7;
					break;
				case 3:
					LiquidDepth = 2;
					break;
				case 4:
					LiquidDepth = 15;
					break;
				case 5:
					LiquidDepth = 4;
					break;
				case 6:
					LiquidDepth = 1;
					break;
				case 7:
					LiquidDepth = 6;
					break;
				case 8:
					LiquidDepth = 10;
					break;
				case 9:
					LiquidDepth = 13;
					break;
				case 10:
					LiquidDepth = 11;
					break;
				case 11:
					LiquidDepth = 5;
					break;
				case 12:
					LiquidDepth = 9;
					break;
				case 13:
					LiquidDepth = 3;
					break;
				case 14:
					LiquidDepth = 8;
					break;
				case 15:
					LiquidDepth = 14;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.LiquidDepth == 0:
					return 0;
				case { } b when true && b.LiquidDepth == 12:
					return 1;
				case { } b when true && b.LiquidDepth == 7:
					return 2;
				case { } b when true && b.LiquidDepth == 2:
					return 3;
				case { } b when true && b.LiquidDepth == 15:
					return 4;
				case { } b when true && b.LiquidDepth == 4:
					return 5;
				case { } b when true && b.LiquidDepth == 1:
					return 6;
				case { } b when true && b.LiquidDepth == 6:
					return 7;
				case { } b when true && b.LiquidDepth == 10:
					return 8;
				case { } b when true && b.LiquidDepth == 13:
					return 9;
				case { } b when true && b.LiquidDepth == 11:
					return 10;
				case { } b when true && b.LiquidDepth == 5:
					return 11;
				case { } b when true && b.LiquidDepth == 9:
					return 12;
				case { } b when true && b.LiquidDepth == 3:
					return 13;
				case { } b when true && b.LiquidDepth == 8:
					return 14;
				case { } b when true && b.LiquidDepth == 14:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Waterlily // 111 typeof=Waterlily
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Web // 30 typeof=Web
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Wheat // 59 typeof=Wheat
	{
		[Range(0, 7)] public int Growth { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Growth = 7;
					break;
				case 1:
					Growth = 5;
					break;
				case 2:
					Growth = 3;
					break;
				case 3:
					Growth = 1;
					break;
				case 4:
					Growth = 2;
					break;
				case 5:
					Growth = 0;
					break;
				case 6:
					Growth = 4;
					break;
				case 7:
					Growth = 6;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Growth == 7:
					return 0;
				case { } b when true && b.Growth == 5:
					return 1;
				case { } b when true && b.Growth == 3:
					return 2;
				case { } b when true && b.Growth == 1:
					return 3;
				case { } b when true && b.Growth == 2:
					return 4;
				case { } b when true && b.Growth == 0:
					return 5;
				case { } b when true && b.Growth == 4:
					return 6;
				case { } b when true && b.Growth == 6:
					return 7;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class WhiteGlazedTerracotta // 220 typeof=WhiteGlazedTerracotta
	{
		[Range(0, 5)] public int FacingDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					FacingDirection = 5;
					break;
				case 1:
					FacingDirection = 2;
					break;
				case 2:
					FacingDirection = 0;
					break;
				case 3:
					FacingDirection = 3;
					break;
				case 4:
					FacingDirection = 4;
					break;
				case 5:
					FacingDirection = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.FacingDirection == 5:
					return 0;
				case { } b when true && b.FacingDirection == 2:
					return 1;
				case { } b when true && b.FacingDirection == 0:
					return 2;
				case { } b when true && b.FacingDirection == 3:
					return 3;
				case { } b when true && b.FacingDirection == 4:
					return 4;
				case { } b when true && b.FacingDirection == 1:
					return 5;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Vine // 106 typeof=Vine
	{
		[Range(0, 9)] public int VineDirectionBits { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					VineDirectionBits = 13;
					break;
				case 1:
					VineDirectionBits = 14;
					break;
				case 2:
					VineDirectionBits = 15;
					break;
				case 3:
					VineDirectionBits = 10;
					break;
				case 4:
					VineDirectionBits = 12;
					break;
				case 5:
					VineDirectionBits = 5;
					break;
				case 6:
					VineDirectionBits = 2;
					break;
				case 7:
					VineDirectionBits = 3;
					break;
				case 8:
					VineDirectionBits = 9;
					break;
				case 9:
					VineDirectionBits = 6;
					break;
				case 10:
					VineDirectionBits = 8;
					break;
				case 11:
					VineDirectionBits = 1;
					break;
				case 12:
					VineDirectionBits = 0;
					break;
				case 13:
					VineDirectionBits = 7;
					break;
				case 14:
					VineDirectionBits = 4;
					break;
				case 15:
					VineDirectionBits = 11;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.VineDirectionBits == 13:
					return 0;
				case { } b when true && b.VineDirectionBits == 14:
					return 1;
				case { } b when true && b.VineDirectionBits == 15:
					return 2;
				case { } b when true && b.VineDirectionBits == 10:
					return 3;
				case { } b when true && b.VineDirectionBits == 12:
					return 4;
				case { } b when true && b.VineDirectionBits == 5:
					return 5;
				case { } b when true && b.VineDirectionBits == 2:
					return 6;
				case { } b when true && b.VineDirectionBits == 3:
					return 7;
				case { } b when true && b.VineDirectionBits == 9:
					return 8;
				case { } b when true && b.VineDirectionBits == 6:
					return 9;
				case { } b when true && b.VineDirectionBits == 8:
					return 10;
				case { } b when true && b.VineDirectionBits == 1:
					return 11;
				case { } b when true && b.VineDirectionBits == 0:
					return 12;
				case { } b when true && b.VineDirectionBits == 7:
					return 13;
				case { } b when true && b.VineDirectionBits == 4:
					return 14;
				case { } b when true && b.VineDirectionBits == 11:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class WitherRose : Block // 0 typeof=Block
	{
		public WitherRose() : base(471)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Wood : Block // 0 typeof=Block
	{
		// Convert this attribute to enum
		//[Enum("x","y","z"]
		public string PillarAxis { get; set; }

		[Range(0, 1)] public byte StrippedBit { get; set; }

		// Convert this attribute to enum
		//[Enum("acacia","birch","dark_oak","jungle","oak","spruce"]
		public string WoodType { get; set; }

		public Wood() : base(467)
		{
		}

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					PillarAxis = "z";
					StrippedBit = 1;
					WoodType = "birch";
					break;
				case 1:
					PillarAxis = "x";
					StrippedBit = 1;
					WoodType = "birch";
					break;
				case 2:
					PillarAxis = "z";
					StrippedBit = 0;
					WoodType = "acacia";
					break;
				case 3:
					PillarAxis = "z";
					StrippedBit = 0;
					WoodType = "oak";
					break;
				case 4:
					PillarAxis = "y";
					StrippedBit = 1;
					WoodType = "birch";
					break;
				case 5:
					PillarAxis = "z";
					StrippedBit = 1;
					WoodType = "spruce";
					break;
				case 6:
					PillarAxis = "z";
					StrippedBit = 1;
					WoodType = "dark_oak";
					break;
				case 7:
					PillarAxis = "y";
					StrippedBit = 1;
					WoodType = "dark_oak";
					break;
				case 8:
					PillarAxis = "x";
					StrippedBit = 1;
					WoodType = "spruce";
					break;
				case 9:
					PillarAxis = "x";
					StrippedBit = 0;
					WoodType = "jungle";
					break;
				case 10:
					PillarAxis = "y";
					StrippedBit = 1;
					WoodType = "acacia";
					break;
				case 11:
					PillarAxis = "y";
					StrippedBit = 0;
					WoodType = "dark_oak";
					break;
				case 12:
					PillarAxis = "y";
					StrippedBit = 0;
					WoodType = "oak";
					break;
				case 13:
					PillarAxis = "y";
					StrippedBit = 0;
					WoodType = "spruce";
					break;
				case 14:
					PillarAxis = "z";
					StrippedBit = 1;
					WoodType = "acacia";
					break;
				case 15:
					PillarAxis = "x";
					StrippedBit = 1;
					WoodType = "oak";
					break;
				case 16:
					PillarAxis = "z";
					StrippedBit = 0;
					WoodType = "spruce";
					break;
				case 17:
					PillarAxis = "x";
					StrippedBit = 0;
					WoodType = "dark_oak";
					break;
				case 18:
					PillarAxis = "y";
					StrippedBit = 1;
					WoodType = "spruce";
					break;
				case 19:
					PillarAxis = "z";
					StrippedBit = 0;
					WoodType = "birch";
					break;
				case 20:
					PillarAxis = "y";
					StrippedBit = 1;
					WoodType = "oak";
					break;
				case 21:
					PillarAxis = "z";
					StrippedBit = 1;
					WoodType = "jungle";
					break;
				case 22:
					PillarAxis = "x";
					StrippedBit = 0;
					WoodType = "birch";
					break;
				case 23:
					PillarAxis = "x";
					StrippedBit = 1;
					WoodType = "acacia";
					break;
				case 24:
					PillarAxis = "x";
					StrippedBit = 0;
					WoodType = "oak";
					break;
				case 25:
					PillarAxis = "z";
					StrippedBit = 0;
					WoodType = "dark_oak";
					break;
				case 26:
					PillarAxis = "y";
					StrippedBit = 0;
					WoodType = "jungle";
					break;
				case 27:
					PillarAxis = "x";
					StrippedBit = 1;
					WoodType = "jungle";
					break;
				case 28:
					PillarAxis = "x";
					StrippedBit = 1;
					WoodType = "dark_oak";
					break;
				case 29:
					PillarAxis = "x";
					StrippedBit = 0;
					WoodType = "spruce";
					break;
				case 30:
					PillarAxis = "x";
					StrippedBit = 0;
					WoodType = "acacia";
					break;
				case 31:
					PillarAxis = "z";
					StrippedBit = 1;
					WoodType = "oak";
					break;
				case 32:
					PillarAxis = "y";
					StrippedBit = 0;
					WoodType = "birch";
					break;
				case 33:
					PillarAxis = "z";
					StrippedBit = 0;
					WoodType = "jungle";
					break;
				case 34:
					PillarAxis = "y";
					StrippedBit = 1;
					WoodType = "jungle";
					break;
				case 35:
					PillarAxis = "y";
					StrippedBit = 0;
					WoodType = "acacia";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.PillarAxis == "z" && b.StrippedBit == 1 && b.WoodType == "birch":
					return 0;
				case { } b when true && b.PillarAxis == "x" && b.StrippedBit == 1 && b.WoodType == "birch":
					return 1;
				case { } b when true && b.PillarAxis == "z" && b.StrippedBit == 0 && b.WoodType == "acacia":
					return 2;
				case { } b when true && b.PillarAxis == "z" && b.StrippedBit == 0 && b.WoodType == "oak":
					return 3;
				case { } b when true && b.PillarAxis == "y" && b.StrippedBit == 1 && b.WoodType == "birch":
					return 4;
				case { } b when true && b.PillarAxis == "z" && b.StrippedBit == 1 && b.WoodType == "spruce":
					return 5;
				case { } b when true && b.PillarAxis == "z" && b.StrippedBit == 1 && b.WoodType == "dark_oak":
					return 6;
				case { } b when true && b.PillarAxis == "y" && b.StrippedBit == 1 && b.WoodType == "dark_oak":
					return 7;
				case { } b when true && b.PillarAxis == "x" && b.StrippedBit == 1 && b.WoodType == "spruce":
					return 8;
				case { } b when true && b.PillarAxis == "x" && b.StrippedBit == 0 && b.WoodType == "jungle":
					return 9;
				case { } b when true && b.PillarAxis == "y" && b.StrippedBit == 1 && b.WoodType == "acacia":
					return 10;
				case { } b when true && b.PillarAxis == "y" && b.StrippedBit == 0 && b.WoodType == "dark_oak":
					return 11;
				case { } b when true && b.PillarAxis == "y" && b.StrippedBit == 0 && b.WoodType == "oak":
					return 12;
				case { } b when true && b.PillarAxis == "y" && b.StrippedBit == 0 && b.WoodType == "spruce":
					return 13;
				case { } b when true && b.PillarAxis == "z" && b.StrippedBit == 1 && b.WoodType == "acacia":
					return 14;
				case { } b when true && b.PillarAxis == "x" && b.StrippedBit == 1 && b.WoodType == "oak":
					return 15;
				case { } b when true && b.PillarAxis == "z" && b.StrippedBit == 0 && b.WoodType == "spruce":
					return 16;
				case { } b when true && b.PillarAxis == "x" && b.StrippedBit == 0 && b.WoodType == "dark_oak":
					return 17;
				case { } b when true && b.PillarAxis == "y" && b.StrippedBit == 1 && b.WoodType == "spruce":
					return 18;
				case { } b when true && b.PillarAxis == "z" && b.StrippedBit == 0 && b.WoodType == "birch":
					return 19;
				case { } b when true && b.PillarAxis == "y" && b.StrippedBit == 1 && b.WoodType == "oak":
					return 20;
				case { } b when true && b.PillarAxis == "z" && b.StrippedBit == 1 && b.WoodType == "jungle":
					return 21;
				case { } b when true && b.PillarAxis == "x" && b.StrippedBit == 0 && b.WoodType == "birch":
					return 22;
				case { } b when true && b.PillarAxis == "x" && b.StrippedBit == 1 && b.WoodType == "acacia":
					return 23;
				case { } b when true && b.PillarAxis == "x" && b.StrippedBit == 0 && b.WoodType == "oak":
					return 24;
				case { } b when true && b.PillarAxis == "z" && b.StrippedBit == 0 && b.WoodType == "dark_oak":
					return 25;
				case { } b when true && b.PillarAxis == "y" && b.StrippedBit == 0 && b.WoodType == "jungle":
					return 26;
				case { } b when true && b.PillarAxis == "x" && b.StrippedBit == 1 && b.WoodType == "jungle":
					return 27;
				case { } b when true && b.PillarAxis == "x" && b.StrippedBit == 1 && b.WoodType == "dark_oak":
					return 28;
				case { } b when true && b.PillarAxis == "x" && b.StrippedBit == 0 && b.WoodType == "spruce":
					return 29;
				case { } b when true && b.PillarAxis == "x" && b.StrippedBit == 0 && b.WoodType == "acacia":
					return 30;
				case { } b when true && b.PillarAxis == "z" && b.StrippedBit == 1 && b.WoodType == "oak":
					return 31;
				case { } b when true && b.PillarAxis == "y" && b.StrippedBit == 0 && b.WoodType == "birch":
					return 32;
				case { } b when true && b.PillarAxis == "z" && b.StrippedBit == 0 && b.WoodType == "jungle":
					return 33;
				case { } b when true && b.PillarAxis == "y" && b.StrippedBit == 1 && b.WoodType == "jungle":
					return 34;
				case { } b when true && b.PillarAxis == "y" && b.StrippedBit == 0 && b.WoodType == "acacia":
					return 35;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class WoodenButton // 143 typeof=WoodenButton
	{
		[Range(0, 1)] public byte ButtonPressedBit { get; set; }
		[Range(0, 5)] public int FacingDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					ButtonPressedBit = 1;
					FacingDirection = 3;
					break;
				case 1:
					ButtonPressedBit = 1;
					FacingDirection = 4;
					break;
				case 2:
					ButtonPressedBit = 0;
					FacingDirection = 4;
					break;
				case 3:
					ButtonPressedBit = 1;
					FacingDirection = 1;
					break;
				case 4:
					ButtonPressedBit = 0;
					FacingDirection = 3;
					break;
				case 5:
					ButtonPressedBit = 1;
					FacingDirection = 5;
					break;
				case 6:
					ButtonPressedBit = 1;
					FacingDirection = 0;
					break;
				case 7:
					ButtonPressedBit = 1;
					FacingDirection = 2;
					break;
				case 8:
					ButtonPressedBit = 0;
					FacingDirection = 0;
					break;
				case 9:
					ButtonPressedBit = 0;
					FacingDirection = 5;
					break;
				case 10:
					ButtonPressedBit = 0;
					FacingDirection = 2;
					break;
				case 11:
					ButtonPressedBit = 0;
					FacingDirection = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.ButtonPressedBit == 1 && b.FacingDirection == 3:
					return 0;
				case { } b when true && b.ButtonPressedBit == 1 && b.FacingDirection == 4:
					return 1;
				case { } b when true && b.ButtonPressedBit == 0 && b.FacingDirection == 4:
					return 2;
				case { } b when true && b.ButtonPressedBit == 1 && b.FacingDirection == 1:
					return 3;
				case { } b when true && b.ButtonPressedBit == 0 && b.FacingDirection == 3:
					return 4;
				case { } b when true && b.ButtonPressedBit == 1 && b.FacingDirection == 5:
					return 5;
				case { } b when true && b.ButtonPressedBit == 1 && b.FacingDirection == 0:
					return 6;
				case { } b when true && b.ButtonPressedBit == 1 && b.FacingDirection == 2:
					return 7;
				case { } b when true && b.ButtonPressedBit == 0 && b.FacingDirection == 0:
					return 8;
				case { } b when true && b.ButtonPressedBit == 0 && b.FacingDirection == 5:
					return 9;
				case { } b when true && b.ButtonPressedBit == 0 && b.FacingDirection == 2:
					return 10;
				case { } b when true && b.ButtonPressedBit == 0 && b.FacingDirection == 1:
					return 11;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class WoodenDoor // 64 typeof=WoodenDoor
	{
		[Range(0, 3)] public int Direction { get; set; }
		[Range(0, 1)] public byte DoorHingeBit { get; set; }
		[Range(0, 1)] public byte OpenBit { get; set; }
		[Range(0, 1)] public byte UpperBlockBit { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Direction = 0;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 1:
					Direction = 0;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 2:
					Direction = 1;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 3:
					Direction = 2;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 4:
					Direction = 1;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 5:
					Direction = 2;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 6:
					Direction = 0;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 7:
					Direction = 3;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 8:
					Direction = 0;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 9:
					Direction = 0;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 10:
					Direction = 2;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 11:
					Direction = 3;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 12:
					Direction = 1;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 13:
					Direction = 2;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 14:
					Direction = 2;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 15:
					Direction = 1;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 16:
					Direction = 0;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 17:
					Direction = 3;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 18:
					Direction = 3;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 19:
					Direction = 3;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 20:
					Direction = 2;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 21:
					Direction = 1;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 22:
					Direction = 3;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 23:
					Direction = 1;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 24:
					Direction = 1;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 25:
					Direction = 2;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 26:
					Direction = 2;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 27:
					Direction = 0;
					DoorHingeBit = 0;
					OpenBit = 0;
					UpperBlockBit = 0;
					break;
				case 28:
					Direction = 0;
					DoorHingeBit = 1;
					OpenBit = 1;
					UpperBlockBit = 1;
					break;
				case 29:
					Direction = 3;
					DoorHingeBit = 1;
					OpenBit = 0;
					UpperBlockBit = 1;
					break;
				case 30:
					Direction = 1;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
				case 31:
					Direction = 3;
					DoorHingeBit = 0;
					OpenBit = 1;
					UpperBlockBit = 0;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 0;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 1;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 2;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 3;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 4;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 5;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 6;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 7;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 8;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 9;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 10;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 11;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 12;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 13;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 14;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 15;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 16;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 17;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 18;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 19;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 20;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 21;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 22;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 23;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 24;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 25;
				case { } b when true && b.Direction == 2 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 26;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 0 && b.OpenBit == 0 && b.UpperBlockBit == 0:
					return 27;
				case { } b when true && b.Direction == 0 && b.DoorHingeBit == 1 && b.OpenBit == 1 && b.UpperBlockBit == 1:
					return 28;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 1 && b.OpenBit == 0 && b.UpperBlockBit == 1:
					return 29;
				case { } b when true && b.Direction == 1 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 30;
				case { } b when true && b.Direction == 3 && b.DoorHingeBit == 0 && b.OpenBit == 1 && b.UpperBlockBit == 0:
					return 31;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class WoodenPressurePlate // 72 typeof=WoodenPressurePlate
	{
		[Range(0, 9)] public int RedstoneSignal { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					RedstoneSignal = 10;
					break;
				case 1:
					RedstoneSignal = 7;
					break;
				case 2:
					RedstoneSignal = 11;
					break;
				case 3:
					RedstoneSignal = 2;
					break;
				case 4:
					RedstoneSignal = 0;
					break;
				case 5:
					RedstoneSignal = 9;
					break;
				case 6:
					RedstoneSignal = 15;
					break;
				case 7:
					RedstoneSignal = 14;
					break;
				case 8:
					RedstoneSignal = 6;
					break;
				case 9:
					RedstoneSignal = 5;
					break;
				case 10:
					RedstoneSignal = 13;
					break;
				case 11:
					RedstoneSignal = 3;
					break;
				case 12:
					RedstoneSignal = 8;
					break;
				case 13:
					RedstoneSignal = 4;
					break;
				case 14:
					RedstoneSignal = 12;
					break;
				case 15:
					RedstoneSignal = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.RedstoneSignal == 10:
					return 0;
				case { } b when true && b.RedstoneSignal == 7:
					return 1;
				case { } b when true && b.RedstoneSignal == 11:
					return 2;
				case { } b when true && b.RedstoneSignal == 2:
					return 3;
				case { } b when true && b.RedstoneSignal == 0:
					return 4;
				case { } b when true && b.RedstoneSignal == 9:
					return 5;
				case { } b when true && b.RedstoneSignal == 15:
					return 6;
				case { } b when true && b.RedstoneSignal == 14:
					return 7;
				case { } b when true && b.RedstoneSignal == 6:
					return 8;
				case { } b when true && b.RedstoneSignal == 5:
					return 9;
				case { } b when true && b.RedstoneSignal == 13:
					return 10;
				case { } b when true && b.RedstoneSignal == 3:
					return 11;
				case { } b when true && b.RedstoneSignal == 8:
					return 12;
				case { } b when true && b.RedstoneSignal == 4:
					return 13;
				case { } b when true && b.RedstoneSignal == 12:
					return 14;
				case { } b when true && b.RedstoneSignal == 1:
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class WoodenSlab // 158 typeof=WoodenSlab
	{
		[Range(0, 1)] public byte TopSlotBit { get; set; }

		// Convert this attribute to enum
		//[Enum("acacia","birch","dark_oak","jungle","oak","spruce"]
		public string WoodType { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					TopSlotBit = 1;
					WoodType = "dark_oak";
					break;
				case 1:
					TopSlotBit = 0;
					WoodType = "dark_oak";
					break;
				case 2:
					TopSlotBit = 1;
					WoodType = "spruce";
					break;
				case 3:
					TopSlotBit = 0;
					WoodType = "birch";
					break;
				case 4:
					TopSlotBit = 0;
					WoodType = "jungle";
					break;
				case 5:
					TopSlotBit = 0;
					WoodType = "oak";
					break;
				case 6:
					TopSlotBit = 1;
					WoodType = "jungle";
					break;
				case 7:
					TopSlotBit = 0;
					WoodType = "acacia";
					break;
				case 8:
					TopSlotBit = 1;
					WoodType = "birch";
					break;
				case 9:
					TopSlotBit = 1;
					WoodType = "acacia";
					break;
				case 10:
					TopSlotBit = 1;
					WoodType = "oak";
					break;
				case 11:
					TopSlotBit = 0;
					WoodType = "spruce";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.TopSlotBit == 1 && b.WoodType == "dark_oak":
					return 0;
				case { } b when true && b.TopSlotBit == 0 && b.WoodType == "dark_oak":
					return 1;
				case { } b when true && b.TopSlotBit == 1 && b.WoodType == "spruce":
					return 2;
				case { } b when true && b.TopSlotBit == 0 && b.WoodType == "birch":
					return 3;
				case { } b when true && b.TopSlotBit == 0 && b.WoodType == "jungle":
					return 4;
				case { } b when true && b.TopSlotBit == 0 && b.WoodType == "oak":
					return 5;
				case { } b when true && b.TopSlotBit == 1 && b.WoodType == "jungle":
					return 6;
				case { } b when true && b.TopSlotBit == 0 && b.WoodType == "acacia":
					return 7;
				case { } b when true && b.TopSlotBit == 1 && b.WoodType == "birch":
					return 8;
				case { } b when true && b.TopSlotBit == 1 && b.WoodType == "acacia":
					return 9;
				case { } b when true && b.TopSlotBit == 1 && b.WoodType == "oak":
					return 10;
				case { } b when true && b.TopSlotBit == 0 && b.WoodType == "spruce":
					return 11;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class Wool // 35 typeof=Wool
	{
		// Convert this attribute to enum
		//[Enum("black","blue","brown","cyan","gray","green","light_blue","lime","magenta","orange","pink","purple","red","silver","white","yellow"]
		public string Color { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					Color = "light_blue";
					break;
				case 1:
					Color = "purple";
					break;
				case 2:
					Color = "gray";
					break;
				case 3:
					Color = "black";
					break;
				case 4:
					Color = "pink";
					break;
				case 5:
					Color = "red";
					break;
				case 6:
					Color = "white";
					break;
				case 7:
					Color = "cyan";
					break;
				case 8:
					Color = "silver";
					break;
				case 9:
					Color = "blue";
					break;
				case 10:
					Color = "magenta";
					break;
				case 11:
					Color = "brown";
					break;
				case 12:
					Color = "orange";
					break;
				case 13:
					Color = "green";
					break;
				case 14:
					Color = "lime";
					break;
				case 15:
					Color = "yellow";
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.Color == "light_blue":
					return 0;
				case { } b when true && b.Color == "purple":
					return 1;
				case { } b when true && b.Color == "gray":
					return 2;
				case { } b when true && b.Color == "black":
					return 3;
				case { } b when true && b.Color == "pink":
					return 4;
				case { } b when true && b.Color == "red":
					return 5;
				case { } b when true && b.Color == "white":
					return 6;
				case { } b when true && b.Color == "cyan":
					return 7;
				case { } b when true && b.Color == "silver":
					return 8;
				case { } b when true && b.Color == "blue":
					return 9;
				case { } b when true && b.Color == "magenta":
					return 10;
				case { } b when true && b.Color == "brown":
					return 11;
				case { } b when true && b.Color == "orange":
					return 12;
				case { } b when true && b.Color == "green":
					return 13;
				case { } b when true && b.Color == "lime":
					return 14;
				case { } b when true && b.Color == "yellow":
					return 15;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class YellowFlower // 37 typeof=YellowFlower
	{
		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true:
					return 0;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class

	public partial class YellowGlazedTerracotta // 224 typeof=YellowGlazedTerracotta
	{
		[Range(0, 5)] public int FacingDirection { get; set; }

		public void SetStateFromMetadata(byte metadata)
		{
			switch (metadata)
			{
				case 0:
					FacingDirection = 3;
					break;
				case 1:
					FacingDirection = 2;
					break;
				case 2:
					FacingDirection = 0;
					break;
				case 3:
					FacingDirection = 5;
					break;
				case 4:
					FacingDirection = 4;
					break;
				case 5:
					FacingDirection = 1;
					break;
			} // switch
		} // method

		public byte GetMetadataFromState()
		{
			switch (this)
			{
				case { } b when true && b.FacingDirection == 3:
					return 0;
				case { } b when true && b.FacingDirection == 2:
					return 1;
				case { } b when true && b.FacingDirection == 0:
					return 2;
				case { } b when true && b.FacingDirection == 5:
					return 3;
				case { } b when true && b.FacingDirection == 4:
					return 4;
				case { } b when true && b.FacingDirection == 1:
					return 5;
			} // switch
			throw new ArithmeticException("Invalid state. Unable to convert state to valid metadata");
		} // method
	} // class
}