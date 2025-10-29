using Application;
using Infrastructure;
using Scalar.AspNetCore;
using WebApi.Pipelines.Middlewares;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddHttpContextAccessor();
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

const string corsName = "AllowClientsApp";
builder.Services.AddCorsForRegisterApp(builder.Configuration, corsName);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("/api-docs");
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseExceptionHandling();

app.MapControllers();

app.UseCors(corsName);

app.Run();
