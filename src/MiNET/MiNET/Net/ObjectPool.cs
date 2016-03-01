using System;
using System.Collections.Concurrent;
using log4net;

namespace MiNET.Net
{
	public class ObjectPool<T> where T : Package
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (ObjectPool<T>));

		private ConcurrentBag<T> _objects;
		private Func<T> _objectGenerator;

		public void FillPool(int count)
		{
			for (int i = 0; i < count; i++)
			{
				var item = _objectGenerator();
				//GC.SuppressFinalize(item);
				_objects.Add(item);
			}
		}

		public ObjectPool(Func<T> objectGenerator)
		{
			if (objectGenerator == null) throw new ArgumentNullException("objectGenerator");
			_objects = new ConcurrentBag<T>();
			_objectGenerator = objectGenerator;
		}

		public T GetObject()
		{
			T item;
			if (_objects.TryTake(out item)) return item;
			return _objectGenerator();
		}

		const long MaxPoolSize = 100;

		public void PutObject(T item)
		{
			//GC.SuppressFinalize(item);
			if (_objects.Count > MaxPoolSize)
			{
				return;
			}

			_objects.Add(item);
		}
	}
}