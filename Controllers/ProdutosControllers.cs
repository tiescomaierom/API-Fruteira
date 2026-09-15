using API_Fruteira.MODELS;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace API_Fruteira.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly IDbConnection _connection;

        public ProdutosController(IDbConnection connection)
        {
            _connection = connection;
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] Produto produto)
        {
            var sql = @"INSERT INTO Produtos (nome, descricao, preco, estoque, ativo) 
                        VALUES (@Nome, @Descricao, @Preco, @Estoque, @Ativo)";
            await _connection.ExecuteAsync(sql, produto);
            return Ok("Produto cadastrado com sucesso!");
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var sql = "SELECT * FROM Produtos";
            var produtos = await _connection.QueryAsync<Produto>(sql);
            return Ok(produtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var sql = "SELECT * FROM Produtos WHERE id = @Id";
            var produto = await _connection.QueryFirstOrDefaultAsync<Produto>(sql, new { Id = id });

            if (produto == null)
                return NotFound("Produto não encontrado.");

            return Ok(produto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] Produto produto)
        {
            produto.Id = id;
            var sql = @"UPDATE Produtos 
                        SET nome = @Nome, 
                            descricao = @Descricao, 
                            preco = @Preco, 
                            estoque = @Estoque, 
                            ativo = @Ativo 
                        WHERE id = @Id";
            var linhasAfetadas = await _connection.ExecuteAsync(sql, produto);

            if (linhasAfetadas == 0)
                return NotFound("Produto não encontrado para atualização.");

            return Ok("Produto atualizado com sucesso!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir(int id)
        {
            
            await _connection.ExecuteAsync("DELETE FROM Itens_Venda WHERE produto_id = @Id", new { Id = id });

     
            var sql = "DELETE FROM Produtos WHERE id = @Id";
            var linhasAfetadas = await _connection.ExecuteAsync(sql, new { Id = id });

            if (linhasAfetadas == 0)
                return NotFound("Produto não encontrado para exclusão.");

            return Ok("Produto excluído com sucesso!");
        }
    }
}