using GerenciadorCondominiosBLL.Models;
using GerenciadorCondominiosDAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorCondominiosDAL.Repositorios
{
    public class HistoricoRecursoRepositorio : RepositorioGenerico<HistoricoRecurso>, IHistoricoRecursoRepositorio
    {

        private readonly Contexto Contexto;
        public HistoricoRecursoRepositorio(Contexto contexto) : base(contexto)
        {
            Contexto = contexto;
        }

        public object PegarHistoricoDespesas(int ano)
        {
            try
            {
                return Contexto.HistoricoRecursos.Include(hr => hr.Mes)
                    .Where(hr => hr.Ano == ano && hr.Tipo == Tipo.Saida).ToList()
                    .OrderBy(hr => hr.MesId).GroupBy(hr => hr.Mes.Nome)
                    .Select(hr => new { Meses = hr.Key, Valores = hr.Sum(v => v.Valor) });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public object PegarHistoricoGanhos(int ano)
        {
            try
            {
                return Contexto.HistoricoRecursos.Include(hr => hr.Mes)
                    .Where(hr => hr.Ano == ano && hr.Tipo == Tipo.Entrada).ToList()
                    .OrderBy(hr => hr.MesId).GroupBy(hr => hr.Mes.Nome)
                    .Select(hr => new { Meses = hr.Key, Valores = hr.Sum(v => v.Valor) });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<decimal> PegarSomaDespesas(int ano)
        {
            try
            {
                return await Contexto.HistoricoRecursos.Include(hr => hr.Mes)
                    .Where(hr => hr.Ano == ano && hr.Tipo == Tipo.Saida)
                    .SumAsync(hr => hr.Valor);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<decimal> PegarSomaGanhos(int ano)
        {
            try
            {
                return await Contexto.HistoricoRecursos.Include(hr => hr.Mes)
                    .Where(hr => hr.Ano == ano && hr.Tipo == Tipo.Entrada)
                    .SumAsync(hr => hr.Valor);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<IEnumerable<HistoricoRecurso>> PegarUltimasMovimentacoes()
        {
            try
            {
                return await Contexto.HistoricoRecursos.Include(hr => hr.Mes).OrderByDescending(hr => hr.HistoricoRecursoId)
                    .Take(5).ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
