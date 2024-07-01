namespace ue.Components;

public abstract class ComponentResolver
{
    public IChatComponent Resolve(IChatComponent component)
    {
        var source = new List<IChatComponent>();
        var clone = component.Clone();
        source.AddRange(clone.Siblings);
        clone.Siblings.Clear();
        source.Insert(0, clone);

        var components = new List<IChatComponent>();
        foreach (var comp in source)
        {
            if (comp.Content is not LiteralContent literal)
            {
                components.Add(comp);
                continue;
            }

            var offset = 0;
            var content = literal.Text;
            var matches = GetResolvedParts(content);
            
            foreach (var match in matches)
            {
                var range = match.Range;
                components.Add(new MutableChatComponent(new LiteralContent(content[offset..range.Start]), comp.Style));
                offset = range.End.Value;

                var produced = match.Resolve(comp.Style);
                if (produced != null) components.Add(produced);
            }

            var remaining = content[offset..];
            if (!string.IsNullOrEmpty(remaining))
                components.Add(new MutableChatComponent(new LiteralContent(remaining), comp.Style));
        }

        return ComponentFlattener.Default.Flatten(components);
    }

    public abstract IReadOnlyList<IResolvedComponentPart> GetResolvedParts(string content);
}

public interface IResolvedComponentPart
{
    public Range Range { get; }
    public IChatComponent? Resolve(IStyle style);
}