using ClassesZone;
using Utilities;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Text_adventure_maker.ProjectManager
{
    /// <summary>
    /// Logique d'interaction pour UserControl_Projet.xaml
    /// UserControl qui apparait pour sélectionner un projet parmis ceux déjà créé.
    /// </summary>
    public partial class UserControl_Projet : UserControl
    {


        public UserControl_Projet(Projet projet, Action<Projet> openProject)
        {
            InitializeComponent();
            Projet = projet;
            OpenProject = openProject;

            img_icon.Source = Utilities.Extensions.BitmapFromUri(new Uri(UserDataManager.ProjectsPath + projet.Path + @"\icon.png"));

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
                image_delete.Visibility = Visibility.Visible; // img en haut à droite de UC pour supprimer le projet 
                this.Background = (Brush)Utilities.Extensions.ColorConverter.ConvertFromString("#FF2B2626");
            };
            this.MouseLeave += (sender, e) =>
            {
                image_delete.Visibility = Visibility.Hidden; // img en haut à droite de UC pour supprimer le projet 
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

        private void image_delete_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                UserControl_GameMaker._UserControl_GameMaker.Grid_message.Children.Add(new Utilities.UserControl_YesNo("Êtes-vous sûre de vouloir supprimer définitivement \"" + Projet.Name + "\" ?", () =>
                {
                    // delete le projet
                    img_icon.Source = null;
                    Directory.Delete(UserDataManager.ProjectsPath + Projet.Path, true); 
                    UserDataManager.UserData.Projets.Remove(Projet);
                    this.Visibility = Visibility.Collapsed;

                }, () =>
                {

                }));
            }
        }
    }
}
