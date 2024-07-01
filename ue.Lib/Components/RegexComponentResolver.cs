using System.Text.RegularExpressions;

namespace ue.Components;

public class RegexComponentResolver : ComponentResolver
{
    private readonly Regex _regex;
    private readonly Func<Match, IStyle, IChatComponent?> _factory;

    public RegexComponentResolver(Regex regex, Func<Match, IStyle, IChatComponent?> factory)
    {
        _regex = regex;
        _factory = factory;
    }

    public override IReadOnlyList<IResolvedComponentPart> GetResolvedParts(string content)
    {
        return _regex.Matches(content)
            .Select(m => new RegexResolvedComponentPart(m, _factory))
            .ToList(); // .OfType<IResolvedComponentPart>().ToList();
    }

    private class RegexResolvedComponentPart : IResolvedComponentPart
    {
        private readonly Match _match;
        private readonly Func<Match, IStyle, IChatComponent?> _factory;

        public Range Range => new(_match.Index, _match.Index + _match.Length);
        
        public RegexResolvedComponentPart(Match match, Func<Match, IStyle, IChatComponent?> factory)
        {
            _match = match;
            _factory = factory;
        }

        public IChatComponent? Resolve(IStyle style) => _factory(_match, style);
    }
}