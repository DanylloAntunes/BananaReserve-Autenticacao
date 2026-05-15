using BananaReserve.Autenticacao.Common.Seguracao;
using BananaReserve.Autenticacao.Common.Validacao;
using BananaReserve.Autenticacao.Domain.Common;
using BananaReserve.Autenticacao.Domain.Validacoes;

namespace BananaReserve.Autenticacao.Domain.Entidades;

public class Usuario : BaseEntity, IUsuario
{
    public string Nome { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Senha { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    string IUsuario.Id => Id.ToString();

    public Usuario()
    {
        CreatedAt = DateTime.UtcNow;
    }

    public ValidacaoDetalheResult Validate()
    {
        var validator = new UsuarioValidator();
        var result = validator.Validate(this);
        return new ValidacaoDetalheResult
        {
            EhValido = result.IsValid,
            Errors = result.Errors.Select(o => (ValidacaoDetalheErro)o)
        };
    }
}