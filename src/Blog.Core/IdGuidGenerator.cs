using Blog.Domain.Interfaces;

namespace Blog.Core;

public class IdGuidGenerator : IIdGenerator
{
    public string GenerateId() => Guid.NewGuid().ToString();
}