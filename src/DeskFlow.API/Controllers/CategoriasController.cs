using DeskFlow.API.Models.DTOs;
using DeskFlow.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeskFlow.API.Controllers;

[ApiController]
[Route("api/categorias")]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaService _service;

    public CategoriasController(ICategoriaService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<List<CategoriaResponse>>> Listar() =>
        Ok(await _service.ListarAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoriaResponse>> ObterPorId(int id) =>
        Ok(await _service.ObterPorIdAsync(id));

    [HttpPost]
    public async Task<ActionResult<CategoriaResponse>> Criar([FromBody] CategoriaRequest request)
    {
        var criada = await _service.CriarAsync(request);
        return CreatedAtAction(nameof(ObterPorId), new { id = criada.Id }, criada);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CategoriaResponse>> Atualizar(int id, [FromBody] CategoriaRequest request) =>
        Ok(await _service.AtualizarAsync(id, request));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Remover(int id)
    {
        await _service.RemoverAsync(id);
        return NoContent();
    }
}
