using System.Windows;
using System.Windows.Forms;
using Core;
using Timer = System.Windows.Forms.Timer;

namespace Sea_Battleship
{
    /// <summary>
    /// Логика взаимодействия для PauseWindow.xaml
    /// </summary>
    public partial class PauseWindow : Window
    {
        private Timer _timer;
        public PauseWindow(PlayerRole role)
        {
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

        private void UnpressPause()
        {
            // послать операцию клиенту
            _timer.Stop();
           Close();
        }
    }
}
