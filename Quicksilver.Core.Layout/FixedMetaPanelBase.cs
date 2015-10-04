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
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ComponentFactory.Quicksilver.Layout
{
    /// <summary>
    /// Base for predefined MetaPanelBase classes.
    /// </summary>
    public abstract class FixedMetaPanelBase : MetaPanelBase
    {
        #region Constants
        private static readonly double DEFAULT_START_OPACITY = 0.0;
        private static readonly double DEFAULT_END_OPACITY = 0.0;
        #endregion

        #region Dependancy Properties
        /// <summary>
        /// Identifies the Duration dependency property.
        /// </summary>
        public static readonly DependencyProperty DurationProperty;

        /// <summary>
        /// Identifies the Easing dependency property.
        /// </summary>
        public static readonly DependencyProperty EasingProperty;

        /// <summary>
        /// Identifies the StartOpacity dependency property.
        /// </summary>
        public static readonly DependencyProperty StartOpacityProperty;

        /// <summary>
        /// Identifies the RemoveEndOpacity dependency property.
        /// </summary>
        public static readonly DependencyProperty EndOpacityProperty;

        /// <summary>
        /// Identifies the NewLocation dependency property.
        /// </summary>
        public static readonly DependencyProperty NewLocationProperty;

        /// <summary>
        /// Identifies the NewSize dependency property.
        /// </summary>
        public static readonly DependencyProperty NewSizeProperty;
        
        /// <summary>
        /// Identifies the RemoveLocation dependency property.
        /// </summary>
        public static readonly DependencyProperty RemoveLocationProperty;

        /// <summary>
        /// Identifies the RemoveSize dependency property.
        /// </summary>
        public static readonly DependencyProperty RemoveSizeProperty;
        #endregion

        #region Identity
        static FixedMetaPanelBase()
        {
            DurationProperty = DependencyProperty.Register("Duration",
                                                           typeof(double),
                                                           typeof(FixedMetaPanelBase),
                                                           new PropertyMetadata((double)EasingAnimate.DEFAULT_DURATION));

            EasingProperty = DependencyProperty.Register("Easing",
                                                         typeof(Easing),
                                                         typeof(FixedMetaPanelBase),
                                                         new PropertyMetadata((Easing)EasingAnimate.DEFAULT_EASING));

            StartOpacityProperty = DependencyProperty.Register("StartOpacity",
                                                               typeof(double),
                                                               typeof(FixedMetaPanelBase),
                                                               new PropertyMetadata((double)DEFAULT_START_OPACITY));

            EndOpacityProperty = DependencyProperty.Register("EndOpacity",
                                                             typeof(double),
                                                             typeof(FixedMetaPanelBase),
                                                             new PropertyMetadata((double)DEFAULT_END_OPACITY));

            NewLocationProperty = DependencyProperty.Register("NewLocation",
                                                              typeof(AnimateLocation),
                                                              typeof(FixedMetaPanelBase),
                                                              new PropertyMetadata(AnimateLocation.Target));

            NewSizeProperty = DependencyProperty.Register("NewSize",
                                                          typeof(AnimateSize),
                                                          typeof(FixedMetaPanelBase),
                                                          new PropertyMetadata(AnimateSize.Original));

            RemoveLocationProperty = DependencyProperty.Register("RemoveLocation",
                                                                 typeof(AnimateLocation),
                                                                 typeof(FixedMetaPanelBase),
                                                                 new PropertyMetadata(AnimateLocation.Target));

            RemoveSizeProperty = DependencyProperty.Register("RemoveSize",
                                                             typeof(AnimateSize),
                                                             typeof(FixedMetaPanelBase),
                                                             new PropertyMetadata(AnimateSize.Original));
        }

        /// <summary>
        /// Initialize a new instance of the FixedMetaPanelBase class.
        /// </summary>
        public FixedMetaPanelBase()
        {
            NewPositionAnimate newPos = new NewPositionAnimate();
            newPos.SetBinding(NewPositionAnimate.DurationProperty, ThisBinding("Duration"));
            newPos.SetBinding(NewPositionAnimate.EasingProperty, ThisBinding("Easing"));
            newPos.SetBinding(NewPositionAnimate.LocationProperty, ThisBinding("NewLocation"));
            newPos.SetBinding(NewPositionAnimate.SizeProperty, ThisBinding("NewSize"));
            Animates.Add(newPos);

            NewOpacityAnimate newOpacity = new NewOpacityAnimate();
            newOpacity.SetBinding(NewOpacityAnimate.DurationProperty, ThisBinding("Duration"));
            newOpacity.SetBinding(NewOpacityAnimate.EasingProperty, ThisBinding("Easing"));
            newOpacity.SetBinding(NewOpacityAnimate.StartProperty, ThisBinding("StartOpacity"));
            Animates.Add(newOpacity);

            MovePositionAnimate movePos = new MovePositionAnimate();
            movePos.SetBinding(MovePositionAnimate.DurationProperty, ThisBinding("Duration"));
            movePos.SetBinding(MovePositionAnimate.EasingProperty, ThisBinding("Easing"));
            Animates.Add(movePos);

            RemovePositionAnimate removePos = new RemovePositionAnimate();
            removePos.SetBinding(RemovePositionAnimate.DurationProperty, ThisBinding("Duration"));
            removePos.SetBinding(RemovePositionAnimate.EasingProperty, ThisBinding("Easing"));
            removePos.SetBinding(RemovePositionAnimate.LocationProperty, ThisBinding("RemoveLocation"));
            removePos.SetBinding(RemovePositionAnimate.SizeProperty, ThisBinding("RemoveSize"));
            Animates.Add(removePos);

            RemoveOpacityAnimate removeOpacity = new RemoveOpacityAnimate();
            removeOpacity.SetBinding(RemoveOpacityAnimate.DurationProperty, ThisBinding("Duration"));
            removeOpacity.SetBinding(RemoveOpacityAnimate.EasingProperty, ThisBinding("Easing"));
            removeOpacity.SetBinding(RemoveOpacityAnimate.EndProperty, ThisBinding("EndOpacity"));
            Animates.Add(removeOpacity);
        }
        #endregion

        #region Public
        /// <summary>
        /// Gets or sets the how long the animation of items should take to reach the new target rectangle.
        /// </summary>
        public double Duration
        {
            get { return (double)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the how to ease items animation values from start to end values.
        /// </summary>
        public Easing Easing
        {
            get { return (Easing)GetValue(EasingProperty); }
            set { SetValue(EasingProperty, value); }
        }

        /// <summary>
        /// Gets and sets the starting opacity for new child items.
        /// </summary>
        public double StartOpacity
        {
            get { return (double)GetValue(StartOpacityProperty); }
            set { SetValue(StartOpacityProperty, value); }
        }

        /// <summary>
        /// Gets and sets the ending opacity for removing child items.
        /// </summary>
        public double EndOpacity
        {
            get { return (double)GetValue(EndOpacityProperty); }
            set { SetValue(EndOpacityProperty, value); }
        }

        /// <summary>
        /// Gets and sets the starting location of new child items.
        /// </summary>
        public AnimateLocation NewLocation
        {
            get { return (AnimateLocation)GetValue(NewLocationProperty); }
            set { SetValue(NewLocationProperty, value); }
        }

        /// <summary>
        /// Gets and sets the starting size of new child items.
        /// </summary>
        public AnimateSize NewSize
        {
            get { return (AnimateSize)GetValue(NewSizeProperty); }
            set { SetValue(NewSizeProperty, value); }
        }

        /// <summary>
        /// Gets and sets the ending location of removing child items.
        /// </summary>
        public AnimateLocation RemoveLocation
        {
            get { return (AnimateLocation)GetValue(RemoveLocationProperty); }
            set { SetValue(RemoveLocationProperty, value); }
        }

        /// <summary>
        /// Gets and sets the ending size of removing child items.
        /// </summary>
        public AnimateSize RemoveSize
        {
            get { return (AnimateSize)GetValue(RemoveSizeProperty); }
            set { SetValue(RemoveSizeProperty, value); }
        }
        #endregion

        #region Protected
        /// <summary>
        /// Create a binding to a property on ourself.
        /// </summary>
        /// <param name="property">Name of property to bind against.</param>
        /// <returns>Binding instance.</returns>
        protected Binding ThisBinding(string property)
        {
            Binding bind = new Binding(property);
            bind.Source = this;
            return bind;
        }
        #endregion
    }
}
