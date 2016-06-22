namespace VTeIC.Requerimientos.Web.Migrations
{
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Text;
    using VTeIC.Requerimientos.Entidades;

    internal sealed class Configuration : DbMigrationsConfiguration<VTeIC.Requerimientos.Web.Models.QuestionDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "VTeIC.Requerimientos.Web.Models.QuestionDBContext";
        }

        private void SaveChanges(DbContext context)
        {
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                throw new DbEntityValidationException(
                    "Entity Validation Failed - errors follow:\n" +
                    sb.ToString(), ex
                ); // Add the original exception as the innerException
            }
        }

        protected override void Seed(VTeIC.Requerimientos.Web.Models.QuestionDBContext context)
        {
            context.Database.ExecuteSqlCommand("DELETE FROM MultipleChoiceAnswers");
            context.Database.ExecuteSqlCommand("DELETE FROM Answers");
            context.Database.ExecuteSqlCommand("DELETE FROM QuestionRelationshipOperators");
            context.Database.ExecuteSqlCommand("DELETE FROM ChoiceOptions");
            context.Database.ExecuteSqlCommand("DELETE FROM QuestionTypes");
            context.Database.ExecuteSqlCommand("DELETE FROM Questions");
            context.Database.ExecuteSqlCommand("DELETE FROM Sessions");
            context.Database.ExecuteSqlCommand("DELETE FROM QuestionGroups");

            QuestionType textField = new QuestionType { Description = "Campo de texto", Type = QuestionTypes.TEXT_FIELD };
            QuestionType booleanQuestion = new QuestionType { Description = "Sí / No", Type = QuestionTypes.BOOLEAN };
            QuestionType multipleChoice = new QuestionType { Description = "Opciones múltiples", Type = QuestionTypes.MULTIPLE_CHOICE };

			context.QuestionTypes.AddOrUpdate(
	            qt => qt.Id,
	                textField,
	                booleanQuestion,
	                multipleChoice
            );

            ChoiceOption sourcePaper = new ChoiceOption
            {
                Text = "Papers",
                UseInSearchKey = true,
                UseInSearchKeyAs = "paper OR cite",
                Weight = 1
            };

            ChoiceOption sourcePatents = new ChoiceOption
            {
                Text = "Patentes",
                UseInSearchKey = true,
                UseInSearchKeyAs = "patents",
                Weight = 2
            };

            ChoiceOption sourceDocuments = new ChoiceOption
            {
                Text = "Documentos",
                UseInSearchKey = false,
                Weight = 3
            };

            ChoiceOption sourceOthers = new ChoiceOption
            {
                Text = "Otros",
                UseInSearchKey = false,
                Weight = 4
            };

            ChoiceOption actionBuy = new ChoiceOption
            {
                Text = "Comprar",
                UseInSearchKey = true,
                UseInSearchKeyAs = "comprar"
            };

            ChoiceOption actionSell = new ChoiceOption
            {
                Text = "Vender",
                UseInSearchKey = true,
                UseInSearchKeyAs = "vender"
            };

            ChoiceOption actionHire = new ChoiceOption
            {
                Text = "Contratar",
                UseInSearchKey = true,
                UseInSearchKeyAs = "contratar"
            };

            ChoiceOption actionKnow = new ChoiceOption
            {
                Text = "Conocer",
                UseInSearchKey = false
            };

            context.QuestionChoices.AddOrUpdate(
                c => c.Id,
                sourcePaper,
                sourcePatents,
                sourceOthers,
                sourceDocuments,
                actionBuy,
                actionSell,
                actionHire,
                actionKnow
            );

            Question actionQ = new Question
            {
                Text = "Especifique qué desea hacer con [previous_answer]",
                QuestionType = multipleChoice,
                IsPivot = false,
                Weight = 1000,
                Title = "Acción",
                HasManyAnswers = false,
                ChoiceOptions = { actionBuy, actionSell, actionHire, actionKnow }
            };

            Question sourceDataQ = new Question
            {
                Text = "Especifique datos adicionales sobre la fuente",
                QuestionType = textField,
                IsPivot = false,
                Weight = 0,
                HasManyAnswers = false,
                Title = "Datos adicionales sobre la fuente"
            };

            Question sourcesQ = new Question
            {
                Text = "¿Qué fuentes de información desea consultar?",
                QuestionType = multipleChoice,
                IsPivot = false,
                Weight = 0,
                Title = "Fuentes de información",
                HasManyAnswers = false,
                ChoiceOptions = { sourcePaper, sourcePatents, sourceOthers, sourceDocuments }
            };

            Question featureQ = new Question
            {
                Text = "Ingrese característica",
                QuestionType = textField,
                IsPivot = false,
                Weight = 0,
                Title = "Características",
                HasManyAnswers = true
            };

            Question anotherFeatureQ = new Question
            {
                Text = "¿[previous_answer] posee otra característica de interés?",
                QuestionType = booleanQuestion,
                IsPivot = false,
                Weight = 0,
                Title = "Características",
                HasManyAnswers = false,
            };

            Question particularFeatureQ = new Question
            {
                Text = "¿Le interesa una característica particular de [previous_answer]?",
                QuestionType = booleanQuestion,
                IsPivot = false,
                Weight = 0,
                Title = "Características",
                HasManyAnswers = false
            };

            Question subjectQ = new Question
            {
                Text = "Introduzca el tema",
                QuestionType = textField,
                IsPivot = true,
                Weight = 999,
                Title = "Tema",
                HasManyAnswers = false
            };

            Question regionQ = new Question
            {
                Text = "Especifique la región geográfica por la que desea filtrar los resultados",
                QuestionType = textField,
                IsPivot = false,
                Weight = 0,
                Title = "Región geográfica",
                HasManyAnswers = false
            };

            context.Questions.AddOrUpdate(
                q => q.Id,
                actionQ,
                sourceDataQ,
                sourcesQ,
                featureQ,
                anotherFeatureQ,
                particularFeatureQ,
                subjectQ,
                regionQ
            );

            context.QuestionGroups.AddOrUpdate(
                q => q.Id,
                new QuestionGroup { Title = "Tema", Questions = { subjectQ } },
                new QuestionGroup { Title = "Características", Questions = { particularFeatureQ, featureQ, anotherFeatureQ } },
                new QuestionGroup { Title = "Fuentes de Información", Questions = { sourcesQ } },
                new QuestionGroup { Title = "Datos adicionales sobre la fuente", Questions = { sourceDataQ } },
                new QuestionGroup { Title = "Acción", Questions = { actionQ } },
                new QuestionGroup { Title = "Región geográfica", Questions = { regionQ } }
            );

            context.QuestionLinks.AddOrUpdate(
                ql => ql.Id,
                new QuestionLink { Question = subjectQ, Next = particularFeatureQ },
                new QuestionLink { Question = particularFeatureQ, Next = featureQ, NextNegative = sourcesQ },
                new QuestionLink { Question = featureQ, Next = anotherFeatureQ },
                new QuestionLink { Question = anotherFeatureQ, Next = featureQ, NextNegative = sourcesQ },
                new QuestionLink { Question = sourcesQ, Next = sourceDataQ },
                new QuestionLink { Question = sourceDataQ, Next = actionQ },
                new QuestionLink { Question = actionQ, Next = regionQ }
            );

            context.Operators.AddOrUpdate(
                o => o.Id,
                new QuestionRelationshipOperator { First = subjectQ, Second = featureQ, Operator = QuestionOperator.AND },
                new QuestionRelationshipOperator { First = subjectQ, Second = sourcesQ, Operator = QuestionOperator.AND },
                new QuestionRelationshipOperator { First = subjectQ, Second = sourceDataQ, Operator = QuestionOperator.AND },
                new QuestionRelationshipOperator { First = subjectQ, Second = actionQ, Operator = QuestionOperator.AND },

                new QuestionRelationshipOperator { First = featureQ, Second = featureQ, Operator = QuestionOperator.AND },
                new QuestionRelationshipOperator { First = featureQ, Second = sourcesQ, Operator = QuestionOperator.AND },
                new QuestionRelationshipOperator { First = featureQ, Second = featureQ, Operator = QuestionOperator.AND },
                
                new QuestionRelationshipOperator { First = sourcesQ, Second = sourcesQ, Operator = QuestionOperator.OR },
                new QuestionRelationshipOperator { First = sourcesQ, Second = sourceDataQ, Operator = QuestionOperator.AND },
                
                new QuestionRelationshipOperator { First = regionQ, Second = sourceDataQ, Operator = QuestionOperator.AND }
            );

            SaveChanges(context);
        }
    }
}
