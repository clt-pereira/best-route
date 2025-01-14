namespace BestRoute.Domain.Entities;

public class Route
{
    public int Id { get; set; }
    public string Origem { get; set; } = string.Empty;
    public string Destino { get; set; } = string.Empty;
    public decimal Custo { get; set; }
}