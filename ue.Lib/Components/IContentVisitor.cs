namespace ue.Components;

public interface IContentVisitor
{
    /// <summary>
    /// Attempts to consume the content directly.
    /// </summary>
    /// <param name="content">The target content.</param>
    /// <param name="style">The style to apply to the content.</param>
    /// <returns>
    /// <see langword="True"/> if the visitor understands the content and consumed the content.
    /// </returns>
    public bool Consume(IContent content, IStyle style);
    
    /// <summary>
    /// Provides the literal representation of the content to the visitor.
    /// </summary>
    /// <param name="content">The literal representation of the content.</param>
    /// <param name="style">The style to apply to the content.</param>
    public void ConsumeLiteral(string content, IStyle style);
}

public interface IContentVisitor<out T> : IContentVisitor
{
    /// <summary>
    /// Visit contents of a <see cref="IChatComponent"/>, and returns the result that the visitor produces.
    /// </summary>
    /// <param name="component">The component to visit.</param>
    /// <returns>The produced result.</returns>
    public T Visit(IChatComponent component);
}