using GerenciadorCondominiosBLL.Models;
using GerenciadorCondominiosDAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorCondominiosDAL.Repositorios
{
    public class PagamentoRepositorio : RepositorioGenerico<Pagamento>, IPagamentoRepositorio
    {
        private readonly Contexto Contexto;

        public PagamentoRepositorio(Contexto contexto) : base(contexto)
        {
            Contexto = contexto;
        }

        public async Task<IEnumerable<Pagamento>> PegarPagamentosPorUsuario(string usuarioId)
        {
            try
            {
                return await Contexto.Pagamentos
                    .Include(p => p.Aluguel)
                    .ThenInclude(p => p.Mes)
                    .Where(p => p.UsuarioId == usuarioId).ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
