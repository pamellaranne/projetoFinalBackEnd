using projetoFinal.Dominio.Entidades;

namespace projetoFinal.Repositorio.Interfaces;
public interface IUsuarioRepositorio
{
    Task<Usuario> ValidarUsuarioAsync(string email, string senha);
    Task<int> CriarAsync(Usuario usuario);
    Task AtualizarAsync(Usuario usuario);
    Task<Usuario> ObterPorIdAsync(int usuarioId, bool ativo);
    Task<Usuario> ObterPorEmailAsync(string email);
    Task<Usuario> ObterPorTokenAsync(string token);
    Task ExcluirAsync(Usuario usuario);
    Task RestaurarAsync(Usuario usuario);
    Task<IEnumerable<Usuario>> ListarAsync(bool ativo);
    
}