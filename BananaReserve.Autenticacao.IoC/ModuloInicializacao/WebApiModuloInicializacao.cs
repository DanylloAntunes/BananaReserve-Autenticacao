using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace BananaReserve.Autenticacao.IoC.ModuloInicializacao;

public class WebApiModuloInicializacao : IModuloInicializacao
{
    public void Inicialize(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
    }
}

