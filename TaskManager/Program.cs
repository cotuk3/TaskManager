using TaskManager.Api.Endpoints.Users;
using TaskManager.Api.EndPoints.Tasks;
using TaskManager.Api.Extensions;
using TaskManager.Domain.Entities;
using TaskManager.Startup;

var builder = WebApplication.CreateBuilder(args);

builder.AddDependencies();

var app = builder.Build();

app.UseOpenApi();

app.UseHttpsRedirection();

app.UseRequestLogging();


app.UseAuthentication();
app.UseAuthorization();

app.AddUserEndPoints();
app.AddTaskEndPoints();

app.MapIdentityApi<User>();

await app.RunAsync();
