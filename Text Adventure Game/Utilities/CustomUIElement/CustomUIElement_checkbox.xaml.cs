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
    /// Logique d'interaction pour CustomUIElement_checkbox.xaml
    /// </summary>
    public partial class CustomUIElement_checkbox : UserControl
    {
        public CustomUIElement_checkbox()
        {
            InitializeComponent();
        }

        public Action ActionWhenCheckedOrUnchecked { get; set; }

        private void img_checked_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                img_checked.Visibility = Visibility.Hidden;
                img_unchecked.Visibility = Visibility.Visible;
                ActionWhenCheckedOrUnchecked();
            }
        }

        private void img_unchecked_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                img_checked.Visibility = Visibility.Visible;
                img_unchecked.Visibility = Visibility.Hidden;
                ActionWhenCheckedOrUnchecked();
            }
        }
    }
}
