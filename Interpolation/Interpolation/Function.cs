using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpolation
{
    class Function
    {
        public Function(Func<double, double, double> functional,string funcString)
        {
            this.functional = functional;
            this.funcString = funcString;
        }

        public Function(Function function)
        {
            this.functional = function.functional;
            this.funcString = function.funcString;
        }

        public string funcString;
        public Func<double, double, double> functional;
    }
}
