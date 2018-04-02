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
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnEasy_Click(object sender, RoutedEventArgs e)
        {
            btnEasy.Background = new SolidColorBrush(Colors.Green);
            btnMed.Background = new SolidColorBrush(Colors.LightGray);
            btnHard.Background = new SolidColorBrush(Colors.LightGray);
            gameMode = GameMode.Easy;
        }

        private void btnMed_Click(object sender, RoutedEventArgs e)
        {
            btnEasy.Background = new SolidColorBrush(Colors.LightGray);
            btnMed.Background = new SolidColorBrush(Colors.Green);
            btnHard.Background = new SolidColorBrush(Colors.LightGray);
            gameMode = GameMode.Medium;
        }

        private void btnHard_Click(object sender, RoutedEventArgs e)
        {
            btnEasy.Background = new SolidColorBrush(Colors.LightGray);
            btnMed.Background = new SolidColorBrush(Colors.LightGray);
            btnHard.Background = new SolidColorBrush(Colors.Green);
            gameMode = GameMode.Hard;
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

        }
    }
}
