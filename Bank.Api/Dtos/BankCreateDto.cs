using System;
using System.ComponentModel.DataAnnotations;

namespace Bank.Api.Dtos
{
    public record PhysicalBankCreateDto
    {
        [Required]
        public double balance { get; init; }
    }
}