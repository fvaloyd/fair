var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();

public sealed class Group
{
    public HashSet<Member> Members { get; init; } = new();
    public HashSet<Contribution> Contributions { get; init; } = new();
    public string Id { get; init; } = string.Empty;

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
}

public sealed class Member
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

public sealed class Contribution
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public float Spent { get; set; }
    public string GroupId { get; set; } = string.Empty;
    public string MemberId { get; set; } = string.Empty;
}

public sealed class Adjust
{
    public string GroupId { get; set; } = string.Empty;
    public string MemberId { get; set; } = string.Empty;
    public float Amount { get; set; }
    public AdjustAction Action { get; set; }
}

public enum AdjustAction
{
    Receive,
    Compensate
}
