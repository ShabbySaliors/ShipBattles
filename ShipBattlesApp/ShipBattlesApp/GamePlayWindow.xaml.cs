using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Media;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.Windows.Shapes;
using ShipBattlesModel;
using System.Windows.Threading;

namespace ShipBattlesApp
{
    /// <summary>
    /// Interaction logic for GamePlayWindow.xaml
    /// </summary>
    public partial class GamePlayWindow : Window
    {
        private SoundPlayer playerLaserPlayer = new SoundPlayer("../../Audio/playerLaser.wav");
        public Controller ctrl = new Controller();
        DispatcherTimer iterationTimer = new DispatcherTimer();
        HighScore hs;

        public GamePlayWindow(HighScore hstemp)
        {
            hs = hstemp;
            InitializeComponent();
        }

        public GamePlayWindow(HighScore hstemp, string name)
        {
            hs = hstemp;
            ctrl.Username = name;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Name.Text = ctrl.Username;
            if (GameWorld.Instance.Level != 0) ctrl.LoadWorld(GameWorld.Instance.Level);
            else
            {
                GameWorld.Instance.Level = 1;
                ctrl.LoadWorld(GameWorld.Instance.Level);
            }
            // Potentially put an if statement here which loads the level of the game. (Done)
            ctrl.MakePlottibles();
            Console.WriteLine(GameWorld.Instance.Plottibles.Count);
            foreach (GameObject obj in GameWorld.Instance.Plottibles) PlotObject(obj);
            iterationTimer.Interval = new TimeSpan(0, 0, 0, 0, 20); // 100 ms
            iterationTimer.Tick += Timer_Tick;
            iterationTimer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            ctrl.IterateGame();
            GameBoardCanvas.Children.Clear();
            foreach (GameObject obj in GameWorld.Instance.Plottibles) PlotObject(obj);
            if (ctrl.IsGameOver())
            {
                hs.SaveHighScore(ctrl.Username, GameWorld.Instance.Score, false);
                iterationTimer.Stop();
                AnimateEnding();
            }
            ctrl.IsLevelOver();
            TimerBlock.Text = ctrl.LevelTimer.Write();
            PointsBlock.Text = "Points: " + GameWorld.Instance.Score.ToString();
            LivesBlock.Text = "Lives: " + ctrl.PlayerShip.Lives;
        }

        private void PlotObject(GameObject obj)
        {
            Image newImage = new Image();
            newImage.Source = new BitmapImage(new Uri(obj.ImageFilepath, UriKind.Relative));
            newImage.Width = 2 * obj.CollideBoxSize + 5;
            newImage.Height = 2 * obj.CollideBoxSize + 5;
            GameBoardCanvas.Children.Add(newImage);
            // newImage.MouseDown += Img_MouseDown;
            // Here we must consider the possibility that the images move instead of all being replotted every time. 
            // What if they just moved up when the "s" key was pressed, down when the "W" key is pressed, and so on?
            if (obj == ctrl.PlayerShip)
            {
                Canvas.SetLeft(newImage, (GameBoardCanvas.Width + GameBoardCanvas.Width % 2) / 2 - newImage.Width / 2);
                Canvas.SetTop(newImage, (GameBoardCanvas.Height + GameBoardCanvas.Height % 2) / 2 - newImage.Height / 2);
            } else
            {
                Canvas.SetLeft(newImage, (GameBoardCanvas.Width + GameBoardCanvas.Width % 2) / 2 + ctrl.PlayerShip.FindX_Dist(obj) - newImage.Width / 2);
                Canvas.SetTop(newImage, (GameBoardCanvas.Height + GameBoardCanvas.Height % 2) / 2 + ctrl.PlayerShip.FindY_Dist(obj) - newImage.Height / 2);
            }

            //Canvas.SetLeft(newImage, obj.Loc.X - newImage.Width / 2);
            //Canvas.SetTop(newImage, obj.Loc.Y - newImage.Height / 2);
        }

        // To handle the logic for moving the player's ship, I need to make sure that the ship will move while the 
        // WASD keys are being pressed, and then stop moving yet still shoot while the keys are not being depressed.
        // The first solution which comes to mind is creating a variable witin
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.W) ctrl.PlayerShip.Direct.Up = -1;
            else if (e.Key == Key.A) ctrl.PlayerShip.Direct.Right = -1;
            else if (e.Key == Key.S) ctrl.PlayerShip.Direct.Up = 1;
            else if (e.Key == Key.D) ctrl.PlayerShip.Direct.Right = 1;
            else if (e.Key == Key.Space)
            {
                playerLaserPlayer.Play();
                ctrl.PlayerShip.ToShoot = true;
            }
            else if (e.Key == Key.C)
            {
                if (ctrl.PlayerShip.IsInCheatMode) ctrl.PlayerShip.IsInCheatMode = false;
                else ctrl.PlayerShip.IsInCheatMode = true;
            }
            else if (e.Key == Key.Q)
            {
                if (!ctrl.IsGameOver())
                {
                    if (iterationTimer.IsEnabled)
                    {
                        iterationTimer.Stop();
                        ctrl.Save();
                    }
                    else if (!iterationTimer.IsEnabled)
                    {
                        iterationTimer.Start();
                    }
                }
            }
            // from https://stackoverflow.com/questions/19013087/how-to-detect-multiple-keys-down-onkeydown-event-in-wpf
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.S))
            {
                iterationTimer.Stop();
                ctrl.Save();
                this.Close();
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.W || e.Key == Key.S) ctrl.PlayerShip.Direct.Up = 0;
            else if (e.Key == Key.A || e.Key == Key.D) ctrl.PlayerShip.Direct.Right = 0;
        }

        // Another problem is that I need to center the coordinate of the picture and then rotate the picture with 
        // respect to that coordinate according to the direction of the object.

        private void AnimateEnding()
        {
            string msg = "Score: ";
            msg += Convert.ToString(GameWorld.Instance.Score);
            msg += "\nCheck to see if you got a high-score!";
            TextBox gameOver = new TextBox()
            {
                Margin = new Thickness(40, 0, 40, 0),
                Text = msg,
                TextAlignment = TextAlignment.Center,
                FontSize = 40,
                FontWeight = FontWeights.ExtraBold,
                Background = Brushes.Red,
            };
            gameOver.IsEnabled = false;

            Window wind = new Window()
            {
                Title = "Game Over",
                Height = 300,
                Width = 850,
                ForceCursor = true,
                WindowStyle = WindowStyle.ToolWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Visibility = Visibility.Visible,
                Background = Brushes.DarkBlue,
                Name = "wind",
                Content = gameOver
            };
        }

        // do we need this?
        public void PrintTimer()
        {
            
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (iterationTimer.IsEnabled) iterationTimer.Stop();
        }
    }
}
