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
    /// Manage a collection of Layout derived instances.
    /// </summary>
    public partial class LayoutCollection : MeasureElementCollection<Layout>
    {
        #region Identity
        /// <summary>
        /// Initialize a new instance of the LayoutCollection class.
        /// </summary>
        /// <param name="owner">Reference to owning control.</param>
        internal LayoutCollection(MetaPanelBase owner)
            : base(owner)
        {
        }
        #endregion

        #region Public
        /// <summary>
        /// Measure the layout size required to arrange all elements.
        /// </summary>
        /// <param name="layoutId">Identifier of the layout to be used.</param>
        /// <param name="metaPanel">Reference to owning panel instance.</param>
        /// <param name="stateDict">Dictionary of per-element state.</param>
        /// <param name="elements">Collection of elements to be measured.</param>
        /// <param name="availableSize">Available size that can be given to elements.</param>
        /// <returns>Size the layout determines it needs based on child element sizes.</returns>
        public Size MeasureChildren(string layoutId,
                                    MetaPanelBase metaPanel,
                                    MetaElementStateDict stateDict,
                                    ICollection elements,
                                    Size availableSize)
        {
            Size retSize = new Size();
            foreach (Layout layout in this)
            {
                Size layoutSize = layout.MeasureChildren(layoutId, metaPanel, stateDict, elements, availableSize);

                // Find the largest requested size
                retSize.Width = Math.Max(retSize.Width, layoutSize.Width);
                retSize.Height = Math.Max(retSize.Height, layoutSize.Height);
            }

            return retSize;
        }

        /// <summary>
        /// Calculate target state for each element based on layout algorithm.
        /// </summary>
        /// <param name="layoutId">Identifier of the layout to be used.</param>
        /// <param name="metaPanel">Reference to owning panel instance.</param>
        /// <param name="stateDict">Dictionary of per-element state.</param>
        /// <param name="elements">Collection of elements to be arranged.</param>
        /// <param name="finalSize">Size that layout should use to arrange child elements.</param>
        public void TargetChildren(string layoutId,
                                   MetaPanelBase metaPanel,
                                   MetaElementStateDict stateDict,
                                   ICollection elements,
                                   Size finalSize)
        {
            foreach (Layout layout in this)
                layout.TargetChildren(layoutId, metaPanel, stateDict, elements, finalSize);
        }

        /// <summary>
        /// Position child elements according to already calculated target state.
        /// </summary>
        /// <param name="layoutId">Identifier of the layout to be used.</param>
        /// <param name="metaPanel">Reference to owning panel instance.</param>
        /// <param name="stateDict">Dictionary of per-element state.</param>
        /// <param name="elements">Collection of elements to be arranged.</param>
        /// <param name="finalSize">Size that layout should use to arrange child elements.</param>
        /// <returns>Size used by the layout panel.</returns>
        public Size ArrangeChildren(string layoutId,
                                    MetaPanelBase metaPanel,
                                    MetaElementStateDict stateDict,
                                    ICollection elements,
                                    Size finalSize)
        {
            foreach (UIElement element in elements)
            {
                MetaElementState elementState = stateDict[element];
                elementState.Element.Arrange(elementState.CurrentRect);
            }

            // Layout panel takes all the provided size
            return finalSize;
        }
        #endregion
    }
}
