using Person.Models;
using Person.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;

namespace Person.Services
{
    class AuthenticationService
    {
        private static readonly FileRepository Repository = new FileRepository();

        public void RegisterUser(RegisterPersonModel person)
        {
            var dbPerson = Repository.Get(person.Email.ToString());
            if (dbPerson != null)
                throw new Exception("Person was already added");
            Repository.AddOrUpdate(new DbPersonModel(new PersonModel(person)));
        }

        public async Task<bool> RegisterUserAsync(RegisterPersonModel person)
        {
            var dbPerson = await Repository.GetAsync(person.Email.ToString());
            if (dbPerson != null)
                throw new Exception("Person was already added");
            await Repository.AddOrUpdateAsync(new DbPersonModel(new PersonModel(person)));
            return true;
        }

        public void MarkSystemInitialized(string isInitializedFilePath)
        {
            Repository.Initialize(isInitializedFilePath);
        }

    }
}