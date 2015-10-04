// *****************************************************************************
// 
//  © Component Factory Pty Ltd 2011. All rights reserved.
//	The software and associated documentation supplied hereunder are the 
//  proprietary information of Component Factory Pty Ltd, 17/267 Nepean Hwy, 
//  Seaford, Vic 3198, Australia and are supplied subject to licence terms.
// 
//  Version 1.0.8.0 	www.ComponentFactory.com
// *****************************************************************************

using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;

namespace ComponentFactory.Quicksilver.Layout
{
    /// <summary>
    /// Manage a collection of grid definition instances.
    /// </summary>
    public partial class DefinitionCollection<T> : IList,
                                                   IList<T>,
                                                   ICollection,
                                                   ICollection<T>,
                                                   IEnumerable,
                                                   IEnumerable<T>
                                                   where T : BaseDefinition
    {
        #region Instance Fields
        private ILogicalParent _logicalParent;
        private List<T> _list;
        private bool _dirty;
        #endregion

        #region Events
        /// <summary>
        /// Occurs when the element requires a measure.
        /// </summary>
        public event EventHandler NeedMeasure;

        /// <summary>
        /// Occurs when the contents of the collection changes.
        /// </summary>
        public event EventHandler CollectionChanged;
        #endregion

        #region Identity
        /// <summary>
        /// Initialize a new instance of the DefinitionCollection class.
        /// </summary>
        protected DefinitionCollection()
        {
            _list = new List<T>();
            _dirty = true;
        }
        #endregion

        #region IList Members
        /// <summary>
        /// Append an item to the collection.
        /// </summary>
        /// <param name="value">Object reference.</param>
        /// <returns>The position into which the new item was inserted.</returns>
        public int Add(object value)
        {
            if (value is T)
            {
                Add(value as T);
                return (Count - 1);
            }
            else
                throw new ArgumentException("Parameter is incorrect type.");
        }

        /// <summary>
        /// Determines whether the collection contains the item.
        /// </summary>
        /// <param name="value">Object reference.</param>
        /// <returns>True if item found; otherwise false.</returns>
        public bool Contains(object value)
        {
            if (value is T)
                return Contains(value as T);
            else
                throw new ArgumentException("Parameter is incorrect type.");
        }

        /// <summary>
        /// Determines the index of the specified item in the collection.
        /// </summary>
        /// <param name="value">Object reference.</param>
        /// <returns>-1 if not found; otherwise index position.</returns>
        public int IndexOf(object value)
        {
            if (value is T)
                return IndexOf(value as T);
            else
                throw new ArgumentException("Parameter is incorrect type.");
        }

        /// <summary>
        /// Inserts an item to the collection at the specified index.
        /// </summary>
        /// <param name="index">Insert index.</param>
        /// <param name="value">Object reference.</param>
        public void Insert(int index, object value)
        {
            if (value is T)
                Insert(index, value as T);
            else
                throw new ArgumentException("Parameter is incorrect type.");
        }

        /// <summary>
        /// Gets a value indicating whether the collection has a fixed size. 
        /// </summary>
        public bool IsFixedSize
        {
            get { return false; }
        }

        /// <summary>
        /// Removes first occurance of specified item.
        /// </summary>
        /// <param name="value">Object reference.</param>
        public void Remove(object value)
        {
            if (value is T)
                Remove(value as T);
            else
                throw new ArgumentException("Parameter is incorrect type.");
        }

        /// <summary>
        /// Gets or sets the item at the specified index.
        /// </summary>
        /// <param name="index">Object index.</param>
        /// <returns>Object at specified index.</returns>
        object IList.this[int index]
        {
            get { return _list[index]; }

            set
            {
                throw new NotImplementedException("Cannot set a collection index with a new value.");
            }
        }
        #endregion

        #region IList<T>
        /// <summary>
        /// Determines the index of the specified item in the collection.
        /// </summary>
        /// <param name="item">Item reference.</param>
        /// <returns>-1 if not found; otherwise index position.</returns>
        public int IndexOf(T item)
        {
            if (item is T)
                return _list.IndexOf(item);
            else
                throw new ArgumentException("Parameter is incorrect type.");
        }

        /// <summary>
        /// Inserts an item to the collection at the specified index.
        /// </summary>
        /// <param name="index">Insert index.</param>
        /// <param name="item">Item reference.</param>
        public void Insert(int index, T item)
        {
            if (item is T)
            {
                _list.Insert(index, item);
                _dirty = true;
                item.AddToLogicalTree(_logicalParent);
                item.NeedMeasure += new EventHandler(OnNeedMeasure);
                OnCollectionChanged(EventArgs.Empty);
            }
            else
                throw new ArgumentException("Parameter is incorrect type.");
        }

        /// <summary>
        /// Removes the item at the specified index.
        /// </summary>
        /// <param name="index">Remove index.</param>
        public void RemoveAt(int index)
        {
            T child = this[index];
            _list.RemoveAt(index);
            _dirty = true;
            child.RemoveFromLogicalTree(_logicalParent);
            child.NeedMeasure -= new EventHandler(OnNeedMeasure);
            OnCollectionChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Gets or sets the item at the specified index.
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <returns>Item at specified index.</returns>
        public T this[int index]
        {
            get { return _list[index]; }

            set
            {
                throw new NotImplementedException("Cannot set a collection index with a new value.");
            }
        }
        #endregion

        #region ICollection
        /// <summary>
        /// Copies all the elements of the current collection to the specified Array. 
        /// </summary>
        /// <param name="array">The Array that is the destination of the elements copied from the collection.</param>
        /// <param name="index">The index in array at which copying begins.</param>
        public void CopyTo(Array array, int index)
        {
            _list.CopyTo(array as T[], index);
        }

        /// <summary>
        /// Gets a value indicating whether access to the collection is synchronized (thread safe).
        /// </summary>
        public bool IsSynchronized
        {
            get { return false; }
        }

        /// <summary>
        /// Gets an object that can be used to synchronize access to the collection. 
        /// </summary>
        public object SyncRoot
        {
            get { return _list; }
        }
        #endregion

        #region ICollection<T>
        /// <summary>
        /// Append an item to the collection.
        /// </summary>
        /// <param name="item">Item reference.</param>
        public void Add(T item)
        {
            if (item is T)
            {
                _list.Add(item);
                _dirty = true;
                item.AddToLogicalTree(_logicalParent);
                item.NeedMeasure += new EventHandler(OnNeedMeasure);
                OnCollectionChanged(EventArgs.Empty);
            }
            else
                throw new ArgumentException("Parameter is incorrect type.");
        }

        /// <summary>
        /// Remove all items from the collection.
        /// </summary>
        public void Clear()
        {
            foreach (T child in this)
            {
                child.RemoveFromLogicalTree(_logicalParent);
                child.NeedMeasure -= new EventHandler(OnNeedMeasure);
            }

            _list.Clear();
            _dirty = true;
            OnCollectionChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Determines whether the collection contains the item.
        /// </summary>
        /// <param name="item">Item reference.</param>
        /// <returns>True if item found; otherwise false.</returns>
        public bool Contains(T item)
        {
            if (item is T)
                return _list.Contains(item);
            else
                throw new ArgumentException("Parameter is incorrect type.");
        }

        /// <summary>
        /// Copies items to specified array starting at particular index.
        /// </summary>
        /// <param name="array">Target array.</param>
        /// <param name="arrayIndex">Starting array index.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the number of items in collection.
        /// </summary>
        public int Count
        {
            get { return _list.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether the collection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes first occurance of specified item.
        /// </summary>
        /// <param name="item">Item reference.</param>
        /// <returns>True if removed; otherwise false.</returns>
        public bool Remove(T item)
        {
            if (item is T)
            {
                bool ret = _list.Remove(item);
                if (ret)
                {
                    _dirty = true;
                    item.RemoveFromLogicalTree(_logicalParent);
                    item.NeedMeasure -= new EventHandler(OnNeedMeasure);
                    OnCollectionChanged(EventArgs.Empty);
                }
                return ret;
            }
            else
                throw new ArgumentException("Parameter is incorrect type.");
        }
        #endregion

        #region IEnumerable<T>
        /// <summary>
        /// Shallow enumerate over items in the collection.
        /// </summary>
        /// <returns>Enumerator instance.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }
        #endregion

        #region IEnumerable
        /// <summary>
        /// Enumerate using non-generic interface.
        /// </summary>
        /// <returns>Enumerator instance.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
        #endregion

        #region Protected
        /// <summary>
        /// Raises the NeedMeasure event.
        /// </summary>
        /// <param name="sender">Source instance.</param>
        /// <param name="e">An EventArgs containing the event data.</param>
        protected virtual void OnNeedMeasure(object sender, EventArgs e)
        {
            EventHandler handler = NeedMeasure;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Raises the CollectionChanged event.
        /// </summary>
        /// <param name="e">An EventArgs containing the event data.</param>
        protected virtual void OnCollectionChanged(EventArgs e)
        {
            EventHandler handler = CollectionChanged;
            if (handler != null)
                handler(this, e);
        }
        #endregion

        #region Internal
        internal bool IsDirty
        {
            get 
            {
                bool isDirty = _dirty;
                _dirty = false;
                return isDirty; 
            }
        }

        internal ILogicalParent LogicalParent
        {
            get { return _logicalParent; }
            set { _logicalParent = value; }
        }
        #endregion
    }
}
