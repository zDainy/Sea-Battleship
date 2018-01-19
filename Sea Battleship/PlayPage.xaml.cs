using Core;
using Sea_Battleship.Engine;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Network;
using WpfAnimatedGif;
using GameStatus = Core.GameStatus;

namespace Sea_Battleship
{
    /// <summary>
    /// Логика взаимодействия для PlayPage.xaml
    /// </summary>
    public partial class PlayPage : Page
    {
        public DispatcherTimer Timer;
        public OnlineGame OnlineGame { get; set; }
        public Game Game { get; set; }
        public bool IsPaused { get; set; }
        public int BeforePauseInt { get; set; }
        public TimeSpan BeforeTimeSpan { get; set; }

        public PlayPage(OnlineGame onlineGame)
        {
            WindowConfig.PlayPageCon = this;
            WindowConfig.OnlineGame = onlineGame;
            WindowConfig.IsLoaded = onlineGame.GameConfig.GameStatus == GameStatus.Loaded;
            OnlineGame = onlineGame;
            WindowConfig.GameState = WindowConfig.State.Online;
            InitializeComponent();
            WindowConfig.GetCurrentAudioImg(AudioImg);
            MyField.PlaceHitted();
            EnemyField.PlaceHitted();
            PauseItem.IsEnabled = OnlineGame.PlayerRole == PlayerRole.Server;
            SaveGameItem.IsEnabled = OnlineGame.PlayerRole == PlayerRole.Server;
            WindowConfig.SetStartColor();
            IsPaused = false;

            //ImageBehavior.SetAnimatedSource(TimerImage, new BitmapImage(new Uri("/Resources/timer.gif", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache });
            //ImageBehavior.SetAnimateInDesignMode(TimerImage, true);
            GameSpeed gs = WindowConfig.GameState == WindowConfig.State.Online ? OnlineGame.GameConfig.GameSpeed : Game.GameConfig.GameSpeed;
            switch (gs)
            {
                case GameSpeed.Fast:
                    timer.Interval = new TimeSpan(0, 0, 0, 1, 250);
                    break;
                case GameSpeed.Medium:
                    timer.Interval = new TimeSpan(0, 0, 0, 2, 500);
                    break;
                case GameSpeed.Slow:
                    timer.Interval = new TimeSpan(0, 0, 0, 5);
                    break;
                case GameSpeed.Turtle:
                    timer.Interval = new TimeSpan(0, 0, 0, 12, 500);
                    break;
            }

          //  timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Tick;
           // timer.Start();

            InitTimer();
            Timer.Start();
            timer.Start();
        }

        public DispatcherTimer timer = new DispatcherTimer();
        public int tickCount = 0;

        private void Tick(object sender, object e)
        {
            if (tickCount == 24)
            {
                if (IsPaused)
                    Pause();
                tickCount = 0;
                if(WindowConfig.GameState == WindowConfig.State.Offline)
                {
                    WindowConfig.SetSwitchColorOff(false);
                }
            }
            var controller = ImageBehavior.GetAnimationController(TimerImage);
            if (controller != null)
            {
                controller.GotoFrame(tickCount);
                controller.Pause();
                tickCount++;
            }
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
                    if (OnlineGame.IsMyTurn)
                        EnemyField.SwitchTurn(true);
                }
            }
            else
            {
                if (!IsPaused)
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

        public PlayPage(Game game)
        {
            WindowConfig.PlayPageCon = this;
            WindowConfig.game = game;
            WindowConfig.GameState = WindowConfig.State.Offline;
            InitializeComponent();
            WindowConfig.GetCurrentAudioImg(AudioImg);
            Game = game;
            PauseItem.IsEnabled = false;
            MyTurnLabel.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF93FF3A"));
            InitTimer();
            Timer.Start();

            GameSpeed gs = WindowConfig.GameState == WindowConfig.State.Online ? OnlineGame.GameConfig.GameSpeed : Game.GameConfig.GameSpeed;
            switch (gs)
            {
                case GameSpeed.Fast:
                    timer.Interval = new TimeSpan(0, 0, 0, 1, 250);
                    break;
                case GameSpeed.Medium:
                    timer.Interval = new TimeSpan(0, 0, 0, 2, 500);
                    break;
                case GameSpeed.Slow:
                    timer.Interval = new TimeSpan(0, 0, 0, 5);
                    break;
                case GameSpeed.Turtle:
                    timer.Interval = new TimeSpan(0, 0, 0, 12, 500);
                    break;
            }
            timer.Tick += Tick;
            timer.Start();
        }

        private void audioChanged(object sender, RoutedEventArgs e)
        {
            WindowConfig.AudioChanged((Image)sender);
        }

        private void ExitItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult res = MessageBox.Show("Сохранить игру перед выходом?", "", MessageBoxButton.YesNoCancel);
            switch (res)
            {
                case MessageBoxResult.Yes:
                    Save();
                    Exit();
                    break;
                case MessageBoxResult.No:
                    Exit();
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }

        public void Exit(bool lastPlayer = false)
        {
            if (!(OnlineGame is null))
            {
                if (IsPaused)
                    Pause();
                if (OnlineGame.PlayerRole == PlayerRole.Server)
                {
                    if (!lastPlayer)
                    {
                        OnlineGame.Connect.Server.SendResponse(OpearationTypes.GameStatus,
                            new Network.GameStatus(GameStatus.Break));
                    }
                    OnlineGame.Connect.Server.Stop();
                }
                else
                {
                    if (!lastPlayer)
                    {
                        OnlineGame.Connect.Client.SendRequest(OpearationTypes.GameStatus,
                            new Network.GameStatus(GameStatus.Break));
                    }
                    OnlineGame.Connect.Client.Close();
                }
                tickCount = 0;
                pr1.Value = 0;
                timer.Stop();
                Timer.Stop();
                OnlineGame = null;
            }
            timer.Tick -= Tick;
            Timer.Tick -= TimerTick;
            timer = null;
            Timer = null;

            WindowConfig.OnlineGame = null;
            WindowConfig.game = null;
            WindowConfig.IsLoaded = false;
            NavigationService.Navigate(new Uri("MainPage.xaml", UriKind.Relative));
            WindowConfig.PlayPageCon = null;
        }

        private void SaveGameItem_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            Pause();
        }

        public void Pause()
        {
            if (!IsPaused)
            {
                IsPaused = true;
                OnlineGame.Connect.Server.SendResponse(OpearationTypes.GameStatus,
                    new Network.GameStatus(GameStatus.Pause));
                PauseItem.Header = "Снять паузу";
                SetPause();
                EnemyField.IsEnabled = false;
            }
            else
            {
                IsPaused = false;
                OnlineGame.Connect.Server.SendResponse(OpearationTypes.GameStatus,
                    new Network.GameStatus(GameStatus.Game));
                Unpause();
                EnemyField.IsEnabled = true;
            }
        }

        public void SetPause()
        {
            BeforePauseInt = tickCount;
            tickCount = 0;
            var controller = ImageBehavior.GetAnimationController(TimerImage);
            controller?.GotoFrame(0);
            timer.Stop();
            BeforeTimeSpan = timer.Interval;
            timer.Interval = new TimeSpan(0, 0, 0, 12, 500);
            timer.Start();
            pr1.Visibility = Visibility.Hidden;
            MyTurnLabel.Background =
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFED8337"));
            MyTurnLabel.Content = "Пауза";
        }

        public void Unpause()
        {
            timer.Stop();
            tickCount = BeforePauseInt;
            var controller = ImageBehavior.GetAnimationController(TimerImage);
            controller?.GotoFrame(tickCount);
            timer.Interval = BeforeTimeSpan;
            timer.Start();
            if (OnlineGame.PlayerRole == PlayerRole.Server)
            {
                MyTurnLabel.Background =
                    new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF93FF3A"));
                MyTurnLabel.Content = "Ваш ход";
                PauseItem.Header = "Пауза";
                pr1.Visibility = Visibility.Visible;
            }
            else
            {
                MyTurnLabel.Background =
                    new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF00287E"));
                MyTurnLabel.Content = "Чужой ход";
            }
        }

        private void Save()
        {
            if (WindowConfig.GameState == WindowConfig.State.Online)
            {
                bool needUnPause = false;
                if (!IsPaused)
                {
                    Pause();
                    needUnPause = true;
                }
                new SaveGameWindow().ShowDialog();
                if (needUnPause)
                    Pause();
            }
            else
            {
                new SaveGameWindow().ShowDialog();
            }
        }

        private void RuleItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("Spravka.html");
            }
            catch
            {
                MessageBox.Show("Справка отсутствует");
            }
        }

        private void AboutItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Игру создали студенты группы 6403:\nКотов Алексей\nОнисич Степан\nШибаева Александра", "Об авторах", MessageBoxButton.OK);
        }
    }
}
