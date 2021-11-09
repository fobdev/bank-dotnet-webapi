using System;

namespace Bank.Api.Dtos
{
    public record PhysicalBankCreateDto
    {
        public double balance { get; init; }
    }
}