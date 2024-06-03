using Collaboration.Contributions;
using Collaboration.Groups;
using Collaboration.Members;

public sealed class GroupStore
{
    private readonly Dictionary<GroupId, Group> _groups = new();

    public List<Group> GetAll()
        => _groups.Values.ToList();

    public Group GetById(string groupId)
    {
        _ = _groups.TryGetValue(new(groupId), out var group);
        return group == null
            ? throw new Exception("Could not found the group.")
            : group;
    }

    public string Create()
    {
        var group = Group.Create(Enumerable.Empty<Member>().ToArray(), Enumerable.Empty<Contribution>().ToArray());
        var result = _groups.TryAdd(group.Id, group);
        return result
            ? group.Id.Value
            : throw new Exception("Could not create the group.");
    }
}

