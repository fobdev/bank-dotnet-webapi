using System;
using Bank.Api.Models;

namespace Bank.Api.Repositories
{
    public interface IBankRepository
    {
        void MakeLoan(Guid sending, Guid receiving, double amount);
        PhysicalBank GetBank(Guid id);
        void CreateBank(PhysicalBank bank);

    }
}