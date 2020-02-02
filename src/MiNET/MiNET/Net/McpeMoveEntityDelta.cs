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
using log4net;
using MiNET.Utils;

namespace MiNET.Net
{
	public partial class McpeMoveEntityDelta
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(McpeMoveEntityDelta));

		public const int HasX = 0x01;
		public const int HasY = 0x02;
		public const int HasZ = 0x04;
		public const int HasRotX = 0x08;
		public const int HasRotY = 0x10;
		public const int HasRotZ = 0x20;
		public const int OnGround = 0x40;

		public PlayerLocation currentPosition; // = null;
		public PlayerLocation prevSentPosition; // = null;
		public bool isOnGround; // = null;

		private int _dX;
		private int _dY;
		private int _dZ;
		private int _dPitch;
		private int _dYaw;
		private int _dHeadYaw;

		partial void BeforeEncode()
		{
			// set the flags
			bool shouldSend = flags != 0 || SetFlags();

			if (Log.IsDebugEnabled && !shouldSend) Log.Warn("Sending delta move with no change. Please fix!");
		}

		public bool SetFlags()
		{
			flags = 0;

			if (currentPosition == null || prevSentPosition == null) return false;

			_dX = ToIntDelta(currentPosition.X, prevSentPosition.X);
			_dY = ToIntDelta(currentPosition.Y, prevSentPosition.Y);
			_dZ = ToIntDelta(currentPosition.Z, prevSentPosition.Z);

			if (_dX != 0) flags |= HasX;
			if (_dY != 0) flags |= HasY;
			if (_dZ != 0) flags |= HasZ;

			if (prevSentPosition.Pitch != currentPosition.Pitch) flags |= HasRotX;
			if (prevSentPosition.Yaw != currentPosition.Yaw) flags |= HasRotY;
			if (prevSentPosition.HeadYaw != currentPosition.HeadYaw) flags |= HasRotZ;

			if (flags != 0 && isOnGround) flags |= OnGround;

			return flags != 0;
		}

		partial void AfterEncode()
		{
			// write the values
			if ((flags & 0x1) != 0)
			{
				WriteSignedVarInt(_dX);
			}
			if ((flags & 0x2) != 0)
			{
				WriteSignedVarInt(_dY);
			}
			if ((flags & 0x4) != 0)
			{
				WriteSignedVarInt(_dZ);
			}

			float d = 256f / 360f;
			if ((flags & 0x8) != 0)
			{
				Write((byte) Math.Round(currentPosition.Pitch * d)); // 256/360
			}

			if ((flags & 0x10) != 0)
			{
				Write((byte) Math.Round(currentPosition.Yaw * d)); // 256/360
			}

			if ((flags & 0x20) != 0)
			{
				Write((byte) Math.Round(currentPosition.HeadYaw * d)); // 256/360
			}
		}

		public static int ToIntDelta(float current, float prev)
		{
			return BitConverter.SingleToInt32Bits(current) - BitConverter.SingleToInt32Bits(prev);
		}

		public static float FromIntDelta(float prev, int delta)
		{
			return BitConverter.Int32BitsToSingle(BitConverter.SingleToInt32Bits(prev) + delta);
		}

		public PlayerLocation GetCurrentPosition(PlayerLocation previousPosition)
		{
			if ((flags & HasX) != 0)
			{
				currentPosition.X = FromIntDelta(previousPosition.X, _dX);
			}
			if ((flags & HasY) != 0)
			{
				currentPosition.Y = FromIntDelta(previousPosition.Y, _dY);
			}
			if ((flags & HasZ) != 0)
			{
				currentPosition.Z = FromIntDelta(previousPosition.Z, _dZ);
			}

			return currentPosition;
		}

		partial void AfterDecode()
		{
			currentPosition = new PlayerLocation();

			if ((flags & HasX) != 0)
			{
				_dX = ReadSignedVarInt();
			}
			if ((flags & HasY) != 0)
			{
				_dY = ReadSignedVarInt();
			}
			if ((flags & HasZ) != 0)
			{
				_dZ = ReadSignedVarInt();
			}

			float d = 1f / (256f / 360f);
			if ((flags & HasRotX) != 0)
			{
				currentPosition.Pitch = ReadByte() * d;
			}

			if ((flags & HasRotY) != 0)
			{
				currentPosition.Yaw = ReadByte() * d;
			}

			if ((flags & HasRotZ) != 0)
			{
				currentPosition.HeadYaw = ReadByte() * d;
			}

			if ((flags & OnGround) != 0)
			{
				isOnGround = true;
			}
		}
	}
}