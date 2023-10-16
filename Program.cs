using api_template.DataRepo;
using api_template.Interfaces;
using api_template.Utilities;
using Microsoft.AspNetCore.Builder;
using NLog.Extensions.Logging;


var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;
builder.Services.AddSingleton(configuration);

string connString = configuration.GetConnectionString("GutenbergBooksConnString");
builder.Services.AddSingleton(connString);

builder.Services.AddSingleton<Utility>();
builder.Services.AddSingleton<IDapperDb, DapperDb>();

// Add services to the container.
builder.Services.AddLogging(lbuilder =>
{
    lbuilder.ClearProviders();
    lbuilder.AddNLog("NLog.config");
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
