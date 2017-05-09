using System;

namespace Marnie.Model
{
    public class Date
    {
        public Date()
        {

        }

        public int Id { get; set; }
        public int RouteId { get; set; }
        public string DateStartLocation { get; set; }
        public DateTime StartTime { get; set; }
        public string DateDestination { get; set; }
        public DateTime EndTime { get; set; }
        public int Person1Id { get; set; }
        public Person Person1 { get; set; }
        public int Person2Id { get; set; }
        public Person Person2 { get; set; }
        public int StatusP1 { get; set; }
        public int StatusP2 { get; set; }
        public int DateStatus { get; set; }
        public Route Route { get; set; }
    }
}

