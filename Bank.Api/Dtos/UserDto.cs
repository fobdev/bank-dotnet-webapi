using System;

namespace Bank.Api.Dtos
{
    public record UserDto
    {
        public Guid id { get; init; }
        public bool premium { get; set; } // premium or common
        public string name { get; init; }
        public double balance { get; set; }
    }
}