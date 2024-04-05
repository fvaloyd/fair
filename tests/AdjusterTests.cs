using Fair;

namespace Collaboration.Tests;

public class AdjusterTests
{
    private readonly Adjuster _adjuster = new Adjuster();

    [Fact]
    public void WithAGroupWithNoContributions_ShouldReturnAnEmptyCollection()
    {
        var m1 = Member.Create("m1");
        var m2 = Member.Create("m2");
        var m3 = Member.Create("m3");
        var g = Group.Create(new Member[] { m1, m2, m3 }, Enumerable.Empty<Contribution>().ToArray());
        List<Adjust> adjusts = _adjuster.Adjust(g);

        Assert.Empty(adjusts);
    }

    [Fact]
    public void WithAMemberWithNoContribution_ShouldReturnACollectionWithTheCorrespondingAdjustments()
    {
        var m1 = Member.Create("m1");
        var m2 = Member.Create("m2");
        var m3 = Member.Create("m3");
        var g = Group.Create(new Member[] { m1, m2, m3 }, Enumerable.Empty<Contribution>().ToArray());

        var c1 = Contribution.Create("Supermarket", 2000, g.Id, m1.Id);
        var c2 = Contribution.Create("Supermarket", 2000, g.Id, m2.Id);
        g.AddContributions(new Contribution[] { c1, c2 });

        List<Adjust> adjusts = _adjuster.Adjust(g);

        var m1Adjust = adjusts.Where(a => a.MemberId == m1.Id).First();
        Assert.Equal(666.67f, m1Adjust.Amount);
        Assert.Equal(AdjustAction.Receive, m1Adjust.Action);

        var m2Adjust = adjusts.Where(a => a.MemberId == m2.Id).First();
        Assert.Equal(666.67f, m2Adjust.Amount);
        Assert.Equal(AdjustAction.Receive, m2Adjust.Action);

        var m3Adjust = adjusts.Where(a => a.MemberId == m3.Id).First();
        Assert.Equal(1333.33f, m3Adjust.Amount);
        Assert.Equal(AdjustAction.Compensate, m3Adjust.Action);
    }

    [Fact]
    public void ForMembersWithDiffentContribution_ShouldReturnACollectionWithTheCorrespondingAdjustments()
    {
        var m1 = Member.Create("m1");
        var m2 = Member.Create("m2");
        var m3 = Member.Create("m3");
        var g = Group.Create(new Member[] { m1, m2, m3 }, Enumerable.Empty<Contribution>().ToArray());

        var c1 = Contribution.Create("Supermarket", 2000, g.Id, m1.Id);
        var c2 = Contribution.Create("Supermarket", 1800, g.Id, m2.Id);
        var c3 = Contribution.Create("Supermarket", 1600, g.Id, m3.Id);
        g.AddContributions(new Contribution[] { c1, c2, c3 });

        List<Adjust> adjusts = _adjuster.Adjust(g);

        var m1Adjust = adjusts.Where(a => a.MemberId == m1.Id).First();
        Assert.Equal(200, m1Adjust.Amount);
        Assert.Equal(AdjustAction.Receive, m1Adjust.Action);

        var m3Adjust = adjusts.Where(a => a.MemberId == m3.Id).First();
        Assert.Equal(200, m3Adjust.Amount);
        Assert.Equal(AdjustAction.Compensate, m3Adjust.Action);

        Assert.Empty(adjusts.Where(a => a.MemberId == m2.Id));
    }
}
