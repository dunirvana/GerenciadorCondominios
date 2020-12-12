using GerenciadorCondominiosBLL.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorCondominiosDAL.Interfaces
{
    public interface IEventoRepositorio : IRepositorioGenerico<Evento>
    {
        Task<IEnumerable<Evento>> PegarEventosPeloId(string usuarioId);
    }
}
