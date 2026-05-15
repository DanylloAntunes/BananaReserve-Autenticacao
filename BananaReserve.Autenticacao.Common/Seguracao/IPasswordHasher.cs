using System;

namespace BananaReserve.Autenticacao.Common.Seguracao;

public interface IPasswordHasher
{
    string HashPassword(string password);

    bool ValidaPassword(string password, string hash);
}
