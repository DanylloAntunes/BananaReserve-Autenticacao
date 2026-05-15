using FluentValidation;

namespace BananaReserve.Autenticacao.Application.Usuarios.RemoverUsuario;

public class RemoverUsuarioValidator : AbstractValidator<RemoverUsuarioCommand>
{
    public RemoverUsuarioValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("O Id do usuário é obrigatório");
    }
}
