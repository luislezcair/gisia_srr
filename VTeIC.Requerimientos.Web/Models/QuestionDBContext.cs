using System.Data.Entity;
using VTeIC.Requerimientos.Entidades;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace VTeIC.Requerimientos.Web.Models
{
    public class QuestionDBContext : DbContext
    {
        public DbSet<Question> Questions { get; set; }
        public DbSet<ChoiceOption> QuestionChoices { get; set; }
        public DbSet<QuestionGroup> QuestionGroups { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<QuestionRelationshipOperator> Operators { get; set; }
        public DbSet<QuestionLink> QuestionLinks { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<SearchKeyString> SearchKeyStrings { get; set; }

        public DbSet<Project> Projects { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QuestionGroup>()
                .HasMany(q => q.Questions)
                .WithOptional(q => q.QuestionGroup);

            modelBuilder.Entity<ChoiceOption>()
                .HasRequired(q => q.Question)
                .WithMany(c => c.ChoiceOptions)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ChoiceOption>()
                .HasMany(c => c.SearchKeyStrings)
                .WithOptional();
                //.WillCascadeOnDelete();
                //.WithRequired(c => c.choice);

            modelBuilder.Entity<Answer>()
                .HasRequired<Question>(q => q.Question)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Answer>()
                .HasMany<ChoiceOption>(c => c.MultipleChoiceAnswer)
                .WithMany()
                .Map(x =>
                {
                    x.MapLeftKey("AnswerId");
                    x.MapRightKey("ChoiceId");
                    x.ToTable("MultipleChoiceAnswers");
                });

            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Session>()
                .HasMany<Answer>(a => a.Answers)
                .WithRequired(s => s.Session);

            modelBuilder.Entity<QuestionRelationshipOperator>()
                .HasRequired<Question>(q => q.First)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<QuestionRelationshipOperator>()
                .HasRequired<Question>(q => q.Second)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<QuestionLink>()
                .HasRequired<Question>(q => q.Question);
            modelBuilder.Entity<QuestionLink>()
                .HasOptional<Question>(q => q.Next);
            modelBuilder.Entity<QuestionLink>()
                .HasOptional<Question>(q => q.NextNegative);

            modelBuilder.Entity<Project>()
                .HasMany(p => p.SearchKeys)
                .WithRequired(s => s.Project);
        }
    }
}