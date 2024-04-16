using Collaboration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<GroupStore>();
builder.Services.AddSingleton<MemberStore>();
builder.Services.AddSingleton<ContributionStore>();
builder.Services.AddSingleton<ContributionTypeStore>();
builder.Services.AddScoped<Adjuster>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/groups", (GroupStore gStore) => Results.Ok(gStore.GetAll()));
app.MapGet("/groups/{groupId}", (string groupId, GroupStore gStore) => Results.Ok(gStore.GetById(groupId)));
app.MapPost("/groups", (GroupStore gStore) =>
{
    string id = gStore.Create();
    return Results.Ok(id);
});
app.MapPost("/groups/{groupId}/adjust-contributions", (
            string groupId,
            GroupStore gStore,
            MemberStore mStore,
            Adjuster adjuster) =>
{
    var group = gStore.GetById(groupId);
    var adjusts = adjuster.Adjust(group);
    var adjustsResponse = adjusts.Select(a =>
    {
        var member = mStore.GetById(a.MemberId.Value);
        return new AdjustResponse(member.Name, a.Amount, a.Action.ToString());
    });
    return Results.Ok(adjustsResponse);
});
app.MapPost("/groups/{groupId}/close-period", (string groupId, GroupStore gStore) =>
{
    var group = gStore.GetById(groupId);
    group.ClosePeriod();
    return Results.Ok();
});
app.MapPost("/groups/{groupId}/start-period", (string groupId, GroupStore gStore) =>
{
    var group = gStore.GetById(groupId);
    group.StartNewPeriod();
    return Results.Ok();
});

app.MapGet("/members", (MemberStore mStore) => Results.Ok(mStore.GetAll()));
app.MapGet("/members/{memberId}", (string memberId, MemberStore mStore) => Results.Ok(mStore.GetById(memberId)));
app.MapPost("/members", (string name, MemberStore mStore) =>
{
    string id = mStore.Create(name);
    return Results.Ok(id);
});
app.MapPost("/members/{memberId}/groups/{groupId}/made-contribution", (
            string memberId,
            string groupId,
            string contributionTypeId,
            float spent,
            GroupStore gStore,
            MemberStore mStore,
            ContributionStore cStore) =>
{
    var member = mStore.GetById(memberId);
    var group = gStore.GetById(groupId);
    var contrId = cStore.Create(contributionTypeId, spent, memberId, groupId, group.CurrentPeriod);
    var contributionCreated = cStore.GetById(contrId);
    group.AddContribution(contributionCreated);
});

app.MapPost("/members/{memberId}/groups/{groupId}/join", (
            string memberId,
            string groupId,
            GroupStore gStore,
            MemberStore mStore) =>
{
    var member = mStore.GetById(memberId);
    var group = gStore.GetById(groupId);
    group.JoinMember(member);
    return Results.Ok();
});

app.MapGet("/contributionsType", (ContributionTypeStore ctStore) => Results.Ok(ctStore.GetAll()));
app.MapGet("/contributionsType/{contributionTypeId}", (string contributionTypeId, ContributionTypeStore ctStore) => Results.Ok(ctStore.GetById(contributionTypeId)));
app.MapPost("/contributionsType", (string name, ContributionTypeStore ctStore) =>
{
    string id = ctStore.Create(name);
    return Results.Ok(id);
});

app.Run();

public record AdjustResponse(string Member, float Amount, string Action);

public record MadeContributionRequest(string name, float spent);

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

public sealed class GroupStore
{
    private readonly Dictionary<GroupId, Group> _groups = new();

    public List<Group> GetAll()
        => _groups.Values.ToList();

    public Group GetById(string groupId)
    {
        _ = _groups.TryGetValue(new(groupId), out var group);
        return group == null
            ? throw new Exception("Could not found the group.")
            : group;
    }

    public string Create()
    {
        var group = Group.Create(Enumerable.Empty<Member>().ToArray(), Enumerable.Empty<Contribution>().ToArray());
        var result = _groups.TryAdd(group.Id, group);
        return result
            ? group.Id.Value
            : throw new Exception("Could not create the group.");
    }
}

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

public sealed class MemberStore
{
    private readonly Dictionary<MemberId, Member> _members = new();

    public List<Member> GetAll()
        => _members.Values.ToList();

    public Member GetById(string memberId)
    {
        _ = _members.TryGetValue(new(memberId), out var member);
        return member is null
            ? throw new Exception("Could not found the member.")
            : member;
    }

    public string Create(string name)
    {
        var member = Member.Create(name);
        var result = _members.TryAdd(member.Id, member);
        return result
            ? member.Id.Value
            : throw new Exception("Could not create the member.");
    }
}
