using System.Threading.Tasks;
using Blog.Domain.Entities;
using Blog.Infrastructure.Db.EF;
using Blog.Infrastructure.Db.EF.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Blog.IntegrationTests;

public class AuthorIntegrationTest :IntegrationTestBase
{
    
    [Fact]
    public async Task Create_New_Author_Should_StoreIt_In_The_Db()
    {
        // SETUP
        await using var context = new BlogDbContext(GenDbContextOptions());
        await  PrepareDatabase(context).ConfigureAwait(false);
        //ATTEMPT 
        var repository = new EfRepository(context);
        await repository.AddAsync(Author.Create("test", "pippo"));
        await context.SaveChangesAsync();
        var author = await repository.GetByIdAsync<Author>("test");
        //VERIFY
        author.Should().NotBeNull();
        author.Name.Should().Be("pippo");
        await CheckDatabaseAndRemoveIt(context);
    }
    [Fact]
    public async Task Modify_Existing_Author_Should_NotThrowsAnException()
    {
        // SETUP
        await using var context = new BlogDbContext(GenDbContextOptions());
        await  PrepareDatabase(context).ConfigureAwait(false);
        context.Authors.Add( Author.Create("test", "pippo"));
        await context.SaveChangesAsync();
        //ATTEMPT 
        var repository = new EfRepository(context);
        var author = await repository.GetByIdAsync<Author>("test");
        author.Update("pluto");
        await repository.UpdateAsync<Author>(author);
        author = await repository.GetByIdAsync<Author>("test");
        //VERIFY
        author.Should().NotBeNull();
        author.Name.Should().Be("pluto");
        await CheckDatabaseAndRemoveIt(context);
    }
    [Fact]
    public async Task Modify_NotExisting_Author_Should_ThrowsAnException()
    {
        // SETUP
        await using var context = new BlogDbContext(GenDbContextOptions());
        await  PrepareDatabase(context).ConfigureAwait(false);
        //VERIFY
        var repository = new EfRepository(context);
        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => repository.UpdateAsync<Author>(Author.Create("test", "pippo")));
        await CheckDatabaseAndRemoveIt(context);
    }
    [Fact]
    public async Task Delete_Existing_Author_Should_NotThrowsAnException()
    {
        // SETUP
        await using var context = new BlogDbContext(GenDbContextOptions());
        await  PrepareDatabase(context).ConfigureAwait(false);
        context.Authors.Add( Author.Create("test", "pippo"));
        await context.SaveChangesAsync();
        //ATTEMPT 
        var repository = new EfRepository(context);
        var author = await repository.GetByIdAsync<Author>("test");
        await repository.DeleteAsync<Author>(author);
        //VERIFY
        author = await repository.GetByIdAsync<Author>("test");
        author.Should().BeNull();
        await CheckDatabaseAndRemoveIt(context);
    }
}