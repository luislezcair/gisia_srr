using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using VTeIC.Requerimientos.Entidades;
using VTeIC.Requerimientos.Web.Models;
using VTeIC.Requerimientos.Web.SerachKey.Strategy;
using VTeIC.Requerimientos.Web.SerachKey.Tree;

namespace VTeIC.Requerimientos.Web.SerachKey
{
    public class SearchKeyGenerator
    {
        private QuestionDBContext db = new QuestionDBContext();

        private string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public List<string> BuildSearchKey(List<Answer> answers)
        {
            // Ordena las respuestas por peso y omite las respuestas booleanas, las respuestas vacías
            // y las respuestas de opciones múltiples sin opciones últiles seleccionadas
            IOrderedEnumerable<Answer> weightedAnswers = answers.Where(a => a.AnswerType.Id != 2)
                                                                .Where(a => !(a.AnswerType.Id == 1 && a.TextAnswer == null))
                                                                .Where(a => !(a.AnswerType.Id == 3 && !a.MultipleChoiceAnswer.Where(c => c.UseInSearchKeyAs != null).Any()))
                                                                .OrderBy(a => a.Question.Weight);

            Node rootNode = BuildSearchKeyTree(weightedAnswers);

            Tree.Tree.Traverse(rootNode);

            SearchKeyGenericStrategy strategy = new SearchKeyGenericStrategy();
            List<string> genericSearchKey = strategy.BuildSearchKey(rootNode);

            SearchKeyBreakORStrategy orStrategy = new SearchKeyBreakORStrategy();
            List<string> orKeys = orStrategy.BuildSearchKey(rootNode);

            genericSearchKey.AddRange(orKeys);

            return genericSearchKey.ConvertAll<string>(s => RemoveDiacritics(s));
        }

        private Node BuildSearchKeyTree(IOrderedEnumerable<Answer> answers)
        {
            NodeAND root = new NodeAND();
            OperatorNode currentNode = root;
            Answer previousAnswer = null;

            foreach (Answer answer in answers)
            {
                // Pregunta con varias opciones: OR entre las opciones seleccionadas.
                if(answer.AnswerType.Id == 3)
                {
                    var options = answer.MultipleChoiceAnswer.Where(c => c.UseInSearchKeyAs != null);

                    Node optionNode;
                    if (options.Count() > 1)
                    {
                        optionNode = new NodeOR();
                        foreach (ChoiceOption choice in options)
                        {
                            optionNode.Children.Add(new DataNode(choice.UseInSearchKeyAs));
                        }
                    }
                    else
                    {
                        // Si hay una sola opción se la agrega directamente al nodo padre sin un nodo OR
                        optionNode = new DataNode(options.First().UseInSearchKeyAs);
                    }
                    root.Children.Add(optionNode);
                }
                else
                {
                    if (previousAnswer != null) 
                    {
                        QuestionRelationshipOperator op = db.Operators.Where(o => (o.First.Id == previousAnswer.Question.Id && o.Second.Id == answer.Question.Id) ||
                                                                                   o.First.Id == answer.Question.Id && o.Second.Id == previousAnswer.Question.Id)
                                                                      .First();

                        if (op.Operator != currentNode.GetQuestionOperator())
                        {
                            currentNode = OperatorNode.CreateFromOperator(op.Operator);
                            root.Children.Add(currentNode);
                        }                        
                    }

                    currentNode.Children.Add(new DataNode(answer.TextAnswer));
                }

                previousAnswer = answer;
            }

            return root;
        }
    }
}