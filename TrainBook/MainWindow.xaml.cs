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
        private BindingList<Route> AllActiveRoutesList;
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

            ActualRoutes.ItemsSource = AllActiveRoutesList;

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

        //Активация окна для сортировки актуальных маршрутов
        private void Sort_actual_routes_Click(object sender, RoutedEventArgs e)
        {
            if (ActualRoutes.Visibility == Visibility.Visible)
            {
                ActualRoutes.Visibility = Visibility.Hidden;
                ActualRoutesLbl.Visibility = Visibility.Hidden;
            }
            else
            {
                ActualRoutes.Visibility = Visibility.Visible;
                ActualRoutesLbl.Visibility = Visibility.Visible;
            }

        }

        //Запрет на ввод лишних символов в поля с датой и временем
        private void DataPick_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!(Char.IsDigit(e.Text, 0) || (e.Text == ".") || (e.Text == "/") || (e.Text == ":")))
            {
                e.Handled = true;
            }
        }
        #endregion

        #endregion


    }
}
