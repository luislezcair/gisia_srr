using VTeIC.Requerimientos.Entidades;

namespace VTeIC.Requerimientos.Web.SearchKey.Tree
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