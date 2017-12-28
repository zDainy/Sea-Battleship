using System.Windows;
using System.Windows.Forms;
using Core;
using Sea_Battleship.Engine;
using Timer = System.Windows.Forms.Timer;

namespace Sea_Battleship
{
    /// <summary>
    /// Логика взаимодействия для PauseWindow.xaml
    /// </summary>
    public partial class PauseWindow : Window
    {
        private Timer _timer;
        private OnlineGame _onlineGame;

        public PauseWindow(PlayerRole role, OnlineGame onGame)
        {
            _onlineGame = onGame;
            InitializeComponent();
            PauseButton.IsEnabled = role == PlayerRole.Server;
            _timer = new Timer {Interval = 300000};
            _timer.Tick += (sender, e) => UnpressPause();
            _timer.Start();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UnpressPause();
        }

        public void UnpressPause()
        {
            if (_onlineGame.PlayerRole == PlayerRole.Server)
            {
                _onlineGame.Turn(-3, -3);
            }
            _timer.Stop();
           Close();
        }
    }
}
