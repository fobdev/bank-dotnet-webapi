using System;
using System.Collections.Generic;
using Bank.Models;

namespace Bank.Repositories
{
    public interface IUserRepository
    {
        User GetUser(Guid id);
        IEnumerable<User> GetUsers();
        void CreateUser(User user);
        double GetUserBalance(Guid id);
        bool GetUserType(Guid id);
        void SetUserBalance(User user, double newBalance);
    }
}