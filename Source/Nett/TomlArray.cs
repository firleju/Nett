namespace Nett
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Extensions;

    public sealed class TomlArray : TomlValue<List<TomlValue>>
    {
        internal TomlArray(ITomlRoot root)
            : this(root, new List<TomlValue>())
        {
        }

        internal TomlArray(ITomlRoot root, params TomlValue[] values)
            : this(root, new List<TomlValue>(values))
        {
        }

        internal TomlArray(ITomlRoot root, List<TomlValue> values)
            : base(root, values)
        {
        }

        public int Length => this.Value.Count;

        public override string ReadableTypeName => "array";

        public override TomlObjectType TomlType => TomlObjectType.Array;

        public TomlValue this[int index]
        {
            get { return this.Value[index]; }
            set { this.Value[index] = value; }
        }

        public T Get<T>(int index) => this.Value[index].Get<T>();

        public override object Get(Type t)
        {
            if (t == Types.TomlArrayType) { return this; }

            if (t.IsArray)
            {
                var et = t.GetElementType();
                var a = Array.CreateInstance(et, this.Value.Count);
                int cnt = 0;
                foreach (var i in this.Value)
                {
                    a.SetValue(i.Get(et), cnt++);
                }

                return a;
            }

            if (!Types.ListType.IsAssignableFrom(t))
            {
                throw new InvalidOperationException(string.Format("Cannot convert TOML array to '{0}'.", t.FullName));
            }

            var collection = (IList)Activator.CreateInstance(t);
            Type itemType = Types.ObjectType;
            if (t.IsGenericType)
            {
                itemType = t.GetGenericArguments()[0];
            }

            foreach (var i in this.Value)
            {
                collection.Add(i.Get(itemType));
            }

            return collection;
        }

        public TomlObject Last() => this.Value[this.Value.Count - 1];

        public IEnumerable<T> To<T>() => this.To<T>(TomlConfig.DefaultInstance);

        public IEnumerable<T> To<T>(TomlConfig config) => this.Value.Select((to) => to.Get<T>());

        public override void Visit(ITomlObjectVisitor visitor)
        {
            visitor.Visit(this);
        }

        internal override TomlObject WithRoot(ITomlRoot root) => this.ArrayWithRoot(root);

        internal override TomlValue ValueWithRoot(ITomlRoot root) => this.ArrayWithRoot(root);

        internal TomlArray ArrayWithRoot(ITomlRoot root)
        {
            root.CheckNotNull(nameof(root));

            return new TomlArray(root, this.Value.Select(i => i.ValueWithRoot(root)).ToArray());
        }
    }
}
