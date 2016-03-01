using System;

namespace MiNET.Utils
{
	// Credit to the wannabe math-dude slikey (@Hypixel) for showing me
	// this amazingly pointless, but effective, code. My life feel so much 
	// more complete at this point.
	public class WannabeRandom
	{
		private long _seed; // No, you can't harvest the result from this one. Go play Minecraft instead.

		public WannabeRandom():this(DateTime.UtcNow.Ticks) // Yeah this is the way to go.
		{
		}

		public WannabeRandom(long seed) // Yeah, if you wannabe a Minecraft farmer and give pumpkin seeds instead.
		{
			_seed = seed;
		}

		protected long Next(int nbits) // This only likes bits, not pieces.
		{
			long x = _seed;
			x ^= x << 21;
			x ^= (long) ((ulong) x >> 35); // Academic coolness. In Java it's >>>
			x ^= x << 4;
			_seed = x;
			return x & (1L << nbits) - 1; // In java they actually care about variable assignments..
		}

		public int NextInt(int bound) // You have to ask slikey about the name of that parameter. Sounds kinky ...
		{
			return (int) Next(31)%bound; // Slikey wanted his random postive, so do I.
		}

		public long NextLong() // ... said the girl.
		{
			return Next(63); // 64 would give random from - to +. Don't want that.
		}

		public double NextDouble() // .. said I.
		{
			return NextInt(32)*(1.0/int.MaxValue);
		}
	}
}