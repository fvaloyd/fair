using Collaboration.Contributions;
using Collaboration.Groups;
using Collaboration.Members;

public sealed class ContributionStore
{
    private readonly Dictionary<ContributionId, Contribution> _contributions = new();

    public List<Contribution> GetAll()
        => _contributions.Values.ToList();

    public Contribution GetById(string contributionId)
    {
        _ = _contributions.TryGetValue(new(contributionId), out var contribution);
        return contribution == null
            ? throw new Exception("Could not found the contribution.")
            : contribution;
    }

    public string Create(string contributionTypeId, float spent, string memberId, string groupId, Period period)
    {
        var contribution = Contribution.Create(new ContributionTypeId(contributionTypeId), spent, new GroupId(groupId), new MemberId(memberId), period);
        var result = _contributions.TryAdd(contribution.Id, contribution);
        return result
            ? contribution.Id.Value
            : throw new Exception("Could not create the contribution.");
    }
}

