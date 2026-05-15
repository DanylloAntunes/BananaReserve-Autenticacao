using BananaReserve.Autenticacao.Application.Autenticacao.AutenticacaoUsuario;
using FluentAssertions;

namespace BananaReserve.Autenticacao.Unitario.Validators;

public class AutenticacaoUsuarioValidatorTests
{
    private readonly AutenticacaoUsuarioValidator _validator;

    public AutenticacaoUsuarioValidatorTests()
    {
        _validator = new AutenticacaoUsuarioValidator();
    }

    [Fact]
    public async Task Deve_SerValido_Quando_DadosCorretos()
    {
        var command = new AutenticacaoUsuarioCommand
        {
            Email = "usuario@email.com",
            Senha = "Senha123"
        };

        var resultado = await _validator.ValidateAsync(command);

        resultado.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("nao_e_email")]
    [InlineData("@semlocal.com")]
    public async Task Deve_FalharValidacao_Quando_EmailInvalido(string email)
    {
        var command = new AutenticacaoUsuarioCommand
        {
            Email = email,
            Senha = "Senha123"
        };

        var resultado = await _validator.ValidateAsync(command);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().Contain(e => e.PropertyName == nameof(AutenticacaoUsuarioCommand.Email));
    }

    [Theory]
    [InlineData("")]
    [InlineData("12345")]
    public async Task Deve_FalharValidacao_Quando_SenhaMuitoCurta(string senha)
    {
        var command = new AutenticacaoUsuarioCommand
        {
            Email = "usuario@email.com",
            Senha = senha
        };

        var resultado = await _validator.ValidateAsync(command);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().Contain(e => e.PropertyName == nameof(AutenticacaoUsuarioCommand.Senha));
    }

    [Fact]
    public async Task Deve_FalharValidacao_Quando_EmailESenhaVazios()
    {
        var command = new AutenticacaoUsuarioCommand
        {
            Email = string.Empty,
            Senha = string.Empty
        };

        var resultado = await _validator.ValidateAsync(command);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().HaveCountGreaterThanOrEqualTo(2);
    }
}
