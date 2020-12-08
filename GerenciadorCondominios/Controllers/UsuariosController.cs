using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GerenciadorCondominios.ViewModels;
using GerenciadorCondominiosBLL.Models;
using GerenciadorCondominiosDAL.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GerenciadorCondominios.Controllers
{    
    public class UsuariosController : Controller
    {
        private readonly IUsuarioRepositorio UsuarioRepositorio;
        private readonly IFuncaoRepositorio FuncaoRepositorio;
        private readonly IWebHostEnvironment WebHostEnvironment;

        public UsuariosController(IUsuarioRepositorio usuarioRepositorio, IWebHostEnvironment webHostEnvironment, IFuncaoRepositorio funcaoRepositorio)
        {
            UsuarioRepositorio = usuarioRepositorio;
            WebHostEnvironment = webHostEnvironment;
            FuncaoRepositorio = funcaoRepositorio;
        }

        public async Task<IActionResult> Index()
        {
            return View(await UsuarioRepositorio.PegarTodos());
        }

        #region Registro

        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Registro(RegistroViewModel model, IFormFile foto)
        {
            if (ModelState.IsValid)
            {
                model.Foto = GerarNomeFoto(foto);
                var usuario = PreencherUsuario(model);


                var cadastrouAdministrador = await CadastrarAdministrador(usuario, model.Senha);
                if (cadastrouAdministrador)
                {
                    await GravarFoto(foto, model.Foto);
                    return RedirectToAction("Index", "Usuarios");
                }

                var cadastroUsuarioEmAnalise = await CadastrarUsuarioEmAnalise(usuario, model.Senha);
                if (cadastroUsuarioEmAnalise)
                {
                    await GravarFoto(foto, model.Foto);
                    return View("Analise", usuario.UserName);
                }
                else
                    return View(model);                
            }

            return View(model);
        }

        private Usuario PreencherUsuario(RegistroViewModel model)
        {
            Usuario usuario = new Usuario
            {
                UserName = model.Nome,
                Cpf = model.CPF,
                Email = model.Email,
                PhoneNumber = model.Telefone,
                Foto = model.Foto
            };

            return usuario;
        }

        private string GerarNomeFoto(IFormFile foto)
        {
            return Guid.NewGuid().ToString() + foto.FileName;
        }
        private async Task GravarFoto(IFormFile foto, string nomFoto)
        {
            if (foto != null)
            {
                string dirImagens = Path.Combine(WebHostEnvironment.WebRootPath, "Imagens");
                
                using (FileStream fileStream = new FileStream(Path.Combine(dirImagens, nomFoto), FileMode.Create))
                {
                    await foto.CopyToAsync(fileStream);
                }
            }
        }

        private async Task<bool> CadastrarAdministrador(Usuario usuario, string senha)
        {
            if (UsuarioRepositorio.VerificarSeExisteRegistro() == 0)
            {
                usuario.PrimeiroAcesso = false;
                usuario.Status = StatusConta.Aprovado;

                var usuarioCriado = await UsuarioRepositorio.CriarUsuario(usuario, senha);
                if (usuarioCriado.Succeeded)
                {
                    await UsuarioRepositorio.IncluirUsuarioEmFuncao(usuario, "Administrador");
                    await UsuarioRepositorio.LogarUsuario(usuario, false);

                    return true;    
                }
            }

            return false;
        }

        private async Task<bool> CadastrarUsuarioEmAnalise(Usuario usuario, string senha)
        {
            usuario.PrimeiroAcesso = true;
            usuario.Status = StatusConta.Analisando;

            var usuarioCriado = await UsuarioRepositorio.CriarUsuario(usuario, senha);
            if (usuarioCriado.Succeeded)
                return true;
            else
                foreach (var error in usuarioCriado.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            return false;
        }

        #endregion Registro

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            if (User.Identity.IsAuthenticated)
                await UsuarioRepositorio.DeslogarUsuario();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await UsuarioRepositorio.DeslogarUsuario();

            return RedirectToAction("Login");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var usuario = await UsuarioRepositorio.PegarUsuarioPeloEmail(model.Email);
                if (usuario != null)
                {
                    if (usuario.Status == StatusConta.Analisando)
                    {
                        return View("Analise", usuario.UserName);
                    }
                    else if (usuario.Status == StatusConta.Reprovado)
                    {
                        return View("Reprovado", usuario.UserName);
                    }
                    else if (usuario.PrimeiroAcesso)
                    {
                        return View("RedefinirSenha", usuario);
                    }
                    else
                    {
                        var passwordHasher = new PasswordHasher<Usuario>();
                        if (passwordHasher.VerifyHashedPassword(usuario, usuario.PasswordHash, model.Senha) != PasswordVerificationResult.Failed)
                        {
                            await UsuarioRepositorio.LogarUsuario(usuario, false);
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Email e/ou senha inválidos");
                            return View(model);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Email e/ou senha inválidos");
                    return View(model);
                }
            }

            return View(model);
        }


        public IActionResult Analise(string nome)
        {
            return View(nome);
        }

        public IActionResult Reprovado(string nome)
        {
            return View(nome);
        }

        public async Task<JsonResult> AprovarUsuario(string usuarioId)
        {
            var usuario = await UsuarioRepositorio.PegarPeloId(usuarioId);
            usuario.Status = StatusConta.Aprovado;

            await UsuarioRepositorio.IncluirUsuarioEmFuncao(usuario, "Morador");

            await UsuarioRepositorio.AtualizarUsuario(usuario);

            return Json(true);
        }

        public async Task<JsonResult> ReprovarUsuario(string usuarioId)
        {
            var usuario = await UsuarioRepositorio.PegarPeloId(usuarioId);
            usuario.Status = StatusConta.Reprovado;

            await UsuarioRepositorio.AtualizarUsuario(usuario);

            return Json(true);
        }

        [HttpGet]
        public async Task<IActionResult> GerenciarUsuario(string usuarioId, string nome)
        {
            if (usuarioId == null)
                return NotFound();

            TempData["usuarioId"] = usuarioId;
            ViewBag.Nome = nome;

            var usuario = await UsuarioRepositorio.PegarPeloId(usuarioId);
            if (usuario == null)
                return NotFound();

            var viewModel = new List<FuncaoUsuarioViewModel>();
            foreach (var funcao in await FuncaoRepositorio.PegarTodos())
            {
                var model = new FuncaoUsuarioViewModel
                {
                    FuncaoId = funcao.Id,
                    Nome = funcao.Name,
                    Descricao = funcao.Descricao,
                    IsSelecionado = false
                };

                if (await UsuarioRepositorio.VerificarSeUsuarioEstaEmFuncao(usuario, funcao.Name))
                    model.IsSelecionado = true;


                viewModel.Add(model);
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> GerenciarUsuario(List<FuncaoUsuarioViewModel> model)
        {
            string usuarioId = TempData["usuarioId"].ToString();

            var usuario = await UsuarioRepositorio.PegarPeloId(usuarioId);
            if (usuario == null)
                return NotFound();

            var funcoes = await UsuarioRepositorio.PegarFuncoesUsuario(usuario);
            var resultado = await UsuarioRepositorio.RemoverFuncoesUsuario(usuario, funcoes);

            if (!resultado.Succeeded)
            {
                ModelState.AddModelError("", "Não foi possível atualizar as funções do usuário");
                return View("GerenciarUsuario", usuarioId);
            }

            resultado = await UsuarioRepositorio.IncluirUsuarioEmFuncoes(usuario, model.Where(x => x.IsSelecionado).Select(x => x.Nome));

            if (!resultado.Succeeded)
            {
                ModelState.AddModelError("", "Não foi possível atualizar as funções do usuário");
                return View("GerenciarUsuario", usuarioId);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
