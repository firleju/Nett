using System;

namespace Nett
{
    internal sealed class TomlKey : TomlObject, IEquatable<TomlKey>
    {
        public readonly string Value;
        public KeyType Type;

        public TomlKey(ITomlRoot root, string s)
            : this(root, s, AutoClassify(s))
        {
        }

        public TomlKey(ITomlRoot root, string key, KeyType type)
            : base(root)
        {
            this.Value = key;
            this.Type = type;
        }

        public enum KeyType
        {
            Undefined,
            Bare,
            Basic,
            Literal,
        }

        public override string ReadableTypeName => "key";

        public override TomlObjectType TomlType => TomlObjectType.Key;

        public static bool operator ==(TomlKey x, TomlKey y) => x.Value == y.Value;

        public static bool operator !=(TomlKey x, TomlKey y) => !(x == y);

        public bool Equals(TomlKey other) => this == other;

        public override bool Equals(object obj) => obj is TomlKey && this == (TomlKey)obj;

        public override int GetHashCode() => this.Value.GetHashCode();

        public override string ToString()
        {
            var type = this.Type == KeyType.Undefined
                ? AutoClassify(this.Value)
                : this.Type;

            switch (type)
            {
                case KeyType.Bare: return this.Value;
                case KeyType.Basic: return "\"" + this.Value.Escape() + "\"";
                case KeyType.Literal: return "'" + this.Value + "'";
                default: return this.Value;
            }
        }

        public override object Get(Type t) => throw new NotSupportedException();

        public override void Visit(ITomlObjectVisitor visitor) => throw new NotSupportedException();

        internal override TomlObject WithRoot(ITomlRoot root) => new TomlKey(root, this.Value, this.Type);

        private static KeyType AutoClassify(string input)
        {
            if (input.HasBareKeyCharsOnly()) { return KeyType.Bare; }
            else if (input.IndexOf("'") >= 0) { return KeyType.Basic; }
            else { return KeyType.Literal; }
        }
    }
}
