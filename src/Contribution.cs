namespace Fair;

public sealed class Contribution
{
    public ContributionId Id { get; }
    public string Name { get; } = string.Empty;
    public float Spent { get; }
    public GroupId GroupId { get; }
    public MemberId MemberId { get; }

    private Contribution(string name, float spent, GroupId groupId, MemberId memberId)
        => (Id, Name, Spent, GroupId, MemberId) = (new(Guid.NewGuid().ToString()), name, spent, groupId, memberId);

    public static Contribution Create(string name, float spent, GroupId groupId, MemberId memberId)
        => new(name, spent, groupId, memberId);
}

public sealed record ContributionId(string Value);
