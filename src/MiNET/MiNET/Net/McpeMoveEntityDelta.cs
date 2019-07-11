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
using log4net;
using MiNET.Utils;

namespace MiNET.Net
{
	public partial class McpeMoveEntityDelta
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(McpeMoveEntityDelta));

		public PlayerLocation currentPosition; // = null;
		public PlayerLocation prevSentPosition; // = null;
		public bool isOnGround; // = null;

		private int _dX = 0;
		private int _dY = 0;
		private int _dZ = 0;
		private int _dPitch = 0;
		private int _dYaw = 0;
		private int _dHeadYaw = 0;

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

			if (_dX != 0) flags |= 0x1;
			if (_dY != 0) flags |= 0x2;
			if (_dZ != 0) flags |= 0x4;

			if (prevSentPosition.Pitch != currentPosition.Pitch) flags |= 0x8;
			if (prevSentPosition.Yaw != currentPosition.Yaw) flags |= 0x10;
			if (prevSentPosition.HeadYaw != currentPosition.HeadYaw) flags |= 0x20;

			if (flags != 0 && isOnGround) flags |= 0x40;

			return flags != 0;
		}

		partial void AfterEncode()
		{
			//if (Log.IsDebugEnabled) Log.Debug($"Flags: 0x{flags:X2} {Convert.ToString((byte) flags, 2)}, {currentPosition.X} {currentPosition.Y} {currentPosition.Z}, {_dX} {_dY} {_dZ}");

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

			var d = 256f / 360f;

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

		public static int ToIntDelta(float a, float b)
		{
			return BitConverter.ToInt32(BitConverter.GetBytes((float) Math.Round(a, 2)), 0) - BitConverter.ToInt32(BitConverter.GetBytes((float) Math.Round(b, 2)), 0);
		}

		partial void AfterDecode()
		{
			//Log.Debug($"Flags: 0x{flags:X2} ({flags}) {Convert.ToString((byte) flags, 2)}");

			if ((flags & 0x1) != 0)
			{
				ReadSignedVarInt();
			}
			if ((flags & 0x2) != 0)
			{
				ReadSignedVarInt();
			}
			if ((flags & 0x4) != 0)
			{
				ReadSignedVarInt();
			}

			if ((flags & 0x8) != 0)
			{
				ReadByte();
			}

			if ((flags & 0x10) != 0)
			{
				ReadByte();
			}

			if ((flags & 0x20) != 0)
			{
				ReadByte();
			}
		}
	}
}