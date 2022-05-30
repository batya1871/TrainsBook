using System;
using System.Collections.Generic;
using System.Text;

namespace TrainBook
{
    public class Route
    {
        public string DeparturePoint { get; set; }
        public string ArrivalPoint { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public bool IsActiveNow { get; set; }
        public int TrainsId { get; set; }
        public Route(string DPoint, string Apoint, DateTime DD, DateTime AD, int triansId)
        {
            DeparturePoint = DPoint;
            ArrivalPoint = Apoint;
            DepartureDate = DD;
            ArrivalDate = AD;
            TrainsId = triansId;
            this.CheckActivity();
        }
        public void CheckActivity()
        {
            DateTime Now = DateTime.Now;
            if ((DepartureDate <= Now) && (Now <= ArrivalDate))
            {
                IsActiveNow = true;
            }
            else IsActiveNow = false;
        }


    }
}
