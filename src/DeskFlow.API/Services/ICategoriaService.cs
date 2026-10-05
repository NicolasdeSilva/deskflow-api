using DeskFlow.API.Models.DTOs;

namespace DeskFlow.API.Services;

public interface ICategoriaService
{
    Task<List<CategoriaResponse>> ListarAsync();
    Task<CategoriaResponse> ObterPorIdAsync(int id);
    Task<CategoriaResponse> CriarAsync(CategoriaRequest request);
    Task<CategoriaResponse> AtualizarAsync(int id, CategoriaRequest request);
    Task RemoverAsync(int id);
}
