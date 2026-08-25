namespace PneuControl.Domain.Entities;

public class RegistroManutencao
{
    public Guid Id { get; set; }
    public Guid PneuId { get; set; }
    public Pneu? Pneu { get; set; }
    public DateTime DataManutencao { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public string TipoManutencao { get; set; } = string.Empty; // Rotação, Balanceamento, etc
    public decimal Custo { get; set; }
    public DateTime CriadoEm { get; set; }
}