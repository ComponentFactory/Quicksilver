// *****************************************************************************
// 
//  © Component Factory Pty Ltd 2011. All rights reserved.
//	The software and associated documentation supplied hereunder are the 
//  proprietary information of Component Factory Pty Ltd, PO Box 1504, 
//  Glen Waverley, Vic 3150, Australia and are supplied subject to licence terms.
// 
//  Version 1.0.8.0 	www.ComponentFactory.com
// *****************************************************************************

using System;
using System.Text;
using System.Collections.Generic;

namespace ComponentFactory.Quicksilver.Binding
{
    /// <summary>
    /// Base class used for implementing specific types of evaluation node.
    /// </summary>
    public abstract class EvalNode
    {
        #region Instance Fields
        private EvalNode _parent;
        private List<EvalNode> _children;
        #endregion

        #region Identity
        /// <summary>
        /// Initialize a new instance of the EvalNode class.
        /// </summary>
        public EvalNode()
        {
            _children = new List<EvalNode>();
        }
        #endregion

        #region Public
        /// <summary>
        /// Gets and sets the parent node instance.
        /// </summary>
        public EvalNode Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        /// <summary>
        /// Gets the root node.
        /// </summary>
        public EvalNode Root
        {
            get 
            { 
                EvalNode node = this;

                // Walk up the parent chain until we arrive at the root
                while (node.Parent != null)
                    node = node.Parent;

                return node;
            }
        }

        /// <summary>
        /// Appends a node to the end of the child collection.
        /// </summary>
        /// <param name="item">Reference to node for appending.</param>
        public void Append(EvalNode item)
        {
            if (item == null)
                throw new ArgumentNullException("item", "cannot append an empty reference");
            else
            {
                _children.Add(item);
                item.Parent = this;
            }
        }

        /// <summary>
        /// Append nodes to the end of the child collection.
        /// </summary>
        /// <param name="items">Collection of nodes.</param>
        public void Append(EvalNode[] items)
        {
            if (items == null)
                throw new ArgumentNullException("item", "cannot append an empty reference");
            else
            {
                foreach (EvalNode item in items)
                    Append(item);
            }
        }

        /// <summary>
        /// Prepends a node to the start of the child collection.
        /// </summary>
        /// <param name="item">Reference to node for prepending.</param>
        public void Prepend(EvalNode item)
        {
            if (item == null)
                throw new ArgumentNullException("item", "cannot append an empty reference");
            else
            {
                _children.Insert(0, item);
                item.Parent = this;
            }
        }

        /// <summary>
        /// Gets access to the indexed child.
        /// </summary>
        /// <param name="index">Index of item to recover.</param>
        /// <returns></returns>
        public EvalNode this[int index]
        {
            get
            {
                if ((index >= 0) && (index < _children.Count))
                    return _children[index];
                else
                    throw new ArgumentOutOfRangeException("index", "index out of range");
            }

            set
            {
                if ((index >= 0) && (index < _children.Count))
                    _children[index] = value;
                else
                    throw new ArgumentOutOfRangeException("index", "index out of range");
            }
        }

        /// <summary>
        /// Gets number of children present.
        /// </summary>
        public int Count
        {
            get { return _children.Count; }
        }

        /// <summary>
        /// Remove all the child nodes and return them as an array.
        /// </summary>
        /// <returns></returns>
        public EvalNode[] RemoveAll()
        {
            EvalNode[] items = (EvalNode[])_children.ToArray();
            _children.Clear();
            return items;
        }

        /// <summary>
        /// Evalaute this node and return result.
        /// </summary>
        /// <param name="thisObject">Reference to object that is exposed as 'this'.</param>
        /// <returns>Result value and type of that result.</returns>
        public abstract EvalResult Evaluate(object thisObject);
        #endregion

        #region Protected
        /// <summary>
        /// Update the TypeCode to reflect the contents of the Value fields.
        /// </summary>
        /// <param name="ret">Instance to update.</param>
        /// <returns>Instance that has been updated.</returns>
        protected EvalResult DiscoverTypeCode(EvalResult ret)
        {
            // Fill in the correct type code for the value
            if (ret.Value == null)
                ret.Type = TypeCode.Object;
            else
                ret.Type = Type.GetTypeCode(ret.Value.GetType());

            return ret;
        }
        #endregion
    }
}
