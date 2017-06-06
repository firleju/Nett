using System;
using Nett.Extensions;

namespace Nett
{
    internal sealed class TomlTableRow : ITomlContainer
    {
        public TomlTableRow(ITomlContainer parent, TomlKey key, TomlObject value)
        {
            this.Parent = parent.CheckNotNull(nameof(parent));

            this.Key = (TomlKey)key.AttachToOrCloneFor(this);
            this.Value = value.AttachToOrCloneFor(key);
        }

        public TomlKey Key { get; }

        public TomlObject Value { get; }

        public ITomlRoot Root => this.Key.Root;

        public ITomlContainer Parent { get; }
    }
}
