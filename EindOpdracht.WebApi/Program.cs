﻿using Microsoft.Identity.Client;
using EindOpdracht.WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.Configure<RouteOptions>(o => o.LowercaseUrls = true);

var sqlConnectionString = builder.Configuration.GetValue<string>("SqlConnectionString"); 
var sqlConnectionStringFound = !string.IsNullOrWhiteSpace(sqlConnectionString);

builder.Services.AddTransient<SqlEnvironment2DRepository, SqlEnvironment2DRepository>(o => new SqlEnvironment2DRepository(sqlConnectionString));

var app = builder.Build();

app.MapGet("/", () => $"The API is up 🛸\nConnection string found: {(sqlConnectionStringFound ? "✅ hij doet het 👍👍👍👍" : "❌")}");

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.MapOpenApi();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
