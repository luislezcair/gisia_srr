using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
                    }
                }
                else
                {
                    key += currentNode.GetSubKey() + " ";
                }
            }

            keys.Add(key.Trim());
            return keys;
        }
    }
}