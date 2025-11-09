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

        builder.MapPost(
            "api/nf",
            async (
                [FromBody] CreateNfModel model,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken = default
            ) =>
            {
                var response =
                    await mediator.Send(
                        new Application.Command.CreateNf.CreateNfRequest(
                            model.Number,
                            model.CompanyId,
                            model.Value,
                            model.DueDate),
                        cancellationToken);

                return Results.Created(
                    $"api/nf/{response.Id}",
                    new
                    {
                        Id = response.Id,
                    });

            })
            .WithValidation<CreateNfModel>();

        builder.MapGet(
            "api/nf",
            async (
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken = default
            ) =>
            {
                var response =
                    await mediator.Send(
                        new Application.Command.GetNf.GetNfRequest(),
                        cancellationToken);

                if (response.Results.Length == 0) 
                    return Results.NoContent();

                return Results.Ok(new
                {
                    data = response.Results
                });

            });

        builder.MapGet(
            "api/nf/{id}",
            async (
                long id,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken = default
            ) =>
            {
                var response =
                    await mediator.Send(
                        new Application.Command.GetNf.GetNfRequest(id),
                        cancellationToken);

                if (response.Results.Length == 0) 
                    return Results.NoContent();

                return Results.Ok(new
                {
                    data = response.Results[0]
                });

            });

        builder.MapGet(
            "api/company",
            async (
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken = default
            ) =>
            {
                var response =
                    await mediator.Send(
                        new Application.Command.GetCompany.GetCompanyRequest(),
                        cancellationToken);

                if (response.Results.Length == 0) 
                    return Results.NoContent();

                return Results.Ok(new
                {
                    data = response.Results
                });

            });

        builder.MapGet(
            "api/company/{id}",
            async (
                long id,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken = default
            ) =>
            {
                var response =
                    await mediator.Send(
                        new Application.Command.GetCompany.GetCompanyRequest(id),
                        cancellationToken);

                if (response.Results.Length == 0) 
                    return Results.NoContent();

                return Results.Ok(new
                {
                    data = response.Results[0]
                });

            });
    }
}
