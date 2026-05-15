using BananaReserve.Autenticacao.Application.Usuarios.CriarUsuario;
using BananaReserve.Autenticacao.Application.Usuarios.ObterUsuario;
using BananaReserve.Autenticacao.Application.Usuarios.RemoverUsuario;
using BananaReserve.Autenticacao.WebApi.Common;
using BananaReserve.Autenticacao.WebApi.Features.Usuarios.CriarUsuario;
using BananaReserve.Autenticacao.WebApi.Features.Usuarios.ObterUsuario;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BananaReserve.Autenticacao.WebApi.Features.Usuarios;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    [ProducesResponseType(typeof(CriarUsuarioResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CriarUsuario([FromBody] CriarUsuarioRequest request, CancellationToken cancellationToken)
    {
        var validator = new CriarUsuarioRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = Obter(request);
        var response = await _mediator.Send(command, cancellationToken);

        return Created(string.Empty, Obter(response));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ObterUsuarioResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterUsuario([FromRoute] int id, CancellationToken cancellationToken)
    {
        var query = new ObterUsuarioQuery(id);
        var response = await _mediator.Send(query, cancellationToken);

        return base.Ok(Obter(response));
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUser([FromRoute] int id, CancellationToken cancellationToken)
    {
        var command = new RemoverUsuarioCommand(id);
        await _mediator.Send(command, cancellationToken);

        return Ok();
    }

    private CriarUsuarioCommand Obter(CriarUsuarioRequest request) =>
        new() { Email = request.Email, Nome = request.Nome, Senha = request.Senha };

    private CriarUsuarioResponse Obter(CriarUsuarioResult resultado) =>
        new() { Id = resultado.Id };

    private ObterUsuarioResponse Obter(ObterUsuarioResult resultado) =>
        new() { Id = resultado.Id, Nome = resultado.Nome, Email = resultado.Email };
}
