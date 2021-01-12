using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GerenciadorCondominiosBLL.Models;
using GerenciadorCondominiosDAL;
using Microsoft.AspNetCore.Hosting;
using GerenciadorCondominiosDAL.Interfaces;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace GerenciadorCondominios.Controllers
{
    [Authorize(Roles = "Administrador,Sindico")]
    public class ApartamentosController : Controller
    {
        private readonly IWebHostEnvironment WebHostEnvironment;
        private readonly IApartamentoRepositorio ApartamentoRepository;
        private readonly IUsuarioRepositorio UsuarioRepositorio;

        public ApartamentosController(IWebHostEnvironment webHostEnvironment, IApartamentoRepositorio apartamentoRepository, IUsuarioRepositorio usuarioRepositorio)
        {
            WebHostEnvironment = webHostEnvironment;
            ApartamentoRepository = apartamentoRepository;
            UsuarioRepositorio = usuarioRepositorio;
        }

        // GET: Apartamentos
        public async Task<IActionResult> Index()
        {
            var usuario = await UsuarioRepositorio.PegarUsuarioPeloNome(User);
            if (await UsuarioRepositorio.VerificarSeUsuarioEstaEmFuncao(usuario, "Sindico"))
            {
                return View(await ApartamentoRepository.PegarTodos());
            }

            return View(await ApartamentoRepository.PegarApartamentoPeloUsuario(usuario.Id));
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["MoradorId"] = new SelectList(await UsuarioRepositorio.PegarTodos(), "Id", "UserName");
            ViewData["ProprietarioId"] = new SelectList(await UsuarioRepositorio.PegarTodos(), "Id", "UserName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ApartamentoId,Numero,Andar,Foto,MoradorId,ProprietarioId")] Apartamento apartamento, IFormFile foto)
        {
            if (ModelState.IsValid)
            {
                if (foto != null)
                {
                    var diretorio = Path.Combine(WebHostEnvironment.WebRootPath, "Imagens");
                    var nomeFoto = Guid.NewGuid().ToString() + foto.FileName;

                    using (var fileStream = new FileStream(Path.Combine(diretorio, nomeFoto), FileMode.Create))
                    {
                        await foto.CopyToAsync(fileStream);

                        apartamento.Foto = "~/Imagens/" + nomeFoto;
                    }
                }

                await ApartamentoRepository.Inserir(apartamento);
                TempData["NovoRegistro"] = $"Apartamento número {apartamento.Numero} registrado com sucesso";
                return RedirectToAction(nameof(Index));
            }

            TempData["Foto"] = apartamento.Foto;
            ViewData["MoradorId"] = new SelectList(await UsuarioRepositorio.PegarTodos(), "Id", "UserName", apartamento.MoradorId);
            ViewData["ProprietarioId"] = new SelectList(await UsuarioRepositorio.PegarTodos(), "Id", "UserName", apartamento.ProprietarioId);

            return View(apartamento);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var apartamento = await ApartamentoRepository.PegarPeloId(id);
            if (apartamento == null)
            {
                return NotFound();
            }

            TempData["Foto"] = apartamento.Foto;
            ViewData["MoradorId"] = new SelectList(await UsuarioRepositorio.PegarTodos(), "Id", "UserName", apartamento.MoradorId);
            ViewData["ProprietarioId"] = new SelectList(await UsuarioRepositorio.PegarTodos(), "Id", "UserName", apartamento.ProprietarioId);

            return View(apartamento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ApartamentoId,Numero,Andar,Foto,MoradorId,ProprietarioId")] Apartamento apartamento, IFormFile foto)
        {
            if (id != apartamento.ApartamentoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (foto != null)
                {
                    var diretorio = Path.Combine(WebHostEnvironment.WebRootPath, "Imagens");
                    var nomeFoto = Guid.NewGuid().ToString() + foto.FileName;

                    using (var fileStream = new FileStream(Path.Combine(diretorio, nomeFoto), FileMode.Create))
                    {
                        await foto.CopyToAsync(fileStream);

                        apartamento.Foto = "~/Imagens/" + nomeFoto;
                        System.IO.File.Delete(TempData["Foto"].ToString().Replace("~", "wwwroot"));
                    }
                }
                else
                {
                    apartamento.Foto = TempData["Foto"].ToString();
                }

                await ApartamentoRepository.Atualizar(apartamento);
                TempData["Atualizacao"] = $"Apartamento número {apartamento.Numero} atualizado com sucesso";

                return RedirectToAction(nameof(Index));
            }
            ViewData["MoradorId"] = new SelectList(await UsuarioRepositorio.PegarTodos(), "Id", "UserName", apartamento.MoradorId);
            ViewData["ProprietarioId"] = new SelectList(await UsuarioRepositorio.PegarTodos(), "Id", "UserName", apartamento.ProprietarioId);

            return View(apartamento);
        }

        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            var apartamento = await ApartamentoRepository.PegarPeloId(id);
            System.IO.File.Delete(apartamento.Foto.Replace("~", "wwwroot"));

            await ApartamentoRepository.Excluir(apartamento);

            var mensagem = $"Apartamento número {apartamento.Numero} excluido com sucesso";
            TempData["Exclusao"] = mensagem;

            return Json(mensagem);
        }

    }
}
