using Core.Application.Extensions;
using Infrastructure.Persistence;
using Presentation.WebApi.Extensions;
using Presentation.WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

// 1. Register Services (Separated by Layer)
builder.Services.AddPresentationServices();
builder.Services.AddApiVersioningConfiguration();
builder.Services.AddSwaggerConfiguration();

builder.Services.AddApplicationServices();

builder.Services.AddInfrastructurePersistence(builder.Configuration);
builder.Services.AddInfrastructureAuthentication(builder.Configuration);

var app = builder.Build();

// 2. Configure the HTTP request pipeline
app.ApplyMigrations();
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
