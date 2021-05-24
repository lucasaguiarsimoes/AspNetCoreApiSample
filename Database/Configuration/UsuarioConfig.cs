using AspNetCoreApiSample.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace AspNetCoreApiSample.Database.Configuration
{
    public class UsuarioConfig : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            // Chaves
            builder.HasKey(u => u.ID);

            // Propriedades
            builder.Property(u => u.Codigo)
                .IsRequired();

            builder.Property(u => u.Nome)
                .IsRequired();

            builder.Property(u => u.Email)
                .IsRequired();

            builder.Property(u => u.Senha)
                .IsRequired();

            builder.Property(u => u.DataHoraUltimaAlteracaoSenha)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(u => u.ExpiracaoSenhaAtivada)
                .IsRequired();

            // Índices
            builder.HasIndex(u => u.Codigo)
                .IsUnique();

            // Relacionamentos
            builder
                .HasMany(u => u.Permissoes)
                .WithOne(p => p.Usuario)
                .HasForeignKey(p => p.UsuarioID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
