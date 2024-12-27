using projetoFinal.Dominio.Entidades;

namespace projetoFinal.Repositorio.Interfaces;
public interface IProdutoRepositorio
{
    int Criar(Produto produto);
    void Atualizar(Produto produto);
    Produto Obter(int produtoId, bool ativo);
    void Excluir(Produto produto);
    void Restaurar(Produto produto);
    IEnumerable<Produto> Listar (bool ativo, int usuarioId);
}