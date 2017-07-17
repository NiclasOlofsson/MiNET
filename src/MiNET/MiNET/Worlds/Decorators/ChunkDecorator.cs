namespace MiNET.Worlds.Decorators
{
	public abstract class ChunkDecorator
	{
		protected int Seed { get; private set; }
		public bool RunPerBlock { get; protected set; } = true;
		protected ChunkDecorator()
		{

		}

		public void SetSeed(int seed)
		{
			Seed = seed;
			InitSeed(seed);
		}

		protected abstract void InitSeed(int seed);

		public abstract void Decorate(ChunkColumn column, Biome biome, float[] thresholdMap, int x, int y, int z, bool surface, bool isBelowMaxHeight);
		//public virtual void Decorate(ChunkColumn column) { }
	}
}
