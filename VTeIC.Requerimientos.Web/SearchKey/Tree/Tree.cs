using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace VTeIC.Requerimientos.Web.SerachKey.Tree
{
    public class Tree
    {
        public static void Traverse(Node node)
        {
            String nodeData = node.GetSubKey();

            Debug.WriteLine("Node data: " + nodeData);

            if (node.Children.Any())
            {
                Debug.WriteLine("NODE " + nodeData + " CHILDREN");
                foreach (Node n in node.Children)
                {
                    Traverse(n);
                }
                Debug.WriteLine("END OF NODE CHILDREN");
            }
        }

        public static void RemoveVisitedFromOperatorNodes(Node root)
        {
            Stack<Node> stack = new Stack<Node>();
            stack.Push(root);

            while(stack.Any())
            {
                Node currentNode = stack.Pop();

                if (currentNode is OperatorNode)
                {
                    currentNode.Visited = false;
                }

                foreach (Node n in currentNode.Children)
                {
                    stack.Push(n);
                }
            }
        }
    }
}