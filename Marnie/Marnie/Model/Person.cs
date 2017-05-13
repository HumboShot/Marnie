using System;
using System.Collections.Generic;

namespace Marnie.Model
{

    public class Person
    {
        public int Id { get; set; }
        public string AuthId { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public string Gender { get; set; }
        public string ProfilePicture { get; set; }        
        public ICollection<Journey> Journeys { get; set; } = new List<Journey>();        
        public ICollection<Date> Dates { get; set; } = new List<Date>();

        public Person(string auth_id)
        {
            AuthId = auth_id;
        }

        public Person()
        {

        }
    }
}

