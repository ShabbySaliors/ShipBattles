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
        public HighScoreWindow(ShipBattlesModel.HighScore hsTemp)
        {
            hs = hsTemp;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (hs.ScoresList[0] == "")
            {
                No_Scores.Text = "There are currently no high-scores.\n Play a level to get the first high-score!";
            }
            else
            {
                Block_0.Text = hs.ScoresList[0];
                Block_1.Text = hs.ScoresList[1];
                Block_2.Text = hs.ScoresList[2];
                Block_3.Text = hs.ScoresList[3];
                Block_4.Text = hs.ScoresList[4];
                Block_5.Text = hs.ScoresList[5];
                Block_6.Text = hs.ScoresList[6];
                Block_7.Text = hs.ScoresList[7];
                Block_8.Text = hs.ScoresList[8];
                Block_9.Text = hs.ScoresList[9];
            }
        }
    }
}
