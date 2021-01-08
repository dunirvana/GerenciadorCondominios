using GerenciadorCondominiosBLL.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorCondominiosDAL.Interfaces
{
    public interface IAluguelRepositorio : IRepositorioGenerico<Aluguel>
    {
        bool AluguelJaExiste(int mesId, int ano);
        new Task<IEnumerable<Aluguel>> PegarTodos();

        Task<IEnumerable<int>> PegarTodosAnos();
    }
}

