using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marnie.Model
{
    public class Date
    {
        public int ID;
        public int TrainID;
        public string StartLocation;
        public string Distination;

        public Date(int iD, int trainID, string startLocation, string distination)
        {
            ID = iD;
            TrainID = trainID;
            StartLocation = startLocation;
            Distination = distination;
        }
    }
}
