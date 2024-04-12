using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Person.Models
{
    public class RegisterPersonModel
    {
        public string Name { get; private set; }
        public string Surname { get; private set; }
        public EmailAddressModel Email { get; private set; }
        public DateTime BirthDate { get; private set; }

        public RegisterPersonModel(string name, string surname, string email, DateTime birthDate)
        {
            Name = name;
            Surname = surname;
            BirthDate = birthDate;
            Email = new EmailAddressModel(email);
        }

        public RegisterPersonModel(PersonModel person) : 
            this(person.Name, person.Surname, person.Email.ToString(), person.BirthDate)
        {}
    }
}
