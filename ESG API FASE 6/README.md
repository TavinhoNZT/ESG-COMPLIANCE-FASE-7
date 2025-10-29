# ESG Compliance API

API .NET 8 para gestão de Licenças e Auditorias Ambientais (ESG), com Docker, Docker Compose e pipeline CI/CD no GitHub Actions.

## Como executar localmente com Docker

- Pré-requisitos: `Docker` e `Docker Compose` instalados.
- Copie `.env.example` para `.env` e ajuste `SQL_SA_PASSWORD` se necessário.
- Suba os serviços:

```
docker compose up -d --build
```

- A API sobe em `http://localhost:8080` e o Swagger em `http://localhost:8080/swagger`.

## Pipeline CI/CD

- Ferramenta: GitHub Actions (`.github/workflows/ci-cd.yml`).
- Disparos:
  - `push` em `main`: executa build, testes, publica imagem no GHCR e faz deploy em staging via SSH.
  - `push` de `tags` no formato `v*`: executa build, testes, publica imagem no GHCR e faz deploy em produção via SSH.
- Jobs:
  - `build_and_test`: `dotnet restore`, `dotnet build`, `dotnet test`.
  - `build_and_push_image`: build/push da imagem usando `ESG.Compliance.Api/Dockerfile` e tags `staging` ou `prod`.
  - `deploy_staging` e `deploy_production`: copiam os `docker-compose` de `deploy/` para os servidores, fazem `docker login` no GHCR, `docker compose pull` e `up -d`.
- Secrets necessários:
  - `GHCR_USERNAME` e `GHCR_TOKEN` (token com permissão de `write:packages`).
  - `SSH_HOST_STAGING`, `SSH_USER_STAGING`, `SSH_KEY_STAGING`, `SQL_SA_PASSWORD_STAGING`.
  - `SSH_HOST_PRODUCTION`, `SSH_USER_PRODUCTION`, `SSH_KEY_PRODUCTION`, `SQL_SA_PASSWORD_PRODUCTION`.

## Containerização

- Dockerfile: `ESG.Compliance.Api/Dockerfile` (multi-stage build). Ajustes:
  - Remove o `USER $APP_UID` para compatibilidade geral.
  - Define `ASPNETCORE_URLS=http://+:8080`.
- Compose local: `docker-compose.yml` com serviços:
  - `api`: constrói a imagem e injeta `ConnectionStrings__DefaultConnection` por variável de ambiente.
  - `db`: `mcr.microsoft.com/mssql/server:2022-latest`, com volume persistente.

## Prints do funcionamento

- Inclua imagens do pipeline rodando (build, teste e deploy), e da aplicação acessível em `staging` e `produção` (Swagger e um endpoint, por exemplo `/api/licencas`).

## Tecnologias utilizadas

- .NET 8, ASP.NET Core, Entity Framework Core (SQL Server).
- Docker, Docker Compose.
- GitHub Actions, GHCR.

## Desenvolvimento e Migrações

- O `Program.cs` aplica `Database.Migrate()` automaticamente apenas quando o provider é relacional (`db.Database.IsRelational()`), funcionando bem em ambiente de container.
- Testes usam provider InMemory para evitar dependência de SQL Server no CI.

## Deploy

- Staging: `deploy/staging/docker-compose.staging.yml` — expõe `8080`.
- Produção: `deploy/production/docker-compose.production.yml` — expõe `80`.
- Ambos aguardam variáveis via `.env` remoto:
  - `GHCR_OWNER`, `SQL_SA_PASSWORD`, `DB_NAME`.
