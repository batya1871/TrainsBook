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

namespace TrainBook
{

    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            passwordTxB.Password = "";
            PassWord1TxB.Password = "";
            PassWord2TxB.Password = "";
        }

        #region design

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
            AllUsers.RemoveMark();
            this.Close();
        }
        private void Mouse_Enter_Close(object sender, RoutedEventArgs e)
        {
            ((Label)sender).Background = Brushes.Red;
        }
        private void Mouse_Leave_Close(object sender, RoutedEventArgs e)
        {
            lbl_Close.Background = new SolidColorBrush(Colors.Gray) { Opacity = 0 };
        }


        #endregion
        #region ResizeAndDragMainWindow

        private void Roll_Up(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void Mouse_Drag_Window(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
        #endregion
        #region LogicForUIElements
        private void SwitchWindow(object sender, MouseButtonEventArgs e)
        {
            TB_message.Visibility = Visibility.Hidden;
            if (LoginZone.Visibility == Visibility.Hidden)
            {
                //Скрываем все данные окна регистрации
                RegistrationZone.Visibility = Visibility.Hidden;
                RegistrationTxB.Text = "";
                PassWord1TxB.Password = "";
                PassWord2TxB.Password = "";
                TB_Registration.Style = (Style)FindResource("HiddenTB");
                TB_checkingPS1.Visibility = Visibility.Hidden;
                TB_checkingPS2.Visibility = Visibility.Hidden;
                //Показываем данные окна входа
                SepBelowSwitcher.Points = new PointCollection() { new Point(0, 0), new Point(93, 0) };
                switcher.Text = "Зарегистрироваться";
                LoginZone.Visibility = Visibility.Visible;
                TB_login.Style = (Style)FindResource("TextBlockForTxB_login");
                TB_password.Visibility = Visibility.Visible;
            }
            else
            {

                //Скрываем данные окна входа
                SepBelowSwitcher.Points = new PointCollection() { new Point(85, 0), new Point(112, 0) };
                switcher.Text = "Уже есть аккаунт? Войти";
                LoginZone.Visibility = Visibility.Hidden;
                TB_login.Style = (Style)FindResource("HiddenTB");
                TB_password.Visibility = Visibility.Hidden;
                loginTxB.Text = "";
                //Показываем все данные окна регистрации
                RegistrationZone.Visibility = Visibility.Visible;
                TB_Registration.Style = (Style)FindResource("TextBlockForTxB_registration");
                TB_checkingPS1.Visibility = Visibility.Visible;
                TB_checkingPS2.Visibility = Visibility.Visible;
            }
        }
        //Событие PreviewTextInput не срабатывает для Ctrl, Shift, Alt и Space
        //Cобытия клавиатуры являются тунельными
        private void pnl_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            short value;
            //Если введена не цифра
            if (!Int16.TryParse(e.Text, out value))
            {
                //то: Указываем, что событие обработано и распространяться далее не должно
                e.Handled = true;
            }
        }
        private void pnl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;

        }

        private void password_KeyDown(object sender, KeyEventArgs e)
        {
            if (((PasswordBox)sender).Name == "passwordTxB")
            {
                TB_password.Visibility = Visibility.Hidden;
            }
            if (((PasswordBox)sender).Name == "PassWord1TxB")
            {
                TB_checkingPS1.Visibility = Visibility.Hidden;
            }
            if (((PasswordBox)sender).Name == "PassWord2TxB")
            {
                TB_checkingPS2.Visibility = Visibility.Hidden;
            }


        }
        private void password_KeyUp(object sender, KeyEventArgs e)
        {
            string name = ((PasswordBox)sender).Name;
            switch (name)
            {
                case "passwordTxB":
                    TB_password.Visibility = Visibility.Hidden;
                    if ((e.Key == Key.Back || e.Key == Key.Delete) && (passwordTxB.Password.Length == 0))
                    {
                        TB_password.Visibility = Visibility.Visible;
                    }
                    break;
                case "PassWord1TxB":
                    TB_checkingPS1.Visibility = Visibility.Hidden;
                    if ((e.Key == Key.Back || e.Key == Key.Delete) && (PassWord1TxB.Password.Length == 0))
                    {
                        TB_checkingPS1.Visibility = Visibility.Visible;
                    }
                    break;
                case "PassWord2TxB":
                    TB_checkingPS2.Visibility = Visibility.Hidden;
                    if ((e.Key == Key.Back || e.Key == Key.Delete) && (PassWord1TxB.Password.Length == 0))
                    {
                        TB_checkingPS2.Visibility = Visibility.Visible;
                    }
                    break;

            }
        }

        #endregion
        #region registration and login
        private void Registration_btn_Click(object sender, RoutedEventArgs e)
        {
            if ((RegistrationTxB.Text != "")  && (PassWord1TxB.Password != "") && (PassWord2TxB.Password != ""))
            {
                if (PassWord1TxB.Password == PassWord2TxB.Password)
                {


                    User user = new User(RegistrationTxB.Text, PassWord2TxB.Password);
                    if (AllUsers.FindUser(user) == null)
                    {
                        AllUsers.RemoveMark();
                        AllUsers.Users.Add(user);
                        AllUsers.MarkUser(user);
                        this.Close();
                    }
                    else
                    {
                        TB_message.Text = "Данный пользователь уже зарегистрирован!";
                        TB_message.Visibility = Visibility.Visible;
                    }

                }
                else
                {
                    TB_message.Text = "Пароли не совпадают!";
                    TB_message.Visibility = Visibility.Visible;
                }
            }
            else
            {
                TB_message.Text = "Не все поля заполнены!";
                TB_message.Visibility = Visibility.Visible;
            }


        }
        private void Login_btn_Click(object sender, RoutedEventArgs e)
        {
            if ((loginTxB.Text != "") && (passwordTxB.Password != ""))
            {

                User user = AllUsers.FindUser(loginTxB.Text, passwordTxB.Password);
                if (user != null)
                {
                    AllUsers.RemoveMark();
                    AllUsers.MarkUser(user);
                    this.Close();
                }
                else
                {
                    TB_message.Text = "Неправильно указан логин или пароль!";
                    TB_message.Visibility = Visibility.Visible;
                }

            }
            else
            {
                TB_message.Text = "Не все поля заполнены!";
                TB_message.Visibility = Visibility.Visible;
            }

        }
        #endregion


    }
}
