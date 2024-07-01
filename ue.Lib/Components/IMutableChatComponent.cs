namespace ue.Components;

public interface IMutableChatComponent : IChatComponent
{
    /// <inheritdoc cref="IChatComponent.Content"/>
    public new IContent Content { get; set; }
    IContent IChatComponent.Content => Content;
    
    /// <inheritdoc cref="IChatComponent.Style"/>
    public new IStyle Style { get; set; }
    IStyle IChatComponent.Style => Style;
    
    /// <inheritdoc cref="IChatComponent.Siblings"/>
    public new List<IChatComponent> Siblings { get; set; }
    IReadOnlyList<IChatComponent> IChatComponent.Siblings => Siblings;
    
    /// <summary>
    /// Adds another component as a sibling of this component.
    /// </summary>
    /// <param name="sibling">The component to be added as a sibling.</param>
    /// <returns>This mutated component.</returns>
    public IMutableChatComponent AddSibling(IChatComponent sibling);
    
    /// <summary>
    /// Overrides the style with the given style.
    /// </summary>
    /// <param name="style">The style to override the current style with.</param>
    /// <returns>This mutated component.</returns>
    public IMutableChatComponent OverrideStyle(IStyle style);

    /// <summary>
    /// Modify the style based on the current style.
    /// </summary>
    /// <param name="modifier">The style modifier.</param>
    /// <returns>This mutated component.</returns>
    public IMutableChatComponent ModifyStyle(Func<IStyle, IStyle> modifier);
    
    /// <summary>
    /// Modify the style based on the current style.
    /// </summary>
    /// <param name="modifier">The style modifier.</param>
    /// <typeparam name="T">The assumed type of the current style.</typeparam>
    /// <returns>This mutated component.</returns>
    public IMutableChatComponent ModifyStyle<T>(Func<T, IStyle> modifier) where T : IStyle;
}

internal class MutableChatComponent : IMutableChatComponent, IChatComponentSelf<MutableChatComponent>
{
    public IContent Content { get; set; }
    public IStyle Style { get; set; }
    public List<IChatComponent> Siblings { get; set; } = [];

    public MutableChatComponent(IContent content, IStyle style)
    {
        Content = content;
        Style = style;
    }

    public MutableChatComponent(IContent content, IStyle style, IEnumerable<IChatComponent> siblings)
        : this(content, style)
    {
        Siblings.AddRange(siblings);
    }

    public void Visit(IContentVisitor visitor, IStyle style)
    {
        style = style.OverrideFrom(Style);
        Content.Visit(visitor, style);
        
        foreach (var sibling in Siblings)
        {
            sibling.Visit(visitor, style);
        }
    }

    public IMutableChatComponent AddSibling(IChatComponent sibling)
    {
        Siblings.Add(sibling);
        return this;
    }

    public IMutableChatComponent OverrideStyle(IStyle style)
    {
        Style = Style.OverrideFrom(style);
        return this;
    }

    public IMutableChatComponent ModifyStyle(Func<IStyle, IStyle> modifier)
    {
        Style = modifier(Style);
        return this;
    }
    
    public IMutableChatComponent ModifyStyle<T>(Func<T, IStyle> modifier) where T : IStyle
    {
        Style = modifier((T)Style);
        return this;
    }

    public MutableChatComponent Clone() => new(Content.Clone(), Style, Siblings.Select(s => s.Clone()));
}
