using API_Fruteira.MODELS;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace API_Fruteira.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AugustoController : ControllerBase
    {
        private readonly IDbConnection _connection;

        public AugustoController(IDbConnection connection)
        {
            _connection = connection;
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] Augusto produto)
        {
            var sql = "INSERT INTO produtos (nome, preco, quantidade) VALUES (@Nome, @Preco, @Quantidade)";
            await _connection.ExecuteAsync(sql, produto);
            return Ok("Produto cadastrado com sucesso!");
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var sql = "SELECT * FROM produtos";
            var produtos = await _connection.QueryAsync<Augusto>(sql);
            return Ok(produtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var sql = "SELECT * FROM produtos WHERE id = @Id";
            var produto = await _connection.QueryFirstOrDefaultAsync<Augusto>(sql, new { Id = id });

            if (produto == null)
                return NotFound("Produto não encontrado.");

            return Ok(produto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] Augusto produto)
        {
            produto.Id = id;
            var sql = "UPDATE produtos SET nome = @Nome, preco = @Preco, quantidade = @Quantidade WHERE id = @Id";
            var linhasAfetadas = await _connection.ExecuteAsync(sql, produto);

            if (linhasAfetadas == 0)
                return NotFound("Produto não encontrado para atualização.");

            return Ok("Produto atualizado com sucesso!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir(int id)
        {
            var sql = "DELETE FROM produtos WHERE id = @Id";
            var linhasAfetadas = await _connection.ExecuteAsync(sql, new { Id = id });

            if (linhasAfetadas == 0)
                return NotFound("Produto não encontrado para exclusão.");

            return Ok("Produto excluído com sucesso!");
        }
    }
}