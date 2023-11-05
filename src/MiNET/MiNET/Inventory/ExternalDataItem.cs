using System.ComponentModel;
using Newtonsoft.Json;

namespace MiNET.Inventory
{
	public class ExternalDataItem
	{
		[JsonProperty("name")]
		public string Id { get; set; }

		[JsonProperty("meta")]
		public short Metadata { get; set; }

		[JsonProperty("block_states")]
		public string BlockStates { get; set; }

		[JsonProperty("nbt")]
		public string ExtraData { get; set; }

		[JsonProperty("tag")]
		public string Tag { get; set; }

		[DefaultValue(1)]
		[JsonProperty("count", DefaultValueHandling = DefaultValueHandling.Populate)]
		public int Count { get; set; }
	}
}
