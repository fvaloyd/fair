namespace Collaboration;

public sealed class Adjustment
{
    public GroupId GroupId { get; }
    public MemberId MemberId { get; }
    public float Amount { get; }
    public AdjustAction Action { get; }

    private Adjustment(GroupId groupId, MemberId memberId, float amount, AdjustAction action)
        => (GroupId, MemberId, Amount, Action) = (groupId, memberId, amount, action);

    public static Adjustment Create(GroupId groupId, MemberId memberId, float amount, AdjustAction action)
        => new(groupId, memberId, amount, action);
}

public enum AdjustAction
{
    Receive,
    Compensate
}
