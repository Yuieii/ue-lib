using System.Text.Json.Nodes;

namespace ue.Components;

/// <summary>
/// The fallback style for every component that doesn't have its style set.
/// </summary>
public class EmptyStyle : IStyleSelf<EmptyStyle>
{
    /// <summary>
    /// The shared singleton instance that represents an empty style.
    /// </summary>
    public static EmptyStyle Instance { get; } = new();
    
    private EmptyStyle() { }
    
    public void Serialize(JsonObject data) { }

    public IStyle OverrideFrom(IStyle other) => other;

    public EmptyStyle Clear() => this;
}