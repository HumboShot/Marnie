using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marnie.Model
{
    class Post
    {
        public int ID;
        public int TrainId;
        public string StartLocation;
        public string Distination;
        public int PersonId;
        public DateTime TravelDate;
        public int Status;

        public Post(int iD, int trainId, string startLocation, string distination, int personId, DateTime travelDate, int status)
        {
            ID = iD;
            TrainId = trainId;
            StartLocation = startLocation;
            Distination = distination;
            PersonId = personId;
            TravelDate = travelDate;
            Status = status;
        }
    }
}
