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
        private PauseWindow _pWindow;

        public PlayWindow(OnlineGame onlineGame)
        {
            WindowConfig.PlayWindowCon = this;
            WindowConfig.OnlineGame = onlineGame;
            if (OnlineGame.PlayerRole == PlayerRole.Server)
            {
                Save.IsEnabled = true;
            }
            else
            {
                Save.IsEnabled = false;
            }
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
            CloseWin();
        }

        public void CloseWin()
        {
            if (OnlineGame.PlayerRole == PlayerRole.Server)
            {
                OnlineGame.Connect.Server.Stop();
            }
            else
            {
                OnlineGame.Connect.Client.Close();
            }
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
            if (OnlineGame.PlayerRole == PlayerRole.Server)
            {
                _pWindow = new PauseWindow(OnlineGame.PlayerRole, OnlineGame);
                _pWindow.ShowDialog();
                OnlineGame.TurnTimer.Start();
            }
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            FileSystem.SaveGame("Онлайн игра", OnlineGame.Game);
            MessageBox.Show("Игра сохранена", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
