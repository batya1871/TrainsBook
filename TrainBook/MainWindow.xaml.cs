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
        public BindingList<Route> AllRoutesList;
        private BindingList<Route> AllActiveRoutesList = new BindingList<Route>();
        public BindingList<Train> AllTrainsList;
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
            double test = Window.HeightProperty.GetHashCode();
            fileIOService = new FileIOService(routePath, trainPath);
            try
            {
                AllRoutesList = fileIOService.LoadAllRoots();
                AllTrainsList = fileIOService.LoadAllTrains();
                if (Train.MaxSpeed == 0) Train.MaxSpeed = 70;
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

            if (AllTrainsList != null)
            {
                foreach (Train train in AllTrainsList)
                {
                    train.isInRoute(AllRoutesList);
                }
            }
            else
            {
                AllTrainsList = new BindingList<Train>();
            }

            ActualRoutesTable.ItemsSource = AllActiveRoutesList;

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
        

        private void AddRoute_Click(object sender, RoutedEventArgs e)
        {
            NewStandarsTrainPanel.Visibility = Visibility.Hidden;
            writeOnTrainPanel.Visibility = Visibility.Hidden;
            writeOffTrainPanel.Visibility = Visibility.Hidden;
            DelTrainPanel.Visibility = Visibility.Hidden;
            NewTrainPanel.Visibility = Visibility.Hidden;
            NewRoute_Warning.Visibility = Visibility.Hidden;
            NewRoutPanel.Visibility = Visibility.Visible;
            ActualRouteslbl.Visibility = Visibility.Hidden;
            ActualRoutesTable.Visibility = Visibility.Hidden;
            SortRoutPanel.Visibility = Visibility.Hidden;
            DeleteRoutPanel.Visibility = Visibility.Hidden;
            NotificationTb.Visibility = Visibility.Hidden;
        }
        //Добавление нового маршрута при нажатии на кнопку
        private void AddRouteBtn_click(object sender, RoutedEventArgs e)
        {
            NotificationTb.Visibility = Visibility.Hidden;
            NewRoute_Warning.Visibility = Visibility.Hidden;
            if ((DepPointTxb.Text != "") && (ArrPointTxb.Text != "") && (DepDateTxb.Text != "") && (ArrDateTxb.Text != "") && (TrainsIdTxb.Text != ""))
            {
                bool isNotInList = true;
                string DepPoint = DepPointTxb.Text;
                string ArrPoint = ArrPointTxb.Text;
                DateTime DepDate = StringToDate(DepDateTxb.Text);
                DateTime ArrDate = StringToDate(ArrDateTxb.Text);
                int TrainId = int.Parse(TrainsIdTxb.Text);

                Route newRoute = new Route(DepPoint, ArrPoint, DepDate, ArrDate, TrainId);
                foreach (Route Route in AllRoutesList)
                {
                    if ((Route.ArrivalDate == ArrDate) && (Route.ArrivalPoint == ArrPoint) && (Route.DepartureDate == DepDate) && (Route.DeparturePoint == DepPoint))
                    {
                        isNotInList = false;
                        NotificationTb.Text = "Маршрут не может быть добавлен т.к. уже находится в списке";
                        break;
                    }
                }

                if (isNotInList)
                {
                    NotificationTb.Text = "Маршрут успешно добавлен!";
                    AllRoutesList.Add(newRoute);
                    if (newRoute.IsActiveNow == true)
                    {
                        AllActiveRoutesList.Add(newRoute);
                    }
                }
                NotificationTb.Visibility = Visibility.Visible;
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
            NewStandarsTrainPanel.Visibility = Visibility.Hidden;
            writeOnTrainPanel.Visibility = Visibility.Hidden;
            writeOffTrainPanel.Visibility = Visibility.Hidden;
            DelTrainPanel.Visibility = Visibility.Hidden;
            NewRoutPanel.Visibility = Visibility.Hidden;
            NewTrainPanel.Visibility = Visibility.Hidden;
            ActualRouteslbl.Visibility = Visibility.Hidden;
            ActualRoutesTable.Visibility = Visibility.Hidden;
            SortRoutPanel.Visibility = Visibility.Visible;
            DeleteRoutPanel.Visibility = Visibility.Hidden;
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
                ActualRoutesTable.ItemsSource = AllActiveRoutesList;
                return;
            }
            ActualRoutesTable.ItemsSource = SortedRoutes;
            
        }
        #endregion

        #region Delete Route
        private void DeleteRoute_Click(object sender, RoutedEventArgs e)
        {
            NewStandarsTrainPanel.Visibility = Visibility.Hidden;
            writeOnTrainPanel.Visibility = Visibility.Hidden;
            writeOffTrainPanel.Visibility = Visibility.Hidden;
            DelTrainPanel.Visibility = Visibility.Hidden;
            NewTrainPanel.Visibility = Visibility.Hidden;
            NewRoutPanel.Visibility = Visibility.Hidden;
            ActualRouteslbl.Visibility = Visibility.Hidden;
            ActualRoutesTable.Visibility = Visibility.Hidden;
            SortRoutPanel.Visibility = Visibility.Hidden;
            DeleteRoutPanel.Visibility = Visibility.Visible;
            DelNotification.Visibility = Visibility.Hidden;

        }
        
        private void DeleteRouteBtn_click(object sender, RoutedEventArgs e)
        {
            DelNotification.Visibility = Visibility.Hidden;
            DelRoute_Warning.Visibility = Visibility.Hidden;
            if ((DelDepPointTxb.Text != "") && (DelArrPointTxb.Text != "") && (DelDepDateTxb.Text != "") && (DelArrDateTxb.Text != "") && (DelTrainsIdTxb.Text != ""))
            {
                bool isFounded = false;
                string DepPoint = DelDepPointTxb.Text;
                string ArrPoint = DelArrPointTxb.Text;
                DateTime DepDate = StringToDate(DelDepDateTxb.Text);
                DateTime ArrDate = StringToDate(DelArrDateTxb.Text);
                int TrainId = int.Parse(DelTrainsIdTxb.Text);


                foreach (Route DelRoute in AllRoutesList)
                {
                    if ((DelRoute.ArrivalDate == ArrDate) && (DelRoute.ArrivalPoint == ArrPoint) && (DelRoute.DepartureDate == DepDate) && (DelRoute.DeparturePoint == DepPoint))
                    {
                        isFounded = true;
                        AllRoutesList.Remove(DelRoute);
                        DelNotification.Text = "Маршрут успешно удален!";
                        if ((DelRoute.IsActiveNow == true))
                        {
                            AllActiveRoutesList.Remove(DelRoute);
                        }
                        break;
                    }
                }
                if (!isFounded) DelNotification.Text = "Маршрут отсутствует в системе!";
                
                ClearAllTxb();
            }
            else
            {
                DelRoute_Warning.Visibility = Visibility.Visible;
            }
            DelNotification.Visibility = Visibility.Visible;
        }
        #endregion

        #region Add Train
        private void AddTrain_Click(object sender, RoutedEventArgs e)
        {
            NewStandarsTrainPanel.Visibility = Visibility.Hidden;
            writeOnTrainPanel.Visibility = Visibility.Hidden;
            writeOffTrainPanel.Visibility = Visibility.Hidden;
            DelTrainPanel.Visibility = Visibility.Hidden;
            NewTrainPanel.Visibility = Visibility.Visible;
            NewRoutPanel.Visibility = Visibility.Hidden;
            ActualRouteslbl.Visibility = Visibility.Hidden;
            ActualRoutesTable.Visibility = Visibility.Hidden;
            SortRoutPanel.Visibility = Visibility.Hidden;
            DeleteRoutPanel.Visibility = Visibility.Hidden;
            TrainNotificationTb.Visibility = Visibility.Hidden;
        }

        private void Train_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!(Char.IsDigit(e.Text, 0) || (e.Text == ",")))
            {
                e.Handled = true;
            }
        }
        private void AddTrainBtn_click(object sender, RoutedEventArgs e)
        {
            TrainNotificationTb.Visibility = Visibility.Hidden;
            NewTrain_Warning.Visibility = Visibility.Hidden;
            if ((NewTrainIdTxb.Text != "") && (NewTrainSpeedTxb.Text != ""))
            {
                bool isNotInList = true;
                double TrainsSpeed = double.Parse(NewTrainSpeedTxb.Text);
                int TrainId = int.Parse(NewTrainIdTxb.Text);

                Train newTrain = new Train(TrainId, TrainsSpeed);
                newTrain.isInRoute(AllRoutesList);
                foreach (Train train in AllTrainsList)
                {
                    if (train.Id == TrainId)
                    {
                        isNotInList = false;
                        TrainNotificationTb.Text = "Поезд с данным id уже находится в списке";
                        break;
                    }
                }

                if (isNotInList)
                {
                    TrainNotificationTb.Text = "Поезд успешно добавлен в список!";
                    AllTrainsList.Add(newTrain);
                }
                TrainNotificationTb.Visibility = Visibility.Visible;
                ClearAllTxb();
            }
            else
            {
                NewTrain_Warning.Visibility = Visibility.Visible;
            }

        }
        #endregion

        #region Delete Train

        private void DeleteTrain_Click(object sender, RoutedEventArgs e)
        {
            NewStandarsTrainPanel.Visibility = Visibility.Hidden;
            writeOnTrainPanel.Visibility = Visibility.Hidden;
            writeOffTrainPanel.Visibility = Visibility.Hidden;
            DelTrainNotificationTb.Visibility = Visibility.Hidden;
            DelTrainPanel.Visibility = Visibility.Visible;
            NewTrainPanel.Visibility = Visibility.Hidden;
            NewRoutPanel.Visibility = Visibility.Hidden;
            ActualRouteslbl.Visibility = Visibility.Hidden;
            ActualRoutesTable.Visibility = Visibility.Hidden;
            SortRoutPanel.Visibility = Visibility.Hidden;
            DeleteRoutPanel.Visibility = Visibility.Hidden;
        }

        private void DelTrainBtn_click(object sender, RoutedEventArgs e)
        {
            DelTrainNotificationTb.Visibility = Visibility.Hidden;
            DelTrain_Warning.Visibility = Visibility.Hidden;
            if (DelTrainIdTxb.Text != "")
            {
                bool isFounded = false;
                int TrainId = int.Parse(DelTrainIdTxb.Text);
                foreach (Train DelTrain in AllTrainsList)
                {
                    if ((DelTrain.Id == TrainId))
                    {
                        isFounded = true;
                        AllTrainsList.Remove(DelTrain);
                        DelNotification.Text = "Поезд успешно удален!";
                        break;
                    }
                }
                if (!isFounded)
                {
                    DelTrainNotificationTb.Text = "Поезд с данным id отсутствует в системе!";
                }
                ClearAllTxb();
                DelTrainNotificationTb.Visibility = Visibility.Visible;
            }
            else
            {
                DelTrain_Warning.Visibility = Visibility.Visible;
            }
            
        }
        #endregion

        #region Write Off Train

        private void WOTrain_Click(object sender, RoutedEventArgs e)
        {
            NewStandarsTrainPanel.Visibility = Visibility.Hidden;
            writeOnTrainPanel.Visibility = Visibility.Hidden;
            writeOffTrainPanel.Visibility = Visibility.Visible;
            WOTrainNotificationTb.Visibility = Visibility.Hidden;
            DelTrainPanel.Visibility = Visibility.Hidden;
            NewTrainPanel.Visibility = Visibility.Hidden;
            NewRoutPanel.Visibility = Visibility.Hidden;
            ActualRouteslbl.Visibility = Visibility.Hidden;
            ActualRoutesTable.Visibility = Visibility.Hidden;
            SortRoutPanel.Visibility = Visibility.Hidden;
            DeleteRoutPanel.Visibility = Visibility.Hidden;
        }

        private void WOTrainBtn_click(object sender, RoutedEventArgs e)
        {
            WOTrainNotificationTb.Visibility = Visibility.Hidden;
            WOTrain_Warning.Visibility = Visibility.Hidden;
            if (WOTrainIdTxb.Text != "")
            {
                bool isFounded = false;
                int TrainId = int.Parse(WOTrainIdTxb.Text);
                foreach (Train WOTrain in AllTrainsList)
                {
                    if ((WOTrain.Id == TrainId))
                    {
                        isFounded = true;
                        if (WOTrain.InRoute)
                        {
                            WOTrainNotificationTb.Text = "Поезд не может быть списан т.к. находится в рейсе!";
                        }
                        else
                        {
                            WOTrain.TechnicalCondition = false;
                            WOTrainNotificationTb.Text = "Поезд списан!";
                        }
                        break;
                    }
                }
                if (!isFounded)
                {
                    WOTrainNotificationTb.Text = "Поезд с данным id отсутствует в системе!";
                }
                ClearAllTxb();
                WOTrainNotificationTb.Visibility = Visibility.Visible;
            }
            else
            {
                WOTrain_Warning.Visibility = Visibility.Visible;
            }

        }
        #endregion

        #region Write On Train

        private void writeOnTrain_Click(object sender, RoutedEventArgs e)
        {
            NewStandarsTrainPanel.Visibility = Visibility.Hidden;
            writeOnTrainPanel.Visibility = Visibility.Visible;
            writeOffTrainPanel.Visibility = Visibility.Hidden;
            WOnTrainNotificationTb.Visibility = Visibility.Hidden;
            DelTrainPanel.Visibility = Visibility.Hidden;
            NewTrainPanel.Visibility = Visibility.Hidden;
            NewRoutPanel.Visibility = Visibility.Hidden;
            ActualRouteslbl.Visibility = Visibility.Hidden;
            ActualRoutesTable.Visibility = Visibility.Hidden;
            SortRoutPanel.Visibility = Visibility.Hidden;
            DeleteRoutPanel.Visibility = Visibility.Hidden;
        }

        private void WOnTrainBtn_click(object sender, RoutedEventArgs e)
        {
            WOnTrainNotificationTb.Visibility = Visibility.Hidden;
            WOnTrain_Warning.Visibility = Visibility.Hidden;
            if (WOnTrainIdTxb.Text != "")
            {
                bool isFounded = false;
                int TrainId = int.Parse(WOnTrainIdTxb.Text);
                foreach (Train WOnTrain in AllTrainsList)
                {
                    if ((WOnTrain.Id == TrainId))
                    {
                        isFounded = true;
                        WOnTrain.TechnicalCondition = true;
                        WOnTrainNotificationTb.Text = "Поезд восстановлен!";
                        break;
                    }
                }
                if (!isFounded)
                {
                    WOnTrainNotificationTb.Text = "Поезд с данным id отсутствует в системе!";
                }
                ClearAllTxb();
                WOnTrainNotificationTb.Visibility = Visibility.Visible;
            }
            else
            {
                WOnTrain_Warning.Visibility = Visibility.Visible;
            }

        }
        #endregion

        #region New standarts for trains

        private void NSTrain_Click(object sender, RoutedEventArgs e)
        {
            NewStandarsTrainPanel.Visibility = Visibility.Visible;
            writeOnTrainPanel.Visibility = Visibility.Hidden;
            writeOffTrainPanel.Visibility = Visibility.Hidden;
            NSTrainNotificationTb.Visibility = Visibility.Hidden;
            DelTrainPanel.Visibility = Visibility.Hidden;
            NewTrainPanel.Visibility = Visibility.Hidden;
            NewRoutPanel.Visibility = Visibility.Hidden;
            ActualRouteslbl.Visibility = Visibility.Hidden;
            ActualRoutesTable.Visibility = Visibility.Hidden;
            SortRoutPanel.Visibility = Visibility.Hidden;
            DeleteRoutPanel.Visibility = Visibility.Hidden;
        }

        private void NSTrainBtn_click(object sender, RoutedEventArgs e)
        {
            NSTrainNotificationTb.Visibility = Visibility.Hidden;
            NSTrain_Warning.Visibility = Visibility.Hidden;
            if (NSTrainSpeedTxb.Text != "")
            {
                Train.MaxSpeed = double.Parse(NSTrainSpeedTxb.Text);
                ClearAllTxb();
                NSTrainNotificationTb.Visibility = Visibility.Visible;
            }
            else
            {
                NSTrain_Warning.Visibility = Visibility.Visible;
            }

        }
        #endregion

        //Очистка всех текстбоксов главного окна
        private void ClearAllTxb()
        {
            WOnTrainIdTxb.Clear();
            WOTrainIdTxb.Clear();
            DelTrainIdTxb.Clear();
            NewTrainSpeedTxb.Clear();
            NewTrainIdTxb.Clear();
            SortDepDateTxb.Clear();
            SortArrDateTxb.Clear();
            SortDepPointTxb.Clear();
            SortArrPointTxb.Clear();
            DepPointTxb.Clear();
            ArrPointTxb.Clear();
            DepDateTxb.Clear();
            ArrDateTxb.Clear();
            TrainsIdTxb.Clear();
            DelDepPointTxb.Clear();
            DelArrPointTxb.Clear();
            DelDepDateTxb.Clear();
            DelArrDateTxb.Clear();
            DelTrainsIdTxb.Clear();

        }
        private void backToTableBtn_Click(object sender, RoutedEventArgs e)
        {
            NewStandarsTrainPanel.Visibility = Visibility.Hidden;
            writeOnTrainPanel.Visibility = Visibility.Hidden;
            writeOffTrainPanel.Visibility = Visibility.Hidden;
            DelTrainPanel.Visibility = Visibility.Hidden;
            NewTrainPanel.Visibility = Visibility.Hidden;
            ActualRouteslbl.Visibility = Visibility.Visible;
            ActualRoutesTable.Visibility = Visibility.Visible;
            SortRoutPanel.Visibility = Visibility.Hidden;
            NewRoutPanel.Visibility = Visibility.Hidden;
            DeleteRoutPanel.Visibility = Visibility.Hidden;
        }


        #endregion

        #endregion

    }
}
