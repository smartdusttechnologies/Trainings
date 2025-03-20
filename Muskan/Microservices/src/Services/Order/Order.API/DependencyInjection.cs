namespace Ordering.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAPIService(this IServiceCollection service)
        {
            //service.AddCarter();
            return service;
        }
        public static WebApplication UseAPIService (this WebApplication app)
        {
            //app.UseCarter();
            return app;
        }
    }
}
