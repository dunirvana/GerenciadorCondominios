using GerenciadorCondominiosBLL.Models;
using GerenciadorCondominiosDAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
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

        public Task<IdentityResult> CriarUsuario(Usuario usuario, string senha)
        {
            throw new NotImplementedException();
        }

        public Task IncluirUsuarioEmFuncao(Usuario usuario, string funcao)
        {
            throw new NotImplementedException();
        }

        public Task LogarUsuario(Usuario usuario, bool lembrar)
        {
            throw new NotImplementedException();
        }

        public int VerificarSeExisteRegistro()
        {
            throw new NotImplementedException();
        }
    }
}
