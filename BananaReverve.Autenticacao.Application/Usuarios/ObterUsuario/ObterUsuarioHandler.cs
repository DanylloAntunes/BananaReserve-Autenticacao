using BananaReserve.Autenticacao.Domain.Entidades;
using BananaReserve.Autenticacao.Domain.Repositorios;
using FluentValidation;
using MediatR;

namespace BananaReserve.Autenticacao.Application.Usuarios.ObterUsuario;

public class ObterUsuarioHandler(IUsuarioRepositorio usuarioRepositorio) : IRequestHandler<ObterUsuarioQuery, ObterUsuarioResult>
{
    private readonly IUsuarioRepositorio _usuarioRepositorio = usuarioRepositorio;

    public async Task<ObterUsuarioResult> Handle(ObterUsuarioQuery request, CancellationToken cancellationToken)
    {
        var validator = new ObterUsuarioValidator();
        var resultadoValidacao = await validator.ValidateAsync(request, cancellationToken);

        if (!resultadoValidacao.IsValid)
            throw new ValidationException(resultadoValidacao.Errors);

        var usuario = await _usuarioRepositorio.ObterPorIdAsync(request.Id, cancellationToken);
        if (usuario == null)
            throw new KeyNotFoundException($"Usuário com ID {request.Id} não encontrado");

        return Obter(usuario);
    }

    private ObterUsuarioResult Obter(Usuario usuario) =>
        new()
        {
            Id = usuario.Id,
            Email = usuario.Email,
            Nome = usuario.Nome
        };
}
