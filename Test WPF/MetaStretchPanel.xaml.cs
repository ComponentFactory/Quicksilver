using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TestWPF
{
    /// <summary>
    /// Interaction logic for MetaStretchPanel.xaml
    /// </summary>
    public partial class MetaStretchPanel : Window
    {
        private Random _random = new Random();
        public MetaStretchPanel()
        {
            InitializeComponent();
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
    }
}
