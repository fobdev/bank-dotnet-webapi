using System;

namespace Bank.Api.Dtos
{
    public record PhysicalBankDto
    {
        public Guid id { get; init; }
        public double balance { get; init; }
    }
}