using System.Text.Json.Nodes;
using LibraryManager.Dto;
using LibraryManager.Model;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace LibraryManager.OpenApi;

public class BookExamplesSchemaTransformer : IOpenApiSchemaTransformer
{
    public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken)
    {
        if (context.JsonTypeInfo.Type == typeof(CreateBookDto))
        {
            schema.Example = new JsonObject
            {
                ["title"] = "Clean Code",
                ["author"] = "Robert C. Martin",
                ["genre"] = "Software Engineering",
                ["price"] = 89.9,
                ["stock"] = 10
            };
        }
        else if (context.JsonTypeInfo.Type == typeof(UpdateBookDto))
        {
            schema.Example = new JsonObject
            {
                ["title"] = "Clean Code",
                ["author"] = "Robert C. Martin",
                ["genre"] = "Software Engineering",
                ["price"] = 99.9,
                ["stock"] = 8
            };
        }
        else if (context.JsonTypeInfo.Type == typeof(Book))
        {
            schema.Example = new JsonObject
            {
                ["id"] = Guid.NewGuid().ToString(),
                ["title"] = "Clean Code",
                ["author"] = "Robert C. Martin",
                ["genre"] = "Software Engineering",
                ["price"] = 89.9,
                ["stock"] = 10,
                ["createdAt"] = DateTime.UtcNow,
                ["updatedAt"] = DateTime.UtcNow
            };
        }

        return Task.CompletedTask;
    }
}

public sealed class BookOperationTransformer : IOpenApiOperationTransformer
{
    public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
    {
        context.Description.ActionDescriptor.RouteValues.TryGetValue("action", out var actionName);

        switch (actionName)
        {
            case "Create":
                operation.Summary = "Cria um novo livro";
                operation.Description = "Retorna 400 se o título+autor já existirem, se o preço/estoque for negativo ou se o gênero for inválido.";
                break;

            case "Update":
                operation.Summary = "Atualiza um livro existente";
                operation.Description = "Atualiza os dados do livro e a data UpdatedAt. Retorna 404 se o livro não existir.";
                break;

            case "GetAll":
                operation.Summary = "Lista todos os livros";
                operation.Description = "Retorna a lista completa de livros cadastrados.";
                break;

            case "GetById":
                operation.Summary = "Obtém um livro pelo Id";
                operation.Description = "Retorna um livro específico ou 404 se não existir.";
                break;

            case "Delete":
                operation.Summary = "Remove um livro pelo Id";
                operation.Description = "Deleta o livro informado. Retorna 404 se não existir.";
                break;
        }

        return Task.CompletedTask;
    }
}

public sealed class LibraryDocumentTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        document.Info = new OpenApiInfo
        {
            Title = "LibraryManager API",
            Version = "v1",
            Description = "API de CRUD para gerenciamento de livros de uma biblioteca.",
        };

        return Task.CompletedTask;
    }
}