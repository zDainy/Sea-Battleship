using Core;
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

namespace Sea_Battleship
{
    /// <summary>
    /// Логика взаимодействия для LoadArrangementWindow.xaml
    /// </summary>
    public partial class LoadArrangementWindow : Window
    {
        public LoadArrangementWindow()
        {
            InitializeComponent();
        }


        private void RetButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (WindowConfig.GameState == WindowConfig.State.Offline)
            {
                if (textBoxSave.Text != "")
                {
                    FileSystem.LoadArrangement(textBoxSave.Text);
                    Close();
                }
                else
                {
                    MessageBox.Show("Введите название расстановки");
                }
            }
        }
    }
}
