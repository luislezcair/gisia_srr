using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VTeIC.Requerimientos.Web.SerachKey.Tree
{
    public abstract class Node
    {
        public Node()
        {
            Children = new List<Node>();
        }

        public List<Node> Children { get; private set; }

        public bool Visited { get; set; }

        public abstract string GetSubKey();
    }
}