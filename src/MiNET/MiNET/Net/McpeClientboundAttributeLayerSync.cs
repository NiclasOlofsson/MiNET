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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2026 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System.Collections.Generic;

namespace MiNET.Net
{
	// payload_type: 0=update_layers, 1=update_layer_settings, 2=update_environment, 3=remove_environment

	public class AttributeValueData
	{
		public byte Type { get; set; } // 0=bool, 1=float, 2=color, 3=string
		public bool BoolValue { get; set; }
		public float FloatValue { get; set; }
		public float? FloatConstraintMin { get; set; }
		public float? FloatConstraintMax { get; set; }
		public byte ColorType { get; set; } // 0=string, 1=array of 4
		public string ColorString { get; set; }
		public int[] ColorArray { get; set; }
		public string StringValue { get; set; }
		public string Operation { get; set; }
	}

	public class AttributeLayerWeightData
	{
		public byte Type { get; set; } // 0=float, 1=string
		public float FloatValue { get; set; }
		public string StringValue { get; set; }
	}

	public class AttributeLayerSettingsData
	{
		public int Priority { get; set; }
		public AttributeLayerWeightData Weight { get; set; } = new AttributeLayerWeightData();
		public bool Enabled { get; set; }
		public bool TransitionsPaused { get; set; }
	}

	public class EnvironmentAttributeData
	{
		public string Name { get; set; }
		public AttributeValueData FromAttribute { get; set; }
		public AttributeValueData Attribute { get; set; }
		public AttributeValueData ToAttribute { get; set; }
		public uint CurrentTransitionTicks { get; set; }
		public uint TotalTransitionTicks { get; set; }
		public string EaseType { get; set; }
		public uint LocalTransitionTicks { get; set; }
		public bool NoiseTransition { get; set; }
	}

	public class AttributeLayerData
	{
		public string Name { get; set; }
		public string NoiseName { get; set; }
		public int Dimension { get; set; }
		public AttributeLayerSettingsData Settings { get; set; } = new AttributeLayerSettingsData();
		public List<EnvironmentAttributeData> Attributes { get; set; } = new List<EnvironmentAttributeData>();
	}

	public partial class McpeClientboundAttributeLayerSync : Packet<McpeClientboundAttributeLayerSync>
	{
		public List<AttributeLayerData> Layers { get; set; } = new List<AttributeLayerData>();

		public string LayerName { get; set; }
		public int LayerDimension { get; set; }
		public AttributeLayerSettingsData LayerSettings { get; set; }

		public string EnvLayerName { get; set; }
		public int EnvDimension { get; set; }
		public List<EnvironmentAttributeData> EnvAttributes { get; set; } = new List<EnvironmentAttributeData>();

		public string RemoveLayerName { get; set; }
		public int RemoveDimension { get; set; }
		public List<string> RemoveAttributes { get; set; } = new List<string>();

		private AttributeValueData ReadAttributeValue()
		{
			var value = new AttributeValueData {Type = (byte) ReadUnsignedVarInt()};

			switch (value.Type)
			{
				case 0:
					value.BoolValue = ReadBool();
					value.Operation = ReadString();
					break;
				case 1:
					value.FloatValue = ReadFloat();
					value.Operation = ReadString();
					if (ReadBool()) value.FloatConstraintMin = ReadFloat();
					if (ReadBool()) value.FloatConstraintMax = ReadFloat();
					break;
				case 2:
					value.ColorType = (byte) ReadUnsignedVarInt();
					if (value.ColorType == 0)
					{
						value.ColorString = ReadString();
					}
					else
					{
						value.ColorArray = new[] {ReadInt(), ReadInt(), ReadInt(), ReadInt()};
					}
					value.Operation = ReadString();
					break;
				case 3:
					value.StringValue = ReadString();
					value.Operation = ReadString();
					break;
			}

			return value;
		}

		private void WriteAttributeValue(AttributeValueData value)
		{
			WriteUnsignedVarInt(value.Type);

			switch (value.Type)
			{
				case 0:
					Write(value.BoolValue);
					Write(value.Operation);
					break;
				case 1:
					Write(value.FloatValue);
					Write(value.Operation);
					Write(value.FloatConstraintMin.HasValue);
					if (value.FloatConstraintMin.HasValue) Write(value.FloatConstraintMin.Value);
					Write(value.FloatConstraintMax.HasValue);
					if (value.FloatConstraintMax.HasValue) Write(value.FloatConstraintMax.Value);
					break;
				case 2:
					WriteUnsignedVarInt(value.ColorType);
					if (value.ColorType == 0)
					{
						Write(value.ColorString);
					}
					else
					{
						Write(value.ColorArray[0]);
						Write(value.ColorArray[1]);
						Write(value.ColorArray[2]);
						Write(value.ColorArray[3]);
					}
					Write(value.Operation);
					break;
				case 3:
					Write(value.StringValue);
					Write(value.Operation);
					break;
			}
		}

		private AttributeLayerSettingsData ReadAttributeLayerSettings()
		{
			var settings = new AttributeLayerSettingsData {Priority = ReadInt()};

			settings.Weight.Type = (byte) ReadUnsignedVarInt();
			if (settings.Weight.Type == 0)
			{
				settings.Weight.FloatValue = ReadFloat();
			}
			else
			{
				settings.Weight.StringValue = ReadString();
			}

			settings.Enabled = ReadBool();
			settings.TransitionsPaused = ReadBool();

			return settings;
		}

		private void WriteAttributeLayerSettings(AttributeLayerSettingsData settings)
		{
			Write(settings.Priority);

			WriteUnsignedVarInt(settings.Weight.Type);
			if (settings.Weight.Type == 0)
			{
				Write(settings.Weight.FloatValue);
			}
			else
			{
				Write(settings.Weight.StringValue);
			}

			Write(settings.Enabled);
			Write(settings.TransitionsPaused);
		}

		private EnvironmentAttributeData ReadEnvironmentAttribute()
		{
			var attribute = new EnvironmentAttributeData {Name = ReadString()};

			attribute.FromAttribute = ReadBool() ? ReadAttributeValue() : null;
			attribute.Attribute = ReadAttributeValue();
			attribute.ToAttribute = ReadBool() ? ReadAttributeValue() : null;
			attribute.CurrentTransitionTicks = ReadUint();
			attribute.TotalTransitionTicks = ReadUint();
			attribute.EaseType = ReadString();
			attribute.LocalTransitionTicks = ReadUint();
			attribute.NoiseTransition = ReadBool();

			return attribute;
		}

		private void WriteEnvironmentAttribute(EnvironmentAttributeData attribute)
		{
			Write(attribute.Name);

			Write(attribute.FromAttribute != null);
			if (attribute.FromAttribute != null) WriteAttributeValue(attribute.FromAttribute);

			WriteAttributeValue(attribute.Attribute);

			Write(attribute.ToAttribute != null);
			if (attribute.ToAttribute != null) WriteAttributeValue(attribute.ToAttribute);

			Write(attribute.CurrentTransitionTicks);
			Write(attribute.TotalTransitionTicks);
			Write(attribute.EaseType);
			Write(attribute.LocalTransitionTicks);
			Write(attribute.NoiseTransition);
		}

		private AttributeLayerData ReadAttributeLayer()
		{
			var layer = new AttributeLayerData {Name = ReadString()};

			layer.NoiseName = ReadBool() ? ReadString() : null;
			layer.Dimension = ReadSignedVarInt();
			layer.Settings = ReadAttributeLayerSettings();

			uint attributeCount = ReadUnsignedVarInt();
			for (int i = 0; i < attributeCount; i++) layer.Attributes.Add(ReadEnvironmentAttribute());

			return layer;
		}

		private void WriteAttributeLayer(AttributeLayerData layer)
		{
			Write(layer.Name);

			Write(layer.NoiseName != null);
			if (layer.NoiseName != null) Write(layer.NoiseName);

			WriteSignedVarInt(layer.Dimension);
			WriteAttributeLayerSettings(layer.Settings);

			WriteUnsignedVarInt((uint) layer.Attributes.Count);
			foreach (var attribute in layer.Attributes) WriteEnvironmentAttribute(attribute);
		}

		partial void AfterDecode()
		{
			switch (payloadType)
			{
				case 0: // update_layers
				{
					uint count = ReadUnsignedVarInt();
					for (int i = 0; i < count; i++) Layers.Add(ReadAttributeLayer());
					break;
				}
				case 1: // update_layer_settings
				{
					LayerName = ReadString();
					LayerDimension = ReadSignedVarInt();
					LayerSettings = ReadAttributeLayerSettings();
					break;
				}
				case 2: // update_environment
				{
					EnvLayerName = ReadString();
					EnvDimension = ReadSignedVarInt();
					uint count = ReadUnsignedVarInt();
					for (int i = 0; i < count; i++) EnvAttributes.Add(ReadEnvironmentAttribute());
					break;
				}
				case 3: // remove_environment
				{
					RemoveLayerName = ReadString();
					RemoveDimension = ReadSignedVarInt();
					uint count = ReadUnsignedVarInt();
					for (int i = 0; i < count; i++) RemoveAttributes.Add(ReadString());
					break;
				}
			}
		}

		partial void AfterEncode()
		{
			switch (payloadType)
			{
				case 0: // update_layers
					WriteUnsignedVarInt((uint) Layers.Count);
					foreach (var layer in Layers) WriteAttributeLayer(layer);
					break;
				case 1: // update_layer_settings
					Write(LayerName);
					WriteSignedVarInt(LayerDimension);
					WriteAttributeLayerSettings(LayerSettings);
					break;
				case 2: // update_environment
					Write(EnvLayerName);
					WriteSignedVarInt(EnvDimension);
					WriteUnsignedVarInt((uint) EnvAttributes.Count);
					foreach (var attribute in EnvAttributes) WriteEnvironmentAttribute(attribute);
					break;
				case 3: // remove_environment
					Write(RemoveLayerName);
					WriteSignedVarInt(RemoveDimension);
					WriteUnsignedVarInt((uint) RemoveAttributes.Count);
					foreach (var name in RemoveAttributes) Write(name);
					break;
			}
		}
	}
}
