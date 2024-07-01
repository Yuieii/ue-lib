using System.Text.RegularExpressions;

namespace ue.Components;

public static class ChatComponents
{
    public static IMutableChatComponent Literal(string text) => Literal(text, EmptyStyle.Instance);
    
    public static IMutableChatComponent Literal(string text, IStyle style) => 
        new MutableChatComponent(new LiteralContent(text), style);

    public static IMutableChatComponent Formatted(string format, params IChatComponent[] parameters) =>
        new MutableChatComponent(new FormattedContent(format, parameters), EmptyStyle.Instance);
    
    public static IMutableChatComponent Formatted(string format, IStyle style, params IChatComponent[] parameters) => 
        new MutableChatComponent(new FormattedContent(format, parameters), style);

    public static IChatComponent ResolveComponents(this IChatComponent component, ComponentResolver resolver) =>
        resolver.Resolve(component);

    public static IChatComponent ResolveComponents(this IChatComponent component, Regex regex,
        Func<Match, IStyle, IChatComponent?> factory) =>
        new RegexComponentResolver(regex, factory).Resolve(component);
}