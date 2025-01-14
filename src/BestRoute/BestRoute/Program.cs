using BestRoute.Application.Services;
using BestRoute.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

var _dbContextOptions = new DbContextOptionsBuilder<SQLiteDbContext>()
    .UseSqlite("Data Source=rotas.db")
    .Options;

using var context = new SQLiteDbContext(_dbContextOptions);
context.Database.EnsureCreated();
context.Seed();

var service = new RouteService(context);

while (true)
{
    Console.WriteLine("Escolha uma opção:");
    Console.WriteLine("1 - Adicionar Nova Rota");
    Console.WriteLine("2 - Consultar Melhor Rota");
    Console.WriteLine("0 - Sair");
    var opcao = Console.ReadLine();

    switch (opcao)
    {
        case "1":
            await AdicionarRotaAsync(service);
            break;
        case "2":
            await ConsultarMelhorRotaAsync(service);
            break;
        case "0":
            return;
        default:
            Console.WriteLine("Opção inválida.");
            break;
    }

    static async Task AdicionarRotaAsync(RouteService service)
    {
        Console.Write("Digite a origem: ");
        var origem = Console.ReadLine();

        Console.Write("Digite o destino: ");
        var destino = Console.ReadLine();

        Console.Write("Digite o custo: ");
        if (decimal.TryParse(Console.ReadLine(), out var custo))
        {
            await service.AdicionarRotaAsync(origem!, destino!, custo);
            Console.WriteLine("Rota adicionada com sucesso!");
        }
        else
        {
            Console.WriteLine("Custo inválido.");
        }
    }

    static async Task ConsultarMelhorRotaAsync(RouteService service)
    {
        Console.Write("Digite a origem: ");
        var origem = Console.ReadLine();

        Console.Write("Digite o destino: ");
        var destino = Console.ReadLine();

        var (rota, custo) = await service.EncontrarRotaMaisBarata(origem!, destino!);

        if (rota.Any())
        {
            Console.WriteLine($"Melhor rota de {origem} para {destino}: ** {string.Join(" - ", rota)} ao custo de ${custo} **");
        }
        else
        {
            Console.WriteLine("Não foi possível encontrar uma rota.");
        }
    }    
}
