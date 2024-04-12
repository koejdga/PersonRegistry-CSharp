using Person.Services;
using Person.Stores;
using Person.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Person.Models;
using Prism.Commands;

namespace Person.ViewModels
{
    public class ShowUserDataViewModel : ViewModelBase
    {
        private ObservableCollection<PersonViewModel> _allPeople;
        private ObservableCollection<PersonViewModel> _people;
        private readonly NavigationStore _navigationStore;
        private readonly PersonService _personService;
        private static readonly string IsInitialized = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PersonAppCSharp", "isInitialized");

        public ObservableCollection<PersonViewModel> People
        {
            get => _people;
            private set
            {
                _people = value;
                OnPropertyChanged();
            }
        }
        public string FilterText { get; set; }
        public string SelectedColumn { get; set; }

        public ShowUserDataViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _personService = new PersonService();

            // _personService.DeleteDataBase();
            Add50People();

            _allPeople = new ObservableCollection<PersonViewModel>(_personService.GetAllPeople());
            People = new ObservableCollection<PersonViewModel>(_allPeople);
        }

        private void Add50People()
        {
            if (File.Exists(IsInitialized)) return;

            string[] names = { "John", "Jane", "Jack", "Jamie", "Jude", "Julia", "Jesse", "Jacob" };
            string[] surnames = { "Smith", "Black", "White", "Williams", "Johns", "Martin", "Wilson", "Taylor"};

            var authService = new AuthenticationService();
            var random = new Random();

            for (var i = 0; i < 50; i++)
            {
                var name = names[random.Next(0, names.Length)];
                var surname = surnames[random.Next(0, surnames.Length)];

                var year = random.Next(1950, 2022);
                var month = random.Next(1, 12);
                var day = random.Next(1, 28);

                var emailLocalPart = "";
                for (var j = 0; j < 5; j++)
                {
                    emailLocalPart += (char)random.Next(97, 122);
                }

                var person = new RegisterPersonModel(name,
                    surname, emailLocalPart + "@gmail.com", new DateTime(year, month, day));

                try
                {
                    authService.RegisterUser(person);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }

            authService.MarkSystemInitialized(IsInitialized);
        }

        #region Reset Command

        private DelegateCommand<PersonViewModel> _resetCommand;

        public DelegateCommand<PersonViewModel> ResetCommand =>
            _resetCommand ?? (_resetCommand = new DelegateCommand<PersonViewModel>(ExecuteResetCommand));

        private void ExecuteResetCommand(PersonViewModel parameter)
        {
            People = new ObservableCollection<PersonViewModel>(_allPeople);
        }

        #endregion

        #region Edit Person Command

        private DelegateCommand<PersonViewModel> _editCommand;

        public DelegateCommand<PersonViewModel> EditCommand =>
            _editCommand ?? (_editCommand = new DelegateCommand<PersonViewModel>(ExecuteEditCommand));

        private void ExecuteEditCommand(PersonViewModel parameter)
        {
            _navigationStore.CurrentViewModel = new InputUserDataViewModel(new PersonModel(parameter.Name,
                parameter.Surname, parameter.Email, Convert.ToDateTime(parameter.BirthDate)), _navigationStore);
        }

        #endregion

        #region Delete Person Command

        private DelegateCommand<PersonViewModel> _deleteCommand;

        public DelegateCommand<PersonViewModel> DeleteCommand =>
            _deleteCommand ?? (_deleteCommand = new DelegateCommand<PersonViewModel>(ExecuteDeleteCommand));

        private void ExecuteDeleteCommand(PersonViewModel parameter)
        {
            _allPeople.Remove(parameter);
            People.Remove(parameter);
            _personService.DeleteUser(parameter);
        }

        #endregion

        #region Add New Person Command

        private RelayCommand<object> _addNewPerson;

        public RelayCommand<object> AddNewPersonCommand
        {
            get
            {
                if (_addNewPerson == null)
                {
                    _addNewPerson = new RelayCommand<object>(_ => AddNewPerson(), CanExecute);
                };
                return _addNewPerson;
            }
        }

        private void AddNewPerson()
        {
            _navigationStore.CurrentViewModel = new InputUserDataViewModel(_navigationStore);
        }

        private bool CanExecute(object obj)
        {
            return true;
        }

        #endregion

        #region Only name Jo Command

        private RelayCommand<object> _filter;

        public RelayCommand<object> FilterCommand
        {
            get
            {
                if (_filter == null)
                {
                    _filter = new RelayCommand<object>(_ => 
                        Filter(FilterText), CanExecuteFilter);
                };
                return _filter;
            }
        }

        private void Filter(string str)
        {
            if (str == null)
            {
                MessageBox.Show("Input is empty");
                return;
            }

            IEnumerable<PersonViewModel> filteredRows;

            filteredRows = from user in _allPeople
                let prop = typeof(PersonViewModel).GetProperty(SelectedColumn.Split()[1])
                where prop.GetValue(user).ToString().Contains(str)
                select user;

            People = new ObservableCollection<PersonViewModel>(filteredRows.ToList());
        }

        private bool CanExecuteFilter(object obj)
        {
            return !string.IsNullOrEmpty(FilterText) && !string.IsNullOrEmpty(SelectedColumn);
        }

        #endregion

    }
}
