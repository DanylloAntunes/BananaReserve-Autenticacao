using BananaReserve.Autenticacao.Application.Usuarios.CriarUsuario;
using BananaReserve.Autenticacao.Common.Seguracao;
using BananaReserve.Autenticacao.Domain.Entidades;
using BananaReserve.Autenticacao.Domain.Repositorios;
using Bogus;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace BananaReserve.Autenticacao.Unitario.Handlers;

public class CriarUsuarioHandlerTests
{
    private readonly IUsuarioRepositorio _repositorio;
    private readonly IPasswordHasher _passwordHasher;
    private readonly CriarUsuarioHandler _handler;
    private readonly Faker _faker;

    public CriarUsuarioHandlerTests()
    {
        _repositorio = Substitute.For<IUsuarioRepositorio>();
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _handler = new CriarUsuarioHandler(_repositorio, _passwordHasher);
        _faker = new Faker("pt_BR");
    }

    [Fact]
    public async Task Deve_CriarUsuario_Quando_DadosValidos()
    {
        var command = new CriarUsuarioCommand
        {
            Nome = "João Silva",
            Email = "joao.silva@email.com",
            Senha = "Senha@123"
        };

        var hashEsperado = "hash_bcrypt_gerado";
        var usuarioCriado = new Usuario { Id = 1, Nome = command.Nome, Email = command.Email, Senha = hashEsperado };

        _repositorio.ObterPorEmailAsync(command.Email, Arg.Any<CancellationToken>()).ReturnsNull();
        _passwordHasher.HashPassword(command.Senha).Returns(hashEsperado);
        _repositorio.CriarAsync(Arg.Any<Usuario>(), Arg.Any<CancellationToken>()).Returns(usuarioCriado);

        var resultado = await _handler.Handle(command, CancellationToken.None);

        resultado.Should().NotBeNull();
        resultado.Id.Should().Be(1);
        await _repositorio.Received(1).ObterPorEmailAsync(command.Email, Arg.Any<CancellationToken>());
        _passwordHasher.Received(1).HashPassword(command.Senha);
        await _repositorio.Received(1).CriarAsync(Arg.Any<Usuario>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Deve_LancarInvalidOperationException_Quando_EmailJaExiste()
    {
        var command = new CriarUsuarioCommand
        {
            Nome = "Maria Souza",
            Email = "maria.souza@email.com",
            Senha = "Senha@123"
        };

        var usuarioExistente = new Usuario { Id = 5, Email = command.Email, Nome = command.Nome };
        _repositorio.ObterPorEmailAsync(command.Email, Arg.Any<CancellationToken>()).Returns(usuarioExistente);

        var acao = async () => await _handler.Handle(command, CancellationToken.None);

        await acao.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"*{command.Email}*");
    }

    [Theory]
    [InlineData("", "usuario@email.com", "Senha@123")]
    [InlineData("AB", "usuario@email.com", "Senha@123")]
    [InlineData("Nome Válido", "email-invalido", "Senha@123")]
    [InlineData("Nome Válido", "usuario@email.com", "sem_maiuscula1@")]
    [InlineData("Nome Válido", "usuario@email.com", "curto")]
    public async Task Deve_LancarValidationException_Quando_DadosInvalidos(string nome, string email, string senha)
    {
        var command = new CriarUsuarioCommand
        {
            Nome = nome,
            Email = email,
            Senha = senha
        };

        var acao = async () => await _handler.Handle(command, CancellationToken.None);

        await acao.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    [Fact]
    public async Task Deve_HashearSenha_Antes_DeSalvar()
    {
        var command = new CriarUsuarioCommand
        {
            Nome = "Carlos Oliveira",
            Email = "carlos@email.com",
            Senha = "Senha@123"
        };

        const string hashGerado = "bcrypt_hash_xyz";
        _repositorio.ObterPorEmailAsync(command.Email, Arg.Any<CancellationToken>()).ReturnsNull();
        _passwordHasher.HashPassword(command.Senha).Returns(hashGerado);
        _repositorio.CriarAsync(Arg.Any<Usuario>(), Arg.Any<CancellationToken>())
            .Returns(callInfo => callInfo.Arg<Usuario>());

        await _handler.Handle(command, CancellationToken.None);

        await _repositorio.Received(1).CriarAsync(
            Arg.Is<Usuario>(u => u.Senha == hashGerado),
            Arg.Any<CancellationToken>());
    }
}
