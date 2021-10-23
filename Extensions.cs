using Bank.Dtos;
using Bank.Models;

namespace Bank
{
    public static class Extensions
    {
        public static UserDto AsUserDto(this User user)
        {
            return new UserDto
            {
                id = user.id,
                name = user.name,
                type = user.type,
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
    }
}