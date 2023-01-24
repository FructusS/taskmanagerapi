using System.ComponentModel.DataAnnotations;

namespace taskmanagerapi.Models.UserModels
{
    public class UserRegistrationModel
    {
        [Required, MaxLength(20), MinLength(3)]
        public string Username { get; set; } = null!;
        [Required, MaxLength(100), MinLength(5)]
        public string Password { get; set; } = null!;

        [Required, MaxLength(100), MinLength(5), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = null!;
    }
}
