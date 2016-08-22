using System;

namespace VTeIC.Requerimientos.Web.SearchKey.Tree
{
    public class DataNode : Node
    {
        public DataNode(string data)
        {
            Data = data;
        }

        public string Data { get; set; }

        public override string GetSubKey()
        {
            return Data;
        }
    }
}