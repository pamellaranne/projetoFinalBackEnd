using System.ComponentModel.DataAnnotations;
using projetoFinal.Dominio.Entidades;

namespace projetoFinal.Aplicacao
{
    public interface IUsuarioAplicacao
    {
        int Criar(Usuario usuario);
        void Atualizar(Usuario usuario);
        Usuario Obter(int usuarioId, bool ativo);
        Usuario ObterPorEmail(string email);
        void Excluir(int usuarioId);
        void Restaurar(int usuarioId);
        IEnumerable<Usuario> Listar (bool ativo);
        Task <Usuario> ValidarUsuario(string email, string senha);

        // Métodos para lidar com redefinição de senha
        Task EsqueciMinhaSenha(string email);
        Task RedefinirSenha(string token, string novaSenha);
    }

}