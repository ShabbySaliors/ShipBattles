﻿//-------------------------------------------------------
//File:   GameplayWindow.xaml.cs
//Desc:   This file contains essential gameplay code that
//        the other files need in order to operate, such
//        as a public Controller instance.
//-------------------------------------------------------
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using ShipBattlesModel;

namespace ShipBattlesApp
{
    // Code for making the gameplay, well, play.
    public partial class GamePlayWindow : Window
    {
        // Used for playing the 'laser fired' sound effect when the player hits SPACEBAR.
        // Does not play if the game is over.
        private SoundPlayer playerLaserPlayer = new SoundPlayer("../../Audio/playerLaser.wav");
        // A public Controller instance that is used by multiple files.
        public Controller ctrl = new Controller();
        // A timer that always runs unless the game is paused or over.
        DispatcherTimer iterationTimer = new DispatcherTimer();
        // A HighScore instance that is potentially updated when the game is over.
        HighScore hs;
        // The name to display in the stackpanel.
        string nameToShow;

        // Begins constructing a game screen for a saved game.
        public GamePlayWindow(HighScore hstemp)
        {
            hs = hstemp;
            InitializeComponent();
        }

        // Begins constructing a game screen for a new game.
        // Also switches the underscores in 'name' with spaces
        // to prep for displaying in the stackpanel.
        public GamePlayWindow(HighScore hstemp, string name)
        {
            hs = hstemp;
            ctrl.Username = name;
            foreach (char c in name)
            {
                if (c == '_') nameToShow += ' ';
                else nameToShow += c;
            }
            InitializeComponent();
        }

        // Executes upon loading the MainWindow.
        // Sets up the necessary data for gameplay to begin, then starts gameplay
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Name.Text = nameToShow;
            if (GameWorld.Instance.LoadedGame)
            {
                ctrl.LoadWorld(1);
                ctrl.Load();
                ctrl.MakePlottibles();
                Name.Text = ctrl.Username;
            }
            else
            {
                GameWorld.Instance.Level = 1;
                ctrl.LoadWorld(GameWorld.Instance.Level);
            }
            ctrl.MakePlottibles();
            Console.WriteLine(GameWorld.Instance.Plottibles.Count);
            foreach (GameObject obj in GameWorld.Instance.Plottibles) PlotObject(obj);
            iterationTimer.Interval = new TimeSpan(0, 0, 0, 0, 20);
            iterationTimer.Tick += Timer_Tick;
            iterationTimer.Start();
        }

        // Runs each time the timer fires.
        // Calls the method that updates the position of all of the
        // objects on the screen, save the player's ship.
        // Then it saves the score (if it is a high-score) and stops
        // gameplay if the game is over.
        private void Timer_Tick(object sender, EventArgs e)
        {
            ctrl.IterateGame();
            GameBoardCanvas.Children.Clear();
            foreach (GameObject obj in GameWorld.Instance.Plottibles) PlotObject(obj);
            if (ctrl.IsGameOver())
            {
                hs.SaveHighScore(ctrl.Username, GameWorld.Instance.Score, false);
                iterationTimer.Stop();
                GameOverBlock.Text = "Game Over!";
            }
            ctrl.IsLevelOver();
            TimerBlock.Text = ctrl.LevelTimer.Write();
            PointsBlock.Text = "Points: " + GameWorld.Instance.Score.ToString();
            PointsBlock.Text = "Score: " + GameWorld.Instance.Score.ToString();
            LivesBlock.Text = "Hitpoints: " + ctrl.PlayerShip.Lives;
        }

        // Updates the position of all of the objects on the screen,
        // save the player's ship.
        private void PlotObject(GameObject obj)
        {
            Image newImage = new Image();
            newImage.Source = new BitmapImage(new Uri(obj.ImageFilepath, UriKind.Relative));
            newImage.Width = 2 * obj.CollideBoxSize + 5;
            newImage.Height = 2 * obj.CollideBoxSize + 5;
            GameBoardCanvas.Children.Add(newImage);
            if (obj == ctrl.PlayerShip)
            {
                Canvas.SetLeft(newImage, (GameBoardCanvas.Width + GameBoardCanvas.Width % 2) / 2 - newImage.Width / 2);
                Canvas.SetTop(newImage, (GameBoardCanvas.Height + GameBoardCanvas.Height % 2) / 2 - newImage.Height / 2);
            } else
            {
                Canvas.SetLeft(newImage, (GameBoardCanvas.Width + GameBoardCanvas.Width % 2) / 2 + ctrl.PlayerShip.FindX_Dist(obj) - newImage.Width / 2);
                Canvas.SetTop(newImage, (GameBoardCanvas.Height + GameBoardCanvas.Height % 2) / 2 + ctrl.PlayerShip.FindY_Dist(obj) - newImage.Height / 2);
            }
        }

        // Does various things that depend on what key is pressed.
        // If a key in WASD is pressed, it moves the ship up, left, down, or right
        // (in reality, it moves everything but the ship in the opposite direction)
        // Q pauses/unpauses the game as well as saves it.
        // C enables Cheat Mode.
        // Space fires the ship's GBG (green ball gun).
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.W)
            {
                ctrl.PlayerShip.Direct.Up = -1;

                if (ctrl.PlayerShip.Direct.Right == -1)
                {
                    ctrl.PlayerShip.ImageFilepath = "Images/playerShip_upLeft.png";
                }
                else if (ctrl.PlayerShip.Direct.Right == 1)
                {
                    ctrl.PlayerShip.ImageFilepath = "Images/playerShip_upRight.png";
                }
                else ctrl.PlayerShip.ImageFilepath = "Images/playerShip_up.png";
            }
            else if (e.Key == Key.A)
            {
                ctrl.PlayerShip.Direct.Right = -1;

                if (ctrl.PlayerShip.Direct.Up == -1)
                {
                    ctrl.PlayerShip.ImageFilepath = "Images/playerShip_upLeft.png";
                }
                else if (ctrl.PlayerShip.Direct.Up == 1)
                {
                    ctrl.PlayerShip.ImageFilepath = "Images/playerShip_downLeft.png";
                }
                else ctrl.PlayerShip.ImageFilepath = "Images/playerShip_left.png";
            }
            else if (e.Key == Key.S)
            {
                ctrl.PlayerShip.Direct.Up = 1;

                if (ctrl.PlayerShip.Direct.Right == -1)
                {
                    ctrl.PlayerShip.ImageFilepath = "Images/playerShip_downLeft.png";
                }
                else if (ctrl.PlayerShip.Direct.Right == 1)
                {
                    ctrl.PlayerShip.ImageFilepath = "Images/playerShip_downRight.png";
                }
                else ctrl.PlayerShip.ImageFilepath = "Images/playerShip_down.png";
            }
            else if (e.Key == Key.D)
            {
                ctrl.PlayerShip.Direct.Right = 1;

                if (ctrl.PlayerShip.Direct.Up == -1)
                {
                    ctrl.PlayerShip.ImageFilepath = "Images/playerShip_upRight.png";
                }
                else if (ctrl.PlayerShip.Direct.Up == 1)
                {
                    ctrl.PlayerShip.ImageFilepath = "Images/playerShip_downRight.png";
                }
                else ctrl.PlayerShip.ImageFilepath = "Images/playerShip_right.png";
            }
            else if (e.Key == Key.Space)
            {
                if (!ctrl.IsGameOver())
                {
                    playerLaserPlayer.Play();
                    ctrl.PlayerShip.ToShoot = true;
                }
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
                    ctrl.Save();
                    if (iterationTimer.IsEnabled)
                    {
                        iterationTimer.Stop();
                    }
                    else if (!iterationTimer.IsEnabled)
                    {
                        iterationTimer.Start();
                    }
                }
            }
        }

        // Handles various events when a key is released. 
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.W || e.Key == Key.S) ctrl.PlayerShip.Direct.Up = 0;
            else if (e.Key == Key.A || e.Key == Key.D) ctrl.PlayerShip.Direct.Right = 0;
            if (e.Key == Key.W || e.Key == Key.A || e.Key == Key.S || e.Key == Key.D)
            {
                if (ctrl.PlayerShip.Direct.Up == -1) ctrl.PlayerShip.ImageFilepath = "Images/playerShip_up.png";
                else if (ctrl.PlayerShip.Direct.Right == -1) ctrl.PlayerShip.ImageFilepath = "Images/playerShip_left.png";
                else if (ctrl.PlayerShip.Direct.Up == 1) ctrl.PlayerShip.ImageFilepath = "Images/playerShip_down.png";
                else if (ctrl.PlayerShip.Direct.Right == 1) ctrl.PlayerShip.ImageFilepath = "Images/playerShip_right.png";
            }        
        }

        // Ends the game when the game screen is closed.
        private void Window_Closed(object sender, EventArgs e)
        {
            if (iterationTimer.IsEnabled) iterationTimer.Stop();
        }
    }
}