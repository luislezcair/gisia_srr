using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VTeIC.Requerimientos.Entidades;

namespace VTeIC.Requerimientos.Web.SerachKey.Tree
{
    public class NodeNOT : OperatorNode
    {
        public override QuestionOperator GetQuestionOperator()
        {
            return QuestionOperator.NOT;
        }

        public override string GetSubKey()
        {
            return "-";
        }
    }
}