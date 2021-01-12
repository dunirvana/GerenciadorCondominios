using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GerenciadorCondominiosBLL.Models;
using GerenciadorCondominiosDAL;
using GerenciadorCondominiosDAL.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace GerenciadorCondominios.Controllers
{
    [Authorize]
    public class VeiculosController : Controller
    {
        private readonly IVeiculoRepositorio VeiculoRepositorio;
        private readonly IUsuarioRepositorio UsuarioRepositorio;

        public VeiculosController(IVeiculoRepositorio veiculoRepositorio, IUsuarioRepositorio usuarioRepositorio)
        {
            VeiculoRepositorio = veiculoRepositorio;
            UsuarioRepositorio = usuarioRepositorio;
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VeiculoId,Nome,Marca,Cor,Placa,UsuarioId")] Veiculo veiculo)
        {
            if (ModelState.IsValid)
            {
                var usuario = await UsuarioRepositorio.PegarUsuarioPeloNome(User);
                veiculo.UsuarioId = usuario.Id;
                await VeiculoRepositorio.Inserir(veiculo);

                return RedirectToAction("MinhasInformacoes", "Usuarios");
            }

            return View(veiculo);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {

            var veiculo = await VeiculoRepositorio.PegarPeloId(id);
            if (veiculo == null)
            {
                return NotFound();
            }

            return View(veiculo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VeiculoId,Nome,Marca,Cor,Placa,UsuarioId")] Veiculo veiculo)
        {
            if (id != veiculo.VeiculoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await VeiculoRepositorio.Atualizar(veiculo);
                return RedirectToAction("MinhasInformacoes", "Usuarios");
            }

            return View(veiculo);
        }

        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            await VeiculoRepositorio.Excluir(id);

            return Json("Veículo excluido com sucesso");
        }

    }
}
