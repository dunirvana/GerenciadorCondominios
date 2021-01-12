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
using GerenciadorCondominios.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace GerenciadorCondominios.Controllers
{
    [Authorize]
    public class ServicosController : Controller
    {
        private readonly IServicoRepositorio ServicoRepositorio;
        private readonly IUsuarioRepositorio UsuarioRepositorio;
        private readonly IServicoPredioRepositorio ServicoPredioRepositorio;
        private readonly IHistoricoRecursoRepositorio HistoricoRecursoRepositorio;

        public ServicosController(IServicoRepositorio servicoRepositorio, IUsuarioRepositorio usuarioRepositorio, IServicoPredioRepositorio servicoPredioRepositorio, IHistoricoRecursoRepositorio historicoRecursoRepositorio)
        {
            ServicoRepositorio = servicoRepositorio;
            UsuarioRepositorio = usuarioRepositorio;
            ServicoPredioRepositorio = servicoPredioRepositorio;
            HistoricoRecursoRepositorio = historicoRecursoRepositorio;
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

        [Authorize(Roles = "Administrador,Sindico")]
        [HttpGet]
        public async Task<IActionResult> AprovarServico(int id)
        {
            var servico = await ServicoRepositorio.PegarPeloId(id);
            var viewModel = new ServicoAprovadoViewModel
            {
                ServicoId = servico.SevicoId,
                Nome = servico.Nome
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Administrador,Sindico")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AprovarServico(ServicoAprovadoViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var servico = await ServicoRepositorio.PegarPeloId(viewModel.ServicoId);
                servico.Status = StatusServico.Aceito;
                await ServicoRepositorio.Atualizar(servico);

                var servicoPredio = new ServicoPredio
                {
                    ServicoId = viewModel.ServicoId,
                    DataExecucao = viewModel.Data
                };

                await ServicoPredioRepositorio.Inserir(servicoPredio);
                var hr = new HistoricoRecurso
                {
                    Valor = servico.Valor,
                    MesId = servicoPredio.DataExecucao.Month,
                    Dia = servicoPredio.DataExecucao.Day,
                    Ano = servicoPredio.DataExecucao.Year,
                    Tipo = Tipo.Saida
                };

                await HistoricoRecursoRepositorio.Inserir(hr);
                TempData["NovoRegistro"] = $"{servico.Nome} escalado com sucesso";

                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        [Authorize(Roles = "Administrador,Sindico")]
        public async Task<IActionResult> RecusarServico(int id)
        {
            var servico = await ServicoRepositorio.PegarPeloId(id);
            if (servico == null)
                return NotFound();

            servico.Status = StatusServico.Recusado;
            await ServicoRepositorio.Atualizar(servico);
            TempData["Exclusao"] = $"{servico.Nome} recusado";

            return RedirectToAction(nameof(Index));
        }
    }
}
