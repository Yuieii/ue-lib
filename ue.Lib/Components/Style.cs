using System.Text.Json.Nodes;

namespace ue.Components;

public record Style(
    TextColor? Color,
    TextColor? BackColor,
    bool? Bold,
    bool? Italic,
    bool? Underlined,
    bool? Strikethrough,
    bool? Reset)
    : ITerminalTextStyle, IStyleSelf<Style>
{
    public static Style Empty { get; } = new(null, null, null, null, null, null, null);
    
    public void Serialize(JsonObject obj)
    {
        
    }

    public IStyle OverrideFrom(IStyle other)
    {
        var reset = other is Style { Reset: true };
        var color = !reset ? Color : null;
        var backColor = !reset ? BackColor : null;
        var bold = !reset ? Bold : null;
        var italic = !reset ? Italic : null;
        var underlined = !reset ? Underlined : null;
        var strikethrough = !reset ? Strikethrough : null;
        
        if (other is ITextColorStyle colorStyle) 
            color = colorStyle.Color ?? color;

        if (other is IBackColorStyle backColorStyle)
            backColor = backColorStyle.BackColor ?? backColor;

        if (other is ITerminalTextStyle terminalStyle)
        {
            bold = terminalStyle.Bold ?? bold;
            italic = terminalStyle.Italic ?? italic;
            underlined = terminalStyle.Underlined ?? underlined;
            strikethrough = terminalStyle.Strikethrough ?? strikethrough;
        }

        return new Style(
            Color: color,
            BackColor: backColor,
            Bold: bold,
            Italic: italic,
            Underlined: underlined,
            Strikethrough: strikethrough,
            Reset: false);
    }

    public Style SetColor(TextColor? color) => this with
    {
        Color = color
    };

    public Style SetBold(bool? value) => this with
    {
        Bold = value
    };

    public Style SetItalic(bool? value) => this with
    {
        Italic = value
    };

    public Style SetUnderlined(bool? value) => this with
    {
        Underlined = value
    };

    public Style SetStrikethrough(bool? value) => this with
    {
        Strikethrough = value
    };

    public Style Clear() => Empty;
}