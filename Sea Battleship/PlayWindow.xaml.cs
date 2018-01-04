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
using System.Windows.Threading;

namespace Sea_Battleship
{
    /// <summary>
    /// Логика взаимодействия для PlayWindow.xaml
    /// </summary>
    public partial class PlayWindow : Window
    {
        public DispatcherTimer Timer;
        public OnlineGame OnlineGame { get; set; }
        public Game Game { get; set; }

        public PlayWindow(OnlineGame onlineGame)
        {
            WindowConfig.PlayWindowCon = this;
            WindowConfig.OnlineGame = onlineGame;
            OnlineGame = onlineGame;
            WindowConfig.GameState = WindowConfig.State.Online;
            InitializeComponent();
            WindowConfig.SetStartColor();
            InitTimer();
            Timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (pr1.Value == 100)
            {
                pr1.Value = 0;
                if (WindowConfig.GameState == WindowConfig.State.Offline)
                {
                    Game.ChangeTurn();
                    EnemyField.ChangeTurn(this);
                }
                else
                {
                    EnemyField.SwitchTurn(true);
                }
            }
            else
            {
                pr1.Value++;
            }
        }

        public void InitTimer()
        {
            Timer = new DispatcherTimer();  // если надо, то в скобках указываем приоритет, например DispatcherPriority.Render
            Timer.Tick += TimerTick;
            GameSpeed gs = WindowConfig.GameState == WindowConfig.State.Online ? OnlineGame.GameConfig.GameSpeed : Game.GameConfig.GameSpeed;
            switch (gs)
            {
                case GameSpeed.Fast:
                    Timer.Interval = new TimeSpan(0, 0, 0, 0, 300);
                    break;
                case GameSpeed.Medium:
                    Timer.Interval = new TimeSpan(0, 0, 0, 0, 600);
                    break;
                case GameSpeed.Slow:
                    Timer.Interval = new TimeSpan(0, 0, 0, 1, 200);
                    break;
                case GameSpeed.Turtle:
                    Timer.Interval = new TimeSpan(0, 0, 0, 3, 0);
                    break;
            }
        }

        public PlayWindow(Game game)
        {
            WindowConfig.PlayWindowCon = this;
            WindowConfig.game = game;
            WindowConfig.GameState = WindowConfig.State.Offline;
            InitializeComponent();
            Game = game;
            MyTurnLabel.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF93FF3A"));
            InitTimer();
            Timer.Start();
        }

        private void audioChanged(object sender, RoutedEventArgs e)
        {
            WindowConfig.AudioChanged((Image)sender);
            
        }

        private void ExitItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult res = MessageBox.Show("Сохранить игру перед выходом?", "", MessageBoxButton.YesNoCancel);
            switch(res)
            {
                case MessageBoxResult.Yes:
                    Save();
                    Owner.Show();
                    Close();
                    break;
                case MessageBoxResult.No:
                    //наверно уведомить второго игрока
                    Owner.Show();
                    Close();
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }

        private void SaveGameItem_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void Save()
        {
            if (WindowConfig.GameState == WindowConfig.State.Online)
            {

            }
            else
            {
                new SaveGameWindow().Show();
            }
        }
    }
}
