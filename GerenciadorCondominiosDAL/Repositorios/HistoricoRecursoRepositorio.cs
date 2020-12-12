﻿using GerenciadorCondominiosBLL.Models;
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

        public HistoricoRecursoRepositorio(Contexto contexto) : base(contexto)
        {
        }


    }
}
