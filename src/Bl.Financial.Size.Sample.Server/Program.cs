using Bl.Financial.Size.Sample.Application.Repository;
using Bl.Financial.Size.Sample.Server.Endpoints;
using Bl.Financial.Size.Sample.Server.Repository;
using Bl.Financial.Size.Sample.Server.Seed;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<FinancialContext, SqlFinancialContext>();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(
        typeof(Bl.Financial.Size.Sample.Application.Command.CreateCompany.CreateCompanyHandler).Assembly);
});

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
    await app.Services.TryAddFakeCompany();
}

app.UseMiddleware<CoreExceptionMiddleware>();

app.MapFinancialEndpoints();

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
