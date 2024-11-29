using Microsoft.AspNetCore.Mvc;
using projetoFinal.Dominio.Entidades;
using projetoFinal.Api.Models.Request;
using projetoFinal.Api.Models.Response;
using projetoFinal.Aplicacao;
using projetoFinal.Dominio.Enumeradores;


namespace projetoFinal.Api
{
    [ApiController]
    [Route("[controller]")]

    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoAplicacao _produtoAplicacao;

        public ProdutoController(IProdutoAplicacao produtoAplicacao)
        {
            _produtoAplicacao = produtoAplicacao;
        }

        [HttpGet]
        [Route("Obter/{produtoId}")]
        public ActionResult Obter([FromRoute] int produtoId, [FromQuery] bool ativo)
        {
            try
            {
                var produtoDominio = _produtoAplicacao.Obter(produtoId, ativo);

                var produtoResposta = new ProdutoResponse()
                {
                    Id = produtoDominio.Id,
                    Nome = produtoDominio.Nome,
                    Quantidade = produtoDominio.Quantidade,
                    TiposCategoriasId = produtoDominio.TiposCategoriasId,
                    UsuarioId = produtoDominio.UsuarioId
                };

                return Ok(produtoResposta);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("Criar")]
        public ActionResult Criar([FromBody] ProdutoCriar produtoCriar)
        {
            try
            {
                var produtoDominio = new Produto()
                {
                    Nome = produtoCriar.Nome,
                    Quantidade = produtoCriar.Quantidade,
                    TiposCategoriasId = produtoCriar.TiposCategoriasId, 
                    UsuarioId = produtoCriar.UsuarioId
                };

                var produtoId = _produtoAplicacao.Criar(produtoDominio);

                return Ok(produtoId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("Atualizar")]
        public ActionResult Atualizar([FromBody] ProdutoAtualizar produtoAtualizar)
        {
            try
            {
                var produtoDominio = new Produto()
                {
                    Id = produtoAtualizar.Id,
                    Nome = produtoAtualizar.Nome,
                    Quantidade = produtoAtualizar.Quantidade,
                    TiposCategoriasId = produtoAtualizar.TiposCategoriasId,
                    UsuarioId = produtoAtualizar.UsuarioId
                };

                _produtoAplicacao.Atualizar(produtoDominio);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar: {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("Deletar/{produtoId}")]
        public ActionResult Deletar([FromRoute] int produtoId)
        {
            try
            {
                _produtoAplicacao.Excluir(produtoId);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("Restaurar/{produtoId}")]
        public ActionResult Restaurar([FromRoute] int produtoId)
        {
            try
            {
                _produtoAplicacao.Restaurar(produtoId);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar: {ex.Message}");
            }
        }

         [HttpGet]
        [Route("Listar")]
        public ActionResult List([FromQuery] bool ativos)
        {
            try
            {
                var produtosDominio = _produtoAplicacao.Listar(ativos);

                var produtos = produtosDominio.Select(produto => new ProdutoResponse()
                {
                    Id = produto.Id,
                    Nome = produto.Nome,
                    Quantidade = produto.Quantidade,
                    TiposCategoriasId = produto.TiposCategoriasId,
                    UsuarioId = produto.UsuarioId
                }).ToList();

                return Ok(produtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar: {ex.Message}");
            }
        }

        [HttpGet("ListarTiposCategorias")]
        public async Task<ActionResult> ListarTiposCategorias()
        {
            try
            {
                await Task.Delay(100);

                var valores = Enum.GetValues<TiposCategoriasEnum>().Cast<int>().ToList();
                var nomes = Enum.GetNames<TiposCategoriasEnum>().ToList();
                var listaTipos = new List<object>();

                for (int i = 0; i < valores.Count(); i++)
                {
                    listaTipos.Add(new
                    {
                        id = valores[i],
                        nome = nomes[i]
                    });
                }

                return Ok(listaTipos);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Erro ao listar tipos de categorias: " + ex.Message });
            }
        }
    }
}