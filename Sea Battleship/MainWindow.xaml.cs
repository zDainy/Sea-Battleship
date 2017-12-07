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
            LogService.Start();
            LogService.Trace("==============================================");
            LogService.Trace("Запуск приложения");
            InitializeComponent();
            LogService.Trace("==============================================");
            LogService.Close();
        }
    }
}
