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

using System.Collections.Concurrent;
using System.Collections.Generic;
using MiNET.Net;
using MiNET.Utils;

namespace MiNET.Entities.ImageProviders
{
	public class FrameTicker
	{
		public int NumberOfFrames { get; set; }
		public int CurrentFrame { private get; set; }

		private object _frameSync = new object();
		private ConcurrentDictionary<object, bool> _providers = new ConcurrentDictionary<object, bool>();

		public FrameTicker(int numberOfFrames)
		{
			NumberOfFrames = numberOfFrames;
		}


		public void Register(object provider)
		{
			_providers.TryAdd(provider, false);
		}

		public int GetCurrentFrame(object caller)
		{
			lock (_frameSync)
			{
				if (ShouldTick())
				{
					foreach (var provider in _providers)
					{
						_providers[provider.Key] = false;
					}

					if (CurrentFrame >= NumberOfFrames) CurrentFrame = 0;
					else CurrentFrame++;
				}

				_providers[caller] = true;

				return CurrentFrame;
			}
		}

		public bool ShouldTick()
		{
			bool shouldTick = true;
			foreach (var kvp in _providers)
			{
				shouldTick = shouldTick && (kvp.Value);
			}

			return shouldTick;
		}
	}

	public class VideoImageProvider : IMapImageProvider
	{
		public List<McpeWrapper> Frames { get; set; } = new List<McpeWrapper>();

		public FrameTicker FrameTicker { get; set; }

		public VideoImageProvider(FrameTicker frameTicker)
		{
			FrameTicker = frameTicker;
			FrameTicker.Register(this);
		}

		public virtual byte[] GetData(MapInfo mapInfo, bool forced)
		{
			return null;
			//return Frames.Count == 0 ? new byte[mapInfo.Col*mapInfo.Row*4] : null;
		}

		public McpeClientboundMapItemData GetClientboundMapItemData(MapInfo mapInfo)
		{
			return null;
		}

		public McpeWrapper GetBatch(MapInfo mapInfo, bool forced)
		{
			if (Frames.Count == 0) return null;

			var currentFrame = FrameTicker.GetCurrentFrame(this);
			if (currentFrame >= Frames.Count) return null;

			return Frames[currentFrame];
		}
	}
}