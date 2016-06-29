﻿using VTeIC.Requerimientos.Entidades;

namespace VTeIC.Requerimientos.Web.SerachKey.Tree
{
    public class NodeOR : OperatorNode
    {
        public override QuestionOperator GetQuestionOperator()
        {
            return QuestionOperator.OR;
        }

        public override string GetSubKey()
        {
            return "OR";
        }
    }
}