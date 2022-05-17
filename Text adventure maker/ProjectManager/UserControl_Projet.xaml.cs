using ClassesZone;
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

namespace Text_adventure_maker
{
    /// <summary>
    /// Logique d'interaction pour UserControl_Projet.xaml
    /// </summary>
    public partial class UserControl_Projet : UserControl
    {
        public UserControl_Projet(Projet projet, Action<Projet> openProject)
        {
            InitializeComponent();
            Projet = projet;
            OpenProject = openProject;

            img_icon.Source = new BitmapImage(new Uri(projet.Path + @"\icon.png"));
            textBlock_nom.Text = projet.Name;
            if(!String.IsNullOrEmpty(projet.Description))
                textBlock_nom.ToolTip = projet.Description; 
        }

        public Projet Projet { get; }
        public Action<Projet> OpenProject { get; }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // transparent
            // #FF3A3737
            this.MouseEnter += (sender, e) =>
            {
                this.Background = (Brush)Utilities.Extensions.ColorConverter.ConvertFromString("#FF2B2626");
            };
            this.MouseLeave += (sender, e) =>
            {
                this.Background = Brushes.Transparent;
            };
            this.MouseDown += (sender, e) =>
            {
                if(e.LeftButton == MouseButtonState.Pressed)
                {
                    var projectToPlaceOnTop = UserDataManager.UserData.Projets[UserDataManager.UserData.Projets.IndexOf(Projet)];
                    UserDataManager.UserData.Projets.RemoveAt(UserDataManager.UserData.Projets.IndexOf(Projet));
                    UserDataManager.UserData.Projets.Insert(0, projectToPlaceOnTop);

                    OpenProject(Projet);
                }
            };
        }
    }
}
