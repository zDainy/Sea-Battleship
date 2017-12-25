using System.Windows;
using Common;
using Core;
using Sea_Battleship.Engine;

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
        }

        public void Wait()
        {
            while (!OnlineGame.Connect.Server.IsClientConnected) { }
            OnlineGame.GoToGameWindow(Placement, Arrangment, Owner);
            Close();
        }
    }
}
