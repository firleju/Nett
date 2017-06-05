using Nett.Extensions;

namespace Nett
{
    public sealed class TomlInt : TomlValue<long>
    {
        internal TomlInt(ITomlRoot root, long value)
            : base(root, value)
        {
        }

        public override string ReadableTypeName => "int";

        public override TomlObjectType TomlType => TomlObjectType.Int;

        public override void Visit(ITomlObjectVisitor visitor)
        {
            visitor.Visit(this);
        }

        internal override TomlValue ValueWithRoot(ITomlRoot root) => this.IntWithRoot(root);

        internal override TomlObject WithRoot(ITomlRoot root) => this.IntWithRoot(root);

        internal TomlInt IntWithRoot(ITomlRoot root)
        {
            root.CheckNotNull(nameof(root));

            return new TomlInt(root, this.Value);
        }

        internal TomlInt CloneIntFor(ITomlContainer newOwner)
        {
            return new TomlInt(this.Root, this.Value) { owner = newOwner };
        }

        internal override TomlObject CloneFor(ITomlContainer newOwner) => this.CloneIntFor(newOwner);
    }
}
