using System.Collections.Generic;

namespace Nett.Parser.Nodes
{
    internal sealed class UnitValueNode : Node
    {
        public UnitValueNode(IReq<Node> value, Token unit)
        {
            this.Value = value;
            this.Unit = new TerminalNode(unit).Req();
        }

        public IReq<Node> Value { get; }

        public IReq<Node> Unit { get; }

        public override IEnumerable<Node> Children
            => NodesAsEnumerable(this.Value, this.Unit);
    }
}
