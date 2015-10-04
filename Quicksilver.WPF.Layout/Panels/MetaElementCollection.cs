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
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows.Media;
using System.Reflection;

namespace ComponentFactory.Quicksilver.Layout
{
    /// <summary>
    /// Provides custom handling of the UIElement collection for a MetaPanel.
    /// </summary>
    public class MetaElementCollection : UIElementCollection
    {
        #region Classes
        private class CompoundMetaElementCollection : ICollection
        {
            #region Classes
            private class CompoundEnumerator : IEnumerator
            {
                #region Instance Fields
                private MetaElementCollection _collection;
                private int _index;
                #endregion

                #region Identity
                /// <summary>
                /// Initialize a new instance of the CompoundEnumerator class.
                /// </summary>
                /// <param name="collection">Reference to collection.</param>
                public CompoundEnumerator(MetaElementCollection collection)
                {
                    _collection = collection;
                    _index = -1;
                }
                #endregion

                #region IEnumerator
                /// <summary>
                /// Gets the current element in the collection.
                /// </summary>
                public object Current
                {
                    get
                    {
                        int count = _collection.InternalCount + _collection.ExternalCount;
                        if ((_index >= 0) && (_index < count))
                        {
                            if (_index < _collection.ExternalCount)
                                return _collection[_index];
                            else
                                return _collection.GetInternalChild(_index - _collection.ExternalCount);
                        }
                        else
                            throw new IndexOutOfRangeException();
                    }
                }

                /// <summary>
                /// Advances the enumerator to the next element of the collection.
                /// </summary>
                /// <returns>True if the enumerator was successfully advanced to the next element; False if the enumerator has passed the end of the collection.</returns>
                public bool MoveNext()
                {
                    _index++;
                    return (_index < (_collection.InternalCount + _collection.ExternalCount));
                }

                /// <summary>
                /// Sets the enumerator to its initial position, which is before the first element in the collection.
                /// </summary>
                public void Reset()
                {
                    _index = -1;
                }
                #endregion
            }
            #endregion

            #region Instance Fields
            private MetaElementCollection _collection;
            #endregion

            #region Identity
            /// <summary>
            /// Initialize a new instance of the CompoundMetaElementCollection class.
            /// </summary>
            /// <param name="collection">Reference to collection.</param>
            public CompoundMetaElementCollection(MetaElementCollection collection)
            {
                _collection = collection;
            }
            #endregion

            #region ICollection
            /// <summary>
            /// Copies the elements from the collection to the array.
            /// </summary>
            /// <param name="array">Array that is the destination for copying.</param>
            /// <param name="index">Zero-based index in the array at which copying begins.</param>
            public void CopyTo(Array array, int index)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Returns the number of elements in the collection.
            /// </summary>
            public int Count
            {
                get { return _collection.InternalCount + _collection.ExternalCount; }
            }

            /// <summary>
            /// Gets a value indicating whether access to the collection is synchronized.
            /// </summary>
            public bool IsSynchronized
            {
                get { return _collection.IsSynchronized; }
            }

            /// <summary>
            /// Gets an object that can be used to synchronize access to the collection.
            /// </summary>
            public object SyncRoot
            {
                get { return _collection.SyncRoot; }
            }

            /// <summary>
            /// Returns an enumerator that iterates through a collection.
            /// </summary>
            /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
            public IEnumerator GetEnumerator()
            {
                return new CompoundEnumerator(_collection);
            }
            #endregion
        }
        #endregion

        #region Instance Fields
        private bool _isItemsHost;
        private UIElement _visualParent;
        private FrameworkElement _logicalParent;
        private List<UIElement> _externalChildren;
        private VisualCollection _internalChildren;
        #endregion

        #region Events
        /// <summary>
        /// Occurs whenever one or more elements have been added.
        /// </summary>
        public event EventHandler<UIElementsEventArgs> UIElementsAdded;

        /// <summary>
        /// Occurs whenever one or more elements are requested to be removed.
        /// </summary>
        public event EventHandler<UIElementsEventArgs> UIElementsRemove;
        #endregion

        #region Identity
        /// <summary>
        /// Initialize a new instance of the MetaElementCollection class.
        /// </summary>
        /// <param name="visualParent">The UIElement parent of the collection.</param>
        /// <param name="logicalParent">The logical parent of the elements in the collection.</param>
        public MetaElementCollection(UIElement visualParent,
                                     FrameworkElement logicalParent)
            : base(visualParent, logicalParent)
        {
            _isItemsHost = false;

            _visualParent = visualParent;
            _logicalParent = logicalParent;

            _externalChildren = new List<UIElement>();
            _internalChildren = new VisualCollection(visualParent);
        }
        #endregion

        #region Public
        /// <summary>
        /// Returns the number of elements in the collection.
        /// </summary>
        public override int Count
        {
            get
            {
                if (_isItemsHost)
                    return base.Count;
                else
                    return _externalChildren.Count;
            }
        }

        /// <summary>
        /// Determines whether the collection contains a specific element.
        /// </summary>
        /// <param name="element">The UIElement to locate in the collection.</param>
        /// <returns>True if the element is found; otherwise false.</returns>
        public override bool Contains(UIElement element)
        {
            if (_isItemsHost)
                return base.Contains(element);
            else
                return _externalChildren.Contains(element);
        }

        /// <summary>
        /// Determines the index of a specific item in the collection.
        /// </summary>
        /// <param name="element">The UIElement to locate in the collection.</param>
        /// <returns>Index of the element; otherwise -1.</returns>
        public override int IndexOf(UIElement element)
        {
            if (_isItemsHost)
                return base.IndexOf(element);
            else
                return _externalChildren.IndexOf(element);
        }

        /// <summary>
        /// Gets an object that can be used to synchronize access to the collection.
        /// </summary>
        public override object SyncRoot
        {
            get { return _internalChildren.SyncRoot; }
        }

        /// <summary>
        /// Gets a value indicating whether access to the collection is synchronized.
        /// </summary>
        public override bool IsSynchronized
        {
            get { return _internalChildren.IsSynchronized; }
        }

        /// <summary>
        /// Copies the elements from the collection to the array.
        /// </summary>
        /// <param name="array">Array that is the destination for copying.</param>
        /// <param name="index">Zero-based index in the array at which copying begins.</param>
        public override void CopyTo(Array array, int index)
        {
            if (_isItemsHost)
                base.CopyTo(array, index);
            else
                _externalChildren.CopyTo((UIElement[])array, index);
        }

        /// <summary>
        /// Copies the elements from the collection to the element array.
        /// </summary>
        /// <param name="array">UIElement that is the destination for copying.</param>
        /// <param name="index">Zero-based index in the array at which copying begins.</param>
        public override void CopyTo(UIElement[] array, int index)
        {
            if (_isItemsHost)
                base.CopyTo(array, index);
            else
                _externalChildren.CopyTo(array, index);
        }

        /// <summary>
        /// Gets and sets the number of elements the collection can contain.
        /// </summary>
        public override int Capacity
        {
            get
            {
                if (_isItemsHost)
                    return base.Capacity;
                else
                    return _internalChildren.Capacity;
            }

            set
            {
                if (_isItemsHost)
                    base.Capacity = value;
                else
                {
                    VerifyWriteAccess();
                    _internalChildren.Capacity = value;
                }
            }
        }

        /// <summary>
        /// Adds the specified element to the collection.
        /// </summary>
        /// <param name="element">The UIElement to add.</param>
        /// <returns>The index position of the added element.</returns>
        public override int Add(UIElement element)
        {
            if (_isItemsHost)
                return base.Add(element);
            else
            {
                ValidateElement(element);
                VerifyWriteAccess();

                // Add to the external list
                int index = _externalChildren.Count;
                _externalChildren.Add(element);

                // Add to internal logical/visual tree
                SetLogicalParent(element);
                _internalChildren.Add(element);

                // Instruct the visual parent it must measure with the new child element
                _visualParent.InvalidateMeasure();

                // Added event is always generated after the base call
                OnUIElementsAdded(new UIElementsEventArgs(element));

                return index;
            }
        }

        /// <summary>
        /// Inserts an element into a collection at the specified index position.
        /// </summary>
        /// <param name="index">The index position where you want to insert the element.</param>
        /// <param name="element">The element to insert into the collection.</param>
        public override void Insert(int index, UIElement element)
        {
            if (_isItemsHost)
                base.Insert(index, element);
            else
            {
                ValidateElement(element);
                VerifyWriteAccess();

                // Add to the external list
                _externalChildren.Insert(index, element);

                // Add to internal logical tree
                SetLogicalParent(element);

                // Add to the internal visual tree (in same relative position in the visual collection)
                if (_internalChildren.Count == 0)
                    _internalChildren.Add(element);
                else if (index == 0)
                    _internalChildren.Insert(0, element);
                else
                    _internalChildren.Insert(_internalChildren.IndexOf(_externalChildren[index - 1]) + 1, element);

                // Instruct the visual parent it must measure with the new child element
                _visualParent.InvalidateMeasure();

                // Added event is always generated after the base call
                OnUIElementsAdded(new UIElementsEventArgs(element));
            }
        }

        /// <summary>
        /// Removes all elements from the collection.
        /// </summary>
        public override void Clear()
        {
            if (_isItemsHost)
                base.Clear();
            else
            {
                VerifyWriteAccess();

                // Make a copy of all contained elements
                UIElement[] elements = _externalChildren.ToArray();

                // Remove all the external list entries
                _externalChildren.Clear();

                // Instruct the visual parent it must measure with the change in state
                _visualParent.InvalidateMeasure();

                // Actual elements are removed from internal collection once removed animation completes
                OnUIElementsRemove(new UIElementsEventArgs(elements));
            }
        }

        /// <summary>
        /// Removes the specified element from the collection. 
        /// </summary>
        /// <param name="element">The element to remove from the collection.</param>
        public override void Remove(UIElement element)
        {
            if (_isItemsHost)
                base.Remove(element);
            else
            {
                VerifyWriteAccess();

                // Remove only from the external collection
                _externalChildren.Remove(element);

                // Instruct the visual parent it must measure with the change in state
                _visualParent.InvalidateMeasure();

                // Actual elements are removed from internal collection once removed animation completes
                OnUIElementsRemove(new UIElementsEventArgs(element));
            }
        }

        /// <summary>
        /// Removes the UIElement at the specified index. 
        /// </summary>
        /// <param name="index">The index of the UIElement that you want to remove.</param>
        public override void RemoveAt(int index)
        {
            if (_isItemsHost)
                base.RemoveAt(index);
            else
            {
                VerifyWriteAccess();

                // Get the element to be removed
                UIElement element = this[index];

                // Remove it from the external collection
                _externalChildren.RemoveAt(index);

                // Instruct the visual parent it must measure with the change in state
                _visualParent.InvalidateMeasure();

                // Actual elements are removed from internal collection once removed animation completes
                OnUIElementsRemove(new UIElementsEventArgs(element));
            }
        }

        /// <summary>
        /// Removes a range of elements from the collection.
        /// </summary>
        /// <param name="index">The index position of the element where removal begins.</param>
        /// <param name="count">The number of elements to remove.</param>
        public override void RemoveRange(int index, int count)
        {
            if (_isItemsHost)
                base.RemoveRange(index, count);
            else
            {
                VerifyWriteAccess();

                // Make a copy of all elements to be removed
                UIElement[] elements = new UIElement[count];
                for (int i = 0; i < count; i++)
                    elements[i] = this[index + i];

                // Remove them from the external collection
                _externalChildren.RemoveRange(index, count);

                // Instruct the visual parent it must measure with the change in state
                _visualParent.InvalidateMeasure();

                // Actual elements are removed from internal collection once removed animation completes
                OnUIElementsRemove(new UIElementsEventArgs(elements));
            }
        }

        /// <summary>
        /// Gets or sets the UIElement stored at the zero-based index position of the collection.
        /// </summary>
        /// <param name="index">The index position of the UIElement.</param>
        /// <returns>A UIElement at the specified index position.</returns>
        public override UIElement this[int index]
        {
            get
            {
                if (_isItemsHost)
                    return base[index];
                else
                    return _externalChildren[index];
            }

            set
            {
                if (_isItemsHost)
                    base[index] = value;
                else
                {
                    if (_externalChildren[index] != value)
                    {
                        ValidateElement(value);
                        VerifyWriteAccess();

                        // Instruct the visual parent it must measure with the change in state
                        _visualParent.InvalidateMeasure();

                        // Actual elements are removed from internal collection once removed animation completes
                        OnUIElementsRemove(new UIElementsEventArgs(_externalChildren[index]));

                        // Update with the new value
                        _externalChildren[index] = value;

                        // Added event is always generated after the base call
                        OnUIElementsAdded(new UIElementsEventArgs(value));
                    }
                }
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public override IEnumerator GetEnumerator()
        {
            if (_isItemsHost)
                return base.GetEnumerator();
            else
                return _externalChildren.GetEnumerator();
        }
        #endregion

        #region Protected
        /// <summary>
        /// Raises the UIElementsAdded event.
        /// </summary>
        /// <param name="e">An UIElementsEventArgs containing the event data.</param>
        protected virtual void OnUIElementsAdded(UIElementsEventArgs e)
        {
            EventHandler<UIElementsEventArgs> handler = UIElementsAdded;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Raises the UIElementsRemove event.
        /// </summary>
        /// <param name="e">An UIElementsEventArgs containing the event data.</param>
        protected virtual void OnUIElementsRemove(UIElementsEventArgs e)
        {
            EventHandler<UIElementsEventArgs> handler = UIElementsRemove;
            if (handler != null)
                handler(this, e);
        }
        #endregion

        #region Internal
        internal bool InternalIsItemsHost
        {
            get { return _isItemsHost; }
            set { _isItemsHost = value; }
        }

        internal Visual GetInternalChild(int index)
        {
            return _internalChildren[index];
        }

        internal int InternalCount
        {
            get { return _internalChildren.Count; }
        }

        internal void InternalAdd(UIElement element)
        {
            _internalChildren.Add(element);
            SetLogicalParent(element);
        }

        internal void InternalRemove(UIElement element)
        {
            _internalChildren.Remove(element);
            ClearLogicalParent(element);
        }

        internal IEnumerator GetInternalEnumerator()
        {
            return _internalChildren.GetEnumerator();
        }

        internal int ExternalCount
        {
            get { return base.Count; }
        }

        internal IEnumerator GetExternalEnumerator()
        {
            return base.GetEnumerator();
        }

        internal int CompoundCount
        {
            get { return InternalCount + ExternalCount; }
        }

        internal ICollection CompoundElements
        {
            get { return new CompoundMetaElementCollection(this); }
        }
        #endregion

        #region Private
        private void ValidateElement(UIElement element)
        {
            if (element == null)
                throw new ArgumentNullException("UIElement cannot be null.");
        }

        private void VerifyWriteAccess()
        {
            MetaPanel panel = _visualParent as MetaPanel;
            if ((panel != null) && panel.IsDataBound)
                throw new InvalidOperationException("Cannot modify Children collection of a data bound MetaPanel.");
        }
        #endregion
    }
}
