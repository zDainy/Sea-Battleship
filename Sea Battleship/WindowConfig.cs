using Core;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Sea_Battleship.Engine;
using System.Windows.Media;

namespace Sea_Battleship
{
    static class WindowConfig
    {
        public static MainPage MainPage;
        public static bool IsLoaded = false;
        public static PlayPage PlayPageCon;

        public enum State
        {
            Online,
            Offline
        };

        public static State GameState;
        public static Game game;
        public static OnlineGame OnlineGame;
        private static bool _audio = true;

        public static bool Audio
        {
            get => _audio;
            set => _audio = value;
        }

        public static void AudioChanged(Image image)
        {
            if (Audio)
            {
                image.Source =
                    new BitmapImage(new Uri("/Resources/no-audio.png", UriKind.Relative))
                    {
                        CreateOptions = BitmapCreateOptions.IgnoreImageCache
                    };
                Audio = false;
            }
            else
            {
                image.Source =
                    new BitmapImage(new Uri("/Resources/audio.png", UriKind.Relative))
                    {
                        CreateOptions = BitmapCreateOptions.IgnoreImageCache
                    };
                Audio = true;
            }
        }

        public static void SetStartColor()
        {
            if (OnlineGame.PlayerRole == PlayerRole.Client)
            {
                PlayPageCon.Dispatcher.Invoke(() =>
                {
                    PlayPageCon.pr1.Visibility = Visibility.Hidden;
                    PlayPageCon.MyTurnLabel.Background =
                        new SolidColorBrush((Color) ColorConverter.ConvertFromString("#FF00287E"));
                    PlayPageCon.MyTurnLabel.Content = "Чужой ход";
                });
            }
            else
            {
                PlayPageCon.Dispatcher.Invoke(() =>
                {
                    PlayPageCon.MyTurnLabel.Background =
                        new SolidColorBrush((Color) ColorConverter.ConvertFromString("#FF93FF3A"));
                });
            }
        }
    }
}
