using FluentValidation;

namespace BananaReserve.Autenticacao.WebApi.Features.Autenticacao.AutenticacaoUsuario;

public class AutenticacaoUsuarioRequestValidator : AbstractValidator<AutenticacaoUsuarioRequest>
{
    public AutenticacaoUsuarioRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("O e-mail é obrigatório")
            .EmailAddress()
            .WithMessage("Formato de e-mail inválido");

        RuleFor(x => x.Senha)
            .NotEmpty()
            .WithMessage("A senha é obrigatória");
    }
}
