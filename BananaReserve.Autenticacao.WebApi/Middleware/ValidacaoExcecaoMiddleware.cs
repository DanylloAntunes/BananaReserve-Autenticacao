using BananaReserve.Autenticacao.Common.Validacao;
using BananaReserve.Autenticacao.WebApi.Common;
using FluentValidation;
using System.Text.Json;

namespace BananaReserve.Autenticacao.WebApi.Middleware;

public class ValidacaoExcecaoMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            await HandleValidationExceptionAsync(context, ex);
        }
        catch (KeyNotFoundException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            var response = new ApiResponse { Sucesso = false, Mensagem = ex.Message };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
        }
        catch (InvalidOperationException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
            var response = new ApiResponse { Sucesso = false, Mensagem = ex.Message };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
        }
    }

    private static Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        var response = new ApiResponse
        {
            Sucesso = false,
            Mensagem = "Validação falhou",
            Errors = exception.Errors
                .Select(error => (ValidacaoDetalheErro)error)
        };

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
    }
}
