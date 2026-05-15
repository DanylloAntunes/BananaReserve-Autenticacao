namespace BananaReserve.Autenticacao.WebApi.Features.Autenticacao.AutenticacaoUsuario;

public sealed class AutenticacaoUsuarioResponse
{
    public string Token { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;   

    public string Nome { get; set; } = string.Empty;
}
