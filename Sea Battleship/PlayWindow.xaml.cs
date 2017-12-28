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
using Core;
using Sea_Battleship.Engine;

namespace Sea_Battleship
{
    /// <summary>
    /// Логика взаимодействия для PlayWindow.xaml
    /// </summary>
    public partial class PlayWindow : Window
    {
        public OnlineGame OnlineGame { get; set; }
        public Game Game { get; set; }

        public PlayWindow(OnlineGame onlineGame)
        {
            WindowConfig.PlayWindowCon = this;
            WindowConfig.OnlineGame = onlineGame;
            OnlineGame = onlineGame;
            InitializeComponent();
            WindowConfig.ChangeSwitch();
        }

        public PlayWindow(Game game)
        {
            WindowConfig.game = game;
            InitializeComponent();
            Game = game;
        }

        private void audioChanged(object sender, RoutedEventArgs e)
        {
            WindowConfig.AudioChanged((Image)sender);

        }

        private void ExitItem_Click(object sender, RoutedEventArgs e)
        {
            Owner.Show();
            Close();
        }

        private void Pause_OnClick(object sender, RoutedEventArgs e)
        {
            if (!(OnlineGame is null))
            {
                if (OnlineGame.PlayerRole == PlayerRole.Server)
                {
                    OnlineGame.Turn(-2, -2);
                }
            }
            CheckPause();
        }

        public void CheckPause()
        {
            OnlineGame.TurnTimer.Stop();
            PauseWindow window = new PauseWindow(OnlineGame.PlayerRole);
            window.ShowDialog();
            OnlineGame.TurnTimer.Start();
        }
    }
}
