using GerenciadorCondominiosDAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorCondominios.ViewComponents
{
    public class VeiculosViewComponent : ViewComponent
    {
        private readonly IVeiculoRepositorio VeiculoRepositorio;

        public VeiculosViewComponent(IVeiculoRepositorio veiculoRepositorio)
        {
            VeiculoRepositorio = veiculoRepositorio;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            return View(await VeiculoRepositorio.PegarVeiculosPorUsuario(id)) ;
        }
    }
}
