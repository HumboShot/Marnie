using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marnie.Model
{
    public class Jorney
    {
        public int Id { get; set; }
        public int RouteId { get; set; }
        public int PersonId { get; set; }
        public string StartLocation { get; set; }
        public DateTime StartTime { get; set; }
        public string Destination { get; set; }
        public DateTime EndTime { get; set; }
        public int Status { get; set; }
        public Person Person { get; set; }
        public Route Route { get; set; }
    }
}
