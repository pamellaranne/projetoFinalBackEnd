using Microsoft.EntityFrameworkCore;
using projetoFinal.Dominio.Entidades;
using projetoFinal.Repositorio.Context;
using projetoFinal.Repositorio.Interfaces;

public class UsuarioRepositorio : IUsuarioRepositorio
{
    readonly private Contexto _contexto;
    public UsuarioRepositorio(Contexto contexto)
    {
        _contexto = contexto;
    }

    public async Task<Usuario> ValidarUsuario(string email, string senha)
    {
        // Buscando o usuário no banco de dados pelo email
        var usuario = await
        _contexto.Usuarios
            .FirstOrDefaultAsync(u => u.Email == email);

        // Se o usuário não foi encontrado
        if (usuario == null)
            return null;

        // Aqui você deve implementar a comparação da senha
        // Em um ambiente real, você deve comparar a senha criptografada (nunca em texto simples)
        if (usuario.Senha != senha)
            return null;
        // Se o usuário foi encontrado e a senha está correta
        return usuario;
    }

    public int Criar(Usuario usuario)
    {
        _contexto.Usuarios.Add(usuario);
        _contexto.SaveChanges();

        return usuario.Id;
    }
    public void Atualizar(Usuario usuario)
    {
        _contexto.Usuarios.Update(usuario);
        _contexto.SaveChanges();
    }
    public Usuario Obter(int usuarioId, bool ativo)
    {
        return _contexto.Usuarios
                    .Where(u => u.Id == usuarioId)
                    .Where(u => u.Ativo == ativo)
                    .FirstOrDefault();
    }

    public Usuario ObterPorEmail(string email)
    {
        return _contexto.Usuarios
                    .Where(u => u.Email == email)
                    .FirstOrDefault();
    }

    public void Excluir(Usuario usuario)
    {
        _contexto.Usuarios.Remove(usuario);
        _contexto.SaveChanges();
    }

    public void Restaurar(Usuario usuario)
    {
        usuario.Restaurar();
        _contexto.Usuarios.Update(usuario);
        _contexto.SaveChanges();
    }

    public IEnumerable<Usuario> Listar(bool ativo)
    {
        return _contexto.Usuarios.Where(u => u.Ativo == ativo).ToList();
    }
}
