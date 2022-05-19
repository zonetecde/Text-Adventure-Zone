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
using Text_Adventure_Game.Classes;
using Text_Adventure_Game.Utilities.CustomUIElement;

namespace Text_Adventure_Game.Text_Adventure_Maker.Project_Manager
{
    /// <summary>
    /// Logique d'interaction pour UserControl_Projet.xaml
    /// UserControl qui apparait pour sélectionner un projet parmis ceux déjà créé.
    /// </summary>
    public partial class UserControl_Projet : UserControl
    {
        public UserControl_Projet(Projet projet, Action<Projet> openProject, UserControl_ProjectLoaderAndCreater userControl_ProjectLoaderAndCreater)
        {
            InitializeComponent();

            Projet = projet;
            OpenProject = openProject;
            UserControl_ProjectLoaderAndCreater = userControl_ProjectLoaderAndCreater;

            // L'icone du projet
            img_icon.Source = Utilities.Extensions.BitmapFromUri(new Uri(UserDataManager.ProjectsPath + projet.Path + @"\icon.png"));
            // Nom du projet
            textBlock_nom.Text = projet.Name;

            // Si il y a une description, on l'affiche en tant que toolTip
            if(!String.IsNullOrEmpty(projet.Description))
                textBlock_nom.ToolTip = projet.Description; 
        }

        public Projet Projet { get; }
        public Action<Projet> OpenProject { get; }
        public UserControl_ProjectLoaderAndCreater UserControl_ProjectLoaderAndCreater { get; } // parent
        public GameWindow GameWindow { get; }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.MouseEnter += (sender, e) =>
            {
                // effet de mouseOver
                image_delete.Visibility = Visibility.Visible; // affiche img pour supprimer le projet 
                this.Background = (Brush)Utilities.Extensions.ColorConverter.ConvertFromString("#FF2B2626");
            };
            this.MouseLeave += (sender, e) =>
            {
                // effet de mouseOver
                image_delete.Visibility = Visibility.Hidden; // cache img pour supprimer le projet 
                this.Background = Brushes.Transparent;
            };
            this.MouseDown += (sender, e) =>
            {
                if(e.LeftButton == MouseButtonState.Pressed)
                {
                    // Le projet est sélectionné
                    // On le met en haut de la liste des projets (ouvert le plus récemment donc)
                    var projectToPlaceOnTop = UserDataManager.UserData.Projets[UserDataManager.UserData.Projets.IndexOf(Projet)];
                    UserDataManager.UserData.Projets.RemoveAt(UserDataManager.UserData.Projets.IndexOf(Projet));
                    UserDataManager.UserData.Projets.Insert(0, projectToPlaceOnTop);

                    // Cache le sélecteur de projet et lance le projet
                    UserControl_ProjectLoaderAndCreater.Visibility = Visibility.Hidden;
                    OpenProject(Projet);
                }
            };
        }

        private void image_delete_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                // Le projet veut être supprimé, on demande quand même pour être sûre
                GameWindow._GameWindow.Grid_Message.Children.Add(new UserControl_YesNo("Êtes-vous sûre de vouloir supprimer définitivement \"" + Projet.Name + "\" ?", () =>
                {
                    // L'utilisateur confirme, supprimation du projet
                    Directory.Delete(UserDataManager.ProjectsPath + Projet.Path, true); // supprime le dossier du projet
                    UserDataManager.UserData.Projets.Remove(Projet); // enlève le projet de la liste des projets
                    this.Visibility = Visibility.Collapsed; // cache le projet de la liste des projets
                }, () =>
                {
                }, UserControl_ProjectLoaderAndCreater));
            }
        }
    }
}
