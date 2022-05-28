using Newtonsoft.Json;
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
using Brushes = System.Windows.Media.Brushes;

namespace Text_Adventure_Game.Text_Adventure_Maker.Project_Manager
{
    /// <summary>
    /// Logique d'interaction pour UserControl_Creater.xaml
    /// </summary>
    public partial class UserControl_ProjectLoaderAndCreater : UserControl
    {
        public Action<RecentlyOpenedProject> OpenProject { get; }
        public GameWindow GameWindow { get; }

        public UserControl_ProjectLoaderAndCreater(Action<RecentlyOpenedProject> openProject)
        {
            InitializeComponent();
            OpenProject = openProject;



            // Affiche l'image du projet par défaut pour en créer un.
            image_GameIcon.Source = Utilities.Extensions.ImageSourceFromBitmap(new Bitmap(Properties.Resources.defaultIcon));

            // Gère l'effet rouge autour de la textBox du nom du projet si le nom est incorrect ou pas
            textBox_gameName.TextChanged += (sender, e) =>
            {
                // Si le string n'est pas empty ni ne contient des caractères que Windows interdit pour ses fichiers
                if (!String.IsNullOrEmpty(textBox_gameName.Text) && Utilities.Extensions.DoNotContainTheses(textBox_gameName.Text, new List<char>()
                {
                    '/', '\\', ':', '?', '"', '<', '>', '|'
                }) && !UserDataManager.UserData.RecentlyOpenedProjects.Any(x => x.ProjectName.Equals(textBox_gameName.Text.Trim())))
                {
                    // Le nom du jeu est correct, enlève de l'effet
                    textBox_gameName.Background = richTextBox_description.Background;
                    Button_Create.IsEnabled = true;
                }
                else
                {
                    // Le nom du jeu est incorrect, donc effet sur la TextBox pour le faire comprendre
                    textBox_gameName.Background = System.Windows.Media.Brushes.LightCoral;
                    Button_Create.IsEnabled = false;
                }

            };
        }

        private void Button_Create_Click(object sender, RoutedEventArgs e)
        {
            // Créer un projet.
            // Pour rappel ce bouton n'est accessible uniquement si le nom du projet et unique et ne contient aucun caractère interdit, voir textBox_gameName.TextChanged dans le constructeur

            // Ajout du projet
            ProjectProperties projet = new ProjectProperties()
            {
                Name = textBox_gameName.Text.Trim(),
                CreationDate = DateTime.Now,
                Description = richTextBox_description.Text,
                Path = textBox_gameName.Text.Trim(),
                TotalIdIncrementation = 1
            };

            // Créer le dossier du projet
            Directory.CreateDirectory(UserDataManager.ProjectsPath + projet.Path);

            // Créer le fichier blueprint du projet
            // Ajoute une fonction au blueprint : celle du start
            File.WriteAllText(UserDataManager.ProjectsPath + projet.Path + @"\blueprint.taz", JsonConvert.SerializeObject(new List<FonctionElement>() { new FonctionElement() { Id = 0, Left = 100, Top = 100 } }));
            File.WriteAllText(UserDataManager.ProjectsPath + projet.Path + @"\connexion.taz", JsonConvert.SerializeObject(new List<Connexion>() { }));
            File.WriteAllText(UserDataManager.ProjectsPath + projet.Path + @"\projectprop.taz", JsonConvert.SerializeObject(projet));

            // Si l'image du projet et celle par défaut on la prend des Properties.Resources, Sinon prend l'image importé
            if (textBlock_imagePath.Text.Equals("default.png"))
                new Bitmap(Text_Adventure_Game.Properties.Resources.defaultIcon).Save(UserDataManager.ProjectsPath + projet.Path + @"\icon.png", ImageFormat.Png);
            else
                        
                Utilities.Extensions.BitmapImage2Bitmap(image_GameIcon.Source as BitmapImage).Save(UserDataManager.ProjectsPath + projet.Path + @"\icon.png", ImageFormat.Png);

            // Ajoute le projet en haut de la liste des projets déjà existant
            UserDataManager.UserData.RecentlyOpenedProjects.Insert(0, new RecentlyOpenedProject()
            {
                ProjectDesc = projet.Description,
                ProjectName = projet.Name,
                ProjectPath = projet.Path
            });

            // Ferme le sélecteur de projet puis ouvre le projet
            this.Visibility = Visibility.Hidden;
            OpenProject(UserDataManager.UserData.RecentlyOpenedProjects[0]);
                

        }

        private void Image_GameIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // L'utilisateur veut changer l'icone de son jeu

            if(e.LeftButton == MouseButtonState.Pressed)
            {
                // Ouvre l'explorateur de fichier pour sélectionner une image
                Microsoft.Win32.OpenFileDialog op = new Microsoft.Win32.OpenFileDialog();               
                op.Title = "Sélectionnez une image";
                op.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;";
                if (op.ShowDialog() == true)
                {
                    // L'image a été choisi, on la change donc dans l'Image (UIElement) et on met son chemin d'accès dans le label
                    image_GameIcon.Source = new BitmapImage(new Uri(op.FileName));
                    textBlock_imagePath.Text = op.FileName;
                }
            }
        }

        private void Button_SupprimerGameIcon_Click(object sender, RoutedEventArgs e)
        {
            // L'utilisateur veut supprimer l'image qu'il a mise, donc on remet la default.png
            textBlock_imagePath.Text = "default.png";
            image_GameIcon.Source = Utilities.Extensions.ImageSourceFromBitmap( new Bitmap(Properties.Resources.defaultIcon));
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Affiche les projets
            foreach(RecentlyOpenedProject projet in UserDataManager.UserData.RecentlyOpenedProjects)
            {
                StackPanel_projets.Children.Add(new UserControl_Projet(projet, OpenProject, this));
            }
        }
    }
}
