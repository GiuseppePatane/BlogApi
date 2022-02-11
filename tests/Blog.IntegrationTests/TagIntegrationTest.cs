using System.Threading.Tasks;
using Blog.Domain.Entities;
using Blog.Infrastructure.Db.EF;
using Blog.Infrastructure.Db.EF.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Blog.IntegrationTests;

public class TagIntegrationTest :IntegrationTestBase
{
    
    [Fact]
    public async Task Create_New_Tag_Should_StoreIt_In_The_Db()
    {
        // SETUP
        await using var context = new BlogDbContext(GenDbContextOptions());
        await  PrepareDatabase(context).ConfigureAwait(false);
        //ATTEMPT 
        var repository = new EfRepository(context);
        await repository.AddAsync(Tag.Create("test", "pippo"));
        await context.SaveChangesAsync();
        var tag = await repository.GetByIdAsync<Tag>("test");
        //VERIFY 
        tag.Should().NotBeNull();
        tag.Name.Should().Be("pippo");
        await CheckDatabaseAndRemoveIt(context);
    }
    [Fact]
    public async Task Modify_Existing_Tag_Should_NotThrowsAnException()
    {
        // SETUP
        await using var context = new BlogDbContext(GenDbContextOptions());
        await  PrepareDatabase(context).ConfigureAwait(false);
        context.Tags.Add( Tag.Create("test", "pippo"));
        await context.SaveChangesAsync();
        //ATTEMPT 
        var repository = new EfRepository(context);
        var tag = await repository.GetByIdAsync<Tag>("test");
        tag.Update("pluto");
        await repository.UpdateAsync<Tag>(tag);
        tag = await repository.GetByIdAsync<Tag>("test");
        //VERIFY 
        tag.Should().NotBeNull();
        tag.Name.Should().Be("pluto");
        await CheckDatabaseAndRemoveIt(context);
    }
    [Fact]
    public async Task Modify_NotExisting_Tag_Should_ThrowsAnException()
    {
        // SETUP
        await using var context = new BlogDbContext(GenDbContextOptions());
        await  PrepareDatabase(context).ConfigureAwait(false);
        var repository = new EfRepository(context);
        //VERIFY 
        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => repository.UpdateAsync<Tag>(Tag.Create("test", "pippo")));
        await CheckDatabaseAndRemoveIt(context);
    }
    [Fact]
    public async Task Delete_Existing_Tag_Should_NotThrowsAnException()
    {
        // SETUP
        await using var context = new BlogDbContext(GenDbContextOptions());
        await  PrepareDatabase(context).ConfigureAwait(false);
        context.Tags.Add( Tag.Create("test", "pippo"));
        await context.SaveChangesAsync();
        //ATTEMPT 
        var repository = new EfRepository(context);
        var tag = await repository.GetByIdAsync<Tag>("test");
        await repository.DeleteAsync<Tag>(tag);
        tag = await repository.GetByIdAsync<Tag>("test");
        //VERIFY 
        tag.Should().BeNull();
        await CheckDatabaseAndRemoveIt(context);
    }
}