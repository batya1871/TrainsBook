using System;
using System.Collections.Generic;
using System.Text;

namespace TrainBook
{
    class Train
    {
        public int Id { get; set; }
        public double Speed { get; set; }
        public bool InRoute { get; set; }
        public bool TechnicalCondition { get; set; }
        Train(int id, double speed, bool inRoute, bool Cond)
        {
            Id = id;
            Speed = speed;
            InRoute = inRoute;
            TechnicalCondition = Cond;
        }
    }
}
