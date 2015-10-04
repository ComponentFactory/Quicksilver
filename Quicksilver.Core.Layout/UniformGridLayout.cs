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
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;

namespace ComponentFactory.Quicksilver.Layout
{
    /// <summary>
    /// Arranges child elements into a grid with each cell the same size.
    /// </summary>
    public class UniformGridLayout : Layout
    {
        #region Dependancy Properties
        /// <summary>
        /// Identifies the Rows dependency property.
        /// </summary>
        public static readonly DependencyProperty RowsProperty;

        /// <summary>
        /// Identifies the Columns dependency property.
        /// </summary>
        public static readonly DependencyProperty ColumnsProperty;

        /// <summary>
        /// Identifies the FirstColumn dependency property.
        /// </summary>
        public static readonly DependencyProperty FirstColumnProperty;
        #endregion

        #region Instance Fields
        private int _rows;
        private int _columns;
        #endregion

        #region Identity
        static UniformGridLayout()
        {
            RowsProperty = DependencyProperty.Register("Rows", 
                                                       typeof(int),
                                                       typeof(UniformGridLayout), 
                                                       new PropertyMetadata(0, 
                                                       new PropertyChangedCallback(OnNeedMeasureOnChanged)));

            ColumnsProperty = DependencyProperty.Register("Columns",
                                                          typeof(int),
                                                          typeof(UniformGridLayout),
                                                          new PropertyMetadata(0,
                                                          new PropertyChangedCallback(OnNeedMeasureOnChanged)));

            FirstColumnProperty = DependencyProperty.Register("FirstColumn",
                                                              typeof(int),
                                                              typeof(UniformGridLayout),
                                                              new PropertyMetadata(0,
                                                              new PropertyChangedCallback(OnNeedMeasureOnChanged)));
        }
        #endregion

        #region Public
        /// <summary>
        ///Gets or sets the number of rows that are in the grid.
        /// </summary>
        public int Rows
        {
            get { return (int)GetValue(RowsProperty); }
            set { SetValue(RowsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the number of columns that are in the grid.
        /// </summary>
        public int Columns
        {
            get { return (int)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the number of leading blank cells in the first row of the grid.
        /// </summary>
        public int FirstColumn
        {
            get { return (int)GetValue(FirstColumnProperty); }
            set { SetValue(FirstColumnProperty, value); }
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
                // Calculate number of rows and columns needed to show all children
                UpdateRowsColumns(stateDict, elements);

                // Each element should be equal sized according to grid cell size
                Size elementSize = new Size(availableSize.Width / (double)_columns,
                                            availableSize.Height / (double)_rows);

                // Measure each element in turn
                double widest = 0;
                double tallest = 0;
                foreach (UIElement element in elements)
                {
                    element.Measure(elementSize);
                    if (element.Visibility != Visibility.Collapsed)
                    {
                        // We ignore items being removed
                        if (stateDict[element].Status != MetaElementStatus.Removing)
                        {
                            // Track widest/tallest for calculating cell size later
                            widest = Math.Max(widest, element.DesiredSize.Width);
                            tallest = Math.Max(tallest, element.DesiredSize.Height);
                        }
                    }
                }

                // We would like a size that ensures each cell is big enough for the biggest child
                return new Size(_columns * widest, _rows * tallest);
            }
            else
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
                // Find the size of each size and start at the top left cell
                Rect cellRect = new Rect(0, 0, finalSize.Width / (double)_columns, finalSize.Height / (double)_rows);

                // Move across to the defined starting column
                cellRect.X += cellRect.Width * FirstColumn;

                // Calculate the right hand side where we need to start a new row
                double finalSizeRight = finalSize.Width - 1.0;

                // Calculate the target rectangle for each element
                foreach (UIElement element in elements)
                {
                    // We ignore items being removed
                    if (stateDict[element].Status != MetaElementStatus.Removing)
                    {
                        // We ignore items that are collapsed from view
                        if (element.Visibility != Visibility.Collapsed)
                        {
                            // The new target is the current cell rect
                            Rect newTargetRect = cellRect;

                            // Move across to next column
                            cellRect.X += cellRect.Width;

                            // Do we start a line row?
                            if (cellRect.X >= finalSizeRight)
                            {
                                cellRect.X = 0;
                                cellRect.Y += cellRect.Height;
                            }

                            // Store the new target rectangle
                            if (!stateDict[element].TargetRect.Equals(newTargetRect))
                            {
                                stateDict[element].TargetChanged = true;
                                stateDict[element].TargetRect = newTargetRect;
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Private
        private void UpdateRowsColumns(MetaElementStateDict stateDict,
                                       ICollection elements)
        {
            // Start with the defined dependancy property values
            _columns = Columns;
            _rows = Rows;

            // Limit check the first column setting
            if (FirstColumn >= _columns)
                FirstColumn = 0;

            // If one or more of the dimensions is not defined, we need to calculate it
            if ((_rows == 0) || (_columns == 0))
            {
                // Find number of elements that need to be positioned
                int children = 0;
                foreach (UIElement element in elements)
                    if ((element.Visibility != Visibility.Collapsed) && 
                        (stateDict[element].Status != MetaElementStatus.Removing))
                            children++;

                // Algorithm needs at least one child to work
                if (children == 0)
                    children = 1;

                // If the number of rows is not defined
                if (_rows == 0)
                {
                    // If number of columns is defined
                    if (_columns > 0)
                    {
                        // Then just find the number of rows needed to display all the children
                        _rows = ((children + FirstColumn) + (_columns - 1)) / _columns;
                    }
                    else
                    {
                        // Find the number of rows and columns that is square
                        _rows = (int)Math.Sqrt((double)children);

                        // Rounding from double to int above means we might need an extra row
                        if ((_rows * _rows) < children)
                            _rows++;

                        // Columns is always the same as rows, to be square
                        _columns = _rows;
                    }
                }
                else if (_columns == 0)
                {
                    // Find number of columns needed to show children over the specified rows
                    _columns = (children + (_rows - 1)) / _rows;
                }
            }
        }
        #endregion
    }
}
