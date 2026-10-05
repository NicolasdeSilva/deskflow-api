using DeskFlow.API.Models.Entities;
using DeskFlow.API.Models.Enums;

namespace DeskFlow.API.Repositories;

public interface IChamadoRepository
{
    Task<List<Chamado>> ListarAsync(StatusChamado? status, Prioridade? prioridade, int? categoriaId);
    Task<Chamado?> ObterPorIdAsync(int id);
    Task<Chamado?> ObterDetalhesAsync(int id);
    Task AdicionarAsync(Chamado chamado);
    Task AtualizarAsync(Chamado chamado);
    Task AdicionarInteracaoAsync(Interacao interacao);
}
