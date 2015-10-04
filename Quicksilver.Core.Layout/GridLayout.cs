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
    /// Arranges child elements in a flexible grid area that consists of columns and rows.
    /// </summary>
    public partial class GridLayout : Layout
    {
        #region Types
        private class DefinitionProxy
        {
            #region Identity
            /// <summary>
            /// Initialize a new instance of the DefinitionProxy class.
            /// </summary>
            /// <param name="definition">Real row/col to act as proxy for.</param>
            public DefinitionProxy(BaseDefinition definition)
            {
                Definition = definition;
                IsColumn = (definition is ColumnDefinition);
            }
            #endregion

            #region Public
            /// <summary>
            /// Gets and sets the associated definition instance.
            /// </summary>
            public BaseDefinition Definition { get; set; }

            /// <summary>
            /// Gets and sets a value indicating if the proxy is for a column definition.
            /// </summary>
            public bool IsColumn { get; set; }

            /// <summary>
            /// Gets and sets the pixel min size of the definition.
            /// </summary>
            public double MinSize { get; set; }

            /// <summary>
            /// Gets and sets the cumulative min sizes for all definitions before this one.
            /// </summary>
            public double MinTotal { get; set; }

            /// <summary>
            /// Gets and sets the cumulative desired size of all the previous definitions.
            /// </summary>
            public double DesiredTotal { get; set; }

            /// <summary>
            /// Gets and sets the desired size of the definition.
            /// </summary>
            public double DesiredSize { get; set; }

            /// <summary>
            /// Gets and sets the size to use when measuring elements in the definintion.
            /// </summary>
            public double MeasureSize { get; set; }

            /// <summary>
            /// Gets and sets if the star definition has been allocated space.
            /// </summary>
            public bool StarAllocated { get; set; }

            /// <summary>
            /// Gets and sets the user size to use for the definition.
            /// </summary>
            public GridUnitType UserGridUnitType { get; set; }

            /// <summary>
            /// Gets the user defined size of the definition (Width for column, Height for row)
            /// </summary>
            public GridLength DefUserSize
            {
                get { return IsColumn ? ((ColumnDefinition)Definition).Width : ((RowDefinition)Definition).Height; }
            }

            /// <summary>
            /// Gets the user defined minimum size of the definition (MinWidth for column, MinHeight for row)
            /// </summary>
            public double DefUserMin
            {
                get { return IsColumn ? ((ColumnDefinition)Definition).MinWidth : ((RowDefinition)Definition).MinHeight; }
            }

            /// <summary>
            /// Gets the user defined maximum size of the definition (MaxWidth for column, MaxHeight for row)
            /// </summary>
            public double DefUserMax
            {
                get { return IsColumn ? ((ColumnDefinition)Definition).MaxWidth : ((RowDefinition)Definition).MaxHeight; }
            }

            /// <summary>
            /// Find the minimum size allowed for this definition during measuring.
            /// </summary>
            /// <param name="availableSize">Maximum available size for the definition.</param>
            /// <returns>Star value for the definition.</returns>
            public double PreMeasure(double availableSize)
            {
                // Reset allocated field each measure cycle
                StarAllocated = false;
                
                // Update cached unit type
                UserGridUnitType = DefUserSize.GridUnitType;

                // If the available size is infinite then any Star type becomes Auto instead
                if (availableSize == double.PositiveInfinity)
                    if (UserGridUnitType == GridUnitType.Star)
                        UserGridUnitType = GridUnitType.Auto;

                double retStars = 0.0;
                switch(UserGridUnitType)
                {
                    case GridUnitType.Pixel:
                        MinSize = Math.Max(DefUserMin, Math.Min(DefUserSize.Value, DefUserMax));
                        MeasureSize = MinSize;
                        break;
                    case GridUnitType.Auto:
                        MinSize = DefUserMin;
                        MeasureSize = double.PositiveInfinity;
                        break;
                    case GridUnitType.Star:
                        retStars = DefUserSize.Value;
                        MinSize = DefUserMin;
                        MeasureSize = availableSize;
                        break;
                }

                DesiredSize = MinSize;
                return retStars;
            }

            /// <summary>
            /// Update definition sizing after measuring a content of the definition.
            /// </summary>
            /// <param name="size">Size of a content within the definintion.</param>
            public void UpdateFromMeasure(double size)
            {
                switch (UserGridUnitType)
                {
                    case GridUnitType.Pixel:
                        // Pixel definitions are fixed, so ignore anything measured inside them
                        break;
                    case GridUnitType.Star:
                        // Track largest measured entry in the star definition
                        DesiredSize = Math.Max(DesiredSize, size);
                        break;
                    case GridUnitType.Auto:
                        // Auto definitions must be as big as the largest entry it contains
                        MinSize = Math.Max(MinSize, size);
                        DesiredSize = MinSize;
                        break;
                }
            }
            #endregion
        }
        #endregion

        #region Dependancy Properties
        /// <summary>
        /// Identifies the Column dependency property.
        /// </summary>
        public static readonly DependencyProperty ColumnProperty;

        /// <summary>
        /// Identifies the ColumnSpan dependency property.
        /// </summary>
        public static readonly DependencyProperty ColumnSpanProperty;

        /// <summary>
        /// Identifies the Row dependency property.
        /// </summary>
        public static readonly DependencyProperty RowProperty;

        /// <summary>
        /// Identifies the RowSpan dependency property.
        /// </summary>
        public static readonly DependencyProperty RowSpanProperty;
        #endregion

        #region Instance Fields
        private ColumnDefinitionCollection _columns;
        private RowDefinitionCollection _rows;
        private DefinitionProxy[] _proxyColumns;
        private DefinitionProxy[] _proxyRows;
        private double _columnStars;
        private double _rowStars;
        #endregion

        #region Identity
        /// <summary>
        /// Initialize a new instance of the GridLayout class.
        /// </summary>
        public GridLayout()
        {
            _columns = new ColumnDefinitionCollection();
            _rows = new RowDefinitionCollection();

            // We need to request a measure/arrange cycle whenever definitions are changed
            _columns.NeedMeasure += new EventHandler(OnNeedMeasure);
            _rows.NeedMeasure += new EventHandler(OnNeedMeasure);
        }
        #endregion

        #region Public
        /// <summary>
        /// Sets the value of the Column attached property to a specified element.
        /// </summary>
        /// <param name="element">The element to which the attached property is written.</param>
        /// <param name="column">The new column value.</param>
        public static void SetColumn(UIElement element, int column)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            element.SetValue(ColumnProperty, column);
        }

        /// <summary>
        /// Gets the value of the Column attached property for a specified UIElement. 
        /// </summary>
        /// <param name="element">The element from which the property value is read.</param>
        /// <returns>The Column property value for the element.</returns>
        #if !SILVERLIGHT
        [AttachedPropertyBrowsableForChildren]
        #endif
        public static int GetColumn(UIElement element)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            return (int)element.GetValue(ColumnProperty);
        }

        /// <summary>
        /// Sets the value of the Row attached property to a specified element.
        /// </summary>
        /// <param name="element">The element to which the attached property is written.</param>
        /// <param name="row">The new row value.</param>
        public static void SetRow(UIElement element, int row)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            element.SetValue(RowProperty, row);
        }

        /// <summary>
        /// Gets the value of the Row attached property for a specified UIElement. 
        /// </summary>
        /// <param name="element">The element from which the property value is read.</param>
        /// <returns>The Row property value for the element.</returns>
        #if !SILVERLIGHT
        [AttachedPropertyBrowsableForChildren]
        #endif
        public static int GetRow(UIElement element)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            return (int)element.GetValue(RowProperty);
        }

        /// <summary>
        /// Sets the value of the ColumnSpan attached property to a specified element.
        /// </summary>
        /// <param name="element">The element to which the attached property is written.</param>
        /// <param name="columnSpan">The new ColumnSpan value.</param>
        public static void SetColumnSpan(UIElement element, int columnSpan)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            element.SetValue(ColumnSpanProperty, columnSpan);
        }

        /// <summary>
        /// Gets the value of the ColumnSpan attached property for a specified UIElement. 
        /// </summary>
        /// <param name="element">The element from which the property value is read.</param>
        /// <returns>The ColumnSpan property value for the element.</returns>
        #if !SILVERLIGHT
        [AttachedPropertyBrowsableForChildren]
        #endif
        public static int GetColumnSpan(UIElement element)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            return (int)element.GetValue(ColumnSpanProperty);
        }

        /// <summary>
        /// Sets the value of the RowSpan attached property to a specified element.
        /// </summary>
        /// <param name="element">The element to which the attached property is written.</param>
        /// <param name="rowSpan">The new RowSpan value.</param>
        public static void SetRowSpan(UIElement element, int rowSpan)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            element.SetValue(RowSpanProperty, rowSpan);
        }

        /// <summary>
        /// Gets the value of the RowSpan attached property for a specified UIElement. 
        /// </summary>
        /// <param name="element">The element from which the property value is read.</param>
        /// <returns>The RowSpan property value for the element.</returns>
        #if !SILVERLIGHT
        [AttachedPropertyBrowsableForChildren]
        #endif
        public static int GetRowSpan(UIElement element)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            return (int)element.GetValue(RowSpanProperty);
        }

        /// <summary>
        /// Gets a ColumnDefinitionCollection defined on this instance of Grid.
        /// </summary>
        #if !SILVERLIGHT
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        #endif
        public ColumnDefinitionCollection ColumnDefinitions
        {
            get { return _columns; }
        }

        /// <summary>
        /// Gets a RowDefinitionCollection defined on this instance of Grid. 
        /// </summary>
        #if !SILVERLIGHT
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        #endif
        public RowDefinitionCollection RowDefinitions
        {
            get { return _rows; }
        }

        /// <summary>
        /// Add this element into the logical tree.
        /// </summary>
        /// <param name="parent">Interface of logical parent.</param>
        public override void AddToLogicalTree(ILogicalParent parent)
        {
            // Add ourself into the logical tree
            base.AddToLogicalTree(parent);

            // Add any existing cols/rows into the logical tree
            ColumnDefinitions.LogicalParent = parent;
            foreach (ColumnDefinition col in ColumnDefinitions)
                col.AddToLogicalTree(parent);

            RowDefinitions.LogicalParent = parent;
            foreach (RowDefinition row in RowDefinitions)
                row.AddToLogicalTree(parent);
        }

        /// <summary>
        /// Add this element from the logial tree.
        /// </summary>
        /// <param name="parent">Interface of logical parent.</param>
        public override void RemoveFromLogicalTree(ILogicalParent parent)
        {
            // Remove ourself into the logical tree
            base.RemoveFromLogicalTree(parent);

            // Remove any existing cols/rows from the logical tree
            foreach (ColumnDefinition col in ColumnDefinitions)
                col.RemoveFromLogicalTree(parent);

            foreach (RowDefinition row in RowDefinitions)
                row.RemoveFromLogicalTree(parent);

            ColumnDefinitions.LogicalParent = parent;
            RowDefinitions.LogicalParent = parent;
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
            Size retSize = new Size();

            // Only apply if we match the incoming layout identifier
            if (string.IsNullOrEmpty(Id) || Id.Equals(layoutId))
            {
                // If there are no column/row definitions then size to available area
                if ((_columns.Count == 0) && (_rows.Count == 0))
                {
                    foreach (UIElement element in elements)
                    {
                        stateDict[element].Element.Measure(availableSize);
                        retSize.Width = Math.Max(retSize.Width, stateDict[element].Element.DesiredSize.Width);
                        retSize.Height = Math.Max(retSize.Height, stateDict[element].Element.DesiredSize.Height);
                    }
                }
                else
                {
                    BuildProxyColumns();
                    BuildProxyRows();
                    PreMeasure(availableSize);
                    MeasureElements(stateDict, elements);
                    UpdateDefinitions(stateDict, elements, false);
                    UpdateDefinitions(stateDict, elements, true);
                    CalculateOffsetsAndTotals();
                    retSize = CalculateDesiredSize(stateDict, elements, availableSize);
                }
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
        public override void TargetChildren(string layoutId,
                                            MetaPanelBase metaPanel,
                                            MetaElementStateDict stateDict,
                                            ICollection elements,
                                            Size finalSize)
        {
            // Only apply if we match the incoming layout identifier
            if (string.IsNullOrEmpty(Id) || Id.Equals(layoutId))
            {
                // If there are no column/row definitions then arrange to entire final size
                if ((_columns.Count == 0) && (_rows.Count == 0))
                {
                    Rect newTargetRect = new Rect(Utility.PointZero, finalSize);
                    foreach (UIElement element in elements)
                    {
                        // We ignore items being removed
                        if (stateDict[element].Status != MetaElementStatus.Removing)
                        {
                            // Store the new target rectangle
                            if (!stateDict[element].TargetRect.Equals(newTargetRect))
                            {
                                stateDict[element].TargetChanged = true;
                                stateDict[element].TargetRect = newTargetRect;
                            }
                        }
                    }
                }
                else
                {
                    // Calculate final width/height of col/row definitions
                    PreTarget(finalSize);

                    // Update new target rectangle for each non-removing element
                    foreach (UIElement element in elements)
                    {
                        // We ignore items being removed
                        if (stateDict[element].Status != MetaElementStatus.Removing)
                        {
                            // Find the arrange size for the element
                            Rect newTargetRect = TargetArrangeSize(element);

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

            // Update rows and columns with actual widths/heights
            for (int i = 0; i < ColumnDefinitions.Count; i++)
                ColumnDefinitions[i].ActualWidth = _proxyColumns[i].MinSize;

            for (int i = 0; i < RowDefinitions.Count; i++)
                RowDefinitions[i].ActualHeight = _proxyRows[i].MinSize;
        }
        #endregion

        #region Private
        private void BuildProxyColumns()
        {
            // Is there a change to the column definitions?
            if (_columns.IsDirty)
            {
                // Do we need to recreate the proxy array?
                if ((_proxyColumns == null) || (_proxyColumns.Length != _columns.Count))
                {
                    // Always need at least one column
                    _proxyColumns = new DefinitionProxy[Math.Max(1, _columns.Count)];
                }

                // Hook each column into the matching proxy
                for (int i = 0; i < _columns.Count; i++)
                    _proxyColumns[i] = new DefinitionProxy(_columns[i]);

                // Our artifical column needs to point at a real column definition
                if (_columns.Count == 0)
                    _proxyColumns[0] = new DefinitionProxy(new ColumnDefinition());
            }
        }

        private void BuildProxyRows()
        {
            // Is there a change to the row definitions?
            if (_rows.IsDirty)
            {
                // Do we need to recreate the proxy array?
                if ((_proxyRows == null) || (_proxyRows.Length != _rows.Count))
                {
                    // Always need at least one row
                    _proxyRows = new DefinitionProxy[Math.Max(1, _rows.Count)];
                }

                // Hook each row into the matching proxy
                for (int i = 0; i < _rows.Count; i++)
                    _proxyRows[i] = new DefinitionProxy(_rows[i]);

                // Our artifical row needs to point at a real row definition
                if (_columns.Count == 0)
                    _proxyRows[0] = new DefinitionProxy(new RowDefinition());
            }
        }

        private void PreMeasure(Size availableSize)
        {
            _columnStars = PreMeasure(availableSize.Width, _proxyColumns);
            _rowStars = PreMeasure(availableSize.Height, _proxyRows);
        }

        private double PreMeasure(double size, DefinitionProxy[] proxies)
        {
            double stars = 0.0;
            foreach (DefinitionProxy proxy in proxies)
                stars += proxy.PreMeasure(size);

            if (stars > 0.0)
                PreTargetStarDefinitions(size, proxies, stars);

            return stars;
        }

        private void MeasureElements(MetaElementStateDict stateDict,
                                     ICollection elements)
        {
            foreach (UIElement element in elements)
            {
                // Find the row/col definitions for this element
                int col = Math.Max(0, Math.Min(_columns.Count - 1, GetColumn(element)));
                int row = Math.Max(0, Math.Min(_rows.Count - 1, GetRow(element)));

                // Find the row/col spanning definitions for this element (convert to zero based spanning)
                int colSpan = Math.Max(Math.Min(_columns.Count - col, GetColumnSpan(element)), 1) - 1;
                int rowSpan = Math.Max(Math.Min(_rows.Count - row, GetRowSpan(element)), 1) - 1;

                // Find column width to use for measuring
                int index = col;
                double measureWidth = 0;
                do
                {
                    measureWidth += _proxyColumns[index].MeasureSize;

                } while ((measureWidth < double.PositiveInfinity) && (index++ < (col + colSpan)));

                // Find row height to use for measuring
                index = row;
                double measureHeight = 0;
                do
                {
                    measureHeight += _proxyRows[index].MeasureSize;

                } while ((measureWidth < double.PositiveInfinity) && (index++ < (row + rowSpan)));

                stateDict[element].Element.Measure(new Size(measureWidth, measureHeight));
            }
        }

        private void UpdateDefinitions(MetaElementStateDict stateDict,
                                       ICollection elements,
                                       bool spanning)
        {
            foreach (UIElement element in elements)
            {
                // Find the row/col definitions for this element
                int col = Math.Max(0, Math.Min(_columns.Count - 1, GetColumn(element)));
                int row = Math.Max(0, Math.Min(_rows.Count - 1, GetRow(element)));

                // Find the row/col spanning definitions for this element (convert to zero based spanning)
                int colSpan = Math.Max(Math.Min(_columns.Count - col, GetColumnSpan(element)), 1) - 1;
                int rowSpan = Math.Max(Math.Min(_rows.Count - row, GetRowSpan(element)), 1) - 1;

                // If cell covers a single column then update the minimum size of column with desired width
                if (!spanning && (colSpan == 0))
                    _proxyColumns[col].UpdateFromMeasure(stateDict[element].Element.DesiredSize.Width);

                if (spanning && (colSpan > 0))
                {
                    // Quantity of desired width to be allocated
                    double desiredWidth = stateDict[element].Element.DesiredSize.Width;

                    // Scan all spanning columns and remove the already allocated space from desired width
                    int index = col;
                    int autoColumns = 0;
                    do
                    {
                        desiredWidth -= _proxyColumns[index].MinSize;
                        if (_proxyColumns[index].UserGridUnitType == GridUnitType.Auto)
                            autoColumns++;

                    } while (index++ < (col + colSpan));

                    // If there is still some width left to allocate and something to allocate into
                    if ((desiredWidth > 0) && (autoColumns > 0))
                    {
                        index = col;
                        do
                        {
                            if (_proxyColumns[index].UserGridUnitType == GridUnitType.Auto)
                            {
                                double extraSpace = desiredWidth / autoColumns--;
                                desiredWidth -= extraSpace;
                                _proxyColumns[index].UpdateFromMeasure(_proxyColumns[index].MinSize + extraSpace);
                            }

                        } while (index++ < (col + colSpan));
                    }
                }

                // If cell covers a single row then update the minimum size of column with desired height
                if (!spanning & (rowSpan == 0))
                    _proxyRows[row].UpdateFromMeasure(stateDict[element].Element.DesiredSize.Height);

                if (spanning && (rowSpan > 0))    
                {
                    // Quantity of desired height to be allocated
                    double desiredHeight = stateDict[element].Element.DesiredSize.Height;

                    // Scan all spanning columns and remove the already allocated space from desired height
                    int index = col;
                    int autoRows = 0;
                    do
                    {
                        desiredHeight -= _proxyColumns[index].MinSize;
                        if (_proxyColumns[index].UserGridUnitType == GridUnitType.Auto)
                            autoRows++;

                    } while (index++ < (col + colSpan));

                    // If there is still some height left to allocate and something to allocate into
                    if ((desiredHeight > 0) && (autoRows > 0))
                    {
                        index = col;
                        do
                        {
                            if (_proxyColumns[index].UserGridUnitType == GridUnitType.Auto)
                            {
                                double extraSpace = desiredHeight / autoRows--;
                                desiredHeight -= extraSpace;
                                _proxyColumns[index].UpdateFromMeasure(_proxyColumns[index].MinSize + extraSpace);
                            }

                        } while (index++ < (col + colSpan));
                    }
                }
            }
        }

        private void CalculateOffsetsAndTotals()
        {
            CalculateOffsetsAndTotals(_proxyColumns);
            CalculateOffsetsAndTotals(_proxyRows);
        }

        private void CalculateOffsetsAndTotals(DefinitionProxy[] proxies)
        {
            double totalMin = 0;
            double totalDesired = 0;
            foreach (DefinitionProxy proxy in proxies)
            {
                proxy.MinTotal = totalMin;
                proxy.DesiredTotal = totalDesired;
                totalMin += proxy.MinSize;
                totalDesired += proxy.DesiredSize;
            }
        }

        private Size CalculateDesiredSize(MetaElementStateDict stateDict,
                                          ICollection elements,
                                          Size availableSize)
        {
            double width = _proxyColumns[_proxyColumns.Length - 1].MinTotal + _proxyColumns[_proxyColumns.Length - 1].MinSize;
            double height = _proxyRows[_proxyRows.Length - 1].MinTotal + _proxyRows[_proxyRows.Length - 1].MinSize;

            // If any of the columns is marked as Star
            if (HasColumnStars)
            {
                // If we have infinite space then return the measured width needed for all columns, otherwise we just take all the space
                if (availableSize.Width == double.PositiveInfinity)
                    width = _proxyColumns[_proxyColumns.Length - 1].DesiredTotal + _proxyColumns[_proxyColumns.Length - 1].DesiredSize;
                else
                    width = availableSize.Width;
            }

            // If any of the rows is marked as Star
            if (HasRowStars)
            {
                // If we have infinite space then return the measured height needed for all rows, otherwise we just take all the space
                if (availableSize.Height == double.PositiveInfinity)
                    height = _proxyRows[_proxyRows.Length - 1].DesiredTotal + _proxyRows[_proxyRows.Length - 1].DesiredSize;
                else
                    height = availableSize.Height;
            }

            return new Size(width, height);
        }

        private void PreTarget(Size availableSize)
        {
            // Need to allocate space to Star columns
            if (_columnStars > 0.0)
                PreTargetStarDefinitions(availableSize.Width, _proxyColumns, _columnStars);

            // Need to allocate space to Star rows
            if (_rowStars > 0.0)
                PreTargetStarDefinitions(availableSize.Height, _proxyRows, _rowStars);
            
            // Need to recalculate the offsets for each column
            CalculateOffsetsAndTotals();
        }

        private void PreTargetStarDefinitions(double size, DefinitionProxy[] proxies, double stars)
        {
            // Find the size that needs allocating to Star defs (by removing Auto/Fixed sizes from incoming total size)
            int starDefs = 0;
            foreach (DefinitionProxy proxy in proxies)
                if (proxy.UserGridUnitType != GridUnitType.Star)
                    size -= proxy.MinSize;
                else
                    starDefs++;

            // Is there any size left to allocate to the Star defs?
            if (size > 0)
            {
                // Handle defs where the allocated space is outside the min/max limits
                foreach (DefinitionProxy proxy in proxies)
                    if (proxy.UserGridUnitType == GridUnitType.Star)
                    {
                        // Only interested if the definition has a defined min or max value
                        if ((proxy.DefUserMin > 0) || (proxy.DefUserMax < double.PositiveInfinity))
                        {
                            // How much size should this definition have based on its Star value
                            double allocateSize = size;
                            if (starDefs > 1)
                                allocateSize = size / stars * proxy.DefUserSize.Value;

                            // If allocated size is less than the defined minimum
                            if ((proxy.DefUserMin > 0) && (allocateSize < proxy.DefUserMin))
                            {
                                // Enfore minimum setting
                                proxy.StarAllocated = true;
                                proxy.MinSize = proxy.DefUserMin;
                                proxy.MeasureSize = proxy.DefUserMin;
                                stars -= proxy.DefUserSize.Value;
                                size -= proxy.MinSize;
                                starDefs--;
                            }
                            else if ((proxy.DefUserMax < double.PositiveInfinity) && (allocateSize > proxy.DefUserMax))
                            {
                                // Enfore maximum setting
                                proxy.StarAllocated = true;
                                proxy.MinSize = proxy.DefUserMax;
                                proxy.MeasureSize = proxy.DefUserMax;
                                stars -= proxy.DefUserSize.Value;
                                size -= proxy.MinSize;
                                starDefs--;
                            }
                        }
                    }

                // Any more Star defs to allocate?
                if ((starDefs > 0) && (size > 0))
                {
                    // Allocate remaining space to remaining star defs according to Star values
                    foreach (DefinitionProxy proxy in proxies)
                        if ((proxy.UserGridUnitType == GridUnitType.Star) && !proxy.StarAllocated)
                        {
                            // How much size should this definition have based on its Star value
                            double allocateSize = size;
                            if (starDefs > 1)
                                allocateSize = size / stars * proxy.DefUserSize.Value;

                            proxy.StarAllocated = true;
                            proxy.MinSize = allocateSize;
                            proxy.MeasureSize = allocateSize;
                            stars -= proxy.DefUserSize.Value;
                            size -= proxy.MinSize;
                            starDefs--;
                        }
                }
            }

            // Reset the star allocated field
            foreach (DefinitionProxy proxy in proxies)
                if (proxy.UserGridUnitType == GridUnitType.Star)
                    proxy.StarAllocated = false;
        }

        private Rect TargetArrangeSize(UIElement element)
        {
            // Find the row/col definitions for this element
            int col = Math.Max(0, Math.Min(_columns.Count - 1, GetColumn(element)));
            int row = Math.Max(0, Math.Min(_rows.Count - 1, GetRow(element)));

            // Find the row/col spanning definitions for this element (convert to zero based spanning)
            int colSpan = Math.Max(Math.Min(_columns.Count - col, GetColumnSpan(element)), 1) - 1;
            int rowSpan = Math.Max(Math.Min(_rows.Count - row, GetRowSpan(element)), 1) - 1;

            return new Rect(_proxyColumns[col].MinTotal, 
                            _proxyRows[row].MinTotal, 
                            _proxyColumns[col + colSpan].MinTotal + _proxyColumns[col + colSpan].MinSize - _proxyColumns[col].MinTotal,
                            _proxyRows[row + rowSpan].MinTotal + _proxyRows[row + rowSpan].MinSize - _proxyRows[row].MinTotal);

        }

        private bool HasColumnStars
        {
            get { return _columnStars > 0.0; }
        }

        private bool HasRowStars
        {
            get { return _rowStars > 0.0; }
        }

        private bool HasStars
        {
            get { return HasColumnStars || HasRowStars; }
        }

        private static void OnAttachedPropertyChanged(DependencyObject d,
                                                      DependencyPropertyChangedEventArgs e)
        {
            UIElement element = d as UIElement;
            if (element != null)
            {
                MetaPanel parent = VisualTreeHelper.GetParent(element) as MetaPanel;
                if (parent != null)
                    parent.InvalidateMeasure();
            }
        }
        #endregion
    }
}
