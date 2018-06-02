namespace MiNET.Blocks
{
	public class MonsterEgg : Block
	{
		public MonsterEgg() : base(97)
		{
			BlastResistance = 3.75f;
			Hardness = 0.75f;
            IsConductive = true;

            // Spawns silverfish on break.	
        }
	}
}