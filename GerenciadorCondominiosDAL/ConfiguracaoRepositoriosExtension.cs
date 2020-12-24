using GerenciadorCondominiosDAL.Interfaces;
using GerenciadorCondominiosDAL.Repositorios;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace GerenciadorCondominiosDAL
{
    public static class ConfiguracaoRepositoriosExtension
    {
        public static void ConfigurarRepositorios(this IServiceCollection services)
        {
            services.AddTransient<IUsuarioRepositorio, UsuarioRepositorio>();
            services.AddTransient<IFuncaoRepositorio, FuncaoRepositorio>();
            services.AddTransient<IVeiculoRepositorio, VeiculoRepositorio>();
            services.AddTransient<IEventoRepositorio, EventoRepositorio>();
            services.AddTransient<IServicoRepositorio, ServicoRepositorio>();
            services.AddTransient<IServicoPredioRepositorio, ServicoPredioRepositorio>();
            services.AddTransient<IHistoricoRecursoRepositorio, HistoricoRecursoRepositorio>();
            services.AddTransient<IApartamentoRepositorio, ApartamentoRepositorio>();
            services.AddTransient<IMesRepositorio, MesRepositorio>();
            services.AddTransient<IAluguelRepositorio, AluguelRepositorio>();
            services.AddTransient<IPagamentoRepositorio, PagamentoRepositorio>();
        }
    }
}
