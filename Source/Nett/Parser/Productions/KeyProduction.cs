namespace Nett.Parser.Productions
{
    internal static class KeyProduction
    {
        public static TomlKey Apply(ITomlRoot root, TokenBuffer tokens) => ApplyInternal(root, tokens, required: true);

        public static TomlKey TryApply(ITomlRoot root, TokenBuffer tokens) => ApplyInternal(root, tokens, required: false);

        private static TomlKey ApplyInternal(ITomlRoot root, TokenBuffer tokens, bool required)
        {
            if (tokens.TryExpect(TokenType.BareKey) || tokens.TryExpect(TokenType.Integer))
            {
                return new TomlKey(root, tokens.Consume().value, TomlKey.KeyType.Bare);
            }
            else if (tokens.TryExpect(TokenType.String))
            {
                return new TomlKey(root, tokens.Consume().value, TomlKey.KeyType.Basic);
            }
            else if (tokens.TryExpect(TokenType.LiteralString))
            {
                return new TomlKey(root, tokens.Consume().value, TomlKey.KeyType.Literal);
            }
            else if (required)
            {
                var t = tokens.Peek();
                if (t.value == "=")
                {
                    throw Parser.CreateParseError(t, "Key is missing.");
                }
                else
                {
                    throw Parser.CreateParseError(t, $"Failed to parse key because unexpected token '{t.value}' was found.");
                }
            }
            else
            {
                return new TomlKey(root, string.Empty);
            }
        }
    }
}
