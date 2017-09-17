﻿using System.Windows;
using Common;
using Network;

namespace Sea_Battleship
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Временно
            LogService.InitInitialize();
            Connection c = new Connection();
            c.CreateLobby();
            LogService.Close();
        }
    }
}
