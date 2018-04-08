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
using System.Windows.Shapes;
using ShipBattlesModel;

namespace ShipBattlesApp
{
    /// <summary>
    /// Interaction logic for GamePlayWindow.xaml
    /// </summary>
    public partial class GamePlayWindow : Window
    {
        Controller ctrl = new Controller();
        public GamePlayWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ctrl.LoadWorld();
            GameWorld.Instance.MakePlottibles();
            foreach (GameObject obj in GameWorld.Instance.Plottibles)
                //Plot(obj)
        }
    }
}
