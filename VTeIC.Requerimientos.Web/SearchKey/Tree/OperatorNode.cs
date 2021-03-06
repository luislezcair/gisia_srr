﻿using System;
using VTeIC.Requerimientos.Entidades;


namespace VTeIC.Requerimientos.Web.SearchKey.Tree
{
    public abstract class OperatorNode : Node
    {
        public bool Broken { get; set; }

        public static OperatorNode CreateFromOperator(QuestionOperator op)
        {
            switch (op)
            {
                case QuestionOperator.AND:
                    return new NodeAND();
                case QuestionOperator.OR:
                    return new NodeOR();
                default:
                    throw new NotImplementedException("Operator node not implemented");
            }
        }

        public abstract QuestionOperator GetQuestionOperator();

        public virtual bool CanHaveSingleChild()
        {
            return false;
        }
    }
}