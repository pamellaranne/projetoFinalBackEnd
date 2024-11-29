namespace projetoFinal.Api.Models.Response;
using projetoFinal.Dominio.Enumeradores;

public class ProdutoResponse
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public int Quantidade { get; set; }
    public TiposCategoriasEnum TiposCategoriasId { get; set; }
    public int UsuarioId { get; set; }
}