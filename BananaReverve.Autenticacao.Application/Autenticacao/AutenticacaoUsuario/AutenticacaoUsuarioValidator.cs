using FluentValidation;

namespace BananaReserve.Autenticacao.Application.Autenticacao.AutenticacaoUsuario;

public class AutenticacaoUsuarioValidator : AbstractValidator<AutenticacaoUsuarioCommand>
{
    public AutenticacaoUsuarioValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Senha)
            .NotEmpty()
            .MinimumLength(6);
    }
}

