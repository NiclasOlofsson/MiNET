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

namespace MiNET
{
	public class PlayerAttribute
	{
		public string Name { get; set; }
		public float MinValue { get; set; }
		public float MaxValue { get; set; }
		public float Value { get; set; }
		public float Default { get; set; }

		public override string ToString()
		{
			return $"{{Name: {Name}, MinValue: {MinValue}, MaxValue: {MaxValue}, Value: {Value}, Default: {Default}}}";
		}
	}

	public class EntityAttribute
	{
		public string Name { get; set; }
		public float MinValue { get; set; }
		public float MaxValue { get; set; }
		public float Value { get; set; }

		public override string ToString()
		{
			return $"{{Name: {Name}, MinValue: {MinValue}, MaxValue: {MaxValue}, Value: {Value}}}";
		}
	}


	public enum GameRulesEnum
	{
		CommandblockOutput,
		DoDaylightcycle,
		DoEntitydrops,
		DoFiretick,
		DoMobloot,
		DoMobspawning,
		DoTiledrops,
		DoWeathercycle,
		DrowningDamage,
		Falldamage,
		Firedamage,
		KeepInventory,
		Mobgriefing,
		Pvp,
		ShowCoordinates,
		NaturalRegeneration,
		TntExplodes,
		SendCommandfeedback,
		ExperimentalGameplay,
		// int,
		DoInsomnia,
		CommandblocksEnabled,
		// int,
		DoImmediateRespawn,
		ShowDeathmessages,
		// int,
	}

	public abstract class GameRule
	{
		public string Name { get; }
		public bool IsPlayerModifiable { get; set; } = true;

		protected GameRule(string name)
		{
			Name = name;
		}

		protected bool Equals(GameRule other)
		{
			return string.Equals(Name, other.Name);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((GameRule) obj);
		}

		public override int GetHashCode()
		{
			return (Name != null ? Name.GetHashCode() : 0);
		}
	}

	public class GameRule<T> : GameRule
	{
		public T Value { get; set; }

		public GameRule(GameRulesEnum rule, T value) : this(rule.ToString(), value)
		{
		}

		public GameRule(string name, T value) : base(name)
		{
			Value = value;
		}
	}
}