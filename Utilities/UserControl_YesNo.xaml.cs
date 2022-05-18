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

namespace Utilities
{
    /// <summary>
    /// Logique d'interaction pour UserControl_YesNo.xaml
    /// </summary>
    public partial class UserControl_YesNo : UserControl
    {
        public UserControl_YesNo(string question, Action yes, Action no)
        {
            InitializeComponent();

            txtBlock_question.Text = question;
            Yes = yes;
            No = no;
        }

        public Action Yes { get; }
        public Action No { get; }

        private void Button_No_Click(object sender, RoutedEventArgs e)
        {
            No();

            this.Visibility = Visibility.Collapsed;
        }

        private void Button_Yes_Click(object sender, RoutedEventArgs e)
        {
            Yes();

            this.Visibility = Visibility.Collapsed;
        }
    }
}
