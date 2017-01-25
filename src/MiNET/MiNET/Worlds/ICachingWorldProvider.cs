using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.Worlds
{
	public interface ICachingWorldProvider
	{

		ChunkColumn[] GetCachedChunks();

		void ClearCachedChunks();

	}
}
