// Copyright (c) 2024 Yuieii.

using System.Drawing;
using System.Runtime.CompilerServices;

namespace ue.Values;

public readonly struct Argb32
{
    /// <summary>
    /// The blue channel of the color.
    /// </summary>
    public readonly byte B;

    /// <summary>
    /// The green channel of the color.
    /// </summary>
    public readonly byte G;

    /// <summary>
    /// The red channel of the color.
    /// </summary>
    public readonly byte R;

    /// <summary>
    /// The alpha channel of the color.
    /// </summary>
    public readonly byte A;

    /// <summary>
    /// The raw ARGB value of the color.
    /// </summary>
    public uint RawValue
    {
        get
        {
            var copy = this;
            return Unsafe.As<Argb32, uint>(ref copy);
        }
    }

    /// <summary>
    /// The raw RGB value of the color, without the alpha channel.
    /// </summary>
    public int Rgb => (int)(RawValue & 0xffffff);

    public static implicit operator Argb32(uint hex) => new(hex);
    public static implicit operator Argb32(int hex) => new(hex);

    public Argb32(uint hex)
    {
        var copy = hex;
        var source = Unsafe.As<uint, Argb32>(ref copy);
        B = source.B;
        R = source.R;
        G = source.G;
        A = source.A;
    }

    public Argb32(int hex)
    {
        var copy = hex;
        var source = Unsafe.As<int, Argb32>(ref copy);
        B = source.B;
        R = source.R;
        G = source.G;
        A = source.A;
    }

    public Argb32(byte a, byte r, byte g, byte b)
    {
        A = a;
        R = r;
        G = g;
        B = b;
    }

    public static Argb32 FromRgb(int hex)
    {
        var copy = hex;
        var uHex = Unsafe.As<int, uint>(ref copy);
        var rawValue = uHex | 0xff000000;
        return new Argb32(rawValue);
    }

    public Color ToColor()
    {
        var hex = RawValue;
        var argb = Unsafe.As<uint, int>(ref hex);
        return Color.FromArgb(argb);
    }
}