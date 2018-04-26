//---------------------------------------------
//File:   HelpWindow.xaml.cs
//Desc:   This file contains info regarding how
//        to play ShipBattles.
//---------------------------------------------
using System.Windows;

namespace ShipBattlesApp
{
    // Standard window code for the Help screen.
    public partial class HelpWindow : Window
    {
        // Same as above.
        public HelpWindow()
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