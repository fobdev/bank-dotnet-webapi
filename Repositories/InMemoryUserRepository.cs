using System;
using System.Collections.Generic;
using System.Linq;
using Bank.Models;

namespace Bank.Repositories
{
    /*
        ===================================================
        local test / no database
        ===================================================
    */
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<User> users = new()
        {
            new User
            {
                id = Guid.NewGuid(),
                name = "Pedro",
                staff = false,
                cpf = "12345678987",
                email = "pedro@pedrodev.net",
                password = "passwd123",
                balance = 1200.00
            },
            new User
            {
                id = Guid.NewGuid(),
                name = "Lucas",
                staff = false,
                cpf = "98765432187",
                email = "lucas@pedrodev.net",
                password = "passwd999",
                balance = 800.00
            },
            new User
            {
                id = Guid.NewGuid(),
                name = "José",
                staff = true,
                cpf = "65432198754",
                email = "jose@pedrodev.net",
                password = "passwd555",
                balance = 100.00
            },
        };
        public IEnumerable<User> GetUsers()
        {
            return users;
        }
        public User GetUser(Guid id)
        {
            return users.Where(user => user.id == id).SingleOrDefault();
        }

        public void CreateUser(User user)
        {
            users.Add(user);
        }
        public bool GetUserType(Guid id)
        {
            return users.Where(user => user.id == id).SingleOrDefault().staff;
        }
        public double GetUserBalance(Guid id)
        {
            return users.Where(user => user.id == id).SingleOrDefault().balance;
        }
        public void SetUserBalance(User user, double newBalance)
        {
            user.balance = newBalance;
        }

    }
}