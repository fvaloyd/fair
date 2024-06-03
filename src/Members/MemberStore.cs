using Collaboration.Members;

public sealed class MemberStore
{
    private readonly Dictionary<MemberId, Member> _members = new();

    public List<Member> GetAll()
        => _members.Values.ToList();

    public Member GetById(string memberId)
    {
        _ = _members.TryGetValue(new(memberId), out var member);
        return member is null
            ? throw new Exception("Could not found the member.")
            : member;
    }

    public string Create(string name)
    {
        var member = Member.Create(name);
        var result = _members.TryAdd(member.Id, member);
        return result
            ? member.Id.Value
            : throw new Exception("Could not create the member.");
    }
}

