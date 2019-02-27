using System;

namespace Nett
{
    public class TomlValueWithUnit : TomlValue
    {
        public TomlValueWithUnit(string value, string unit)
            : base(null, null)
        {
            this.Value = value;
            this.Unit = unit;
        }

        public string Value { get; }

        public string Unit { get; }

        public override string ReadableTypeName => throw new NotImplementedException();

        public override TomlObjectType TomlType => throw new NotImplementedException();

        public override object Get(Type t)
        {
            throw new NotImplementedException();
        }

        public override void Visit(ITomlObjectVisitor visitor)
        {
            throw new NotImplementedException();
        }

        internal override TomlObject CloneFor(ITomlRoot root)
        {
            throw new NotImplementedException();
        }

        internal override TomlValue ValueWithRoot(ITomlRoot root)
        {
            throw new NotImplementedException();
        }

        internal override TomlObject WithRoot(ITomlRoot root)
        {
            throw new NotImplementedException();
        }
    }
}
