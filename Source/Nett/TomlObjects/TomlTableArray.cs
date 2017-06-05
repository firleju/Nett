namespace Nett
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Extensions;

    public sealed class TomlTableArray : TomlObject, IEnumerable<TomlTable>, ICollection<TomlTable>, IList<TomlTable>
    {
        private static readonly Type ListType = typeof(IList);
        private static readonly Type ObjectType = typeof(object);
        private static readonly Type TableArrayType = typeof(TomlTableArray);

        private readonly List<TomlTable> items = new List<TomlTable>();

        public TomlTableArray(ITomlRoot root)
            : this(root, new List<TomlTable>())
        {
        }

        public TomlTableArray(ITomlRoot root, IEnumerable<TomlTable> enumerable)
            : this(root, new List<TomlTable>(enumerable))
        {
        }

        public TomlTableArray(ITomlRoot root, List<TomlTable> tables)
            : base(root)
        {
            foreach (var t in tables) { this.CheckTableCanBeAttached(t); }

            this.items = tables;
        }

        public int Count => this.items.Count;

        public List<TomlTable> Items => this.items;

        public override string ReadableTypeName => "array of tables";

        public override TomlObjectType TomlType => TomlObjectType.ArrayOfTables;

        public bool IsReadOnly => false;

        public TomlTable this[int index]
        {
            get => this.items[index];
            set
            {
                this.CheckTableCanBeAttached(value);
                this.items[index] = value;
            }
        }

        public void Add(TomlTable table)
        {
            this.CheckTableCanBeAttached(table);
            this.items.Add(table);
        }

        public void AddRange(IEnumerable<TomlTable> tables)
        {
            foreach (var t in tables) { this.CheckTableCanBeAttached(t); }
            this.items.AddRange(tables);
        }

        public override object Get(Type t)
        {
            if (t == TableArrayType) { return this; }

            if (t.IsArray)
            {
                var et = t.GetElementType();
                var a = Array.CreateInstance(et, this.items.Count);
                int cnt = 0;
                foreach (var i in this.items)
                {
                    a.SetValue(i.Get(et), cnt++);
                }

                return a;
            }

            if (!ListType.IsAssignableFrom(t))
            {
                throw new InvalidOperationException(string.Format("Cannot convert TOML array to '{0}'.", t.FullName));
            }

            var collection = (IList)Activator.CreateInstance(t);
            Type itemType = ObjectType;
            if (t.IsGenericType)
            {
                itemType = t.GetGenericArguments()[0];
            }

            foreach (var i in this.items)
            {
                collection.Add(i.Get(itemType));
            }

            return collection;
        }

        public TomlTable Last() => this.items[this.items.Count - 1];

        public override void Visit(ITomlObjectVisitor visitor)
        {
            visitor.Visit(this);
        }

        public IEnumerator<TomlTable> GetEnumerator() => this.items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.items.GetEnumerator();

        public int IndexOf(TomlTable item) => this.items.IndexOf(item);

        public void Insert(int index, TomlTable item) => this.items.Insert(index, item);

        public void RemoveAt(int index) => this.items.RemoveAt(index);

        public void Clear() => this.items.Clear();

        public bool Contains(TomlTable item) => this.items.Contains(item);

        public void CopyTo(TomlTable[] array, int arrayIndex) => this.items.CopyTo(array, arrayIndex);

        public bool Remove(TomlTable item) => this.items.Remove(item);

        internal override TomlObject WithRoot(ITomlRoot root)
        {
            throw new NotImplementedException();
        }

        internal TomlTableArray TableArrayWithRoot(ITomlRoot root)
        {
            root.CheckNotNull(nameof(root));

            var a = new TomlTableArray(root, this.Items.Select(t => t.TableWithRoot(root)));
            return a;
        }

        internal TomlTableArray CloneArrayFor(TomlObject newParent)
        {
            var ta = new TomlTableArray(this.Root)
            {
                parent = newParent,
            };

            foreach (var t in this.items)
            {
                ta.Add(t.CloneTableFor(ta));
            }

            return ta;
        }

        internal override TomlObject CloneFor(TomlObject newParent)
        {
            throw new NotImplementedException();
        }

        private void CheckTableCanBeAttached(TomlTable table)
        {
            if (this.Root != table.Root)
            {
                throw new InvalidOperationException(
                    "Cannot add table to table array because it already belongs to a different TOML object graph.");
            }
        }
    }
}
