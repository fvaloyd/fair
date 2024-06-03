using Collaboration.Contributions;

namespace Collaboration.Members;

public static class MemberEndpoints
{
    public static void MapMemberEndpoints(this IEndpointRouteBuilder builder)
    {
        var memberGroup = builder
            .MapGroup("/members");

        memberGroup.MapGet("/", (MemberStore mStore) => Results.Ok(mStore.GetAll()));

        memberGroup.MapGet("/{memberId}", (string memberId, MemberStore mStore) => Results.Ok(mStore.GetById(memberId)));

        memberGroup.MapPost("/", (string name, MemberStore mStore) =>
        {
            string id = mStore.Create(name);
            return Results.Ok(id);
        });

        memberGroup.MapPost("/{memberId}/groups/{groupId}/made-contribution", (
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

        memberGroup.MapPost("/{memberId}/groups/{groupId}/join", (
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

        memberGroup.MapGet("/{memberId}/groups/{groupId}/contributions/{contributionTypeId}", (
            string memberId,
            string groupId,
            string contributionTypeId,
            GroupStore gStore) =>
        {
            var g = gStore.GetById(groupId);
            return Results.Ok(g.GetSpecContributionsByMember(new MemberId(memberId), new ContributionTypeId(contributionTypeId)));
        });
    }
}
