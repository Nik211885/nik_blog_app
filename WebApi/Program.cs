using Application;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Scalar.AspNetCore;
using WebApi.Pipelines.Middlewares;
using WebApi.Services;
using WebApi.Services.BackgroundTaskQueue;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddHttpContextAccessor();
builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
builder.Services.AddHostedService<QueuedHostedService>();

var authenticationExtend = builder.Configuration.GetSection("AuthenticationExtend");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle((options) =>
{
    options.ClientId = authenticationExtend["Google:ClientId"] ?? throw new Exception("Not config for google authentication");
    options.ClientSecret = authenticationExtend["Google:ClientSecret"] ?? throw new Exception("Not config for google authentication");
    options.CallbackPath = authenticationExtend["Google:CallBackPath"];
});

builder.Services.AddAuthorization();

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
