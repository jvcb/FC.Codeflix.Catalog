using FC.Codeflix.Catalog.Domain.Entities;
using FC.Codeflix.Catalog.Domain.Exceptions;
using FluentAssertions;

namespace FC.Codeflix.Catalog.UnitTest.Domain.Entities.Categories;

[Collection(nameof(CategoryTestFixture))]
public class CategoryTest
{
    private readonly CategoryTestFixture _categoryTestFixture;

    public CategoryTest(CategoryTestFixture categoryTestFixture)
        => _categoryTestFixture = categoryTestFixture;
   
    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Category - Entity")]
    public void Instantiate()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var datetimeBefore = DateTime.Now;
        var category = new Category(validCategory.Name, validCategory.Description);
        var datetimeAfter = DateTime.Now.AddSeconds(1);

        category.Should().NotBeNull();
        category.Id.Should().NotBe(Guid.Empty);
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.IsActive.Should().BeTrue();
        category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        (datetimeBefore <= category.CreatedAt).Should().BeTrue();
        (datetimeAfter >= category.CreatedAt).Should().BeTrue();
    }

    [Theory(DisplayName = nameof(InstantiateWithIsActiveStatus))]
    [InlineData(true)]
    [InlineData(false)]
    [Trait("Domain", "Category - Entity")]
    public void InstantiateWithIsActiveStatus(bool isActive)
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var datetimeBefore = DateTime.Now;
        var category = new Category(validCategory.Name, validCategory.Description, isActive);
        var datetimeAfter = DateTime.Now.AddSeconds(1);

        category.Should().NotBeNull();
        category.Id.Should().NotBeEmpty();
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.IsActive.Should().Be(isActive);
        category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        (datetimeBefore <= category.CreatedAt).Should().BeTrue();
        (datetimeAfter >= category.CreatedAt).Should().BeTrue();
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("    ")]
    [Trait("Domain", "Category - Entity")]
    public void InstantiateErrorWhenNameIsEmpty(string name)
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        Action action = () => new Category(name, validCategory.Description);

        action.Should().Throw<EntityValidationException>().WithMessage("Name should not be empty or null");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
    [Trait("Domain", "Category - Entity")]
    public void InstantiateErrorWhenDescriptionIsNull()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        Action action = () => new Category(validCategory.Name, null!);

        action.Should().Throw<EntityValidationException>().WithMessage("Description should not be empty or null");
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLessThan3Characters))]
    [InlineData("1")]
    [InlineData("12")]
    [InlineData("a")]
    [InlineData("ab")]
    [Trait("Domain", "Category - Entity")]
    public void InstantiateErrorWhenNameIsLessThan3Characters(string invalidName)
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        Action action = () => new Category(invalidName, validCategory.Description);

        action.Should().Throw<EntityValidationException>().WithMessage("Name should be at leasts 3 characters long");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Entity")]
    public void InstantiateErrorWhenNameIsGreaterThan255Characters()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var invalidName = String.Join("", Enumerable.Range(0, 256).Select(_ => "a").ToArray());

        Action action = () => new Category(invalidName, validCategory.Description);

        action.Should().Throw<EntityValidationException>().WithMessage("Name should be greater than 255 characters long");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsGreaterThan10000Characters))]
    [Trait("Domain", "Category - Entity")]
    public void InstantiateErrorWhenDescriptionIsGreaterThan10000Characters()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var invalidDescription = String.Join(
            String.Empty, 
            Enumerable.Range(0, 10_001).Select(_ => "a").ToArray());

        Action action = () => new Category(validCategory.Name, invalidDescription);

        action.Should().Throw<EntityValidationException>().WithMessage("Description should be greater than 10000 characters long");
    }

    [Fact(DisplayName = nameof(Activate))]
    [Trait("Domain", "Category - Entity")]
    public void Activate()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var category = new Category(validCategory.Name, validCategory.Description, false);

        category.Activate();

        category.IsActive.Should().BeTrue();
    }

    [Fact(DisplayName = nameof(Deactivate))]
    [Trait("Domain", "Category - Entity")]
    public void Deactivate()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var category = new Category(validCategory.Name, validCategory.Description);

        category.Deactivate();

        category.IsActive.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Domain", "Category - Entity")]
    public void Update()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var category = new Category(validCategory.Name, validCategory.Description);
        var newValues = new
        {
            Name = "New Name",
            Description = "New Description"
        };

        category.Update(newValues.Name, newValues.Description);

        category.Should().NotBeNull();
        category.Name.Should().Be(newValues.Name);
        category.Description.Should().Be(newValues.Description);
    }

    [Fact(DisplayName = nameof(UpdateOnlyName))]
    [Trait("Domain", "Category - Entity")]
    public void UpdateOnlyName()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var category = new Category(validCategory.Name, validCategory.Description);
        var newValues = new { Name = "New Name" };
        var currentDescription = category.Description;

        category.Update(newValues.Name);

        category.Should().NotBeNull();
        category.Name.Should().Be(newValues.Name);
        category.Description.Should().Be(currentDescription);
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("    ")]
    [Trait("Domain", "Category - Entity")]
    public void UpdateErrorWhenNameIsEmpty(string name)
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var category = new Category(validCategory.Name, validCategory.Description);

        Action action = () => category.Update(name);

        action.Should().Throw<EntityValidationException>().WithMessage("Name should not be empty or null");
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsLessThan3Characters))]
    [InlineData("1")]
    [InlineData("12")]
    [InlineData("a")]
    [InlineData("ab")]
    [Trait("Domain", "Category - Entity")]
    public void UpdateErrorWhenNameIsLessThan3Characters(string invalidName)
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var category = new Category(validCategory.Name, validCategory.Description);

        Action action = () => category.Update(invalidName, validCategory.Description);

        action.Should().Throw<EntityValidationException>().WithMessage("Name should be at leasts 3 characters long");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Entity")]
    public void UpdateErrorWhenNameIsGreaterThan255Characters()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var invalidName = String.Join("", Enumerable.Range(0, 256).Select(_ => "a").ToArray());

        var category = new Category(validCategory.Name, validCategory.Description);

        Action action = () => category.Update(invalidName, validCategory.Description);

        action.Should().Throw<EntityValidationException>().WithMessage("Name should be greater than 255 characters long");
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsGreaterThan10000Characters))]
    [Trait("Domain", "Category - Entity")]
    public void UpdateErrorWhenDescriptionIsGreaterThan10000Characters()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var category = new Category(validCategory.Name, validCategory.Description);

        var invalidDescription = String.Join(
            String.Empty,
            Enumerable.Range(0, 10_001).Select(_ => "a").ToArray());

        Action action = () => category.Update(validCategory.Name, invalidDescription);

        action.Should().Throw<EntityValidationException>().WithMessage("Description should be greater than 10000 characters long");
    }
}