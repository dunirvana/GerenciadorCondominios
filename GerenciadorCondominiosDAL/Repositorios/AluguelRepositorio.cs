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
    public class AluguelRepositorio : RepositorioGenerico<Aluguel>, IAluguelRepositorio
    {
        private readonly Contexto Contexto;

        public AluguelRepositorio(Contexto contexto) : base(contexto)
        {
            Contexto = contexto;
        }

        public bool AluguelJaExiste(int mesId, int ano)
        {
            try
            {
                return Contexto.Alugueis.Any(a => a.MesId == mesId && a.Ano == ano);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public new async Task<IEnumerable<Aluguel>> PegarTodos()
        {
            try
            {
                return await Contexto.Alugueis
                    .Include(a => a.Mes)  
                    .ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<IEnumerable<int>> PegarTodosAnos()
        {
            try
            {
                return await Contexto.Alugueis.Select(a => a.Ano).Distinct().ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
