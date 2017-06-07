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