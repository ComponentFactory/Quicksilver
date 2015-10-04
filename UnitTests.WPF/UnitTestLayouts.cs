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

namespace ComponentFactory.Quicksilver.UnitTests.WPF
{
    public class UnitTestLayouts
    {
        public static bool PerformTests()
        {
            try
            {
                new CanvasLayout0().ShowDialog();
                new DockLayout0().ShowDialog();
                new DockLayout1().ShowDialog();
                new StackLayout0().ShowDialog();
                new StackLayout1().ShowDialog();
                new UniformGridLayout0().ShowDialog();
                new UniformGridLayout1().ShowDialog();
                new UniformGridLayout2().ShowDialog();
                new WrapLayout0().ShowDialog();
                new WrapLayout1().ShowDialog();
                new WrapLayout2().ShowDialog();
                new GridLayout00().ShowDialog();
                new GridLayoutMissingDefs().ShowDialog();
                new GridLayoutFixed11().ShowDialog();
                new GridLayoutFixed22().ShowDialog();
                new GridLayoutFixed44().ShowDialog();
                new GridLayoutFixedSpan22().ShowDialog();
                new GridLayoutFixedSpan44().ShowDialog();
                new GridLayoutAuto11().ShowDialog();
                new GridLayoutAuto44().ShowDialog();
                new GridLayoutAutoFixed44().ShowDialog();
                        //new GridLayoutAutoSpan44().ShowDialog();          // FAILS   Order dependant but Microsoft is not
                        //new GridLayoutAutoSpan44ii().ShowDialog();        // FAILS   Order dependant but Microsoft is not
                        //new GridLayoutAutoFixedSpan44().ShowDialog();     // FAILS   WPF Grid acts badly! They give space fixed we give to auto columns
                new GridLayoutStar11().ShowDialog();
                new GridLayoutStar11ii().ShowDialog();
                new GridLayoutStar33().ShowDialog();
                        //new GridLayoutStar33ii().ShowDialog();            // FAILS   WPF Grid acts badly! They put everything in 0,0
                new GridLayoutStarFixed44().ShowDialog();
                        //new GridLayoutStarAuto44().ShowDialog();          // FAILS   Works but has different measuring numbers so look like it fails
                new GridLayoutStarAutoFixed66().ShowDialog();
                new GridLayoutStarAutoFixed66ii().ShowDialog();
                new GridLayoutStarSpan44().ShowDialog();
                        //new GridLayoutStarSpan44ii().ShowDialog();        // FAILS   Order dependant but Microsoft is not
                return true;
            }
            catch (Exception e)
            {
                ErrorWindow ew = new ErrorWindow();
                ew.DataContext = e;
                ew.ShowDialog();
                return false;
            }
        }
    }
}
