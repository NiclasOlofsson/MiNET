using System;
using System.Collections.Concurrent;

namespace MiNET.Net
{
	public class ObjectPool<T> where T : Package
	{
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

		public void PutObject(T item)
		{
			//GC.SuppressFinalize(item);
			_objects.Add(item);
		}
	}
}