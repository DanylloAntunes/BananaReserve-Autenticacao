using FluentValidation;

namespace BananaReserve.Autenticacao.Common.Validacao;

public static class Validator
{
    public static async Task<IEnumerable<ValidacaoDetalheErro>> ValidateAsync<T>(T instance)
    {
        Type validatorType = typeof(IValidator<>).MakeGenericType(typeof(T));

        if (Activator.CreateInstance(validatorType) is not IValidator validator)
        {
            throw new InvalidOperationException($"Validator não encontrado para: {typeof(T).Name}");
        }

        var result = await validator.ValidateAsync(new ValidationContext<T>(instance));

        if (!result.IsValid)
        {
            return result.Errors.Select(o => (ValidacaoDetalheErro)o);
        }

        return [];
    }
}
