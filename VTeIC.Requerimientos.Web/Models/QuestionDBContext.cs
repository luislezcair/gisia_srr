using System.Data.Entity;
using VTeIC.Requerimientos.Entidades;

namespace VTeIC.Requerimientos.Web.Models
{
    public class QuestionDBContext : DbContext
    {
        public DbSet<Question> Questions { get; set; }
        public DbSet<ChoiceOption> QuestionChoices { get; set; }
        public DbSet<QuestionGroup> QuestionGroups { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<QuestionRelationshipOperator> Operators { get; set; }
        public DbSet<QuestionLink> QuestionLinks { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<SearchKeyString> SearchKeyStrings { get; set; }

        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectSearchKey> ProjectSearchKeys { get; set; }

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
                .WithOptional()
                .WillCascadeOnDelete();

            modelBuilder.Entity<Answer>()
                .HasRequired(q => q.Question)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Answer>()
                .HasMany(c => c.MultipleChoiceAnswer)
                .WithMany()
                .Map(x =>
                {
                    x.MapLeftKey("Answer_Id");
                    x.MapRightKey("ChoiceOption_Id");
                    x.ToTable("MultipleChoiceAnswers");
                });

            modelBuilder.Entity<QuestionRelationshipOperator>()
                .HasRequired(q => q.First)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<QuestionRelationshipOperator>()
                .HasRequired(q => q.Second)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<QuestionLink>()
                .HasRequired(q => q.Question);

            modelBuilder.Entity<QuestionLink>()
                .HasOptional(q => q.Next);

            modelBuilder.Entity<QuestionLink>()
                .HasOptional(q => q.NextNegative);

            modelBuilder.Entity<Project>()
                .HasMany(p => p.SearchKeys)
                .WithRequired(s => s.Project);

            modelBuilder.Entity<Project>()
                .HasMany(a => a.Answers)
                .WithRequired(p => p.Project)
                .WillCascadeOnDelete(true);
        }
    }
}