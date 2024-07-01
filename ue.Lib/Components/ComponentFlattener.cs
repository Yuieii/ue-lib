namespace ue.Components;

public class ComponentFlattener
{
    public static ComponentFlattener Default => new();

    public IChatComponent Flatten(IChatComponent component)
    {
        var storage = new List<IChatComponent>();
        VisitFlatten(component, storage);
        return Flatten(storage);
    }

    public IChatComponent Flatten(IEnumerable<IChatComponent> components)
    {
        var storage = components.ToList();
        storage.RemoveAll(x => x.Content is LiteralContent literal && string.IsNullOrEmpty(literal.Text));
        
        if (!storage.Any()) 
            return ChatComponents.Literal("");

        var first = storage.First();
        var a = new MutableChatComponent(new LiteralContent(""), first.Style.Clear());
        foreach (var component in storage)
        {
            a.Siblings.Add(component);
        }

        return a;
    }

    private static void VisitFlatten(IChatComponent component, List<IChatComponent> storage)
    {
        var x = component.Clone();
        x.Siblings.Clear();
        storage.Add(x);
        
        foreach (var sibling in component.Siblings)
        {
            VisitFlatten(sibling, storage);
        }
    }
}