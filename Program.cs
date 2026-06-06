using CrudProductsApi.Dtos;
using CrudProductsApi.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var produtos = new List<Produto>
{
    new()
    {
        Id = 1,
        Nome = "Mouse sem fio",
        Preco = 89.90m
    },
    new()
    {
        Id = 2,
        Nome = "Teclado mecânico",
        Preco = 249.90m
    },
    new()
    {
        Id = 3,
        Nome = "Monitor 24 polegadas",
        Preco = 899.00m
    }
};

app.MapGet("/", () => Results.Ok(new
{
    mensagem = "CrudProductsApi está funcionando!",
    proximoPasso = "Testar as rotas CRUD em /produtos"
}));

app.MapGet("/health", () => Results.Ok(new
{
    status = "OK",
    horarioUtc = DateTimeOffset.UtcNow
}));

app.MapGet("/produtos", () => Results.Ok(produtos));

app.MapGet("/produtos/{id:int}", (int id) =>
{
    var produto = produtos.FirstOrDefault(produto => produto.Id == id);

    return produto is null
        ? Results.NotFound(new { mensagem = $"Produto com id {id} não encontrado." })
        : Results.Ok(produto);
});

app.MapPost("/produtos", (CriarProdutoRequest request) =>
{
    var erroValidacao = ValidarProdutoRequest(request);

    if (erroValidacao is not null)
    {
        return Results.BadRequest(new { mensagem = erroValidacao });
    }

    var novoId = produtos.Count == 0
        ? 1
        : produtos.Max(produto => produto.Id) + 1;

    var produto = new Produto
    {
        Id = novoId,
        Nome = request.Nome.Trim(),
        Preco = request.Preco
    };

    produtos.Add(produto);

    return Results.Created($"/produtos/{produto.Id}", produto);
});

app.MapPut("/produtos/{id:int}", (int id, CriarProdutoRequest request) =>
{
    var erroValidacao = ValidarProdutoRequest(request);

    if (erroValidacao is not null)
    {
        return Results.BadRequest(new { mensagem = erroValidacao });
    }

    var produto = produtos.FirstOrDefault(produto => produto.Id == id);

    if (produto is null)
    {
        return Results.NotFound(new { mensagem = $"Produto com id {id} não encontrado." });
    }

    produto.Nome = request.Nome.Trim();
    produto.Preco = request.Preco;

    return Results.Ok(produto);
});

app.MapDelete("/produtos/{id:int}", (int id) =>
{
    var produto = produtos.FirstOrDefault(produto => produto.Id == id);

    if (produto is null)
    {
        return Results.NotFound(new { mensagem = $"Produto com id {id} não encontrado." });
    }

    produtos.Remove(produto);

    return Results.NoContent();
});

app.Run();

static string? ValidarProdutoRequest(CriarProdutoRequest request)
{
    if (string.IsNullOrWhiteSpace(request.Nome))
    {
        return "O nome do produto é obrigatório.";
    }

    if (request.Nome.Trim().Length > 100)
    {
        return "O nome do produto deve ter no máximo 100 caracteres.";
    }

    if (request.Preco <= 0)
    {
        return "O preço do produto deve ser maior que zero.";
    }

    return null;
}
