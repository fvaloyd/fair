using AdjustType = Collaboration.Adjustment;

namespace Collaboration;

public sealed class Adjuster
{
    public List<Adjustment> Adjust(Group group)
    {
        if (group.Contributions.Count == 0)
            return Enumerable.Empty<Adjustment>().ToList();

        List<Adjustment> adjusts = new();

        float fairGoal = CalcFairGoal(group);

        foreach (var mem in group.Members)
        {
            var memContributions = group.Contributions.Where(c => c.MemberId == mem.Id);
            float totalSpent = memContributions.Any() ? memContributions.Sum(c => c.Spent) : 0;

            if (totalSpent == fairGoal)
                continue;

            var adjust = AdjustType.Create(
                    group.Id,
                    mem.Id,
                    totalSpent < fairGoal
                        ? (float)Math.Round(fairGoal - totalSpent, 2)
                        : (float)Math.Round(totalSpent - fairGoal, 2),
                    totalSpent < fairGoal
                        ? AdjustAction.Compensate
                        : AdjustAction.Receive);

            adjusts.Add(adjust);
        }

        return adjusts;
    }

    private static float CalcFairGoal(Group group)
        => group.GetTotalSpent() / group.GetTotalMembers();
}
