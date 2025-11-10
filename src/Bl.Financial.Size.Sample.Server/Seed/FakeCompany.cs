using Bl.Financial.Size.Sample.Application.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bl.Financial.Size.Sample.Server.Seed;

public static class FakeCompany
{

    public async static Task TryAddFakeCompany(
        this IServiceProvider provider,
        CancellationToken cancellationToken = default)
    {
        var logger = provider.GetRequiredService<ILogger<Program>>();

        try
        {
            await using var scope = provider.CreateAsyncScope();

            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var companyRequest = new Application.Command.CreateCompany.CreateCompanyRequest(
                    CompanyName: "Nome da Empresa",
                    Cnpj: "00.000.000/0000-00",
                    ServiceKind: "Service",
                    MonthlyBilling: 100_000);

            if (await ContainsCompany(companyRequest.Cnpj, scope, cancellationToken))
            {
                return;
            }

            var companyResult = await mediator.Send(companyRequest, cancellationToken);

            var lastMonthDay = DateOnly.FromDateTime(new DateTime(
                DateTime.UtcNow.AddMonths(1).Year,
                DateTime.UtcNow.AddMonths(1).Month,
                1)
                .AddDays(-1));

            var nf1 = await mediator.Send(
                new Application.Command.CreateNf.CreateNfRequest(
                    Number: 1,
                    CompanyId: companyResult.Id,
                    Value: 850.80m,
                    DueDate: lastMonthDay),
                cancellationToken);

            var nf2 = await mediator.Send(
                new Application.Command.CreateNf.CreateNfRequest(
                    Number: 10,
                    CompanyId: companyResult.Id,
                    Value: 5000,
                    DueDate: lastMonthDay),
                cancellationToken);

            var nf3 = await mediator.Send(
                new Application.Command.CreateNf.CreateNfRequest(
                    Number: 11,
                    CompanyId: companyResult.Id,
                    Value: 7000,
                    DueDate: lastMonthDay),
                cancellationToken);

            var nfsResults = new Application.Command.CreateNf.CreateNfResponse[] { nf1, nf2, nf3 };

            foreach (var nfResult in nfsResults)
            {
                await mediator.Send(
                    new Application.Command.AddCartAnticipation.AddCartAnticipationRequest(nfResult.Id),
                    cancellationToken);
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to create fake company.");
        }
    }

    private static Task<bool> ContainsCompany(
        string cnpj,
        IServiceScope scope,
        CancellationToken cancellationToken = default)
    {
        var ctx = scope.ServiceProvider.GetRequiredService<FinancialContext>();

        var parsedCnpj = string.Concat(
            cnpj.Where(char.IsNumber));

        return ctx.Companies
            .AsNoTracking()
            .Where(x => x.Cnpj == parsedCnpj)
            .AnyAsync(cancellationToken);
    }
}
