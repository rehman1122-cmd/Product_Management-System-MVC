using System.Globalization;
using Lab9.Models;
using Lab9.Repositories;
using Microsoft.Data.Sqlite;

namespace Lab9.Data;

public sealed class ProductRepository : IProductRepository
{
    private const string SelectColumns = "Id, Name, Brand, Price, Stock, Description";
    private readonly SqliteConnectionFactory _connectionFactory;

    public ProductRepository(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<Product>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        await using var connection = await _connectionFactory.OpenConnectionAsync(cancellationToken);
        await using var command = connection.CreateCommand();
        command.CommandText = $"SELECT {SelectColumns} FROM Products ORDER BY Brand, Name";

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        var products = new List<Product>();
        while (await reader.ReadAsync(cancellationToken))
        {
            products.Add(ReadProduct(reader));
        }

        return products;
    }

    public async Task<Product?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        await using var connection = await _connectionFactory.OpenConnectionAsync(cancellationToken);
        await using var command = connection.CreateCommand();
        command.CommandText = $"SELECT {SelectColumns} FROM Products WHERE Id = $id";
        command.Parameters.AddWithValue("$id", id);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        return await reader.ReadAsync(cancellationToken) ? ReadProduct(reader) : null;
    }

    public async Task<int> CreateAsync(
        Product product,
        CancellationToken cancellationToken = default)
    {
        await using var connection = await _connectionFactory.OpenConnectionAsync(cancellationToken);
        await using var command = connection.CreateCommand();
        command.CommandText = """
            INSERT INTO Products (Name, Brand, Price, Stock, Description)
            VALUES ($name, $brand, $price, $stock, $description);
            SELECT last_insert_rowid();
            """;
        AddProductParameters(command, product);

        var result = await command.ExecuteScalarAsync(cancellationToken);
        return Convert.ToInt32(result, CultureInfo.InvariantCulture);
    }

    public async Task<bool> UpdateAsync(
        Product product,
        CancellationToken cancellationToken = default)
    {
        await using var connection = await _connectionFactory.OpenConnectionAsync(cancellationToken);
        await using var command = connection.CreateCommand();
        command.CommandText = """
            UPDATE Products
            SET Name = $name, Brand = $brand, Price = $price, Stock = $stock,
                Description = $description
            WHERE Id = $id
            """;
        AddProductParameters(command, product);
        command.Parameters.AddWithValue("$id", product.Id);

        return await command.ExecuteNonQueryAsync(cancellationToken) > 0;
    }

    public async Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        await using var connection = await _connectionFactory.OpenConnectionAsync(cancellationToken);
        await using var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM Products WHERE Id = $id";
        command.Parameters.AddWithValue("$id", id);

        return await command.ExecuteNonQueryAsync(cancellationToken) > 0;
    }

    private static void AddProductParameters(SqliteCommand command, Product product)
    {
        command.Parameters.AddWithValue("$name", product.Name);
        command.Parameters.AddWithValue("$brand", product.Brand);
        command.Parameters.AddWithValue("$price", product.Price);
        command.Parameters.AddWithValue("$stock", product.Stock);
        command.Parameters.AddWithValue("$description", product.Description);
    }

    private static Product ReadProduct(SqliteDataReader reader)
    {
        return new Product
        {
            Id = reader.GetInt32(0),
            Name = reader.GetString(1),
            Brand = reader.GetString(2),
            Price = Convert.ToDecimal(reader.GetValue(3), CultureInfo.InvariantCulture),
            Stock = reader.GetInt32(4),
            Description = reader.GetString(5)
        };
    }
}
