namespace BananaReserve.Autenticacao.Common.Seguracao;

public interface IJwtTokenGenerator
{
    string GeradorDeToken(IUsuario user);
}

