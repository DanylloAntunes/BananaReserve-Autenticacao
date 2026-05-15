using BananaReserve.Autenticacao.Application.Usuarios.ObterUsuario;
using BananaReserve.Autenticacao.Domain.Entidades;
using BananaReserve.Autenticacao.Domain.Repositorios;
using Bogus;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace BananaReserve.Autenticacao.Unitario.Handlers;

public class ObterUsuarioHandlerTests
{
    private readonly IUsuarioRepositorio _repositorio;
    private readonly ObterUsuarioHandler _handler;
    private readonly Faker _faker;

    public ObterUsuarioHandlerTests()
    {
        _repositorio = Substitute.For<IUsuarioRepositorio>();
        _handler = new ObterUsuarioHandler(_repositorio);
        _faker = new Faker("pt_BR");
    }

    [Fact]
    public async Task Deve_RetornarUsuario_Quando_IdValido()
    {
        var usuario = new Usuario { Id = 1, Nome = "Fernanda Rocha", Email = "fernanda@email.com" };
        var query = new ObterUsuarioQuery(1);

        _repositorio.ObterPorIdAsync(1, Arg.Any<CancellationToken>()).Returns(usuario);

        var resultado = await _handler.Handle(query, CancellationToken.None);

        resultado.Should().NotBeNull();
        resultado.Id.Should().Be(usuario.Id);
        resultado.Nome.Should().Be(usuario.Nome);
        resultado.Email.Should().Be(usuario.Email);
    }

    [Fact]
    public async Task Deve_LancarKeyNotFoundException_Quando_UsuarioNaoExiste()
    {
        var query = new ObterUsuarioQuery(99);

        _repositorio.ObterPorIdAsync(99, Arg.Any<CancellationToken>()).ReturnsNull();

        var acao = async () => await _handler.Handle(query, CancellationToken.None);

        await acao.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("*99*");
    }

    [Fact]
    public async Task Deve_LancarValidationException_Quando_IdZero()
    {
        var query = new ObterUsuarioQuery(0);

        var acao = async () => await _handler.Handle(query, CancellationToken.None);

        await acao.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    [Fact]
    public async Task Deve_MapearTodosOsCampos_Quando_UsuarioEncontrado()
    {
        var faker = new Faker<Usuario>("pt_BR")
            .RuleFor(u => u.Id, f => f.Random.Int(1, 1000))
            .RuleFor(u => u.Nome, f => f.Name.FullName())
            .RuleFor(u => u.Email, f => f.Internet.Email());

        var usuario = faker.Generate();
        var query = new ObterUsuarioQuery(usuario.Id);

        _repositorio.ObterPorIdAsync(usuario.Id, Arg.Any<CancellationToken>()).Returns(usuario);

        var resultado = await _handler.Handle(query, CancellationToken.None);

        resultado.Id.Should().Be(usuario.Id);
        resultado.Nome.Should().Be(usuario.Nome);
        resultado.Email.Should().Be(usuario.Email);
    }
}
