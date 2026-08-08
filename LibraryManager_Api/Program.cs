using LibraryManager.Data;
using LibraryManager.OpenApi;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer(new LibraryDocumentTransformer());
    options.AddOperationTransformer(new BookOperationTransformer());
    options.AddSchemaTransformer(new BookExamplesSchemaTransformer());
});

builder.Services.AddDbContext<LibraryManagerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LibraryManagerDb")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<LibraryManagerDbContext>();
    db.Database.EnsureCreated();
}

app.MapOpenApi();
app.MapSwaggerUI("swagger", options => options.SwaggerEndpoint("/openapi/v1.json", "LibraryManager API v1"));
app.MapControllers();

app.Run();