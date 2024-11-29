using projetoFinal.Dominio.Enumeradores;

namespace projetoFinal.Dominio.Entidades
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Quantidade { get; set; }
        public bool Ativo { get; set; }
        public TiposCategoriasEnum TiposCategoriasId { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public Produto()
        {
            Ativo = true;
        }
        public void Excluir()
        {
            Ativo = false;
        }
        public void Restaurar()
        {
            Ativo = true;
        }
    }
}
    

