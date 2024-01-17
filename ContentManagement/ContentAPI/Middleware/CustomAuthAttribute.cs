namespace ContentAPI.Middleware
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class CustomAuthAttribute : Attribute
    {
        public CustomAuthAttribute() { }
    }
}
