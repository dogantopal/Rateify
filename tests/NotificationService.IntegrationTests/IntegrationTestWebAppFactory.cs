using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.Redis;

namespace NotificationService.IntegrationTests;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    public readonly RedisContainer RedisContainer = new RedisBuilder()
        .WithImage("redis:7.0")
        .Build();

    public Task InitializeAsync()
    {
        return RedisContainer.StartAsync();
    }

    public new Task DisposeAsync()
    {
        return RedisContainer.StopAsync();
    }
}