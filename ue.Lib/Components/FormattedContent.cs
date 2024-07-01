// Copyright (c) 2024 Yuieii.

using System.Text.RegularExpressions;
using ue.Extensions;

namespace ue.Components;

public class FormattedContent : IContentSelf<FormattedContent>
{
    private readonly List<IChatComponent> _parameters = [];
    public IReadOnlyList<IChatComponent> Parameters => _parameters;

    private readonly Dictionary<Type, List<IChatComponent>> _decomposed = new();

    private List<IChatComponent> Decompose(IStyle style)
    {
        return _decomposed.ComputeIfAbsent(style.GetType(), _ =>
        {
            var offset = 0;
            var counter = 0;
            var fmt = Format;
            var matches = new Regex(@"%(?:(?:(\d*?)\$)?)s").Matches(fmt);
            var parameters = _parameters.ToList();

            var result = new List<IChatComponent>();
            foreach (Match m in matches)
            {
                var c = m.Groups[1].Value;
                var ci = c.Length == 0 ? counter++ : int.Parse(c) - 1;

                var front = fmt[offset..m.Index];
                if (front.Length > 0)
                    result.Add(new MutableChatComponent(new LiteralContent(front), style.Clear()));

                result.Add(ci >= parameters.Count && ci < 0
                    ? new MutableChatComponent(new LiteralContent(m.Value), style.Clear())
                    : parameters[ci].Clone());

                offset = m.Index + m.Length;
            }

            result.Add(new MutableChatComponent(new LiteralContent(fmt[offset..]), style.Clear()));
            return result;
        });
    }

    public string Format { get; }

    public FormattedContent(string format, params IChatComponent[] contents)
    {
        Format = format;
        _parameters.AddRange(contents);
    }

    public FormattedContent(string format, IEnumerable<IChatComponent> contents)
    {
        Format = format;
        _parameters.AddRange(contents);
    }

    public void Visit(IContentVisitor visitor, IStyle style)
    {
        var decomposed = Decompose(style);
        foreach (var component in decomposed)
        {
            component.Visit(visitor, style);
        }
    }

    public FormattedContent Clone() => new(Format, _parameters.Select(x => x.Clone()));
}