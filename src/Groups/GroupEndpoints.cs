using Collaboration.Adjusters;

namespace Collaboration.Groups;

public static class GroupEndpoints
{
    public static void MapGroupEndpoints(this IEndpointRouteBuilder builder)
    {
        var groupGroup = builder
            .MapGroup("/groups");

        groupGroup.MapGet("/", (GroupStore gStore) => Results.Ok(gStore.GetAll()));

        groupGroup.MapGet("/{groupId}", (string groupId, GroupStore gStore) => Results.Ok(gStore.GetById(groupId)));

        groupGroup.MapPost("/", (GroupStore gStore) =>
        {
            string id = gStore.Create();
            return Results.Ok(id);
        });

        groupGroup.MapPost("/{groupId}/adjust-contributions", (
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

        groupGroup.MapPost("/{groupId}/close-period", (string groupId, GroupStore gStore) =>
        {
            var group = gStore.GetById(groupId);
            group.ClosePeriod();
            return Results.Ok();
        });

        groupGroup.MapPost("/{groupId}/start-period", (string groupId, GroupStore gStore) =>
        {
            var group = gStore.GetById(groupId);
            group.StartNewPeriod();
            return Results.Ok();
        });
    }
}

record AdjustResponse(string Name, float Amount, string Action);
