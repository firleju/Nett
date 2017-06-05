namespace Nett.Parser.Productions
{
    using System.Collections.Generic;

    internal static class TableKeyProduction
    {
        public static IList<TomlKey> Apply(ITomlRoot root, TokenBuffer tokens)
        {
            List<TomlKey> keyChain = new List<TomlKey>();
            var key = KeyProduction.Apply(root, tokens);
            keyChain.Add(key);

            while (tokens.TryExpect(TokenType.Dot))
            {
                tokens.Consume();
                keyChain.Add(KeyProduction.TryApply(root, tokens));
            }

            return keyChain;
        }
    }
}
