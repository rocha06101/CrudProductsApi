
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
        Preco = 89.90m,
        QuantidadeEmEstoque = 12
    },
    new()
    {
        Id = 2,
        Nome = "Teclado mecânico",
        Preco = 249.90m,
        QuantidadeEmEstoque = 5
    },
    new()
    {
        Id = 3,
        Nome = "Monitor 24 polegadas",
        Preco = 899.00m,
        QuantidadeEmEstoque = 3
    }
};

app.MapGet("/", () => Results.Ok(new
{
    mensagem = "CrudProductsApi está funcionando!",
    proximoPasso = "Testar as rotas GET /produtos e GET /produtos/{id}"
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
    if (string.IsNullOrWhiteSpace(request.Nome))
    {
        return Results.BadRequest(new { mensagem = "O nome do produto é obrigatório." });
    }

    if (request.Preco <= 0)
    {
        return Results.BadRequest(new { mensagem = "O preço do produto deve ser maior que zero." });
    }

    if (request.QuantidadeEmEstoque < 0)
    {
        return Results.BadRequest(new { mensagem = "A quantidade em estoque não pode ser negativa." });
    }

    var novoId = produtos.Count == 0
        ? 1
        : produtos.Max(produto => produto.Id) + 1;

    var produto = new Produto
    {
        Id = novoId,
        Nome = request.Nome.Trim(),
        Preco = request.Preco,
        QuantidadeEmEstoque = request.QuantidadeEmEstoque
    };

    produtos.Add(produto);

    return Results.Created($"/produtos/{produto.Id}", produto);
});

app.Run();
