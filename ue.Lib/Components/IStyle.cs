using System.Text.Json.Nodes;

namespace ue.Components;

public interface IStyle
{
    /// <summary>
    /// Serialize the style into the JSON object representing its containing component.
    /// </summary>
    /// <param name="data">The target JSON object where the serialized data will be inserted.</param>
    public void Serialize(JsonObject data);
    
    /// <summary>
    /// Creates an equivalent style that overrides this style from other style.
    /// </summary>
    /// <param name="other">The other style that will override this style.</param>
    /// <returns>A newly created style.</returns>
    public IStyle OverrideFrom(IStyle other);
    
    /// <summary>
    /// Returns a cleared style of this type. 
    /// </summary>
    /// <returns>A cleared style of this type.</returns>
    public IStyle Clear();
}

/// <summary>
/// Flags the self type of the style.
/// </summary>
/// <typeparam name="T">The type of the style.</typeparam>
public interface IStyleSelf<out T> : IStyle where T : IStyleSelf<T>
{
    /// <inheritdoc cref="IStyle.Clear"/>
    public new T Clear();
    IStyle IStyle.Clear() => Clear();
}

/// <summary>
/// Indicates that the specific style applies color to the content.
/// </summary>
public interface ITextColorStyle : IStyle
{
    /// <summary>
    /// The color of the content.
    /// </summary>
    public TextColor? Color { get; }
}

/// <summary>
/// Indicates that the specific style applied background color to the content.
/// </summary>
public interface IBackColorStyle : IStyle
{
    /// <summary>
    /// The background color of the content.
    /// </summary>
    public TextColor? BackColor { get; }
}

/// <summary>
/// Indicates that the specific style is applicable to be applied to the terminal display.
/// </summary>
public interface ITerminalTextStyle : ITextColorStyle, IBackColorStyle
{
    /// <summary>
    /// Whether the text should be bold.
    /// </summary>
    public bool? Bold { get; }
    
    /// <summary>
    /// Whether the text should be italic.
    /// </summary>
    public bool? Italic { get; }
    
    /// <summary>
    /// Whether the text should be underlined.
    /// </summary>
    public bool? Underlined { get; }
    
    /// <summary>
    /// Whether the text should be a strikethrough text.
    /// </summary>
    public bool? Strikethrough { get; }
}