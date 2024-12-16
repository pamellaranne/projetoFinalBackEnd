using projetoFinal.Dominio.Entidades;
using projetoFinal.Repositorio.Interfaces;


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
                throw new ("Usuário não encontrado e/ou senha incorreta.");

            // Se o usuário foi encontrado e a senha está correta
            return usuario;
        }
    }

}
