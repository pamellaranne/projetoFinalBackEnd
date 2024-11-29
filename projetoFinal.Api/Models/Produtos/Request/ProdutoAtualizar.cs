namespace projetoFinal.Api.Models.Request;
using projetoFinal.Dominio.Enumeradores;

public class ProdutoAtualizar
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public int Quantidade { get; set; }
    public TiposCategoriasEnum TiposCategoriasId { get; set; }
    public int UsuarioId { get; set; }
}