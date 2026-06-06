namespace CrudProductsApi.Dtos;

public class CriarProdutoRequest
{
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public int QuantidadeEmEstoque { get; set; }
}
