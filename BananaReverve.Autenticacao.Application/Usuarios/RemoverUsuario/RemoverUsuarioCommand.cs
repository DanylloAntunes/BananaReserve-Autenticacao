using MediatR;

namespace BananaReserve.Autenticacao.Application.Usuarios.RemoverUsuario;

public record RemoverUsuarioCommand : IRequest<RemoverUsuarioResponse>
{
    public int Id { get; }

    public RemoverUsuarioCommand(int id)
    {
        Id = id;
    }
}
