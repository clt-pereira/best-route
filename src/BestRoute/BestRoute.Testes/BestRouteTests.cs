using BestRoute.Application.Services;
using BestRoute.Domain.Entities;
using BestRoute.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace BestRoute.Testes;

public class BestRouteTests
{
    private readonly DbContextOptions<SQLiteDbContext> _dbContextOptions;
    private SQLiteDbContext _context; // Contexto compartilhado entre os testes

    public BestRouteTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<SQLiteDbContext>()
            .UseSqlite("Data Source=:memory:") // Usando SQLite em memória
            .Options;

        _context = new SQLiteDbContext(_dbContextOptions);
        _context.Database.OpenConnection();
        _context.Database.EnsureCreated();
    }

    [Fact]
    public async Task TestarEncontrarRotaMaisBarata_DeveRetornarRotaCorretaParaGRU_CDG()
    {
        // Arrange
        var rotas = new List<Route>
        {
            new() { Origem = "GRU", Destino = "BRC", Custo = 10 },
            new() { Origem = "BRC", Destino = "SCL", Custo = 5 },
            new() { Origem = "GRU", Destino = "CDG", Custo = 75 },
            new() { Origem = "GRU", Destino = "SCL", Custo = 20 },
            new() { Origem = "GRU", Destino = "ORL", Custo = 56 },
            new() { Origem = "ORL", Destino = "CDG", Custo = 5 },
            new() { Origem = "SCL", Destino = "ORL", Custo = 20 },
        };

        _context.Rotas.AddRange(rotas);
        _context.SaveChanges();

        var routeService = new RouteService(_context);

        // Action
        var (rota, custo) = await routeService.EncontrarRotaMenorCusto("GRU", "CDG");

        // Assert
        var rotaEsperada = new List<string> { "GRU", "BRC", "SCL", "ORL", "CDG" };
        var custoEsperado = 40m;

        Assert.Equal(rotaEsperada, rota);
        Assert.Equal(custoEsperado, custo);
    }

    [Fact]
    public async Task TestarEncontrarRotaMaisBarata_DeveRetornarRotaCorretaParaBRC_SCL()
    {
        // Arrange
        var rotas = new List<Route>
        {
            new() { Origem = "GRU", Destino = "BRC", Custo = 10 },
            new() { Origem = "BRC", Destino = "SCL", Custo = 5 },
            new() { Origem = "GRU", Destino = "CDG", Custo = 75 },
            new() { Origem = "GRU", Destino = "SCL", Custo = 20 },
            new() { Origem = "GRU", Destino = "ORL", Custo = 56 },
            new() { Origem = "ORL", Destino = "CDG", Custo = 5 },
            new() { Origem = "SCL", Destino = "ORL", Custo = 20 },
        };

        _context.Rotas.AddRange(rotas);
        _context.SaveChanges();

        var routeService = new RouteService(_context);

        // Action
        var (rota, custo) = await routeService.EncontrarRotaMenorCusto("BRC", "SCL");

        // Assert
        var rotaEsperada = new List<string> { "BRC", "SCL" };
        var custoEsperado = 5m;

        Assert.Equal(rotaEsperada, rota);
        Assert.Equal(custoEsperado, custo);
    }

    [Fact]
    public async Task TestarEncontrarRotaMaisBarata_DeveRetornarRotaVaziaParaDestinoInexistente()
    {
        // Arrange
        var rotas = new List<Route>
        {
            new() { Origem = "BRC", Destino = "SCL", Custo = 5 },
            new() { Origem = "ORL", Destino = "CDG", Custo = 5 },
            new() { Origem = "SCL", Destino = "ORL", Custo = 20 },
        };

        _context.Rotas.AddRange(rotas);
        _context.SaveChanges();

        var routeService = new RouteService(_context);

        // Action
        var (rota, custo) = await routeService.EncontrarRotaMenorCusto("GRU", "CDG");

        // Assert
        Assert.Empty(rota);
        Assert.Equal(0m, custo);
    }
}
