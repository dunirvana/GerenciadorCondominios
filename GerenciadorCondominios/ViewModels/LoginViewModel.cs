using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorCondominios.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Campo {0} obigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo {0} obigatório")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }
    }
}
