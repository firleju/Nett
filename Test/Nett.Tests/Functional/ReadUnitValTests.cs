using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Nett.Tests.Functional
{
    public sealed class ReadUnitValTests
    {
        [Fact]
        public void Foox()
        {
            // Arrange

            // Act
            var t = Toml.ReadString("x = 10 mm");

            // Assert
        }
    }
}
