using Microsoft.EntityFrameworkCore;
using OnlineJudger.Domain.Entities;
using OnlineJudger.Domain.Stores;
namespace OnlineJudger.Infrastructure.Persistance
{
    public class OnlineJudgeContext : DbContext, IUnitOfWork
    {
        public OnlineJudgeContext(DbContextOptions<OnlineJudgeContext> options) : base(options)
        {
        }
        public DbSet<JudgeQueue> JudgeQueue { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Problem> Problems { get; set; }
        public DbSet<Submission> Submissions { get; set; }
        public DbSet<TestCase> TestCases { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CodeSnippet> CodeSnippets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Submission>(entity =>
            {
                entity.Property(s => s.UserId).HasColumnName("user_id");
                entity.Property(s => s.ProblemId).HasColumnName("problem_id");
                entity.Property(s => s.LanguageId).HasColumnName("language_id");
                entity.Property(s => s.SourceCode).HasColumnName("source_code");
                entity.Property(s => s.Status).HasConversion<string>()
                    .HasMaxLength(30)
                    .IsUnicode(false);
                entity.Property(s => s.ExecutionTimeMs).HasColumnName("execution_time_ms");
                entity.Property(s => s.MemoryUsedKb).HasColumnName("memory_used_kb");
                entity.Property(s => s.ErrorMessage).HasColumnName("error_message");
                entity.Property(s => s.CreatedAt).HasColumnName("created_at");
                entity.Property(s => s.UpdatedAt).HasColumnName("updated_at");
            });
            modelBuilder.Entity<JudgeQueue>(entity =>
            {
                entity.Property(j => j.SubmissionId).HasColumnName("submission_id");
                entity.Property(j => j.Status).HasConversion<string>()
                    .HasMaxLength(30)
                    .IsUnicode(false);
                entity.Property(j => j.CreatedAt).HasColumnName("created_at");
                entity.Property(j => j.StartedAt).HasColumnName("started_at");
                entity.Property(j => j.FinishedAt).HasColumnName("finished_at");
            });
            modelBuilder.Entity<Language>(entity =>
            {
                entity.Property(l => l.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(l => l.FileName).HasColumnName("file_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(l => l.CompileCommand).HasColumnName("compile_command")
                    .HasMaxLength(100)
                    .IsUnicode(false); ;
                entity.Property(l => l.RunCommand).HasColumnName("run_command")
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(l => l.DockerImage).HasColumnName("docker_image")
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Problem>(entity =>
            {
                entity.Property(p => p.Title)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(p => p.TimeLimitMs).HasColumnName("time_limit_ms");
                entity.Property(p => p.MemoryLimitMb).HasColumnName("memory_limit_mb");
                entity.Property(p => p.CreatedAt).HasColumnName("created_at");
                entity.Property(p => p.MethodName).HasColumnName("method_name")
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TestCase>(entity =>
            {
                entity.Property(p => p.ProblemId).HasColumnName("problem_id");
                entity.Property(p => p.InputData).HasColumnName("input_data");
                entity.Property(p => p.ExpectedOutput).HasColumnName("expected_output");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(u => u.Email)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(u => u.PasswordHash)
                    .HasColumnName("password_hash")
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(u => u.CreatedAt)
                    .HasColumnName("created_at");
            });
            modelBuilder.Entity<CodeSnippet>(entity =>
            {
                entity.Property(cs => cs.ProblemId)
                    .HasColumnName("problem_id");
                entity.Property(cs => cs.LanguageId)
                    .HasColumnName("language_id");
                entity.Property(cs => cs.Code)
                    .HasColumnName("code");
            });

            modelBuilder.Entity<Submission>()
                .HasOne(s => s.Problem)
                .WithMany(p => p.Submissions)
                .HasForeignKey(s => s.ProblemId);
            modelBuilder.Entity<Submission>()
                .HasOne(s => s.User)
                .WithMany(u => u.Submissions)
                .HasForeignKey(s => s.UserId);
            modelBuilder.Entity<Submission>()
                .HasOne(s => s.Language)
                .WithMany(l => l.Submissions)
                .HasForeignKey(s => s.LanguageId);
            modelBuilder.Entity<TestCase>()
                .ToTable("test_cases")
                .HasOne(tc => tc.Problem)
                .WithMany(p => p.TestCases)
                .HasForeignKey(tc => tc.ProblemId);


            modelBuilder.Entity<JudgeQueue>()
                .ToTable("judge_queue")
                .HasOne(jq => jq.Submission)
                .WithOne()
                .HasForeignKey<JudgeQueue>(jq => jq.SubmissionId);
            modelBuilder.Entity<CodeSnippet>()
                .ToTable("code_snippets")
                .HasKey(cs => new { cs.ProblemId, cs.LanguageId });
        }
    }
}
