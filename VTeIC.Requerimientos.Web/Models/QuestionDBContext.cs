using System.Data.Entity;
using VTeIC.Requerimientos.Entidades;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace VTeIC.Requerimientos.Web.Models
{
    public class QuestionDBContext : DbContext
    {
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionType> QuestionTypes { get; set; }
        public DbSet<ChoiceOption> QuestionChoices { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<QuestionRelationshipOperator> Operators { get; set; }
        public DbSet<QuestionLink> QuestionLinks { get; set; }

        public DbSet<Project> Projects { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Question>()
                .HasRequired<QuestionType>(q => q.QuestionType);

            modelBuilder.Entity<ChoiceOption>()
                .HasRequired<Question>(q => q.Question)
                .WithMany(c => c.ChoiceOptions)
                .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Question>()
            //    .HasOptional(q => q.NextQuestion)
            //    .WithMany()
            //    .HasForeignKey(q => q.NextQuestionId);

            //modelBuilder.Entity<Question>()
            //    .HasOptional(q => q.NextQuestionNegative);
                //.WithMany()
                //.HasForeignKey(q => q.NextQuestionNegativeId)
                //.WillCascadeOnDelete(false);

            modelBuilder.Entity<Answer>()
                .HasRequired<Question>(q => q.Question)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Answer>()
                .HasRequired<QuestionType>(a => a.AnswerType)
                .WithMany();

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
        }
    }
}