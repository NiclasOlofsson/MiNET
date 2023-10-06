using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.Crafting
{
	public class TrimPattern
	{
		public string ItemId { get; set; }
		public string PatternId { get; set; }
	}

	public class TrimMaterial
	{
		public string MaterialId { get; set; }
		public string Color { get; set; }
		public string ItemId { get; set; }
	}
}
