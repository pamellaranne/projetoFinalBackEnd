namespace projetoFinal.Dominio.Entidades
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public bool Ativo { get; set; }
        public List <Produto> Produtos { get; set; } 
        public string TokenRedefinicao { get; set; }
        public DateTime? DataExpiracaoToken { get; set; }
        public Usuario()
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
    

