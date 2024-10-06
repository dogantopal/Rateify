using AutoFixture;
using StackExchange.Redis;

namespace NotificationService.IntegrationTests;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
{
    protected readonly IFixture Fixture;
    protected readonly IConnectionMultiplexer Redis;

    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        var options = ConfigurationOptions.Parse($"{factory.RedisContainer.Hostname}:{factory.RedisContainer.GetMappedPublicPort(6379)}");
        Redis = ConnectionMultiplexer.Connect(options);
        
        Fixture = new Fixture();
        Fixture.Customizations.Add(
            new RandomNumericSequenceGenerator(1,
                1000));
        Fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }
}