namespace projetoFinal.Api.Models.Request;

public class UsuarioAtualizar

{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }
}