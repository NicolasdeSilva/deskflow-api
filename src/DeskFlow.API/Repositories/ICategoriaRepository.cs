using DeskFlow.API.Models.Entities;

namespace DeskFlow.API.Repositories;

public interface ICategoriaRepository
{
    Task<List<Categoria>> ListarAsync();
    Task<Categoria?> ObterPorIdAsync(int id);
    Task<bool> ExisteNomeAsync(string nome, int? ignorarId = null);
    Task<bool> PossuiChamadosAsync(int id);
    Task AdicionarAsync(Categoria categoria);
    Task AtualizarAsync(Categoria categoria);
    Task RemoverAsync(Categoria categoria);
}
