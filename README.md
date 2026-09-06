# MedVet - 2TDSPJ

## Integrantes do Grupo

| Nome | RM |
|---|---|
| João Henrique Batista | RM564361 |
| Gutemberg Rocha | RM562267 |
| Erik Miyasato | RM565771 |
| Juliana da Silva Stigliani | RM561171 |
| Gustavo Arthur Carvalho Sartori | RM561650 |

---

## Dominio Escolhido

Clinica de Medicina Veterinaria. O sistema gerencia o fluxo de atendimento da clinica, incluindo proprietarios de animais (donos), pets, veterinarios, medicamentos, prescricoes e consultas.

---

## SGBD e Configuracao de Credenciais

- **Desenvolvimento/Testes Locais (Padrao):** A solucao vem configurada por padrao em `appsettings.Development.json` com `"Database:UseSqlite": true` utilizando SQLite local (`medvet-dev.db`). O banco e as tabelas sao inicializados automaticamente via `EnsureCreated()`, permitindo executar e testar a aplicacao, o Swagger e os Health Checks de imediato, sem necessidade de banco externo ou credenciais.
- **Oracle Database:** As connection strings nos arquivos `appsettings.json` e `appsettings.Development.json` utilizam os placeholders `REPLACE_USER` e `REPLACE_PASSWORD` para proteger credenciais contra exposicao em versionamento de codigo. Caso queira conectar a uma instancia real do Oracle:
  1. No arquivo `appsettings.Development.json`, altere `"UseSqlite": false`.
  2. Substitua os valores `REPLACE_USER` e `REPLACE_PASSWORD` pelo seu usuario e senha reais do Oracle (ex.: `User ID=RMxxxxxx;Password=xxxxxx;`).

---

## Como Executar a API

### Pre-requisitos
- .NET 9 SDK instalado

### Executando a API
Acesse a pasta da solucao e execute o projeto API:

```bash
cd MedVet
dotnet run --project MedVet.Api/MedVet.Api.csproj
```

A API iniciara escutando por padrao em:
- HTTP: `http://localhost:5033`
- HTTPS: `https://localhost:7139`

### Swagger / OpenAPI
Com a aplicacao em modo de desenvolvimento, a interface do Swagger esta disponivel na raiz e no endpoint dedicado:
- `http://localhost:5033/`
- `http://localhost:5033/swagger`

Todos os endpoints estao documentados com metadados OpenAPI, codigos de resposta HTTP (`ProducesResponseType`) e comentarios XML refletidos diretamente na UI.

---

## Health Checks

A API implementa health checks centralizados no endpoint:
- **URL:** `GET /health`

### Checks Registrados
1. **`self`**: Verifica se o processo da API esta ativo e respondendo (`HealthCheckResult.Healthy`).
2. **`database`**: Verifica a conectividade com o banco de dados atraves do `AddDbContextCheck<MedVetContext>`.
3. **`fiap`**: Verifica a conectividade com servico externo (FIAP) via requisicao HTTP.

### Resposta JSON Customizada (RFC)
O retorno e estruturado no formato JSON padronizado via `HealthCheckResponseWriter`:
- **Status 200 OK:** Quando todos os checks essenciais estao operacionais (`Healthy`).
- **Status 503 Service Unavailable:** Quando qualquer dependencia critica falha (`Unhealthy`).

Exemplo de resposta:
```json
{
  "status": "Healthy",
  "duration": "00:00:00.7621556",
  "checks": [
    {
      "name": "self",
      "status": "Healthy",
      "description": "Servico da API ativo e operacional.",
      "duration": "00:00:00.0008198",
      "error": null
    },
    {
      "name": "database",
      "status": "Healthy",
      "description": "Conexao com o banco de dados estabelecida com sucesso.",
      "duration": "00:00:00.0191409",
      "error": null
    },
    {
      "name": "fiap",
      "status": "Healthy",
      "description": "Conectividade externa com portal FIAP verificada.",
      "duration": "00:00:00.7557943",
      "error": null
    }
  ]
}
```

---

## Observabilidade e Logs Estruturados

A aplicacao utiliza `ILogger<T>` com logs estruturados e propriedades nomeadas correlacionadas por `traceId` (`HttpContext.TraceIdentifier`):
- Fluxos de escrita (como cadastro de Dono e Pet) registram inicio e conclusao com parametros semanticos (`NomeDono`, `PetId`, `TraceId`).
- Falhas e excecoes sao capturadas no `GlobalExceptionHandler` e registradas em nivel `Error` contendo o mesmo identificador de correlacao `TraceId`.

---

## Repositorio Generico

Para padronizar o acesso a dados e desacoplar a camada de aplicacao da infraestrutura:
- **Contrato:** `IRepository<T>` em `MedVet.Application/Interfaces/Repositories/IRepository.cs`, restrito a entidades que derivam de `BaseEntity`.
  - Operacoes: `GetAll()`, `GetById(id)`, `Add(entity)`, `Delete(id)`, `ExistsById(id)`.
- **Implementacao EF Core:** `Repository<T>` em `MedVet.Infrastructre/Repositories/Repository.cs`.
- **Registro na DI:** `services.AddScoped(typeof(IRepository<>), typeof(Repository<>));`.
- **Uso no Dominio:** O servico `MedicamentoService` consome diretamente `IRepository<Medicamento>`, demonstrando a utilizacao consistente do repositorio generico no fluxo da aplicacao.

---

## Tratamento Global de Erros (GlobalExceptionHandler)

Implementado com `IExceptionHandler` e registrado via `AddExceptionHandler<GlobalExceptionHandler>()` e `AddProblemDetails()`, interceptando todas as excecoes e formatando as respostas no padrao **RFC 7807** (`application/problem+json`).

### Tabela de Mapeamento de Excecoes

| Excecao | Status HTTP | Titulo ProblemDetails | Descricao |
|---|---|---|---|
| `ArgumentNullException` | 400 Bad Request | Requisicao invalida | Parametro obrigatorio ausente ou nulo |
| `ArgumentException` | 400 Bad Request | Requisicao invalida | Argumento invalido fornecido na requisicao |
| `DomainException` | 400 Bad Request | Erro de dominio | Violacao de invariante ou regra de negocio do dominio |
| `InvalidOperationException` | 400 Bad Request | Operacao invalida | Operacao invalida (ex: referencia inexistente) |
| `KeyNotFoundException` | 404 Not Found | Recurso nao encontrado | Identificador solicitado nao existe no sistema |
| `UnauthorizedAccessException` | 401 Unauthorized | Nao autorizado | Acesso nao autorizado ao recurso |
| Excecoes nao mapeadas | 500 Internal Server Error | Erro interno do servidor | Mensagem generica sem vazar stack trace em producao |

Em ambiente de desenvolvimento (`Development`), o `traceId` e adicionado automaticamente ao dicionario `Extensions` do `ProblemDetails` para facilitar o rastreamento.

---

## Testes Automatizados (xUnit + Moq)

A solucao conta com suites de testes automatizados organizados de acordo com a piramide de testes:

1. **`MedVet.Domain.Tests` (sem mock):**
   - Testa diretamente regras de negocio e invariantes das entidades de dominio (`Medicamento`, `Dono`, `Pet`).
   - Utiliza padrao AAA explcito.
   - Contem testes de caminho feliz (`[Fact]`) e validacao de condicoes de erro (`[Theory]` + `[InlineData]`) garantindo que `DomainException` e lancada em dados invalidos.
2. **`MedVet.Application.Tests` (com mock via Moq):**
   - Testa servicos da aplicacao (`PetService`, `MedicamentoService`).
   - Moca interfaces de repositorio (`IPetRepository`, `IDonoRepository`, `IRepository<Medicamento>`).
   - Valida cenarios de erro sem persistencia (`Times.Never`) e cenarios de sucesso persistindo uma vez (`Times.Once`).

### Como Rodar os Testes

Na pasta da solucao:

```bash
dotnet test MedVet/MedVet.sln
```

Todos os testes devem executar e passar com 100% de aproveitamento.

---

## Documentos (`/docs`)

- `docs/med-vet-models.pdf`: Modelagem do banco de dados MedVet (CP1/CP2).
- `docs/evidencias health unhealthy.pdf`: Evidencias de teste dos endpoints de Health Check (cenarios Healthy 200 OK e Unhealthy 503 Service Unavailable).

