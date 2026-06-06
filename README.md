# CrudProductsApi

API REST de produtos em ASP.NET Core com armazenamento em memória.

## Endpoints

- `POST /produtos`: cria um produto.
- `GET /produtos`: lista todos os produtos.
- `GET /produtos/{id}`: busca um produto pelo identificador.
- `PUT /produtos/{id}`: atualiza um produto existente.
- `DELETE /produtos/{id}`: remove um produto existente.

## Modelo Produto

```json
{
  "id": 1,
  "nome": "Mouse sem fio",
  "preco": 89.90
}
```

## Validações

- `nome` é obrigatório.
- `nome` deve ter no máximo 100 caracteres.
- `preco` deve ser maior que zero.
