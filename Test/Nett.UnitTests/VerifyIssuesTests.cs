﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Nett.UnitTests.Util;
using Xunit;

namespace Nett.UnitTests
{
    [ExcludeFromCodeCoverage]
    public sealed class VerifyIssuesTests
    {
        public class RootTable
        {
            public SubTable SubTable { get; set; } = new SubTable();

            public override string ToString()
            {
                return $"RootTable({SubTable})";
            }
        }

        public class SubTable
        {
            public class ListTable
            {
                public int SomeValue { get; set; } = 5;

                public override string ToString()
                {
                    return $"ListTable({SomeValue})";
                }
            }

            public List<ListTable> Values { get; set; } = new List<ListTable>();

            public override string ToString()
            {
                return $"SubTable({string.Join(",", Values.Select(p => p.ToString()))})";
            }
        }

        [Fact(DisplayName = "Verify issue #14 was fixed: Array of tables serialization forgot parent key")]
        public void WriteWithArrayOfTables_ProducesCorrectToml()
        {
            // Arrange
            var root = new RootTable();
            root.SubTable.Values.AddRange(new[]
            {
                new SubTable.ListTable() { SomeValue = 1 }, new SubTable.ListTable() { SomeValue = 5 }
            });
            const string expected = @"
[SubTable]

[[SubTable.Values]]
SomeValue = 1
[[SubTable.Values]]
SomeValue = 5";

            // Act
            var tml = Toml.WriteString(root);

            // Assert
            tml.ShouldBeSemanticallyEquivalentTo(expected);
        }

        [Fact(DisplayName = "Verify that issue #8 was fixed")]
        public void ReadAndWriteFloat_Issue8_IsFixed()
        {
            // Arrange
            MyObject obj = new MyObject();
            obj.MyFloat = 123;
            string output = Toml.WriteString<MyObject>(obj);

            // Act
            MyObject parsed = Toml.ReadString<MyObject>(output);

            // Assert
            parsed.MyFloat.Should().Be(123.0f);
        }

        public class MyObject
        {
            public float MyFloat { get; set; }
        }

    }
}
