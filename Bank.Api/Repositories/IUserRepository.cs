using System;
using System.Collections.Generic;
using Bank.Api.Models;

namespace Bank.Api.Repositories
{
    public interface IUserRepository
    {
        User GetUserById(Guid id);
        IEnumerable<User> GetUsers();
        void CreateUser(User user);
        double GetUserBalance(Guid id);
        bool GetUserType(Guid id);
        void SetUserBalance(User user, double newBalance);
        bool ExistsEmail(string email);
        bool ExistsCPF(string cpf);
    }
}