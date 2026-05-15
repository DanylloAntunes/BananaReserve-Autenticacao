using BananaReserve.Autenticacao.Domain.Validacoes;
using FluentValidation;

namespace BananaReserve.Autenticacao.WebApi.Features.Usuarios.CriarUsuario;

public class CriarUsuarioRequestValidator : AbstractValidator<CriarUsuarioRequest>
{
    public CriarUsuarioRequestValidator()
    {
        RuleFor(user => user.Email).SetValidator(new EmailValidator());
        RuleFor(user => user.Nome).NotEmpty().Length(3, 50);
        RuleFor(user => user.Senha).SetValidator(new SenhaValidator());
    }
}