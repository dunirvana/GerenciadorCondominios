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
    public class PagamentosController : Controller
    {
        private readonly IPagamentoRepositorio PagamentoRepositorio;
        private readonly IAluguelRepositorio AluguelRepositorio;
        private readonly IHistoricoRecursoRepositorio HistoricoRecursoRepositorio;
        private readonly IUsuarioRepositorio UsuarioRepositorio;

        public PagamentosController(IPagamentoRepositorio pagamentoRepositorio, IAluguelRepositorio aluguelRepositorio, IHistoricoRecursoRepositorio historicoRecursoRepositorio, IUsuarioRepositorio usuarioRepositorio)
        {
            PagamentoRepositorio = pagamentoRepositorio;
            AluguelRepositorio = aluguelRepositorio;
            HistoricoRecursoRepositorio = historicoRecursoRepositorio;
            UsuarioRepositorio = usuarioRepositorio;
        }

        public async Task<IActionResult> Index()
        {
            var usuario = await UsuarioRepositorio.PegarUsuarioPeloNome(User);
            return View(await PagamentoRepositorio.PegarPagamentosPorUsuario(usuario.Id));
        }

        public async Task<IActionResult> EfetuarPagamento(int id)
        {
            var pagamento = await PagamentoRepositorio.PegarPeloId(id);
            pagamento.DataPagamento = DateTime.Now.Date;
            pagamento.Status = StatusPagamento.Pago;

            await PagamentoRepositorio.Atualizar(pagamento);

            var aluguel = await AluguelRepositorio.PegarPeloId(pagamento.AluguelId);
            var hr = new HistoricoRecurso
            {
                Valor = aluguel.Valor,
                MesId = aluguel.MesId,
                Dia = DateTime.Now.Day,
                Ano = aluguel.Ano,
                Tipo = Tipo.Entrada
            };

            await HistoricoRecursoRepositorio.Inserir(hr);
            TempData["NovoRegistro"] = $"Pagamento no valor de {pagamento.Aluguel.Valor} realizado";

            return RedirectToAction(nameof(Index));
        }
    }
}
