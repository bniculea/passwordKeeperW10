using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Common
{
    public class ObservableRangeCollection<T> : ObservableCollection<T>
    {
        public ObservableRangeCollection() : base()
        {
            
        }

        public ObservableRangeCollection(IEnumerable<T> collection): base(collection)
        {
            
        }

        /// <summary>
        /// Adds the elements of the specified collection to the end of the ObservableCollection
        /// </summary>
        /// <param name="collectionEnumerable"></param>
        public void AddRange(IEnumerable<T> collectionEnumerable)
        {
            if(collectionEnumerable == null) throw new ArgumentNullException(nameof(collectionEnumerable));
            foreach (T item in collectionEnumerable)
            {
                Items.Add(item);
            }
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add,
                collectionEnumerable as List<T>));
        }

        public void RemoveRange(IEnumerable<T> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            foreach (T item in collection)
            {
                Items.Remove(item);
            }
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, collection as List<T>));
        }

        /// <summary> 
        /// Clears the current collection and replaces it with the specified item. 
        /// </summary> 
        public void Replace(T item)
        {
            ReplaceRange(new T[] {item});
        }

        /// <summary> 
        /// Clears the current collection and replaces it with the specified collection. 
        /// </summary> 
        public void ReplaceRange(IEnumerable<T> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            Items.Clear();
            foreach (var i in collection) Items.Add(i);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}
