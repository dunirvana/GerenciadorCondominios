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
    public class ServicoRepositorio : RepositorioGenerico<Servico>, IServicoRepositorio
    {
        private readonly Contexto Contexto;

        public ServicoRepositorio(Contexto contexto) : base(contexto)
        {
            Contexto = contexto;
        }

        public async Task<IEnumerable<Servico>> PegarServicoPeloUsuario(string usuarioId)
        {
            try
            {
                return await Contexto.Servicos.Where(e => e.UsuarioId == usuarioId).ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
