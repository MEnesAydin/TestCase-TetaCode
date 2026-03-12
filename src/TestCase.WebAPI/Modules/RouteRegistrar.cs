namespace TestCase.WebAPI.Modules;

public static class RouteRegistrar
{
    public static void RegisterRoutes(this WebApplication app)
    {
        app.MapAuth();
    }
}