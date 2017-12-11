﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Sea_Battleship
{
    static class WindowConfig
    {
        public enum State {None, Ship1, Ship2, Ship3, Ship4 };
        public static State ShipState;
       // public static bool isShipUp = false;
        private static bool _audio = true;
        public static bool Audio { get => _audio; set => _audio = value; }
        public static void AudioChanged(Image image)
        {
            if (WindowConfig.Audio)
            {
                image.Source = new BitmapImage(new Uri("/Resources/no-audio.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache };
                WindowConfig.Audio = false;
            }
            else
            {
                image.Source = new BitmapImage(new Uri("/Resources/audio.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache };
                WindowConfig.Audio = true;
            }
        }
    }
}