using DeskFlow.API.Exceptions;
using DeskFlow.API.Models.DTOs;
using DeskFlow.API.Models.Entities;
using DeskFlow.API.Repositories;

namespace DeskFlow.API.Services;

public class CategoriaService : ICategoriaService
{
    private readonly ICategoriaRepository _repository;

    public CategoriaService(ICategoriaRepository repository) => _repository = repository;

    public async Task<List<CategoriaResponse>> ListarAsync()
    {
        var categorias = await _repository.ListarAsync();
        return categorias.Select(ParaResponse).ToList();
    }

    public async Task<CategoriaResponse> ObterPorIdAsync(int id)
    {
        var categoria = await BuscarOuFalharAsync(id);
        return ParaResponse(categoria);
    }

    public async Task<CategoriaResponse> CriarAsync(CategoriaRequest request)
    {
        var nome = request.Nome.Trim();

        if (await _repository.ExisteNomeAsync(nome))
            throw new BusinessException($"Já existe uma categoria com o nome '{nome}'.");

        var categoria = new Categoria { Nome = nome };
        await _repository.AdicionarAsync(categoria);
        return ParaResponse(categoria);
    }

    public async Task<CategoriaResponse> AtualizarAsync(int id, CategoriaRequest request)
    {
        var categoria = await BuscarOuFalharAsync(id);
        var nome = request.Nome.Trim();

        if (await _repository.ExisteNomeAsync(nome, id))
            throw new BusinessException($"Já existe uma categoria com o nome '{nome}'.");

        categoria.Nome = nome;
        await _repository.AtualizarAsync(categoria);
        return ParaResponse(categoria);
    }

    public async Task RemoverAsync(int id)
    {
        var categoria = await BuscarOuFalharAsync(id);

        if (await _repository.PossuiChamadosAsync(id))
            throw new BusinessException("Não é possível remover a categoria pois existem chamados associados a ela.");

        await _repository.RemoverAsync(categoria);
    }

    private async Task<Categoria> BuscarOuFalharAsync(int id) =>
        await _repository.ObterPorIdAsync(id)
        ?? throw new NotFoundException($"Categoria com Id {id} não encontrada.");

    private static CategoriaResponse ParaResponse(Categoria c) =>
        new() { Id = c.Id, Nome = c.Nome };
}
