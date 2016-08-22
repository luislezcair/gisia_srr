using System.Collections.Generic;
using System.Linq;
using VTeIC.Requerimientos.Web.SearchKey.Tree;

namespace VTeIC.Requerimientos.Web.SearchKey.Strategy
{
    public class SearchKeyBreakORStrategy : ISearchKeyBuildStrategy
    {
        private string BuildSingleKey(Node root)
        {
            string key = "";

            Stack<Node> stack = new Stack<Node>();
            stack.Push(root);

            OperatorNode currentOr = null;

            while (stack.Any())
            {
                Node currentNode = stack.Pop();

                // Si este es un operador y le sigue un operador que se aplica sobre cada uno de sus hijos
                // omitimos este operador.
                if (currentNode is OperatorNode && stack.Any())
                {
                    Node nextNode = stack.Peek();
                    if (nextNode is OperatorNode)
                    {
                        OperatorNode opNode = (OperatorNode)nextNode;
                        if (opNode.CanHaveSingleChild())
                        {
                            continue;
                        }
                    }
                }

                if (!currentNode.Visited && currentNode is OperatorNode)
                {
                    OperatorNode op = (OperatorNode)currentNode;
                    if (op is NodeOR && !op.Broken && (currentOr == null || currentOr == op))
                    {
                        currentOr = op;

                        // Busca el primer hijo no visitado y lo usa en la clave
                        var nodes = currentOr.Children.Where(n => !n.Visited);
                        if(nodes.Any())
                        {
                            // Si es el último nodo no visitado, establece este nodo OR como roto.
                            if (nodes.Count() == 1)
                            {
                                currentOr.Broken = true;
                            }

                            Node node = nodes.First();
                            node.Visited = true;
                            stack.Push(node);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < currentNode.Children.Count() - 1; i++)
                        {
                            stack.Push(currentNode.Children.ElementAt(i));
                            stack.Push(currentNode);
                        }
                        stack.Push(currentNode.Children.Last());
                        currentNode.Visited = true;

                        // Si el operador se aplica sobre cada uno de sus hijos, hay que agregarlo siempre
                        OperatorNode opNode = (OperatorNode)currentNode;
                        if (opNode.CanHaveSingleChild())
                        {
                            stack.Push(opNode);
                        }
                    }
                }
                else
                {
                    key += currentNode.GetSubKey();
                    if (currentNode.InsertSubKeySeparator())
                    {
                        key += " ";
                    }
                }
            }

            return key.Trim();
        }

        private int GetIterations(Node root)
        {
            Stack<Node> stack = new Stack<Node>();
            stack.Push(root);

            int count = 0;

            while (stack.Any())
            {
                Node currentNode = stack.Pop();

                if (currentNode is NodeOR)
                {
                    count += currentNode.Children.Count();
                }

                foreach(Node node in currentNode.Children)
                {
                    stack.Push(node);
                }
            }

            return count;
        }

        public List<string> BuildSearchKey(Node root)
        {
            List<string> keys = new List<string>();
            int iterations = GetIterations(root);

            for(int i = 0; i < iterations; i++)
            {
                Tree.Tree.RemoveVisitedFromOperatorNodes(root);
                string k = BuildSingleKey(root);
                keys.Add(k);
            }

            return keys;
        }
    }
}