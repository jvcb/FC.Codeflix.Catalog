using FC.Codeflix.Catalog.Domain.Common;
using FC.Codeflix.Catalog.Domain.Entities;

namespace FC.Codeflix.Catalog.Domain.Repositories;

public interface ICategoryRepository : IGenericRepository<Category, Guid>
{

}
