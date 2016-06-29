using System;

namespace VTeIC.Requerimientos.Web.SerachKey.Tree
{
    public class DataNode : Node
    {
        public DataNode(string data)
        {
            Data = data;
        }

        public String Data { get; set; }

        public override string GetSubKey()
        {
            return Data;
        }
    }
}