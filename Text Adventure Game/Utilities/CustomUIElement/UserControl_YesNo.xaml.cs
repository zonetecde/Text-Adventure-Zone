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

namespace Text_Adventure_Game.Utilities.CustomUIElement
{
    /// <summary>
    /// Logique d'interaction pour UserControl_YesNo.xaml
    /// </summary>
    public partial class UserControl_YesNo : UserControl
    {
        public UserControl_YesNo(string question, Action yes, Action no, UIElement toShow)
        {
            InitializeComponent();

            txtBlock_question.Text = question;
            Yes = yes;
            No = no;
            ToShow = toShow;
        }

        public Action Yes { get; }
        public Action No { get; }
        public UIElement ToShow { get; }

        private void Button_No_Click(object sender, RoutedEventArgs e)
        {
            No();

            GameWindow._GameWindow.Grid_Message.Children.Remove(this);
        }

        private void Button_Yes_Click(object sender, RoutedEventArgs e)
        {
            Yes();

            GameWindow._GameWindow.Grid_Message.Children.Remove(this);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ToShow.Visibility = Visibility.Visible;
        }
    }
}
