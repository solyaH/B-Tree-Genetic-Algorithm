using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpolation
{
    class SymbolRegression
    {
        Random rand;
        List<string> variables;
        List<double> constants;
        List<Function> functions;
        List<double> xIn;
        List<double> yIn;
        int minDepth, maxDepth;
        int terminalCount;
        int functionalCount;
        int range = 2;
        int size = 50;
        double Pm;
        double Pc;
        double Psi;
        int k = 8;
        //int maxIteration = 10;
        public double absMinFunction;
        public List<Node> population;
        //int minIteration;
        public List<double> minFuncEachIteration;
        public Node minArgument;
        double eps = 0.01;
        //Node root;


        public SymbolRegression(List<double> xIn, List<double> yIn,
            List<string> variables, List<double> constants, List<Function> functions,
            int minDepth, int maxDepth, double Pm, double Pc, double Psi)
        {
            this.xIn = new List<double>(xIn);
            this.yIn = new List<double>(yIn);
            this.variables = new List<string>(variables);
            this.constants = new List<double>(constants);
            this.functions = new List<Function>(functions);
            this.minDepth = minDepth;
            this.maxDepth = maxDepth;
            this.Pm = Pm;
            this.Pc = Pc;
            this.Psi = Psi;
            this.rand = new Random();
            this.minFuncEachIteration = new List<double>();
            terminalCount = TerminalCount();
            functionalCount = FunctionalCount();
        }

        public void Interpolate()
        {

            //Node nt = GenerateFullTree();
            //string text1 = PrintTree(nt, "", "");
            //int index = 1;
            //Node result = RAlgorithm(nt, null, ref index);
            //text1 += "\n\n" + PrintTree(result, "", ""); 

            //Node no = new FuncNode(null, functions[1]);
            //(no as FuncNode).children.Add(new ConstNode(no, 2));
            //(no as FuncNode).children.Add(new FuncNode(no, functions[0]));
            //Node ch = (no as FuncNode).children[1];
            //(ch as FuncNode).children.Add(new ConstNode(ch, 1));
            //(ch as FuncNode).children.Add(new VariableNode(ch, variables[0]));

            //Node nM = Mutation(no);
            //text1 += "\n\n" + PrintTree(no, "", "");
            //text1 += "\n\n" + PrintTree(nM, "", "");

            Node no = GenerateFullTree();
            Node no2 = GenerateFullTree();
            string text2 = PrintTree(no, "", "");
            text2 += "\n\n" + PrintTree(no2, "", "");
            List<Node> cr = Crossover(no, no2);
            //text2 += "\n\n" + PrintTree(no, "", "");
            //text2 += "\n\n" + PrintTree(no2, "", "");
            foreach (Node individual in cr)
            {
                text2 += "\n\n" + PrintTree(individual, "", "");
            }

            //var a = 1;

            //    //double e1 = individual.Evaluate(3);


            population = GeneratePopulation();
            int iteration = 0;
            string tew = "";
            foreach (Node individual in population)
            {
                //Node ne = Mutation(individual);

                tew+= PrintTree(individual, "", "")+"\n\n";

                //double e1 = individual.Evaluate(3);

            }
            //foreach (Node individual in population)
            //{
            //    //Node ne = Mutation(individual);
            //    tew += PrintTree(individual, "", "") + "\n";
            //    //double e1 = individual.Evaluate(3);

            //}

            //var l = 1;


            absMinFunction = int.MaxValue;
            int counter = 0;
            do
            {
                List<double> calculatedFunction = population.Select(x => EvaluateIndividual(x)).ToList();
                double newIterMinFunction = calculatedFunction.Min();
                minFuncEachIteration.Add(newIterMinFunction);//population.Select(ind => FitnessFunctionMulti(Converter.ConvertBitToMultiIntervals(intervalStarts, intervalEnds, maxLengthArr, ind))).Max();
                if (newIterMinFunction >= absMinFunction)
                {
                    counter++;
                }
                else
                {
                    counter = 0;
                    absMinFunction = newIterMinFunction;
                    var indexOfMin = calculatedFunction.IndexOf(newIterMinFunction);
                    minArgument = population[indexOfMin];
                    //maxFunction = FitnessFunctionMulti(maxArgument);
                }
                List<Node> newPopulation = new List<Node>();
                newPopulation.AddRange(EliteAlgo());
                do
                {
                    List<Node> newIndividuals = Crossover(Select(), Select());
                    newIndividuals[0] = Mutation(newIndividuals[0]);
                    newIndividuals[1] = Mutation(newIndividuals[1]);
                    newPopulation.AddRange(newIndividuals);
                }
                while (newPopulation.Count < population.Count);
                population = new List<Node>(newPopulation);
                iteration++;
            }
            while (absMinFunction>eps);// counter <= maxIteration);


        }

        List<Node> GeneratePopulation()
        {
            List<Node> population = new List<Node>();
            for (int i = 0; i < size; i++)
            {
                population.Add(GenerateFullTree());
            }

            return population;
        }

        Node GenerateFullTree()
        {
            int depth = rand.Next(minDepth, maxDepth + 1);
            int ltc = depth ;

            int index = rand.Next(functions.Count);
            Node root = new FuncNode(null, functions[index]);

            GenerateTree(root, ltc);

            return root;
        }

        void GenerateTree(Node parent, int ltc)
        {
            if (ltc == 0)
            {
                for (int i = 0; i < range; i++)
                {
                    Node child = SelectTerminal();
                    child.parent = parent;
                    (parent as FuncNode).children.Add(child);
                }
                return;
            }
            else
            {
                if (ltc >= minDepth)
                {
                    for (int i = 0; i < range; i++)
                    {
                        Node child = SelectFunctional();
                        child.parent = parent;
                        (parent as FuncNode).children.Add(child);
                        GenerateTree(child, ltc - 1);
                    }
                }
                else
                {
                    for (int i = 0; i < range; i++)
                    {
                        double r = rand.NextDouble();
                        if (r <= 0.5)
                        {

                            Node child = SelectFunctional();
                            child.parent = parent;
                            (parent as FuncNode).children.Add(child);
                            GenerateTree(child, ltc - 1);

                        }
                        else
                        {
                            int k = terminalCount + functionalCount;
                            int ind = rand.Next(k);
                            if (ind < functionalCount)
                            {
                                Node child = SelectFunctional();
                                child.parent = parent;
                                (parent as FuncNode).children.Add(child);
                                GenerateTree(child, ltc - 1);

                            }
                            else
                            {
                                Node child = SelectTerminal();
                                child.parent = parent;
                                (parent as FuncNode).children.Add(child);
                                //return;
                            }
                        }
                    }
                }
            }
        }

        int TerminalCount()
        {
            return variables.Count + constants.Count;
        }

        int FunctionalCount()
        {
            return functions.Count;
        }

        Node SelectTerminal()
        {
            int r = rand.Next(terminalCount);
            if (r < variables.Count)
            {
                return new VariableNode(null, variables[r]);
            }
            r = r - variables.Count;
            return new ConstNode(null, constants[r]);
        }

        Node SelectFunctional()
        {
            int r = rand.Next(functionalCount);
            return new FuncNode(null, functions[r]);
        }

        double EvaluateIndividual(Node individual)
        {
            double sum = 0;
            for (int i = 0; i < xIn.Count; i++)
            {
                sum += Math.Abs(individual.Evaluate(xIn[i]) - yIn[i]);
            }
            return sum;
        }

        Node TournamentAlgo()
        {
            List<Node> sample = population.OrderBy(x => rand.Next()).Take(k).ToList();
            string t1 = "";
            string t2 = "";
            //for(int i=0;i< sample.Count;i++)
            //{
            //    t1+= PrintTree(population[i], "", "")+"\n\n";
            //    t2+= PrintTree(sample[i], "", "") + "\n\n";
            //}
            double min = int.MaxValue;
            Node theBest = null;
            foreach (Node node in sample)
            {
                var eval = EvaluateIndividual(node);
                if (eval < min)
                {
                    theBest = node;
                    min = eval;
                }
            }
            return theBest;
        }

        List<Node> EliteAlgo()
        {
            int numberOfSuper = (int)(population.Count * Psi);

            return population.OrderBy(x => EvaluateIndividual(x)).Take(numberOfSuper).ToList();
        }

        Node Select()
        {
            return TournamentAlgo();
        }

        Node Mutation(Node individual)
        {
            Node newIndividual = CopyNode(individual);
            double r = rand.NextDouble();
            if (r < Pm)
            {
                string text = PrintTree(newIndividual, "", "");
                int index = 1;
                Node result = RAlgorithm(newIndividual, null, ref index);
                if (result == null) { result = (newIndividual as FuncNode).children[rand.Next(0, 2)]; }
                string text12 = PrintTree(result, "", "");

                for (int i = 0; i < range; i++)
                {
                    if ((result.parent as FuncNode).children[i] == result)
                    {
                        Node tree = GenerateFullTree();
                        tree.parent = result.parent;
                        (result.parent as FuncNode).children[i] = tree;
                            //new ConstNode(result.parent, 1000)
                    }
                }
                string text1 = PrintTree(result, "", "");
                string text2 = PrintTree(newIndividual, "", "");
            }
            return newIndividual;
        }

        List<Node> Crossover(Node individual1, Node individual2)
        {
            Node newIndividual1 = CopyNode(individual1);
            Node newIndividual2 = CopyNode(individual2);
            double r = rand.NextDouble();
            if (r < Pc)
            {
                string text1 = PrintTree(newIndividual1, "", "");
                string text2 = PrintTree(newIndividual2, "", "");
                int index = 1;
                Node result1 = RAlgorithm(newIndividual1, null, ref index);
                if (result1 == null) { result1 = (newIndividual1 as FuncNode).children[rand.Next(0, 2)]; }
                index = 1;
                Node result2 = RAlgorithm(newIndividual2, null, ref index);
                if (result2 == null) { result2 = (newIndividual2 as FuncNode).children[rand.Next(0, 2)]; }
                string text12 = PrintTree(result1, "", "");
                string text21 = PrintTree(result2, "", "");

                for (int i = 0; i < range; i++)
                {
                    if ((result1.parent as FuncNode).children[i] == result1)
                    {
                        //Node result1New = new Node(result2, result1.parent);
                        //(result1.parent as FuncNode).children[i] = result1New;
                        (result1.parent as FuncNode).children[i] = result2;
                    }
                    if ((result2.parent as FuncNode).children[i] == result2)
                    {
                        //Node result2New = new Node(result1, result2.parent);
                        //(result2.parent as FuncNode).children[i] = result2New;
                        (result2.parent as FuncNode).children[i] = result1;
                    }
                }
                var temp = result1.parent;
                result1.parent = result2.parent;
                result2.parent = temp;

                string text11 = PrintTree(individual1, "", "");
                string text22 = PrintTree(individual2, "", "");
            }
            return new List<Node>() { newIndividual1, newIndividual2};
        }

        Node RAlgorithm(Node root, Node result, ref int index)
        {
            string text = PrintTree(root, "", "");
            //index++;
            double r = rand.NextDouble();
            double a = (1.0 / index);
            if (index != 1)
            {
                if (r <= a)
                {
                    result = root;
                }
            }

            if (root is TerminalNode)
            {
                return result;
            }

            if (root is FuncNode)
            {
                foreach (Node child in (root as FuncNode).children)
                {
                    index++;
                    result = RAlgorithm(child, result, ref index);
                }
            }
            return result;
        }

        public string PrintTree(Node tree, string indent, string text)
        {
            string str = "";
            if (tree is FuncNode)
            {
                str = (tree as FuncNode).function.funcString;
            }
            else if (tree is ConstNode)
            {
                str = (tree as ConstNode).constant.ToString();
            }
            else if (tree is VariableNode)
            {
                str = (tree as VariableNode).variable;
            }
            text += indent + "(" + str + ")\n";
            indent += "|  ";

            if (tree is TerminalNode)
            {
                return text;
            }


            if (tree is FuncNode)
            {
                for (int i = 0; i < (tree as FuncNode).children.Count; i++)
                {
                    text = PrintTree((tree as FuncNode).children[i], indent, text);
                }
            }

            return text;
        }

        Node CopyNode(Node tree)
        {
            Node newtree=new ConstNode(null,0);
            if (tree is FuncNode)
            {
                newtree = new FuncNode(null, (tree as FuncNode).function);
                for (int i = 0; i < (tree as FuncNode).children.Count; i++)
                {
                    Node child=CopyNode((tree as FuncNode).children[i]);
                    child.parent = newtree;
                    (newtree as FuncNode).children.Add(child);
                }
            }
            if (tree is VariableNode)
            {
                return new VariableNode(null, (tree as VariableNode).variable);
            }
            if (tree is ConstNode)
            {
                return new ConstNode(null, (tree as ConstNode).constant);
            }

            return newtree;
            //Node Newindividual = new Node(Newindividual.parent);
        }
    }
}
