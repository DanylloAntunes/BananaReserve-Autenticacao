using FluentValidation.Results;

namespace BananaReserve.Autenticacao.Common.Validacao;

public class ValidacaoDetalheErro
{
    public string Erro { get; init; } = string.Empty;
    public string Detalhe { get; init; } = string.Empty;

    public static explicit operator ValidacaoDetalheErro(ValidationFailure validationFailure)
    {
        return new ValidacaoDetalheErro
        {
            Detalhe = validationFailure.ErrorMessage,
            Erro = validationFailure.ErrorCode
        };
    }
}
