namespace projetoFinal.Api.Models.Request;
using projetoFinal.Dominio.Enumeradores;

public class ProdutoCriar
{
    public string Nome { get; set; }
    public int Quantidade { get; set; }
    public TiposCategoriasEnum TiposCategoriasId { get; set; }
    public int UsuarioId { get; set; }
}