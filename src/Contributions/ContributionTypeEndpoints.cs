namespace Collaboration.Contributions;

public static class ContributionTypeEndpoints
{
    public static void MapContributionTypeEndpoints(this IEndpointRouteBuilder builder)
    {
        var contributionTypeGroup = builder
            .MapGroup("contributionTypes");

        contributionTypeGroup.MapGet("/contributionsType", (ContributionTypeStore ctStore) => Results.Ok(ctStore.GetAll()));

        contributionTypeGroup.MapGet("/contributionsType/{contributionTypeId}", (string contributionTypeId, ContributionTypeStore ctStore) => Results.Ok(ctStore.GetById(contributionTypeId)));

        contributionTypeGroup.MapPost("/contributionsType", (string name, ContributionTypeStore ctStore) =>
        {
            string id = ctStore.Create(name);
            return Results.Ok(id);
        });
    }
}
