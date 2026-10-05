using System.ComponentModel.DataAnnotations;
using DeskFlow.API.Models.Enums;

namespace DeskFlow.API.Models.DTOs;

public class ChamadoCreateRequest
{
    [Required(ErrorMessage = "O título é obrigatório.")]
    [StringLength(150, MinimumLength = 3, ErrorMessage = "O título deve ter entre 3 e 150 caracteres.")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "A descrição é obrigatória.")]
    [StringLength(2000, MinimumLength = 5, ErrorMessage = "A descrição deve ter entre 5 e 2000 caracteres.")]
    public string Descricao { get; set; } = string.Empty;

    [EnumDataType(typeof(Prioridade), ErrorMessage = "Prioridade inválida. Use Baixa, Media ou Alta.")]
    public Prioridade Prioridade { get; set; }

    [Required(ErrorMessage = "O nome do solicitante é obrigatório.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "O nome do solicitante deve ter entre 2 e 100 caracteres.")]
    public string SolicitanteNome { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Informe uma categoria válida.")]
    public int CategoriaId { get; set; }
}

public class EncerrarChamadoRequest
{
    [Required(ErrorMessage = "A solução é obrigatória para encerrar o chamado.")]
    [StringLength(2000, MinimumLength = 3, ErrorMessage = "A solução deve ter entre 3 e 2000 caracteres.")]
    public string Solucao { get; set; } = string.Empty;
}

public class InteracaoCreateRequest
{
    [Required(ErrorMessage = "O autor é obrigatório.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "O autor deve ter entre 2 e 100 caracteres.")]
    public string Autor { get; set; } = string.Empty;

    [Required(ErrorMessage = "A mensagem é obrigatória.")]
    [StringLength(2000, MinimumLength = 1, ErrorMessage = "A mensagem deve ter até 2000 caracteres.")]
    public string Mensagem { get; set; } = string.Empty;
}

public class InteracaoResponse
{
    public int Id { get; set; }
    public int ChamadoId { get; set; }
    public string Autor { get; set; } = string.Empty;
    public string Mensagem { get; set; } = string.Empty;
    public DateTime DataRegistro { get; set; }
}

// Usado na listagem (sem a lista de interações)
public class ChamadoResponse
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public Prioridade Prioridade { get; set; }
    public StatusChamado Status { get; set; }
    public string SolicitanteNome { get; set; } = string.Empty;
    public DateTime DataAbertura { get; set; }
    public DateTime? DataFechamento { get; set; }
    public string? Solucao { get; set; }
    public int CategoriaId { get; set; }
    public string? CategoriaNome { get; set; }
}

// Usado no GET /api/chamados/{id} (com Categoria e Interações)
public class ChamadoDetalheResponse : ChamadoResponse
{
    public CategoriaResponse? Categoria { get; set; }
    public List<InteracaoResponse> Interacoes { get; set; } = new();
}

public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Mensagem { get; set; } = string.Empty;
}
