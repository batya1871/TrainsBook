using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TrainBook
{
    public class Train
    {
        public int Id { get; set; }
        public double Speed { get; set; }
        public bool InRoute { get; set; }
        public bool TechnicalCondition { get; set; }
        [JsonProperty]
        public static double MaxSpeed { get; set; }
        public Train(int id, double speed)
        {
            Id = id;
            Speed = speed;
            TechnicalCondition = true;
        }

       public void isInRoute(BindingList<Route> list)
        {
            bool isInRoute = false;
            foreach (Route route in list)
            {
                if (route.TrainsId == Id)
                {
                    InRoute = true;
                    isInRoute = true;
                    break;
                }
            }
            if (!isInRoute) InRoute = false;
        }
    }
}
