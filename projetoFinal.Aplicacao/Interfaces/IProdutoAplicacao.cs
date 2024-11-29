using projetoFinal.Dominio.Entidades;

namespace projetoFinal.Aplicacao
{
    public interface IProdutoAplicacao
    {
        int Criar(Produto produto);
        void Atualizar(Produto produto);
        Produto Obter(int produtoId, bool ativo);
        void Excluir(int produtoId);
        void Restaurar(int produtoId);
        IEnumerable<Produto> Listar (bool ativo);
    }
}