using Collaboration.Contributions;

public sealed class ContributionTypeStore
{
    private readonly Dictionary<ContributionTypeId, ContributionType> _contributionsType = new();

    public List<ContributionType> GetAll()
        => _contributionsType.Values.ToList();

    public ContributionType GetById(string contributionTypeId)
    {
        _ = _contributionsType.TryGetValue(new(contributionTypeId), out var contributionType);
        return contributionType == null
            ? throw new Exception("Could not found the contributionType.")
            : contributionType;
    }

    public string Create(string name)
    {
        var contributionType = ContributionType.Create(name);
        var result = _contributionsType.TryAdd(contributionType.Id, contributionType);
        return result
            ? contributionType.Id.Value
            : throw new Exception("Could not create the contributionType.");
    }
}

