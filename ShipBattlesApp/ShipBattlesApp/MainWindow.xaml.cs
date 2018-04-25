//---------------------------------------------------------------
//File:   MainWindow.xaml.cs
//Desc:   This file contains the necessary code to show
//        every screen available, starting with the title screen.
//---------------------------------------------------------------
using System.IO;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ShipBattlesModel;
using System.Media;

namespace ShipBattlesApp
{
    // Contains code for summoning the various screens in the game.
    // It summons the Title Screen first by default.
    public partial class MainWindow : Window
    {
        // `SoundPlayer` instance used to play background music upon startup of MainWindow
        private SoundPlayer backgroundMusicPlayer = new SoundPlayer("../../Audio/MystOnTheMoor.wav");
        HighScore hs = new HighScore();
        Button oldBtn;
        Button btn;

        // Auto-generated class for loading background elements
        private void FrameworkElement_Loaded(object sender, RoutedEventArgs e)
        {

        }

        // Executes upon loading the MainWindow.
        // It sets the default difficulty to Easy.
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            backgroundMusicPlayer.PlayLooping();
            // https://docs.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement.minwidth?view=netframework-4.7.1#System_Windows_FrameworkElement_MinWidth
            this.MinWidth = 1440.0;
            this.MinHeight = 900.0;
            btnEasy.Background = new SolidColorBrush(Colors.Green);
            hs.CheckHighScoresFile(false);
            hs.LoadHighScores(false);
            oldBtn = btnEasy;
        }

        // Initializes the component. What more could you ask for?
        public MainWindow()
        {
            InitializeComponent();
        }

        // Handles all three difficulty buttons.
        // If a non-highlighted diff. button is clicked, it changes the difficulty
        // and highlights the selected button.
        private void btnDiff_Click(object sender, RoutedEventArgs e)
        {
            oldBtn.Background = new SolidColorBrush(Colors.LightGray);
            btn = (Button)sender;
            btn.Background = new SolidColorBrush(Colors.Green);
            oldBtn = btn;
            if (btn == btnEasy) GameWorld.Instance.Level = 1;
            else if (btn == btnMed) GameWorld.Instance.Level = 3;
            else if (btn == btnHard) GameWorld.Instance.Level = 6;
        }

        // If there is nothing in the username box, it shows an error window saying that
        // there is no username and does not start a new game.
        // Otherwise, deletes the save file, if it exists, and starts a new game.
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            string tempName = nameBox.Text;
            string name = "";
            List<int> indexList = new List<int> { };
            if (nameBox.Text == "")
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

            if (File.Exists("SaveFile.txt"))
            {
                File.Delete("SaveFile.txt");
            }
            backgroundMusicPlayer.Stop();
            GamePlayWindow gpwindow = new GamePlayWindow(hs, name);
            gpwindow.Show();
        }

        // Opens the About screen. That's 'about' it.
        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.Show();
        }

        // Opens the Help screen.
        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            HelpWindow helpWindow = new HelpWindow();
            helpWindow.Show();
        }

        // Opens the High Scores screen and gives it the necessary data
        // to load the high-scores.
        private void btnHighScore_Click(object sender, RoutedEventArgs e)
        {
            // from https://stackoverflow.com/questions/11133947/how-to-open-second-window-from-first-window-in-wpf
            // and https://stackoverflow.com/questions/30023419/how-to-call-a-variable-from-one-window-to-another-window
            HighScoreWindow hswindow = new HighScoreWindow(hs);
            hswindow.Show();
        }

        // Checks to see if there is a save file.
        // If there is, it loads the save file and starts a game with it.
        // Otherwise, it shows an error window saying that there is no save file to load.
        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists("SaveFile.txt"))
            {
                TextBox noSaveFile = new TextBox()
                {
                    Margin = new Thickness(40, 0, 40, 0),
                    Text = "There is no save file.",
                    TextAlignment = TextAlignment.Center,
                    FontSize = 70,
                    FontWeight = FontWeights.ExtraBold,
                    Foreground = Brushes.Green,
                };
                noSaveFile.IsEnabled = false;

                Window noSaveWind = new Window()
                {
                    Title = "There is no save file.",
                    Height = 150,
                    Width = 1000,
                    Visibility = Visibility.Visible,
                    Name = "noSaveWind",
                    Content = noSaveFile
                };
                GameWorld.Instance.LoadedGame = false;
                e.Handled = false;
                return;
            }
            backgroundMusicPlayer.Stop();
            GameWorld.Instance.LoadedGame = true;
            GamePlayWindow gpwindow = new GamePlayWindow(hs);
            gpwindow.Show();
        }

        // Checks to see if the key that was pressed is a letter or a space.
        // If the key is neither, the event handler stops the key's corresponding character
        // from being entered into the username textbox.
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
