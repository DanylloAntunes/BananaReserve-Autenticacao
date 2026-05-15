using FluentValidation.Results;

namespace BananaReserve.Autenticacao.Common.Validacao;

public class ValidacaoDetalheResult
{
    public bool EhValido { get; set; }
    public IEnumerable<ValidacaoDetalheErro> Errors { get; set; } = [];

    public ValidacaoDetalheResult()
    {
        
    }

    public ValidacaoDetalheResult(ValidationResult validationResult)
    {
        EhValido = validationResult.IsValid;
        Errors = validationResult.Errors.Select(o => (ValidacaoDetalheErro)o);
    }
}
