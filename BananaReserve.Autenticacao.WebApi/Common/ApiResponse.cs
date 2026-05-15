using BananaReserve.Autenticacao.Common.Validacao;

namespace BananaReserve.Autenticacao.WebApi.Common;

public class ApiResponse
{
    public bool Sucesso { get; set; }
    public string Mensagem { get; set; } = string.Empty;
    public IEnumerable<ValidacaoDetalheErro> Errors { get; set; } = [];
}
