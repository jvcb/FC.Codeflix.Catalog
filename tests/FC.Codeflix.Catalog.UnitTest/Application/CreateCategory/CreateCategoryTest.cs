using FC.Codeflix.Catalog.Domain.Entities;
using Moq;

namespace FC.Codeflix.Catalog.UnitTest.Application.CreateCategory;

public class CreateCategoryTest
{
    [Fact(DisplayName = nameof(CreateCategory))]
    [Trait("Application", "CreateCategory - UseCase")]
    public async void CreateCategory()
    {
        var repositoryMock = new Mock<ICategoryRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var input = new CreateCategoryInput(
            "Category Name",
            "Category Description",
            true);

        var useCase = new CreateCategoryUseCase(
            repositoryMock.Object,
            unitOfWorkMock.Object);

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(repository => 
            repository.Create(It.IsAny<Category>, CancellationToken.None), 
            Times.Once);

        unitOfWorkMock.Verify(
            uow => uow.Commit(It.IsAny<CancellationToken>()), 
            Times.Once);

        output.Should().NotBeNull();
        output.Id.Should().NotBe(Guid.Empty);
        output.Name.Should().Be("Category Name");
        output.Description.Should().Be("Category Name");
        output.IsActive.Should().BeTrue();
        output.CreatedAt.Should().NotBeSameDateAs(DateTime.MinValue);
    }
}