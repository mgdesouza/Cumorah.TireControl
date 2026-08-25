namespace PneuControl.Domain.Interfaces;

using PneuControl.Domain.Entities;

public interface IPneuRepository : IRepository<Pneu>
{
    Task<IEnumerable<Pneu>> GetByVeiculoIdAsync(Guid veiculoId);
}