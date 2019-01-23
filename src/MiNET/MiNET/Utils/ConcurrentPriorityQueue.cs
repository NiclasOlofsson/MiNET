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
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;

namespace MiNET.Utils
{
	/// <summary>Provides a thread-safe priority queue data structure.</summary>
	/// <typeparam name="TKey">Specifies the type of keys used to prioritize values.</typeparam>
	/// <typeparam name="TValue">Specifies the type of elements in the queue.</typeparam>
	[DebuggerDisplay("Count={Count}")]
	public class ConcurrentPriorityQueue<TKey, TValue> :
		IProducerConsumerCollection<KeyValuePair<TKey, TValue>>
		where TKey : IComparable<TKey>
	{
		private readonly object _syncLock = new object();
		private readonly MinBinaryHeap _minHeap = new MinBinaryHeap();

		/// <summary>Initializes a new instance of the ConcurrentPriorityQueue class.</summary>
		public ConcurrentPriorityQueue()
		{
		}

		/// <summary>
		///     Initializes a new instance of the ConcurrentPriorityQueue class that contains elements copied from the
		///     specified collection.
		/// </summary>
		/// <param name="collection">The collection whose elements are copied to the new ConcurrentPriorityQueue.</param>
		public ConcurrentPriorityQueue(IEnumerable<KeyValuePair<TKey, TValue>> collection)
		{
			if (collection == null) throw new ArgumentNullException("collection");
			foreach (var item in collection) _minHeap.Insert(item);
		}

		/// <summary>Adds the key/value pair to the priority queue.</summary>
		/// <param name="priority">The priority of the item to be added.</param>
		/// <param name="value">The item to be added.</param>
		public void Enqueue(TKey priority, TValue value)
		{
			Enqueue(new KeyValuePair<TKey, TValue>(priority, value));
		}

		/// <summary>Adds the key/value pair to the priority queue.</summary>
		/// <param name="item">The key/value pair to be added to the queue.</param>
		public void Enqueue(KeyValuePair<TKey, TValue> item)
		{
			lock (_syncLock) _minHeap.Insert(item);
		}

		/// <summary>Attempts to remove and return the next prioritized item in the queue.</summary>
		/// <param name="result">
		///     When this method returns, if the operation was successful, result contains the object removed. If
		///     no object was available to be removed, the value is unspecified.
		/// </param>
		/// <returns>
		///     true if an element was removed and returned from the queue succesfully; otherwise, false.
		/// </returns>
		public bool TryDequeue(out KeyValuePair<TKey, TValue> result)
		{
			result = default(KeyValuePair<TKey, TValue>);
			lock (_syncLock)
			{
				if (_minHeap.Count > 0)
				{
					result = _minHeap.Remove();
					return true;
				}
			}
			return false;
		}

		/// <summary>Attempts to return the next prioritized item in the queue.</summary>
		/// <param name="result">
		///     When this method returns, if the operation was successful, result contains the object.
		///     The queue was not modified by the operation.
		/// </param>
		/// <returns>
		///     true if an element was returned from the queue succesfully; otherwise, false.
		/// </returns>
		public bool TryPeek(out KeyValuePair<TKey, TValue> result)
		{
			result = default(KeyValuePair<TKey, TValue>);
			lock (_syncLock)
			{
				if (_minHeap.Count > 0)
				{
					result = _minHeap.Peek();
					return true;
				}
			}
			return false;
		}

		/// <summary>Empties the queue.</summary>
		public void Clear()
		{
			lock (_syncLock) _minHeap.Clear();
		}

		/// <summary>Gets whether the queue is empty.</summary>
		public bool IsEmpty
		{
			get { return Count == 0; }
		}

		/// <summary>Gets the number of elements contained in the queue.</summary>
		public int Count
		{
			get
			{
				lock (_syncLock) return _minHeap.Count;
			}
		}

		/// <summary>Copies the elements of the collection to an array, starting at a particular array index.</summary>
		/// <param name="array">
		///     The one-dimensional array that is the destination of the elements copied from the queue.
		/// </param>
		/// <param name="index">
		///     The zero-based index in array at which copying begins.
		/// </param>
		/// <remarks>The elements will not be copied to the array in any guaranteed order.</remarks>
		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
		{
			lock (_syncLock) _minHeap.Items.CopyTo(array, index);
		}

		/// <summary>Copies the elements stored in the queue to a new array.</summary>
		/// <returns>A new array containing a snapshot of elements copied from the queue.</returns>
		public KeyValuePair<TKey, TValue>[] ToArray()
		{
			lock (_syncLock)
			{
				var clonedHeap = new MinBinaryHeap(_minHeap);
				var result = new KeyValuePair<TKey, TValue>[_minHeap.Count];
				for (int i = 0; i < result.Length; i++)
				{
					result[i] = clonedHeap.Remove();
				}
				return result;
			}
		}

		/// <summary>Attempts to add an item in the queue.</summary>
		/// <param name="item">The key/value pair to be added.</param>
		/// <returns>
		///     true if the pair was added; otherwise, false.
		/// </returns>
		bool IProducerConsumerCollection<KeyValuePair<TKey, TValue>>.TryAdd(KeyValuePair<TKey, TValue> item)
		{
			Enqueue(item);
			return true;
		}

		/// <summary>Attempts to remove and return the next prioritized item in the queue.</summary>
		/// <param name="item">
		///     When this method returns, if the operation was successful, result contains the object removed. If
		///     no object was available to be removed, the value is unspecified.
		/// </param>
		/// <returns>
		///     true if an element was removed and returned from the queue succesfully; otherwise, false.
		/// </returns>
		bool IProducerConsumerCollection<KeyValuePair<TKey, TValue>>.TryTake(out KeyValuePair<TKey, TValue> item)
		{
			return TryDequeue(out item);
		}

		/// <summary>Returns an enumerator that iterates through the collection.</summary>
		/// <returns>An enumerator for the contents of the queue.</returns>
		/// <remarks>
		///     The enumeration represents a moment-in-time snapshot of the contents of the queue. It does not
		///     reflect any updates to the collection after GetEnumerator was called. The enumerator is safe to
		///     use concurrently with reads from and writes to the queue.
		/// </remarks>
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			var arr = ToArray();
			return ((IEnumerable<KeyValuePair<TKey, TValue>>) arr).GetEnumerator();
		}

		/// <summary>Returns an enumerator that iterates through a collection.</summary>
		/// <returns>An IEnumerator that can be used to iterate through the collection.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <summary>Copies the elements of the collection to an array, starting at a particular array index.</summary>
		/// <param name="array">
		///     The one-dimensional array that is the destination of the elements copied from the queue.
		/// </param>
		/// <param name="index">
		///     The zero-based index in array at which copying begins.
		/// </param>
		void ICollection.CopyTo(Array array, int index)
		{
			lock (_syncLock) ((ICollection) _minHeap.Items).CopyTo(array, index);
		}

		/// <summary>
		///     Gets a value indicating whether access to the ICollection is synchronized with the SyncRoot.
		/// </summary>
		bool ICollection.IsSynchronized
		{
			get { return true; }
		}

		/// <summary>
		///     Gets an object that can be used to synchronize access to the collection.
		/// </summary>
		object ICollection.SyncRoot
		{
			get { return _syncLock; }
		}

		/// <summary>Implements a binary heap that prioritizes smaller values.</summary>
		private sealed class MinBinaryHeap
		{
			private readonly List<KeyValuePair<TKey, TValue>> _items;

			/// <summary>Initializes an empty heap.</summary>
			public MinBinaryHeap()
			{
				_items = new List<KeyValuePair<TKey, TValue>>();
			}

			/// <summary>Initializes a heap as a copy of another heap instance.</summary>
			/// <param name="heapToCopy">The heap to copy.</param>
			/// <remarks>Key/Value values are not deep cloned.</remarks>
			public MinBinaryHeap(MinBinaryHeap heapToCopy)
			{
				_items = new List<KeyValuePair<TKey, TValue>>(heapToCopy.Items);
			}

			/// <summary>Empties the heap.</summary>
			public void Clear()
			{
				_items.Clear();
			}

			/// <summary>Adds an item to the heap.</summary>
			public void Insert(TKey key, TValue value)
			{
				// Create the entry based on the provided key and value
				Insert(new KeyValuePair<TKey, TValue>(key, value));
			}

			/// <summary>Adds an item to the heap.</summary>
			public void Insert(KeyValuePair<TKey, TValue> entry)
			{
				// Add the item to the list, making sure to keep track of where it was added.
				_items.Add(entry);
				int pos = _items.Count - 1;

				// If the new item is the only item, we're done.
				if (pos == 0) return;

				// Otherwise, perform log(n) operations, walking up the tree, swapping
				// where necessary based on key values
				while (pos > 0)
				{
					// Get the next position to check
					int nextPos = (pos - 1) / 2;

					// Extract the entry at the next position
					var toCheck = _items[nextPos];

					// Compare that entry to our new one.  If our entry has a smaller key, move it up.
					// Otherwise, we're done.
					if (entry.Key.CompareTo(toCheck.Key) < 0)
					{
						_items[pos] = toCheck;
						pos = nextPos;
					}
					else break;
				}

				// Make sure we put this entry back in, just in case
				_items[pos] = entry;
			}

			/// <summary>Returns the entry at the top of the heap.</summary>
			public KeyValuePair<TKey, TValue> Peek()
			{
				// Returns the first item
				if (_items.Count == 0) throw new InvalidOperationException("The heap is empty.");
				return _items[0];
			}

			/// <summary>Removes the entry at the top of the heap.</summary>
			public KeyValuePair<TKey, TValue> Remove()
			{
				// Get the first item and save it for later (this is what will be returned).
				if (_items.Count == 0) throw new InvalidOperationException("The heap is empty.");
				KeyValuePair<TKey, TValue> toReturn = _items[0];

				// Remove the first item if there will only be 0 or 1 items left after doing so.  
				if (_items.Count <= 2) _items.RemoveAt(0);
				// A reheapify will be required for the removal
				else
				{
					// Remove the first item and move the last item to the front.
					_items[0] = _items[_items.Count - 1];
					_items.RemoveAt(_items.Count - 1);

					// Start reheapify
					int current = 0, possibleSwap = 0;

					// Keep going until the tree is a heap
					while (true)
					{
						// Get the positions of the node's children
						int leftChildPos = 2 * current + 1;
						int rightChildPos = leftChildPos + 1;

						// Should we swap with the left child?
						if (leftChildPos < _items.Count)
						{
							// Get the two entries to compare (node and its left child)
							var entry1 = _items[current];
							var entry2 = _items[leftChildPos];

							// If the child has a lower key than the parent, set that as a possible swap
							if (entry2.Key.CompareTo(entry1.Key) < 0) possibleSwap = leftChildPos;
						}
						else break; // if can't swap this, we're done

						// Should we swap with the right child?  Note that now we check with the possible swap
						// position (which might be current and might be left child).
						if (rightChildPos < _items.Count)
						{
							// Get the two entries to compare (node and its left child)
							var entry1 = _items[possibleSwap];
							var entry2 = _items[rightChildPos];

							// If the child has a lower key than the parent, set that as a possible swap
							if (entry2.Key.CompareTo(entry1.Key) < 0) possibleSwap = rightChildPos;
						}

						// Now swap current and possible swap if necessary
						if (current != possibleSwap)
						{
							var temp = _items[current];
							_items[current] = _items[possibleSwap];
							_items[possibleSwap] = temp;
						}
						else break; // if nothing to swap, we're done

						// Update current to the location of the swap
						current = possibleSwap;
					}
				}

				// Return the item from the heap
				return toReturn;
			}

			/// <summary>Gets the number of objects stored in the heap.</summary>
			public int Count
			{
				get { return _items.Count; }
			}

			internal List<KeyValuePair<TKey, TValue>> Items
			{
				get { return _items; }
			}
		}
	}
}