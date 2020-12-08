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
    public class UsuarioRepositorio : RepositorioGenerico<Usuario>, IUsuarioRepositorio
    {
        private readonly Contexto Contexto;
        private readonly UserManager<Usuario> GerenciadorUsuarios;
        private readonly SignInManager<Usuario> GerenciadorLogin;

        public UsuarioRepositorio(Contexto contexto, UserManager<Usuario> gerenciadorUsuarios, SignInManager<Usuario> gerenciadorLogin) : base(contexto)
        {
            Contexto = contexto;
            GerenciadorUsuarios = gerenciadorUsuarios;
            GerenciadorLogin = gerenciadorLogin;
        }

        public async Task<IdentityResult> CriarUsuario(Usuario usuario, string senha)
        {
            try
            {
                return await GerenciadorUsuarios.CreateAsync(usuario, senha);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task IncluirUsuarioEmFuncao(Usuario usuario, string funcao)
        {
            try
            {
                await GerenciadorUsuarios.AddToRoleAsync(usuario, funcao);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task LogarUsuario(Usuario usuario, bool lembrar)
        {
            try
            {
                await GerenciadorLogin.SignInAsync(usuario, lembrar);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task DeslogarUsuario()
        {
            try
            {
                await GerenciadorLogin.SignOutAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public int VerificarSeExisteRegistro()
        {
            try
            {
                return Contexto.Usuarios.Count();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<Usuario> PegarUsuarioPeloEmail(string email)
        {
            try
            {
                return await GerenciadorUsuarios.FindByEmailAsync(email);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task AtualizarUsuario(Usuario usuario)
        {
            try
            {
                await GerenciadorUsuarios.UpdateAsync(usuario);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<bool> VerificarSeUsuarioEstaEmFuncao(Usuario usuario, string funcao)
        {
            try
            {
                return await GerenciadorUsuarios.IsInRoleAsync(usuario, funcao);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<IEnumerable<string>> PegarFuncoesUsuario(Usuario usuario)
        {
            try
            {
                return await GerenciadorUsuarios.GetRolesAsync(usuario);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<IdentityResult> RemoverFuncoesUsuario(Usuario usuario, IEnumerable<string> funcoes)
        {
            try
            {
                return await GerenciadorUsuarios.RemoveFromRolesAsync(usuario, funcoes);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<IdentityResult> IncluirUsuarioEmFuncoes(Usuario usuario, IEnumerable<string> funcoes)
        {
            try
            {
                return await GerenciadorUsuarios.AddToRolesAsync(usuario, funcoes);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
