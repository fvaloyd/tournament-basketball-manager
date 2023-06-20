using Application.Common.Exceptions;
using Domain.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.API.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureExceptionHandlerMiddlware(this WebApplication app)
    {
        app.UseExceptionHandler(cfg =>
        {
            cfg.Run(async ctx =>
            {
                var ctxFeature = ctx.Features.Get<IExceptionHandlerFeature>();
                if (ctxFeature is not null)
                {
                    switch (ctxFeature.Error)
                    {
                        case NotFoundException:
                            await Results.NotFound(new ProblemDetails() { Detail = ctxFeature.Error.Message }).ExecuteAsync(ctx);
                            break;
                        case BadRequestException:
                            await Results.BadRequest(new ProblemDetails() { Detail = ctxFeature.Error.Message}).ExecuteAsync(ctx);
                            break;
                        case ValidationException:
                            var exception = (ValidationException)ctxFeature.Error;
                            await Results.UnprocessableEntity(new ValidationProblemDetails(exception.Errors)).ExecuteAsync(ctx);
                            break;
                        default:
                            await Results.Problem().ExecuteAsync(ctx);
                            break;
                    }
                }
            });
        });
    }
}