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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2017 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System;
using log4net;
using MiNET.Entities.Behaviors;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Entities.Passive
{
	public class Horse : PassiveMob
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (Horse));

		private int _type = 0;

		public bool IsEating { get; set; }

		public Horse(Level level, bool isDonkey = false, Random rnd = null) : base(isDonkey ? EntityType.Donkey : EntityType.Horse, level)
		{
			Width = Length = 1.4;
			Height = 1.6;
			var random = rnd ?? new Random((int) DateTime.UtcNow.Ticks);
			_type = random.Next(7);
			Speed = 0.1125 + new Random().NextDouble()*(0.3375 - 0.1125);

			Behaviors.Add(new PanicBehavior(this, 60, Speed, 1.2));
			Behaviors.Add(new HorseEatBlockBehavior(this, 100));
			Behaviors.Add(new WanderBehavior(this, Speed, 0.7));
			Behaviors.Add(new LookAtPlayerBehavior(this));
			Behaviors.Add(new RandomLookaroundBehavior(this));
		}

		public override MetadataDictionary GetMetadata()
		{
			var metadata = base.GetMetadata();
			metadata[2] = new MetadataInt(_type);
			metadata[16] = new MetadataInt(IsEating ? 32 : 0); // 0 or 32?
			metadata[(int)MetadataFlags.Scale] = new MetadataFloat(IsBaby ? 0.5582917f : 1.0);
			return metadata;
		}
	}
}