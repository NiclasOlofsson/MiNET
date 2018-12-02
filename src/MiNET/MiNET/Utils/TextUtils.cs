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
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MiNET.Utils
{
	public static class ChatColors
	{
		public const string Black = "§0";
		public const string DarkBlue = "§1";
		public const string DarkGreen = "§2";
		public const string DarkAqua = "§3";
		public const string DarkRed = "§4";
		public const string DarkPurple = "§5";
		public const string Gold = "§6";
		public const string Gray = "§7";
		public const string DarkGray = "§8";
		public const string Blue = "§9";
		public const string Green = "§a";
		public const string Aqua = "§b";
		public const string Red = "§c";
		public const string LightPurple = "§d";
		public const string Yellow = "§e";
		public const string White = "§f";
	}

	public static class ChatFormatting
	{
		public const string Obfuscated = "§k";
		public const string Bold = "§l";
		public const string Strikethrough = "§m";
		public const string Underline = "§n";
		public const string Italic = "§o";
		public const string Reset = "§r";
	}

	public class TextUtils
	{
		private const int LineLength = 30;
		private const int CharWidth = 6;

		private const char SpaceChar = ' ';

		private static readonly Regex CleanAllFormattingFilter = new Regex("(?:&|§)([0123456789abcdefklmnor])",
			RegexOptions.Compiled & RegexOptions.IgnoreCase);

		private static readonly Regex CleanColourFilter = new Regex("(?:&|§)([0123456789abcdef])",
			RegexOptions.Compiled & RegexOptions.IgnoreCase);

		private static readonly Regex BoldTextRegex = new Regex("(?:&|§)l(.+?)(?:[&|§]r|$)",
			RegexOptions.Compiled & RegexOptions.IgnoreCase);

		private static readonly IDictionary<char, int> CharWidths = new Dictionary<char, int>
		{
			{' ', 4},
			{'!', 2},
			{'"', 5},
			{'\'', 3},
			{'(', 5},
			{')', 5},
			{'*', 5},
			{',', 2},
			{'.', 2},
			{':', 2},
			{';', 2},
			{'<', 5},
			{'>', 5},
			{'@', 7},
			{'I', 4},
			{'[', 4},
			{']', 4},
			{'f', 5},
			{'i', 2},
			{'k', 5},
			{'l', 3},
			{'t', 4},
			{'{', 5},
			{'|', 2},
			{'}', 5},
			{'~', 7},
			{'█', 9},
			{'░', 8},
			{'▒', 9},
			{'▓', 9},
			{'▌', 5},
			{'─', 9}
			//{'-', 9},
		};

		public static string CenterLine(string input)
		{
			return Center(input, LineLength * CharWidth);
		}

		public static string Center(string input)
		{
			return Center(input, 0);
		}

		public static string Center(string input, int maxLength, bool addRightPadding = false)
		{
			var lines = input.Trim().Split('\n');
			var sortedLines = lines.OrderByDescending(GetPixelLength);

			var longest = sortedLines.First();
			if (maxLength == 0)
			{
				maxLength = GetPixelLength(longest);
			}

			var result = "";

			var spaceWidth = GetCharWidth(SpaceChar);

			foreach (var sortedLine in lines)
			{
				var len = Math.Max(maxLength - GetPixelLength(sortedLine), 0);
				var padding = (int) Math.Round(len / (2d * spaceWidth));
				var paddingRight = (int) Math.Floor(len / (2d * spaceWidth));
				result += new string(SpaceChar, padding) + sortedLine + "§r" + (addRightPadding ? new string(SpaceChar, paddingRight) : "") + "\n";
			}

			result = result.TrimEnd('\n');

			return result;
		}

		private static int GetCharWidth(char c)
		{
			int width;
			if (CharWidths.TryGetValue(c, out width))
				return width;
			return CharWidth;
		}

		public static int GetPixelLength(string line)
		{
			var clean = CleanAllFormattingFilter.Replace(line, "");
			var length = clean.Sum(GetCharWidth);

			// +1 for each bold character

			var boldMatches = BoldTextRegex.Matches(line);
			if (boldMatches.Count > 0)
			{
				foreach (Match boldText in boldMatches)
				{
					var cleanBoldText = CleanAllFormattingFilter.Replace(boldText.Value, "");
					length += cleanBoldText.Length;
				}
			}

			return length;
		}

		private static string Strip(string input, bool keepBold = false)
		{
			string result;
			if (keepBold)
			{
				result = CleanColourFilter.Replace(input, "\u1234");
			}
			else
			{
				result = CleanAllFormattingFilter.Replace(input, "\u1236");
			}
			return result;
		}

		public static string RemoveFormatting(string input)
		{
			return CleanAllFormattingFilter.Replace(input, "");
		}
	}
}