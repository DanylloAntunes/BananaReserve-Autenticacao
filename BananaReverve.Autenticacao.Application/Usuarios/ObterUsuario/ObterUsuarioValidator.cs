using FluentValidation;

namespace BananaReserve.Autenticacao.Application.Usuarios.ObterUsuario;

public class ObterUsuarioValidator : AbstractValidator<ObterUsuarioQuery>
{
    public ObterUsuarioValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("O Id do usuário é obrigatório");
    }
}
