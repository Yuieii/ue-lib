using System.Text;

namespace ue.Components;

public class PlainTextVisitor : IContentVisitor<string>
{
    private readonly StringBuilder _sb = new();
    
    public string Visit(IChatComponent component)
    {
        _sb.Clear();
        component.Visit(this, component.Style);
        return _sb.ToString();
    }

    bool IContentVisitor.Consume(IContent content, IStyle style) => false;

    void IContentVisitor.ConsumeLiteral(string content, IStyle style) => _sb.Append(content);
}