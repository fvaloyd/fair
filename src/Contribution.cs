namespace Collaboration;

public sealed class Contribution
{
    public ContributionId Id { get; }
    public ContributionTypeId ContributionTypeId { get; }
    public float Spent { get; }
    public GroupId GroupId { get; }
    public MemberId MemberId { get; }
    public Period Period { get; }

    private Contribution(ContributionTypeId contributionTypeId, float spent, GroupId groupId, MemberId memberId, Period period)
        => (Id, ContributionTypeId, Spent, GroupId, MemberId, Period) = (new(Guid.NewGuid().ToString()), contributionTypeId, spent, groupId, memberId, period);

    public static Contribution Create(ContributionTypeId contributionTypeId, float spent, GroupId groupId, MemberId memberId, Period period)
        => new(contributionTypeId, spent, groupId, memberId, period);
}

public sealed record ContributionId(string Value);
