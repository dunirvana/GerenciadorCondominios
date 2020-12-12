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
    public class EventoRepositorio : RepositorioGenerico<Evento>, IEventoRepositorio
    {
        private readonly Contexto Contexto;

        public EventoRepositorio(Contexto contexto) : base(contexto)
        {
            Contexto = contexto;
        }

        public async Task<IEnumerable<Evento>> PegarEventosPeloId(string usuarioId)
        {
            try
            {
                return await Contexto.Eventos.Where(e => e.UsuarioId == usuarioId).ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
