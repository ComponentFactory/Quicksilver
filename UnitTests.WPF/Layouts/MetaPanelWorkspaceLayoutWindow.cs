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
using ComponentFactory.Quicksilver.Layout;

namespace ComponentFactory.Quicksilver.UnitTests.WPF
{
    public class MetaPanelWorkspaceLayoutWindow : Window
    {
        public class ExpResult
        {
            public string Name { get; set; }
            public Size RenderSize { get; set; }
            public Point Location { get; set; }
        }

        protected virtual ExpResult[] TestResults() 
        { 
            return null; 
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);

            MetaPanel morph = (MetaPanel)this.FindName("morph");
            foreach (ExpResult expResult in TestResults())
            {
                for (int i = 0; i < morph.Children.Count; i++)
                {
                    ButtonOutput morphChild = (ButtonOutput)morph.Children[i];
                    if ((string)morphChild.Content == expResult.Name)
                    {
                        if (!IsClose(expResult.RenderSize, morphChild.RenderSize))
                            throw new ApplicationException("Child " + expResult.Name + " Orig RenderSize " + expResult.RenderSize.ToString() + " != Morph RenderSize " + morphChild.RenderSize + " on " + Title);

                        Point relativeMorph = morphChild.TranslatePoint(new Point(0, 0), morph);
                        if (!IsClose(expResult.Location, relativeMorph))
                            throw new ApplicationException("Child " + expResult.Name + " Orig TopLeft " + expResult.ToString() + " != Morph TopLeft " + relativeMorph + " on " + Title);
                    }
                }
            }

            Close();
        }

        private bool IsClose(Size s1, Size s2)
        {
            return IsClose(s1.Width, s2.Width) && IsClose(s1.Height, s2.Height);
        }

        private bool IsClose(Point p1, Point p2)
        {
            return IsClose(p1.X, p2.X) && IsClose(p1.Y, p2.Y);
        }

        private bool IsClose(double in1, double in2)
        {
            return Math.Round(in1, 3) == Math.Round(in2, 3);
        }
    }
}
