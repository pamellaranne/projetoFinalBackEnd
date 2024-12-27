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

    public async Task EsqueciMinhaSenha(string email)
    {
        // Obtém o usuário pelo e-mail
        var usuario = await _contexto.Usuarios
            .FirstOrDefaultAsync(u => u.Email == email);

        if (usuario == null)
        {
            throw new Exception("Usuário não encontrado.");
        }

        // Gera um token único para redefinição de senha
        var token = Guid.NewGuid().ToString();

        // Define a data de expiração do token (ex: 1 hora)
        var expiracao = DateTime.UtcNow.AddHours(1);

        // Atualiza o usuário com o token e a data de expiração
        usuario.TokenRedefinicao = token;
        usuario.DataExpiracaoToken = expiracao;

        // Salva as mudanças no banco de dados
        _contexto.Usuarios.Update(usuario);
        await _contexto.SaveChangesAsync();

        // Aqui você pode enviar o token por e-mail ou outro meio
    }

    public async Task RedefinirSenha(string token, string novaSenha)
    {
        var usuario = await _contexto.Usuarios
            .FirstOrDefaultAsync(u => u.TokenRedefinicao == token);

        if (usuario == null)
        {
            throw new Exception("Token inválido.");
        }

        if (usuario.DataExpiracaoToken < DateTime.UtcNow)
        {
            throw new Exception("O token de redefinição expirou.");
        }

        // Se o token for válido e não expirou, redefine a senha
        usuario.Senha = novaSenha;
        usuario.TokenRedefinicao = null; // Limpa o token após a redefinição
        usuario.DataExpiracaoToken = null; // Limpa a data de expiração

        // Salva a nova senha no banco de dados
        _contexto.Usuarios.Update(usuario);
        await _contexto.SaveChangesAsync();
    }

    public async Task Salvar(Usuario usuario)
    {
        if (usuario.Id == 0)
        {
            // Caso o usuário não tenha ID (ou seja, é um novo usuário), adicione
            _contexto.Usuarios.Add(usuario);
        }
        else
        {
            // Caso contrário, atualize o usuário existente
            _contexto.Usuarios.Update(usuario);
        }

        // Salve as alterações no banco de dados
        await _contexto.SaveChangesAsync();
    }


}


