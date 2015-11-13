using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VTeIC.Requerimientos.Entidades;

namespace VTeIC.Requerimientos.Web.SerachKey.Tree
{
    public class NodeAND : OperatorNode
    {
        public override QuestionOperator GetQuestionOperator()
        {
            return QuestionOperator.AND;
        }

        public override string GetSubKey()
        {
            return "AND";
        }
    }
}