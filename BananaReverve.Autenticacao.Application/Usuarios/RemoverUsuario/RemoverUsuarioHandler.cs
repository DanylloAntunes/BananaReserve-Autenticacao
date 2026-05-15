using BananaReserve.Autenticacao.Domain.Repositorios;
using FluentValidation;
using MediatR;

namespace BananaReserve.Autenticacao.Application.Usuarios.RemoverUsuario;

public class RemoverUsuarioHandler(IUsuarioRepositorio usuarioRepositorio) : IRequestHandler<RemoverUsuarioCommand, RemoverUsuarioResponse>
{
    private readonly IUsuarioRepositorio _usuarioRepositorio = usuarioRepositorio;

    public async Task<RemoverUsuarioResponse> Handle(RemoverUsuarioCommand request, CancellationToken cancellationToken)
    {
        var validator = new RemoverUsuarioValidator();
        var resultadoValidacao = await validator.ValidateAsync(request, cancellationToken);

        if (!resultadoValidacao.IsValid)
            throw new ValidationException(resultadoValidacao.Errors);

        var sucesso = await _usuarioRepositorio.RemoverAsync(request.Id, cancellationToken);
        if (!sucesso)
            throw new KeyNotFoundException($"Usuário com ID {request.Id} não encontrado");

        return new RemoverUsuarioResponse { Sucesso = true };
    }
}
