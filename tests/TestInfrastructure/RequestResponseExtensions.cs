using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FluentValidation.HttpExtensions.TestInfrastructure
{
    /// <summary>
    /// Extension methods for simplify API calls
    /// </summary>
    internal static class RequestResponseExtensions
    {
        public static StringContent ToStringContent(this object request)
        {
            return new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        }

        public static async Task<T> DeserializeAs<T>(this HttpResponseMessage response)
        {
            if (response == null) throw new ArgumentNullException(nameof(response));

            if (response.Content == null)
                return default;

            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonSerializer.Deserialize<T>(content);
        }
    }
}
