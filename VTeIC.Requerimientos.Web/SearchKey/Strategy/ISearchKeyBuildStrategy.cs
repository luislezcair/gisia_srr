using System.Collections.Generic;
using VTeIC.Requerimientos.Web.SerachKey.Tree;

namespace VTeIC.Requerimientos.Web.SerachKey.Strategy
{
    interface ISearchKeyBuildStrategy
    {
        List<string> BuildSearchKey(Node root);
    }
}
