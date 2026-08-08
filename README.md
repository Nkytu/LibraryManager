# LibraryManager

CRUD simples de livros construído com **ASP.NET Core 10** + **Entity Framework Core (SQL Server)** e documentação **Swagger/OpenAPI** automática com exemplos.

## Requisitos

- [.NET SDK 10](https://dotnet.microsoft.com/download/dotnet/10.0) (ou superior)
- SQL Server (o projeto usa **LocalDB**, que já vem com o Visual Studio)
- PowerShell, cmd ou terminal qualquer

## Como rodar

```bash
# na raiz do projeto
dotnet restore
dotnet run --project LibraryManager_Api
```

A API sobe em `http://localhost:5007`.

Em ambiente de desenvolvimento o banco `LibraryManagerDb` e a tabela `Books` são criados automaticamente (`EnsureCreated`), portanto não é necessário configurar nada no SQL.

> Alternativa manual: executar o script `LibraryManager_Api/Scripts/create.sql` no SQL Server para criar o banco e a tabela.

## Como visualizar (Swagger)

Com a aplicação rodando, abra no navegador:

```
http://localhost:5007/swagger
```

A interface do Swagger lista os 5 endpoints e já traz **exemplos preenchidos** nos corpos de requisição (`CreateBookDto`/`UpdateBookDto`) e nos retornos (`Book`).

O documento OpenAPI bruto fica em:

```
http://localhost:5007/openapi/v1.json
```

## Endpoints

| Método | Rota             | Descrição                                 | Retornos        |
|--------|------------------|-------------------------------------------|-----------------|
| GET    | `/api/book`      | Lista todos os livros                     | 200             |
| GET    | `/api/book/{id}` | Obtém um livro pelo Id                    | 200, 404        |
| POST   | `/api/book`      | Cria um novo livro                        | 201, 400        |
| PUT    | `/api/book/{id}` | Atualiza um livro existente               | 200, 400, 404   |
| DELETE | `/api/book/{id}` | Remove um livro                           | 204, 404        |

Exemplo de corpo no `POST /api/book`:

```json
{
  "title": "Clean Code",
  "author": "Robert C. Martin",
  "genre": "Software Engineering",
  "price": 89.9,
  "stock": 10
}
```

## Regras de negócio

- `title` + `author` não podem se repetir
- `price` e `stock` não podem ser negativos
- `genre` deve ser um dos gêneros válidos (ex.: `Romance`, `Terror`, `Tecnologia`, `Software Engineering`)
- Ao criar, `createdAt` e `updatedAt` são preenchidos automaticamente; ao editar, apenas `updatedAt` é atualizado

Violações retornam `400 Bad Request` com a mensagem do motivo.

## Estrutura do projeto

```
LibraryManager_Api/
├── Biz/                 # Regras de negócio (BookRulesBiz)
├── Controllers/         # BookController (CRUD)
├── Data/                # DbContext
├── Dto/                 # CreateBookDto, UpdateBookDto
├── Exceptions/          # BusinessRuleException
├── Models/              # Entidade Book
├── OpenApi/             # Transformers de documentação (título, resumos, exemplos)
└── Scripts/             # create.sql
```

## Teste rápido com .http

O arquivo `LibraryManager_Api/LibraryManager.http` possui exemplos prontos de todas as chamadas (basta usar a extensão *REST Client* no VS Code).
