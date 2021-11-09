using System;

namespace Bank.Api.Models
{
    public record PhysicalBank
    {
        public Guid id { get; init; }
        public double balance { get; init; }
    }
}