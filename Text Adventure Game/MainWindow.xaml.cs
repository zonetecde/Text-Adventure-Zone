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

        // Blueprint
        Utilities.Blueprint_extensions.MovingAround movingAroundExtension = new Utilities.Blueprint_extensions.MovingAround();

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

        private void OpenProject(Projet projet)
        {
            // Setup l'environnement 
            SetupBlueprint();

            // Ouvre le projet
            foreach (FonctionElement fonctionElement in JsonConvert.DeserializeObject<List<FonctionElement>>( File.ReadAllText(UserDataManager.ProjectsPath + projet.Path + @"\blueprint.taz")))
            {
                UIElement uIElement = new UIElement();

                // Ajoute la fonction au blueprint
                switch(fonctionElement.Id)
                {
                    case 0:
                        // Fonction : WhenGameStart
                        uIElement = new UserControl_WhenGameStart();
                        break;
                }

                Canvas_Blueprint.Children.Add(uIElement);

                Canvas.SetTop(uIElement, fonctionElement.Top);
                Canvas.SetLeft(uIElement, fonctionElement.Left);
            }
        }

        private void SetupBlueprint()
        {
            movingAroundExtension.MovingAroundInit(Canvas_Blueprint, Grid_BlueprintParent);
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
    }
}
