using System;
using System.ComponentModel.DataAnnotations;

namespace Bank.Api.Models
{
    public record TransactionCreateDto
    {
        [Required]
        public Guid sender { get; init; }
        [Required]
        public Guid receiver { get; init; }
        [Required]
        public double amount { get; set; }
    }
}