# Evidências do Checkpoint 5 (CP5) - MedVet

Este documento consolida as evidências de execução e validação dos requisitos do CP5: Versionamento de API, Paginação e Rate Limiting.

---

## 1. Versionamento (Recurso: `/api/medicamento`)

### A. Requisição na v1 (Legada - Deprecada) via Query String
```http
GET /api/medicamento?api-version=1.0 HTTP/1.1
Host: localhost:5033
```
**Resposta:**
```http
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
Server: Kestrel
api-deprecated-versions: 1.0

[
  {
    "id": "ae6885bd-7138-4722-ac93-80cedf925028",
    "nomeMedicamento": "Amoxicilina 500mg",
    "marca": "VetPharma",
    "modoDeUso": "1 comprimido ao dia",
    "preco": 45.5
  },
  {
    "id": "c22f1a1a-beaf-47de-916c-6954ccb3c8d0",
    "nomeMedicamento": "Dipirona Gotas",
    "marca": "VetCare",
    "modoDeUso": "10 gotas a cada 12h",
    "preco": 18
  },
  {
    "id": "fd62a820-d90a-47ea-bc6c-edfbcf9d8495",
    "nomeMedicamento": "Prednisolona 20mg",
    "marca": "BioVet",
    "modoDeUso": "Meio comprimido pela manha",
    "preco": 32.9
  }
]
```

### B. Requisição na v1 via Header HTTP `X-Api-Version`
```http
GET /api/medicamento HTTP/1.1
Host: localhost:5033
X-Api-Version: 1.0
```
**Resposta:**
```http
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
Server: Kestrel
api-deprecated-versions: 1.0

[
  {
    "id": "ae6885bd-7138-4722-ac93-80cedf925028",
    "nomeMedicamento": "Amoxicilina 500mg",
    "marca": "VetPharma",
    "modoDeUso": "1 comprimido ao dia",
    "preco": 45.5
  },
  ...
]
```

### C. Requisição na v2 (Atual) sem especificação de versão (Omissão -> Default 2.0)
```http
GET /api/medicamento HTTP/1.1
Host: localhost:5033
```
**Resposta:**
```http
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
Server: Kestrel
api-supported-versions: 2.0

{
  "items": [
    {
      "id": "ae6885bd-7138-4722-ac93-80cedf925028",
      "nomeMedicamento": "Amoxicilina 500mg",
      "marca": "VetPharma",
      "modoDeUso": "1 comprimido ao dia",
      "preco": 45.5
    },
    {
      "id": "c22f1a1a-beaf-47de-916c-6954ccb3c8d0",
      "nomeMedicamento": "Dipirona Gotas",
      "marca": "VetCare",
      "modoDeUso": "10 gotas a cada 12h",
      "preco": 18
    },
    {
      "id": "fd62a820-d90a-47ea-bc6c-edfbcf9d8495",
      "nomeMedicamento": "Prednisolona 20mg",
      "marca": "BioVet",
      "modoDeUso": "Meio comprimido pela manha",
      "preco": 32.9
    }
  ],
  "page": 1,
  "pageSize": 20,
  "totalItems": 3,
  "totalPages": 1,
  "hasPrevious": false,
  "hasNext": false
}
```

---

## 2. Paginação na Versão 2 (`/api/medicamento`)

### A. Página 1 com `pageSize=2`
```http
GET /api/medicamento?page=1&pageSize=2 HTTP/1.1
Host: localhost:5033
```
**Resposta:**
```json
{
  "items": [
    {
      "id": "ae6885bd-7138-4722-ac93-80cedf925028",
      "nomeMedicamento": "Amoxicilina 500mg",
      "marca": "VetPharma",
      "modoDeUso": "1 comprimido ao dia",
      "preco": 45.5
    },
    {
      "id": "c22f1a1a-beaf-47de-916c-6954ccb3c8d0",
      "nomeMedicamento": "Dipirona Gotas",
      "marca": "VetCare",
      "modoDeUso": "10 gotas a cada 12h",
      "preco": 18
    }
  ],
  "page": 1,
  "pageSize": 2,
  "totalItems": 3,
  "totalPages": 2,
  "hasPrevious": false,
  "hasNext": true
}
```

### B. Página 2 com `pageSize=2` (Itens distintos, sem sobreposição)
```http
GET /api/medicamento?page=2&pageSize=2 HTTP/1.1
Host: localhost:5033
```
**Resposta:**
```json
{
  "items": [
    {
      "id": "fd62a820-d90a-47ea-bc6c-edfbcf9d8495",
      "nomeMedicamento": "Prednisolona 20mg",
      "marca": "BioVet",
      "modoDeUso": "Meio comprimido pela manha",
      "preco": 32.9
    }
  ],
  "page": 2,
  "pageSize": 2,
  "totalItems": 3,
  "totalPages": 2,
  "hasPrevious": true,
  "hasNext": false
}
```

### C. Página além do total (`page=3` com `totalItems=3` e `pageSize=2`) -> 200 OK com items vazio
```http
GET /api/medicamento?page=3&pageSize=2 HTTP/1.1
Host: localhost:5033
```
**Resposta:**
```json
{
  "items": [],
  "page": 3,
  "pageSize": 2,
  "totalItems": 3,
  "totalPages": 2,
  "hasPrevious": true,
  "hasNext": false
}
```

### D. Validação de Erro de Página: `page=0` -> 400 Bad Request
```http
GET /api/medicamento?page=0&pageSize=20 HTTP/1.1
Host: localhost:5033
```
**Resposta:**
```http
HTTP/1.1 400 Bad Request
Content-Type: application/json; charset=utf-8

{
  "title": "Parametros de paginacao invalidos",
  "status": 400,
  "detail": "page deve ser >= 1."
}
```

### E. Validação de Erro de Página: `pageSize=9999` -> 400 Bad Request
```http
GET /api/medicamento?page=1&pageSize=9999 HTTP/1.1
Host: localhost:5033
```
**Resposta:**
```http
HTTP/1.1 400 Bad Request
Content-Type: application/json; charset=utf-8

{
  "title": "Parametros de paginacao invalidos",
  "status": 400,
  "detail": "pageSize deve estar entre 1 e 100."
}
```

---

## 3. Rate Limiting (Fixed Window)

- **Endpoint limitado:** `POST /api/medicamento` (v2)
- **Política:** 10 requisições por janela de 1 minuto (`fixed`).
- **Comportamento no estouro:** Status HTTP **429 Too Many Requests**, cabeçalho **`Retry-After: 60`** e corpo JSON padronizado.

### Resposta após exceder o limite (11ª requisição no mesmo minuto):
```http
HTTP/1.1 429 Too Many Requests
Content-Type: application/json; charset=utf-8
Server: Kestrel
Retry-After: 60

{
  "type": "https://tools.ietf.org/html/rfc6585#section-4",
  "title": "Too Many Requests",
  "status": 429,
  "detail": "Limite de requisicoes excedido. Tente novamente mais tarde."
}
```

### Validação do Health Check após o estouro do Rate Limit
O endpoint `GET /health` possui `.DisableRateLimiting()`, mantendo-se **200 OK Healthy** mesmo quando o rate limiter está rejeitando chamadas no `POST`:

```http
GET /health HTTP/1.1
Host: localhost:5033
```
**Resposta:**
```http
HTTP/1.1 200 OK
Content-Type: application/json
Server: Kestrel

{
  "status": "Healthy",
  "duration": "00:00:00.6538449",
  "checks": [
    {
      "name": "self",
      "status": "Healthy",
      "description": "Servico da API ativo e operacional.",
      "duration": "00:00:00.0008712",
      "error": null
    },
    {
      "name": "database",
      "status": "Healthy",
      "description": "Conexao com o banco de dados estabelecida com sucesso.",
      "duration": "00:00:00.0054365",
      "error": null
    },
    {
      "name": "fiap",
      "status": "Healthy",
      "description": "Conectividade externa com portal FIAP verificada.",
      "duration": "00:00:00.6451754",
      "error": null
    }
  ]
}
```

---

## 4. Swagger com Documentação por Versão

A API expõe dois documentos Swagger independentes acessíveis no topo da UI:
- **v1.0 (Deprecada):** `/swagger/v1.0/swagger.json`
  - Descrição: `"Esta versao esta deprecada. Utilize a versao mais recente (v2.0) com suporte a paginacao."`
- **v2.0 (Atual):** `/swagger/v2.0/swagger.json`
  - Inclui endpoint paginado e DTO de envelope `MedicamentoResponsePagedResponse`.
- Controllers não versionados explicitamente (`Consulta`, `Dono`, `Pet`, `Veterinario`) foram marcados com `[ApiVersionNeutral]` e aparecem em ambas as versões.

---

## 5. Testes Automatizados (`dotnet test`)

Execução de todos os 35 testes unitários (Domain + Application):
```
Aprovado!  – Com falha: 0, Aprovado: 15, Ignorado: 0, Total: 15 - MedVet.Domain.Tests.dll (net9.0)
Aprovado!  – Com falha: 0, Aprovado: 20, Ignorado: 0, Total: 20 - MedVet.Application.Tests.dll (net9.0)
```
100% dos testes passando com sucesso.
