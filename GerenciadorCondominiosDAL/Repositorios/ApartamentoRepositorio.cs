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
    public class ApartamentoRepositorio : RepositorioGenerico<Apartamento>, IApartamentoRepositorio
    {
        private readonly Contexto Contexto;

        public ApartamentoRepositorio(Contexto contexto) : base(contexto)
        {
            Contexto = contexto;
        }

        public async Task<IEnumerable<Apartamento>> PegarApartamentoPeloUsuario(string usuarioId)
        {
            try
            {
                return await Contexto.Apartamentos
                    .Include(a => a.Morador)
                    .Include(a => a.Proprietario)
                    .Where(a => a.MoradorId == usuarioId || a.ProprietarioId == usuarioId).ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public new async Task<IEnumerable<Apartamento>> PegarTodos()
        {
            try
            {
                return await Contexto.Apartamentos
                    .Include(a => a.Morador)
                    .Include(a => a.Proprietario).ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
