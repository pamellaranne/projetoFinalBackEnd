using projetoFinal.Dominio.Entidades;

namespace projetoFinal.Repositorio.Interfaces;
public interface IUsuarioRepositorio
{
    Task <Usuario> ValidarUsuario(string email, string senha);
    int Criar(Usuario usuario);
    void Atualizar(Usuario usuario);
    Usuario Obter(int usuarioId, bool ativo);
    Usuario ObterPorEmail(string email);
    void Excluir(Usuario usuario);
    void Restaurar(Usuario usuario);
    IEnumerable<Usuario> Listar (bool ativo);
    Task Salvar(Usuario usuario);
}