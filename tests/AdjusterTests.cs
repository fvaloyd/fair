using Collaboration.Members;
using Collaboration.Adjusters;
using Collaboration.Contributions;
using Collaboration.Groups;

namespace Collaboration.Tests;

public class AdjusterTests
{
    private readonly Adjuster _adjuster = new Adjuster();

    [Fact]
    public void Adjust_WithoutContributions_ShouldReturnAnEmptyCollection()
    {
        var m1 = Member.Create("m1");
        var m2 = Member.Create("m2");
        var m3 = Member.Create("m3");
        var g = Group.Create(new Member[] { m1, m2, m3 }, Enumerable.Empty<Contribution>().ToArray());
        List<Adjustment> adjustments = _adjuster.Adjust(g);

        Assert.Empty(adjustments);
    }

    [Fact]
    public void Adjust_WithAMemberWithNoContribution_ShouldReturnTheAdjustments()
    {
        var m1 = Member.Create("m1");
        var m2 = Member.Create("m2");
        var m3 = Member.Create("m3");
        var g = Group.Create(new Member[] { m1, m2, m3 }, Enumerable.Empty<Contribution>().ToArray());

        var ct = ContributionType.Create("Supermarket");
        var c1 = Contribution.Create(ct.Id, 2000, g.Id, m1.Id, g.CurrentPeriod);
        var c2 = Contribution.Create(ct.Id, 2000, g.Id, m2.Id, g.CurrentPeriod);
        g.AddContributions(new Contribution[] { c1, c2 });

        List<Adjustment> adjustments = _adjuster.Adjust(g);

        var m1Adjustment = adjustments.Where(a => a.MemberId == m1.Id).First();
        Assert.Equal(666.67f, m1Adjustment.Amount);
        Assert.Equal(AdjustAction.Receive, m1Adjustment.Action);

        var m2Adjustment = adjustments.Where(a => a.MemberId == m2.Id).First();
        Assert.Equal(666.67f, m2Adjustment.Amount);
        Assert.Equal(AdjustAction.Receive, m2Adjustment.Action);

        var m3Adjustment = adjustments.Where(a => a.MemberId == m3.Id).First();
        Assert.Equal(1333.33f, m3Adjustment.Amount);
        Assert.Equal(AdjustAction.Compensate, m3Adjustment.Action);
    }

    [Fact]
    public void Adjust_ForMembersWithDiffentContribution_ShouldReturnTheAdjustments()
    {
        var m1 = Member.Create("m1");
        var m2 = Member.Create("m2");
        var m3 = Member.Create("m3");
        var g = Group.Create(new Member[] { m1, m2, m3 }, Enumerable.Empty<Contribution>().ToArray());

        var ct = ContributionType.Create("Supermarket");
        var c1 = Contribution.Create(ct.Id, 2000, g.Id, m1.Id, g.CurrentPeriod);
        var c2 = Contribution.Create(ct.Id, 1800, g.Id, m2.Id, g.CurrentPeriod);
        var c3 = Contribution.Create(ct.Id, 1600, g.Id, m3.Id, g.CurrentPeriod);
        g.AddContributions(new Contribution[] { c1, c2, c3 });

        List<Adjustment> adjusts = _adjuster.Adjust(g);

        var m1Adjust = adjusts.Where(a => a.MemberId == m1.Id).First();
        Assert.Equal(200, m1Adjust.Amount);
        Assert.Equal(AdjustAction.Receive, m1Adjust.Action);

        var m3Adjust = adjusts.Where(a => a.MemberId == m3.Id).First();
        Assert.Equal(200, m3Adjust.Amount);
        Assert.Equal(AdjustAction.Compensate, m3Adjust.Action);

        Assert.Empty(adjusts.Where(a => a.MemberId == m2.Id));
    }

    [Fact]
    public void Adjust_ShouldReturnTheAdjustments()
    {
        var m1 = Member.Create("m1");
        var m2 = Member.Create("m2");
        var g = Group.Create(new Member[] { m1, m2 }, Enumerable.Empty<Contribution>().ToArray());

        var ct = ContributionType.Create("Supermarket");
        var c1 = Contribution.Create(ct.Id, 5000, g.Id, m1.Id, g.CurrentPeriod);
        var c2 = Contribution.Create(ct.Id, 4500, g.Id, m2.Id, g.CurrentPeriod);
        g.AddContributions(new Contribution[] { c1, c2 });

        List<Adjustment> adjusts = _adjuster.Adjust(g);

        var m1Adjust = adjusts.Where(a => a.MemberId == m1.Id).First();
        Assert.Equal(250, m1Adjust.Amount);
        Assert.Equal(AdjustAction.Receive, m1Adjust.Action);

        var m2Adjust = adjusts.Where(a => a.MemberId == m2.Id).First();
        Assert.Equal(250, m2Adjust.Amount);
        Assert.Equal(AdjustAction.Compensate, m2Adjust.Action);
    }
}
