using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FluentValidation.HttpExtensions.Internal
{
    /// <summary>
    /// Wrapper around <see cref="DefaultProblemDetailsFactory"/> supporting custom HTTP status codes,
    /// other than HadRequest.
    /// </summary>
    internal class CustomProblemDetailsFactory : ProblemDetailsFactory
    {
        private readonly ProblemDetailsFactory _internalProblemDetailsFactory;
        private readonly HttpErrorPriorityProvider _httpErrorPriorityProvider;

        public CustomProblemDetailsFactory(
            ProblemDetailsFactory internalProblemDetailsFactory,
            HttpErrorPriorityProvider httpErrorPriorityProvider)
        {
            _internalProblemDetailsFactory = internalProblemDetailsFactory;
            _httpErrorPriorityProvider = httpErrorPriorityProvider;
        }

        public override ProblemDetails CreateProblemDetails(HttpContext httpContext, int? statusCode = null, string title = null,
            string type = null, string detail = null, string instance = null)
        {
            return _internalProblemDetailsFactory.CreateProblemDetails(httpContext, statusCode, title, type, detail, instance);
        }

        public override ValidationProblemDetails CreateValidationProblemDetails(HttpContext httpContext,
            ModelStateDictionary modelStateDictionary, int? statusCode = null, string title = null, string type = null,
            string detail = null, string instance = null)
        {
            foreach (var errorCode in _httpErrorPriorityProvider.GetSupportedErrorCodes())
            {
                var key = $"{ErrorStatusConst.Prefix}{errorCode}";
                if (modelStateDictionary.TryGetValue(key, out var errors))
                {
                    var trimmedModelStateDictionary = ToModelStateDictionary(errors);
                    statusCode = errorCode;
                    return CreateValidationProblemDetails(trimmedModelStateDictionary);
                }
            }

            return CreateValidationProblemDetails(modelStateDictionary);

            ValidationProblemDetails CreateValidationProblemDetails(ModelStateDictionary partialModelStateDictionary) =>
                _internalProblemDetailsFactory.CreateValidationProblemDetails(httpContext, partialModelStateDictionary, statusCode, title, type, detail, instance);
        }

        private ModelStateDictionary ToModelStateDictionary(ModelStateEntry errors)
        {
            var result = new ModelStateDictionary();
            foreach (ModelError error in errors.Errors)
            {
                result.AddModelError("", error.ErrorMessage);
            }

            return result;
        }
    }
}
