using GerenciadorCondominiosDAL.Interfaces;
using GerenciadorCondominiosDAL.Repositorios;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorCondominios.ViewComponents
{
    public class UltimasMovimentacoesViewComponent : ViewComponent
    {
        private readonly IHistoricoRecursoRepositorio HistoricoRecursosRepositorio;

        public UltimasMovimentacoesViewComponent(IHistoricoRecursoRepositorio historicoRecursosRepositorio)
        {
            HistoricoRecursosRepositorio = historicoRecursosRepositorio;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await HistoricoRecursosRepositorio.PegarUltimasMovimentacoes());
        }
    }
}
