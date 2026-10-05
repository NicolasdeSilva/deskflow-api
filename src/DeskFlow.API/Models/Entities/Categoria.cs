namespace DeskFlow.API.Models.Entities;

public class Categoria
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;

    // Relacionamento 1:N -> uma categoria possui vários chamados
    public ICollection<Chamado> Chamados { get; set; } = new List<Chamado>();
}
