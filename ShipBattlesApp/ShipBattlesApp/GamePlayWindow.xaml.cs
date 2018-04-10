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

namespace ShipBattlesApp
{
    /// <summary>
    /// Interaction logic for GamePlayWindow.xaml
    /// </summary>
    public partial class GamePlayWindow : Window
    {
        Controller ctrl = new Controller();
        public GamePlayWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ctrl.LoadWorld();
            GameWorld.Instance.MakePlottibles();
            foreach (GameObject obj in GameWorld.Instance.Plottibles)
            {
                PlotObject(obj);
            }
        }

        private void PlotObject(GameObject obj)
        {
            Image newImage = new Image();
            newImage.Source = new BitmapImage(new Uri(obj.ImageFilepath, UriKind.Relative));
            //newImage.Width = imgWidth;
            //newImage.Height = imgHieght;
            GameBoardCanvas.Children.Add(newImage);
            // newImage.MouseDown += Img_MouseDown;
            // Here we must concider the possibility that the images move instead of all being replotted every time. 
            // What if they just moved up when the "s" key was pressed, down when the "W" key is pressed, and so on? 
            Canvas.SetLeft(newImage, obj.Loc.X);
            Canvas.SetTop(newImage, obj.Loc.Y);
        }
    }
}
