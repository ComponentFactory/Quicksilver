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
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ComponentFactory.Quicksilver.Layout
{
    /// <summary>
    /// MetaPanel predefined with WrapLayout appropriate settings.
    /// </summary>
    public partial class MetaWrapPanel : FixedMetaPanelBase
    {
        #region Dependancy Properties
        /// <summary>
        /// Identifies the ItemWidth dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemWidthProperty;

        /// <summary>
        /// Identifies the ItemHeight dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemHeightProperty;

        /// <summary>
        /// Identifies the Orientation dependency property.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty;

        /// <summary>
        /// Identifies the Break dependency property.
        /// </summary>
        public static readonly DependencyProperty BreakAfterProperty;
        #endregion

        #region Identity
        static MetaWrapPanel()
        {
            ItemWidthProperty = DependencyProperty.Register("ItemWidth", 
                                                            typeof(double),
                                                            typeof(MetaWrapPanel), 
                                                            new PropertyMetadata(double.NaN, 
                                                            new PropertyChangedCallback(OnNeedMeasureOnChanged)));

            ItemHeightProperty = DependencyProperty.Register("ItemHeight",
                                                             typeof(double),
                                                             typeof(MetaWrapPanel),
                                                             new PropertyMetadata(double.NaN,
                                                             new PropertyChangedCallback(OnNeedMeasureOnChanged)));

            OrientationProperty = DependencyProperty.Register("Orientation",
                                                             typeof(Orientation),
                                                             typeof(MetaWrapPanel),
                                                             new PropertyMetadata(Orientation.Horizontal,
                                                             new PropertyChangedCallback(OnNeedMeasureOnChanged)));

            BreakAfterProperty = DependencyProperty.RegisterAttached("BreakAfter",
                                                                     typeof(bool),
                                                                     typeof(MetaWrapPanel),
                                                                     new PropertyMetadata(false,
                                                                     new PropertyChangedCallback(OnWrapChanged)));
        }

        /// <summary>
        /// Initialize a new instance of the MetaWrapPanel class.
        /// </summary>
        public MetaWrapPanel()
        {
            WrapLayout layout = new WrapLayout();
            layout.SetBinding(WrapLayout.ItemWidthProperty, ThisBinding("ItemWidth"));
            layout.SetBinding(WrapLayout.ItemHeightProperty, ThisBinding("ItemHeight"));
            layout.SetBinding(WrapLayout.OrientationProperty, ThisBinding("Orientation"));
            layout.SetBinding(WrapLayout.BreakAfterProperty, ThisBinding("BreakAfter"));
            Layouts.Add(layout);
        }
        #endregion

        #region Public
        /// <summary>
        /// Gets or sets a value that specifies the width of all items that are contained within a WrapPanel.
        /// </summary>
        public double ItemWidth
        {
            get { return (double)GetValue(ItemWidthProperty); }
            set { SetValue(ItemWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies the height of all items that are contained within a WrapPanel.
        /// </summary>
        public double ItemHeight
        {
            get { return (double)GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies the dimension in which child content is arranged.
        /// </summary>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /// <summary>
        /// Sets the BreakAfter attached property of the provided element.
        /// </summary>
        /// <param name="element">Target element.</param>
        /// <param name="value">BreakAfter value.</param>
        public static void SetBreakAfter(UIElement element, bool value)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            else
                element.SetValue(BreakAfterProperty, value);
        }

        /// <summary>
        /// Gets the BreakAfter attached property of the provided element.
        /// </summary>
        /// <param name="element">Target element.</param>
        /// <returns>BreakAfter value.</returns>
        public static bool GetBreakAfter(UIElement element)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            else
                return (bool)element.GetValue(BreakAfterProperty);
        }
        #endregion

        #region Private
        private static void OnWrapChanged(DependencyObject d,
                                          DependencyPropertyChangedEventArgs e)
        {
            UIElement element = d as UIElement;
            if (element != null)
            {
                MetaPanelBase parent = VisualTreeHelper.GetParent(element) as MetaPanelBase;
                if (parent != null)
                    parent.InvalidateMeasure();
            }
        }
        #endregion
    }
}
