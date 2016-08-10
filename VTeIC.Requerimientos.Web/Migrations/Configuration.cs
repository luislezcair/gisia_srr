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
            context.SaveChanges();
        }

        protected override void Seed(VTeIC.Requerimientos.Web.Models.QuestionDBContext context)
        {
            context.Database.ExecuteSqlCommand("DELETE FROM MultipleChoiceAnswers");
            context.Database.ExecuteSqlCommand("DELETE FROM Answers");
            context.Database.ExecuteSqlCommand("DELETE FROM Sessions");

            Language english = new Language
            {
                Id = 1,
                Name = "Inglés"
            };

            Language spanish = new Language
            {
                Id = 2,
                Name = "Español"
            };

            context.Languages.AddOrUpdate(l => l.Id, english, spanish);

            SearchKeyString keyBuy = new SearchKeyString
            {
                Id = 1,
                Language = english,
                SearchKeyParam = "buy"
            };

            SearchKeyString keyBuy2 = new SearchKeyString
            {
                Id = 2,
                Language = spanish,
                SearchKeyParam = "comprar"
            };

            SearchKeyString keySell = new SearchKeyString
            {
                Id = 3,
                Language = english,
                SearchKeyParam = "sell"
            };

            SearchKeyString keySell2 = new SearchKeyString
            {
                Id = 4,
                Language = spanish,
                SearchKeyParam = "vender"
            };

            SearchKeyString keyHire = new SearchKeyString
            {
                Id = 5,
                Language = english,
                SearchKeyParam = "hire"
            };

            SearchKeyString keyHire2 = new SearchKeyString
            {
                Id = 6,
                Language = spanish,
                SearchKeyParam = "contratar"
            };

            context.SearchKeyStrings.AddOrUpdate(s => s.Id,
                keyBuy,
                keyBuy2,
                keyHire,
                keyHire2,
                keySell,
                keySell2);

            ChoiceOption sourcePaper = new ChoiceOption
            {
                Id = 1,
                Text = "Papers",
                UseInSearchKey = true,
                UseInSearchKeyAs = "paper OR cite",
                Weight = 1
            };

            ChoiceOption sourcePatents = new ChoiceOption
            {
                Id = 2,
                Text = "Patentes",
                UseInSearchKey = true,
                UseInSearchKeyAs = "patents",
                Weight = 2
            };

            ChoiceOption sourceDocuments = new ChoiceOption
            {
                Id = 3,
                Text = "Documentos",
                UseInSearchKey = false,
                Weight = 3
            };

            ChoiceOption sourceOthers = new ChoiceOption
            {
                Id = 4,
                Text = "Otros",
                UseInSearchKey = false,
                Weight = 4
            };

            ChoiceOption actionBuy = new ChoiceOption
            {
                Id = 5,
                Text = "Comprar",
                UseInSearchKey = true,
                UseInSearchKeyAs = "comprar",
                SearchKeyStrings = { keyBuy, keyBuy2 }
            };

            ChoiceOption actionSell = new ChoiceOption
            {
                Id = 6,
                Text = "Vender",
                UseInSearchKey = true,
                UseInSearchKeyAs = "vender",
                SearchKeyStrings = { keySell, keySell2 }
            };

            ChoiceOption actionHire = new ChoiceOption
            {
                Id = 7,
                Text = "Contratar",
                UseInSearchKey = true,
                UseInSearchKeyAs = "contratar",
                SearchKeyStrings = { keyHire, keyHire2 } 
            };

            ChoiceOption actionKnow = new ChoiceOption
            {
                Id = 8,
                Text = "Conocer",
                UseInSearchKey = false,
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
                Id = 1,
                Text = "Especifique qué desea hacer con [previous_answer]",
                QuestionType = QuestionTypes.MULTIPLE_CHOICE,
                IsPivot = false,
                Weight = 1000,
                ChoiceOptions = { actionBuy, actionSell, actionHire, actionKnow }
            };

            Question sourceDataQ = new Question
            {
                Id = 2,
                Text = "Especifique datos adicionales sobre la fuente",
                QuestionType = QuestionTypes.TEXT_FIELD,
                IsPivot = false,
                Weight = 0
            };

            Question sourcesQ = new Question
            {
                Id = 3,
                Text = "¿Qué fuentes de información desea consultar?",
                QuestionType = QuestionTypes.MULTIPLE_CHOICE,
                IsPivot = false,
                Weight = 0,
                ChoiceOptions = { sourcePaper, sourcePatents, sourceOthers, sourceDocuments }
            };

            Question featureQ = new Question
            {
                Id = 4,
                Text = "Ingrese característica",
                QuestionType = QuestionTypes.TEXT_FIELD,
                IsPivot = false,
                Weight = 0
            };

            Question anotherFeatureQ = new Question
            {
                Id = 5,
                Text = "¿[previous_answer] posee otra característica de interés?",
                QuestionType = QuestionTypes.BOOLEAN,
                IsPivot = false,
                Weight = 0
            };

            Question particularFeatureQ = new Question
            {
                Id = 6,
                Text = "¿Le interesa una característica particular de [previous_answer]?",
                QuestionType = QuestionTypes.BOOLEAN,
                IsPivot = false,
                Weight = 0
            };

            Question subjectQ = new Question
            {
                Id = 7,
                Text = "Introduzca el tema",
                QuestionType = QuestionTypes.TEXT_FIELD,
                IsPivot = true,
                Weight = 999,
            };

            Question regionQ = new Question
            {
                Id = 8,
                Text = "Especifique la región geográfica por la que desea filtrar los resultados",
                QuestionType = QuestionTypes.TEXT_FIELD,
                IsPivot = false,
                Weight = 0
            };

            Question exclusionQ = new Question
            {
                Id = 9,
                Text = "¿Desea excluir algún término de la búsqueda?",
                QuestionType = QuestionTypes.EXCLUSION_TERMS,
                IsPivot = false,
                Weight = -1
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
                regionQ,
                exclusionQ
            );

            context.QuestionGroups.AddOrUpdate(
                q => q.Id,
                new QuestionGroup { Id = 1, Title = "Tema", Questions = { subjectQ } },
                new QuestionGroup { Id = 2, Title = "Características", Questions = { particularFeatureQ, featureQ, anotherFeatureQ } },
                new QuestionGroup { Id = 3, Title = "Fuentes de Información", Questions = { sourcesQ } },
                new QuestionGroup { Id = 4, Title = "Datos adicionales sobre la fuente", Questions = { sourceDataQ } },
                new QuestionGroup { Id = 5, Title = "Acción", Questions = { actionQ } },
                new QuestionGroup { Id = 6, Title = "Región geográfica", Questions = { regionQ } },
                new QuestionGroup { Id = 7, Title = "Términos a excluir", Questions = { exclusionQ } }
            );

            context.QuestionLinks.AddOrUpdate(
                ql => ql.Id,
                new QuestionLink { Id = 1, Question = subjectQ, Next = particularFeatureQ },
                new QuestionLink { Id = 2, Question = particularFeatureQ, Next = featureQ, NextNegative = sourcesQ },
                new QuestionLink { Id = 3, Question = featureQ, Next = anotherFeatureQ },
                new QuestionLink { Id = 4, Question = anotherFeatureQ, Next = featureQ, NextNegative = sourcesQ },
                new QuestionLink { Id = 5, Question = sourcesQ, Next = sourceDataQ },
                new QuestionLink { Id = 6, Question = sourceDataQ, Next = actionQ },
                new QuestionLink { Id = 7, Question = actionQ, Next = regionQ },
                new QuestionLink { Id = 8, Question = regionQ, Next = exclusionQ }
            );

            context.Operators.AddOrUpdate(
                o => o.Id,
                new QuestionRelationshipOperator { Id = 1, First = subjectQ, Second = featureQ, Operator = QuestionOperator.AND },
                new QuestionRelationshipOperator { Id = 2, First = subjectQ, Second = sourcesQ, Operator = QuestionOperator.AND },
                new QuestionRelationshipOperator { Id = 3, First = subjectQ, Second = sourceDataQ, Operator = QuestionOperator.AND },
                new QuestionRelationshipOperator { Id = 4, First = subjectQ, Second = actionQ, Operator = QuestionOperator.AND },

                new QuestionRelationshipOperator { Id = 5, First = featureQ, Second = featureQ, Operator = QuestionOperator.AND },
                new QuestionRelationshipOperator { Id = 6, First = featureQ, Second = sourcesQ, Operator = QuestionOperator.AND },
                new QuestionRelationshipOperator { Id = 7, First = featureQ, Second = featureQ, Operator = QuestionOperator.AND },

                new QuestionRelationshipOperator { Id = 8, First = sourcesQ, Second = sourcesQ, Operator = QuestionOperator.OR },
                new QuestionRelationshipOperator { Id = 9, First = sourcesQ, Second = sourceDataQ, Operator = QuestionOperator.AND },
                
                new QuestionRelationshipOperator { Id = 10, First = regionQ, Second = sourceDataQ, Operator = QuestionOperator.AND }
            );

            SaveChanges(context);
        }
    }
}
