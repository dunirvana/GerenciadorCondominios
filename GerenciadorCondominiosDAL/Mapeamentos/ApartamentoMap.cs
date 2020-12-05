using GerenciadorCondominiosBLL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GerenciadorCondominiosDAL.Mapeamentos
{
    public class ApartamentoMap : IEntityTypeConfiguration<Apartamento>
    {
        public void Configure(EntityTypeBuilder<Apartamento> builder)
        {
            builder.HasKey(a => a.ApartamentoId);
            builder.Property(a => a.Numero).IsRequired();
            builder.Property(a => a.Andar).IsRequired();
            builder.Property(a => a.Foto).IsRequired();
            builder.Property(a => a.ProprietarioId).IsRequired();
            builder.Property(a => a.MoradorId).IsRequired(false);

            builder.HasOne(a => a.Proprietario).WithMany(a => a.ProprietariosApartamento).HasForeignKey(a => a.ProprietarioId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(a => a.Morador).WithMany(a => a.MoradoresApartamento).HasForeignKey(a => a.MoradorId).HasForeignKey(a => a.ProprietarioId).OnDelete(DeleteBehavior.NoAction);

            builder.ToTable("Apartamentos");
        }
    }
}
