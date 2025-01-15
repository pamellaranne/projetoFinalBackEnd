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
        public async Task<ActionResult> Obter([FromRoute] int usuarioId, [FromQuery] bool ativo)
        {
            try
            {
                var usuarioDominio = await _usuarioAplicacao.ObterPorIdAsync(usuarioId, ativo);

                var usuarioResposta = new UsuarioResponse()
                {
                    Id = usuarioDominio.Id,
                    Nome = usuarioDominio.Nome,
                    Email = usuarioDominio.Email,
                };

                return Ok(usuarioResposta);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("Criar")]
        public async Task<ActionResult> Criar([FromBody] UsuarioCriar usuarioCriar)
        {
            try
            {
                var usuarioDominio = new Usuario()
                {
                    Nome = usuarioCriar.Nome,
                    Email = usuarioCriar.Email,
                    Senha = usuarioCriar.Senha,
                };

                var usuarioId = await _usuarioAplicacao.CriarAsync(usuarioDominio);

                return Ok(usuarioId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("Atualizar")]
        public async Task<ActionResult> Atualizar([FromBody] UsuarioAtualizar usuarioAtualizar)
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

                await _usuarioAplicacao.AtualizarAsync(usuarioDominio);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar : {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("Deletar/{usuarioId}")]
        public async Task<ActionResult> Deletar([FromRoute] int usuarioId)
        {
            try
            {
                await _usuarioAplicacao.ExcluirAsync(usuarioId);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao deletar: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("Restaurar/{usuarioId}")]
        public async Task<ActionResult> Restaurar([FromRoute] int usuarioId)
        {
            try
            {
                await _usuarioAplicacao.RestaurarAsync(usuarioId);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao restaurar: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("Listar")]
        public async Task<ActionResult> Listar([FromQuery] bool ativos)
        {
            try
            {
                var usuariosDominio = await _usuarioAplicacao.ListarAsync(ativos);

                var usuarios =  usuariosDominio.Select(usuario => new UsuarioResponse()
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
                return StatusCode(500, $"Erro ao listar: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> Login([FromBody] UsuarioLogin usuarioRequest)
        {
            try
            {
                var usuario = await _usuarioAplicacao.ValidarUsuarioAsync(usuarioRequest.Email, usuarioRequest.Senha);
                var usuarioResponse = new UsuarioResponse()
                {
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

        [HttpPost]
        [Route("EsqueciMinhaSenha")]
        public async Task<ActionResult> EsqueciMinhaSenha([FromBody] EsqueciSenha esqueciSenhaRequest)
        {
            try
            {
                await _usuarioAplicacao.EsqueciMinhaSenhaAsync(esqueciSenhaRequest.Email);

                
                return Ok("Link de redefinição de senha enviado para o seu e-mail.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao processar solicitação: {ex.Message}");
            }
        }


        [HttpPost]
        [Route("RedefinirMinhaSenha")]
        public async Task<ActionResult> RedefinirMinhaSenha([FromBody] RedefinirSenha redefinirSenhaRequest)
        {
            try
            {
                await _usuarioAplicacao.RedefinirSenhaAsync(redefinirSenhaRequest.Token, redefinirSenhaRequest.NovaSenha);

                
                return Ok("Senha redefinida com sucesso");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao processar solicitação: {ex.Message}");
            }
        }

    }
}