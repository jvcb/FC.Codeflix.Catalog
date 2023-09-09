﻿using FC.Codeflix.Catalog.Application.UseCases.Categories.GetCategory;
using FluentAssertions;
using Moq;

namespace FC.Codeflix.Catalog.UnitTest.Application.GetCategory;

[Collection(nameof(GetCategoryTestFixture))]
public class GetCategoryTest
{
    private readonly GetCategoryTestFixture _fixture;

    public GetCategoryTest(GetCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(GetCategory))]
    [Trait("Application", "GetCategory - UseCase")]
    public async Task GetCategory()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var exampleCategory = _fixture.GetValidCategory();

        repositoryMock.Setup(repository
            => repository.Get(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(exampleCategory);

        var input = new GetCategoryInput(exampleCategory.Id);
        var useCase = new GetCategoryUseCase(repositoryMock.Object);

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(repository =>
            repository.Get(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()), 
            Times.Once);

        output.Should().NotBeNull();
        output.Id.Should().Be(exampleCategory.Id);
        output.Name.Should().Be(exampleCategory.Name);
        output.Description.Should().Be(exampleCategory.Description);
        output.IsActive.Should().Be(exampleCategory.IsActive);
        output.CreatedAt.Should().Be(exampleCategory.CreatedAt);
    }
}