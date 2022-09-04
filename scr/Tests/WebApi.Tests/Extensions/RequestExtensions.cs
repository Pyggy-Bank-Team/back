using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace WebApi.Tests.Extensions
{
    internal static class RequestExtensions
    {
        public static StringContent ToStringContent<T>(this T request)
            => new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
    }
}