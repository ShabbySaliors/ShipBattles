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
            this.MinWidth = 1440.0;
            this.MinHeight = 900.0;

            if (hs.ScoresList.Count == 0)
            {
                No_Scores.Text = "There are currently no high-scores.\n Play a level to get the first high-score!";
            }
            else
            {
                List<TextBlock> nameList = new List<TextBlock> { Name_1, Name_2, Name_3, Name_4, Name_5, };
                List<TextBlock> scoreList = new List<TextBlock> { Score_1, Score_2, Score_3, Score_4, Score_5, };
                int count = 0;
                foreach (ShipBattlesModel.Score s in hs.ScoresList)
                {
                    nameList[count].Text = s.Name;
                    scoreList[count].Text = s.Points.ToString();
                    count++;
                }
            }
        }
    }
}
