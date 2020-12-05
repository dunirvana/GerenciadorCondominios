using System;
using System.Collections.Generic;
using System.Text;

namespace GerenciadorCondominiosBLL.Models
{
    public class Pagamento
    {
        public int PagamentoId { get; set; }

        public string  UsuarioId { get; set; }

        public virtual Usuario Usuario { get; set; }

        public int AluguelId { get; set; }

        public virtual Aluguel Aluguel { get; set; }

        public DateTime? DataPagamento { get; set; }

        public StatusPagamento Status { get; set; }
    }

    public enum StatusPagamento
    {
        Pago,
        Pendente,
        Atrasado
    }
}
