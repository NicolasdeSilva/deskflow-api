using DeskFlow.API.Models.DTOs;
using DeskFlow.API.Models.Enums;
using DeskFlow.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeskFlow.API.Controllers;

[ApiController]
[Route("api/chamados")]
public class ChamadosController : ControllerBase
{
    private readonly IChamadoService _service;

    public ChamadosController(IChamadoService service) => _service = service;

    // GET /api/chamados?status=Aberto&prioridade=Alta&categoriaId=1
    [HttpGet]
    public async Task<ActionResult<List<ChamadoResponse>>> Listar(
        [FromQuery] StatusChamado? status,
        [FromQuery] Prioridade? prioridade,
        [FromQuery] int? categoriaId) =>
        Ok(await _service.ListarAsync(status, prioridade, categoriaId));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ChamadoDetalheResponse>> ObterPorId(int id) =>
        Ok(await _service.ObterDetalhesAsync(id));

    [HttpPost]
    public async Task<ActionResult<ChamadoResponse>> Abrir([FromBody] ChamadoCreateRequest request)
    {
        var chamado = await _service.AbrirAsync(request);
        return CreatedAtAction(nameof(ObterPorId), new { id = chamado.Id }, chamado);
    }

    [HttpPatch("{id:int}/iniciar")]
    public async Task<ActionResult<ChamadoResponse>> Iniciar(int id) =>
        Ok(await _service.IniciarAsync(id));

    [HttpPatch("{id:int}/encerrar")]
    public async Task<ActionResult<ChamadoResponse>> Encerrar(int id, [FromBody] EncerrarChamadoRequest request) =>
        Ok(await _service.EncerrarAsync(id, request));

    [HttpPost("{id:int}/interacoes")]
    public async Task<ActionResult<InteracaoResponse>> AdicionarInteracao(int id, [FromBody] InteracaoCreateRequest request)
    {
        var interacao = await _service.AdicionarInteracaoAsync(id, request);
        return CreatedAtAction(nameof(ObterPorId), new { id }, interacao);
    }
}
