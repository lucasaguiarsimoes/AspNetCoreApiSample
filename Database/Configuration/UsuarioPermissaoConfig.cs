using AspNetCoreApiSample.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspNetCoreApiSample.Database.Configuration
{
    public class UsuarioPermissaoConfig : IEntityTypeConfiguration<UsuarioPermissao>
    {
        public void Configure(EntityTypeBuilder<UsuarioPermissao> builder)
        {
            // Chaves
            builder.HasKey(p => p.ID);

            builder.HasAlternateKey(p => new { p.UsuarioID, p.Permissao });

            // Propriedades
            builder.Property(p => p.UsuarioID)
                .IsRequired();

            builder.Property(p => p.Permissao)
                .IsRequired();

            // Índices
            builder.HasIndex(p => p.UsuarioID);

            // Relacionamentos
            builder
                .HasOne(p => p.Usuario)
                .WithMany(u => u.Permissoes)
                .HasForeignKey(p => p.UsuarioID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
