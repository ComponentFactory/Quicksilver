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
    public class MetaPanelLayoutWindow : Window
    {
        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);

            // Get access to the panel and the morph panel
            Panel original = (Panel)this.FindName("original");
            MetaPanel morph = (MetaPanel)this.FindName("morph");

            // Must still have the same number of children
            if (original.Children.Count != morph.Children.Count)
                throw new ApplicationException("Number of children differs on " + Title);

            for (int i = 0; i < original.Children.Count; i++)
            {
                UIElement originalChild = original.Children[i];
                UIElement morphChild = morph.Children[i];

                if (!IsClose(originalChild.RenderSize, morphChild.RenderSize))
                    throw new ApplicationException("Child " + i.ToString() + " Orig RenderSize " + originalChild.RenderSize.ToString() + " != Morph RenderSize " + morphChild.RenderSize + " on " + Title);

                if (!IsClose(originalChild.DesiredSize, morphChild.DesiredSize))
                    throw new ApplicationException("Child " + i.ToString() + " Orig DesiredSize " + originalChild.DesiredSize.ToString() + " != Morph DesiredSize " + morphChild.DesiredSize + " on " + Title);

                Point relativeOriginal = originalChild.TranslatePoint(new Point(0, 0), original);
                Point relativeMorph = morphChild.TranslatePoint(new Point(0, 0), morph);
                if (!IsClose(relativeOriginal, relativeMorph))
                    throw new ApplicationException("Child " + i.ToString() + " Orig TopLeft " + relativeOriginal.ToString() + " != Morph TopLeft " + relativeMorph + " on " + Title);
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
