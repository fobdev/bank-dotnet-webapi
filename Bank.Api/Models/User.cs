using System;

namespace Bank.Api.Models
{
    public record User
    {
        public Guid id { get; init; }
        public bool staff { get; init; }
        public string name { get; init; }
        public string cpf { get; init; }
        public string email { get; init; }
        public string password { get; init; }
        public double balance { get; set; }
    }
}