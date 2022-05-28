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
using Text_Adventure_Game.Text_Adventure_Maker.Blueprint.Fonction;

namespace Text_Adventure_Game.Text_Adventure_Maker.Blueprint
{
    /// <summary>
    /// Logique d'interaction pour UserControl_FonctionSearcher.xaml
    /// </summary>
    public partial class UserControl_FonctionSearcher : UserControl
    {
        public UserControl_FonctionSearcher()
        {
            InitializeComponent();
        }

        private void listViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListViewItem item = sender as ListViewItem;

            // ligne de connexion definitive
            Line connexion = new Line();
            connexion.X1 = GameWindow._GameWindow.BlueprintLine.X1;
            connexion.Y1 = GameWindow._GameWindow.BlueprintLine.Y1;

            ConnecteurTag connecteurTag = new ConnecteurTag();
            connecteurTag.ConnecteurId = GameWindow._GameWindow.ProjectProperties.TotalIdIncrementation + 1;
            connexion.Tag = connecteurTag;
            GameWindow._GameWindow.ProjectProperties.TotalIdIncrementation += 2;

            // le tag là c'est l'id du premier uc;
            // il faut donc ajouter au premier uc comme quoi il y a connexion
            // minWidth = connexion depart
            // opacity = connexion fin
            foreach(UIElement ui in GameWindow._GameWindow.Canvas_Blueprint.Children)
            {
                if(ui is UserControl)
                {
                    if ((ui as UserControl).Tag != null)
                        if ((ui as UserControl).Tag.Equals(GameWindow._GameWindow.BlueprintLine.Tag))                        
                            (ui as UserControl).Tag MinWidth = (Convert.ToInt32(connexion.Tag));
                        
                }

            }

            connexion.Stroke = new SolidColorBrush(Colors.Yellow);
            connexion.StrokeThickness = 6.0;
            GameWindow._GameWindow.Canvas_Blueprint.Children.Add(connexion);

            switch (item.Content)
            {
                case "Texte avec choix":

                    // Ajoute la fonction texte avec choix au blueprint
                    UserControl_TexteAvecChoix uc = new UserControl_TexteAvecChoix(connexion);

                    FonctionTag tag = new FonctionTag()
                    {
                       FonctionId = 1,
                       IdOnBlueprint = GameWindow._GameWindow.ProjectProperties.TotalIdIncrementation + 1,
                       IdConnecteurEntrée = - 1,
                       IdConnecteurSortie = - 1
                    };
                    uc.Tag = tag; // id on blueprint du uc
                    GameWindow._GameWindow.ProjectProperties.TotalIdIncrementation += 2; // incrémentente l'id total
                    GameWindow._GameWindow.FonctionElementsOpenedProject.Add(new Classes.FonctionElement
                    {
                        Id = 1,
                        IdOnBlueprint = Convert.ToInt32(tag.IdOnBlueprint),
                        Left = Canvas.GetLeft(this),
                        Top = Canvas.GetTop(this)
                        
                    });

                    tag.IdConnecteurEntrée (connexion.Tag);

                    Canvas.SetLeft(uc, Canvas.GetLeft(this));
                    Canvas.SetTop(uc, Canvas.GetTop(this));

                    GameWindow._GameWindow.Canvas_Blueprint.Children.Add(uc);

                    break;
            }

            this.Visibility = Visibility.Hidden;

        }
    }
}
