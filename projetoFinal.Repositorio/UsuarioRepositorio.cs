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

    public async Task<Usuario> ValidarUsuarioAsync(string email, string senha)
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

    public async Task<int> CriarAsync(Usuario usuario)
    {
        await _contexto.Usuarios.AddAsync(usuario);
        await _contexto.SaveChangesAsync();

        return usuario.Id;
    }

    public async Task AtualizarAsync(Usuario usuario)
    {
        _contexto.Usuarios.Update(usuario);
        await _contexto.SaveChangesAsync();
    }

    public async Task<Usuario> ObterPorIdAsync(int usuarioId, bool ativo)
    {
        return await _contexto.Usuarios
                    .Where(u => u.Id == usuarioId)
                    .Where(u => u.Ativo == ativo)
                    .FirstOrDefaultAsync();
    }

    public async Task<Usuario> ObterPorEmailAsync(string email)
    {
        return await _contexto.Usuarios
                    .Where(u => u.Email == email)
                    .FirstOrDefaultAsync();
    }
    
    public async Task<Usuario> ObterPorTokenAsync(string token)
    {
        return await _contexto.Usuarios
                    .Where(u => u.TokenRedefinicao == token)
                    .FirstOrDefaultAsync();
    }

    public async Task ExcluirAsync(Usuario usuario)
    {
        _contexto.Usuarios.Remove(usuario);
        await _contexto.SaveChangesAsync();
    }

    public async Task RestaurarAsync(Usuario usuario)
    {
        usuario.Restaurar();

        _contexto.Usuarios.Update(usuario);
        await _contexto.SaveChangesAsync();
    }

    public async Task<IEnumerable<Usuario>> ListarAsync(bool ativo)
    {
        return await _contexto.Usuarios.Where(u => u.Ativo == ativo).ToListAsync();
    }




    //NAO ENTENDI PRA QUE QUE SERVE ISSO SE JA TEM O CRIAR KSKSKSKSK
    // public async Task Salvar(Usuario usuario)
    // {
    //     if (usuario.Id == 0)
    //     {
    //         // Caso o usuário não tenha ID (ou seja, é um novo usuário), adicione
    //         _contexto.Usuarios.Add(usuario);
    //     }
    //     else
    //     {
    //         // Caso contrário, atualize o usuário existente
    //         _contexto.Usuarios.Update(usuario);
    //     }

    //     // Salve as alterações no banco de dados
    //     await _contexto.SaveChangesAsync();
    // }


}


