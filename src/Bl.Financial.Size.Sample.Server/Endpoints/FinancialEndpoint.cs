using Bl.Financial.Size.Sample.Server.Model;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bl.Financial.Size.Sample.Server.Endpoints;

public static class FinancialEndpoint
{
    public static void MapFinancialEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapPost(
            "api/company",
            async (
                [FromBody] CreateCompanyModel model,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken = default
            ) =>
            {
                var response = 
                    await mediator.Send(
                        new Application.Command.CreateCompany.CreateCompanyRequest(
                            model.CompanyName,
                            model.Cnpj,
                            model.ServiceKind,
                            model.MonthlyBilling),
                        cancellationToken);

                return Results.Created(
                    $"api/company/{response.Id}",
                    new
                    {
                        Id = response.Id,
                    });

            })
            .WithValidation<CreateCompanyModel>();
    }
}
