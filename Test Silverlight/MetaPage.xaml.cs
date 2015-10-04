using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using ComponentFactory.Quicksilver.Layout;

namespace TestSilverlight
{
    public partial class MetaPage : UserControl
    {
        private Random _random = new Random();
        private Easing[] _easings = new Easing[]
        {
            Easing.Linear,
            Easing.QuadraticIn,
            Easing.QuadraticOut,
            Easing.QuadraticInOut,
            Easing.CubicIn,
            Easing.CubicOut,
            Easing.CubicInOut,
            Easing.QuarticIn,
            Easing.QuarticOut,
            Easing.QuarticInOut,
            Easing.QuinticIn,
            Easing.QuinticOut,
            Easing.QuinticInOut,
            Easing.SineIn,
            Easing.SineOut,
            Easing.SineInOut,
            Easing.CircularIn,
            Easing.CircularOut,
            Easing.CircularInOut,
            Easing.ExponentialIn,
            Easing.ExponentialOut,
            Easing.ExponentialInOut,
            Easing.BounceIn,
            Easing.BounceOut,
            Easing.BounceInOut,
            Easing.BackIn,
            Easing.BackOut,
            Easing.BackInOut,
        };
        private AnimateLocation[] _location = new AnimateLocation[]
        {
            AnimateLocation.Target,
            AnimateLocation.Center,
            AnimateLocation.Top,
            AnimateLocation.TopPaged,
            AnimateLocation.Bottom,
            AnimateLocation.BottomPaged,
            AnimateLocation.Left,
            AnimateLocation.LeftPaged,
            AnimateLocation.Right,
            AnimateLocation.RightPaged,
            AnimateLocation.TopLeft,
            AnimateLocation.TopRight,
            AnimateLocation.BottomLeft,
            AnimateLocation.BottomRight,
            AnimateLocation.NearestEdge,
            AnimateLocation.NearestEdgePaged,
        };
        private AnimateSize[] _size = new AnimateSize[]
        {
            AnimateSize.Original,
            AnimateSize.ZeroZeroCenter,
            AnimateSize.ZeroZeroTopLeft,
            AnimateSize.ZeroZeroTopRight,
            AnimateSize.ZeroZeroBottomLeft,
            AnimateSize.ZeroZeroBottomRight,
            AnimateSize.ZeroWidthLeft,
            AnimateSize.ZeroWidthCenter,
            AnimateSize.ZeroWidthRight,
            AnimateSize.ZeroHeightTop,
            AnimateSize.ZeroHeightCenter,
            AnimateSize.ZeroHeightBottom,
        };

        public MetaPage()
        {
            InitializeComponent();

            // FindName does not work with embedded items so we need to find them ourselves
            AnimateDefinitions ac = (AnimateDefinitions)TargetPanel.AnimateDefinitions;
            NewOpacity = (NewOpacityAnimate)ac[0];
            RemoveOpacity = (RemoveOpacityAnimate)ac[1];
            NewPosition = (NewPositionAnimate)ac[2];
            MovePosition = (MovePositionAnimate)ac[3];
            RemovePosition = (RemovePositionAnimate)ac[4];
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Button b = new Button();
            b.Content = DateTime.Now.Millisecond;
            b.Padding = new Thickness(5);
            b.SetValue(ComponentFactory.Quicksilver.Layout.CanvasLayout.LeftProperty, (double)_random.Next(300));
            b.SetValue(ComponentFactory.Quicksilver.Layout.CanvasLayout.TopProperty, (double)_random.Next(300));
            b.SetValue(ComponentFactory.Quicksilver.Layout.DockLayout.DockProperty, (Dock)_random.Next(4));
            b.SetValue(ComponentFactory.Quicksilver.Layout.GridLayout.ColumnProperty, _random.Next(16));
            b.SetValue(ComponentFactory.Quicksilver.Layout.GridLayout.RowProperty, _random.Next(16));
            b.SetValue(ComponentFactory.Quicksilver.Layout.GridLayout.ColumnSpanProperty, _random.Next(2) + 1);
            b.SetValue(ComponentFactory.Quicksilver.Layout.GridLayout.RowSpanProperty, _random.Next(2) + 1);
            TargetPanel.Children.Insert(_random.Next(TargetPanel.Children.Count), b);
        }

        private void Add5_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 5; i++)
                Add_Click(sender, e);
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (TargetPanel.Children.Count > 0)
                TargetPanel.RemoveChild(TargetPanel.Children[_random.Next(TargetPanel.Children.Count - 1)]);
        }

        private void Remove5_Click(object sender, RoutedEventArgs e)
        {
            int remove = Math.Min(TargetPanel.Children.Count, 5);
            if (remove > 0)
            {
                int index = _random.Next(TargetPanel.Children.Count - remove);
                for (int i = 0; i < remove; i++)
                    TargetPanel.RemoveChild(TargetPanel.Children[index + i]);
            }
        }

        private void Both5_Click(object sender, RoutedEventArgs e)
        {
            Remove5_Click(sender, e);

            for (int i = 0; i < 5; i++)
                Add_Click(sender, e);
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            TargetPanel.ClearChildren();
        }

        private void Canvas_Click(object sender, RoutedEventArgs e)
        {
            ComponentFactory.Quicksilver.Layout.CanvasLayout layout = new ComponentFactory.Quicksilver.Layout.CanvasLayout();
            TargetPanel.LayoutDefinitions.Clear();
            TargetPanel.LayoutDefinitions.Add(layout);
        }

        private void Wrap_Click(object sender, RoutedEventArgs e)
        {
            ComponentFactory.Quicksilver.Layout.WrapLayout layout = new ComponentFactory.Quicksilver.Layout.WrapLayout();
            TargetPanel.LayoutDefinitions.Clear();
            TargetPanel.LayoutDefinitions.Add(layout);
        }

        private void Stack_Click(object sender, RoutedEventArgs e)
        {
            ComponentFactory.Quicksilver.Layout.StackLayout layout = new ComponentFactory.Quicksilver.Layout.StackLayout();
            TargetPanel.LayoutDefinitions.Clear();
            TargetPanel.LayoutDefinitions.Add(layout);
        }

        private void Stretch_Click(object sender, RoutedEventArgs e)
        {
            ComponentFactory.Quicksilver.Layout.StretchLayout layout = new ComponentFactory.Quicksilver.Layout.StretchLayout();
            TargetPanel.LayoutDefinitions.Clear();
            TargetPanel.LayoutDefinitions.Add(layout);
        }

        private void Grid_Click(object sender, RoutedEventArgs e)
        {
            ComponentFactory.Quicksilver.Layout.GridLayout layout = new ComponentFactory.Quicksilver.Layout.GridLayout();
            for (int i = 0; i < 16; i++)
            {
                layout.ColumnDefinitions.Add(new ComponentFactory.Quicksilver.Layout.ColumnDefinition());
                layout.RowDefinitions.Add(new ComponentFactory.Quicksilver.Layout.RowDefinition());
                layout.ColumnDefinitions[i].Width = new GridLength(1.0, GridUnitType.Star);
                layout.RowDefinitions[i].Height = new GridLength(1.0, GridUnitType.Star);
            }
            TargetPanel.LayoutDefinitions.Clear();
            TargetPanel.LayoutDefinitions.Add(layout);
        }

        private void UniformGrid_Click(object sender, RoutedEventArgs e)
        {
            ComponentFactory.Quicksilver.Layout.UniformGridLayout layout = new ComponentFactory.Quicksilver.Layout.UniformGridLayout();
            TargetPanel.LayoutDefinitions.Clear();
            TargetPanel.LayoutDefinitions.Add(layout);
        }

        private void Dock_Click(object sender, RoutedEventArgs e)
        {
            ComponentFactory.Quicksilver.Layout.DockLayout layout = new ComponentFactory.Quicksilver.Layout.DockLayout();
            TargetPanel.LayoutDefinitions.Clear();
            TargetPanel.LayoutDefinitions.Add(layout);
        }

        private void Radial_Click(object sender, RoutedEventArgs e)
        {
            ComponentFactory.Quicksilver.Layout.RadialLayout layout = new ComponentFactory.Quicksilver.Layout.RadialLayout();
            TargetPanel.LayoutDefinitions.Clear();
            TargetPanel.LayoutDefinitions.Add(layout);
        }

        private void Attached_Click(object sender, RoutedEventArgs e)
        {
            foreach (UIElement child in TargetPanel.Children)
            {
                child.SetValue(ComponentFactory.Quicksilver.Layout.CanvasLayout.LeftProperty, (double)_random.Next(300));
                child.SetValue(ComponentFactory.Quicksilver.Layout.CanvasLayout.TopProperty, (double)_random.Next(300));
                child.SetValue(ComponentFactory.Quicksilver.Layout.DockLayout.DockProperty, (Dock)_random.Next(4));
                child.SetValue(ComponentFactory.Quicksilver.Layout.GridLayout.ColumnProperty, _random.Next(16));
                child.SetValue(ComponentFactory.Quicksilver.Layout.GridLayout.RowProperty, _random.Next(16));
                child.SetValue(ComponentFactory.Quicksilver.Layout.GridLayout.ColumnSpanProperty, _random.Next(2) + 1);
                child.SetValue(ComponentFactory.Quicksilver.Layout.GridLayout.RowSpanProperty, _random.Next(2) + 1);
                child.SetValue(ComponentFactory.Quicksilver.Layout.WrapLayout.BreakAfterProperty, _random.Next(10) == 5);
            }
        }

        private void WrapDO_Click(object sender, RoutedEventArgs e)
        {
            if (TargetPanel.LayoutDefinitions[0] is WrapLayout)
            {
                WrapLayout wl = (WrapLayout)TargetPanel.LayoutDefinitions[0];
                if (wl.Orientation == Orientation.Horizontal)
                    wl.Orientation = Orientation.Vertical;
                else
                    wl.Orientation = Orientation.Horizontal;

                if (wl.ItemHeight != 80)
                    wl.ItemHeight = 80;
                else
                    wl.ItemHeight = double.NaN;

                if (wl.ItemWidth != 80)
                    wl.ItemWidth = 80;
                else
                    wl.ItemWidth = double.NaN;
            }
        }

        private void StackDO_Click(object sender, RoutedEventArgs e)
        {
            if (TargetPanel.LayoutDefinitions[0] is StackLayout)
            {
                StackLayout sl = (StackLayout)TargetPanel.LayoutDefinitions[0];
                if (sl.Orientation == Orientation.Horizontal)
                    sl.Orientation = Orientation.Vertical;
                else
                    sl.Orientation = Orientation.Horizontal;
            }
        }

        private void GridDO_Click(object sender, RoutedEventArgs e)
        {
            if (TargetPanel.LayoutDefinitions[0] is GridLayout)
            {
                GridLayout gl = (GridLayout)TargetPanel.LayoutDefinitions[0];

                if (gl.ColumnDefinitions[0].Width.IsStar)
                {
                    for (int i = 0; i < 16; i++)
                    {
                        gl.ColumnDefinitions[i].Width = new GridLength();
                        gl.RowDefinitions[i].Height = new GridLength();
                    }
                }
                else
                {
                    for (int i = 0; i < 16; i++)
                    {
                        gl.ColumnDefinitions[i].Width = new GridLength(1.0, GridUnitType.Star);
                        gl.RowDefinitions[i].Height = new GridLength(1.0, GridUnitType.Star);
                    }
                }
            }
        }

        private void UniformGridDO_Click(object sender, RoutedEventArgs e)
        {
            if (TargetPanel.LayoutDefinitions[0] is UniformGridLayout)
            {
                UniformGridLayout ugl = (UniformGridLayout)TargetPanel.LayoutDefinitions[0];

                if (ugl.Columns != 10)
                    ugl.Columns = 10;
                else
                    ugl.Columns = 0;

                if (ugl.Rows != 10)
                    ugl.Rows = 10;
                else
                    ugl.Rows = 0;

                if (ugl.FirstColumn != 5)
                    ugl.FirstColumn = 5;
                else
                    ugl.FirstColumn = 0;
            }
        }

        private void DockDO_Click(object sender, RoutedEventArgs e)
        {
            if (TargetPanel.LayoutDefinitions[0] is DockLayout)
            {
                DockLayout dl = (DockLayout)TargetPanel.LayoutDefinitions[0];
                dl.LastChildFill = !dl.LastChildFill;
            }
        }

        private void RadialDO_Click(object sender, RoutedEventArgs e)
        {
            if (TargetPanel.LayoutDefinitions[0] is RadialLayout)
            {
                RadialLayout rl = (RadialLayout)TargetPanel.LayoutDefinitions[0];
                rl.Circle = !rl.Circle;
                rl.StartAngle = _random.Next(360);
                rl.EndAngle = _random.Next(360);
            }
        }

        private void Duration_Click(object sender, RoutedEventArgs e)
        {
            double d = _random.Next(2000);
            NewOpacity.Duration = d;
            RemoveOpacity.Duration = d;
            NewPosition.Duration = d;
            MovePosition.Duration = d;
            RemovePosition.Duration = d;
        }

        private void Easing_Click(object sender, RoutedEventArgs e)
        {
            Easing easing = _easings[_random.Next(_easings.Length)];
            NewOpacity.Easing = easing;
            RemoveOpacity.Easing = easing;
            NewPosition.Easing = easing;
            MovePosition.Easing = easing;
            RemovePosition.Easing = easing;
        }

        private void Opacity_Click(object sender, RoutedEventArgs e)
        {
            double start = ((double)_random.Next(50)) / 100;
            double end = (50.0 + (double)_random.Next(50)) / 100;
            NewOpacity.Start = start;
            NewOpacity.End = end;
            RemoveOpacity.End = start;
            RemoveOpacity.Start = end;
        }

        private void Location_Click(object sender, RoutedEventArgs e)
        {
            AnimateLocation l = _location[_random.Next(_location.Length)];
            NewPosition.Location = l;
            RemovePosition.Location = l;
        }

        private void Size_Click(object sender, RoutedEventArgs e)
        {
            AnimateSize s = _size[_random.Next(_size.Length)];
            NewPosition.Size = s;
            RemovePosition.Size = s;
        }
    }
}
