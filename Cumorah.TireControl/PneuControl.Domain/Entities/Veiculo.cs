namespace PneuControl.Domain.Entities;

public class Veiculo
{
    public Guid Id { get; set; }
    public string Placa { get; set; } = string.Empty;
    public string Marca { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public int Ano { get; set; }
    public string NumeroSerie { get; set; } = string.Empty;
    public DateTime DataAquisicao { get; set; }
    public DateTime CriadoEm { get; set; }
    public DateTime? AtualizadoEm { get; set; }
    
    public ICollection<Pneu> Pneus { get; set; } = new List<Pneu>();
}