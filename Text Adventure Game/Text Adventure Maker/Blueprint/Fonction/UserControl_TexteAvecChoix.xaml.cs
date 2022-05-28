using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Text_Adventure_Game.Classes;
using Text_Adventure_Game.Utilities.Blueprint_extensions;

namespace Text_Adventure_Game.Text_Adventure_Maker.Blueprint.Fonction
{
    /// <summary>
    /// Logique d'interaction pour UserControl_TexteAvecChoix.xaml
    /// </summary>
    public partial class UserControl_TexteAvecChoix : UserControl
    {
        public Line Connexion { get; }

        public UserControl_TexteAvecChoix(Line connexion = null)
        {
            Connexion = connexion;

            InitializeComponent();
        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {
            MovingAround movingAround = new MovingAround();
            movingAround.MovingAroundFonctionInit(this,Image_ArrowEntrée , Image_Arrow);



        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Ligne pour connecter 2 élements
            DrawConnection.DrawNewLine(sender, (this.Tag as FonctionTag).IdOnBlueprint);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Connexion jusqu'à la flèche
            Utilities.Blueprint_extensions.ConnexionExtension.SetTheConnexion(Connexion, Image_ArrowEntrée);
        }


    }
}
