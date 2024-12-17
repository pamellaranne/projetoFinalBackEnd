using Microsoft.AspNetCore.Mvc;
using projetoFinal.Dominio.Entidades;
using projetoFinal.Api.Models.Request;
using projetoFinal.Api.Models.Response;
using projetoFinal.Aplicacao;


namespace projetoFinal.Api
{
    [ApiController]
    [Route("[controller]")]

    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioAplicacao _usuarioAplicacao;

        public UsuarioController(IUsuarioAplicacao usuarioAplicacao)
        {
            _usuarioAplicacao = usuarioAplicacao;
        }

        [HttpGet]
        [Route("Obter/{usuarioId}")]
        public ActionResult Obter([FromRoute] int usuarioId, [FromQuery] bool ativo)
        {
            try
            {
                var usuarioDominio = _usuarioAplicacao.Obter(usuarioId, ativo);

                var usuarioResposta = new UsuarioResponse()
                {
                    Id = usuarioDominio.Id,
                    Nome = usuarioDominio.Nome,
                    Email = usuarioDominio.Email
                };

                return Ok(usuarioResposta);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("Criar")]
        public ActionResult Criar([FromBody] UsuarioCriar usuarioCriar)
        {
            try
            {
                var usuarioDominio = new Usuario()
                {
                    Nome = usuarioCriar.Nome,
                    Email = usuarioCriar.Email,
                    Senha = usuarioCriar.Senha,
                };

                var usuarioId = _usuarioAplicacao.Criar(usuarioDominio);

                return Ok(usuarioId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("Atualizar")]
        public ActionResult Atualizar([FromBody] UsuarioAtualizar usuarioAtualizar)
        {
            try
            {
                var usuarioDominio = new Usuario()
                {
                    Id = usuarioAtualizar.Id,
                    Nome = usuarioAtualizar.Nome,
                    Email = usuarioAtualizar.Email,
                    Senha = usuarioAtualizar.Senha
                };

                _usuarioAplicacao.Atualizar(usuarioDominio);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar: {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("Deletar/{usuarioId}")]
        public ActionResult Deletar([FromRoute] int usuarioId)
        {
            try
            {
                _usuarioAplicacao.Excluir(usuarioId);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("Restaurar/{usuarioId}")]
        public ActionResult Restaurar([FromRoute] int usuarioId)
        {
            try
            {
                _usuarioAplicacao.Restaurar(usuarioId);

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
                var usuariosDominio = _usuarioAplicacao.Listar(ativos);

                var usuarios = usuariosDominio.Select(usuario => new UsuarioResponse()
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Ativo = usuario.Ativo,
                }).ToList();

                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task <ActionResult> Login([FromBody] UsuarioLogin usuarioRequest)
        {
            try
            {
                var usuario = await _usuarioAplicacao.ValidarUsuario(usuarioRequest.Email, usuarioRequest.Senha);
                var usuarioResponse = new UsuarioResponse(){
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Ativo = usuario.Ativo
                };
                return Ok(usuarioResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar: {ex.Message}");
            }
        }
    }
}