using System;

namespace Bank.Dtos
{
    public record UserDto
    {
        public Guid id { get; init; }
        public string type { get; set; } // salesuser or commonuser
        public string name { get; init; }
        public double balance { get; set; }
    }
}