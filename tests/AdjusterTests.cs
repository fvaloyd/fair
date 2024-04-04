namespace Collaboration.Tests;

public class AdjusterTests
{
    private readonly Adjuster _adjuster = new Adjuster();

    [Fact]
    public void WithAGroupWithNoContributions_ShouldReturnAnEmptyCollection()
    {
        var m1 = new Member() { Id = Guid.NewGuid().ToString(), Name = "m1" };
        var m2 = new Member() { Id = Guid.NewGuid().ToString(), Name = "m2" };
        var m3 = new Member() { Id = Guid.NewGuid().ToString(), Name = "m3" };
        var g = new Group() { Id = Guid.NewGuid().ToString(), Members = new HashSet<Member>() { m1, m2, m3 } };

        List<Adjust> adjusts = _adjuster.Adjust(g);

        Assert.Empty(adjusts);
    }

    [Fact]
    public void WithAMemberWithNoContribution_ShouldReturnACollectionWithTheCorrespondingAdjustments()
    {
        var m1 = new Member() { Id = Guid.NewGuid().ToString(), Name = "m1" };
        var m2 = new Member() { Id = Guid.NewGuid().ToString(), Name = "m2" };
        var m3 = new Member() { Id = Guid.NewGuid().ToString(), Name = "m3" };
        var g = new Group() { Id = Guid.NewGuid().ToString(), Members = new HashSet<Member>() { m1, m2, m3 } };

        var c1 = new Contribution() { Name = "Supermarket", GroupId = g.Id, MemberId = m1.Id, Spent = 2000 };
        var c2 = new Contribution() { Name = "Supermarket", GroupId = g.Id, MemberId = m2.Id, Spent = 2000 };
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
        var m1 = new Member() { Id = Guid.NewGuid().ToString(), Name = "m1" };
        var m2 = new Member() { Id = Guid.NewGuid().ToString(), Name = "m2" };
        var m3 = new Member() { Id = Guid.NewGuid().ToString(), Name = "m3" };
        var g = new Group() { Id = Guid.NewGuid().ToString(), Members = new HashSet<Member>() { m1, m2, m3 } };

        var c1 = new Contribution() { Name = "Supermarket", GroupId = g.Id, MemberId = m1.Id, Spent = 2000 };
        var c2 = new Contribution() { Name = "Supermarket", GroupId = g.Id, MemberId = m2.Id, Spent = 1800 };
        var c3 = new Contribution() { Name = "Supermarket", GroupId = g.Id, MemberId = m3.Id, Spent = 1600 };
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
