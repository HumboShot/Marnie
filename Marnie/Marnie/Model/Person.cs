using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marnie.Model
{
    public class Person
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public string Sex { get; set; }
        public string ProfilPicture { get; set; }

        public Person(int iD, string name, DateTime birthday, string sex, string profilPicture)
        {
            ID = iD;
            Name = name;
            Birthday = birthday;
            Sex = sex;
            ProfilPicture = profilPicture;
        }
    }
}
