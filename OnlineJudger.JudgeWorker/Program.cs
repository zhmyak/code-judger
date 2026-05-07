using Microsoft.EntityFrameworkCore;
using OnlineJudger.Domain.Stores;
using OnlineJudger.Infrastructure.Persistance;
using OnlineJudger.JudgeWorker;
using OnlineJudger.JudgeWorker.Interfaces;
using OnlineJudger.JudgeWorker.Services;


var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<OnlineJudgeContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

builder.Services.AddTransient<ISandboxExecuter, FileExecuter>();
builder.Services.AddTransient<ITestRunner, TestRunner>();
builder.Services.AddTransient<ITestCaseRepository, TestCaseRepository>();
builder.Services.AddTransient<ISubmissionRepository, SubmissionRepository>();
builder.Services.AddTransient<IProblemRepository, ProblemRepository>();
builder.Services.AddTransient<IQueueRepository, QueueRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IScoreCounter, ScoreCounter>();
builder.Services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<OnlineJudgeContext>());
builder.Services.AddTransient<JudgeEngine>();
builder.Services.AddHostedService<Worker>();
var host = builder.Build();
host.Run();
