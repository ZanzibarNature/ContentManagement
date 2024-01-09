using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
namespace ContentAPI.Middleware
{
    public class AuthMiddleware : IMiddleware
    {
        private readonly HttpClient _httpClient;

        public AuthMiddleware()
        {
            _httpClient = new HttpClient();
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var accessToken = string.Empty;

            if (context.Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                accessToken = authHeader.ToString().Split(' ')[1];
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var reponse = await _httpClient.GetAsync("https://authentication-api-kawa-foundation-app-dev.apps.ocp6-inholland.joran-bergfeld.com/authentication/userinfo", context.RequestAborted);

            if (!reponse.IsSuccessStatusCode)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            var jsonContent = await reponse.Content.ReadAsStringAsync();
            JArray roles = JsonConvert.DeserializeObject<dynamic>(jsonContent).roles;
            if (!roles.Contains("cms"))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            // Call the next middleware in the pipeline
            await next(context);
        }

        public void Configure(IApplicationBuilder app)
        {
            // You can configure the middleware here if needed
        }
    }
}
