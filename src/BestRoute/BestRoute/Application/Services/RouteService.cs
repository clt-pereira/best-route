﻿using BestRoute.Domain.Entities;
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

    //public async Task<(List<string> Rota, decimal Custo)> EncontrarRotaMaisBarata(string origem, string destino)
    //{
    //    // Passo 1: Normalizar as entradas (origem e destino) para minúsculas
    //    origem = origem.ToUpper();
    //    destino = destino.ToUpper();

    //    // Passo 1: Obter todas as rotas disponíveis do banco de dados
    //    var rotas = await _context.Rotas.ToListAsync();

    //    // Passo 2: Construir um dicionário de adjacências onde cada cidade de origem tem uma lista de destinos possíveis
    //    var rotasPorOrigem = rotas
    //        .GroupBy(rota => rota.Origem.ToUpper()) // Agrupar rotas pela rota de origem
    //        .ToDictionary(grupo => grupo.Key, grupo => grupo.ToList()); // Mapear cada origem para uma lista de rotas

    //    // Passo 3: Garantir que todas os destinos estejam incluídas no dicionário de adjacências
    //    foreach (var rota in rotas)
    //    {
    //        if (!rotasPorOrigem.ContainsKey(rota.Destino.ToUpper()))
    //        {
    //            rotasPorOrigem[rota.Destino.ToUpper()] = new List<Route>(); // Adiciona destinos sem rotas de origem
    //        }
    //    }

    //    // Passo 4: Inicializar dicionários para armazenar os custos (g) e os predecessores dos destinos
    //    var custoTotalPorDestino = new Dictionary<string, decimal>(); // Guarda o custo acumulado para cada cidade
    //    var destinoAnterior = new Dictionary<string, string>(); // Guarda o destino anterior no caminho para reconstrução da rota

    //    // Passo 5: Inicializar todos os custos como infinito, exceto para a cidade de origem que tem custo 0
    //    foreach (var rotaOrigem in rotasPorOrigem.Keys)
    //    {
    //        custoTotalPorDestino[rotaOrigem] = decimal.MaxValue; // Custo inicial é infinito
    //        destinoAnterior[rotaOrigem] = null; // Sem destino anterior no início
    //    }

    //    custoTotalPorDestino[origem] = 0; // O custo do destino cidade de origem é 0

    //    // Passo 6: Inicializar as listas de destinos a serem visitadas e destinos já visitadas
    //    var destinosParaVisitar = new List<string> { origem }; // Destinos ainda a serem visitadas
    //    var destinosVisitados = new HashSet<string>(); // Destinos já visitadas

    //    // Passo 7: Algoritmo A* - enquanto houver destinos a serem visitadas
    //    while (destinosParaVisitar.Any())
    //    {
    //        // Passo 7.1: Seleciona o destino com o menor custo acumulado (g) da lista de destinos a serem visitadas
    //        var destinoAtual = destinosParaVisitar.OrderBy(c => custoTotalPorDestino[c]).First();
    //        destinosParaVisitar.Remove(destinoAtual); // Remove da lista de destinos para visitar
    //        destinosVisitados.Add(destinoAtual); // Marca como visitada

    //        // Passo 7.2: Se chegamos no destino, reconstrua a rota e retorne o custo total
    //        if (destinoAtual == destino)
    //        {
    //            var caminho = new List<string>(); // Lista para armazenar o caminho da origem ao destino
    //            var cidade = destino;

    //            // Reconstrução do caminho a partir dos predecessores
    //            while (cidade != null)
    //            {
    //                caminho.Insert(0, cidade); // Adiciona a cidade ao início da lista de caminho
    //                cidade = destinoAnterior[cidade]; // Vai para a cidade anterior no caminho
    //            }

    //            return (caminho, custoTotalPorDestino[destino]); // Retorna o caminho e o custo total
    //        }

    //        // Passo 7.3: Explora os vizinhos da cidade atual
    //        if (rotasPorOrigem.ContainsKey(destinoAtual))
    //        {
    //            foreach (var rota in rotasPorOrigem[destinoAtual])
    //            {
    //                // Passo 7.4: Ignora rotas para os destinos já visitados
    //                if (destinosVisitados.Contains(rota.Destino.ToUpper())) continue;

    //                // Passo 7.5: Calcula o custo tentativo para a cidade de destino via cidadeAtual
    //                var custoTentativo = custoTotalPorDestino[destinoAtual] + rota.Custo;

    //                // Passo 7.6: Se o custo tentativo for menor que o custo atual para o destino, atualiza
    //                if (custoTentativo < custoTotalPorDestino[rota.Destino.ToUpper()])
    //                {
    //                    custoTotalPorDestino[rota.Destino.ToUpper()] = custoTentativo; // Atualiza o custo
    //                    destinoAnterior[rota.Destino.ToUpper()] = destinoAtual; // Atualiza o predecessor

    //                    // Passo 7.7: Adiciona o destino à lista destinos a visitar, se não estiver na lista
    //                    if (!destinosParaVisitar.Contains(rota.Destino.ToUpper()))
    //                    {
    //                        destinosParaVisitar.Add(rota.Destino.ToUpper());
    //                    }
    //                }
    //            }
    //        }
    //    }

    //    // Se não encontrar caminho para o destino
    //    return (new List<string>(), 0);
    //}

    //public async Task<(List<string> Rota, decimal Custo)> EncontrarRotaMenorCusto(string origem, string destino)
    //{
    //    origem = origem.ToUpper();
    //    destino = destino.ToUpper();

    //    // Obter rotas disponíveis
    //    var rotas = await _context.Rotas.ToListAsync();

    //    // Construir grafo de adjacência com os custos
    //    var grafo = rotas
    //        .GroupBy(r => r.Origem.ToUpper())
    //        .ToDictionary(g => g.Key, g => g.Select(r => (r.Destino.ToUpper(), r.Custo)).ToList());

    //    // Inicializar estruturas
    //    var custos = new Dictionary<string, decimal>(); // Armazena o menor custo conhecido até cada nó
    //    var predecessores = new Dictionary<string, string>(); // Rastreia o caminho
    //    var visitados = new HashSet<string>(); // Conjunto de nós já visitados

    //    // Obter todas as cidades (origens e destinos)
    //    var todasAsCidades = rotas
    //        .SelectMany(r => new[] { r.Origem.ToUpper(), r.Destino.ToUpper() })
    //        .Distinct();

    //    // Inicializar os custos para todas as cidades
    //    foreach (var cidade in todasAsCidades)
    //    {
    //        custos[cidade] = decimal.MaxValue; // Inicializar como infinito
    //    }
    //    custos[origem] = 0; // O custo da origem é 0

    //    // Lista de nós para explorar
    //    var aExplorar = new List<string> { origem };

    //    while (aExplorar.Any())
    //    {
    //        // Selecionar o nó com o menor custo acumulado
    //        var atual = aExplorar.OrderBy(n => custos[n]).First();
    //        aExplorar.Remove(atual);

    //        // Se chegamos ao destino, reconstruir a rota
    //        if (atual == destino)
    //        {
    //            var rota = new List<string>();
    //            var cidade = destino;

    //            while (cidade != null)
    //            {
    //                rota.Insert(0, cidade);
    //                cidade = predecessores.ContainsKey(cidade) ? predecessores[cidade] : null;
    //            }

    //            return (rota, custos[destino]);
    //        }

    //        // Marcar como visitado
    //        visitados.Add(atual);

    //        // Explorar vizinhos do nó atual
    //        if (grafo.ContainsKey(atual))
    //        {
    //            foreach (var (vizinho, custoRota) in grafo[atual])
    //            {
    //                if (visitados.Contains(vizinho)) continue;

    //                // Calcular o custo para alcançar o vizinho
    //                var novoCusto = custos[atual] + custoRota;

    //                // Atualizar o custo e o predecessor, se o novo custo for menor
    //                if (novoCusto < custos[vizinho])
    //                {
    //                    custos[vizinho] = novoCusto;
    //                    predecessores[vizinho] = atual;

    //                    // Adicionar o vizinho à lista de nós para explorar
    //                    if (!aExplorar.Contains(vizinho))
    //                    {
    //                        aExplorar.Add(vizinho);
    //                    }
    //                }
    //            }
    //        }
    //    }

    //    // Caso não haja rota até o destino
    //    return (new List<string>(), 0);
    //}

    public async Task<(List<string> Rota, decimal Custo)> EncontrarRotaMenorCusto(string origem, string destino)
    {
        origem = origem.ToUpper();
        destino = destino.ToUpper();

        // Obter rotas disponíveis
        var rotas = await _context.Rotas.ToListAsync();

        // Construir grafo de adjacência com os custos
        var grafo = rotas
            .GroupBy(r => r.Origem.ToUpper())
            .ToDictionary(g => g.Key, g => g.Select(r => (r.Destino.ToUpper(), r.Custo)).ToList());

        // Inicializar estruturas
        var rota = new List<string> { origem }; // Caminho percorrido
        var custoTotal = 0m; // Custo acumulado
        var atual = origem;

        // Enquanto não alcançarmos o destino
        while (atual != destino)
        {
            // Verificar se há vizinhos para o nó atual
            if (!grafo.ContainsKey(atual) || !grafo[atual].Any())
            {
                // Caso não haja rota possível
                return (new List<string>(), 0);
            }

            // Selecionar o vizinho com o menor custo
            var (proximo, custo) = grafo[atual].OrderBy(r => r.Custo).First();

            // Atualizar a rota e o custo total
            rota.Add(proximo);
            custoTotal += custo;

            // Avançar para o próximo nó
            atual = proximo;

            // Evitar ciclos infinitos (grafo mal formado)
            if (rota.Count(n => n == atual) > 1)
            {
                return (new List<string>(), 0); // Ciclo detectado
            }
        }

        // Retornar a rota encontrada e o custo total
        return (rota, custoTotal);
    }
}