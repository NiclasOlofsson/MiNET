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
using System.Security.Cryptography;

namespace MiNET.Utils.Cryptography
{
	public class CryptoRandom : RandomNumberGenerator
	{
		private static RandomNumberGenerator r;

		/// <summary>
		///     Creates an instance of the default implementation of a cryptographic random number generator that can be used to
		///     generate random data.
		/// </summary>
		public CryptoRandom()
		{
			r = Create();
		}

		/// <summary>
		///     Fills the elements of a specified array of bytes with random numbers.
		/// </summary>
		/// <param name=� buffer�>An array of bytes to contain random numbers.</param>
		public override void GetBytes(byte[] buffer)
		{
			r.GetBytes(buffer);
		}

		/// <summary>
		///     Returns a random number between 0.0 and 1.0.
		/// </summary>
		public double NextDouble()
		{
			byte[] b = new byte[4];
			r.GetBytes(b);
			return (double) BitConverter.ToUInt32(b, 0) / UInt32.MaxValue;
		}

		/// <summary>
		///     Returns a random number within the specified range.
		/// </summary>
		/// <param name=� minValue�>The inclusive lower bound of the random number returned.</param>
		/// <param name=� maxValue�>
		///     The exclusive upper bound of the random number returned. maxValue must be greater than or equal
		///     to minValue.
		/// </param>
		public int Next(int minValue, int maxValue)
		{
			return (int) Math.Round(NextDouble() * (maxValue - minValue - 1)) + minValue;
		}

		/// <summary>
		///     Returns a nonnegative random number.
		/// </summary>
		public int Next()
		{
			return Next(0, Int32.MaxValue);
		}

		/// <summary>
		///     Returns a nonnegative random number less than the specified maximum
		/// </summary>
		/// <param name=� maxValue�>
		///     The inclusive upper bound of the random number returned. maxValue must be greater than or equal
		///     0
		/// </param>
		public int Next(int maxValue)
		{
			return Next(0, maxValue);
		}
	}
}