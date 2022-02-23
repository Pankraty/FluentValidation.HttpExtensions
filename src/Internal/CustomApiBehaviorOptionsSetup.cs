using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FluentValidation.HttpExtensions.Internal
{
    /// <summary>
    /// Wrapper around <see cref="ApiBehaviorOptionsSetup"/> supporting custom HTTP codes, other than BadRequest.
    /// </summary>
    internal class CustomApiBehaviorOptionsSetup : IConfigureOptions<ApiBehaviorOptions>
    {
        private readonly IConfigureOptions<ApiBehaviorOptions> _internalConfigureOptions;
        private ProblemDetailsFactory _problemDetailsFactory;
        private Func<ActionContext,IActionResult> _internalInvalidModelStateResponseFactory;

        public CustomApiBehaviorOptionsSetup(IConfigureOptions<ApiBehaviorOptions> internalConfigureOptions)
        {
            _internalConfigureOptions = internalConfigureOptions;
        }

        public void Configure(ApiBehaviorOptions options)
        {
            _internalConfigureOptions.Configure(options);
            _internalInvalidModelStateResponseFactory = options.InvalidModelStateResponseFactory;
            options.InvalidModelStateResponseFactory = context =>
            {
                _problemDetailsFactory ??= context.HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
                return ProblemDetailsInvalidModelStateResponse(_problemDetailsFactory, context);
            };
        }

        private IActionResult ProblemDetailsInvalidModelStateResponse(ProblemDetailsFactory problemDetailsFactory, ActionContext context)
        {
            var problemDetails = problemDetailsFactory.CreateValidationProblemDetails(context.HttpContext, context.ModelState);
            if (problemDetails.Status == (int)HttpStatusCode.BadRequest)
            {
                return _internalInvalidModelStateResponseFactory(context);
            }

            return new ObjectResult(problemDetails)
            {
                StatusCode = problemDetails.Status,
                ContentTypes =
                {
                    "application/problem+json",
                    "application/problem+xml",
                }
            };
        }
    }
}
