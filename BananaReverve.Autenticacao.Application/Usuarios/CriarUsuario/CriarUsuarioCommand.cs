using BananaReserve.Autenticacao.Common.Validacao;
using MediatR;

namespace BananaReserve.Autenticacao.Application.Usuarios.CriarUsuario;

public class CriarUsuarioCommand : IRequest<CriarUsuarioResult>
{
    public string Nome { get; set; } = string.Empty;

    public string Senha { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public ValidacaoDetalheResult Validate()
    {
        var validator = new CriarUsuarioValidator();
        var result = validator.Validate(this);
        return new ValidacaoDetalheResult
        {
            EhValido = result.IsValid,
            Errors = result.Errors.Select(o => (ValidacaoDetalheErro)o)
        };
    }
}