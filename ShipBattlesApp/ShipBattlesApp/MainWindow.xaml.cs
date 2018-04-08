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

namespace ShipBattlesApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public enum GameMode { Easy, Medium, Hard }
        GameMode gameMode = new GameMode();
        ShipBattlesModel.HighScore hsTemp = new ShipBattlesModel.HighScore();
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
            hsTemp.CheckHighScoresFile();
            hsTemp.LoadHighScores();
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

        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnHighScore_Click(object sender, RoutedEventArgs e)
        {
            // from https://stackoverflow.com/questions/11133947/how-to-open-second-window-from-first-window-in-wpf
            // and https://stackoverflow.com/questions/30023419/how-to-call-a-variable-from-one-window-to-another-window
            HighScoreWindow hswindow = new HighScoreWindow(hsTemp);
            hswindow.Show();
        }
    }
}
