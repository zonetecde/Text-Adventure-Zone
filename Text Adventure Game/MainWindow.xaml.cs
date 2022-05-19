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
using Text_Adventure_Game.Text_Adventure_Maker.Project_Manager;

namespace Text_Adventure_Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        public static GameWindow _GameWindow { get; private set; }

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
            // Ouvre le projet
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
    }
}
