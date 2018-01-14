using Core;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Sea_Battleship.Engine;
using System.Windows.Media;
using System.Windows.Navigation;

namespace Sea_Battleship
{
    static class WindowConfig
    {
        public static NavigationService NavigationService;
        public static MediaPlayer Player = new MediaPlayer();
       // public static System.Media.SoundPlayer Player = new System.Media.SoundPlayer() {Stream=Properties.Resources.pirat};
        public static System.Media.SoundPlayer ShotSound = new System.Media.SoundPlayer() { Stream=Properties.Resources.krik};
        public static string GifPath = "/Resources/drawn2.gif";
        public static MainPage MainPage;
        public static PlacingPage PlacingPage;
        public static bool IsLoaded = false;
        public static PlayPage PlayPageCon;

        public static void PlaySound()
        {
            //MediaPlayer lala = new MediaPlayer();
            //string str = Properties.Resources.krik.ToString();
            //Uri uri = new Uri(@"C:\Users\Пользователь\Desktop\Наиболее морской бой\Sea-Battleship\Sea Battleship\Resources\krik.wav", System.UriKind.Relative);
            //lala.Open(uri);
            //lala.Play();
            ShotSound.Play();
        }

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
                Player.Stop();
                Audio = false;
            }
            else
            {
                image.Source =
                    new BitmapImage(new Uri("/Resources/audio.png", UriKind.Relative))
                    {
                        CreateOptions = BitmapCreateOptions.IgnoreImageCache
                    };
                Player.Play();
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
