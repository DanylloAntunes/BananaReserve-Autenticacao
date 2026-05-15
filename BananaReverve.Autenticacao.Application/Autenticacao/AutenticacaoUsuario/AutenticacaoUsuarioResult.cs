namespace BananaReserve.Autenticacao.Application.Autenticacao.AutenticacaoUsuario;

public sealed class AutenticacaoUsuarioResult
{
    public string Token { get; set; } = string.Empty;

    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
}
