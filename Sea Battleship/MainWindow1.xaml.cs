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
using Common;
using Core;
using System.Threading;
using System.Windows.Threading;

namespace Sea_Battleship
{
    /// <summary>
    /// Логика взаимодействия для MainWindow1.xaml
    /// </summary>
    public partial class MainWindow1 : Window
    {
        public MainWindow1()
        {
            InitializeComponent();
            MainFrame.Content = new MainPage();
            WindowConfig.Player.Open(new Uri(Environment.CurrentDirectory + @"\pirat.wav"));
            WindowConfig.Player.Volume = 50;
              WindowConfig.Player.Play();
            LogService.Start();
            LogService.Trace("==============================================");
        }
       
    }
}
