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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2021 Niclas Olofsson.
// All Rights Reserved.
#endregion

using System;
using System.Collections.Generic;
using System.Numerics;
using log4net;
using MiNET.Items;
using MiNET.Utils;
using Newtonsoft.Json;

namespace MiNET.Net.Items
{
	public class ItemTranslator
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(ItemTranslator));
		
		private IDictionary<int, TranslatedItem> _networkIdToInternal;
		private IDictionary<int, int> _simpleNetworkIdToInternal;

		private Dictionary<int, ComplexMappingEntry> _internalIdToNetwork;
		private Dictionary<int, int> _simpleInternalIdToNetwork;

		private Dictionary<string, string> _internalNameToNetworkName;
		public ItemTranslator(Itemstates itemstates)
		{
			var internalNameToNetworkName = new Dictionary<string, string>(StringComparer.Ordinal);
			var legacyTranslations = ResourceUtil.ReadResource<Dictionary<string, short>>("item_id_map.json", typeof(Item), "Data");
			var r16Mapping = ResourceUtil.ReadResource<R16ToCurrentMap>("r16_to_current_item_map.json", typeof(Item), "Data");

			var simpleMappings = new Dictionary<string, short>();

			foreach (var entry in r16Mapping.Simple)
			{
				var oldId = entry.Key;
				var newId = entry.Value;

				if (simpleMappings.ContainsKey(newId))
				{
					Log.Warn($"Duplicate mapping for StringID. NewId={newId} OldId={oldId}");

					continue;
				}


				if (!legacyTranslations.ContainsKey(oldId))
				{
					Log.Warn($"Could not translate item! OldId={oldId} NewId={newId}");
					continue;
				}
				
				simpleMappings[newId] = legacyTranslations[oldId];
				internalNameToNetworkName[oldId] = newId;
			}

			foreach (var entry in legacyTranslations)
			{
				var stringId = entry.Key;
				var integerId = entry.Value;
				
				if (simpleMappings.ContainsKey(stringId))
					continue;
				
				simpleMappings[stringId] = integerId;
			}

			var complexMapping = new Dictionary<string, TranslatedItem>();
			foreach (var entry in r16Mapping.Complex)
			{
				string oldId = entry.Key;
				if (!legacyTranslations.ContainsKey(oldId))
					continue;
				
				var legacyIntegerId = legacyTranslations[oldId];
				foreach (var mappingEntry in entry.Value)
				{
					var newId = mappingEntry.Value;
					if (short.TryParse(mappingEntry.Key, out var meta))
					{
						if (!complexMapping.TryAdd(newId, new TranslatedItem(legacyIntegerId, meta)))
						{
							Log.Warn($"Duplicate complex... OldId={oldId} NewId={newId} (IntegerID={legacyIntegerId} Meta={meta})");
						}
					}
				}
			}
			
			var internalToNetwork = new Dictionary<int, ComplexMappingEntry>();
			var simpleInternalToNetwork = new Dictionary<int, int>();
			var networkIdToInternal = new Dictionary<int, TranslatedItem>();
			var simpleNetworkIdToInternal = new Dictionary<int, int>();
			foreach (var state in itemstates)
			{
				var stringId = state.Name;
				var netId = state.Id;

				if (complexMapping.TryGetValue(stringId, out var translatedItem))
				{
					var internalId = translatedItem.Id;
					var internalMeta = translatedItem.Meta;
					
					ComplexMappingEntry mappingEntry;

					if (!internalToNetwork.TryGetValue(internalId, out mappingEntry))
					{
						mappingEntry = new ComplexMappingEntry();
						internalToNetwork.Add(internalId, mappingEntry);
					}

					mappingEntry.Add(internalMeta, netId);
					
					internalToNetwork[internalId] = mappingEntry;
					networkIdToInternal.Add(netId, translatedItem);
				}
				else if (simpleMappings.TryGetValue(stringId, out var legacyId))
				{
					simpleNetworkIdToInternal.Add(netId, legacyId);
					simpleInternalToNetwork.Add(legacyId, netId);
				}
			}

			_internalIdToNetwork = internalToNetwork;
			_simpleInternalIdToNetwork = simpleInternalToNetwork;
			_networkIdToInternal = networkIdToInternal;
			_simpleNetworkIdToInternal = simpleNetworkIdToInternal;
			_internalNameToNetworkName = internalNameToNetworkName;
		}

		internal bool TryGetNetworkId(int id, short meta, out TranslatedItem item)
		{
			int netId;
			if (_internalIdToNetwork.TryGetValue(id, out var complex) && complex.TryGet(meta, out netId))
			{
				item = new TranslatedItem(netId, 0);
				return true;
			}
			else if (_simpleInternalIdToNetwork.TryGetValue(id, out netId))
			{
				item = new TranslatedItem(netId, meta);
				return true;
			}

			item = default;
			return false;
		}
		
		internal TranslatedItem ToNetworkId(int id, short meta)
		{
			int netId;
			if (_internalIdToNetwork.TryGetValue(id, out var complex) && complex.TryGet(meta, out netId))
			{
				id = netId;
				meta = 0;
			}
			else if (_simpleInternalIdToNetwork.TryGetValue(id, out netId))
			{
				id = netId;
			}

			return new TranslatedItem(id, meta);
		}
		
		internal TranslatedItem FromNetworkId(int id, short meta)
		{
			if (_networkIdToInternal.TryGetValue(id, out var value))
			{
				id = value.Id;
				meta = value.Meta;
			}
			else if (_simpleNetworkIdToInternal.TryGetValue(id, out var simpleValue))
			{
				id = simpleValue;
			}

			return new TranslatedItem(id, meta);
		}

		public bool TryGetName(string input, out string output)
		{
			return _internalNameToNetworkName.TryGetValue(input, out output);
		}
	}

	internal class TranslatedItem : IEquatable<TranslatedItem>
	{
		public int Id { get; }
		public short Meta { get; }

		public TranslatedItem(int id, short meta)
		{
			Id = id;
			Meta = meta;
		}

		/// <inheritdoc />
		public bool Equals(TranslatedItem other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;

			return Id == other.Id && Meta == other.Meta;
		}

		/// <inheritdoc />
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;

			return Equals((TranslatedItem)obj);
		}

		/// <inheritdoc />
		public override int GetHashCode()
		{
			return HashCode.Combine(Id, Meta);
		}
	}
	
	class R16ToCurrentMap
	{
		[JsonProperty("complex")]
		public Dictionary<string, Dictionary<string, string>> Complex { get; set; }
			
		[JsonProperty("simple")]
		public Dictionary<string, string> Simple { get; set; }
	}

	internal class ComplexMappingEntry
	{
		private Dictionary<short, int> _mapping = new Dictionary<short, int>();
		public void Add(short meta, short translatedItem)
		{
			_mapping.Add(meta, translatedItem);
		}

		public bool TryGet(short meta, out int result)
		{
			return _mapping.TryGetValue(meta, out result);
		}
	}
}