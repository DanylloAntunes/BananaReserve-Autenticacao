# BananaReserve - Autenticacao

## Objetivo

API de autenticacao e gerenciamento de usuarios para o sistema BananaReserve. Responsavel por cadastro, login com JWT e exclusao de usuarios, servindo como servico base de identidade para os demais modulos da plataforma.

---

## Tecnologias

- **.NET 8** — framework principal
- **ASP.NET Core** — camada web e controllers
- **PostgreSQL 13** — banco de dados relacional
- **Entity Framework Core 8** — ORM e migrations
- **MediatR 12** — padrao CQRS com commands e queries
- **FluentValidation 11** — validacao de entrada
- **BCrypt.Net** — hash de senhas
- **JWT Bearer** — autenticacao stateless
- **Swagger / Swashbuckle** — documentacao interativa da API
- **Docker / Docker Compose** — containerizacao
- **xUnit + NSubstitute + Bogus + FluentAssertions** — testes unitarios

---

## Arquitetura

Clean Architecture em camadas com CQRS via MediatR:

```
BananaReserve.Autenticacao.WebApi          # Controllers, middlewares, requests/responses
BananaReserve.Autenticacao.IoC             # Injecao de dependencias
BananaReverve.Autenticacao.Application     # Handlers, commands, queries, validators
BananaReserve.Autenticacao.Domain          # Entidades, interfaces de repositorio, validadores de dominio
BananaReserve.Autenticacao.Infrastructure  # DbContext, implementacao dos repositorios
BananaReserve.Autenticacao.Common          # JWT, PasswordHasher, pipeline de validacao
BananaReserve.Autenticacao.Unitario        # Testes unitarios
```

O fluxo de uma requisicao segue: `Controller → MediatR → Handler → Repositorio → Banco`.

---

## Decisoes Tecnicas

- **CQRS com MediatR**: commands e queries separados por responsabilidade, handlers coesos e faceis de testar isoladamente.
- **Validacao em dois niveis**: FluentValidation valida os commands na camada de aplicacao; validators de dominio garantem a integridade da entidade antes de persistir.
- **BCrypt para senhas**: algoritmo de hashing adaptativo com salt embutido, resistente a ataques de forca bruta.
- **JWT stateless**: tokens com expiracao de 8 horas, sem necessidade de sessao no servidor.
- **Middleware global de excecoes**: `ValidacaoExcecaoMiddleware` intercepta `ValidationException`, `KeyNotFoundException` e `InvalidOperationException` retornando respostas padronizadas sem poluir os controllers.
- **Stage `migrate` no Dockerfile**: migrations executadas em container separado antes de subir a API, garantindo que o banco esteja atualizado antes do inicio da aplicacao.

---

## Variaveis de Ambiente

| Variavel | Descricao | Exemplo |
|---|---|---|
| `ASPNETCORE_ENVIRONMENT` | Ambiente da aplicacao | `Development` |
| `ConnectionStrings__DefaultConnection` | String de conexao do PostgreSQL | `Host=...;Port=5432;Database=...;Username=...;Password=...` |
| `Jwt__SecretKey` | Chave secreta para assinar os tokens JWT (minimo 32 caracteres) | `SuaChaveSecretaAqui32caracteres!` |

---

## Como Executar

### Pre-requisitos

- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (para rodar pelo Visual Studio ou CLI)

---

### Pelo Docker (recomendado)

Sobe o banco, executa as migrations e inicia a API automaticamente:

```bash
docker-compose up -d
```

A API ficara disponivel em `http://localhost:8080/swagger`.

Para acompanhar os logs:

```bash
docker-compose logs -f bananareserve.autenticacao.webapi
```

Para parar e remover os containers:

```bash
docker-compose down
```

---

### Pelo Visual Studio

1. Abra o arquivo `BananaReserve.Autenticacao.slnx`
2. Defina `BananaReserve.Autenticacao.WebApi` como projeto de inicializacao
3. Configure o `appsettings.Development.json` com a connection string apontando para um PostgreSQL local (veja a secao de variaveis de ambiente)
4. Execute as migrations (veja secao abaixo)
5. Pressione `F5` ou selecione o perfil `https`

A API abrira o Swagger automaticamente em `https://localhost:7181/swagger`.

---

### Pelo .NET CLI

```bash
# Restaurar dependencias
dotnet restore

# Rodar a API
dotnet run --project BananaReserve.Autenticacao.WebApi/BananaReserve.Autenticacao.WebApi.csproj
```

---

### Executar Migrations (Update Database)

Certifique-se de que o PostgreSQL esta rodando e a connection string esta configurada no `appsettings.Development.json`.

```bash
# Aplicar migrations no banco
dotnet ef database update \
  --project BananaReserve.Autenticacao.Infrastructure \
  --startup-project BananaReserve.Autenticacao.WebApi

# Criar nova migration
dotnet ef migrations add NomeDaMigration \
  --project BananaReserve.Autenticacao.Infrastructure \
  --startup-project BananaReserve.Autenticacao.WebApi
```

> Pelo Docker, as migrations sao aplicadas automaticamente pelo servico `bananareserve.autenticacao.migrator` antes da API iniciar.

---

## Endpoints

| Metodo | Rota | Descricao | Auth |
|---|---|---|---|
| `POST` | `/api/autenticacao` | Login, retorna JWT | Nao |
| `POST` | `/api/usuarios` | Cadastrar usuario | Nao |
| `GET` | `/api/usuarios/{id}` | Buscar usuario por ID | Sim |
| `DELETE` | `/api/usuarios/{id}` | Remover usuario | Sim |

---

## Testes

```bash
# Rodar todos os testes unitarios
dotnet test BananaReserve.Autenticacao.Unitario/BananaReserve.Autenticacao.Unitario.csproj

# Com relatorio de cobertura
dotnet test BananaReserve.Autenticacao.Unitario/BananaReserve.Autenticacao.Unitario.csproj --collect:"XPlat Code Coverage"
```

---

## Melhorias Futuras

- **Refresh token**: renovacao do JWT sem novo login, melhorando a experiencia do usuario
- **Revogacao de tokens**: blacklist de tokens invalidos para suporte a logout real
- **Rate limiting**: protecao contra brute force nos endpoints de autenticacao
- **Observabilidade**: traces distribuidos com OpenTelemetry e metricas expostas para Prometheus
- **Soft delete**: inativacao de usuarios em vez de exclusao fisica, preservando historico
- **Paginacao**: listagem de usuarios com suporte a filtros e paginacao
- **Cache**: Redis para tokens validados com alta frequencia
- **Audit log**: registro de acoes sensiveis (login, exclusao) para rastreabilidade
