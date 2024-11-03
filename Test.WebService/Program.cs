using System.Reflection;
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

    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
    }

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.UseExceptionHandler();

    app.MapControllers();

    app.Run();
}
finally
{
    NLog.LogManager.Shutdown();
}
