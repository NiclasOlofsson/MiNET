using System;
using System.Collections.Concurrent;
using log4net;

namespace MiNET.Net
{
	public class ObjectPool<T> where T : Package
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (ObjectPool<T>));

		private ConcurrentQueue<T> _objects;
		//private ConcurrentBag<T> _objects;
		private Func<T> _objectGenerator;
		public int Size => _objects.Count;

		public void FillPool(int count)
		{
			for (int i = 0; i < count; i++)
			{
				var item = _objectGenerator();
				_objects.Enqueue(item);
			}
		}

		public ObjectPool(Func<T> objectGenerator)
		{
			if (objectGenerator == null) throw new ArgumentNullException("objectGenerator");
			//_objects = new ConcurrentBag<T>();
			_objects = new ConcurrentQueue<T>();
			_objectGenerator = objectGenerator;
		}

		public T GetObject()
		{
			T item;
			if (_objects.TryDequeue(out item)) return item;
			return _objectGenerator();
		}

		const long MaxPoolSize = 10000000;

		public void PutObject(T item)
		{
			//if (_objects.Count > MaxPoolSize)
			//{
			//	Log.Warn($"Pool for {typeof (T).Name} is full");
			//	return;
			//}

			_objects.Enqueue(item);
		}
	}
}