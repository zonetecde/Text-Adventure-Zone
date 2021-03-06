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
    /// Logique d'interaction pour UserControl_WhenGameStart.xaml
    /// </summary>
    public partial class UserControl_WhenGameStart : UserControl
    {
        public UserControl_WhenGameStart()
        {
            InitializeComponent();
        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {
            MovingAround movingAround = new MovingAround();
            movingAround.MovingAroundFonctionInit(this, null, img_arrow);
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Ligne pour connecter 2 élements
            DrawConnection.DrawNewLine(sender, (this.Tag as FonctionTag).IdOnBlueprint);
        }
    }
}
