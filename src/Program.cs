var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();

public sealed class Group
{
    public List<Member> Members { get; set; } = new();
    public List<Contribution> Contributions { get; set; } = new();
    public string Id { get; set; } = string.Empty;
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
