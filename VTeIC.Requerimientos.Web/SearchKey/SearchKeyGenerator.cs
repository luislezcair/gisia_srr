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
        private static string RemoveDiacritics(string text)
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

        public static List<string> BuildSearchKey(List<Answer> answers)
        {
            // Ordena las respuestas por peso y omite las respuestas booleanas, las respuestas vacías
            // y las respuestas de opciones múltiples sin opciones últiles seleccionadas
            IOrderedEnumerable<Answer> weightedAnswers = answers.Where(a => a.AnswerType != QuestionTypes.BOOLEAN)
                                                                .Where(a => !((a.AnswerType == QuestionTypes.TEXT_FIELD || a.AnswerType == QuestionTypes.EXCLUSION_TERMS) && a.TextAnswer == null))
                                                                .Where(a => !(a.AnswerType == QuestionTypes.MULTIPLE_CHOICE && !a.MultipleChoiceAnswer.Where(c => c.UseInSearchKeyAs != null).Any()))
                                                                .OrderBy(a => a.Question.Weight);

            var pivotAnswer = answers.FirstOrDefault(a => a.Question.IsPivot);

            Node rootNode = BuildSearchKeyTree(weightedAnswers);

            //Tree.Tree.Traverse(rootNode);

            SearchKeyGenericStrategy strategy = new SearchKeyGenericStrategy();
            List<string> genericSearchKey = strategy.BuildSearchKey(rootNode);

            SearchKeyBreakORStrategy orStrategy = new SearchKeyBreakORStrategy();
            List<string> orKeys = orStrategy.BuildSearchKey(rootNode);

            genericSearchKey.AddRange(orKeys);

            // Genera una clave más con la respuesta a la pregunta pivot (Tema) y la coloca
            // siempre primera en la lista.
            if (pivotAnswer != null)
            {
                genericSearchKey.Insert(0, pivotAnswer.TextAnswer);
            }

            foreach (var key in genericSearchKey)
            {
                Debug.Print("SEARCH KEY: {0}", key);
            }

            return genericSearchKey.ConvertAll(s => RemoveDiacritics(s));
        }

        private static Node BuildSearchKeyTree(IOrderedEnumerable<Answer> answers)
        {
            QuestionDBContext db = new QuestionDBContext();

            NodeAND root = new NodeAND();
            OperatorNode currentNode = root;
            Answer previousAnswer = null;

            foreach (Answer answer in answers)
            {
                // Pregunta con varias opciones: OR entre las opciones seleccionadas.
                if(answer.AnswerType == QuestionTypes.MULTIPLE_CHOICE)
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
                else if(answer.AnswerType == QuestionTypes.EXCLUSION_TERMS)
                {
                    // Si es una pregunta de exclusión se crea un nodo NOT por cada término y
                    // se lo agrega directamente al nodo raíz.
                    var wordList = answer.TextAnswer.Split(' ', ',').Where(w => w.Length > 0);

                    NodeNOT nodeNot = new NodeNOT();
                    foreach (var word in wordList)
                    {
                        nodeNot.Children.Add(new DataNode(word));
                    }
                    root.Children.Add(nodeNot);
                }
                else
                {
                    if (previousAnswer != null) 
                    {
                        QuestionRelationshipOperator db_op = db.Operators.Where(o => (o.First.Id == previousAnswer.Question.Id && o.Second.Id == answer.Question.Id) ||
                                                                                      o.First.Id == answer.Question.Id && o.Second.Id == previousAnswer.Question.Id)
                                                                         .FirstOrDefault();
                        QuestionOperator op = db_op != null ? db_op.Operator : QuestionOperator.AND;

                        if (op != currentNode.GetQuestionOperator())
                        {
                            currentNode = OperatorNode.CreateFromOperator(op);
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