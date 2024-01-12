namespace ContentAPI.DAL
{
    public abstract class BaseRepo
    {
        protected readonly string connectionString;
        public BaseRepo(IConfiguration config)
        {
            IConfiguration _config = config;
            connectionString = Environment.GetEnvironmentVariable("AZURE_CONNECTION") ?? _config["AzureEmulatorAccess"];
        }
    }
}
