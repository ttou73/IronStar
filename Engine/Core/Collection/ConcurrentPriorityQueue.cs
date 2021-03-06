﻿using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;


namespace Fusion.Core.Collection
{
    [DebuggerDisplay("Count = {Count}")]
    [Serializable]
    public class ConcurrentPriorityQueue<TPriority, TValue> : IProducerConsumerCollection<KeyValuePair<TPriority, TValue>> where TPriority : IComparable<TPriority>
    {

        private List<KeyValuePair<TPriority, TValue>> items;
        private Comparer<TPriority> comparer;
        private object lockObject = new object();

        /// <summary>
        /// 
        /// </summary>
        public ConcurrentPriorityQueue()
        {
            comparer = Comparer<TPriority>.Default;
            items = new List<KeyValuePair<TPriority, TValue>>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comparer"></param>
        public ConcurrentPriorityQueue(Comparer<TPriority> comparer)
        {
            items = new List<KeyValuePair<TPriority, TValue>>();
            this.comparer = comparer ?? Comparer<TPriority>.Default;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="capacity"></param>
        /// <param name="comparer"></param>

        public ConcurrentPriorityQueue(int capacity, Comparer<TPriority> comparer)
        {
            items = new List<KeyValuePair<TPriority, TValue>>(capacity);
            this.comparer = comparer ?? Comparer<TPriority>.Default;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="comparer"></param>
        public ConcurrentPriorityQueue(IEnumerable<TValue> data, IEnumerable<TPriority> priorities, Comparer<TPriority> comparer)
        {
            this.comparer = comparer ?? Comparer<TPriority>.Default;
            var _data = data.ToList();
            var _priorities = priorities.ToList();

            items = new List<KeyValuePair<TPriority, TValue>>(_data.Count);

            for (int i = 0; i < items.Count(); i++)
            {
                Enqueue(_priorities[i], _data[i]);
            }
        }

        /// <summary>
        /// Return amount of items in collection.
        /// </summary>
        public int Count
        {
            get { return items.Count; }
        }

        /// <summary>
        /// Return comparer.
        /// </summary>
        public Comparer<TPriority> Comparer
        {
            get { return comparer; }
        }

        /// <summary>
        /// Clear the collection.
        /// </summary>
        public void Clear()
        {
            lock (lockObject)
            {
                items.Clear();
            }
        }


        /// <summary>
        /// Add element to collection.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public void Enqueue(TPriority priority, TValue element)
        {
            lock (lockObject)
            {
                items.Add(new KeyValuePair<TPriority, TValue>(priority, element));
                SiftUp(Count - 1);
            }
        }


        /// <summary>
        /// Add element to collection.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public void Enqueue(KeyValuePair<TPriority, TValue> item)
        {
            lock (lockObject)
            {
                items.Add(item);
                SiftUp(Count - 1);
            }
        }


        /// <summary>Attempts to add an item in the queue.</summary>
        /// <param name="item">The key/value pair to be added.</param>
        /// <returns>
        /// true if the pair was added; otherwise, false.
        /// </returns>
        bool IProducerConsumerCollection<KeyValuePair<TPriority, TValue>>.TryAdd(KeyValuePair<TPriority, TValue> item)
        {
            Enqueue(item);
            return true;
        }

        /// <summary>Attempts to remove and return the next prioritized item in the queue.</summary>
        /// <param name="item">
        /// When this method returns, if the operation was successful, result contains the object removed. If
        /// no object was available to be removed, the value is unspecified.
        /// </param>
        /// <returns>
        /// true if an element was removed and returned from the queue succesfully; otherwise, false.
        /// </returns>
        bool IProducerConsumerCollection<KeyValuePair<TPriority, TValue>>.TryTake(out KeyValuePair<TPriority, TValue> item)
        {
            return TryDequeue(out item);
        }

        /// <summary>
        /// Return element and priority and remove it from collection.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public bool TryDequeue(out KeyValuePair<TPriority, TValue> element)
        {
            lock (lockObject)
            {
                if (Count == 0)
                {
                    element = default(KeyValuePair<TPriority, TValue>);
                    return false;
                }
                else
                {
                    element = items[0];
                    items[0] = items[Count - 1];
                    items.RemoveAt(Count - 1);
                    SiftDown(0);
                    return true;
                }
            }
        }

        /// <summary>
        /// Return element and remove it from collection.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public bool TryDequeue(out TValue element)
        {
            KeyValuePair<TPriority, TValue> temp;
            bool t = TryDequeue(out temp);
            element = temp.Value;
            return t;
        }

        /// <summary>
        /// Return element and remove it from collection.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        /*public TValue Dequeue()
        {
            TValue temp;
            TryTake(out temp);
            return temp;
        }	  */

        /// <summary>
        /// Return element and priority from collection.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public bool TryPeek(out KeyValuePair<TPriority, TValue> element)
        {
            lock (lockObject)
            {
                if (Count == 0)
                {
                    element = default(KeyValuePair<TPriority, TValue>);
                    return false;
                }
                else
                {
                    element = items[0];
                    return true;
                }
            }
        }

        /// <summary>
        /// Return element and priority from collection.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public bool TryPeek(out TValue element)
        {
            KeyValuePair<TPriority, TValue> temp;
            bool t = TryPeek(out temp);
            element = temp.Value;
            return t;
        }


        public void CopyTo(KeyValuePair<TPriority, TValue>[] array, int index)
        {
            lock (lockObject)
            {
                items.CopyTo(array, index);
            }
        }

        public KeyValuePair<TPriority, TValue>[] ToArray()
        {
            return items.ToArray();
        }



        public IEnumerator<KeyValuePair<TPriority, TValue>> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool IsSynchronized
        {
            get { return true; }
        }

        public object SyncRoot
        {
            get { return lockObject; }
        }


        /// <summary>
        /// Sift down element in heap.
        /// </summary>
        /// <param name="index"></param>
        private void SiftUp(int index)
        {
            while (index != 0 && comparer.Compare(items[index].Key, items[(index - 1) / 2].Key) < 0)
            {
                var t = items[index];
                items[index] = items[(index - 1) / 2];
                items[(index - 1) / 2] = t;
                index = (index - 1) / 2;
            }
        }


        /// <summary>
        /// Sift up element in heap.
        /// </summary>
        /// <param name="index"></param>
        private void SiftDown(int index)
        {
            while (index * 2 + 1 < Count)
            {
                int l = 2 * index + 1;
                int r = 2 * index + 2;
                int swapPosition = l;
                if (r < Count)
                {
                    if (comparer.Compare(items[l].Key, items[r].Key) > 0)
                    {
                        swapPosition = r;
                    }
                }
                if (comparer.Compare(items[index].Key, items[swapPosition].Key) >= 0)
                {
                    var t = items[index];
                    items[index] = items[swapPosition];
                    items[swapPosition] = t;
                    index = swapPosition;
                }
                else
                {
                    break;
                }
            }
        }


    }
}
