using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpolation
{
    abstract class TerminalNode: Node
    {
        public TerminalNode(Node parent):base(parent)
        {
        }

        public override double SelectRandom()
        {
            throw new NotImplementedException();
        }
    }
}
