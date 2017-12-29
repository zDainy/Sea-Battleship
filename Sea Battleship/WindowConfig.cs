using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Sea_Battleship.Engine;
using System.Windows.Media;

namespace Sea_Battleship
{
    static class WindowConfig
    {
        public static bool IsLoaded = false;
        public static PlayWindow PlayWindowCon;
        public enum State { Online, Offline };
        public static State GameState;
        public static Game game;
        public static OnlineGame OnlineGame;
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

        public static void ChangeSwitch()
        {
            if (OnlineGame.PlayerRole == PlayerRole.Client)
            {
                WindowConfig.PlayWindowCon.Dispatcher.Invoke(() =>
                {
                    //_pw.EnemyTurnLabel.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF00287E")); //(Brush)new BrushConverter().ConvertFromString("#FF00287E");
                    WindowConfig.PlayWindowCon.MyTurnLabel.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF00287E"));
                });
            }
            else
            {
                WindowConfig.PlayWindowCon.Dispatcher.Invoke(() =>
                {
                    //_pw.EnemyTurnLabel.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF00287E")); //(Brush)new BrushConverter().ConvertFromString("#FF00287E");
                    WindowConfig.PlayWindowCon.MyTurnLabel.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF93FF3A"));
                });
            }
        }
    }
}
