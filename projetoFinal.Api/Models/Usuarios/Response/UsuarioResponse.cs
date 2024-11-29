namespace projetoFinal.Api.Models.Response;

public class UsuarioResponse
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public bool Ativo { get; set; }
}