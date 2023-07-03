using Microsoft.Extensions.Configuration;

namespace ConsoleApp1.Models;

public class Settings
{
    private readonly IConfiguration _configuration;

    public Settings(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public int WorkersCount => _configuration.GetValue<int>("WorkersCount");
    public int RunInterval => _configuration.GetValue<int>("RunInterval");
}