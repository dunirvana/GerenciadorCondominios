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
    public class MesRepositorio : RepositorioGenerico<Mes>, IMesRepositorio
    {
        private readonly Contexto Contexto;

        public MesRepositorio(Contexto contexto) : base(contexto)
        {
            Contexto = contexto;
        }

        public new async Task<IEnumerable<Mes>> PegarTodos()
        {
            try
            {
                return await Contexto.Meses.OrderBy(m => m.MesId).ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
