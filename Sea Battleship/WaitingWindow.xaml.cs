using System.Windows;
using Common;
using Core;
using Sea_Battleship.Engine;
using System;

namespace Sea_Battleship
{
    /// <summary>
    /// Логика взаимодействия для WaitingWindow.xaml
    /// </summary>
    public partial class WaitingWindow : Window
    {
        public OnlineGame OnlineGame { get; set; }
        public ShipArrangement Arrangment { get; set; }
        public PlacementState Placement { get; set; }

        public WaitingWindow(OnlineGame onlineGame, ShipArrangement arrangment, PlacementState placement)
        {
            InitializeComponent();
            OnlineGame = onlineGame;
            Arrangment = arrangment;
            Placement = placement;
           //Wait();
        }

        public void Wait()
        {
            while (!OnlineGame.Connect.Server.IsClientConnected) { }
            if (Placement == PlacementState.Manualy)
            {
                PlacingPage window = new PlacingPage(OnlineGame);
                WindowConfig.MainPage.NavigationService.Navigate(window, UriKind.Relative);
            }
            else if (Placement == PlacementState.Loaded)
            {
                OnlineGame.LoadGame(OnlineGame.Game);
                PlayPage window = new PlayPage(OnlineGame);
                WindowConfig.MainPage.NavigationService.Navigate(window, UriKind.Relative);
            }
            else
            {
                OnlineGame.CreateGame(Arrangment);
                PlayPage window = new PlayPage(OnlineGame);
                WindowConfig.MainPage.NavigationService.Navigate(window, UriKind.Relative);
            }
            Close();
        }
    }
}
