using System;
using System.Collections.Generic;
using System.Text;

namespace MiNET.Utils.Skins
{
	public class SkinPiece
	{
		public string PieceType { get; set; }

		public List<string> Colors { get; set; }

		public SkinPiece()
		{
			Colors = new List<string>();
		}
	}
}
