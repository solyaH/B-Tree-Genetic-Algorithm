using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpolation
{
    abstract class Node
    {
        public Node parent;

        public Node(Node parent)
        {
            this.parent = parent;
        }

        

        public abstract double Evaluate(double x);
        public abstract double SelectRandom();
    }
}
