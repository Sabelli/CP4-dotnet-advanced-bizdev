# maetS API — .NET

API RESTful desenvolvida em **ASP.NET Core** para o gerenciamento de um catálogo de jogos, como Checkpoint 4 (CP4) da disciplina **Advanced Business Development with .NET**.

A API é responsável pelo cadastro e manutenção de **jogos, categorias, desenvolvedoras e plataformas**, com relacionamentos N:N (Jogo↔Categoria, Jogo↔Plataforma) e N:1 (Jogo→Desenvolvedora).

O projeto é organizado em **Clean Architecture** (Domain / Application / Infrastructure / Presentation), com **paginação** em todos os endpoints de listagem, **índices de banco de dados**, **rate limiting**, **compressão de resposta** (Brotli / Gzip), **observabilidade** (logging estruturado com Serilog, health checks e Application Insights) e uma suíte de **testes** (xUnit) cobrindo repositories, use cases e controllers.

---

## Integrantes

| Nome | RM |
|------|----|
| Victor Sabelli | RM566224 |
| Gustavo Crevelari | RM561408 |
| Lucca Gomes | RM561996 |
| Rafaela Ferreira | RM561671 |

---

## Repositório

[![GitHub](https://img.shields.io/badge/GitHub-Acessar%20Repositório-181717?style=for-the-badge&logo=github&logoColor=white)](https://github.com/Sabelli/CP4-dotnet-advanced-bizdev.git)

---

## Estrutura do Projeto

```
CP4-dotnet-advanced-bizdev/
├── Jogos.slnx                          # solução
├── Jogos.API/                          # API ASP.NET Core
│   ├── Program.cs
│   ├── Domain/
│   │   ├── Entities/                   # JogoEntity, CategoriaEntity, DesenvolvedoraEntity, PlataformaEntity
│   │   ├── Interfaces/                 # contratos de repositório
│   │   └── Models/                     # PageResultModel<T>
│   ├── Application/
│   │   ├── Dtos/                       # DTOs de request e response
│   │   ├── Interfaces/                 # contratos de use case
│   │   ├── Mappers/                    # DTO -> Entity
│   │   └── UseCases/                   # regras de negócio
│   ├── Infrastructure/
│   │   └── Data/
│   │       ├── ApplicationContext.cs   # DbContext (EF Core + Oracle)
│   │       ├── Migrations/             # migrations do banco Oracle
│   │       └── Repositories/           # acesso a dados
│   ├── Presentation/
│   │   └── Controllers/                # endpoints REST
│   └── Doc/Samples/                    # exemplos de request/response do Swagger
└── Jogos.Test/                         # testes automatizados (xUnit)
    └── App/                            # unidade (repositories, use cases) + funcionais (controllers)
```

Divisão em camadas:

- **Domain** — entidades e contratos, sem dependência de frameworks.
- **Application** — use cases (regras de negócio), DTOs e mappers.
- **Infrastructure** — `ApplicationContext` (EF Core / Oracle), repositories e migrations.
- **Presentation** — controllers que expõem o HTTP.

---

## Modelagem

- `Jogo` **N:N** `Categoria` — um jogo pode ter várias categorias, uma categoria vários jogos.
- `Jogo` **N:N** `Plataforma` — um jogo pode estar disponível em várias plataformas (PC, PS5, Xbox...), uma plataforma tem vários jogos.
- `Jogo` **N:1** `Desenvolvedora` — um jogo tem uma desenvolvedora, uma desenvolvedora tem vários jogos.

Índices: `Nome` único em `Categoria`, `Desenvolvedora` e `Plataforma`; `Nome` único em `Jogo`. Duplicata é bloqueada na aplicação (checagem case-insensitive antes do insert) e reforçada pelo índice único no banco.

---

## Tecnologias

- .NET 8 / ASP.NET Core Web API (Controllers)
- Entity Framework Core 8 + Oracle.EntityFrameworkCore
- Oracle Database (FIAP)
- Microsoft.AspNetCore.RateLimiting — fixed window limiter
- Microsoft.AspNetCore.ResponseCompression — Brotli + Gzip
- Swashbuckle.AspNetCore (Swagger/OpenAPI) + `.Annotations` e `.Filters` (exemplos de request/response)
- **Observabilidade:**
  - Serilog (`Serilog.AspNetCore`) — logging estruturado para console e arquivo rotativo em disco
  - `AspNetCore.HealthChecks.Oracle` — health checks de liveness e readiness
  - Application Insights via OpenTelemetry (`Azure.Monitor.OpenTelemetry.AspNetCore`)
- **Testes:** xUnit, Moq, EF Core InMemory, Microsoft.AspNetCore.Mvc.Testing

---

## Instalação e Execução

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Acesso ao banco Oracle da FIAP (`oracle.fiap.com.br`)

### 1. Clone o repositório

```bash
git clone https://github.com/Sabelli/CP4-dotnet-advanced-bizdev.git
cd CP4-dotnet-advanced-bizdev
```

### 2. Configure a string de conexão

Em `Jogos.API/appsettings.Development.json`, preencha `ConnectionStrings:Oracle` com seu usuário e senha do Oracle FIAP:

```json
{
  "ConnectionStrings": {
    "Oracle": "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=oracle.fiap.com.br)(PORT=1521))) (CONNECT_DATA=(SERVER=DEDICATED)(SID=ORCL)));User Id=SEU_RM;Password=SUA_SENHA;"
  }
}
```

### 3. Migrations

Migrations em `Jogos.API/Infrastructure/Data/Migrations/`:

- `JogoCategoriaDesenvolvedora` — schema inicial (Jogo, Categoria, Desenvolvedora)
- `JogoPlataformaRefatoracao` — Plataforma vira entidade N:N com Jogo (era campo string)
- `PlataformaAumentaMin` — ajuste de tamanho mínimo do nome da Plataforma

Aplicar no banco (Package Manager Console, projeto padrão `Jogos.API`):

```powershell
Update-Database
```

### 4. Execute a API

```bash
dotnet run --project Jogos.API
```

A API estará disponível em `https://localhost:7001` / `http://localhost:5101` (portas em `Jogos.API/Properties/launchSettings.json`).

### 5. Acesse o Swagger

```
https://localhost:7001/swagger
```

---

## Testes

Testes automatizados com **xUnit** no projeto `Jogos.Test` — 65 testes, cobrindo:

- **Repositories** — testes de unidade via EF Core InMemory (`Microsoft.EntityFrameworkCore.InMemory`).
- **Use Cases** — testes de unidade com os repositórios mockados via **Moq**.
- **Controllers** — testes funcionais (integração) via `WebApplicationFactory` (`Microsoft.AspNetCore.Mvc.Testing`), com os use cases mockados.

Cobre as 4 entidades: `Categoria`, `Desenvolvedora`, `Plataforma` e `Jogo` (incluindo vínculo/desvínculo N:N em lote e o caso de id inexistente na lista, que lança `EntidadeNaoEncontradaException` → `404`).

### Executar todos os testes

```bash
dotnet test Jogos.Test/Jogos.Test.csproj
```

### Filtrar por trait

Os testes usam traits `Repository`, `UseCase` e `Controller`, cada um categorizado por entidade:

```bash
dotnet test --filter "Repository=Jogo"
dotnet test --filter "UseCase=Categoria"
dotnet test --filter "Controller=Plataforma"
```

---

## Comportamentos Transversais

### Paginação

Todos os endpoints de listagem (inclusive os filtros de `Jogo`) aceitam `Deslocamento` e `RegistroRetornado`:

| Param | Default | Regras |
|-------|---------|--------|
| `Deslocamento` | `0` | valores negativos são tratados como `0` |
| `RegistroRetornado` | `50` | aplicado quando vem `0` ou negativo |

Exemplo: `GET /api/jogo?Deslocamento=0&RegistroRetornado=10`

A resposta (`200 OK`) vem em um envelope com metadados de paginação (`PageResultModel<T>`), ou `204 No Content` quando vazio:

```json
{
  "data": [ { "id": 1, "nome": "The Witcher 3" } ],
  "deslocamento": 0,
  "registroRetornado": 50,
  "totalRegistros": 1
}
```

Ordenação por `Id` ascendente.

### Rate Limiting

Fixed window particionado por IP do cliente (`politica_5_tentativas`), aplicada em todo GET de listagem/filtro: **5 requisições a cada 20 segundos por IP**, fila de 2. Ao exceder: **429 Too Many Requests**.

Desabilitado no ambiente `Testing` (`Program.cs`, guardado por `IsEnvironment("Testing")`) para a suíte de testes não compartilhar o mesmo bucket entre os métodos de uma mesma classe de teste.

### Compressão de Resposta

Brotli e Gzip habilitados (nível `Fastest`), negociados via header `Accept-Encoding`.

---

## Observabilidade

### Logging (Serilog)

Console + arquivo (`logs/api-<data>.log`, rotação diária, retenção de 7 arquivos). Cada controller e use case recebe `ILogger<T>` por injeção de dependência:

| Nível | Uso |
|-------|-----|
| `LogInformation` | entrada nos métodos, eventos de negócio |
| `LogWarning` | recurso não encontrado (404), duplicata (409) |
| `LogError` | exceções inesperadas, antes de retornar 400 |

### Health Check

| Endpoint | Tipo | 200 | 503 |
|----------|------|-----|-----|
| `GET /health/live` | liveness | processo saudável | processo travado |
| `GET /health/db` | readiness | banco acessível | banco indisponível |
| `GET /api/health2/live` | liveness (JSON detalhado) | idem, com `{ status, checks[] }` | idem |
| `GET /api/health2/db` | readiness (JSON detalhado) | idem | idem |

### Application Insights

Telemetria via OpenTelemetry + Azure Monitor. Connection string em `ApplicationInsights:ConnectionString` (`appsettings.json`). Vazia por padrão — a telemetria só é ativada se preenchida, sem quebrar a execução local.

---

## Rotas

### Categorias — `/api/categoria`

| Método | Rota | Descrição | Retorno |
|--------|------|-----------|---------|
| GET | `/api/categoria` | Lista categorias (paginado) | 200 / 204 / 400 |
| GET | `/api/categoria/{id}` | Busca categoria por id | 200 / 404 / 400 |
| POST | `/api/categoria` | Cria categoria | 201 / 409 / 400 |
| PUT | `/api/categoria/{id}` | Atualiza categoria | 200 / 404 / 409 / 400 |
| DELETE | `/api/categoria/{id}` | Remove categoria | 200 / 404 / 400 |

**POST / PUT — Body:**
```json
{ "nome": "RPG" }
```

---

### Desenvolvedoras — `/api/desenvolvedora`

| Método | Rota | Descrição | Retorno |
|--------|------|-----------|---------|
| GET | `/api/desenvolvedora` | Lista desenvolvedoras (paginado) | 200 / 204 / 400 |
| GET | `/api/desenvolvedora/{id}` | Busca desenvolvedora por id | 200 / 404 / 400 |
| POST | `/api/desenvolvedora` | Cria desenvolvedora | 201 / 409 / 400 |
| PUT | `/api/desenvolvedora/{id}` | Atualiza desenvolvedora | 200 / 404 / 409 / 400 |
| DELETE | `/api/desenvolvedora/{id}` | Remove desenvolvedora | 200 / 404 / 400 |

**POST / PUT — Body:**
```json
{ "nome": "CD Projekt Red" }
```

---

### Plataformas — `/api/plataforma`

| Método | Rota | Descrição | Retorno |
|--------|------|-----------|---------|
| GET | `/api/plataforma` | Lista plataformas (paginado) | 200 / 204 / 400 |
| GET | `/api/plataforma/{id}` | Busca plataforma por id | 200 / 404 / 400 |
| POST | `/api/plataforma` | Cria plataforma | 201 / 409 / 400 |
| PUT | `/api/plataforma/{id}` | Atualiza plataforma | 200 / 404 / 409 / 400 |
| DELETE | `/api/plataforma/{id}` | Remove plataforma | 200 / 404 / 400 |

**POST / PUT — Body:**
```json
{ "nome": "PC" }
```
> Nome aceita mínimo de 2 caracteres (diferente das demais entidades, que exigem 3) — pra aceitar siglas como "PC".

---

### Jogos — `/api/jogo`

| Método | Rota | Descrição | Retorno |
|--------|------|-----------|---------|
| GET | `/api/jogo` | Lista jogos (paginado) | 200 / 204 / 400 |
| GET | `/api/jogo/{id}` | Busca jogo por id | 200 / 404 / 400 |
| GET | `/api/jogo/nome/{nome}` | Busca jogos por nome (parcial) | 200 / 204 / 400 |
| GET | `/api/jogo/plataforma/{idPlataforma}` | Lista jogos por plataforma | 200 / 204 / 404 / 400 |
| GET | `/api/jogo/desenvolvedora/{idDesenvolvedora}` | Lista jogos por desenvolvedora | 200 / 204 / 404 / 400 |
| GET | `/api/jogo/categoria/{idCategoria}` | Lista jogos por categoria | 200 / 204 / 404 / 400 |
| POST | `/api/jogo` | Cria jogo | 201 / 404 / 409 / 400 |
| PUT | `/api/jogo/{id}` | Atualiza jogo | 200 / 404 / 409 / 400 |
| DELETE | `/api/jogo/{id}` | Remove jogo | 200 / 404 / 400 |
| POST | `/api/jogo/categoria/{idJogo}` | Vincula categorias existentes (lote) | 200 / 404 / 400 |
| DELETE | `/api/jogo/categoria/{idJogo}` | Desvincula categorias (lote) | 200 / 404 / 400 |
| POST | `/api/jogo/plataforma/{idJogo}` | Vincula plataformas existentes (lote) | 200 / 404 / 400 |
| DELETE | `/api/jogo/plataforma/{idJogo}` | Desvincula plataformas (lote) | 200 / 404 / 400 |

> As listagens/filtros aceitam `?Deslocamento=&RegistroRetornado=` — ver [Comportamentos Transversais](#comportamentos-transversais).
> Filtros por `plataforma`/`desenvolvedora`/`categoria` retornam `404` se o id não existir, e `204` se existir mas não tiver jogos vinculados.
> Vínculos de Categoria/Plataforma são opcionais no `POST /api/jogo` (`categoriaIds`/`plataformaIds`) ou feitos depois pelos endpoints dedicados, que recebem uma lista de ids no corpo. Se algum id da lista não existir, a requisição inteira falha com `404` (mensagem lista o(s) id(s) faltante(s)) — mesma validação estrita aplicada ao `POST /api/jogo`. `PUT` não altera vínculos.
> `POST /api/jogo` valida `desenvolvedoraId`, `categoriaIds` e `plataformaIds` juntos: se mais de um vier inválido ao mesmo tempo, o `404` retorna todas as mensagens de erro concatenadas numa única resposta, em vez de parar no primeiro problema.

**POST/DELETE `/api/jogo/categoria/{idJogo}` e `/api/jogo/plataforma/{idJogo}` — Body:**
```json
[1, 2, 3]
```

**POST — Body:**
```json
{
  "nome": "The Witcher 3",
  "preco": 59.99,
  "dataLancamento": "2015-05-19T00:00:00",
  "desenvolvedoraId": 1,
  "categoriaIds": [1, 2],
  "plataformaIds": [1, 2]
}
```
> `categoriaIds`/`plataformaIds` são opcionais.

**PUT — Body:**
```json
{
  "nome": "The Witcher 3",
  "preco": 59.99,
  "dataLancamento": "2015-05-19T00:00:00",
  "desenvolvedoraId": 1
}
```

---

### Health — `/api/health2` e `/health`

| Método | Rota | Descrição | Retorno |
|--------|------|-----------|---------|
| GET | `/health/live` | Liveness (formato padrão) | 200 / 503 |
| GET | `/health/db` | Readiness — banco Oracle (formato padrão) | 200 / 503 |
| GET | `/api/health2/live` | Liveness (JSON detalhado) | 200 / 503 |
| GET | `/api/health2/db` | Readiness (JSON detalhado) | 200 / 503 |

---

## Evidências de Testes

Prints de todos os endpoints testados manualmente estão na pasta `prints/`, organizados por recurso:

```
prints/
├── Categoria/       (5 endpoints)
├── Desenvolvedora/  (5 endpoints)
├── Plataforma/      (5 endpoints)
├── Jogo/            (13 endpoints)
├── Health/          (2 endpoints)
├── Tests/           (3 evidências)
└── Telemetria/      (1 evidência — Application Insights)
```

| Controller | Endpoints | Evidência |
| :--- | :---: | :--- |
| **Categoria** | 5 endpoints | [Visualizar Prints](prints/Categoria/) |
| **Desenvolvedora** | 5 endpoints | [Visualizar Prints](prints/Desenvolvedora/) |
| **Plataforma** | 5 endpoints | [Visualizar Prints](prints/Plataforma/) |
| **Jogo** | 13 endpoints | [Visualizar Prints](prints/Jogo/) |
| **Health** | 2 endpoints | [Visualizar Prints](prints/Health/) |

> **Total:** 30 endpoints testados e documentados.

Além dos prints por endpoint, também há evidências de execução da suíte de testes e de telemetria:

| Evidência | Itens | Descrição | Link |
| :--- | :---: | :--- | :--- |
| **Tests** | 3 prints | Execução por trait (`Controller`, `Repository`, `UseCase`) | [Visualizar Prints](prints/Tests/) |
| **Telemetria** | 1 print | Application Insights (OpenTelemetry) | [Visualizar Prints](prints/Telemetria/) |

---

## Observações

- Só `Jogo` expõe suas relações (`Desenvolvedora`, `Categorias`, `Plataformas`).
- Checagem de duplicata por `Nome` é case-insensitive na aplicação (Categoria, Desenvolvedora, Plataforma e Jogo), além do índice único no banco — aplicada tanto no `POST` (nome já existente) quanto no `PUT` (renomear pra um nome que já existe em outro registro), ambos retornando `409` com mensagem clara.
- Todo endpoint que retorna um `Jogo` (`GET`, `POST`, `PUT`, `DELETE` e vínculo/desvínculo de categoria/plataforma) traz `desenvolvedora`, `categorias` e `plataformas` preenchidos, não só a relação que o endpoint alterou.
- Todas as respostas (`GET`, `POST`, `PUT`, `DELETE`) usam DTOs de resposta dedicados (`JogoResponseDto`, `CategoriaResponseDto`, `DesenvolvedoraResponseDto`, `PlataformaResponseDto`) em vez da entidade EF crua.