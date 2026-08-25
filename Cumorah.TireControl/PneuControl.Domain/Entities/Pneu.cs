namespace PneuControl.Domain.Entities;

public class Pneu
{
    public Guid Id { get; set; }
    public string Marca { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public string Tamanho { get; set; } = string.Empty;
    public int ProfundidadeTread { get; set; }
    public DateTime DataInstalacao { get; set; }
    public DateTime? DataUltimaManutencao { get; set; }
    public PneuStatus Status { get; set; }
    public DateTime CriadoEm { get; set; }
    public DateTime? AtualizadoEm { get; set; }
}
