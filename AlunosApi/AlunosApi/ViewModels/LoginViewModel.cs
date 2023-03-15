using System.ComponentModel.DataAnnotations;

namespace AlunosApi.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Email é obrigatório")]
        [EmailAddress(ErrorMessage ="Formato de email inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Email é obrigatório")]
        [StringLength(20, ErrorMessage ="A {0} deve ter no minimo {2} e no maximo {1} caracteres.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
