using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using API_Product.Models;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly string _connectionString = "Server=localhost\\SQLEXPRESS;Database=ProductDB;Integrated Security=True;TrustServerCertificate=True;";

    // GET: api/products
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        List<Product> products = new List<Product>();
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var command = new SqlCommand("GetAllProducts", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var product = new Product
                    {
                        ProductId = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Description = reader.GetString(2),
                        Price = reader.GetDecimal(3),
                        ReleaseDate = reader.GetDateTime(4)
                    };
                    products.Add(product);
                }
            }
        }
        return Ok(products);
    }

    // GET: api/products/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        Product product = null;
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var command = new SqlCommand("GetProductById", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int) { Value = id });

            using (var reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    product = new Product
                    {
                        ProductId = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Description = reader.GetString(2),
                        Price = reader.GetDecimal(3),
                        ReleaseDate = reader.GetDateTime(4)
                    };
                }
            }
        }

        if (product == null)
        {
            return NotFound(new { message = "Product not found" });
        }

        return Ok(product);
    }

    // POST: api/products
    [HttpPost]
    public async Task<ActionResult<Product>> AddProduct(Product product)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var command = new SqlCommand("AddProducts", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@Name", product.Name);
            command.Parameters.AddWithValue("@Description", product.Description);
            command.Parameters.AddWithValue("@Price", product.Price);
            command.Parameters.AddWithValue("@ReleaseDate", product.ReleaseDate);

            await command.ExecuteNonQueryAsync();
        }
        return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, product);
    }

    // PUT: api/products/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, Product product)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var command = new SqlCommand("UpdateProduct", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@ProductId", id);
            command.Parameters.AddWithValue("@Name", product.Name);
            command.Parameters.AddWithValue("@Description", product.Description);
            command.Parameters.AddWithValue("@Price", product.Price);
            command.Parameters.AddWithValue("@ReleaseDate", product.ReleaseDate);

            int rowsAffected = await command.ExecuteNonQueryAsync();
            if (rowsAffected == 0)
            {
                return NotFound();
            }
        }

        return NoContent();
    }

    // DELETE: api/products/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var command = new SqlCommand("DeleteProduct", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@ProductId", id);

            int rowsAffected = await command.ExecuteNonQueryAsync();
            if (rowsAffected == 0)
            {
                return NotFound();
            }
        }

        return NoContent();
    }
}
