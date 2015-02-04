namespace MiNET.API
{
	public interface IMiNETPlugin
	{
		/// <summary>
		///     This function will be called on plugin initialization.
		/// </summary>
		void OnEnable();

		/// <summary>
		///     This function will be called when the plugin will be disabled.s
		/// </summary>
		void OnDisable();
	}
}