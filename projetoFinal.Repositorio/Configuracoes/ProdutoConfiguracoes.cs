using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using projetoFinal.Dominio.Entidades;


namespace projetoFinal.Repositorio.Configuracoes
{

    public class ProdutoConfiguracoes : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.ToTable("Produtos").HasKey(x => x.Id);

            builder.Property(nameof(Produto.Id)).HasColumnName("ProdutoId");
            builder.Property(nameof(Produto.Nome)).HasColumnName("Nome").IsRequired(true);
            builder.Property(nameof(Produto.Quantidade)).HasColumnName("Quantidade").IsRequired(true);
            builder.Property(nameof(Produto.Ativo)).HasColumnName("Ativo").IsRequired(true);
            builder.Property(nameof(Produto.TiposCategoriasId)).HasColumnName("TiposCategoria").IsRequired(true);

            builder.Property(p => p.UsuarioId).HasColumnName("UsuarioId").IsRequired(true);

            // Relacionamento com Usuario
            builder.HasOne(p => p.Usuario)
                   .WithMany(u => u.Produtos)
                   .HasForeignKey(p => p.UsuarioId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}