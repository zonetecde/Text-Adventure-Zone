using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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

namespace Text_Adventure_Game.Text_Adventure_Maker.Project_Manager
{
    /// <summary>
    /// Logique d'interaction pour UserControl_Creater.xaml
    /// </summary>
    public partial class UserControl_ProjectLoaderAndCreater : UserControl
    {
        public Action<Projet> OpenProject { get; }
        public GameWindow GameWindow { get; }

        public UserControl_ProjectLoaderAndCreater(Action<Projet> openProject)
        {
            InitializeComponent();
            OpenProject = openProject;

            // elle n'apparait pas au lancement
            image_GameIcon.Source = Utilities.Extensions.ImageSourceFromBitmap(new Bitmap(Properties.Resources.defaultIcon));
        }

        private void Button_Create_Click(object sender, RoutedEventArgs e)
        {
            if(!String.IsNullOrEmpty(textBox_gameName.Text) && Utilities.Extensions.DoNotContainTheses(textBox_gameName.Text, new List<char>()
            {
                '/', '\\', ':', '?', '"', '<', '>', '|'
            }))
            {
                // Un autre projet existe déjà sous ce nom
                if(UserDataManager.UserData.Projets.Any(x => x.Name.Equals(textBox_gameName.Text)))
                {
                    textBox_gameName.Text = textBox_gameName.Text + " - " + new Random().Next(100,999);
                    ProjectNameErrorEffect();
                }
                else
                {
                    // Ajout du projet
                    Projet projet = new Projet()
                    {
                        Name = textBox_gameName.Text,
                        CreationDate = DateTime.Now,
                        Description = richTextBox_description.Text,
                        Path = textBox_gameName.Text
                    };

                    Directory.CreateDirectory(UserDataManager.ProjectsPath + projet.Path);
                    
                    if(textBlock_imagePath.Text.Equals("default.png"))
                        new Bitmap(Text_Adventure_Game.Properties.Resources.defaultIcon).Save(UserDataManager.ProjectsPath + projet.Path + @"\icon.png", ImageFormat.Png);
                    else
                        Utilities.Extensions.BitmapImage2Bitmap(image_GameIcon.Source as BitmapImage).Save(UserDataManager.ProjectsPath + projet.Path + @"\icon.png", ImageFormat.Png);

                    UserDataManager.UserData.Projets.Insert(0, projet);

                    this.Visibility = Visibility.Collapsed;
                    OpenProject(projet);
                }
            }
            else
            {
                ProjectNameErrorEffect();
            }
        }

        private void ProjectNameErrorEffect()
        {
            // Le nom du jeu est demandé.
            textBox_gameName.BorderBrush = System.Windows.Media.Brushes.Red;
            textBox_gameName.BorderThickness = new Thickness(2, 2, 2, 2);

            // Enlève l'effet rouge de la textBox lorsqu'on commence à écrire dedans
            textBox_gameName.TextChanged += (sender, e) =>
            {
                textBox_gameName.BorderBrush = richTextBox_description.BorderBrush;
            };
        }

        private void Image_GameIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                // Ouvre l'explorateur de fichier pour sélectionner l'image du jeu.
                Microsoft.Win32.OpenFileDialog op = new Microsoft.Win32.OpenFileDialog();               
                op.Title = "Sélectionnez une image";
                op.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;";
                if (op.ShowDialog() == true)
                {
                    image_GameIcon.Source = new BitmapImage(new Uri(op.FileName));
                    textBlock_imagePath.Text = op.FileName;
                }
            }
        }

        private void Button_SupprimerGameIcon_Click(object sender, RoutedEventArgs e)
        {
            textBlock_imagePath.Text = "default.png";
            image_GameIcon.Source = Utilities.Extensions.ImageSourceFromBitmap( new Bitmap(Properties.Resources.defaultIcon));
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Affiche les projets
            foreach(Projet projet in UserDataManager.UserData.Projets)
            {
                StackPanel_projets.Children.Add(new UserControl_Projet(projet, OpenProject, this));
            }
        }
    }
}
