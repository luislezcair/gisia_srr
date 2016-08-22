using System.Collections.Generic;

namespace VTeIC.Requerimientos.Web.SearchKey.Tree
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

        public virtual bool InsertSubKeySeparator()
        {
            return true;
        }
    }
}