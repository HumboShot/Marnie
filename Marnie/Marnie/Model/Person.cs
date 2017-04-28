using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marnie.Model
{
    class Person
    {
        public int ID;
        public string Name;
        public DateTime Birthday;
        public string Sex;
        public string ProfilPicture;

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
