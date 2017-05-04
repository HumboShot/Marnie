using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marnie.Model
{
    public class Post
    {
        public int ID { get; set; }
        public int TrainId { get; set; }
        public string StartLocation { get; set; }
        public string Distination { get; set; }
        public int PersonId { get; set; }
        public DateTime TravelDate { get; set; }
        public int Status { get; set; }

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
