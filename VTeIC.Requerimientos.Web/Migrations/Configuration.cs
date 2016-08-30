namespace VTeIC.Requerimientos.Web.Migrations
{
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
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
            context.SaveChanges();
        }

        protected override void Seed(VTeIC.Requerimientos.Web.Models.QuestionDBContext context)
        {
            context.Database.ExecuteSqlCommand("DELETE FROM MultipleChoiceAnswers");
            context.Database.ExecuteSqlCommand("DELETE FROM Answers");

            /*
            Language english = new Language
            {
                Name = "Inglés"
            };

            Language spanish = new Language
            {
                Name = "Español"
            };

            context.Languages.AddOrUpdate(l => l.Name, english, spanish);

            SearchKeyString keyBuy = new SearchKeyString
            {
                Language = english,
                SearchKeyParam = "buy"
            };

            SearchKeyString keyBuy2 = new SearchKeyString
            {
                Language = spanish,
                SearchKeyParam = "comprar"
            };

            SearchKeyString keySell = new SearchKeyString
            {
                Language = english,
                SearchKeyParam = "sell"
            };

            SearchKeyString keySell2 = new SearchKeyString
            {
                Language = spanish,
                SearchKeyParam = "vender"
            };

            SearchKeyString keyHire = new SearchKeyString
            {
                Language = english,
                SearchKeyParam = "hire"
            };

            SearchKeyString keyHire2 = new SearchKeyString
            {
                Language = spanish,
                SearchKeyParam = "contratar"
            };

            context.SearchKeyStrings.AddOrUpdate(s => s.SearchKeyParam,
                keyBuy,
                keyBuy2,
                keyHire,
                keyHire2,
                keySell,
                keySell2);

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
                UseInSearchKeyAs = "comprar",
                SearchKeyStrings = { keyBuy, keyBuy2 }
            };

            ChoiceOption actionSell = new ChoiceOption
            {
                Text = "Vender",
                UseInSearchKey = true,
                UseInSearchKeyAs = "vender",
                SearchKeyStrings = { keySell, keySell2 }
            };

            ChoiceOption actionHire = new ChoiceOption
            {
                Text = "Contratar",
                UseInSearchKey = true,
                UseInSearchKeyAs = "contratar",
                SearchKeyStrings = { keyHire, keyHire2 } 
            };

            ChoiceOption actionKnow = new ChoiceOption
            {
                Text = "Conocer",
                UseInSearchKey = false,
            };

            context.QuestionChoices.AddOrUpdate(
                c => c.Text,
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
                QuestionType = QuestionTypes.MULTIPLE_CHOICE,
                IsPivot = false,
                Weight = 1000,
                ChoiceOptions = { actionBuy, actionSell, actionHire, actionKnow }
            };

            Question sourceDataQ = new Question
            {
                Text = "Especifique datos adicionales sobre la fuente",
                QuestionType = QuestionTypes.TEXT_FIELD,
                IsPivot = false,
                Weight = 0
            };

            Question sourcesQ = new Question
            {
                Text = "¿Qué fuentes de información desea consultar?",
                QuestionType = QuestionTypes.MULTIPLE_CHOICE,
                IsPivot = false,
                Weight = 0,
                ChoiceOptions = { sourcePaper, sourcePatents, sourceOthers, sourceDocuments }
            };

            Question featureQ = new Question
            {
                Text = "Ingrese característica",
                QuestionType = QuestionTypes.TEXT_FIELD,
                IsPivot = false,
                Weight = 0
            };

            Question anotherFeatureQ = new Question
            {
                Text = "¿[previous_answer] posee otra característica de interés?",
                QuestionType = QuestionTypes.BOOLEAN,
                IsPivot = false,
                Weight = 0
            };

            Question particularFeatureQ = new Question
            {
                Text = "¿Le interesa una característica particular de [previous_answer]?",
                QuestionType = QuestionTypes.BOOLEAN,
                IsPivot = false,
                Weight = 0
            };

            Question subjectQ = new Question
            {
                Text = "Introduzca el tema",
                QuestionType = QuestionTypes.TEXT_FIELD,
                IsPivot = true,
                Weight = 999,
            };

            Question regionQ = new Question
            {
                Text = "Especifique la región geográfica por la que desea filtrar los resultados",
                QuestionType = QuestionTypes.TEXT_FIELD,
                IsPivot = false,
                Weight = 0
            };

            Question exclusionQ = new Question
            {
                Text = "¿Desea excluir algún término de la búsqueda?",
                QuestionType = QuestionTypes.EXCLUSION_TERMS,
                IsPivot = false,
                Weight = -1
            };

            context.Questions.AddOrUpdate(
                q => q.Text,
                actionQ,
                sourceDataQ,
                sourcesQ,
                featureQ,
                anotherFeatureQ,
                particularFeatureQ,
                subjectQ,
                regionQ,
                exclusionQ
            );

            context.QuestionGroups.AddOrUpdate(
                q => q.Title,
                new QuestionGroup { Title = "Tema", Questions = { subjectQ } },
                new QuestionGroup { Title = "Características", Questions = { particularFeatureQ, featureQ, anotherFeatureQ } },
                new QuestionGroup { Title = "Fuentes de Información", Questions = { sourcesQ } },
                new QuestionGroup { Title = "Datos adicionales sobre la fuente", Questions = { sourceDataQ } },
                new QuestionGroup { Title = "Acción", Questions = { actionQ } },
                new QuestionGroup { Title = "Región geográfica", Questions = { regionQ } },
                new QuestionGroup { Title = "Términos a excluir", Questions = { exclusionQ } }
            );

            SaveChanges(context);

            context.QuestionLinks.AddOrUpdate(
                ql => new { ql.QuestionId, ql.NextId, ql.NextNegativeId },
                new QuestionLink { Question = subjectQ, Next = particularFeatureQ },
                new QuestionLink { Question = particularFeatureQ, Next = featureQ, NextNegative = sourcesQ },
                new QuestionLink { Question = featureQ, Next = anotherFeatureQ },
                new QuestionLink { Question = anotherFeatureQ, Next = featureQ, NextNegative = sourcesQ },
                new QuestionLink { Question = sourcesQ, Next = sourceDataQ },
                new QuestionLink { Question = sourceDataQ, Next = actionQ },
                new QuestionLink { Question = actionQ, Next = regionQ },
                new QuestionLink { Question = regionQ, Next = exclusionQ }
            );

            context.Operators.AddOrUpdate(
                o => new { o.FirstId, o.SecondId, o.Operator },
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
            */

            SaveChanges(context);
        }
    }
}
