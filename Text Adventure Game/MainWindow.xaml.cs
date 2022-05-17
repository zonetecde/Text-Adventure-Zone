﻿using ClassesZone;
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
using Text_adventure_maker;

namespace Text_Adventure_Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        public GameWindow()
        {
            InitializeComponent();
        }

        private void Image_Settings_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                ShowSettings();
        }

        private void ShowSettings()
        {
            Grid_Settings.Visibility = Visibility.Visible;
            checkbox_fullscreen.img_checked.Visibility = this.WindowStyle == WindowStyle.None ? Visibility.Visible : Visibility.Hidden;
            checkbox_fullscreen.img_unchecked.Visibility = this.WindowStyle == WindowStyle.None ? Visibility.Hidden : Visibility.Visible;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            // Init les données de l'utilisateur
            UserDataManager.InitDataSaverAndLoader();

            checkbox_fullscreen.ActionWhenCheckedOrUnchecked = () => {
                this.WindowStyle = this.WindowStyle == WindowStyle.None ? WindowStyle.SingleBorderWindow : WindowStyle.None;
            };
        }

        private void Button_Créer_Click(object sender, RoutedEventArgs e)
        {
            ShowGameMaker();
        }

        private void ShowGameMaker()
        {
            Grid_GameMaker.Children.Add(new UserControl_GameMaker());
            Grid_GameMaker.Visibility = Visibility.Visible;
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
