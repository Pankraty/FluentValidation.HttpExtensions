using System.Net.Http;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.TestHost;
using TestApplication;

namespace FluentValidation.HttpExtensions.Unit
{
    public abstract class BaseController
    {
        protected readonly HttpClient _apiClient;
        protected static readonly TestServer _server = new TestServer(WebHost.CreateDefaultBuilder<Startup>(new string[0]));

        public BaseController()
        {
            _apiClient = _server.CreateClient();
        }
    }
}
