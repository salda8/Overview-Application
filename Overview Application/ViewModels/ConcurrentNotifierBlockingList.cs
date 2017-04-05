using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using System.Windows.Threading;

namespace OverviewApp.ViewModels
{
    public class ConcurrentNotifierBlockingList<T> : IEnumerable, INotifyCollectionChanged
    {
        private readonly Dictionary<Delegate, Thread> handlerThreads;
        private readonly object lockObj = new object();
        public List<T> Collection;

        public ConcurrentNotifierBlockingList()
        {
            Collection = new List<T>();
            handlerThreads = new Dictionary<Delegate, Thread>();
        }

        public T this[int index]
        {
            get
            {
                lock (lockObj)
                {
                    return Collection[index];
                }
            }

            set
            {
                lock (lockObj)
                {
                    Collection[index] = value;
                }
            }
        }

        #region IEnumerable Members

        /// <summary>
        ///     Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator GetEnumerator()
        {
            return Collection.GetEnumerator();
        }

        #endregion

        #region INotifyCollectionChanged Members

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add { handlerThreads.Add(value, Thread.CurrentThread); }
            remove { handlerThreads.Remove(value); }
        }

        #endregion

        public bool TryAdd(T value, int timeout = -1)
        {
            var res = false;
            if (timeout == -1)
            {
                lock (lockObj)
                {
                    Collection.Add(value);
                    res = true;
                }
            }
            else
            {
                if (Monitor.TryEnter(lockObj, timeout))
                {
                    try
                    {
                        Collection.Add(value);
                        res = true;
                    }
                    finally
                    {
                        Monitor.Exit(lockObj);
                    }
                }
            }

            if (res)
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value));
            return res;
        }

        public bool TryRemove(T value, int timeout = -1)
        {
            var res = false;
            var index = 0;
            if (timeout == -1)
            {
                lock (lockObj)
                {
                    index = Collection.IndexOf(value);
                    if (index >= 0)
                    {
                        Collection.RemoveAt(index);
                        res = true;
                    }
                }
            }
            else
            {
                if (Monitor.TryEnter(lockObj, timeout))
                {
                    try
                    {
                        index = Collection.IndexOf(value);
                        if (index >= 0)
                        {
                            Collection.RemoveAt(index);
                            res = true;
                        }
                    }
                    finally
                    {
                        Monitor.Exit(lockObj);
                    }
                }
            }

            if (res)
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, value,
                    index));
            return res;
        }

        protected void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            foreach (Delegate handler in handlerThreads.Keys)
            {
                var dispatcher = Dispatcher.FromThread(handlerThreads[handler]);
                dispatcher?.Invoke(DispatcherPriority.Send, handler, this, e);
            }
        }
    }
}