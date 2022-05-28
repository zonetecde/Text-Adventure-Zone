using Newtonsoft.Json;
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
using Text_Adventure_Game.Text_Adventure_Maker.Blueprint;
using Text_Adventure_Game.Text_Adventure_Maker.Blueprint.Fonction;
using Text_Adventure_Game.Text_Adventure_Maker.Project_Manager;

namespace Text_Adventure_Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        public static GameWindow _GameWindow { get; private set; }

        public RecentlyOpenedProject OpenedProject { get; set; }
        public List<FonctionElement> FonctionElementsOpenedProject { get; set; }
        public ProjectProperties ProjectProperties { get; set; }
        public List<Connexion> ConnexionElementsOpenedProject { get; set; }

        // Blueprint
        public bool IsMoving { get; internal set; }
        Utilities.Blueprint_extensions.MovingAround movingAroundExtension = new Utilities.Blueprint_extensions.MovingAround();
        UserControl_FonctionSearcher userControl_FonctionSearcher = new UserControl_FonctionSearcher();
        public Line BlueprintLine = new Line();

        public GameWindow()
        {
            InitializeComponent();

            _GameWindow = this;
        }

        private void Image_Settings_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Affiche la page des paramètres
            if (e.LeftButton == MouseButtonState.Pressed)
                ShowSettings();
        }

        private void ShowSettings()
        {
            // Set les settings actuelle en fonction de la fenêtre
            Grid_Settings.Visibility = Visibility.Visible;
            checkbox_fullscreen.img_checked.Visibility = this.WindowStyle == WindowStyle.None ? Visibility.Visible : Visibility.Hidden;
            checkbox_fullscreen.img_unchecked.Visibility = this.WindowStyle == WindowStyle.None ? Visibility.Hidden : Visibility.Visible;

            // Settings : Fullscreen / Windowed
            checkbox_fullscreen.ActionWhenCheckedOrUnchecked = () => {
                this.WindowStyle = this.WindowStyle == WindowStyle.None ? WindowStyle.SingleBorderWindow : WindowStyle.None;
            };
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            // Init les données de l'utilisateur
            UserDataManager.InitDataSaverAndLoader();
        }

        private void Button_Créer_Click(object sender, RoutedEventArgs e)
        {
            ShowGameMaker();
        }

        private void ShowGameMaker()
        {
            // Affiche le sélecteur de projet 
            Grid_GameMaker.Children.Add(new UserControl_ProjectLoaderAndCreater(OpenProject));
            Grid_GameMaker.Visibility = Visibility.Visible;
        }

        private void OpenProject(RecentlyOpenedProject projet)
        {
            OpenedProject = projet;

            // Setup l'environnement 
            SetupBlueprint();

            // Ouvre le projet
            FonctionElementsOpenedProject = JsonConvert.DeserializeObject<List<FonctionElement>>(File.ReadAllText(UserDataManager.ProjectsPath + projet.ProjectPath + @"\blueprint.taz"));
            ConnexionElementsOpenedProject = JsonConvert.DeserializeObject<List<Connexion>>(File.ReadAllText(UserDataManager.ProjectsPath + projet.ProjectPath + @"\connexion.taz"));
            ProjectProperties = JsonConvert.DeserializeObject<ProjectProperties>(File.ReadAllText(UserDataManager.ProjectsPath + projet.ProjectPath + @"\projectprop.taz"));
            foreach (FonctionElement fonctionElement in FonctionElementsOpenedProject)
            {
                UIElement uIElement = new UIElement();

                // Ajoute la fonction au blueprint
                switch(fonctionElement.Id)
                {
                    case 0:
                        // Fonction : WhenGameStart
                        uIElement = new UserControl_WhenGameStart();
                        (uIElement as UserControl_WhenGameStart).Tag = fonctionElement.IdOnBlueprint;

                        try
                        {
                            (uIElement as UserControl_WhenGameStart).MinWidth = ConnexionElementsOpenedProject.FirstOrDefault(x => fonctionElement.IdOnBlueprint.Equals(x.IdDepart)).Id;
                        }
                        catch { }
                            try
                            {

                                (uIElement as UserControl_WhenGameStart).Opacity = ConnexionElementsOpenedProject.FirstOrDefault(x => fonctionElement.IdOnBlueprint.Equals(x.IdArrivee)).Id;
                            }
                        catch { }
                        break;
                    case 1:
                        // Fonction : TexteAvecChoix
                        uIElement = new UserControl_TexteAvecChoix();
                        (uIElement as UserControl_TexteAvecChoix).Tag = fonctionElement.IdOnBlueprint;

                        try
                        {
                            (uIElement as UserControl_TexteAvecChoix).MinWidth = ConnexionElementsOpenedProject.FirstOrDefault(x => fonctionElement.IdOnBlueprint.Equals(x.IdDepart)).Id;
                        }

                        catch { }
                        try
                        {
                            (uIElement as UserControl_TexteAvecChoix).Opacity = ConnexionElementsOpenedProject.FirstOrDefault(x => fonctionElement.IdOnBlueprint.Equals(x.IdArrivee)).Id;

                        }
                        catch { }
                        break;
                }

                

                

                Canvas_Blueprint.Children.Add(uIElement);

                Canvas.SetTop(uIElement, fonctionElement.Top);
                Canvas.SetLeft(uIElement, fonctionElement.Left);
            }

            // Ajout les lignes de connexions
            foreach(Connexion connexion in ConnexionElementsOpenedProject)
            {
                Line l = new Line();
                Canvas_Blueprint.Children.Add(l);
                l.Stroke = new SolidColorBrush(Colors.Yellow);
                l.StrokeThickness = 6.0;
                l.X1 = connexion.X1;
                l.X2 = connexion.X2;
                l.Y1 = connexion.Y1;
                l.Y2 = connexion.Y2;
                l.Tag = new List<int>()
                {
                    connexion.IdDepart,
                    connexion.IdArrivee
                };


                // Ajoute les refs dans les fonctions que c'est leur connexion
            }
        }

        private void SetupBlueprint()
        {
            movingAroundExtension.MovingAroundCanvasInit(Canvas_Blueprint, Grid_BlueprintParent);
        }

        private void CloseSettingsButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                Grid_Settings.Visibility = Visibility.Hidden;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            UserDataManager.SaveData();
        }

        private void MenuItem_MeRecentrer_Click(object sender, RoutedEventArgs e)
        {
            // Lorsqu'on est sur le blueprint d'un jeu et qu'on se perd (= on voit que du noir) permet de recentrer
            (this.Canvas_Blueprint.RenderTransform as TransformGroup).Children[1] = new TranslateTransform(0, 0);
            (this.Canvas_Blueprint.RenderTransform as TransformGroup).Children[0] = new MatrixTransform();
            movingAroundExtension._currentTT = new TranslateTransform(0, 0);
        }

        private void Canvas_Blueprint_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // Lorsque on fait une connexion entre un uc, autre click = annule la connexion
            // ou affiche les fonctions

            try
            {
                if (IsMoving)
                {
                    // on affiche les fonctions disponible à ajouté
                    IsMoving = false;
                    userControl_FonctionSearcher.Visibility = Visibility.Visible; // set la visibility du uc avec l'ajouteur de fonction
                    Canvas.SetLeft(userControl_FonctionSearcher, Mouse.GetPosition(Canvas_Blueprint).X); // le place x y là où il y a la souris
                    Canvas.SetTop(userControl_FonctionSearcher, Mouse.GetPosition(Canvas_Blueprint).Y);
                    if(!Canvas_Blueprint.Children.Contains(userControl_FonctionSearcher)) // l'ajoute si il n'y est pas encore
                        Canvas_Blueprint.Children.Add(userControl_FonctionSearcher);

                }
                else
                {
                    // si on appuie autre part sur le canvas on enlève la ligne et le uc fonction searcher (car on annule la connexion)
                    // sauf si on appuie sur le fonction searcher pour sélectionner une fonction
                    if (e.Source != userControl_FonctionSearcher) 
                    {
                        IsMoving = false;
                        userControl_FonctionSearcher.Visibility = Visibility.Hidden;
                        BlueprintLine.Visibility = Visibility.Hidden;
                    }
                }
            }
            catch { }
        }

        private void Canvas_Blueprint_MouseMove(object sender, MouseEventArgs e)
        {
            // Bouge la ligne de connexion là où le curseur est
            if(e.LeftButton == MouseButtonState.Pressed && IsMoving)
            {
                try
                {
                    BlueprintLine.X2 = Mouse.GetPosition(Canvas_Blueprint).X;
                    BlueprintLine.Y2 = Mouse.GetPosition(Canvas_Blueprint).Y;
                }
                catch { }

            }
        }
    }
}
