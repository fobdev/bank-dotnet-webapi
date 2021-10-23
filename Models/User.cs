using System;

namespace Bank.Models
{
    public record User
    {
        public Guid id { get; init; }
        public string type { get; init; }
        public string name { get; init; }
        public string cpf { get; init; }
        public string email { get; init; }
        public string password { get; init; }
        public double balance { get; set; }
    }
}