﻿using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Sea_Battleship
{
    public class Ship4 : AShip
    {
        public Ship4()
        {
            IsPlaced = false;
            IsHorizontal = true;
            Length = 4;
            Images = new Image[4];
            Images[0] = new Image
            {
                Stretch = Stretch.Fill,
                Source = new BitmapImage(new Uri("/Resources/1.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache },
                //LayoutTransform = new RotateTransform(90),
            };
            Images[1] = new Image
            {
                Stretch = Stretch.Fill,
                Source = new BitmapImage(new Uri("/Resources/2.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache },

            };
            Images[2] = new Image
            {
                Stretch = Stretch.Fill,
                Source = new BitmapImage(new Uri("/Resources/3.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache },
            };
            Images[3] = new Image
            {
                Stretch = Stretch.Fill,
                Source = new BitmapImage(new Uri("/Resources/4.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache },
            };
            for (int i = 0; i < Length; i++)
            {
                Images[i].MouseLeftButtonDown += Ship_MouseLeftButtonDown;
                Images[i].MouseRightButtonDown += Ship_MouseRightButtonDown;
            }
        }

        private void Ship_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowConfig.ShipState = WindowConfig.State.Ship4;
            IsPlaced = false;
            for (int i = 0; i < Length; i++)
            {
                PlayField.DeleteCell(Grid.GetColumn(Images[i]), Grid.GetRow(Images[i]), playField);
            }
        }

        private void Ship_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            int x = Grid.GetColumn(Images[0]);
            int y = Grid.GetRow(Images[0]);
            for (int i = 0; i < Length; i++)
            {
                PlayField.DeleteCell(Grid.GetColumn(Images[i]), Grid.GetRow(Images[i]), playField);
            }
            ChangeOrientation();
            Place(x, y, playField);
        }
    }
}
