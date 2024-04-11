namespace Collaboration;

public sealed class Group
{
    private readonly HashSet<Member> _members = new();
    public IReadOnlySet<Member> Members => _members;
    private readonly HashSet<Contribution> _contributions = new();
    public HashSet<Contribution> Contributions => _contributions.Where(c => c.Period == CurrentPeriod).ToHashSet();
    public Period CurrentPeriod { get; private set; }
    public GroupId Id { get; }

    private Group(Member[] members, Contribution[] contributions)
        => (Id, _members, _contributions, CurrentPeriod) = (new(Guid.NewGuid().ToString()), members.ToHashSet(), contributions.ToHashSet(), Period.CreateCurrent());

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
            _contributions.Add(cont);
        }
    }

    public void AddContribution(Contribution contribution)
    {
        if (!_members.Any(m => m.Id == contribution.MemberId))
            throw new Exception("That member is not part of this group.");
        _contributions.Add(contribution);
    }

    public void JoinMember(Member member)
    {
        _members.Add(member);
    }

    public void ClosePeriod()
        => CurrentPeriod = Period.Empty();

    public void StartNewPeriod()
        => CurrentPeriod = Period.CreateCurrent();
}

public sealed record GroupId(string Value);
