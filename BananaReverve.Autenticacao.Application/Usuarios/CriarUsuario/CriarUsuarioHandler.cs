using BananaReserve.Autenticacao.Common.Seguracao;
using BananaReserve.Autenticacao.Domain.Entidades;
using BananaReserve.Autenticacao.Domain.Repositorios;
using FluentValidation;
using MediatR;

namespace BananaReserve.Autenticacao.Application.Usuarios.CriarUsuario;

public class CriarUsuarioHandler(IUsuarioRepositorio usuarioRepositorio, IPasswordHasher passwordHasher) : IRequestHandler<CriarUsuarioCommand, CriarUsuarioResult>
{
    private readonly IUsuarioRepositorio _usuarioRepositorio = usuarioRepositorio;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;

    public async Task<CriarUsuarioResult> Handle(CriarUsuarioCommand command, CancellationToken cancellationToken)
    {
        var validator = new CriarUsuarioValidator();
        var resultadoValidacao = await validator.ValidateAsync(command, cancellationToken);

        if (!resultadoValidacao.IsValid)
            throw new ValidationException(resultadoValidacao.Errors);

        var usuarioExistente = await _usuarioRepositorio.ObterPorEmailAsync(command.Email, cancellationToken);
        if (usuarioExistente != null)
            throw new InvalidOperationException($"Já existe um usuário com o e-mail {command.Email}.");

        var usuario = Obter(command);
        usuario.Senha = _passwordHasher.HashPassword(command.Senha);

        var usuarioCriado = await _usuarioRepositorio.CriarAsync(usuario, cancellationToken);

        return Obter(usuarioCriado);
    }

    private Usuario Obter(CriarUsuarioCommand command) => 
        new() { 
            Email = command.Email, 
            Senha = command.Senha,
            Nome = command.Nome
        };

    private CriarUsuarioResult Obter(Usuario usuario) =>
        new() { Id = usuario.Id };

}
