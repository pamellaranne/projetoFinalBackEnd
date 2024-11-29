using projetoFinal.Dominio.Entidades;
using projetoFinal.Repositorio.Interfaces;


namespace projetoFinal.Aplicacao
{
    public class ProdutoAplicacao : IProdutoAplicacao
    {
        readonly IProdutoRepositorio _produtoRepositorio;

        public ProdutoAplicacao(IProdutoRepositorio produtoRepositorio)
        {
            _produtoRepositorio = produtoRepositorio;
        }

        public int Criar(Produto produto)
        {
            if (produto == null)
                throw new Exception("Produto não pode ser nulo.");

            return _produtoRepositorio.Criar(produto);

        }

        public void Atualizar(Produto produto)
        {
            var produtoDominio = _produtoRepositorio.Obter(produto.Id, true);

            if (produtoDominio == null)
                throw new Exception("Produto não encontrado.");

            produtoDominio.Nome = produto.Nome;
            produtoDominio.Quantidade = produto.Quantidade;
            produtoDominio.TiposCategoriasId = produto.TiposCategoriasId;

            _produtoRepositorio.Atualizar(produtoDominio);
        }

        public Produto Obter(int produtoId, bool ativo)
        {
            var produtoDominio = _produtoRepositorio.Obter(produtoId, ativo);

            if (produtoDominio == null)
                throw new Exception("Produto não encontrado.");

            return produtoDominio;
        }

        public void Excluir(int produtoId)
        {
            var produtoDominio = _produtoRepositorio.Obter(produtoId, true);

            if (produtoDominio == null)
                throw new Exception("Produto não encontrado.");


            _produtoRepositorio.Excluir(produtoDominio);
        }

        public void Restaurar(int produtoId)
        {
            var produtoDominio = _produtoRepositorio.Obter(produtoId, false);

            if (produtoDominio == null)
                throw new Exception("Produto não encontrado.");


            _produtoRepositorio.Restaurar(produtoDominio);
        }

         public IEnumerable<Produto> Listar(bool ativo)
        {
            return _produtoRepositorio.Listar(ativo);
        }
    }
}
