using BananaReserve.Autenticacao.Domain.Entidades;
using FluentValidation;

namespace BananaReserve.Autenticacao.Domain.Validacoes;

public class UsuarioValidator : AbstractValidator<Usuario>
{
    public UsuarioValidator()
    {
        RuleFor(user => user.Email).SetValidator(new EmailValidator());

        RuleFor(user => user.Nome)
            .NotEmpty()
            .MinimumLength(3).WithMessage("O nome de usuário deve ter pelo menos 3 caracteres.")
            .MaximumLength(50).WithMessage("O nome de usuário não pode ter mais de 50 caracteres.");
        
        RuleFor(user => user.Senha).SetValidator(new SenhaValidator());
        
    }
}
