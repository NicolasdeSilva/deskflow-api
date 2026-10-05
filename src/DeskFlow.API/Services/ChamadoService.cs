using DeskFlow.API.Exceptions;
using DeskFlow.API.Models.DTOs;
using DeskFlow.API.Models.Entities;
using DeskFlow.API.Models.Enums;
using DeskFlow.API.Repositories;

namespace DeskFlow.API.Services;

public class ChamadoService : IChamadoService
{
    private readonly IChamadoRepository _chamadoRepository;
    private readonly ICategoriaRepository _categoriaRepository;

    public ChamadoService(IChamadoRepository chamadoRepository, ICategoriaRepository categoriaRepository)
    {
        _chamadoRepository = chamadoRepository;
        _categoriaRepository = categoriaRepository;
    }

    public async Task<List<ChamadoResponse>> ListarAsync(StatusChamado? status, Prioridade? prioridade, int? categoriaId)
    {
        var chamados = await _chamadoRepository.ListarAsync(status, prioridade, categoriaId);
        return chamados.Select(ParaResponse).ToList();
    }

    public async Task<ChamadoDetalheResponse> ObterDetalhesAsync(int id)
    {
        var chamado = await _chamadoRepository.ObterDetalhesAsync(id)
            ?? throw new NotFoundException($"Chamado com Id {id} não encontrado.");

        return new ChamadoDetalheResponse
        {
            Id = chamado.Id,
            Titulo = chamado.Titulo,
            Descricao = chamado.Descricao,
            Prioridade = chamado.Prioridade,
            Status = chamado.Status,
            SolicitanteNome = chamado.SolicitanteNome,
            DataAbertura = chamado.DataAbertura,
            DataFechamento = chamado.DataFechamento,
            Solucao = chamado.Solucao,
            CategoriaId = chamado.CategoriaId,
            CategoriaNome = chamado.Categoria?.Nome,
            Categoria = chamado.Categoria is null
                ? null
                : new CategoriaResponse { Id = chamado.Categoria.Id, Nome = chamado.Categoria.Nome },
            Interacoes = chamado.Interacoes
                .OrderBy(i => i.DataRegistro)
                .Select(ParaResponse)
                .ToList()
        };
    }

    public async Task<ChamadoResponse> AbrirAsync(ChamadoCreateRequest request)
    {
        var categoria = await _categoriaRepository.ObterPorIdAsync(request.CategoriaId)
            ?? throw new BusinessException($"A categoria com Id {request.CategoriaId} não existe.");

        var chamado = new Chamado
        {
            Titulo = request.Titulo.Trim(),
            Descricao = request.Descricao.Trim(),
            Prioridade = request.Prioridade,
            SolicitanteNome = request.SolicitanteNome.Trim(),
            CategoriaId = categoria.Id,
            Status = StatusChamado.Aberto,        // regra: todo chamado nasce Aberto
            DataAbertura = DateTime.UtcNow        // regra: data/hora atual
        };

        await _chamadoRepository.AdicionarAsync(chamado);
        chamado.Categoria = categoria;
        return ParaResponse(chamado);
    }

    public async Task<ChamadoResponse> IniciarAsync(int id)
    {
        var chamado = await BuscarOuFalharAsync(id);

        if (chamado.Status != StatusChamado.Aberto)
            throw new BusinessException(
                $"Só é possível iniciar um chamado com status Aberto. Status atual: {chamado.Status}.");

        chamado.Status = StatusChamado.EmAndamento;
        await _chamadoRepository.AtualizarAsync(chamado);
        return ParaResponse(chamado);
    }

    public async Task<ChamadoResponse> EncerrarAsync(int id, EncerrarChamadoRequest request)
    {
        var chamado = await BuscarOuFalharAsync(id);

        if (chamado.Status != StatusChamado.EmAndamento)
            throw new BusinessException(
                $"Só é possível encerrar um chamado com status EmAndamento. Status atual: {chamado.Status}.");

        if (string.IsNullOrWhiteSpace(request.Solucao))
            throw new BusinessException("A solução é obrigatória para encerrar o chamado.");

        chamado.Solucao = request.Solucao.Trim();
        chamado.DataFechamento = DateTime.UtcNow;
        chamado.Status = StatusChamado.Fechado;

        await _chamadoRepository.AtualizarAsync(chamado);
        return ParaResponse(chamado);
    }

    public async Task<InteracaoResponse> AdicionarInteracaoAsync(int chamadoId, InteracaoCreateRequest request)
    {
        var chamado = await BuscarOuFalharAsync(chamadoId);

        if (chamado.Status == StatusChamado.Fechado)
            throw new BusinessException("Não é possível adicionar interações a um chamado Fechado.");

        var interacao = new Interacao
        {
            ChamadoId = chamado.Id,
            Autor = request.Autor.Trim(),
            Mensagem = request.Mensagem.Trim(),
            DataRegistro = DateTime.UtcNow
        };

        await _chamadoRepository.AdicionarInteracaoAsync(interacao);
        return ParaResponse(interacao);
    }

    private async Task<Chamado> BuscarOuFalharAsync(int id) =>
        await _chamadoRepository.ObterPorIdAsync(id)
        ?? throw new NotFoundException($"Chamado com Id {id} não encontrado.");

    private static ChamadoResponse ParaResponse(Chamado c) => new()
    {
        Id = c.Id,
        Titulo = c.Titulo,
        Descricao = c.Descricao,
        Prioridade = c.Prioridade,
        Status = c.Status,
        SolicitanteNome = c.SolicitanteNome,
        DataAbertura = c.DataAbertura,
        DataFechamento = c.DataFechamento,
        Solucao = c.Solucao,
        CategoriaId = c.CategoriaId,
        CategoriaNome = c.Categoria?.Nome
    };

    private static InteracaoResponse ParaResponse(Interacao i) => new()
    {
        Id = i.Id,
        ChamadoId = i.ChamadoId,
        Autor = i.Autor,
        Mensagem = i.Mensagem,
        DataRegistro = i.DataRegistro
    };
}
