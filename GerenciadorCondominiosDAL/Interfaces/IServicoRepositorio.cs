using GerenciadorCondominiosBLL.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorCondominiosDAL.Interfaces
{
    public interface IServicoRepositorio : IRepositorioGenerico<Servico>
    {
        Task<IEnumerable<Servico>> PegarServicoPeloUsuario(string usuarioId);
    }
}
