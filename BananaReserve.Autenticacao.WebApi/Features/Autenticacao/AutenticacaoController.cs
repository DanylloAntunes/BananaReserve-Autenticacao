using BananaReserve.Autenticacao.Application.Autenticacao.AutenticacaoUsuario;
using BananaReserve.Autenticacao.WebApi.Common;
using BananaReserve.Autenticacao.WebApi.Features.Autenticacao.AutenticacaoUsuario;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BananaReserve.Autenticacao.WebApi.Features.Autenticacao;

[ApiController]
[Route("api/[controller]")]
public class AutenticacaoController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;


    [HttpPost]
    [ProducesResponseType(typeof(AutenticacaoUsuarioResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AuthenticateUser([FromBody] AutenticacaoUsuarioRequest request, CancellationToken cancellationToken)
    {
        var validator = new AutenticacaoUsuarioRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = Obter(request);
        var response = await _mediator.Send(command, cancellationToken);

        return Ok(Obter(response));
    }

    private AutenticacaoUsuarioCommand Obter(AutenticacaoUsuarioRequest request) =>
        new() { Email = request.Email, Senha = request.Senha };

    private AutenticacaoUsuarioResponse Obter(AutenticacaoUsuarioResult resultado) =>
        new() { Email = resultado.Email, Nome = resultado.Nome, Token = resultado.Token };
}
