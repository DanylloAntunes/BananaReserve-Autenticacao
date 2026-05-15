using BananaReserve.Autenticacao.Domain.Validacoes;
using FluentValidation;

namespace BananaReserve.Autenticacao.Application.Usuarios.CriarUsuario;

public class CriarUsuarioValidator : AbstractValidator<CriarUsuarioCommand>
{
    public CriarUsuarioValidator()
    {
        RuleFor(user => user.Email).SetValidator(new EmailValidator());
        RuleFor(user => user.Nome).NotEmpty().Length(3, 50);
        RuleFor(user => user.Senha).SetValidator(new SenhaValidator());
    }
}