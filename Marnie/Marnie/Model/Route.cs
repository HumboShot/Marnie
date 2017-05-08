using System.Collections.Generic;

namespace Marnie.Model
{
    public class Route
    {
        public Route()
        {

        }

        public int Id { get; set; }        
        public string Name { get; set; }        
        public ICollection<Stop> Stops { get; set; } = new List<Stop>();
    }
}