using BananaReserve.Autenticacao.Application.Usuarios.CriarUsuario;
using FluentAssertions;

namespace BananaReserve.Autenticacao.Unitario.Validators;

public class CriarUsuarioValidatorTests
{
    private readonly CriarUsuarioValidator _validator;

    public CriarUsuarioValidatorTests()
    {
        _validator = new CriarUsuarioValidator();
    }

    [Fact]
    public async Task Deve_SerValido_Quando_DadosCorretos()
    {
        var command = new CriarUsuarioCommand
        {
            Nome = "Thiago Neves",
            Email = "thiago.neves@email.com",
            Senha = "Senha@123"
        };

        var resultado = await _validator.ValidateAsync(command);

        resultado.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("AB")]
    [InlineData("A")]
    public async Task Deve_FalharValidacao_Quando_NomeInvalido(string nome)
    {
        var command = new CriarUsuarioCommand
        {
            Nome = nome,
            Email = "email@valido.com",
            Senha = "Senha@123"
        };

        var resultado = await _validator.ValidateAsync(command);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().Contain(e => e.PropertyName == nameof(CriarUsuarioCommand.Nome));
    }

    [Theory]
    [InlineData("email_invalido")]
    [InlineData("sem_arroba.com")]
    [InlineData("")]
    [InlineData("@semdominio.com")]
    public async Task Deve_FalharValidacao_Quando_EmailInvalido(string email)
    {
        var command = new CriarUsuarioCommand
        {
            Nome = "Nome Válido",
            Email = email,
            Senha = "Senha@123"
        };

        var resultado = await _validator.ValidateAsync(command);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().Contain(e => e.PropertyName == nameof(CriarUsuarioCommand.Email));
    }

    [Theory]
    [InlineData("curta")]
    [InlineData("semuppercase1@")]
    [InlineData("SEMMINUSCULA1@")]
    [InlineData("SemNumero@Ab")]
    [InlineData("SemEspecial123")]
    public async Task Deve_FalharValidacao_Quando_SenhaInvalida(string senha)
    {
        var command = new CriarUsuarioCommand
        {
            Nome = "Nome Válido",
            Email = "email@valido.com",
            Senha = senha
        };

        var resultado = await _validator.ValidateAsync(command);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().Contain(e => e.PropertyName == nameof(CriarUsuarioCommand.Senha));
    }

    [Fact]
    public async Task Deve_RetornarMultiplosErros_Quando_TodosDadosInvalidos()
    {
        var command = new CriarUsuarioCommand
        {
            Nome = "",
            Email = "invalido",
            Senha = "fraca"
        };

        var resultado = await _validator.ValidateAsync(command);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().HaveCountGreaterThan(1);
    }
}
