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
- .NET 9 SDK (ou superior) instalado

### Executando a API
Acesse a pasta da solucao e execute o projeto API:

```bash
cd MedVet
dotnet run --project MedVet.Api/MedVet.Api.csproj
```

A API iniciara escutando por padrao em:
- HTTP: `http://localhost:5033`
- HTTPS: `https://localhost:7139`

### URLs Principais
- **Swagger UI:** `http://localhost:5033/` ou `http://localhost:5033/swagger` (com alternância entre as versões `v1.0` e `v2.0` no topo).
- **Health Checks:** `http://localhost:5033/health`
- **Listagem v1 (Legada - Deprecada):** `http://localhost:5033/api/medicamento?api-version=1.0`
- **Listagem v2 (Atual - Paginada):** `http://localhost:5033/api/medicamento`

---

## Versionamento da API (CP5)

O recurso **Medicamento** foi escolhido para demonstrar a convivência de dois contratos de API sem quebra de retrocompatibilidade:

- **Versão 1.0 (Deprecada):** Mantém o contrato legado do CP3, devolvendo o array direto de medicamentos (`IReadOnlyList<MedicamentoResponse>`). Está anotada com `[ApiVersion("1.0", Deprecated = true)]` e envia o header de resposta `api-deprecated-versions: 1.0`.
- **Versão 2.0 (Atual / Padrão):** Introduz a listagem paginada em formato de envelope com metadados (`PagedResponse<MedicamentoResponse>`). Anotada com `[ApiVersion("2.0")]` e envia o header `api-supported-versions: 2.0`.
- **Recursos Neutros:** Os demais controllers (`PetController`, `DonoController`, `VeterinarioController`, `ConsultaController`) foram marcados com `[ApiVersionNeutral]`, permanecendo acessíveis em ambas as versões.

### Como o Cliente Escolhe a Versão

A API combina múltiplos leitores (`ApiVersionReader.Combine`), suportando as três formas:

1. **Via Query String:**
   ```http
   GET /api/medicamento?api-version=1.0
   ```
2. **Via Header HTTP:**
   ```http
   GET /api/medicamento
   X-Api-Version: 1.0
   ```
3. **Por Omissão (Padrão):**
   ```http
   GET /api/medicamento
   ```
   Quando nenhuma versão é informada, o sistema assume automaticamente a versão **2.0** (`AssumeDefaultVersionWhenUnspecified = true`).

---

## Paginação na Versão 2 (CP5)

Na versão 2.0, o endpoint `GET /api/medicamento` deixa de retornar todos os registros e passa a paginar a consulta **diretamente no banco de dados**, utilizando `Count`, ordenação estável (`OrderBy(x => x.CreatedAt)`), `Skip` e `Take` sobre o `IQueryable` no repositório genérico.

### Parâmetros de Consulta (`PaginationQuery`)
| Parâmetro | Tipo | Padrão | Regra de Validação |
|---|---|---|---|
| `page` | Inteiro | `1` | Deve ser maior ou igual a 1 (`page >= 1`) |
| `pageSize` | Inteiro | `20` | Deve estar entre 1 e 100 (`1 <= pageSize <= 100`) |

- **Validação de Erro:** Se `page < 1` ou `pageSize` for menor que 1 ou maior que 100, a API responde **`400 Bad Request`** com `ProblemDetails` detalhando a falha.
- **Página Além do Total:** Retorna **`200 OK`** com `items: []` (não é erro 404).

### Envelope de Resposta (`PagedResponse<T>`)
```json
{
  "items": [
    {
      "id": "ae6885bd-7138-4722-ac93-80cedf925028",
      "nomeMedicamento": "Amoxicilina 500mg",
      "marca": "VetPharma",
      "modoDeUso": "1 comprimido ao dia",
      "preco": 45.5
    }
  ],
  "page": 1,
  "pageSize": 20,
  "totalItems": 1,
  "totalPages": 1,
  "hasPrevious": false,
  "hasNext": false
}
```

---

## Rate Limiting (CP5)

Para proteger a API contra sobrecargas e ataques de negação de serviço, foi configurado o middleware nativo `Microsoft.AspNetCore.RateLimiting`:

- **Algoritmo:** Janela Fixa (`FixedWindowRateLimiter`).
- **Endpoint Limitado:** `POST /api/medicamento` (decorado com `[EnableRateLimiting("fixed")]`).
- **Política ("fixed"):**
  - **PermitLimit:** 10 requisições permitidas.
  - **Window:** Janela de 1 minuto (`TimeSpan.FromMinutes(1)`).
- **Comportamento ao Estourar o Limite (11ª requisição):**
  - Status HTTP **429 Too Many Requests**.
  - Cabeçalho HTTP **`Retry-After: 60`** informando os segundos necessários para nova tentativa.
  - Corpo JSON RFC 6585 com detalhes do erro:
    ```json
    {
      "type": "https://tools.ietf.org/html/rfc6585#section-4",
      "title": "Too Many Requests",
      "status": 429,
      "detail": "Limite de requisicoes excedido. Tente novamente mais tarde."
    }
    ```
- **Isenção do Health Check:** O endpoint `GET /health` foi explicitamente desvinculado do rate limit (`.DisableRateLimiting()`), garantindo que probes de orquestradores e monitoramentos continuem recebendo status **200 OK** mesmo sob rajadas de tráfego.

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

---

## Observabilidade e Logs Estruturados

A aplicacao utiliza `ILogger<T>` com logs estruturados e propriedades nomeadas correlacionadas por `traceId` (`HttpContext.TraceIdentifier`):
- Fluxos de escrita registram inicio e conclusao com parametros semanticos (`NomeMedicamento`, `Id`, `TraceId`).
- Falhas e excecoes sao capturadas no `GlobalExceptionHandler` e registradas em nivel `Error` contendo o mesmo identificador de correlacao `TraceId`.

---

## Repositorio Generico

Para padronizar o acesso a dados e desacoplar a camada de aplicacao da infraestrutura:
- **Contrato:** `IRepository<T>` em `MedVet.Application/Interfaces/Repositories/IRepository.cs`, restrito a entidades que derivam de `BaseEntity`.
  - Operacoes: `GetAll()`, `GetPaged(pageNumber, pageSize)`, `GetById(id)`, `Add(entity)`, `Delete(id)`, `ExistsById(id)`.
- **Implementacao EF Core:** `Repository<T>` em `MedVet.Infrastructre/Repositories/Repository.cs`.
- **Registro na DI:** `services.AddScoped(typeof(IRepository<>), typeof(Repository<>));`.
- **Uso no Dominio:** O servico `MedicamentoService` consome diretamente `IRepository<Medicamento>`, utilizando tanto o CRUD tradicional quanto a consulta paginada `GetPaged`.

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

---

## Testes Automatizados (xUnit + Moq)

A solucao conta com 35 testes automatizados organizados de acordo com a piramide de testes:

1. **`MedVet.Domain.Tests` (sem mock):**
   - Testa diretamente regras de negocio e invariantes das entidades de dominio (`Medicamento`, `Dono`, `Pet`).
   - Contem testes de caminho feliz (`[Fact]`) e validacao de condicoes de erro (`[Theory]` + `[InlineData]`).
2. **`MedVet.Application.Tests` (com mock via Moq):**
   - Testa servicos da aplicacao (`PetService`, `MedicamentoService`).
   - Valida regras de paginacao (`PaginationQueryTests`) com `[Theory]` e `[Fact]`.
   - Moca interfaces de repositorio (`IRepository<Medicamento>`, `IPetRepository`, `IDonoRepository`), testando cenarios felizes (`Times.Once`) e de erro sem persistencia (`Times.Never`).

### Como Rodar os Testes

Na pasta raiz da solucao:

```bash
dotnet test MedVet/MedVet.sln
```

Todos os testes devem executar e passar com 100% de aproveitamento.

---

## Documentos e Evidencias (`/docs`)

- **Prints e Capturas do CP5 (`docs/cp5/`):**
  - `docs/cp5/v2-api.jpeg`: Captura da requisição `GET /api/medicamento` demonstrando o contrato da versão 2.0 em formato de envelope com metadados de paginação (`items`, `page`, `pageSize`, `totalItems`, `totalPages`, `hasPrevious`, `hasNext`).
  - `docs/cp5/deprecated-api-version.jpeg`: Captura da requisição na versão legada 1.0 (`GET /api/medicamento?api-version=1.0`) retornando o array plano e o cabeçalho `api-deprecated-versions: 1.0`.
  - `docs/cp5/erro400paginacao.jpeg`: Captura da validação de regras de paginação retornando status `400 Bad Request` com ProblemDetails para parâmetros inválidos (`page < 1` ou `pageSize > 100`).
  - `docs/cp5/429tomanyrequests.jpeg`: Captura do disparo do Rate Limiting no `POST /api/medicamento` ao estourar o limite de 10 requisições/minuto, respondendo `429 Too Many Requests` com cabeçalho `Retry-After: 60`.
- **Relatório de Execução do CP5:**
  - `docs/cp5-evidencias.md`: Documentação técnica consolidada contendo payloads JSON completos, cabeçalhos de resposta HTTP, testes de isolamento do Health Check e sumário de testes unitários.
- **Entregas Anteriores:**
  - `docs/med-vet-models.pdf`: Modelagem do banco de dados relacional MedVet (CP1/CP2).
  - `docs/evidencias health unhealthy.pdf`: Evidencias de teste dos endpoints de Health Check (CP4).
