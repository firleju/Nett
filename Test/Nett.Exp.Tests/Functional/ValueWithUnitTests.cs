using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Xunit;

namespace Nett.Exp.Tests.Functional
{
    public sealed class ValueWithUnitTests
    {
        [Fact]
        public void ReadValuewWithUnit_WithSpecialUnitChar_ReadsItAsValidTomlValueWithUnit()
        {
            // Arrange

            // Act
            var t = Toml.ReadString("X = 11.4 ($)", CreateSettings());

            // Assert
            var obj = t.Get<TomlFloat>("X");

            obj.Unit.Should().Be("$");
            obj.Value.Should().Be(11.4);
        }

        [Fact]
        public void ReadValueWithUnit_WithSpecialUnitChar_CanBeReadAsNormalTomlValueAlso()
        {
            // Act
            var t = Toml.ReadString("X = 11.4 ($)", CreateSettings());

            // Assert
            var obj = t.Get<TomlFloat>("X");
            obj.Value.Should().Be(11.4);
        }

        [Fact]
        public void ReadValuewWithUnit_WithSimpleUnit_ReadsItAsValueWithUnit()
        {
            // Act
            var t = Toml.ReadString<Root>("X = 11.4 ( USD )", CreateSettings());

            // Assert
            t.X.Value.Should().Be(11.4);
            t.X.Currency.Should().Be("USD");
        }

        [Fact]
        public void WriteValueWithUnit_ProducesCorrectTomlFragment()
        {
            // Act
            var t = Toml.WriteString(new Root(), CreateSettings());

            // Assert
            t.Trim().Should().Be("X = 10.2 (EUR)");
        }

        [Fact]
        public void WriteValueWithUnit_WithArrayOfSuchValues_WritesCorrectArray()
        {
            // Act
            var t = Toml.WriteString(new Root2(), CreateSettings());

            // Assert
            t.Trim().Should().Be("X = [1.5 ($), -1.6 (€)]");
        }

        [Fact]
        public void ReadValueWithUnit_WithArrayOfSuchValues_WritesCorrectArray()
        {
            // Act
            var t = Toml.ReadString<Root2>("X = [1.5 ($), -1.6 (€)]", CreateSettings());

            // Assert
            t.X.Should().BeEquivalentTo(new Root2().X);
        }

        private static TomlSettings CreateSettings()
        {
            var cfg = TomlSettings.Create(s => s
                .EnableExperimentalFeatures(f => f
                    .ValuesWithUnit())
                .ConfigureType<Money>(ct => ct
                    .WithConversionFor<TomlFloat>(conv => conv
                        .ToToml(m => Tuple.Create(m.Value, m.Currency))
                        .FromToml(uv => new Money() { Currency = uv.Unit, Value = uv.Value }))));
            return cfg;
        }

        public class Root
        {
            public Money X { get; set; } = new Money();
        }

        public class Root2
        {
            public Money[] X { get; set; } = new Money[]
            {
                new Money() { Value = 1.5, Currency = "$" },
                new Money() { Value = -1.6, Currency = "€" },
            };
        }

        public class Money
        {
            public double Value { get; set; } = 10.2;

            public string Currency { get; set; } = "EUR";

            public override bool Equals(object obj)
            {
                return obj is Money m
                    ? this.Value == m.Value && this.Currency == m.Currency
                    : false;
            }
        }
    }
}
