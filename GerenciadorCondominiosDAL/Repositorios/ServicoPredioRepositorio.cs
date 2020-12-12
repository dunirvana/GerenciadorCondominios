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
    public class ServicoPredioRepositorio : RepositorioGenerico<ServicoPredio>, IServicoPredioRepositorio
    {

        public ServicoPredioRepositorio(Contexto contexto) : base(contexto)
        {
        }


    }
}
