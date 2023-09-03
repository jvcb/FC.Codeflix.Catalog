using FC.Codeflix.Catalog.Domain.Entities;
using FC.Codeflix.Catalog.Domain.Exceptions;

namespace FC.Codeflix.Catalog.UnitTest.Domain.Entities.Categories;

public class CategoryTest
{
    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Category - Entity")]
    public void Instantiate()
    {
        var validData = new
        {
            Name = "Category Name",
            Description = "Category Description",
        };

        var datetimeBefore = DateTime.Now;
        var category = new Category(validData.Name, validData.Description);
        var datetimeAfter = DateTime.Now;

        Assert.NotNull(category);
        Assert.NotEqual(Guid.Empty, category.Id);
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
        Assert.True(category.IsActive);
        Assert.NotEqual(DateTime.MinValue, category.CreatedAt);
        Assert.True(datetimeBefore < category.CreatedAt);
        Assert.True(datetimeAfter > category.CreatedAt);
    }

    [Theory(DisplayName = nameof(InstantiateWithIsActiveStatus))]
    [InlineData(true)]
    [InlineData(false)]
    [Trait("Domain", "Category - Entity")]
    public void InstantiateWithIsActiveStatus(bool isActive)
    {
        var validData = new
        {
            Name = "Category Name",
            Description = "Category Description",
        };

        var datetimeBefore = DateTime.Now;
        var category = new Category(validData.Name, validData.Description, isActive);
        var datetimeAfter = DateTime.Now;

        Assert.NotNull(category);
        Assert.NotEqual(Guid.Empty, category.Id);
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
        Assert.Equal(isActive, category.IsActive);
        Assert.NotEqual(DateTime.MinValue, category.CreatedAt);
        Assert.True(datetimeBefore < category.CreatedAt);
        Assert.True(datetimeAfter > category.CreatedAt);
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("    ")]
    [Trait("Domain", "Category - Entity")]
    public void InstantiateErrorWhenNameIsEmpty(string name)
    {
        Action action =
            () => new Category(name, "Category Description");

        var exception = Assert.Throws<EntityValidationException>(() => action());
        Assert.Equal("Name should not be empty or null", exception.Message);
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
    [Trait("Domain", "Category - Entity")]
    public void InstantiateErrorWhenDescriptionIsNull()
    {
        Action action =
            () => new Category("Category Name", null!);

        var exception = Assert.Throws<EntityValidationException>(() => action());
        Assert.Equal("Description should not be empty or null", exception.Message);
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLessThan3Characters))]
    [InlineData("1")]
    [InlineData("12")]
    [Trait("Domain", "Category - Entity")]
    public void InstantiateErrorWhenNameIsLessThan3Characters(string invalidName)
    {
        Action action =
            () => new Category(invalidName, "Category Description");

        var exception = Assert.Throws<EntityValidationException>(action);

        Assert.Equal("Name should be at leasts 3 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Entity")]
    public void InstantiateErrorWhenNameIsGreaterThan255Characters()
    {
        var invalidName = String.Join("", Enumerable.Range(0, 256).Select(_ => "a").ToArray());

        Action action =
            () => new Category(invalidName, "Category Description");

        var exception = Assert.Throws<EntityValidationException>(action);

        Assert.Equal("Name should be greater than 255 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsGreaterThan10000Characters))]
    [Trait("Domain", "Category - Entity")]
    public void InstantiateErrorWhenDescriptionIsGreaterThan10000Characters()
    {
        var invalidDescription = String.Join(
            String.Empty, 
            Enumerable.Range(0, 10_001).Select(_ => "a").ToArray());

        Action action =
            () => new Category("Category Name", invalidDescription);

        var exception = Assert.Throws<EntityValidationException>(action);

        Assert.Equal("Description should be greater than 10000 characters long", exception.Message);
    }
}
