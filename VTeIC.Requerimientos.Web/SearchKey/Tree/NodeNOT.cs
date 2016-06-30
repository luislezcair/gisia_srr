﻿using VTeIC.Requerimientos.Entidades;

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

        public override bool InsertSubKeySeparator()
        {
            return false;
        }

        public override bool CanHaveSingleChild()
        {
            return true;
        }
    }
}