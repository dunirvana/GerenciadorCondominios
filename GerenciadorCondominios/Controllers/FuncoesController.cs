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
    public class FuncoesController : Controller
    {
        private readonly IFuncaoRepositorio FuncaoRepositorio;
        
        public FuncoesController(IFuncaoRepositorio funcaoRepositorio)
        {
            FuncaoRepositorio = funcaoRepositorio;
        }

        // GET: Funcoes
        public async Task<IActionResult> Index()
        {
            return View(await FuncaoRepositorio.PegarTodos());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Descricao,Id,Name,NormalizedName,ConcurrencyStamp")] Funcao funcao)
        {
            if (ModelState.IsValid)
            {                
                await FuncaoRepositorio.AdicionarFuncao(funcao);
                TempData["NovoRegistro"] = $"Função {funcao.Name} adicionado";
                return RedirectToAction(nameof(Index));
            }
            return View(funcao);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var funcao = await FuncaoRepositorio.PegarPeloId(id);
            if (funcao == null)
            {
                return NotFound();
            }
            return View(funcao);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Descricao,Id,Name,NormalizedName,ConcurrencyStamp")] Funcao funcao)
        {
            if (id != funcao.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await FuncaoRepositorio.Atualizar(funcao);
                TempData["Atualizacao"] = $"Função {funcao.Name} atualizada";
                return RedirectToAction(nameof(Index));
            }
            return View(funcao);
        }

        [HttpPost]
        public async Task<JsonResult> Delete(string id)
        {
            await FuncaoRepositorio.Excluir(id);

            var mensagem = "Função excluida";
            TempData["Exclusao"] = mensagem;

            return Json(mensagem);
        }
    }
}
