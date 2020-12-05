using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GerenciadorCondominiosBLL.Models
{
    public class Veiculo
    {
        public int VeiculoId { get; set; }

        [Required(ErrorMessage = "Campo {0} obrigatório")]
        [StringLength(20, ErrorMessage = "Tamanho máximo 20 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Campo {0} obrigatório")]
        [StringLength(20, ErrorMessage = "Tamanho máximo 20 caracteres")]
        public string Marca { get; set; }

        [Required(ErrorMessage = "Campo {0} obrigatório")]
        [StringLength(20, ErrorMessage = "Tamanho máximo 20 caracteres")]
        public string Cor { get; set; }

        [Required(ErrorMessage = "Campo {0} obrigatório")]
        public string Placa { get; set; }

        public string UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
