using BananaReserve.Autenticacao.Common.Validacao;

namespace BananaReserve.Autenticacao.Domain.Common;

public class BaseEntity : IComparable<BaseEntity>
{
    public int Id { get; set; }

    public Task<IEnumerable<ValidacaoDetalheErro>> ValidateAsync()
    {
        return Validator.ValidateAsync(this);
    }

    public int CompareTo(BaseEntity? other)
    {
        if (other == null)
        {
            return 1;
        }

        return other!.Id.CompareTo(Id);
    }
}
