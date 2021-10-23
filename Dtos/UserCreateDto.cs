using System.ComponentModel.DataAnnotations;

namespace Banking.Models
{
    public record UserCreateDto
    {
        [Required]
        public string type { get; set; } // sales user or common user
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