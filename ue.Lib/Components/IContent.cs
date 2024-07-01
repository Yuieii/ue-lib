namespace ue.Components;

public interface IContent
{
    public void Visit(IContentVisitor visitor, IStyle style);
    public IContent Clone();
}

public interface IContentSelf<out T> : IContent where T : IContentSelf<T>
{
    public new T Clone();
    IContent IContent.Clone() => Clone();
}