using GerenciadorCondominiosBLL.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorCondominiosDAL.Interfaces
{
    public interface IHistoricoRecursoRepositorio : IRepositorioGenerico<HistoricoRecurso>
    {
        object PegarHistoricoGanhos(int ano);
        object PegarHistoricoDespesas(int ano);

        public Task<decimal> PegarSomaDespesas(int ano);
        public Task<decimal> PegarSomaGanhos(int ano);

        public Task<IEnumerable<HistoricoRecurso>> PegarUltimasMovimentacoes();
    }
}
