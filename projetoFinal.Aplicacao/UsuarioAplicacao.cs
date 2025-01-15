using projetoFinal.Dominio.Entidades;
using projetoFinal.Repositorio.Interfaces;
using projetoFinal.Servicos.Interfaces;
using System.Text;
using System.Net;
using System.Net.Mail;



namespace projetoFinal.Aplicacao
{
    public class UsuarioAplicacao : IUsuarioAplicacao
    {

        readonly IUsuarioRepositorio _usuarioRepositorio;
        readonly IRecuperarSenhaServico _recuperarSenhaServico;

        public UsuarioAplicacao(IUsuarioRepositorio usuarioRepositorio, IRecuperarSenhaServico recuperarSenhaServico)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _recuperarSenhaServico = recuperarSenhaServico;
        }

        public async Task<int> CriarAsync(Usuario usuario)
        {
            if (usuario == null)
                throw new Exception("Usuário não pode ser nulo.");

            if (string.IsNullOrEmpty(usuario.Senha))
                throw new Exception("Senha não pode ser nulo.");

            return await _usuarioRepositorio.CriarAsync(usuario);

        }

        public async Task AtualizarAsync(Usuario usuario)
        {
            var usuarioDominio = await _usuarioRepositorio.ObterPorIdAsync(usuario.Id, true);

            if (usuarioDominio == null)
                throw new Exception("Usuário não encontrado.");

            usuarioDominio.Nome = usuario.Nome;
            usuarioDominio.Email = usuario.Email;
            usuarioDominio.Senha = usuario.Senha;

            await _usuarioRepositorio.AtualizarAsync(usuarioDominio);
        }

        public async Task<Usuario> ObterPorIdAsync(int usuarioId, bool ativo)
        {
            var usuarioDominio = await _usuarioRepositorio.ObterPorIdAsync(usuarioId, ativo);

            if (usuarioDominio == null)
                throw new Exception("Usuário não encontrado.");

            return usuarioDominio;
        }

        public async Task<Usuario> ObterPorEmailAsync(string email)
        {
            var usuarioDominio = await _usuarioRepositorio.ObterPorEmailAsync(email);

            if (usuarioDominio == null)
                throw new Exception("Usuário não encontrado.");

            return usuarioDominio;
        }

        public async Task ExcluirAsync(int usuarioId)
        {
            var usuarioDominio = await _usuarioRepositorio.ObterPorIdAsync(usuarioId, true);

            if (usuarioDominio == null)
                throw new Exception("Usuário não encontrado.");

            usuarioDominio.Excluir();

            await _usuarioRepositorio.AtualizarAsync(usuarioDominio);
        }

        public async Task RestaurarAsync(int usuarioId)
        {
            var usuarioDominio = await _usuarioRepositorio.ObterPorIdAsync(usuarioId, false);

            if (usuarioDominio == null)
                throw new Exception("Usuário não encontrado.");


            usuarioDominio.Restaurar();

            await _usuarioRepositorio.AtualizarAsync(usuarioDominio);
        }

        public async Task<IEnumerable<Usuario>> ListarAsync(bool ativo)
        {
            return await _usuarioRepositorio.ListarAsync(ativo);
        }

        //Valida o usuario para fazer login no site 
        public async Task<Usuario> ValidarUsuarioAsync(string email, string senha)
        {
            // Buscando o usuário no banco de dados pelo email
            var usuario = await _usuarioRepositorio.ValidarUsuarioAsync(email, senha);

            // Se o usuário não foi encontrado
            if (usuario == null)
                throw new("Usuário não encontrado e/ou senha incorreta.");

            // Se o usuário foi encontrado e a senha está correta
            return usuario;
        }


        //Quando você clica em "Esqueci minha senha", o sistema vai pedir o seu e-mail para verificar se ele está cadastrado. Se o e-mail for encontrado, o sistema vai criar um código (chamado de "token") que vai expirar em 1 hora. Esse token será enviado para o seu e-mail. Quando você inserir o token, o sistema vai checar se ele é o mesmo que foi salvo no seu usuario.
        public async Task EsqueciMinhaSenhaAsync(string email)
        {
            var usuario = await _usuarioRepositorio.ObterPorEmailAsync(email);

            if (usuario == null)
                throw new Exception("Usuário não encontrado.");

            // Gera um token de redefinição
            var token = await GerarTokenRedefinicao();
            var dataExpiracao = DateTime.UtcNow.AddHours(1); // O token expira em 1 hora

            // Atualiza o usuário com o token e a data de expiração
            usuario.TokenRedefinicao = token;
            usuario.DataExpiracaoToken = dataExpiracao;

            // Salva as alterações no banco
            await _usuarioRepositorio.AtualizarAsync(usuario);
            
            await _recuperarSenhaServico.EnviarEmailRecuperacaoAsync(email, token);

            // Aqui você pode implementar o envio do token por e-mail ou outro meio
            //Lugar onde sera chamado minha função que esta na service
            await _recuperarSenhaServico.EnviarEmailRecuperacaoAsync(email, token);
        }

        public async Task RedefinirSenhaAsync(string token, string novaSenha)
        {
            var usuario = await _usuarioRepositorio.ObterPorTokenAsync(token);

            if (usuario == null)
                throw new Exception("Token inválido.");

            if (usuario.DataExpiracaoToken < DateTime.UtcNow)
                throw new Exception("O token de redefinição expirou.");

            // Se o token for válido e não expirou, redefine a senha
            usuario.Senha = novaSenha;
            usuario.TokenRedefinicao = null; // Limpa o token após a redefinição
            usuario.DataExpiracaoToken = null; // Limpa a data de expiração

            // Salva a nova senha no banco de dados
            await _usuarioRepositorio.AtualizarAsync(usuario);
        }


        #region Uteis
        private async Task<string> GerarTokenRedefinicao()
        {
            return await Task.Run(() =>
            {
                var random = new Random();
                var tokenBuilder = new StringBuilder();
                const string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

                for (int i = 0; i < 32; i++) // 32 é o tamanho do token
                {
                    tokenBuilder.Append(caracteres[random.Next(caracteres.Length)]);
                }

                return tokenBuilder.ToString();
            });
        }


       
        #endregion

    }

}
