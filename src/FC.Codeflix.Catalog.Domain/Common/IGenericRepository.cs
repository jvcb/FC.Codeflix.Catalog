namespace FC.Codeflix.Catalog.Domain.Common;

public interface IGenericRepository<TAggregate, TPrimaryKey> : IRepository
{ 
    public Task Insert(TAggregate aggregate, CancellationToken cancellationToken);
    public Task<TAggregate> Get(TPrimaryKey id, CancellationToken cancellationToken);
}
