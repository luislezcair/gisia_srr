using System.Collections.Generic;
using VTeIC.Requerimientos.Web.SearchKey.Tree;

namespace VTeIC.Requerimientos.Web.SearchKey.Strategy
{
    interface ISearchKeyBuildStrategy
    {
        List<string> BuildSearchKey(Node root);
    }
}
