using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TrainBook
{
    /// <summary>
    /// Логика взаимодействия для TrainTable.xaml
    /// </summary>
    public partial class TrainTable : Window
    {
        private BindingList<Train> TrainsList;
        public TrainTable()
        {
            InitializeComponent();

        }

        private void Table_Loaded(object sender, RoutedEventArgs e)
        {

            TrainsTable.ItemsSource = Container.TrainsList;
            TrainsList = Container.TrainsList;

        }

        #region basic actions with the window

        private void Mouse_Drag_Window(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
        private void Header_Mouse_Enter(object sender, RoutedEventArgs e)
        {
            ((Label)sender).Foreground = Brushes.White;
        }
        private void Header_Mouse_Leave(object sender, RoutedEventArgs e)
        {
            ((Label)sender).Foreground = Brushes.Gray;
        }
        private void Resize_Wondow(object sender, RoutedEventArgs e)
        {

            if (WindowState == WindowState.Maximized)
                this.WindowState = WindowState.Normal;
            else
                this.WindowState = WindowState.Maximized;
        }
        private void Roll_Up(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void Mouse_Enter(object sender, RoutedEventArgs e)
        {
            ((Label)sender).Background = new SolidColorBrush(Colors.Gray) { Opacity = 0.2 };
        }
        private void Mouse_Leave(object sender, RoutedEventArgs e)
        {
            ((Label)sender).Background = new SolidColorBrush(Colors.Gray) { Opacity = 0 };
        }
     
        private void Mouse_Enter_Close(object sender, RoutedEventArgs e)
        {
            ((Label)sender).Background = Brushes.Red;
        }
        private void Mouse_Leave_Close(object sender, RoutedEventArgs e)
        {
            lbl_Close.Background = new SolidColorBrush(Colors.Gray) { Opacity = 0 };
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Container.WindowData[0] = true;
            this.Close();
        }
        private void BackToMainWindow(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }



        private void ButtonUp(object sender, MouseButtonEventArgs e)
        {
            ((Label)sender).Background = (Brush)Application.Current.MainWindow.FindResource("ButtonBrush2");

        }


        #endregion

        private void SortBtn_Click(object sender, RoutedEventArgs e)
        {
            BindingList<Train> SortedTrains = new BindingList<Train>();
            int id = -1;
            if (IdTxb.Text != "") id = int.Parse(IdTxb.Text);
            bool isInRoute = (bool)inRouteChB.IsChecked;
            bool isDontWork = (bool)DontWorkChB.IsChecked;
            bool isBroken = (bool)BrokenChB.IsChecked;
            bool byThePlea = (bool)byThePleaChB.IsChecked;
            if (id == -1)
            {
                if(isDontWork)
                {
                    foreach (Train train in TrainsList)
                    {
                        if ( (!train.InRoute) && (train.TechnicalCondition))
                            SortedTrains.Add(train);
                    }
                }
                else if (isInRoute)
                {
                    foreach (Train train in TrainsList)
                    {
                        if  (train.InRoute)
                            SortedTrains.Add(train);
                    }
                }
                else if (isBroken)
                {
                    foreach (Train train in TrainsList)
                    {
                        if  (!train.TechnicalCondition)
                            SortedTrains.Add(train);
                    }
                }
            }
            else
            {
                if (byThePlea)
                {
                    foreach (Train train in TrainsList)
                    {
                        if (train.Id == id)
                            SortedTrains.Add(train);
                    }
                }
                else if (isDontWork)
                {
                    foreach (Train train in TrainsList)
                    {
                        if ((train.Id == id) && (!train.InRoute) && (train.TechnicalCondition))
                            SortedTrains.Add(train);
                    }
                }
                else if (isInRoute)
                {
                    foreach (Train train in TrainsList)
                    {
                        if ((train.Id == id) && (train.InRoute))
                            SortedTrains.Add(train);
                    }
                }
                else if (isBroken)
                {
                    foreach (Train train in TrainsList)
                    {
                        if ((train.Id == id) && (!train.TechnicalCondition))
                            SortedTrains.Add(train);
                    }
                }
            }

            notification.Text = "Сортировка завершена";
            TrainsTable.ItemsSource = SortedTrains;
            notification.Visibility = Visibility.Visible;

        }

        private void ChB_Checked(object sender, RoutedEventArgs e)
        {
            if (inRouteChB == null) return;
            if ((((CheckBox)sender).Name == "DontWorkChB") &&(DontWorkChB.IsChecked == true))
            {
                inRouteChB.IsChecked = false;
                BrokenChB.IsChecked = false;
                byThePleaChB.IsChecked = false;
            }
            else if ((((CheckBox)sender).Name == "inRouteChB") && (inRouteChB.IsChecked == true))
            {
                DontWorkChB.IsChecked = false;
                BrokenChB.IsChecked = false;
                byThePleaChB.IsChecked = false;
            }
            else if ((((CheckBox)sender).Name == "BrokenChB") && (BrokenChB.IsChecked == true))
            {
                inRouteChB.IsChecked = false;
                DontWorkChB.IsChecked = false;
                byThePleaChB.IsChecked = false;
            }
            else if ((((CheckBox)sender).Name == "byThePleaChB") && (byThePleaChB.IsChecked == true))
            {
                inRouteChB.IsChecked = false;
                DontWorkChB.IsChecked = false;
                BrokenChB.IsChecked = false;
            }
            else
            {
                if ((((CheckBox)sender).Name == "DontWorkChB") )
                {
                    BrokenChB.IsChecked = true;
                }
                else DontWorkChB.IsChecked = true;
            }
        }

        private void AllList_Click(object sender, RoutedEventArgs e)
        {
            TrainsTable.ItemsSource = TrainsList;
            notification.Text = "Список восстановлен";
            notification.Visibility = Visibility.Visible;
        }

        private void BackToTableBtn_Click(object sender, RoutedEventArgs e)
        {
            notification.Visibility = Visibility.Hidden;
            TrainsLbl.Visibility = Visibility.Visible;
            TrainsTable.Visibility = Visibility.Visible;
            SortPanel.Visibility = Visibility.Hidden;
        }
        private void Open_Sort_Click(object sender, RoutedEventArgs e)
        {
            TrainsLbl.Visibility = Visibility.Hidden;
            TrainsTable.Visibility = Visibility.Hidden;
            SortTrainPanel.Visibility = Visibility.Visible;
        }

        private void Train_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!(Char.IsDigit(e.Text, 0)))
            {
                e.Handled = true;
            }
        }

        private void NotTable_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Container.WindowData[2] = true;
            this.Close();
        }

        private void RouteTable_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Container.WindowData[3] = true;
            this.Close();
        }
    }
}
