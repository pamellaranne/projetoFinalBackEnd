using System.ComponentModel.DataAnnotations;
using projetoFinal.Dominio.Entidades;

namespace projetoFinal.Aplicacao
{
    public interface IUsuarioAplicacao
    {
        Task<int> CriarAsync(Usuario usuario);
        Task AtualizarAsync(Usuario usuario);
        Task<Usuario> ObterPorIdAsync(int usuarioId, bool ativo);
        Task<Usuario> ObterPorEmailAsync(string email);
        Task ExcluirAsync(int usuarioId);
        Task RestaurarAsync(int usuarioId);
        Task<IEnumerable<Usuario>> ListarAsync(bool ativo);
        Task<Usuario> ValidarUsuarioAsync(string email, string senha);

        // Métodos para lidar com redefinição de senha
        Task EsqueciMinhaSenhaAsync(string email);
        Task RedefinirSenhaAsync(string token, string novaSenha);
    }

}