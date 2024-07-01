// Copyright (c) 2024 Yuieii.

using System.Text;
using ue.Texts;

namespace ue.Components;

public class AnsiTextVisitor : IContentVisitor<string>
{
    private readonly StringBuilder _sb = new();

    public string Visit(IChatComponent component)
    {
        _sb.Clear();
        component.Visit(this, component.Style);
        return _sb.ToString();
    }

    bool IContentVisitor.Consume(IContent content, IStyle style) => false;

    void IContentVisitor.ConsumeLiteral(string content, IStyle style)
    {
        _sb.Append(ClassicAnsiColor.Reset.ToAnsiCode());

        if (style is ITextColorStyle colorStyle)
        {
            var color = colorStyle.Color == null
                ? ClassicAnsiColor.Reset
                : UeConstants.AnsiUseRgb
                    ? AnsiColor.CreateRgb(colorStyle.Color)
                    : AnsiColor.FromTextColor(colorStyle.Color);

            _sb.Append(color.ToAnsiCode());
        }

        if (style is ITerminalTextStyle terminalStyle)
        {
            if (terminalStyle.Bold == true)
                _sb.Append("\u001b[1m");

            if (terminalStyle.Italic == true)
                _sb.Append("\u001b[3m");

            if (terminalStyle.Underlined == true)
                _sb.Append("\u001b[4m");

            if (terminalStyle.Strikethrough == true)
                _sb.Append("\u001b[53m");
        }

        _sb.Append(content);
        _sb.Append(ClassicAnsiColor.Reset.ToAnsiCode());
    }
}