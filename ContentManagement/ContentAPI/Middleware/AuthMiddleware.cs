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
            // Check for token
            if (!context.Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return;
            }
            string accessToken = authHeader.ToString().Split(' ')[1];

            // Check if request works (AKA is token valid)
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var reponse = await _httpClient.GetAsync("https://authentication-api-kawa-foundation-app-dev.apps.ocp6-inholland.joran-bergfeld.com/authentication/userinfo", context.RequestAborted);

            if (!reponse.IsSuccessStatusCode)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            // Check for CMS (content manager) role
            var jsonContent = await reponse.Content.ReadAsStringAsync();
            JArray roles = JsonConvert.DeserializeObject<dynamic>(jsonContent).roles;
            List<string> list = (List<string>)roles.ToObject(typeof(List<string>));
            if (!list.Contains("cms"))
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
