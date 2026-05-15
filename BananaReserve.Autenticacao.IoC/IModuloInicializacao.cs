using Microsoft.AspNetCore.Builder;

namespace BananaReserve.Autenticacao.IoC;

public interface IModuloInicializacao
{
    void Inicialize(WebApplicationBuilder builder);
}
