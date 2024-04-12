using System.Windows.Forms;
using Person.Models;
using Person.Stores;
using Person.Tools;
using MessageBox = System.Windows.Forms.MessageBox;

namespace Person.ViewModels
{
    public class PersonViewModel : ViewModelBase
    {
        private readonly PersonModel _person;

        public string Name => _person.Name;
        public string Surname => _person.Surname;
        public string Email => _person.Email.ToString();
        public string BirthDate => _person.BirthDate.ToString("d");

        public string IsAdult => _person.IsAdult ? "yes" : "no";
        public string SunSign => _person.SunSign.ToString();
        public string ChineseSign => _person.ChineseSign.ToString();
        public string IsBirthday => _person.IsBirthday ? "yes" : "no";


        public PersonViewModel(PersonModel person)
        {
            _person = person;
        }

    }
}
