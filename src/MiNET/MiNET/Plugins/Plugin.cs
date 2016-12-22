using System;

namespace MiNET.Plugins
{
	public abstract class Plugin : IPlugin
	{
		protected PluginContext Context { get; set; }

		[ThreadStatic] public static Player CurrentPlayer = null;

		public void OnEnable(PluginContext context)
		{
			Context = context;
			OnEnable();
		}

		protected virtual void OnEnable()
		{
		}

		public virtual void OnDisable()
		{
		}
	}
}