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
using System.Diagnostics;
using System.Windows.Controls.Primitives;

namespace ComponentFactory.Quicksilver.UnitTests.WPF
{
    public class ButtonOutput : Button
    {
        protected override Size MeasureOverride(Size constraint)
        {
            Size retSize = base.MeasureOverride(constraint);
            Console.WriteLine("{2} MeasureOverride {0} {1}", retSize, Content.ToString(), constraint);
            return retSize;
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            Size retSize = base.ArrangeOverride(arrangeBounds);
            Console.WriteLine("{2} ArrangeOverride {0} {1}", retSize, Content.ToString(), arrangeBounds);
            return retSize;
        }
    }

    public class GridOutput : Grid
    {
        protected override Size MeasureOverride(Size constraint)
        {
            Size retSize = base.MeasureOverride(constraint);
            Console.WriteLine("{0} Grid::MeasureOverride {1}", retSize, constraint);
            return retSize;
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            Size retSize = base.ArrangeOverride(arrangeBounds);
            Console.WriteLine("{0} Grid::ArrangeOverride {1}", retSize, arrangeBounds);
            return retSize;
        }
    }

    public class UniformGridOutput : UniformGrid
    {
        protected override Size MeasureOverride(Size constraint)
        {
            Size retSize = base.MeasureOverride(constraint);
            Console.WriteLine("{0} UFG::MeasureOverride {1}", retSize, constraint);
            return retSize;
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            Size retSize = base.ArrangeOverride(arrangeBounds);
            Console.WriteLine("{0} UFG::ArrangeOverride {1}", retSize, arrangeBounds);
            return retSize;
        }
    }
}
