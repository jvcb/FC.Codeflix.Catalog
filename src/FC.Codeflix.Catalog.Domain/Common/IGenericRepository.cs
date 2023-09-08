namespace FC.Codeflix.Catalog.Domain.Common;

public interface IGenericRepository<TAggregate> : IRepository
{
    public Task Insert(TAggregate category, CancellationToken cancellationToken);
}
