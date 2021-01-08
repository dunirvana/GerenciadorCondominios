using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GerenciadorCondominiosDAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GerenciadorCondominios.Controllers
{
    public class DashboardController : Controller
    {
        public readonly IAluguelRepositorio AluguelRepositorio;
        public readonly IHistoricoRecursoRepositorio HistoricoRecursoRepositorio;

        public DashboardController(IAluguelRepositorio aluguelRepositorio, IHistoricoRecursoRepositorio historicoRecursoRepositorio)
        {
            AluguelRepositorio = aluguelRepositorio;
            HistoricoRecursoRepositorio = historicoRecursoRepositorio;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Anos"] = new SelectList(await AluguelRepositorio.PegarTodosAnos());
            return View();
        }
    }
}
 