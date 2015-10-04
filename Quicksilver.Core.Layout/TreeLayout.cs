// *****************************************************************************
// 
//  © Component Factory Pty Ltd 2009. All rights reserved.
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
    /// Positions elements in a tree hierarchy relative to a provided root element.
    /// </summary>
    public class TreeLayout : Layout
    {
        #region Dependancy Properties
        /// <summary>
        /// Identifies the StartNode dependency property.
        /// </summary>
        public static readonly DependencyProperty StartNodeProperty;
        #endregion

        #region Identity
        static TreeLayout()
        {
            StartNodeProperty = DependencyProperty.Register("StartNode",
                                                            typeof(ITreeNode),
                                                            typeof(TreeLayout), 
                                                            new PropertyMetadata(null, 
                                                            new PropertyChangedCallback(OnNeedMeasureOnChanged)));
        }
        #endregion

        #region Public
        /// <summary>
        /// Gets or sets a value that specifies the starting node within a TreeLayout.
        /// </summary>
        public ITreeNode StartNode
        {
            get { return (ITreeNode)GetValue(StartNodeProperty); }
            set { SetValue(StartNodeProperty, value); }
        }

        /// <summary>
        /// Measure the layout size required to arrange all elements.
        /// </summary>
        /// <param name="layoutId">Identifier of the layout to be used.</param>
        /// <param name="metaPanel">Reference to owning panel instance.</param>
        /// <param name="stateDict">Dictionary of per-element state.</param>
        /// <param name="elements">Collection of elements to be measured.</param>
        /// <param name="availableSize">Available size that can be given to elements.</param>
        /// <returns>Size the layout determines it needs based on child element sizes.</returns>
        public override Size MeasureChildren(string layoutId,
                                             MetaPanelBase metaPanel,
                                             MetaElementStateDict stateDict,
                                             ICollection elements,
                                             Size availableSize)
        {
            // Only apply if we match the incoming layout identifier
            if (string.IsNullOrEmpty(Id) || Id.Equals(layoutId))
            {
                // TODO measure elements that are not in tree drawing
                return MeasureTreeNodes(StartNode, stateDict, false);
            }

            return Size.Empty;
        }

        /// <summary>
        /// Calculate target state for each element based on layout algorithm.
        /// </summary>
        /// <param name="layoutId">Identifier of the layout to be used.</param>
        /// <param name="metaPanel">Reference to owning panel instance.</param>
        /// <param name="stateDict">Dictionary of per-element state.</param>
        /// <param name="elements">Collection of elements to be arranged.</param>
        /// <param name="finalSize">Size that layout should use to arrange child elements.</param>
        public override void TargetChildren(string layoutId,
                                            MetaPanelBase metaPanel,
                                            MetaElementStateDict stateDict,
                                            ICollection elements,
                                            Size finalSize)
        {
            // Only apply if we match the incoming layout identifier
            if (string.IsNullOrEmpty(Id) || Id.Equals(layoutId))
            {
                // TODO target elements that are not in tree drawing
                TargetTreeNodes(Utility.PointZero, StartNode, stateDict, false);
            }
        }
        #endregion

        #region Private
        private Size MeasureTreeNodes(ITreeNode node, 
                                      MetaElementStateDict stateDict,
                                      bool downwards)
        {
            Size totalSize = Utility.SizeZero;

            UIElement element = node as UIElement;
            if ((element != null) && (stateDict.ContainsKey(element)))
            {
                // Use element size as the starting total size
                element.Measure(Utility.SizeInfinity);

                // Do not measure children of items being removed
                if (stateDict[element].Status != MetaElementStatus.Removing)
                {
                    // Starting size covers only the node
                    totalSize = element.DesiredSize;

                    // Process each sub tree
                    Size childTotalSize = Utility.SizeZero;
                    foreach (ITreeNode child in node.ChildNodes())
                    {
                        Size childSize = MeasureTreeNodes(child, stateDict, downwards);

                        // Stack all the sub trees vertically in sizing
                        childTotalSize.Width = Math.Max(childTotalSize.Width, childSize.Width);
                        childTotalSize.Height += childSize.Height;
                    }

                    // Place the set of sub trees to the right of this node
                    totalSize.Width += childTotalSize.Width;
                    totalSize.Height = Math.Max(totalSize.Height, childTotalSize.Height);
                }
            }

            return totalSize;
        }

        private Size TargetTreeNodes(Point location, 
                                     ITreeNode node, 
                                     MetaElementStateDict stateDict,
                                     bool downwards)
        {
            Size totalSize = Utility.SizeZero;

            UIElement element = node as UIElement;
            if ((element != null) && (stateDict.ContainsKey(element)))
            {
                // Do not measure children of items being removed
                if (stateDict[element].Status != MetaElementStatus.Removing)
                {
                    // Starting size covers only the node
                    totalSize = element.DesiredSize;

                    // Process each sub tree
                    Point childLocation = new Point(location.X + element.DesiredSize.Width, location.Y);
                    Size childTotalSize = Utility.SizeZero;
                    foreach (ITreeNode child in node.ChildNodes())
                    {
                        Size childSize = TargetTreeNodes(childLocation, child, stateDict, downwards);

                        // Stack all the sub trees vertically in sizing
                        childTotalSize.Width = Math.Max(childTotalSize.Width, childSize.Width);
                        childTotalSize.Height += childSize.Height;
                        childLocation.Y += childSize.Height;
                    }

                    // Place the set of sub trees to the right of this node
                    totalSize.Width += childTotalSize.Width;
                    totalSize.Height = Math.Max(totalSize.Height, childTotalSize.Height);
                }

                // Position the node vertically centered
                Rect newTargetRect = new Rect(location.X, 
                                              location.Y + (totalSize.Height - element.DesiredSize.Height) / 2, 
                                              element.DesiredSize.Width, 
                                              element.DesiredSize.Height);

                // Store the new target rectangle
                if (!stateDict[element].TargetRect.Equals(newTargetRect))
                {
                    stateDict[element].TargetChanged = true;
                    stateDict[element].TargetRect = newTargetRect;
                }
            }

            return totalSize;
        }
        #endregion
    }
}
