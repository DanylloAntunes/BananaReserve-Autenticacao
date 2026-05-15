using BananaReserve.Autenticacao.Application.Usuarios.RemoverUsuario;
using BananaReserve.Autenticacao.Domain.Repositorios;
using FluentAssertions;
using NSubstitute;

namespace BananaReserve.Autenticacao.Unitario.Handlers;

public class RemoverUsuarioHandlerTests
{
    private readonly IUsuarioRepositorio _repositorio;
    private readonly RemoverUsuarioHandler _handler;

    public RemoverUsuarioHandlerTests()
    {
        _repositorio = Substitute.For<IUsuarioRepositorio>();
        _handler = new RemoverUsuarioHandler(_repositorio);
    }

    [Fact]
    public async Task Deve_RetornarSucesso_Quando_UsuarioExiste()
    {
        var command = new RemoverUsuarioCommand(5);

        _repositorio.RemoverAsync(5, Arg.Any<CancellationToken>()).Returns(true);

        var resultado = await _handler.Handle(command, CancellationToken.None);

        resultado.Should().NotBeNull();
        resultado.Sucesso.Should().BeTrue();
    }

    [Fact]
    public async Task Deve_LancarKeyNotFoundException_Quando_UsuarioNaoExiste()
    {
        var command = new RemoverUsuarioCommand(99);

        _repositorio.RemoverAsync(99, Arg.Any<CancellationToken>()).Returns(false);

        var acao = async () => await _handler.Handle(command, CancellationToken.None);

        await acao.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("*99*");
    }

    [Fact]
    public async Task Deve_LancarValidationException_Quando_IdZero()
    {
        var command = new RemoverUsuarioCommand(0);

        var acao = async () => await _handler.Handle(command, CancellationToken.None);

        await acao.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    [Fact]
    public async Task Deve_ChamarRepositorio_Com_IdCorreto()
    {
        const int id = 7;
        var command = new RemoverUsuarioCommand(id);

        _repositorio.RemoverAsync(id, Arg.Any<CancellationToken>()).Returns(true);

        await _handler.Handle(command, CancellationToken.None);

        await _repositorio.Received(1).RemoverAsync(id, Arg.Any<CancellationToken>());
    }
}
