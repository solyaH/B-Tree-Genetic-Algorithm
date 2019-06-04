using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpolation
{
    class VariableNode:TerminalNode
    {
        //public Variable variable;
        public string variable;

        public VariableNode(Node parent, string variable) : base(parent)
        {
            this.variable = variable;
        }

        public override double Evaluate(double x)
        {
            return x;
        }
    }
}
