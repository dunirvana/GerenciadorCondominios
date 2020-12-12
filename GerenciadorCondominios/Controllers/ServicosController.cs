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

namespace GerenciadorCondominios.Controllers
{
    public class ServicosController : Controller
    {
        private readonly IServicoRepositorio ServicoRepositorio;
        private readonly IUsuarioRepositorio UsuarioRepositorio;

        public ServicosController(IServicoRepositorio servicoRepositorio, IUsuarioRepositorio usuarioRepositorio)
        {
            ServicoRepositorio = servicoRepositorio;
            UsuarioRepositorio = usuarioRepositorio;
        }

        public async Task<IActionResult> Index()
        {
            var usuario = await UsuarioRepositorio.PegarUsuarioPeloNome(User);
            if (await UsuarioRepositorio.VerificarSeUsuarioEstaEmFuncao(usuario, "Morador"))
            {
                return View(await ServicoRepositorio.PegarServicoPeloUsuario(usuario.Id));
            }

            return View(await ServicoRepositorio.PegarTodos());
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var usuario = await UsuarioRepositorio.PegarUsuarioPeloNome(User);
            var servico = new Servico
            {
                UsuarioId = usuario.Id
            };

            return View(servico);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SevicoId,Nome,Valor,Status,UsuarioId")] Servico servico)
        {
            if (ModelState.IsValid)
            {
                servico.Status = StatusServico.Pendente;
                await ServicoRepositorio.Inserir(servico);
                TempData["NovoRegistro"] = $"Serviço {servico.Nome} inserido com sucesso";

                return RedirectToAction(nameof(Index));
            }

            return View(servico);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var servico = await ServicoRepositorio.PegarPeloId(id);
            if (servico == null)
            {
                return NotFound();
            }

            return View(servico);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SevicoId,Nome,Valor,Status,UsuarioId")] Servico servico)
        {
            if (id != servico.SevicoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await ServicoRepositorio.Atualizar(servico);
                TempData["Atualizacao"] = $"Serviço {servico.Nome} atualizado com sucesso";
                return RedirectToAction(nameof(Index));
            }

            return View(servico);
        }

        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            await ServicoRepositorio.Excluir(id);
            TempData["Exclusao"] = "Serviço excluido";

            return Json("Serviço excluido");
        }

    }
}
