using GerenciadorCondominiosBLL.Models;
using GerenciadorCondominiosDAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorCondominiosDAL.Repositorios
{
    public class FuncaoRepositorio : RepositorioGenerico<Funcao>, IFuncaoRepositorio
    {
        private readonly RoleManager<Funcao> RoleManager;

        public FuncaoRepositorio(Contexto contexto, RoleManager<Funcao> roleManager) : base(contexto)
        {
            RoleManager = roleManager;
        }

        public async Task AdicionarFuncao(Funcao funcao)
        {
            try
            {
                funcao.Id = Guid.NewGuid().ToString();

                await RoleManager.CreateAsync(funcao);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public new async Task Atualizar(Funcao funcao)
        {
            try
            {
                var f = await PegarPeloId(funcao.Id);

                f.Name = funcao.Name;
                f.Descricao = funcao.Descricao;
                f.NormalizedName = funcao.NormalizedName;

                await RoleManager.UpdateAsync(f);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
