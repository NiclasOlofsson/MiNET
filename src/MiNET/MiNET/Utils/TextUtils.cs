using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MiNET.Utils
{
	public class TextUtils
	{
		private const int LineLength = 30;
		private const int CharWidth = 6;
		private const char SpaceChar = ' ';
		private const int SpaceWidth = 4;


		public static string Center(string input)
		{
			var lines = input.Trim().Split('\n');
			var sortedLines = lines.OrderByDescending(GetPixelLenght);

			string longest = sortedLines.First();
			int maxLenght = GetPixelLenght(longest);

			string result = "";
			foreach (var sortedLine in lines)
			{
				int len = GetPixelLenght(sortedLine);
				int padding = (int) Math.Round((maxLenght - len)/2d/SpaceWidth);
				int paddingRight = (int) Math.Floor((maxLenght - len)/2d/SpaceWidth);
				result += new string(' ', padding) + sortedLine + "" + new string(' ', paddingRight) + '\n';
			}

			return result;
		}

		private static int GetPixelLenght(string line)
		{
			var charWidths = new Dictionary<char, int>
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
				{'─', 9},
				//{'-', 9},
			};

			line = Strip(line, true);

			int lenght = 0;
			bool isBold = false;
			foreach (char c in line)
			{
				if (c == '\u1236')
				{
					isBold = false;
					continue;
				}
				if (c == '\u1234')
				{
					isBold = !isBold;
					continue;
				}

				if (!charWidths.ContainsKey(c))
					lenght += CharWidth;
				else
					lenght += charWidths[c];

				if (isBold) lenght++;
			}

			return lenght;
		}

		public static string Strip(string input, bool keepBold = false)
		{
			string result;
			if (keepBold)
			{
				Regex rgx = new Regex("(?:&|§|\u00a7)([0123456789abcdefkmnor])");
				result = rgx.Replace(input, "\u1236");
				Regex rgxBold = new Regex("(?:&|§|\u00a7)([l])");
				result = rgxBold.Replace(result, "\u1234");
			}
			else
			{
				Regex rgx = new Regex("(?:&|§|\u00a7)([0123456789abcdefklmnor])");
				result = rgx.Replace(input, "\u1236");
			}
			return result;
		}
	}
}