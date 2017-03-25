namespace MiNET.Entities.Behaviors
{
	public interface IBehavior
	{
		bool ShouldStart(Entity entity);
		bool OnTick(Entity entity);
		bool CalculateNextMove(Entity entity);
	}
}