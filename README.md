# DeskFlow API — Gestão de Chamados e Helpdesk de TI

## Sobre o Projeto
A **DeskFlow API** é uma Web API RESTful construída em .NET 10 com Entity Framework Core e SQL Server.
Ela centraliza o gerenciamento de chamados de suporte técnico: categorias, abertura de chamados,
ciclo de vida do atendimento, histórico de interações e filtros de consulta.

## Tecnologias Utilizadas
- .NET 10 / ASP.NET Core Web API
- Entity Framework Core 10 (Migrations)
- SQL Server
- Swagger / OpenAPI (Swashbuckle)

## Como Executar a Aplicação

### Pré-requisitos
- .NET SDK 10
- SQL Server em execução (LocalDB, SQL Server Express ou Docker)
- Ferramenta do EF Core: `dotnet tool install --global dotnet-ef`

### Passo a passo
1. Clone o repositório:
   ```bash
   git clone https://github.com/NicolasdeSilva/deskflow-api.git
   ```
2. Acesse a pasta do projeto:
   ```bash
   cd deskflow-api/src/DeskFlow.API
   ```
3. Configure a connection string em `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost;Database=DeskFlowDb;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```
4. Crie o banco de dados (**opção A – migrations**):
   ```bash
   dotnet ef database update
   ```
   ou (**opção B – script SQL**) execute o arquivo [`database/script.sql`](database/script.sql) no SQL Server.
5. Execute a API:
   ```bash
   dotnet run
   ```
6. Acesse o Swagger: https://localhost:7000/swagger

##  Endpoints

| Método | Rota | Descrição | Sucesso |
|--------|------|-----------|---------|
| GET | `/api/categorias` | Lista categorias | 200 |
| GET | `/api/categorias/{id}` | Busca categoria | 200 / 404 |
| POST | `/api/categorias` | Cadastra categoria | 201 |
| PUT | `/api/categorias/{id}` | Atualiza categoria | 200 |
| DELETE | `/api/categorias/{id}` | Remove categoria (se sem chamados) | 204 / 400 |
| GET | `/api/chamados?status=&prioridade=&categoriaId=` | Lista com filtros combinados | 200 |
| GET | `/api/chamados/{id}` | Detalhes + Categoria + Interações | 200 / 404 |
| POST | `/api/chamados` | Abre chamado (status Aberto) | 201 |
| PATCH | `/api/chamados/{id}/iniciar` | Aberto → EmAndamento | 200 / 400 |
| PATCH | `/api/chamados/{id}/encerrar` | EmAndamento → Fechado (exige solução) | 200 / 400 |
| POST | `/api/chamados/{id}/interacoes` | Adiciona comentário (chamado não Fechado) | 201 / 400 |

### Exemplo: abrir chamado
```json
POST /api/chamados
{
  "titulo": "Notebook não liga",
  "descricao": "O notebook não responde ao apertar o botão de energia.",
  "prioridade": "Alta",
  "solicitanteNome": "Maria Silva",
  "categoriaId": 1
}
```

### Exemplo: encerrar chamado
```json
PATCH /api/chamados/1/encerrar
{ "solucao": "Fonte de alimentação substituída." }
```

### Exemplo de erro padronizado
```json
{ "statusCode": 400, "titulo": "Requisição inválida", "mensagem": "Não é possível adicionar interações a um chamado Fechado." }
```

## 🧠 Ciclo de Vida do Chamado
- **Aberto**: chamado registrado pelo solicitante.
- **EmAndamento**: suporte em atendimento.
- **Fechado**: encerrado com texto de solução e data de fechamento.

Transições permitidas: `Aberto → EmAndamento → Fechado`.

## Arquitetura em Camadas
```
Controller -> Service -> Repository -> AppDbContext (SQL Server)
```
- **Controllers**: recebem as requisições HTTP e definem os Status Codes.
- **Services**: regras de negócio e validação dos status.
- **Repositories**: acesso a dados via EF Core.
- **Models**: entidades, enums e DTOs.
- **Middlewares**: `ExceptionHandlingMiddleware` padroniza os erros em JSON, sem stack trace.
- **Data**: `AppDbContext` e Migrations.

## Vídeo de Apresentação
[Clique aqui para assistir ao vídeo](Adicionarei futuramente!)
