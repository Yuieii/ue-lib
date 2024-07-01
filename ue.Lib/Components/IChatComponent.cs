// Copyright (c) 2024 Yuieii.

namespace ue.Components;

public interface IChatComponent
{
    /// <summary>
    /// The content of the component.
    /// </summary>
    public IContent Content { get; }

    /// <summary>
    /// The style applied to this component and its siblings. Overrides the style inherited from the parent.
    /// </summary>
    public IStyle Style { get; }

    /// <summary>
    /// The siblings of this component. The style of this component will be applied to these siblings.
    /// </summary>
    public IReadOnlyList<IChatComponent> Siblings { get; }

    /// <summary>
    /// Creates a mutable copy of this component.
    /// </summary>
    /// <returns></returns>
    public IMutableChatComponent Clone();

    /// <summary>
    /// Provide its content and style, alongside with its siblings, to the visitor.
    /// </summary>
    /// <param name="visitor">The visitor that will consume contents and style configurations.</param>
    /// <param name="style">The base style that will be overriden by the style of this component.</param>
    public void Visit(IContentVisitor visitor, IStyle style);
}

public interface IChatComponentSelf<out T> : IChatComponent where T : IChatComponentSelf<T>, IMutableChatComponent
{
    /// <inheritdoc cref="IChatComponent.Clone"/>
    public new T Clone();

    IMutableChatComponent IChatComponent.Clone() => Clone();
}