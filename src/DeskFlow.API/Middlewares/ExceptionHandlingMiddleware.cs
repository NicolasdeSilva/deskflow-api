using DeskFlow.API.Exceptions;
using DeskFlow.API.Models.DTOs;

namespace DeskFlow.API.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await TratarExcecaoAsync(context, ex);
        }
    }

    private async Task TratarExcecaoAsync(HttpContext context, Exception ex)
    {
        var (statusCode, titulo, mensagem) = ex switch
        {
            NotFoundException => (StatusCodes.Status404NotFound, "Recurso não encontrado", ex.Message),
            BusinessException => (StatusCodes.Status400BadRequest, "Requisição inválida", ex.Message),
            _ => (StatusCodes.Status500InternalServerError, "Erro interno",
                  "Ocorreu um erro inesperado. Tente novamente mais tarde.")
        };

        if (statusCode == StatusCodes.Status500InternalServerError)
            _logger.LogError(ex, "Erro não tratado na requisição {Path}", context.Request.Path);
        else
            _logger.LogWarning("{Titulo}: {Mensagem}", titulo, mensagem);

        // Nunca devolvemos stack trace para o cliente
        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(new ErrorResponse
        {
            StatusCode = statusCode,
            Titulo = titulo,
            Mensagem = mensagem
        });
    }
}
