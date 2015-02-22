using MiNET.Worlds;
using System;

namespace MiNET.API
{
	public abstract class MiNETPlugin : IMiNETPlugin
	{
		public MiNETPlugin()
		{
		}

		public virtual void OnEnable(Level level)
		{
			throw new NotImplementedException();
		}

		public virtual void OnDisable()
		{
			throw new NotImplementedException();
		}
	}
}