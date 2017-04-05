namespace MiNET.Entities.Behaviors
{
	public interface IBehavior
	{
		bool ShouldStart();
		bool CanContinue();
		void OnTick();
		void OnEnd();
	}
}