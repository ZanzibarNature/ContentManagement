using Newtonsoft.Json;
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
            var hasCustomAuthAttribute = context.GetEndpoint()?.Metadata.GetMetadata<CustomAuthAttribute>() != null;
            if (hasCustomAuthAttribute)
            {
                // Check for token
                if (!context.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync("No bearer token found");
                    return;
                }
                string accessToken = authHeader.ToString().Split(' ')[1];

                // Check if request to security microservice works (AKA is token valid)
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var reponse = await _httpClient.GetAsync("https://authentication-api-kawa-foundation-app-dev.apps.ocp6-inholland.joran-bergfeld.com/authentication/userinfo", context.RequestAborted);

                if (!reponse.IsSuccessStatusCode)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Token is not valid");
                    return;
                }

                // Check for CMS (content manager) role
                var jsonContent = await reponse.Content.ReadAsStringAsync();
                List<string> roles = (List<string>)JsonConvert.DeserializeObject<dynamic>(jsonContent).roles.ToObject(typeof(List<string>));
                if (!roles.Contains("cms"))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("User does not have permission to access this endpoint");
                    return;
                }
            }

            // Call the next middleware in the pipeline
            await next(context);
        }

        public void Configure(IApplicationBuilder app)
        {
            //app.UseMiddleware<AuthMiddleware>();
        }
    }
}
