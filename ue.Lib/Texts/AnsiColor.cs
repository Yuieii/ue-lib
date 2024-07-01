// Copyright (c) 2024 Yuieii.

using System.Drawing;
using ue.Components;
using ue.Values;

namespace ue.Texts;

public abstract class AnsiColor
{
    public static AnsiColor FromTextColor(TextColor color) => ClassicAnsiColor.InternalFromTextColor(color);

    public static AnsiColor CreateRgb(TextColor color) => new RgbAnsiColor(color.Color);

    public abstract string ToAnsiCode();
}

public class ClassicAnsiColor : AnsiColor
{
    public byte Color { get; }
    public bool IsBright { get; }
    
    // @formatter:off
    public static ClassicAnsiColor Black       { get; } = new(30);
    public static ClassicAnsiColor DarkBlue    { get; } = new(34);
    public static ClassicAnsiColor DarkGreen   { get; } = new(32);
    public static ClassicAnsiColor DarkAqua    { get; } = new(36);
    public static ClassicAnsiColor DarkRed     { get; } = new(31);
    public static ClassicAnsiColor DarkPurple  { get; } = new(35);
    public static ClassicAnsiColor Gold        { get; } = new(33);
    public static ClassicAnsiColor Gray        { get; } = new(37);
    public static ClassicAnsiColor DarkGray    { get; } = new(30, true);
    public static ClassicAnsiColor Blue        { get; } = new(34, true);
    public static ClassicAnsiColor Green       { get; } = new(32, true);
    public static ClassicAnsiColor Aqua        { get; } = new(36, true);
    public static ClassicAnsiColor Red         { get; } = new(31, true);
    public static ClassicAnsiColor LightPurple { get; } = new(35, true);
    public static ClassicAnsiColor Yellow      { get; } = new(33, true);
    public static ClassicAnsiColor White       { get; } = new(37, true);
    public static ClassicAnsiColor Reset       { get; } = new(0);
    // @formatter:on

    private static readonly Dictionary<TextColor, ClassicAnsiColor> _colorMappings = new();

    static ClassicAnsiColor()
    {
        // @formatter:off
        _colorMappings[TextColor.Black]       = Black;
        _colorMappings[TextColor.DarkBlue]    = DarkBlue;
        _colorMappings[TextColor.DarkGreen]   = DarkGreen;
        _colorMappings[TextColor.DarkAqua]    = DarkAqua;
        _colorMappings[TextColor.DarkRed]     = DarkRed;
        _colorMappings[TextColor.DarkPurple]  = DarkPurple;
        _colorMappings[TextColor.Gold]        = Gold;
        _colorMappings[TextColor.Gray]        = Gray;
        _colorMappings[TextColor.DarkGray]    = DarkGray;
        _colorMappings[TextColor.Blue]        = Blue;
        _colorMappings[TextColor.Green]       = Green;
        _colorMappings[TextColor.Aqua]        = Aqua;
        _colorMappings[TextColor.Red]         = Red;
        _colorMappings[TextColor.LightPurple] = LightPurple;
        _colorMappings[TextColor.Yellow]      = Yellow;
        _colorMappings[TextColor.White]       = White;
        // @formatter:on
    }

    internal static AnsiColor InternalFromTextColor(TextColor color)
    {
        var closest = color.ToNearestPredefinedColor();
        if (_colorMappings.TryGetValue(closest, out var result))
            return result;

        throw new ArgumentException($"Color of predefined color {closest} is not defined in the mapping.");
    }

    private ClassicAnsiColor(byte color, bool isBright = false)
    {
        Color = color;
        IsBright = isBright;
    }

    public override string ToAnsiCode()
    {
        var brightPrefix = IsBright ? "1;" : "0;";
        return $"\u001b[{brightPrefix}{Color}m";
    }
}

public class RgbAnsiColor : AnsiColor
{
    public Argb32 Color { get; }

    public RgbAnsiColor(Argb32 color)
    {
        Color = color;
    }

    public override string ToAnsiCode() => $"\u001b[38;2;{Color.R};{Color.G};{Color.B}m";
}