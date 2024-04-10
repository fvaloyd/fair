namespace Collaboration;

public sealed class Contribution
{
    public ContributionId Id { get; }
    public string Name { get; } = string.Empty;
    public float Spent { get; }
    public GroupId GroupId { get; }
    public MemberId MemberId { get; }
    public Period Period { get; }

    private Contribution(string name, float spent, GroupId groupId, MemberId memberId, Period period)
        => (Id, Name, Spent, GroupId, MemberId, Period) = (new(Guid.NewGuid().ToString()), name, spent, groupId, memberId, period);

    public static Contribution Create(string name, float spent, GroupId groupId, MemberId memberId, Period period)
        => new(name, spent, groupId, memberId, period);
}

public sealed record ContributionId(string Value);
