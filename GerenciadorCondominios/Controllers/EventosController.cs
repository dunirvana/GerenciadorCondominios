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
    public class EventosController : Controller
    {
        private readonly IEventoRepositorio EventoRepositorio;
        private readonly IUsuarioRepositorio UsuarioRepositorio;

        public EventosController(IEventoRepositorio eventoRepositorio, IUsuarioRepositorio usuarioRepositorio)
        {
            EventoRepositorio = eventoRepositorio;
            UsuarioRepositorio = usuarioRepositorio;
        }

        public async Task<IActionResult> Index()
        {
            var usuario = await UsuarioRepositorio.PegarUsuarioPeloNome(User);
            if (await UsuarioRepositorio.VerificarSeUsuarioEstaEmFuncao(usuario, "Morador"))
            {
                return View(await EventoRepositorio.PegarEventosPeloId(usuario.Id));
            }

            return View(await EventoRepositorio.PegarTodos());
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var usuario = await UsuarioRepositorio.PegarUsuarioPeloNome(User);
            var evento = new Evento
            {
                UsuarioId = usuario.Id
            };

            return View(evento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventoId,Nome,Data,UsuarioId")] Evento evento)
        {
            if (ModelState.IsValid)
            {
                await EventoRepositorio.Inserir(evento);
                TempData["NovoRegistro"] = $"Evento {evento.Nome} inserido com sucesso";

                return RedirectToAction(nameof(Index));
            }

            return View(evento);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var evento = await EventoRepositorio.PegarPeloId(id);
            if (evento == null)
            {
                return NotFound();
            }
            
            return View(evento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventoId,Nome,Data,UsuarioId")] Evento evento)
        {
            if (id != evento.EventoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await EventoRepositorio.Atualizar(evento);
                TempData["Atualizacao"] = $"Evento {evento.Nome} atualizado com sucesso";
                return RedirectToAction(nameof(Index));
            }

            return View(evento);
        }

        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            await EventoRepositorio.Excluir(id);
            TempData["Exclusao"] = "Evento excluido";

            return Json("Evento excluido");
        }

    }
}
