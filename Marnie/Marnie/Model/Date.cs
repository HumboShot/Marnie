using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marnie.Model
{
    public class Date
    {       
        public int ID { get; set; }
        public int TrainID { get; set; }
        public string StartLocation { get; set; }
        public string Distination { get; set; }

        public Date(int iD, int trainID, string startLocation, string distination)
        {
            ID = iD;
            TrainID = trainID;
            StartLocation = startLocation;
            Distination = distination;
        }
    }
}
