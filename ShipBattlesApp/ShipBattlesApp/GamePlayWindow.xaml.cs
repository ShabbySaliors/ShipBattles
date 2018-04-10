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
using ShipBattlesModel;
using System.Windows.Threading;

namespace ShipBattlesApp
{
    /// <summary>
    /// Interaction logic for GamePlayWindow.xaml
    /// </summary>
    public partial class GamePlayWindow : Window
    {
        Controller ctrl = new Controller();
        DispatcherTimer iterationTimer = new DispatcherTimer();
        public GamePlayWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ctrl.LoadWorld();
            ctrl.MakePlottibles();
            Console.WriteLine(GameWorld.Instance.Plottibles.Count);
            foreach (GameObject obj in GameWorld.Instance.Plottibles)
            {
                PlotObject(obj);
            }

            iterationTimer.Interval = new TimeSpan(0, 0, 0, 0, 100); // 50 ms
            iterationTimer.Tick += Timer_Tick;
            iterationTimer.Start();
        }

        // Mr. Malloy
        // Looking for more help over the summer. If I'm interested, I can let Mr. Malloy know.
        // Call Malachi back. 
        private void Timer_Tick(object sender, EventArgs e)
        {
            ctrl.IterateGame();
            GameBoardCanvas.Children.Clear();
            foreach (GameObject obj in GameWorld.Instance.Plottibles)
                PlotObject(obj);
        }

        private void PlotObject(GameObject obj)
        {
            Image newImage = new Image();
            newImage.Source = new BitmapImage(new Uri(obj.ImageFilepath, UriKind.Relative));
            newImage.Width = 2 * obj.CollideBoxSize + 5;
            newImage.Height = 2 * obj.CollideBoxSize + 5;
            GameBoardCanvas.Children.Add(newImage);
            // newImage.MouseDown += Img_MouseDown;
            // Here we must concider the possibility that the images move instead of all being replotted every time. 
            // What if they just moved up when the "s" key was pressed, down when the "W" key is pressed, and so on? 
            Canvas.SetLeft(newImage, obj.Loc.X - newImage.Width / 2);
            Canvas.SetTop(newImage, obj.Loc.Y - newImage.Height / 2);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.W)
            {
                ctrl.PlayerShip.Direct.Up = 1;
            }
            else if (e.Key == Key.A)
                ctrl.PlayerShip.Direct.Right = -1;
            else if (e.Key == Key.S)
                ctrl.PlayerShip.Direct.Up = -1;
            else if (e.Key == Key.D)
                ctrl.PlayerShip.Direct.Right = 1;
            else if (e.Key == Key.Space)
                ctrl.PlayerShip.ToShoot = true;
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.W || e.Key == Key.S)
                ctrl.PlayerShip.Direct.Up = 0;
            else if (e.Key == Key.A || e.Key == Key.D)
                ctrl.PlayerShip.Direct.Right = 0;
        }

        // To handle the logic for moving the player's ship, I need to make sure that the ship will move while the 
        // WASD keys are being pressed, and then stop moving yet still shoot while the keys are not being depressed.
        // The first solution which comes to mind is creating a variable witin


        private void Wind_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.W)
                ctrl.PlayerShip.Direct.Up = -1;
            else if (e.Key == Key.A)
                ctrl.PlayerShip.Direct.Right = 1;
            else if (e.Key == Key.S)
                ctrl.PlayerShip.Direct.Up = -1;
            else if (e.Key == Key.D)
                ctrl.PlayerShip.Direct.Right = 1;
            else if (e.Key == Key.Space)
                ctrl.PlayerShip.ToShoot = true;
        }

        private void Wind_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.W || e.Key == Key.S)
                ctrl.PlayerShip.Direct.Up = 0;
            else if (e.Key == Key.A || e.Key == Key.D)
                ctrl.PlayerShip.Direct.Right = 0;
        }

        // Another problem is that I need to center the coordinate of the picture and then rotate the picture with 
        // respect to that coordinate according to the direction of the object.
    }
}
