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

namespace ShipBattlesApp
{
    /// <summary>
    /// Interaction logic for HighScoreWindow.xaml
    /// </summary>
    public partial class HighScoreWindow : Window
    {
        ShipBattlesModel.HighScore hs;

        // from https://stackoverflow.com/questions/30023419/how-to-call-a-variable-from-one-window-to-another-window
        public HighScoreWindow(ShipBattlesModel.HighScore hstemp)
        {
            hs = hstemp;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (hs.ScoresList.Count == 1)
            {
                No_Scores.Text = "There are currently no high-scores.\n Play a level to get the first high-score!";
            }
        }
    }
}
