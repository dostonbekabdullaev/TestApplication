using System.Reflection;
using Test.Application.Configuration;
using Test.Core;
using Test.DAL.Data;
using Test.DAL.Repository;
using Microsoft.EntityFrameworkCore;
using Test.Application;
using Test.WebService;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.AddLogger();

    new Startup(builder.Configuration).ConfigureServices(builder.Services);

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
finally
{
    NLog.LogManager.Shutdown();
}
