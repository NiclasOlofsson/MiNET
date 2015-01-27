using System;
using System.Collections.Concurrent;

namespace MiNET.Net
{
	public class ObjectPool<T>
	{
		private ConcurrentBag<T> _objects;
		private Func<T> _objectGenerator;
		public bool IsInitialized = false;

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

		public void PutObject(T item)
		{
			//if (IsInitialized && _objects.Count < 10000) Console.WriteLine("Pool for " + item.GetType().Name + " is running low.");
			_objects.Add(item);
		}

		public int Count()
		{
			return _objects.Count;
		}
	}
}