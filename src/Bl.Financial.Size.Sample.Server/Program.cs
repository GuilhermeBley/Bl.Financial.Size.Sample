using Bl.Financial.Size.Sample.Application.Repository;
using Bl.Financial.Size.Sample.Server.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<FinancialContext, SqlFinancialContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    await TryApplyMigrationsAsync(app.Services);
}

await app.RunAsync();


async Task TryApplyMigrationsAsync(IServiceProvider provider)
{
    var logger = provider.GetRequiredService<ILogger<Program>>();
    try
    {
        await using var scope = provider.CreateAsyncScope();

        var ctx  = scope.ServiceProvider.GetRequiredService<FinancialContext>();

        await ctx.Database.MigrateAsync();

        await ctx.Database.EnsureCreatedAsync();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Failed to apply migrations, check your sql connection string.");
    }
}
