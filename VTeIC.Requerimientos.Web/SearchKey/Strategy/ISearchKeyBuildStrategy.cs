using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VTeIC.Requerimientos.Web.SerachKey.Tree;

namespace VTeIC.Requerimientos.Web.SerachKey.Strategy
{
    interface ISearchKeyBuildStrategy
    {
        List<string> BuildSearchKey(Node root);
    }
}
