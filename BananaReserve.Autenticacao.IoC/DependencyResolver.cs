using BananaReserve.Autenticacao.IoC.ModuloInicializacao;
using Microsoft.AspNetCore.Builder;

namespace BananaReserve.Autenticacao.IoC;

public static class DependencyResolver
{
    public static void RegisterDependencies(this WebApplicationBuilder builder)
    {
        new ApplicationModuloInicializacao().Inicialize(builder);
        new InfrastructureModuloInicializacao().Inicialize(builder);
        new WebApiModuloInicializacao().Inicialize(builder);
    }
}