using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Text_Adventure_Game.Utilities.Blueprint_extensions
{
    internal static class ConnexionExtension
    {
        internal static void SetTheConnexion(Line Connexion, System.Windows.Controls.Image arrowEntree)
        {
            if (Connexion != null)
            {
                try
                {
                    Connexion.X2 = arrowEntree.TransformToAncestor(GameWindow._GameWindow.Canvas_Blueprint).Transform(new Point(0, 0)).X + 31;
                    Connexion.Y2 = arrowEntree.TransformToAncestor(GameWindow._GameWindow.Canvas_Blueprint).Transform(new Point(0, 0)).Y + 33;
                    Panel.SetZIndex(Connexion, 9999);

                    // ajoute la connexion
                    GameWindow._GameWindow.ConnexionElementsOpenedProject.Add(new Classes.Connexion()
                    {
                        Id = GameWindow._GameWindow.ProjectProperties.TotalIdIncrementation + 1,
                        X1 = Connexion.X1,
                        X2 = Connexion.X2,
                        Y1 = Connexion.Y1,
                        Y2 = Connexion.Y1,
                        IdDepart = (Connexion.Tag as List<int>)[0],
                        IdArrivee = (Connexion.Tag as List<int>)[1]
                    });

                    GameWindow._GameWindow.ProjectProperties.TotalIdIncrementation += 2;
                }
                catch
                {
                }
            }
        }
    }
}
