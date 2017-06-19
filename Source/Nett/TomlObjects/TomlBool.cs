using Nett.Extensions;

namespace Nett
{
    public sealed class TomlBool : TomlValue<bool>
    {
        internal TomlBool(bool value)
            : base(null, value)
        {
        }

        public override string ReadableTypeName => "bool";

        public override TomlObjectType TomlType => TomlObjectType.Bool;

        internal override ITomlRoot Root => this.owner.Root;

        public override void Visit(ITomlObjectVisitor visitor)
        {
            visitor.Visit(this);
        }

        internal override TomlValue ValueWithRoot(ITomlRoot root) => this.BoolWithRoot(root);

        internal override TomlObject WithRoot(ITomlRoot root) => this.BoolWithRoot(root);

        internal TomlBool BoolWithRoot(ITomlRoot root)
        {
            root.CheckNotNull(nameof(root));

            return new TomlBool(this.Value);
        }

        internal TomlBool CloneBoolFor(ITomlContainer newOwner)
        {
            return new TomlBool(this.Value) { owner = newOwner };
        }

        internal override TomlObject CloneFor(ITomlContainer newParent) => this.CloneBoolFor(newParent);
    }
}
