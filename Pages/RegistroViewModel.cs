using System.ComponentModel.DataAnnotations;

namespace Agencia_Bogota.Pages
{
    public class RegistroViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }
}
