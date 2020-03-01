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

using System;
using System.Collections.Concurrent;
using log4net;

namespace MiNET.Net
{
	public class ObjectPool<T> where T : Packet
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(ObjectPool<T>));

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
			if (_objects.TryDequeue(out T item)) return item;
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