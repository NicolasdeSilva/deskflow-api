using DeskFlow.API.Models.DTOs;
using DeskFlow.API.Models.Enums;

namespace DeskFlow.API.Services;

public interface IChamadoService
{
    Task<List<ChamadoResponse>> ListarAsync(StatusChamado? status, Prioridade? prioridade, int? categoriaId);
    Task<ChamadoDetalheResponse> ObterDetalhesAsync(int id);
    Task<ChamadoResponse> AbrirAsync(ChamadoCreateRequest request);
    Task<ChamadoResponse> IniciarAsync(int id);
    Task<ChamadoResponse> EncerrarAsync(int id, EncerrarChamadoRequest request);
    Task<InteracaoResponse> AdicionarInteracaoAsync(int chamadoId, InteracaoCreateRequest request);
}
