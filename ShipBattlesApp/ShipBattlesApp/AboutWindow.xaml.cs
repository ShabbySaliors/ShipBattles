//---------------------------------------------
//File:   AboutWindow.xaml.cs
//Desc:   This file contains minimum info about
//        the programmers of ShipBattles.
//---------------------------------------------
using System.Windows;

namespace ShipBattlesApp
{
    // Standard window code for the About screen.
    public partial class AboutWindow : Window
    {
        // Same as above
        public AboutWindow()
        {
            InitializeComponent();
        }

        // Makes sure that the window is exactly the size of the minimum screen resolution.
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.MinWidth = 1440.0;
            this.MinHeight = 900.0;
        }
    }
}