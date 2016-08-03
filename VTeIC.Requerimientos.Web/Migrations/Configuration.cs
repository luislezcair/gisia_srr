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
            context.Database.ExecuteSqlCommand("DELETE FROM QuestionRelationshipOperators");
            //context.Database.ExecuteSqlCommand("DELETE FROM ChoiceOptions");
            context.Database.ExecuteSqlCommand("DELETE FROM Questions");
            context.Database.ExecuteSqlCommand("DELETE FROM Sessions");
            context.Database.ExecuteSqlCommand("DELETE FROM QuestionGroups");
            //context.Database.ExecuteSqlCommand("DELETE FROM SearchKeyStrings");
            //context.Database.ExecuteSqlCommand("DELETE FROM Languages");

            Language english = new Language
            {
                Id = 1,
                Name = "Ingl�s"
            };

            Language spanish = new Language
            {
                Id = 2,
                Name = "Espa�ol"
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
                Text = "Especifique qu� desea hacer con [previous_answer]",
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
                Text = "�Qu� fuentes de informaci�n desea consultar?",
                QuestionType = QuestionTypes.MULTIPLE_CHOICE,
                IsPivot = false,
                Weight = 0,
                ChoiceOptions = { sourcePaper, sourcePatents, sourceOthers, sourceDocuments }
            };

            Question featureQ = new Question
            {
                Text = "Ingrese caracter�stica",
                QuestionType = QuestionTypes.TEXT_FIELD,
                IsPivot = false,
                Weight = 0
            };

            Question anotherFeatureQ = new Question
            {
                Text = "�[previous_answer] posee otra caracter�stica de inter�s?",
                QuestionType = QuestionTypes.BOOLEAN,
                IsPivot = false,
                Weight = 0
            };

            Question particularFeatureQ = new Question
            {
                Text = "�Le interesa una caracter�stica particular de [previous_answer]?",
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
                Text = "Especifique la regi�n geogr�fica por la que desea filtrar los resultados",
                QuestionType = QuestionTypes.TEXT_FIELD,
                IsPivot = false,
                Weight = 0
            };

            Question exclusionQ = new Question
            {
                Text = "�Desea excluir alg�n t�rmino de la b�squeda?",
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
                new QuestionGroup { Title = "Tema", Questions = { subjectQ } },
                new QuestionGroup { Title = "Caracter�sticas", Questions = { particularFeatureQ, featureQ, anotherFeatureQ } },
                new QuestionGroup { Title = "Fuentes de Informaci�n", Questions = { sourcesQ } },
                new QuestionGroup { Title = "Datos adicionales sobre la fuente", Questions = { sourceDataQ } },
                new QuestionGroup { Title = "Acci�n", Questions = { actionQ } },
                new QuestionGroup { Title = "Regi�n geogr�fica", Questions = { regionQ } },
                new QuestionGroup { Title = "T�rminos a excluir", Questions = { exclusionQ } }
            );

            context.QuestionLinks.AddOrUpdate(
                ql => ql.Id,
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
