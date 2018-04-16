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
using System.Text.RegularExpressions;

namespace ShipBattlesApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public enum GameMode { Easy, Medium, Hard }
        GameMode gameMode = new GameMode();
        ShipBattlesModel.HighScore hs = new ShipBattlesModel.HighScore();
        Button oldBtn;
        Button btn;

        private void FrameworkElement_Loaded(object sender, RoutedEventArgs e)
        {

        }

        // This event executes upon loading the MainWindow
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // https://docs.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement.minwidth?view=netframework-4.7.1#System_Windows_FrameworkElement_MinWidth
            this.MinWidth = 1440.0;
            this.MinHeight = 900.0;
            btnEasy.Background = new SolidColorBrush(Colors.Green);
            gameMode = GameMode.Easy;
            hs.CheckHighScoresFile();
            hs.LoadHighScores(false);
            oldBtn = btnEasy;
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnDiff_Click(object sender, RoutedEventArgs e)
        {
            oldBtn.Background = new SolidColorBrush(Colors.LightGray);
            btn = (Button)sender;
            btn.Background = new SolidColorBrush(Colors.Green);
            oldBtn = btn;
            gameMode = GameMode.Easy;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            string tempName = nameBox.Text;
            string name = "";
            nameBox.Text = "";
            List<int> indexList = new List<int> { };
            if (tempName == "")
            {
                TextBox noName = new TextBox()
                {
                    Margin = new Thickness(40, 0, 40, 0),
                    Text = "Please enter a name.",
                    TextAlignment = TextAlignment.Center,
                    FontSize = 70,
                    FontWeight = FontWeights.ExtraBold,
                    Foreground = Brushes.Green,
                };
                noName.IsEnabled = false;

                Window noNameWind = new Window()
                {
                    Title = "Please enter a name.",
                    Height = 150,
                    Width = 1000,
                    Visibility = Visibility.Visible,
                    Name = "noNameWind",
                    Content = noName
                };
                return;
            }

            foreach (char c in tempName)
            {
                if (c == ' ') name += '_';
                else name += c;
            }


            GamePlayWindow gpwindow = new GamePlayWindow(hs, name);
            gpwindow.Show();
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.Show();
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            HelpWindow helpWindow = new HelpWindow();
            helpWindow.Show();
        }

        private void btnHighScore_Click(object sender, RoutedEventArgs e)
        {
            // from https://stackoverflow.com/questions/11133947/how-to-open-second-window-from-first-window-in-wpf
            // and https://stackoverflow.com/questions/30023419/how-to-call-a-variable-from-one-window-to-another-window
            HighScoreWindow hswindow = new HighScoreWindow(hs);
            hswindow.Show();
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            GamePlayWindow gpwindow = new GamePlayWindow(hs);
            gpwindow.Show();
        }

        private void nameBox_KeyDown(object sender, KeyEventArgs e)
        {
            List<string> list = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P",
            "Q", "R", "S", "T" ,"U", "V", "W", "X", "Y", "Z"};

            foreach (string s in list)
            {
                if (e.Key.ToString() == s)
                {
                    e.Handled = false;
                    return;
                }
            }
            e.Handled = true;
        }
    }
}
