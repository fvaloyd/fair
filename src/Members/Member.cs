namespace Collaboration.Members;

public sealed class Member
{
    public MemberId Id { get; }
    public string Name { get; } = string.Empty;

    private Member(string name)
        => (Id, Name) = (new(Guid.NewGuid().ToString()), name);

    public static Member Create(string name)
        => new(name);
}

public sealed record MemberId(string Value);
