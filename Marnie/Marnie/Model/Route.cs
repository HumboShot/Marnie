using System.Collections.Generic;

namespace Marnie.Model
{
    public class Route
    {
        public Route()
        {

        }

        public Route(string name, ICollection<Stop> stops)
        {
            Name = name;
            Stops = stops;
        }

        public int Id { get; set; }        
        public string Name { get; set; }
        public Stop StopFrom { get; set; }
        public Stop StopTo { get; set; }
        public ICollection<Stop> Stops { get; set; } = new List<Stop>();
    }
}