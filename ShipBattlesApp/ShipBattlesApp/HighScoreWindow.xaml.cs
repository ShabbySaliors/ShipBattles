//-----------------------------------------------
//File:   HighScoreWindow.xaml.cs
//Desc:   This file contains code for displaying
//        the current high-scores in ShipBattles.
//-----------------------------------------------
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ShipBattlesModel;

namespace ShipBattlesApp
{
    // Code for the High-Scores screen
    public partial class HighScoreWindow : Window
    {
        // Stores high-score data to be displayed in this window.
        HighScore hs;

        // Receives an instance of HighScore and stores it to prep for loading the current high-scores.
        // from https://stackoverflow.com/questions/30023419/how-to-call-a-variable-from-one-window-to-another-window
        public HighScoreWindow(HighScore hsTemp)
        {
            hs = hsTemp;
            InitializeComponent();
        }

        // Displays the current high-scores.
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
                foreach (Score s in hs.ScoresList)
                {
                    string realName = "";
                    foreach (char c in hs.ScoresList[count].Name)
                    {
                        if (c == '_') realName += ' ';
                        else realName += c;
                    }
                    nameList[count].Text = realName;
                    scoreList[count].Text = s.Points.ToString();
                    count++;
                }
            }
        }
    }
}
