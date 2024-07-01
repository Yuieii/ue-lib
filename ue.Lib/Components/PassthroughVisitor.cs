// Copyright (c) 2024 Yuieii.

namespace ue.Components;

/// <summary>
/// A <see cref="PassthroughVisitor"/> consumes entries of a content and a style, and packs each of them into a
/// <see cref="IChatComponent"/>.
/// </summary>
public class PassthroughVisitor : IContentVisitor<List<IChatComponent>>
{
    private readonly List<IChatComponent> _result = [];

    /// <summary>
    /// Visit contents of a <see cref="IChatComponent"/>, and returns a list of consumed contents and style configurations,
    /// each represented with a <see cref="IChatComponent"/>. 
    /// </summary>
    /// <param name="component">The component to visit.</param>
    /// <returns>
    /// A list of visited contents and style configurations, represented with a list of <see cref="IChatComponent"/>.
    /// </returns>
    public List<IChatComponent> Visit(IChatComponent component)
    {
        _result.Clear();
        component.Visit(this, component.Style);
        return _result.ToList();
    }

    bool IContentVisitor.Consume(IContent content, IStyle style)
    {
        _result.Add(new MutableChatComponent(content, style));
        return true;
    }

    void IContentVisitor.ConsumeLiteral(string content, IStyle style)
    {
    }
}