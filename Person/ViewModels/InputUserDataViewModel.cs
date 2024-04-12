using Person.Exceptions;
using Person.Models;
using Person.ViewModels;
using Person.Stores;
using Person.Tools;
using Person.Services;
using System;
using System.Threading.Tasks;
using System.Windows;
using Person.Views;

namespace Person.ViewModels
{
    public class InputUserDataViewModel : ViewModelBase
    {

        #region Private Fields
        private bool _isEnabled = true;
        private RelayCommand<object> _proceedCommand;
        private RegisterPersonModel _newPerson;
        private PersonModel _person;
        private readonly NavigationStore _navigationStore;
        #endregion

        #region Person Fields
        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private string _surname;

        public string Surname
        {
            get => _surname;
            set
            {
                _surname = value;
                OnPropertyChanged();
            }
        }

        private string _email;

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        private DateTime _birthdate = new DateTime(2023, 3, 20);

        public DateTime Birthdate
        {
            get => _birthdate;
            set
            {
                _birthdate = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Properties
        public RelayCommand<object> ProceedCommand
        {
            get
            {
                if (_proceedCommand == null)
                {
                    _proceedCommand = new RelayCommand<object>(_ => Proceed(), CanExecute);
                };
                return _proceedCommand;
            }
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Proceed Button

        private async void Proceed()
        {
            try
            {
                IsEnabled = false;
                if (_person == null)
                {
                    _newPerson = new RegisterPersonModel(Name,
                        Surname,
                        Email,
                        Birthdate);
                    var authService = new AuthenticationService();
                    await Task.Run(() => authService.RegisterUserAsync(_newPerson));
                }
                else
                {
                    _person = new PersonModel(Name, Surname, Email, Birthdate);
                    var personService = new PersonService();
                    await Task.Run(() => personService.UpdateUser(_person));
                    MessageBox.Show($"Person successfully edited");
                }
                
                _navigationStore.CurrentViewModel =
                    new ShowUserDataViewModel(_navigationStore);
            }
            catch (BirthDateIsInFutureException e)
            {
                HandleException(e);
            }
            catch (BirthDateIssTooFarAwayException e)
            {
                HandleException(e);
            }
            catch (InvalidEmailAddressException e)
            {
                HandleException(e);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return;
            }
            finally
            {
                IsEnabled = true;
            }
        }

        private void HandleException(Exception e)
        {
            _ = MessageBox.Show(e.Message, "Error", MessageBoxButton.OK);
        }

        private bool CanExecute(object obj)
        {
            return !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Surname) &&
                !string.IsNullOrEmpty(Email);
        }

        #endregion

        #region Cancel Command

        private RelayCommand<object> _cancel;

        public RelayCommand<object> CancelCommand
        {
            get
            {
                if (_cancel == null)
                {
                    _cancel = new RelayCommand<object>(_ => Cancel(), CanExecuteCancel);
                };
                return _cancel;
            }
        }

        private void Cancel()
        {
            _navigationStore.CurrentViewModel = new ShowUserDataViewModel(_navigationStore);
        }

        private bool CanExecuteCancel(object obj)
        {
            return true;
        }

        #endregion

        #region Constructors

        public InputUserDataViewModel(PersonModel person, NavigationStore navigationStore) : this(navigationStore)
        {
            _person = person;
            Name = _person.Name;
            Surname = _person.Surname;
            Email = _person.Email.ToString();
            Birthdate = _person.BirthDate;
        }

        public InputUserDataViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        #endregion



    }
}
