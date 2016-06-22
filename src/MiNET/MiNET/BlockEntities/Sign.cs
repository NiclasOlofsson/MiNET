using System;
using System.Collections.Generic;

using MiNET.BlockEntities;
using MiNET.Utils;

using fNbt;

using Newtonsoft.Json.Linq;

namespace MiNET.BlockEntities
{
	public class Sign : BlockEntity
	{
		public string Text1 { get; set; }
		public string Text2 { get; set; }
		public string Text3 { get; set; }
		public string Text4 { get; set; }
		
		public static readonly Dictionary<string, string> Formats = new Dictionary<string, string>
		{
			{ChatFormatting.Bold, "bold"},
			{ChatFormatting.Italic, "italic"},
			{ChatFormatting.Strikethrough, "strikethrough"},
			{ChatFormatting.Underline, "underlined"},
			{ChatFormatting.Obfuscated, "obfuscated"}
	        };

		public static readonly Dictionary<string, string> Colors = new Dictionary<string, string>
		{
				{ChatColors.Aqua, "aqua"},
				{ChatColors.Black, "black"},
				{ChatColors.Blue, "blue"},
				{ChatColors.DarkAqua, "dark_aqua"},
				{ChatColors.DarkBlue, "dark_blue"},
				{ChatColors.DarkGray, "dark_gray"},
				{ChatColors.DarkGreen, "dark_green"},
				{ChatColors.DarkPurple, "dark_purple"},
				{ChatColors.DarkRed, "dark_red"},
				{ChatColors.Gold, "gold"},
				{ChatColors.Gray, "gray"},
				{ChatColors.Green, "green"},
				{ChatColors.LightPurple, "light_purple"},
				{ChatColors.Red, "red"},
				{ChatColors.White, "white"},
				{ChatColors.Yellow, "yellow"},
                };

		public Sign() : base("Sign")
		{
			Text1 = string.Empty;
			Text2 = string.Empty;
			Text3 = string.Empty;
			Text4 = string.Empty;
		}

		public override NbtCompound GetCompound()
		{
			var compound = new NbtCompound(string.Empty)
			{
				new NbtString("id", Id),
				new NbtString("Text1", Text1 ?? string.Empty),
				new NbtString("Text2", Text2 ?? string.Empty),
				new NbtString("Text3", Text3 ?? string.Empty),
				new NbtString("Text4", Text4 ?? string.Empty),
				new NbtInt("x", Coordinates.X),
				new NbtInt("y", Coordinates.Y),
				new NbtInt("z", Coordinates.Z)
			};

			return compound;
		}

		public override void SetCompound(NbtCompound compound)
		{
			Text1 = GetTextValue(compound, "Text1");
			Text2 = GetTextValue(compound, "Text2");
			Text3 = GetTextValue(compound, "Text3");
			Text4 = GetTextValue(compound, "Text4");
		}

		private string GetTextValue(NbtCompound compound, string key)
		{
			NbtString text;
			compound.TryGet(key, out text);
			return text != null ? (text.StringValue ?? string.Empty) : string.Empty;
		}
		
		private static string DecodeSignText(string text)
		{
			
			var message = "";

			try {
			
				var obj = JObject.Parse(text);
				var extra = (JArray) obj["extra"];
				
				if (extra != null)
				{
				
					foreach (var jToken in extra)
					{
						var strings = (JObject) jToken;
						
						foreach (var format in Formats)
						{
						
							JToken form;
							if (!strings.TryGetValue(format.Value, out form)) continue;
							
							if ((bool) form) message += format.Key;
							
						}
						
						foreach (var color in Colors)
						{
						
							JToken form;
							if (!strings.TryGetValue("color", out form)) continue;
						
							if (color.Value == (string) form) message += color.Key;
						}
						
						message += (string) strings["text"];
					
					}
				
				} 
				else
				{
				
					JToken txt;
					obj.TryGetValue("text", out txt);
				
					message = (string) txt;
				
				}
				
			} 
			catch (Exception e)
			{
				message = text;
			}
			
			return message;
			
		}
		
	}
}
