using BestRoute.Domain.Entities;
using BestRoute.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace BestRoute.Application.Services;

public class RouteService
{
    private readonly SQLiteDbContext _context;
    private Dictionary<string, List<Route>> _rotas;


    public RouteService(SQLiteDbContext context)
    {
        _context = context;
        _rotas = new Dictionary<string, List<Route>>();
    }

    public async Task AdicionarRotaAsync(string origem, string destino, decimal custo)
    {
        var rota = new Route { Origem = origem, Destino = destino, Custo = custo };
        _context.Rotas.Add(rota);
        await _context.SaveChangesAsync();
    }

    public async Task<(List<string> Rota, decimal Custo)> EncontrarRotaMaisBarata(string origem, string destino)
    {
        // Passo 1: Normalizar as entradas (origem e destino) para minúsculas
        origem = origem.ToUpper();
        destino = destino.ToUpper();

        // Passo 1: Obter todas as rotas disponíveis do banco de dados
        var rotas = await _context.Rotas.ToListAsync();

        // Passo 2: Construir um dicionário de adjacências onde cada cidade de origem tem uma lista de destinos possíveis
        var rotasPorOrigem = rotas
            .GroupBy(rota => rota.Origem.ToUpper()) // Agrupar rotas pela rota de origem
            .ToDictionary(grupo => grupo.Key, grupo => grupo.ToList()); // Mapear cada origem para uma lista de rotas

        // Passo 3: Garantir que todas os destinos estejam incluídas no dicionário de adjacências
        foreach (var rota in rotas)
        {
            if (!rotasPorOrigem.ContainsKey(rota.Destino.ToUpper()))
            {
                rotasPorOrigem[rota.Destino.ToUpper()] = new List<Route>(); // Adiciona destinos sem rotas de origem
            }
        }

        // Passo 4: Inicializar dicionários para armazenar os custos (g) e os predecessores dos destinos
        var custoTotalPorDestino = new Dictionary<string, decimal>(); // Guarda o custo acumulado para cada cidade
        var destinoAnterior = new Dictionary<string, string>(); // Guarda o destino anterior no caminho para reconstrução da rota

        // Passo 5: Inicializar todos os custos como infinito, exceto para a cidade de origem que tem custo 0
        foreach (var rotaOrigem in rotasPorOrigem.Keys)
        {
            custoTotalPorDestino[rotaOrigem] = decimal.MaxValue; // Custo inicial é infinito
            destinoAnterior[rotaOrigem] = null; // Sem destino anterior no início
        }

        custoTotalPorDestino[origem] = 0; // O custo do destino cidade de origem é 0

        // Passo 6: Inicializar as listas de destinos a serem visitadas e destinos já visitadas
        var destinosParaVisitar = new List<string> { origem }; // Destinos ainda a serem visitadas
        var destinosVisitados = new HashSet<string>(); // Destinos já visitadas

        // Passo 7: Algoritmo A* - enquanto houver destinos a serem visitadas
        while (destinosParaVisitar.Any())
        {
            // Passo 7.1: Seleciona o destino com o menor custo acumulado (g) da lista de destinos a serem visitadas
            var destinoAtual = destinosParaVisitar.OrderBy(c => custoTotalPorDestino[c]).First();
            destinosParaVisitar.Remove(destinoAtual); // Remove da lista de destinos para visitar
            destinosVisitados.Add(destinoAtual); // Marca como visitada

            // Passo 7.2: Se chegamos no destino, reconstrua a rota e retorne o custo total
            if (destinoAtual == destino)
            {
                var caminho = new List<string>(); // Lista para armazenar o caminho da origem ao destino
                var cidade = destino;

                // Reconstrução do caminho a partir dos predecessores
                while (cidade != null)
                {
                    caminho.Insert(0, cidade); // Adiciona a cidade ao início da lista de caminho
                    cidade = destinoAnterior[cidade]; // Vai para a cidade anterior no caminho
                }

                return (caminho, custoTotalPorDestino[destino]); // Retorna o caminho e o custo total
            }

            // Passo 7.3: Explora os vizinhos da cidade atual
            if (rotasPorOrigem.ContainsKey(destinoAtual))
            {
                foreach (var rota in rotasPorOrigem[destinoAtual])
                {
                    // Passo 7.4: Ignora rotas para os destinos já visitados
                    if (destinosVisitados.Contains(rota.Destino.ToUpper())) continue;

                    // Passo 7.5: Calcula o custo tentativo para a cidade de destino via cidadeAtual
                    var custoTentativo = custoTotalPorDestino[destinoAtual] + rota.Custo;

                    // Passo 7.6: Se o custo tentativo for menor que o custo atual para o destino, atualiza
                    if (custoTentativo < custoTotalPorDestino[rota.Destino.ToUpper()])
                    {
                        custoTotalPorDestino[rota.Destino.ToUpper()] = custoTentativo; // Atualiza o custo
                        destinoAnterior[rota.Destino.ToUpper()] = destinoAtual; // Atualiza o predecessor

                        // Passo 7.7: Adiciona o destino à lista destinos a visitar, se não estiver na lista
                        if (!destinosParaVisitar.Contains(rota.Destino.ToUpper()))
                        {
                            destinosParaVisitar.Add(rota.Destino.ToUpper());
                        }
                    }
                }
            }
        }

        // Se não encontrar caminho para o destino
        return (new List<string>(), 0);
    }

}