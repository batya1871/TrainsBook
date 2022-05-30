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
        public bool InRoute { get; set; }
        public bool TechnicalCondition { get; set; }
        public string DescriptionOfCondition { get; set; }
        public DateTime dateOfDebiting { get; set; }
        public Train(int id)
        {
            Id = id;
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
       public void DetermineState()
        {
            if (DescriptionOfCondition == null)
            {
                if (InRoute) DescriptionOfCondition = "В рейсе";
                if (!TechnicalCondition) DescriptionOfCondition = "Списан. ";
                if ((!InRoute) && (TechnicalCondition)) DescriptionOfCondition = "В простое.";
            }
        }
    }
}
