using System;
using Bank.Api.Models;

namespace Bank.Api.Repositories
{
    public interface IBankRepository
    {
        void MakeLoan(LoanCreateDto loanCreateDto);
        void CreateBank(PhysicalBank bank);
        PhysicalBank GetBankById(Guid id);
    }
}