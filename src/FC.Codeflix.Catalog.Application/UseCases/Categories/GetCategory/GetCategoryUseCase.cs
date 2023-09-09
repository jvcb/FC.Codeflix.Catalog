using FC.Codeflix.Catalog.Application.UseCases.Categories.Common;
using FC.Codeflix.Catalog.Domain.Repositories;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Categories.GetCategory;
public class GetCategoryUseCase : IGetCategoryUseCase
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoryUseCase(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryModelOutput> Handle(
        GetCategoryInput input, 
        CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.Get(input.Id, cancellationToken);

        return CategoryModelOutput.FromCategory(category);
    }
}
