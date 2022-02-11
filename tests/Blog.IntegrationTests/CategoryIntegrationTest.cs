using System.Threading.Tasks;
using Blog.Domain.Entities;
using Blog.Infrastructure.Db.EF;
using Blog.Infrastructure.Db.EF.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Blog.IntegrationTests;

public class CategoryIntegrationTest: IntegrationTestBase
{

    [Fact]
    public async Task Create_New_Category_Should_StoreIt_In_The_Db()
    {
        // SETUP
        await using var context = new BlogDbContext(GenDbContextOptions());
        await  PrepareDatabase(context).ConfigureAwait(false);
        //ATTEMPT 
        var repository = new EfRepository(context);
        await repository.AddAsync(Category.Create("test", "pippo"));
        await context.SaveChangesAsync();
        var category = await repository.GetByIdAsync<Category>("test");
        //VERIFY 
        category.Should().NotBeNull();
        category.Name.Should().Be("pippo");
        await CheckDatabaseAndRemoveIt(context);
    }
    [Fact]
    public async Task Modify_Existing_Category_Should_NotThrowsAnException()
    {
        // SETUP
        await using var context = new BlogDbContext(GenDbContextOptions());
        await  PrepareDatabase(context).ConfigureAwait(false);
        context.Categories.Add( Category.Create("test", "pippo"));
        await context.SaveChangesAsync();
        //ATTEMPT 
        var repository = new EfRepository(context);
        var category = await repository.GetByIdAsync<Category>("test");
        category.Update("pluto");
        await repository.UpdateAsync<Category>(category);
        category = await repository.GetByIdAsync<Category>("test");
        //VERIFY 
        category.Should().NotBeNull();
        category.Name.Should().Be("pluto");
        await CheckDatabaseAndRemoveIt(context);
    }
    [Fact]
    public async Task Modify_NotExisting_Category_Should_ThrowsAnException()
    {
        // SETUP
        await using var context = new BlogDbContext(GenDbContextOptions());
        await  PrepareDatabase(context).ConfigureAwait(false);
        var repository = new EfRepository(context);
        //VERIFY 
        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => repository.UpdateAsync<Category>(Category.Create("test", "pippo")));
        await CheckDatabaseAndRemoveIt(context);
    }
    [Fact]
    public async Task Delete_Existing_Category_Should_NotThrowsAnException()
    {
        // SETUP
        await using var context = new BlogDbContext(GenDbContextOptions());
        await  PrepareDatabase(context).ConfigureAwait(false);
        context.Categories.Add( Category.Create("test", "pippo"));
        await context.SaveChangesAsync();
        //ATTEMPT 
        var repository = new EfRepository(context);
        var category = await repository.GetByIdAsync<Category>("test");
        await repository.DeleteAsync<Category>(category);
        category = await repository.GetByIdAsync<Category>("test");
        //VERIFY 
        category.Should().BeNull();
        await CheckDatabaseAndRemoveIt(context);
    }
}


