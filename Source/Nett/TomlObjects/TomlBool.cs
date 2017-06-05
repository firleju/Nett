using Nett.Extensions;

namespace Nett
{
    public sealed class TomlBool : TomlValue<bool>
    {
        internal TomlBool(ITomlRoot root, bool value)
            : base(root, value)
        {
        }

        public override string ReadableTypeName => "bool";

        public override TomlObjectType TomlType => TomlObjectType.Bool;

        public override void Visit(ITomlObjectVisitor visitor)
        {
            visitor.Visit(this);
        }

        internal override TomlValue ValueWithRoot(ITomlRoot root) => this.BoolWithRoot(root);

        internal override TomlObject WithRoot(ITomlRoot root) => this.BoolWithRoot(root);

        internal TomlBool BoolWithRoot(ITomlRoot root)
        {
            root.CheckNotNull(nameof(root));

            return new TomlBool(root, this.Value);
        }

        internal TomlBool CloneBoolFor(TomlObject newParent)
        {
            return new TomlBool(this.Root, this.Value)
            {
                parent = newParent,
            };
        }

        internal override TomlObject CloneFor(TomlObject newParent) => this.CloneBoolFor(newParent);
    }
}
