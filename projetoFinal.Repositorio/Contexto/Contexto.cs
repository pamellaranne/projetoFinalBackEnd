using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.Data.SqlClient;
using projetoFinal.Repositorio.Configuracoes;
using projetoFinal.Dominio.Entidades;

namespace projetoFinal.Repositorio.Context
{
    public class Contexto : DbContext
    {
        public string stringConexao { get; set; } = "Server=DESKTOP-58UEHOH\\SQLEXPRESS;Database=ProjetoFinal;TrustServerCertificate=true;Trusted_connection=True";
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Produto> Produtos { get; set; }

        private readonly DbContextOptions _options;

        public Contexto() { }
        public Contexto(DbContextOptions options) : base(options)
        {
            _options = options;
        }

        //Configura as opções de conexão com  o banco de dados
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_options == null)
                optionsBuilder.UseSqlServer(stringConexao);
        }

        public IDbConnection CriarConexao()
        {
            return new SqlConnection(stringConexao);
        }

        //Aplica as configurações de entidade para o modelo do banco de dados

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsuarioConfiguracoes());
            modelBuilder.ApplyConfiguration(new ProdutoConfiguracoes());
        }
    }

}
