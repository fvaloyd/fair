namespace Collaboration;

public sealed class ContributionType
{
    public ContributionTypeId Id { get; }
    public string Name { get; } = string.Empty;

    public ContributionType(string name)
        => (Id, Name) = (new(Guid.NewGuid().ToString()), name);

    public static ContributionType Create(string name)
        => new(name);
}

public record struct ContributionTypeId(string Value);
