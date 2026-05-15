using MediatR;

namespace BananaReserve.Autenticacao.Application.Autenticacao.AutenticacaoUsuario;

public class AutenticacaoUsuarioCommand : IRequest<AutenticacaoUsuarioResult>
{
    public string Email { get; set; } = string.Empty;

    public string Senha { get; set; } = string.Empty;
}
