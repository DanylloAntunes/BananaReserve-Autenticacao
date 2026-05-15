using BananaReserve.Autenticacao.Domain.Repositorios;
using BananaReserve.Autenticacao.Infrastructure;
using BananaReserve.Autenticacao.Infrastructure.Repositorios;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BananaReserve.Autenticacao.IoC.ModuloInicializacao;

public class InfrastructureModuloInicializacao : IModuloInicializacao
{
    public void Inicialize(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<DefaultContext>());
        builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();

        builder.Services.AddDbContext<DefaultContext>(options =>
            options.UseNpgsql(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("BananaReserve.Autenticacao.Infrastructure")
            )
        );
    }
}