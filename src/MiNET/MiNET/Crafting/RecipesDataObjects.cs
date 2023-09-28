using System.Collections.Generic;
using MiNET.Inventory;

namespace MiNET.Crafting
{
	internal class ShapelessRecipeData
	{
		public string Block { get; set; }

		public ExternalDataItem[] Input { get; set; }

		public ExternalDataItem[] Output { get; set; }

		public int Priority { get; set; }

		public string[] Shape { get; set; }
	}

	internal class ShapedRecipeData
	{
		public string Block { get; set; }

		public Dictionary<string, ExternalDataItem> Input { get; set; }

		public ExternalDataItem[] Output { get; set; }

		public int Priority { get; set; }

		public string[] Shape { get; set; }
	}
}
