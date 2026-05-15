using MediatR;

namespace BananaReserve.Autenticacao.Application.Usuarios.ObterUsuario;

public record ObterUsuarioQuery : IRequest<ObterUsuarioResult>
{
    public int Id { get; }

    public ObterUsuarioQuery(int id)
    {
        Id = id;
    }
}
