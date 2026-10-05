using DeskFlow.API.Data;
using DeskFlow.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeskFlow.API.Repositories;

public class CategoriaRepository : ICategoriaRepository
{
    private readonly AppDbContext _context;

    public CategoriaRepository(AppDbContext context) => _context = context;

    public async Task<List<Categoria>> ListarAsync() =>
        await _context.Categorias.AsNoTracking().OrderBy(c => c.Nome).ToListAsync();

    public async Task<Categoria?> ObterPorIdAsync(int id) =>
        await _context.Categorias.FirstOrDefaultAsync(c => c.Id == id);

    public async Task<bool> ExisteNomeAsync(string nome, int? ignorarId = null) =>
        await _context.Categorias.AnyAsync(c => c.Nome == nome && (ignorarId == null || c.Id != ignorarId));

    public async Task<bool> PossuiChamadosAsync(int id) =>
        await _context.Chamados.AnyAsync(c => c.CategoriaId == id);

    public async Task AdicionarAsync(Categoria categoria)
    {
        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Categoria categoria)
    {
        _context.Categorias.Update(categoria);
        await _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(Categoria categoria)
    {
        _context.Categorias.Remove(categoria);
        await _context.SaveChangesAsync();
    }
}
