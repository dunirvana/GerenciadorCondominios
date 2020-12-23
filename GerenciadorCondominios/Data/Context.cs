using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GerenciadorCondominiosBLL.Models;

namespace GerenciadorCondominiosDAL
{
    public class Context : DbContext
    {
        public Context (DbContextOptions<Context> options)
            : base(options)
        {
        }

        public DbSet<GerenciadorCondominiosBLL.Models.Funcao> Funcao { get; set; }
    }
}
