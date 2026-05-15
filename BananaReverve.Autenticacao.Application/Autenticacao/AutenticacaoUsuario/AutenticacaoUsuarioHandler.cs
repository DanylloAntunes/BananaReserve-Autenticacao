using BananaReserve.Autenticacao.Common.Seguracao;
using BananaReserve.Autenticacao.Domain.Repositorios;
using MediatR;

namespace BananaReserve.Autenticacao.Application.Autenticacao.AutenticacaoUsuario;

public class AutenticacaoUsuarioHandler(
    IUsuarioRepositorio usuarioRepository,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator) : IRequestHandler<AutenticacaoUsuarioCommand, AutenticacaoUsuarioResult>
{
    private readonly IUsuarioRepositorio _usuarioRepositorio = usuarioRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;

    public async Task<AutenticacaoUsuarioResult> Handle(AutenticacaoUsuarioCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioRepositorio.ObterPorEmailAsync(request.Email, cancellationToken);
            
        if (usuario == null || !_passwordHasher.ValidaPassword(request.Senha, usuario.Senha))
        {
            throw new UnauthorizedAccessException("Credenciais inválidas");
        }

        var token = _jwtTokenGenerator.GeradorDeToken(usuario);

        return new AutenticacaoUsuarioResult
        {
            Token = token,
            Email = usuario.Email,
            Nome = usuario.Nome
        };
    }
}

