// Copyright (c) 2024 Yuieii.

using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using ue.Values;

namespace ue.Components;

public class TextColor
{
    private readonly int _ordinal;
    private static int _count;

    private static readonly Dictionary<string, TextColor> _byName = new();

    // @formatter:off
    public static TextColor Black       { get; } = new("black",        Argb32.FromRgb(0));
    public static TextColor DarkBlue    { get; } = new("dark_blue",    Argb32.FromRgb(0xaa));
    public static TextColor DarkGreen   { get; } = new("dark_green",   Argb32.FromRgb(0xaa00));
    public static TextColor DarkAqua    { get; } = new("dark_aqua",    Argb32.FromRgb(0xaaaa));
    public static TextColor DarkRed     { get; } = new("dark_red",     Argb32.FromRgb(0xaa0000));
    public static TextColor DarkPurple  { get; } = new("dark_purple",  Argb32.FromRgb(0xaa00aa));
    public static TextColor Gold        { get; } = new("gold",         Argb32.FromRgb(0xffaa00));
    public static TextColor Gray        { get; } = new("gray",         Argb32.FromRgb(0xaaaaaa));
    public static TextColor DarkGray    { get; } = new("dark_gray",    Argb32.FromRgb(0x555555));
    public static TextColor Blue        { get; } = new("blue",         Argb32.FromRgb(0x5555ff));
    public static TextColor Green       { get; } = new("green",        Argb32.FromRgb(0x55ff55));
    public static TextColor Aqua        { get; } = new("aqua",         Argb32.FromRgb(0x55ffff));
    public static TextColor Red         { get; } = new("red",          Argb32.FromRgb(0xff5555));
    public static TextColor LightPurple { get; } = new("light_purple", Argb32.FromRgb(0xff55ff));
    public static TextColor Yellow      { get; } = new("yellow",       Argb32.FromRgb(0xffff55));
    public static TextColor White       { get; } = new("white",        Argb32.FromRgb(0xffffff));
    // @formatter:on

    private TextColor(string name, Argb32 color)
    {
        _ordinal = _count++;
        Name = name;
        Color = color;

        _byName[name] = this;
    }

    private TextColor(string name, Argb32 color, CustomRgbColor _)
    {
        Name = name;
        Color = color;
    }

    public string Name { get; }

    public Argb32 Color { get; }

    private static readonly TextColor[] _predefined =
    [
        Black, DarkBlue, DarkGreen, DarkAqua, DarkRed, DarkPurple, Gold, Gray,
        DarkGray, Blue, Green, Aqua, Red, LightPurple, Yellow, White
    ];

    public static IReadOnlyList<TextColor> PredefinedColors => _predefined;

    public static TextColor Create(Color color)
    {
        Argb32 argb = color.ToArgb();
        return Create($"#{argb.Rgb:x6}");
    }

    public static TextColor Create(string name)
    {
        if (name.StartsWith("#") && name.Length == 7)
        {
            if (!int.TryParse(name[1..], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var rgb))
                throw new ArgumentException($"Illegal hex string: {name}");

            return new TextColor(name, Argb32.FromRgb(rgb), CustomRgbColor.Unit);
        }

        if (_byName.TryGetValue(name, out var defined))
            return defined;

        throw new ArgumentException($"Could not parse TextColor: {name}");
    }

    public TextColor ToNearestPredefinedColor()
    {
        if (Name[0] != '#')
            return this;

        var closest = default(TextColor);
        var cl = Color;
        var smallestDiff = 0;

        foreach (var tc in _predefined)
        {
            var rAverage = (tc.Color.R + cl.R) / 2;
            var rDiff = tc.Color.R - cl.R;
            var gDiff = tc.Color.G - cl.G;
            var bDiff = tc.Color.B - cl.B;

            var diff = ((2 + (rAverage >> 8)) * rDiff * rDiff) +
                       (4 * gDiff * gDiff) +
                       ((2 + ((255 - rAverage) >> 8)) * bDiff * bDiff);

            if (closest == null || diff < smallestDiff)
            {
                closest = tc;
                smallestDiff = diff;
            }
        }

        if (closest == null)
            throw new Exception("No predefined colors!");

        return closest;
    }

    private struct CustomRgbColor
    {
        public static CustomRgbColor Unit { get; } = new();
    }
}