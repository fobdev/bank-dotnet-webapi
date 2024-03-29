using Bank.Api.Dtos;
using Bank.Api.Models;

namespace Bank.Api
{
    public static class Extensions
    {
        public static UserDto AsUserDto(this User user)
        {
            return new UserDto
            {
                id = user.id,
                name = user.name,
                premium = user.premium,
                balance = user.balance
            };
        }
        public static TransactionDto AsTransactionDto(this Transaction transaction)
        {
            return new TransactionDto
            {
                id = transaction.id,
                amount = transaction.amount,
                receiver = transaction.receiver,
                sender = transaction.sender,
                createdAt = transaction.createdAt
            };
        }

        public static PhysicalBankDto AsBankDto(this PhysicalBank bank)
        {
            return new PhysicalBankDto
            {
                id = bank.id,
                balance = bank.balance
            };
        }
    }
}