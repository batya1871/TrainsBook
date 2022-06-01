using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainBook
{
    public enum Posts
    {
        Admin,
        Head,
        User
    }
    /// <summary>
    /// Класс, хранящий в себе данные для одного пользователя
    /// </summary>
    [Serializable]
    public class User
    {
        /// <summary>
        /// Пароль для входа в аккаунт
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// ФИО пользователя
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Должность пользователя. Не вводится при авторизации
        /// </summary>
        public Posts Post { get; set; }
        /// <summary>
        /// Флаг, указывающий является ли этот пользователь активным
        /// </summary>
        public bool Mark { get; set; }


        public User()
        {
            Post = Posts.User;
        }

        public User(string name, string password)
        {
            Name = name;
            Password = password;
            Mark = false;
            Post = Posts.User;
        }

    }
    [Serializable]
    public static class AllUsers
    {
        public static List<User> Users { get; set; }
        static AllUsers()
        {
            Users = new List<User>();
        }
        /// <summary>
        /// Находит и возвращает указанного пользователя
        /// </summary>
        /// <param name="SelectedUser"></param>
        /// <returns></returns>
        public static User FindUser(User SelectedUser)
        {
            foreach (User user in Users)
            {
                if ((user.Name == SelectedUser.Name) && (user.Password == SelectedUser.Password) && (user.Post == SelectedUser.Post))
                {
                    return user;
                }
            }
            return null;
        }
        /// <summary>
        /// Находит и возвращает указанного пользователя
        /// </summary>
        /// <param name="SelectedUser"></param>
        /// <returns></returns>
        public static User FindUser(string name, string password)
        {
            foreach (User user in Users)
            {
                if ((user.Name == name) && (user.Password == password))
                {
                    return user;
                }
            }
            return null;
        }
        /// <summary>
        /// Помечает пользователя как активного
        /// </summary>
        /// <param name="SelectedUser"></param>
        public static void MarkUser(User SelectedUser)
        {
            foreach (User user in Users)
            {
                if ((user.Name == SelectedUser.Name) && (user.Password == SelectedUser.Password) && (user.Post == SelectedUser.Post))
                {
                    user.Mark = true;
                }
            }
        }
        /// <summary>
        /// Убирает метку активности с выбранного на данный момент пользователя
        /// </summary>
        public static void RemoveMark()
        {
            foreach (User user in Users)
            {
                if (user.Mark == true)
                {
                    user.Mark = false;
                }
            }
        }
        /// <summary>
        /// Находит и возвращает активного пользователя
        /// </summary>
        /// <returns></returns>
        public static User FindMarkedUser()
        {
            foreach (User user in Users)
            {
                if (user.Mark == true)
                {
                    return user;
                }
            }
            return null;
        }
    }
    /// <summary>
    /// Отдельный класс, хранящий данные для сериализации\десериализации
    /// </summary>
    [Serializable]
    public class DataForSavingUsers
    {
        /// <summary>
        /// Активный пользователь
        /// </summary>
        public User MarkedUser { get; set; }
        /// <summary>
        /// Список пользователей
        /// </summary>
        public List<User> Users { get; set; }
        public DataForSavingUsers(User markedUser, List<User> users)
        {
            MarkedUser = markedUser;
            Users = users;
        }
        public DataForSavingUsers(List<User> users)
        {
            Users = users;
        }

    }
}
