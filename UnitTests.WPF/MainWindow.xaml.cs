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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ComponentFactory.Quicksilver.UnitTests.WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TestArithmetic_Click(object sender, RoutedEventArgs e)
        {
            UnitTestArithmetic.PerformTests();
            Message.Text = "Finish TestArithmetic";
        }

        private void TestEval_Click(object sender, RoutedEventArgs e)
        {
            UnitTestEval.PerformTests();
            Message.Text = "Finish TestEval";
        }

        private void TestLayouts_Click(object sender, RoutedEventArgs e)
        {
            UnitTestLayouts.PerformTests();
            Message.Text = "Finish TestLayouts";
        }
    }
}
