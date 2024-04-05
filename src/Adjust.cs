namespace Fair;

public sealed class Adjust
{
    public GroupId GroupId { get; }
    public MemberId MemberId { get; }
    public float Amount { get; }
    public AdjustAction Action { get; }

    private Adjust(GroupId groupId, MemberId memberId, float amount, AdjustAction action)
        => (GroupId, MemberId, Amount, Action) = (groupId, memberId, amount, action);

    public static Adjust Create(GroupId groupId, MemberId memberId, float amount, AdjustAction action)
        => new(groupId, memberId, amount, action);
}

public enum AdjustAction
{
    Receive,
    Compensate
}
