using System.Collections.Generic;
using System.Net;

namespace FluentValidation.HttpExtensions.Internal
{
    internal class HttpErrorPriorityProvider
    {
        private static readonly int[] _supportedErrorCodes =
        {
            (int) HttpStatusCode.Forbidden,
            (int) HttpStatusCode.NotFound,
            (int) HttpStatusCode.MethodNotAllowed,
            (int) HttpStatusCode.NotAcceptable,
            (int) HttpStatusCode.Conflict,
            (int) HttpStatusCode.Gone,
            (int) HttpStatusCode.Locked,
        };

        public IEnumerable<int> GetSupportedErrorCodes() => _supportedErrorCodes;
    }
}
