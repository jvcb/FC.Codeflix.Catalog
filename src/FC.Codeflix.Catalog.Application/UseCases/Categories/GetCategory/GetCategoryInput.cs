using FC.Codeflix.Catalog.Application.UseCases.Categories.Common;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Categories.GetCategory;
public class GetCategoryInput : IRequest<CategoryModelOutput>
{
    public Guid Id { get; set; }

    public GetCategoryInput(Guid id)
    {
        Id = id;
    }
}
