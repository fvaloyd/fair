using Collaboration.Adjusters;
using Collaboration.Contributions;
using Collaboration.Groups;
using Collaboration.Members;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<GroupStore>();
builder.Services.AddSingleton<MemberStore>();
builder.Services.AddSingleton<ContributionStore>();
builder.Services.AddSingleton<ContributionTypeStore>();
builder.Services.AddScoped<Adjuster>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGroupEndpoints();
app.MapMemberEndpoints();
app.MapContributionTypeEndpoints();

app.Run();
