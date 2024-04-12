using Person.Exceptions;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;

namespace Person.Models
{
    public class DbPersonModel
    {
        public string Name { get; private set; }
        public string Surname { get; private set; }
        public string Email { get; private set; }
        public string BirthDate { get; private set; }
        public bool IsAdult { get; private set; }
        public int SunSign { get; private set; }
        public int ChineseSign { get; private set; }
        public bool IsBirthday { get; private set; }

        public DbPersonModel(PersonModel person) :
            this(person.Name, person.Surname, person.Email.ToString(), person.BirthDate.ToString("d"), person.IsAdult,
                (int)person.SunSign, (int)person.ChineseSign, person.IsBirthday)
        { }

        [JsonConstructor]
        public DbPersonModel(string name, string surname, string email, string birthdate,
            bool isAdult, int sunSign, int chineseSign, bool isBirthday)
        {
            Name = name;
            Surname = surname;
            Email = email;
            BirthDate = birthdate;
            IsAdult = isAdult;
            SunSign = sunSign;
            ChineseSign = chineseSign;
            IsBirthday = isBirthday;
        }

    }
}
