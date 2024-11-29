using projetoFinal.Dominio.Entidades;
using projetoFinal.Repositorio.Context;
using projetoFinal.Repositorio.Interfaces;

public class ProdutoRepositorio : IProdutoRepositorio
{
    readonly private Contexto _contexto;
    public ProdutoRepositorio(Contexto contexto)
    {
        _contexto = contexto;
    }

    public int Criar(Produto produto)
    {
        _contexto.Produtos.Add(produto);
        _contexto.SaveChanges();

        return produto.Id;
    }
    public void Atualizar(Produto produto)
    {
        _contexto.Produtos.Update(produto);
        _contexto.SaveChanges();
    }
    public Produto Obter(int produtoId, bool ativo)
    {
        return _contexto.Produtos
                    .Where(u => u.Id == produtoId)
                    .Where(u => u.Ativo == ativo)
                    .FirstOrDefault();
    }

    public void Excluir(Produto produto)
    {
        produto.Excluir();
        _contexto.Produtos.Update(produto);
        _contexto.SaveChanges();
    }

    public void Restaurar(Produto produto)
    {
        produto.Restaurar();
        _contexto.Produtos.Update(produto);
        _contexto.SaveChanges();
    }

    public IEnumerable<Produto> Listar(bool ativo)
    {
        return _contexto.Produtos.Where(u => u.Ativo == ativo).ToList();
    }
}
