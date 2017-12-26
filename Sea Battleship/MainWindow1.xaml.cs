﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Common;
using Core;

namespace Sea_Battleship
{
    /// <summary>
    /// Логика взаимодействия для MainWindow1.xaml
    /// </summary>
    public partial class MainWindow1 : Window
    {
        public MainWindow1()
        {
            InitializeComponent();
            LogService.Start();
            LogService.Trace("==============================================");
            //for (int y = 0; y < 10; y++)
            //    for (int x = 0; x < 10; x++)
            //    {
            //        Image img = new Image
            //        {
            //            Stretch = Stretch.Fill,
            //            Source = new BitmapImage(new Uri("/Resources/Water.jpg", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache },
            //            Opacity = 0
            //        };
            //        img.MouseLeftButtonUp += FieldCell_Click;
            //        gr.Children.Add(img);
            //        Grid.SetColumn(img, x);
            //        Grid.SetRow(img, y);
            //    }
            //new WaitingWindow().Show();
        }


        private void audioChanged(object sender, RoutedEventArgs e)
        {
            WindowConfig.AudioChanged((Image)sender);
        }

        private void Loading_Click(object sender, RoutedEventArgs e)
        {
            LoadingWindow lw = new LoadingWindow
            {
                Owner = this.Owner,
                z = 3
            };
            lw.Show();
            Hide();
        }

        private void NewGame_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            //PlacingWindow lw = new PlacingWindow();
            //lw.Show();
            //Hide();
        }





        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            TestWindow test = new TestWindow();
            test.Show();
            Hide();
        }

        private void CreateLobby_Click(object sender, RoutedEventArgs e)
        {
            ConfigOnlineHostWindow window = new ConfigOnlineHostWindow();
            window.Owner = this;
            window.Show();
            this.Hide();
        }



        private void ConnetToLobbyItem_Click(object sender, RoutedEventArgs e)
        {
            ConfigOnlineNotHostWindow window = new ConfigOnlineNotHostWindow();
            window.Owner = this;
            window.Show();
            this.Hide();
        }

        private void AboutItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Нет", "да", MessageBoxButton.OK);
            
        }
    }
}
