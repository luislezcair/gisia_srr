using System.Collections.Generic;
using System.Linq;
using VTeIC.Requerimientos.Web.SerachKey.Tree;

namespace VTeIC.Requerimientos.Web.SerachKey.Strategy
{
    public class SearchKeyGenericStrategy : ISearchKeyBuildStrategy
    {
        public List<string> BuildSearchKey(Node root)
        {
            List<string> keys = new List<string>();
            string key = "";

            Stack<Node> stack = new Stack<Node>();
            stack.Push(root);

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
                    if (currentNode.Children.Any())
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
                        if(opNode.CanHaveSingleChild())
                        {
                            stack.Push(opNode);
                        }
                    }
                }
                else
                {
                    key += currentNode.GetSubKey();
                    if(currentNode.InsertSubKeySeparator())
                    {
                        key += " ";
                    }
                }
            }

            keys.Add(key.Trim());
            return keys;
        }
    }
}