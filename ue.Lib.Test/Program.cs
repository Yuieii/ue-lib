// Copyright (c) 2024 Yuieii.

using System.Text.RegularExpressions;
using ue.Components;

var component = ChatComponents.Formatted("Hello, %s", ChatComponents.Literal("wor")
        .AddSibling(ChatComponents.Literal("ld").OverrideStyle(Style.Empty.SetColor(TextColor.Gold))))
    .AddSibling(ChatComponents.Literal("!"));

var flattened = ComponentFlattener.Default.Flatten(new PassthroughVisitor().Visit(component))
    .ResolveComponents(new Regex("Hel"),
        (_, _) => ChatComponents.Literal("HEL", Style.Empty.SetColor(TextColor.Red).SetItalic(true)));

// Outputs: HELlo, world!
// HEL -> Red, italic
// ld  -> Gold
// Others are normal
Console.WriteLine(new AnsiTextVisitor().Visit(flattened));