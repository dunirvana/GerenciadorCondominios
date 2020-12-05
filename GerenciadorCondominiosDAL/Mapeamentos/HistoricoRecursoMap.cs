using GerenciadorCondominiosBLL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GerenciadorCondominiosDAL.Mapeamentos
{
    public class HistoricoRecursoMap : IEntityTypeConfiguration<HistoricoRecurso>
    {
        public void Configure(EntityTypeBuilder<HistoricoRecurso> builder)
        {
            builder.HasKey(h => h.HistoricoRecursoId);
            builder.Property(h => h.Valor).IsRequired();
            builder.Property(h => h.Tipo).IsRequired();
            builder.Property(h => h.Dia).IsRequired();
            builder.Property(h => h.MesId).IsRequired();
            builder.Property(h => h.Ano).IsRequired();

            builder.HasOne(h => h.Mes).WithMany(h => h.HistoricoRecursos).HasForeignKey(h => h.MesId);

            builder.ToTable("HistoricoRecursos");
        }
    }
}
