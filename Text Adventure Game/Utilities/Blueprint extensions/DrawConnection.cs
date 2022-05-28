using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Image = System.Windows.Controls.Image;

namespace Text_Adventure_Game.Utilities.Blueprint_extensions
{
    internal class DrawConnection
    {
        internal static void DrawNewLine(object sender, int idOnBlueprint)
        {

            Line l = new Line();
            l.Stroke = new SolidColorBrush(Colors.Yellow);
            l.StrokeThickness = 6.0;
            l.X1 = (sender as Image).TransformToAncestor(GameWindow._GameWindow.Canvas_Blueprint).Transform(new Point(0, 0)).X + 51; // + 31 car image / 2 pour que ça soit centré sur arrow
            l.X2 = (sender as Image).TransformToAncestor(GameWindow._GameWindow.Canvas_Blueprint).Transform(new Point(0, 0)).X + 51; // + 31 car image / 2 pour que ça soit centré sur arrow
            l.Y1 = (sender as Image).TransformToAncestor(GameWindow._GameWindow.Canvas_Blueprint).Transform(new Point(0, 0)).Y + 33;
            l.Y2 = (sender as Image).TransformToAncestor(GameWindow._GameWindow.Canvas_Blueprint).Transform(new Point(0, 0)).Y + 33;
            l.Tag = idOnBlueprint; // tag = id du usercontrol de début

            GameWindow._GameWindow.BlueprintLine.Visibility = Visibility.Hidden;
            GameWindow._GameWindow.BlueprintLine = l;
            GameWindow._GameWindow.IsMoving = true;
            GameWindow._GameWindow.Canvas_Blueprint.Children.Add(l);
        }
    }
}
