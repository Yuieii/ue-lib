namespace ue.Components;

public class LiteralContent(string text) : IContentSelf<LiteralContent>
{
    public string Text { get; } = text;

    public LiteralContent Clone() => new(Text);

    public void Visit(IContentVisitor visitor, IStyle style)
    {
        if (!visitor.Consume(this, style))
            visitor.ConsumeLiteral(Text, style);
    }
}