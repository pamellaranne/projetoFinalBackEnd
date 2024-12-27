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
        public async Task<ActionResult> Login([FromBody] UsuarioLogin usuarioRequest)
        {
            try
            {
                var usuario = await _usuarioAplicacao.ValidarUsuario(usuarioRequest.Email, usuarioRequest.Senha);
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
                // Verifica se o usuário existe
                var usuario = await _usuarioAplicacao.ObterPorEmail(request.Email);
                if (usuario == null)
                {
                    return BadRequest("Usuário não encontrado com este e-mail.");
                }

                // Gera o token de redefinição
                var token = GerarTokenRedefinicao();
                var dataExpiracao = DateTime.UtcNow.AddHours(1); // Token expira em 1 hora

                // Salva o token e a data de expiração no banco
                await _tokenAplicacao.SalvarTokenRedefinicao(usuario.Id, token, dataExpiracao);

                // Envia o link de redefinição de senha por e-mail
                var linkRedefinicao = $"https://seusite.com/redefinir-senha/{token}";
                await _emailService.EnviarEmail(usuario.Email, "Redefinição de Senha",
                    $"Clique no link para redefinir sua senha: {linkRedefinicao}");

                return Ok("Link de redefinição de senha enviado para o seu e-mail.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao processar solicitação: {ex.Message}");
            }
        }

    }
}