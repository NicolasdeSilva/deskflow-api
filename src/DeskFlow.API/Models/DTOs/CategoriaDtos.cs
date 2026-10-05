using System.ComponentModel.DataAnnotations;

namespace DeskFlow.API.Models.DTOs;

public class CategoriaRequest
{
    [Required(ErrorMessage = "O nome da categoria é obrigatório.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "O nome deve ter entre 2 e 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;
}

public class CategoriaResponse
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
}
