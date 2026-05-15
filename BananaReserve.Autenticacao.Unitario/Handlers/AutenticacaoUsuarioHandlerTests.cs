using BananaReserve.Autenticacao.Application.Autenticacao.AutenticacaoUsuario;
using BananaReserve.Autenticacao.Common.Seguracao;
using BananaReserve.Autenticacao.Domain.Entidades;
using BananaReserve.Autenticacao.Domain.Repositorios;
using Bogus;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace BananaReserve.Autenticacao.Unitario.Handlers;

public class AutenticacaoUsuarioHandlerTests
{
    private readonly IUsuarioRepositorio _repositorio;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly AutenticacaoUsuarioHandler _handler;
    private readonly Faker _faker;

    public AutenticacaoUsuarioHandlerTests()
    {
        _repositorio = Substitute.For<IUsuarioRepositorio>();
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _jwtTokenGenerator = Substitute.For<IJwtTokenGenerator>();
        _handler = new AutenticacaoUsuarioHandler(_repositorio, _passwordHasher, _jwtTokenGenerator);
        _faker = new Faker("pt_BR");
    }

    [Fact]
    public async Task Deve_RetornarTokenEDadosDoUsuario_Quando_CredenciaisValidas()
    {
        var email = "usuario@email.com";
        var senha = "Senha@123";
        var tokenEsperado = "jwt_token_gerado";

        var usuario = new Usuario
        {
            Id = 1,
            Nome = "Ana Lima",
            Email = email,
            Senha = "hash_senha"
        };

        var command = new AutenticacaoUsuarioCommand { Email = email, Senha = senha };

        _repositorio.ObterPorEmailAsync(email, Arg.Any<CancellationToken>()).Returns(usuario);
        _passwordHasher.ValidaPassword(senha, usuario.Senha).Returns(true);
        _jwtTokenGenerator.GeradorDeToken(usuario).Returns(tokenEsperado);

        var resultado = await _handler.Handle(command, CancellationToken.None);

        resultado.Should().NotBeNull();
        resultado.Token.Should().Be(tokenEsperado);
        resultado.Email.Should().Be(email);
        resultado.Nome.Should().Be(usuario.Nome);
    }

    [Fact]
    public async Task Deve_LancarUnauthorizedAccessException_Quando_UsuarioNaoEncontrado()
    {
        var command = new AutenticacaoUsuarioCommand
        {
            Email = "inexistente@email.com",
            Senha = "Senha@123"
        };

        _repositorio.ObterPorEmailAsync(command.Email, Arg.Any<CancellationToken>()).ReturnsNull();

        var acao = async () => await _handler.Handle(command, CancellationToken.None);

        await acao.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("Credenciais inválidas");
    }

    [Fact]
    public async Task Deve_LancarUnauthorizedAccessException_Quando_SenhaIncorreta()
    {
        var email = "usuario@email.com";
        var usuario = new Usuario { Id = 2, Nome = "Pedro Costa", Email = email, Senha = "hash_correto" };

        var command = new AutenticacaoUsuarioCommand { Email = email, Senha = "SenhaErrada@1" };

        _repositorio.ObterPorEmailAsync(email, Arg.Any<CancellationToken>()).Returns(usuario);
        _passwordHasher.ValidaPassword(command.Senha, usuario.Senha).Returns(false);

        var acao = async () => await _handler.Handle(command, CancellationToken.None);

        await acao.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("Credenciais inválidas");
    }

    [Fact]
    public async Task Deve_ChamarGeradorDeToken_Quando_AutenticacaoBemSucedida()
    {
        var email = "gerente@empresa.com";
        var senha = "Senha@123";
        var usuario = new Usuario { Id = 10, Nome = "Gerente", Email = email, Senha = "hash" };

        var command = new AutenticacaoUsuarioCommand { Email = email, Senha = senha };

        _repositorio.ObterPorEmailAsync(email, Arg.Any<CancellationToken>()).Returns(usuario);
        _passwordHasher.ValidaPassword(senha, usuario.Senha).Returns(true);
        _jwtTokenGenerator.GeradorDeToken(usuario).Returns("token_x");

        await _handler.Handle(command, CancellationToken.None);

        _jwtTokenGenerator.Received(1).GeradorDeToken(usuario);
    }

    [Fact]
    public async Task NaoDeve_ChamarGeradorDeToken_Quando_AutenticacaoFalhar()
    {
        var command = new AutenticacaoUsuarioCommand { Email = "nao@existe.com", Senha = "Senha@123" };

        _repositorio.ObterPorEmailAsync(command.Email, Arg.Any<CancellationToken>()).ReturnsNull();

        var acao = async () => await _handler.Handle(command, CancellationToken.None);

        await acao.Should().ThrowAsync<UnauthorizedAccessException>();
        _jwtTokenGenerator.DidNotReceive().GeradorDeToken(Arg.Any<IUsuario>());
    }
}
