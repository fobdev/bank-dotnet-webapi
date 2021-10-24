using System.ComponentModel.DataAnnotations;

namespace Bank.Api.Models
{
    public record UserCreateDto
    {
        [Required]
        public bool staff { get; set; } // sales or common
        [Required]
        public string name { get; init; }
        [Required]
        public string cpf { get; init; }
        [Required]
        public string email { get; init; }
        [Required]
        public string password { get; init; }
    }
}