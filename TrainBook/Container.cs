using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TrainBook
{
    public static  class Container
    {
        public static BindingList<Route> RoutesList;
        public static  BindingList<Train> TrainsList;
        public static LinkedList<Train> EventsList;

       

        //0 = true - закрыть приложение
        //1 = true - открыть список поездов
        //2 = true - открыть оповещения
        //3 = true - открыть архив маршрутов
        public static bool[] WindowData = new bool[4];
        public static bool NewNotes { get; set; } = false;
        static Container()
        {
            RoutesList = new BindingList<Route>();
            TrainsList = new BindingList<Train>();
            EventsList = new LinkedList<Train>();

            for (int i = 0; i < 4; i++)
            {

                WindowData[i] = false;
            }
        }
        public static void ClearWD()
        {
            for (int i = 0; i < 4; i++)
            {

                WindowData[i] = false;
            }
        }
        public static void getData(BindingList<Route> rl, BindingList<Train> tl)
        {
            RoutesList = rl;
            TrainsList = tl;
        }
    }
}
