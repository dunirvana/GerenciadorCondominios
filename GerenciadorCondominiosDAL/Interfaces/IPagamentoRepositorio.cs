﻿using GerenciadorCondominiosBLL.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorCondominiosDAL.Interfaces
{
    public interface IPagamentoRepositorio : IRepositorioGenerico<Pagamento>
    {
        Task<IEnumerable<Pagamento>> PegarPagamentosPorUsuario(string usuarioId);
    }
}
