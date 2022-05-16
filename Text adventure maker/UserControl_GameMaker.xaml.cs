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

namespace Text_adventure_maker
{
    /// <summary>
    /// Logique d'interaction pour UserControl_GameMaker.xaml
    /// </summary>
    public partial class UserControl_GameMaker : UserControl
    {
        public UserControl_GameMaker()
        {
            InitializeComponent();

            Grid_main.Children.Add(new UserControl_ProjectLoaderAndCreater());
        }
    }
}
