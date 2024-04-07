namespace Fair;

public sealed class Group
{
    private readonly HashSet<Member> _members = new();
    public IReadOnlySet<Member> Members => _members;
    private readonly HashSet<Contribution> _contributions = new();
    public HashSet<Contribution> Contributions => _contributions;
    public GroupId Id { get; }

    private Group(Member[] members, Contribution[] contributions)
        => (Id, _members, _contributions) = (new(Guid.NewGuid().ToString()), members.ToHashSet(), contributions.ToHashSet());

    public static Group Create(Member[] members, Contribution[] contributions)
        => new(members, contributions);

    public int GetTotalMembers()
        => Members.Count;

    public float GetTotalSpent()
        => Contributions.Sum(c => c.Spent);

    public void AddContributions(Contribution[] contributions)
    {
        foreach (var cont in contributions)
        {
            Contributions.Add(cont);
        }
    }

    public void AddContribution(Contribution contribution)
    {
        if (!_members.Any(m => m.Id == contribution.MemberId))
            throw new Exception("That member is not part of this group.");
        _contributions.Add(contribution);
    }

    public void IntegrateMember(Member member)
    {
        _members.Add(member);
    }
}

public sealed record GroupId(string Value);
