using BananaReserve.Autenticacao.Common.Seguracao;
using BananaReserve.Autenticacao.Common.Validacao;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace BananaReserve.Autenticacao.IoC.ModuloInicializacao;

public class ApplicationModuloInicializacao : IModuloInicializacao
{
    public void Inicialize(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();

        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidacaoBehavior<,>));

        builder.Services.AddJwtAuthentication(builder.Configuration);
    }
}