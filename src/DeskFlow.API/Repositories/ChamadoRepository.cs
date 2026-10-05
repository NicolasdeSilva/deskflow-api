using DeskFlow.API.Data;
using DeskFlow.API.Models.Entities;
using DeskFlow.API.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace DeskFlow.API.Repositories;

public class ChamadoRepository : IChamadoRepository
{
    private readonly AppDbContext _context;

    public ChamadoRepository(AppDbContext context) => _context = context;

    // Filtros dinâmicos: só aplica o Where se o filtro foi informado
    public async Task<List<Chamado>> ListarAsync(StatusChamado? status, Prioridade? prioridade, int? categoriaId)
    {
        var query = _context.Chamados
            .AsNoTracking()
            .Include(c => c.Categoria)
            .AsQueryable();

        if (status.HasValue)
            query = query.Where(c => c.Status == status.Value);

        if (prioridade.HasValue)
            query = query.Where(c => c.Prioridade == prioridade.Value);

        if (categoriaId.HasValue)
            query = query.Where(c => c.CategoriaId == categoriaId.Value);

        return await query.OrderByDescending(c => c.DataAbertura).ToListAsync();
    }

    public async Task<Chamado?> ObterPorIdAsync(int id) =>
        await _context.Chamados.FirstOrDefaultAsync(c => c.Id == id);

    public async Task<Chamado?> ObterDetalhesAsync(int id) =>
        await _context.Chamados
            .Include(c => c.Categoria)
            .Include(c => c.Interacoes)
            .FirstOrDefaultAsync(c => c.Id == id);

    public async Task AdicionarAsync(Chamado chamado)
    {
        _context.Chamados.Add(chamado);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Chamado chamado)
    {
        _context.Chamados.Update(chamado);
        await _context.SaveChangesAsync();
    }

    public async Task AdicionarInteracaoAsync(Interacao interacao)
    {
        _context.Interacoes.Add(interacao);
        await _context.SaveChangesAsync();
    }
}
