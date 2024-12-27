using projetoFinal.Dominio.Entidades;
using projetoFinal.Repositorio.Interfaces;
using System.Text;


namespace projetoFinal.Aplicacao
{
    public class UsuarioAplicacao : IUsuarioAplicacao
    {

        readonly IUsuarioRepositorio _usuarioRepositorio;

        public UsuarioAplicacao(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        public int Criar(Usuario usuario)
        {
            if (usuario == null)
                throw new Exception("Usuário não pode ser nulo.");

            if (string.IsNullOrEmpty(usuario.Senha))
                throw new Exception("Senha não pode ser nulo.");

            return _usuarioRepositorio.Criar(usuario);

        }

        public void Atualizar(Usuario usuario)
        {
            var usuarioDominio = _usuarioRepositorio.Obter(usuario.Id, true);

            if (usuarioDominio == null)
                throw new Exception("Usuário não encontrado.");

            usuarioDominio.Nome = usuario.Nome;
            usuarioDominio.Email = usuario.Email;
            usuarioDominio.Senha = usuario.Senha;

            _usuarioRepositorio.Atualizar(usuarioDominio);
        }

        public Usuario Obter(int usuarioId, bool ativo)
        {
            var usuarioDominio = _usuarioRepositorio.Obter(usuarioId, ativo);

            if (usuarioDominio == null)
                throw new Exception("Usuário não encontrado.");

            return usuarioDominio;
        }

        public Usuario ObterPorEmail(string email)
        {
            var usuarioDominio = _usuarioRepositorio.ObterPorEmail(email);

            if (usuarioDominio == null)
                throw new Exception("Usuário não encontrado.");

            return usuarioDominio;
        }

        public void Excluir(int usuarioId)
        {
            var usuarioDominio = _usuarioRepositorio.Obter(usuarioId, true);

            if (usuarioDominio == null)
                throw new Exception("Usuário não encontrado.");

            usuarioDominio.Excluir();

            _usuarioRepositorio.Atualizar(usuarioDominio);
        }
        public void Restaurar(int usuarioId)
        {
            var usuarioDominio = _usuarioRepositorio.Obter(usuarioId, false);

            if (usuarioDominio == null)
                throw new Exception("Usuário não encontrado.");


            usuarioDominio.Restaurar();

            _usuarioRepositorio.Atualizar(usuarioDominio);
        }

        public IEnumerable<Usuario> Listar(bool ativo)
        {
            return _usuarioRepositorio.Listar(ativo);
        }

        public async Task<Usuario> ValidarUsuario(string email, string senha)
        {
            // Buscando o usuário no banco de dados pelo email
            var usuario = await
            _usuarioRepositorio.ValidarUsuario(email, senha);

            // Se o usuário não foi encontrado
            if (usuario == null)
                throw new("Usuário não encontrado e/ou senha incorreta.");

            // Se o usuário foi encontrado e a senha está correta
            return usuario;
        }

        private string GerarTokenRedefinicao()
        {
            var random = new Random();
            var tokenBuilder = new StringBuilder();
            const string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            for (int i = 0; i < 32; i++) // 32 é o tamanho do token
            {
                tokenBuilder.Append(caracteres[random.Next(caracteres.Length)]);
            }

            return tokenBuilder.ToString();
        }

        public async Task EsqueciMinhaSenha(string email)
        {
            var usuario = await _usuarioRepositorio.ObterPorEmail(email);

            if (usuario == null)
                throw new Exception("Usuário não encontrado.");

            // Gera um token de redefinição
            var token = GerarTokenRedefinicao();
            var dataExpiracao = DateTime.UtcNow.AddHours(1); // O token expira em 1 hora

            // Atualiza o usuário com o token e a data de expiração
            usuario.TokenRedefinicao = token;
            usuario.DataExpiracaoToken = dataExpiracao;

            // Salva as alterações no banco
            _usuarioRepositorio.Atualizar(usuario);

            // Aqui você pode implementar o envio do token por e-mail ou outro meio
        }

        public async Task RedefinirSenha(string token, string novaSenha)
        {
            var usuario = await _usuarioRepositorio.ObterPorToken(token);

            if (usuario == null)
                throw new Exception("Token inválido.");

            if (usuario.DataExpiracaoToken < DateTime.UtcNow)
                throw new Exception("O token de redefinição expirou.");

            // Se o token for válido e não expirou, redefine a senha
            usuario.Senha = novaSenha;
            usuario.TokenRedefinicao = null; // Limpa o token após a redefinição
            usuario.DataExpiracaoToken = null; // Limpa a data de expiração

            // Salva a nova senha no banco de dados
            _usuarioRepositorio.Atualizar(usuario);
        }
    }

}
