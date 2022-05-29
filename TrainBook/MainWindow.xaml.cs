using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
namespace TrainBook
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string routePath = $"{Environment.CurrentDirectory}\\Route.json";
        private readonly string trainPath = $"{Environment.CurrentDirectory}\\Train.json";
        private BindingList<Route> AllRoutesList;
        private BindingList<Route> AllActiveRoutesList = new BindingList<Route>();
        private BindingList<Train> AllTrainsList;
        private FileIOService fileIOService;
        static private User activeUser = new User();
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Save and load Serialized objects
        void DeserializedUsersData()
        {
            // создаем объект BinaryFormatter
            BinaryFormatter formatter = new BinaryFormatter();
            // десериализация из файла с данными о пользователях
            using (FileStream fs = new FileStream("Data.dat", FileMode.Open))
            {
                try
                {
                    DataForSavingUsers data = (DataForSavingUsers)formatter.Deserialize(fs);
                    activeUser = data.MarkedUser;
                    if (activeUser != null) activeUser.Mark = true;
                    AllUsers.Users = data.Users;
                }
                catch (Exception)
                {
                    //Если ошибка, то ничего не происходит
                }

            }
            //Указываем имя пользователя и открываем правый сайдбар
            if (activeUser != null)
            {
                Login_label.Content = activeUser.Name;
             
            }
            else
            {
               
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            fileIOService = new FileIOService(routePath, trainPath);
            try
            {
                AllRoutesList = fileIOService.LoadAllRoots();
                AllTrainsList = fileIOService.LoadAllTrains();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных!");
                Close();
            }

            if (AllRoutesList != null)
            {
                for (int i = 0; i < AllRoutesList.Count; i++)
                {
                    AllRoutesList[i].CheckActivity();
                }
                for (int i = 0; i < AllRoutesList.Count; i++)
                {
                    if (AllRoutesList[i].IsActiveNow)
                    {
                        AllActiveRoutesList.Add(AllRoutesList[i]);
                    }
                }
            }
            else
            {
                AllRoutesList = new BindingList<Route>();
            }

            ActualRoutes.ItemsSource = AllActiveRoutesList;

        }
        #endregion

        #region internal service functions
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
                                if (dateStr[iter] != '/')
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
                                if (dateStr[iter] != '/')
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


        #endregion

        #region Interaction with window

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
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true; // Отмена закрытия окна 
        }
        private void Mouse_Enter(object sender, RoutedEventArgs e)
        {
            ((Label)sender).Background = new SolidColorBrush(Colors.Gray) { Opacity = 0.2 };
        }
        private void Mouse_Leave(object sender, RoutedEventArgs e)
        {
            ((Label)sender).Background = new SolidColorBrush(Colors.Gray) { Opacity = 0 };
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            // объект для сериализации
            DataForSavingUsers save = new DataForSavingUsers(activeUser, AllUsers.Users);
            // создаем объект BinaryFormatter
            BinaryFormatter formatter = new BinaryFormatter();
            // получаем поток, куда будем записывать сериализованный объект
            using (FileStream fs = new FileStream("Data.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, save);
            }
            fileIOService.SaveAllRoute(AllRoutesList);
            fileIOService.SaveAllTrains(AllTrainsList);
            Application.Current.Shutdown();
        }
        private void Mouse_Enter_Close(object sender, RoutedEventArgs e)
        {
            ((Label)sender).Background = Brushes.Red;
        }
        private void Mouse_Leave_Close(object sender, RoutedEventArgs e)
        {
            lbl_Close.Background = new SolidColorBrush(Colors.Gray) { Opacity = 0 };
        }

        //Открытие окна для входа в аккаунт\регистрации
        private void LoginWindow_Open(object sender, MouseButtonEventArgs e)
        {
            //Прячем основное окно
            this.Visibility = Visibility.Hidden;
            LoginWindow log = new LoginWindow();
            log.ShowDialog();
            //Вновь открываем его
            this.Visibility = Visibility.Visible;
            //Нахождение вбитого пользователя
            activeUser = AllUsers.FindMarkedUser();
            //При нахождении вписываем его ФИО в правый верхний угол и даем доступ к правому сайдбару
            if (activeUser != null)
            {
                Login_label.Content = activeUser.Name;
            }
            //Иначе возвращаемся к  значениям по умолчанию
            else
            {
                Login_label.Content = "Войти...";
            }
        }

        private void ButtonUp(object sender, MouseButtonEventArgs e)
        {
            ((Label)sender).Background = (Brush)Application.Current.MainWindow.FindResource("ButtonBrush2");

        }

        #endregion

        #region switching and working the workspace

        #region add Route
        //Запрет на ввод лишних символов в поля с датой и временем
        private void DataPick_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!(Char.IsDigit(e.Text, 0) || (e.Text == ".") || (e.Text == "/") || (e.Text == ":")))
            {
                e.Handled = true;
            }
        }
        private void backToTableBtn_Click(object sender, RoutedEventArgs e)
        {
            ActualRoutes.Visibility = Visibility.Visible;
            ActualRoutesLbl.Visibility = Visibility.Visible;
            SortRoutPanel.Visibility = Visibility.Hidden;
            NewRoutPanel.Visibility = Visibility.Hidden;
        }

        private void AddRoute_Click(object sender, RoutedEventArgs e)
        {
            NewRoute_Warning.Visibility = Visibility.Hidden;
            NewRoutPanel.Visibility = Visibility.Visible;
            ActualRoutesLbl.Visibility = Visibility.Hidden;
            ActualRoutes.Visibility = Visibility.Hidden;
            SortRoutPanel.Visibility = Visibility.Hidden;
        }
        //Добавление нового маршрута при нажатии на кнопку
        private void AddRouteBtn_click(object sender, RoutedEventArgs e)
        {
            NewRoute_Warning.Visibility = Visibility.Hidden;
            if ((DepPointTxb.Text != "") && (ArrPointTxb.Text != "") && (DepDateTxb.Text != "") && (ArrDateTxb.Text != "") && (TrainsIdTxb.Text != ""))
            {
                string DepPoint = DepPointTxb.Text;
                string ArrPoint = ArrPointTxb.Text;
                DateTime DepDate = StringToDate(DepDateTxb.Text);
                DateTime ArrDate = StringToDate(ArrDateTxb.Text);
                int TrainId = int.Parse(TrainsIdTxb.Text);

                Route newRoute = new Route(DepPoint, ArrPoint, DepDate, ArrDate, TrainId);

                if (!AllRoutesList.Contains(newRoute))
                {
                    AllRoutesList.Add(newRoute);
                }
                if ((newRoute.IsActiveNow == true) && (!AllActiveRoutesList.Contains(newRoute)))
                {
                    AllActiveRoutesList.Add(newRoute);
                }
                ClearAllTxb();
            }
            else
            {
                NewRoute_Warning.Visibility = Visibility.Visible;
            }

        }
        #endregion

        #region Sort Route
        //Активация окна для сортировки актуальных маршрутов
        
        private void Sort_actual_routes_Click(object sender, RoutedEventArgs e)
        {
            NewRoute_Warning.Visibility = Visibility.Hidden;
            NewRoutPanel.Visibility = Visibility.Hidden;
            ActualRoutesLbl.Visibility = Visibility.Hidden;
            ActualRoutes.Visibility = Visibility.Hidden;
            SortRoutPanel.Visibility = Visibility.Visible;
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

            if ((isPointSortChB.IsChecked == true)&& (isDateSortChB.IsChecked == true))
            {
                foreach (Route route in AllActiveRoutesList)
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
                foreach (Route route in AllActiveRoutesList)
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

                foreach (Route route in AllActiveRoutesList)
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
                ActualRoutes.ItemsSource = AllActiveRoutesList;
                return;
            }
            ActualRoutes.ItemsSource = SortedRoutes;
            
        }
        #endregion



        //Метод для переключения видимости двух элеменов. Первый аргумент становится видимым, а второй скрывается
        private void Swither(UIElement el1, UIElement el2)
        {
            el1.Visibility = Visibility.Visible;
            el2.Visibility = Visibility.Hidden;
        }
        //Очистка всех текстбоксов главного окна
        private void ClearAllTxb()
        {
            SortDepDateTxb.Clear();
            SortArrDateTxb.Clear();
            SortDepPointTxb.Clear();
            SortArrPointTxb.Clear();
            DepPointTxb.Clear();
            ArrPointTxb.Clear();
            DepDateTxb.Clear();
            ArrDateTxb.Clear();
            TrainsIdTxb.Clear();

        }







        #endregion

        #endregion

        
    }
}
