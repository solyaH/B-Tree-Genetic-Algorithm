using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpolation
{
    class FuncNode:Node
    {
        public List<Node> children;
        public Function function;
        //public Func<double, double, double> function;

        public FuncNode(Node parent, Function function) : base(parent)
        {
            this.children = new List<Node>();
            this.function = new Function(function);
        }

        public override double SelectRandom()
        {
            throw new NotImplementedException();
        }

        public override double Evaluate(double x)
        {
            return function.functional(children[0].Evaluate(x), children[1].Evaluate(x));           
        }
    }
}
