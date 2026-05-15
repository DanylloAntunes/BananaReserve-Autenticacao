namespace BananaReserve.Autenticacao.WebApi.Features.Autenticacao.AutenticacaoUsuario;

public class AutenticacaoUsuarioRequest
{
    public string Email { get; set; } = string.Empty;

    public string Senha { get; set; } = string.Empty;
}
