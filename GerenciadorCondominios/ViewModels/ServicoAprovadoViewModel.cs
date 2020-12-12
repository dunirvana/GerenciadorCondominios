using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorCondominios.ViewModels
{
    public class ServicoAprovadoViewModel
    {
        public int ServicoId { get; set; }

        public string Nome { get; set; }

        [Required(ErrorMessage = "Campo {0} obrigatório")]
        [Display(Name ="Dat aexecução")]
        public DateTime Data { get; set; }
    }
}
