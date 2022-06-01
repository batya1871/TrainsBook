using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace TrainBook
{
    /// <summary>
    /// Логика взаимодействия для TrainTable.xaml
    /// </summary>
    public partial class RouteTable : Window
    {
        private BindingList<Route> RoutesList;
        public RouteTable()
        {
            InitializeComponent();

        }

        private void Table_Loaded(object sender, RoutedEventArgs e)
        {

            RoutesTable.ItemsSource = Container.RoutesList;
            RoutesList = Container.RoutesList;

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



        private void ButtonUp(object sender, MouseButtonEventArgs e)
        {
            ((Label)sender).Background = (Brush)Application.Current.MainWindow.FindResource("ButtonBrush2");

        }


        #endregion

        private DateTime StringToDate(string dateStr)
        {
            dateStr = System.Text.RegularExpressions.Regex.Replace(dateStr, @"\s+", " ");
            if (dateStr[0] == ' ') dateStr = dateStr.Remove(0, 1);
            if (dateStr[dateStr.Length - 1] == ' ') dateStr = dateStr.Remove(dateStr.Length - 1, 1);
            string tmp = "";
            int iter = 0;
            int step = 0;
            int year = 0;
            int month = 0;
            int day = 0;
            int hour = 0;
            int minute = 0;
            try
            {
                while (step != 4)
                {
                    switch (step)
                    {
                        case 0:
                            {
                                if (iter > 2) throw new Exception();
                                if (dateStr[iter] != '.')
                                {
                                    tmp += dateStr[iter];
                                }
                                else
                                {
                                    day = int.Parse(tmp);
                                    if (day > 31) day = 1;
                                    tmp = "";
                                    step = 1;
                                }
                            }
                            break;
                        case 1:
                            {
                                if (iter > 7) throw new Exception();
                                if (dateStr[iter] != '.')
                                {
                                    tmp += dateStr[iter];
                                }
                                else
                                {
                                    month = int.Parse(tmp);
                                    if (month > 12) month = 12;
                                    tmp = "";
                                    step = 2;
                                }
                            }
                            break;
                        case 2:
                            {
                                if (iter > 14) throw new Exception();
                                if (dateStr[iter] != ' ')
                                {
                                    tmp += dateStr[iter];
                                }
                                else
                                {
                                    if (tmp.Length > 4) year = 2022;
                                    else year = int.Parse(tmp);
                                    tmp = "";
                                    step = 3;

                                }
                            }
                            break;
                        case 3:
                            {
                                if (iter > 16) throw new Exception();
                                if (dateStr[iter] != ':')
                                {
                                    tmp += dateStr[iter];
                                }
                                else
                                {
                                    hour = int.Parse(tmp);
                                    if (hour > 24) day = 1;
                                    tmp = "";
                                    step = 4;
                                }
                            }
                            break;
                        default:
                            break;
                    }
                    iter++;
                }
                minute = int.Parse(("" + dateStr[dateStr.Length - 2] + dateStr[dateStr.Length - 1]));
            }
            catch (Exception)
            {

                return new DateTime();
            }

            return new DateTime(year, month, day, hour, minute, 0);
        }



        private void Sort_actual_routes_Click(object sender, RoutedEventArgs e)
        {
            
            SortRoutPanel.Visibility = Visibility.Visible;
            Routeslbl.Visibility = Visibility.Hidden;
            RoutesTable.Visibility = Visibility.Hidden;
        }

        private void isPointSortChB_Click(object sender, RoutedEventArgs e)
        {
            if (isPointSortChB.IsChecked == true)
            {
                PointSortPanel.Visibility = Visibility.Visible;
            }
            else
            {
                SortDepPointTxb.Clear();
                SortArrPointTxb.Clear();
                PointSortPanel.Visibility = Visibility.Hidden;
            }
        }

        private void isDateSortChB_Click(object sender, RoutedEventArgs e)
        {
            if (isDateSortChB.IsChecked == true)
            {
                DateSortPanel.Visibility = Visibility.Visible;
            }
            else
            {
                SortArrDateTxb.Clear();
                SortDepDateTxb.Clear();
                DateSortPanel.Visibility = Visibility.Hidden;
            }
        }

        private void SortRouteBtn_Click(object sender, RoutedEventArgs e)
        {
            bool itFits = false;
            BindingList<Route> SortedRoutes = new BindingList<Route>();
            string DepPoint = "";
            string ArrPoint = "";
            DateTime DepDate = DateTime.MinValue;
            DateTime ArrDate = DateTime.MinValue;

            if (SortDepPointTxb.Text != "") DepPoint = SortDepPointTxb.Text;
            if (SortArrPointTxb.Text != "") ArrPoint = SortArrPointTxb.Text;
            if (SortDepDateTxb.Text != "") DepDate = StringToDate(SortDepDateTxb.Text);
            if (SortArrDateTxb.Text != "") ArrDate = StringToDate(SortArrDateTxb.Text);

            if ((SortDepPointTxb.Text == "") && (SortArrPointTxb.Text == ""))
            {
                isPointSortChB.IsChecked = false;
                SortDepPointTxb.Clear();
                SortArrPointTxb.Clear();
                PointSortPanel.Visibility = Visibility.Hidden;
            }
            if ((SortDepDateTxb.Text == "") && (SortArrDateTxb.Text == ""))
            {
                isDateSortChB.IsChecked = false;
                SortArrDateTxb.Clear();
                SortDepDateTxb.Clear();
                DateSortPanel.Visibility = Visibility.Hidden;
            }

            if ((isPointSortChB.IsChecked == true) && (isDateSortChB.IsChecked == true))
            {
                foreach (Route route in RoutesList)
                {
                    itFits = ((route.ArrivalPoint == ArrPoint) && (route.DeparturePoint == DepPoint)
                        && (DepDate <= route.DepartureDate) && (route.ArrivalDate <= ArrDate)) ||

                        ((ArrPoint == "") && (route.DeparturePoint == DepPoint)
                        && (DepDate <= route.DepartureDate) && (route.ArrivalDate <= ArrDate)) ||
                        ((route.ArrivalPoint == ArrPoint) && (DepPoint == "")
                        && (DepDate <= route.DepartureDate) && (route.ArrivalDate <= ArrDate)) ||
                        ((route.ArrivalPoint == ArrPoint) && (route.DeparturePoint == DepPoint)
                        && (DepDate == DateTime.MinValue) && (route.ArrivalDate <= ArrDate)) ||
                        ((route.ArrivalPoint == ArrPoint) && (route.DeparturePoint == DepPoint)
                        && (DepDate <= route.DepartureDate) && (ArrDate == DateTime.MinValue)) ||

                        ((ArrPoint == "") && (route.DeparturePoint == DepPoint)
                        && (DepDate == DateTime.MinValue) && (route.ArrivalDate <= ArrDate)) ||
                        ((ArrPoint == "") && (route.DeparturePoint == DepPoint)
                        && (DepDate <= route.DepartureDate) && (ArrDate == DateTime.MinValue)) ||

                        ((ArrPoint == route.ArrivalPoint) && (DepPoint == "")
                        && (DepDate == DateTime.MinValue) && (route.ArrivalDate <= ArrDate)) ||
                        ((ArrPoint == route.ArrivalPoint) && (DepPoint == "")
                        && (DepDate <= route.DepartureDate) && (ArrDate == DateTime.MinValue));


                    if (itFits)
                    {
                        SortedRoutes.Add(route);
                    }
                }
            }
            else if (isPointSortChB.IsChecked == true)
            {
                foreach (Route route in RoutesList)
                {
                    itFits = ((route.ArrivalPoint == ArrPoint) && (route.DeparturePoint == DepPoint)) ||
                        ((DepPoint == "") && (ArrPoint == route.ArrivalPoint)) ||
                        ((ArrPoint == "") && (DepPoint == route.DeparturePoint));
                    if (itFits)
                    {
                        SortedRoutes.Add(route);
                    }
                }
            }
            else if (isDateSortChB.IsChecked == true)
            {

                foreach (Route route in RoutesList)
                {
                    itFits = ((DepDate <= route.DepartureDate) && (route.ArrivalDate <= ArrDate)) ||
                             ((DepDate == DateTime.MinValue) && (route.ArrivalDate <= ArrDate)) ||
                             ((ArrDate == DateTime.MinValue) && (DepDate <= route.DepartureDate));

                    if (itFits)
                    {
                        SortedRoutes.Add(route);
                    }
                }
            }
            else
            {
                RoutesTable.ItemsSource = RoutesList;
                return;
            }
            RoutesTable.ItemsSource = SortedRoutes;

        }

        private void DataPick_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!(Char.IsDigit(e.Text, 0) || (e.Text == ".") || (e.Text == ":")))
            {
                e.Handled = true;
            }
        }

        private void backToTableBtn_Click(object sender, RoutedEventArgs e)
        {
            Routeslbl.Visibility = Visibility.Visible;
            RoutesTable.Visibility = Visibility.Visible;
            SortRoutPanel.Visibility = Visibility.Hidden;
        }

        private void NotTabl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Container.WindowData[2] = true;
            this.Close();
        }

        private void TrainTabll_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Container.WindowData[1] = true;
            this.Close();
        }

        private void BackToMainWindow(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

    }
}
