using GerenciadorCondominiosBLL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GerenciadorCondominiosDAL.Mapeamentos
{
    public class ServicoPredioMap : IEntityTypeConfiguration<ServicoPredio>
    {
        public void Configure(EntityTypeBuilder<ServicoPredio> builder)
        {
            builder.HasKey(s => s.ServicoPredioId);
            builder.Property(s => s.ServicoId).IsRequired();
            builder.Property(s => s.DataExecucao).IsRequired();

            builder.HasOne(s => s.Servico).WithMany(s => s.ServicoPredios).HasForeignKey(s => s.ServicoId);

            builder.ToTable("ServicoPredios");
        }
    }
}
