using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using projetoFinal.Dominio.Entidades;


namespace projetoFinal.Repositorio.Configuracoes
{

    public class UsuarioConfiguracoes : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios").HasKey(x => x.Id);

            builder.Property(nameof(Usuario.Id)).HasColumnName("UsuarioId");
            builder.Property(nameof(Usuario.Nome)).HasColumnName("Nome").IsRequired(true);
            builder.Property(nameof(Usuario.Email)).HasColumnName("Email").IsRequired(true);
            builder.Property(nameof(Usuario.Senha)).HasColumnName("Senha").IsRequired(true);
            builder.Property(nameof(Usuario.Ativo)).HasColumnName("Ativo").IsRequired(true);
            builder.Property(nameof(Usuario.TokenRedefinicao)).HasColumnName("TokenRedefinicao").HasMaxLength(255);
            builder.Property(nameof(Usuario.DataExpiracaoToken)).HasColumnName("DataExpiracaoToken").HasColumnType("datetime");
        }
    }
}
