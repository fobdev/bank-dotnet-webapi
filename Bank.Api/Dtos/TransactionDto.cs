using System;

namespace Bank.Api.Models
{
    public record TransactionDto
    {
        public Guid id { get; init; }
        public Guid sender { get; init; }
        public Guid receiver { get; init; }
        public DateTimeOffset createdAt { get; set; }
        public double amount { get; set; }
    }
}