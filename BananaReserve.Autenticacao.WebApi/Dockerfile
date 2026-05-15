FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BananaReserve.Autenticacao.WebApi/BananaReserve.Autenticacao.WebApi.csproj", "BananaReserve.Autenticacao.WebApi/"]
COPY ["BananaReserve.Autenticacao.IoC/BananaReserve.Autenticacao.IoC.csproj", "BananaReserve.Autenticacao.IoC/"]
COPY ["BananaReserve.Autenticacao.Domain/BananaReserve.Autenticacao.Domain.csproj", "BananaReserve.Autenticacao.Domain/"]
COPY ["BananaReserve.Autenticacao.Common/BananaReserve.Autenticacao.Common.csproj", "BananaReserve.Autenticacao.Common/"]
COPY ["BananaReverve.Autenticacao.Application/BananaReserve.Autenticacao.Application.csproj", "BananaReverve.Autenticacao.Application/"]
COPY ["BananaReserve.Autenticacao.Infrastructure/BananaReserve.Autenticacao.Infrastructure.csproj", "BananaReserve.Autenticacao.Infrastructure/"]
RUN dotnet restore "BananaReserve.Autenticacao.WebApi/BananaReserve.Autenticacao.WebApi.csproj"
COPY . .
WORKDIR "/src/BananaReserve.Autenticacao.WebApi"
RUN dotnet build "BananaReserve.Autenticacao.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BananaReserve.Autenticacao.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM build AS migrate
RUN dotnet tool install --global dotnet-ef --version 8.0.14
ENV PATH="${PATH}:/root/.dotnet/tools"
WORKDIR /src
ENTRYPOINT ["dotnet", "ef", "database", "update", "--project", "BananaReserve.Autenticacao.Infrastructure", "--startup-project", "BananaReserve.Autenticacao.WebApi"]

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BananaReserve.Autenticacao.WebApi.dll"]