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
    /// Логика взаимодействия для PlacingWindow.xaml
    /// </summary>
    public partial class PlacingWindow : Window
    {
        public PlacingWindow()
        {
            InitializeComponent();
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    Image img = new Image
                    {
                        Stretch = Stretch.Fill,
                        Source = new BitmapImage(new Uri("/Resources/Water.jpg", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache },
                        Opacity = 0
                    };
                    img.MouseLeftButtonUp += FieldCell_Click;
                    gr.Children.Add(img);
                    Grid.SetColumn(img, x);
                    Grid.SetRow(img, y);
                }
            }
        }

        private void FieldCell_Click(object sender, MouseButtonEventArgs e)
        {
            Grid parent = (Grid)st.Parent;
            parent.Children.Remove(st);
            st.Margin = new Thickness();
            gr.Children.Add(st);
            Grid.SetRow(st, 1);
            Grid.SetColumn(st, 1);
            Grid.SetColumnSpan(st, 2);
        }

        private void audioChanged(object sender, RoutedEventArgs e)
        {
            WindowConfig.AudioChanged((Image)sender);
        }

        // private void Mouse_Down(object sender, MouseButtonEventArgs e) => WindowConfig.ShipState = WindowConfig.State.Ship4;
        // private void Mouse_Down3(object sender, MouseButtonEventArgs e) => WindowConfig.ShipState = WindowConfig.State.Ship3;
        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (Yes)
            {
                st.Margin = new Thickness(e.GetPosition(null).X, e.GetPosition(null).Y, 0, 0);
            }
        }

        private void gr_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            if (Yes)
            {
                Yes = !Yes;
                Grid.SetZIndex(st, 1);
                Grid parent = (Grid)st.Parent;
                parent.Children.Remove(st);
                st.Margin = new Thickness();
                gr.Children.Add(st);
                Grid.SetRow(st, 1);
                Grid.SetColumn(st, 1);
                Grid.SetColumnSpan(st, 2);
            }

        }


        private void st_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //CaptureMouse();
            //st.ReleaseMouseCapture();
            //gr.CaptureMouse();
            // if (gr.IsMouseOver)
            // if (st.IsMouseOver)
            //    Yes = !Yes;
            Yes = !Yes;
        }

        bool Yes = false;

        private void st_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Yes = !Yes;
            if (st.Parent == gr)
            {
                Grid.SetZIndex(st, 0);
                gr.Children.Remove(st);
                MainGrid.Children.Add(st);
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //StackPanel st = new StackPanel
            //{
            //    Background = new SolidColorBrush(Color.FromRgb(0, 0, 0))
            //};
            Grid parent = (Grid)st.Parent;
            parent.Children.Remove(st);
            st.Margin = new Thickness();
            //Image img = new Image
            //{
            //    Stretch = Stretch.Fill,
            //    Source = new BitmapImage(new Uri("/Resources/Water.jpg", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache },
            //    //Opacity = 0
            //};
            // st.Children.Add(img);

            StackPanel stack = new StackPanel
            {
                Background = new SolidColorBrush(Color.FromRgb(0, 0, 0))
            };
            gr.Children.Add(stack);
            Grid.SetRow(stack, 1);
            Grid.SetColumn(stack, 1);
            Grid.SetColumnSpan(stack, 2);
            gr.Children.Add(st);
            Grid.SetRow(st, 1);
            Grid.SetColumn(st, 1);
            Grid.SetColumnSpan(st, 2);
        }


    }
}
