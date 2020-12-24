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
using GerenciadorCondominiosDAL.Repositorios;

namespace GerenciadorCondominios.Controllers
{
    public class AlugueisController : Controller
    {
        private readonly IAluguelRepositorio AluguelRepositorio;
        private readonly IUsuarioRepositorio UsuariosRepositorio;
        private readonly IPagamentoRepositorio PagamentoRepositorio;
        private readonly IMesRepositorio MesRepositorio;

        public AlugueisController(IAluguelRepositorio aluguelRepositorio, IUsuarioRepositorio usuariosRepositorio, IPagamentoRepositorio pagamentoRepositorio, IMesRepositorio mesRepositorio)
        {
            AluguelRepositorio = aluguelRepositorio;
            UsuariosRepositorio = usuariosRepositorio;
            PagamentoRepositorio = pagamentoRepositorio;
            MesRepositorio = mesRepositorio;
        }


        public async Task<IActionResult> Index()
        {            
            return View(await AluguelRepositorio.PegarTodos());
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["MesId"] = new SelectList(await MesRepositorio.PegarTodos(), "MesId", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AluguelId,Valor,MesId,Ano")] Aluguel aluguel)
        {
            if (ModelState.IsValid)
            {
                if (!AluguelRepositorio.AluguelJaExiste(aluguel.MesId, aluguel.Ano))
                {
                    await AluguelRepositorio.Inserir(aluguel);

                    var usuarios = await UsuariosRepositorio.PegarTodos();
                    Pagamento pagamento;
                    var pagamentos = new List<Pagamento>();

                    foreach (var u in usuarios)
                    {
                        pagamento = new Pagamento
                        {
                            AluguelId = aluguel.AluguelId,
                            UsuarioId = u.Id,
                            DataPagamento = null,
                            Status = StatusPagamento.Pendente
                        };

                        pagamentos.Add(pagamento);
                    }                    

                    await PagamentoRepositorio.Inserir(pagamentos);
                    TempData["NovoRegistro"] = $"Aluguel no valor de {aluguel.Valor} do mês {aluguel.MesId} do ano {aluguel.Ano} adicionado";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Aluguel já existe");
                    ViewData["MesId"] = new SelectList(await MesRepositorio.PegarTodos(), "MesId", "Nome");
                    return View(aluguel);
                }

            }

            ViewData["MesId"] = new SelectList(await MesRepositorio.PegarTodos(), "MesId", "Nome");
            return View(aluguel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var aluguel = await AluguelRepositorio.PegarPeloId(id);
            if (aluguel == null)
            {
                return NotFound();
            }
            ViewData["MesId"] = new SelectList(await MesRepositorio.PegarTodos(), "MesId", "Nome");
            return View(aluguel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AluguelId,Valor,MesId,Ano")] Aluguel aluguel)
        {
            if (id != aluguel.AluguelId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await AluguelRepositorio.Atualizar(aluguel);
                TempData["Atualiacao"] = $"Aluguel do mês {aluguel.MesId} do ano {aluguel.Ano} atualizado";
                return RedirectToAction(nameof(Index));
            }
            ViewData["MesId"] = new SelectList(await MesRepositorio.PegarTodos(), "MesId", "Nome");
            return View(aluguel);
        }

        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            await AluguelRepositorio.Excluir(id);
            var mensagem = "Aluguel excluido";
            TempData["Exclusao"] = mensagem;

            return Json(mensagem);
        }

    }
}
