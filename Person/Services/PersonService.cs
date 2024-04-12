using Person.Models;
using Person.ViewModels;
using Person.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Person.Services
{
    class PersonService
    {
        private static readonly FileRepository Repository = new FileRepository();

        public List<PersonViewModel> GetAllPeople()
        {
            return Repository.GetAll().
                Select(person => new PersonViewModel(new PersonModel(person))).ToList();
        }

        public void DeleteUser(PersonViewModel person)
        {
            Repository.Delete(person.Email);
        }

        public async Task UpdateUser(PersonModel person)
        {
            await Repository.AddOrUpdateAsync(new DbPersonModel(person));
        }

        public void DeleteDataBase()
        {
            Repository.DeleteAll();
        }
    }
}