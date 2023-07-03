namespace WebApplication1.Extensions;
public static class CoreHostExtentions
{
    public static Task RunService(this IHostBuilder hostBuilder)
    {
        return hostBuilder.RunConsoleAsync();
    }
}
