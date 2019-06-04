using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpolation
{
    class ConstNode:TerminalNode
    {
        public double constant;

        public ConstNode(Node parent,double constant):base(parent)
        {
            this.constant = constant;
        }

        public override double Evaluate(double x)
        {
            return constant;
        }
    }
}
