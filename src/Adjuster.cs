namespace Collaboration;

public sealed class Adjuster
{
    private const float HUNDRED_PERCENT = 100;

    public List<Adjust> Adjust(Group group)
    {
        if (group.Contributions.Count == 0)
            return Enumerable.Empty<Adjust>().ToList();

        List<Adjust> adjusts = new();

        float fairGoal = CalcFairGoal(group);

        foreach (var mem in group.Members)
        {
            var contributions = group.Contributions.Where(c => c.MemberId == mem.Id);
            float totalSpent = contributions.Any() ? contributions.Sum(c => c.Spent) : 0;

            if (totalSpent == fairGoal)
                continue;

            var adjust = new Adjust()
            {
                GroupId = group.Id,
                MemberId = mem.Id,
                Amount = totalSpent < fairGoal
                    ? (float)Math.Round(fairGoal - totalSpent, 2)
                    : (float)Math.Round(totalSpent - fairGoal, 2),
                Action = totalSpent < fairGoal
                    ? AdjustAction.Compensate
                    : AdjustAction.Receive
            };

            adjusts.Add(adjust);
        }

        return adjusts;
    }

    private static float CalcFairGoal(Group group)
        => (float)group.Contributions.Sum(mc => mc.Spent) / group.Members.Select(c => c.Id).Distinct().Count();
}
