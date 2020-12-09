using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorCondominios.ViewModels
{
    public class AtualizarViewModel
    {
        public string UsuarioId { get; set; }

        [Required(ErrorMessage = "Campo {0} obrigatório")]
        [StringLength(40, ErrorMessage = "Tamanho máximo de 40 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Campo {0} obrigatório")]
        public string CPF { get; set; }

        [Required(ErrorMessage = "Campo {0} obrigatório")]
        public string Telefone { get; set; }

        public string Foto { get; set; }

        [Required(ErrorMessage = "Campo {0} obrigatório")]
        [StringLength(40, ErrorMessage = "Tamanho máximo de 40 caracteres")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; }
    }
}
