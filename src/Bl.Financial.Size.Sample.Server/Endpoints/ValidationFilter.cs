using System.ComponentModel.DataAnnotations;

namespace Bl.Financial.Size.Sample.Server.Endpoints;

public static class ValidationExtensions
{
    public static RouteHandlerBuilder WithValidation<T>(this RouteHandlerBuilder builder)
        where T : class
    {
        return builder.AddEndpointFilter<ValidationFilter<T>>();
    }

    private class ValidationFilter<T> : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(
            EndpointFilterInvocationContext context,
            EndpointFilterDelegate next)
        {
            var arg = context.Arguments
                .OfType<T>()
                .FirstOrDefault();

            if (arg is null)
            {
                return Results.BadRequest("Model binding failed");
            }

            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(arg,
                new ValidationContext(arg), validationResults, true);

            if (!isValid)
            {
                return Results.BadRequest(validationResults.Select(v => new
                {
                    v.ErrorMessage,
                    v.MemberNames
                }));
            }

            return await next(context);
        }
    }
}
