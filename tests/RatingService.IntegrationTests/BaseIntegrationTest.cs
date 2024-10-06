using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using RatingService.Data;

namespace RatingService.IntegrationTests;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
{
    protected readonly RatingDbContext DbContext;
    protected readonly IFixture Fixture;

    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        var scope = factory.Services.CreateScope();

        DbContext = scope.ServiceProvider.GetRequiredService<RatingDbContext>();
        
        Fixture = new Fixture();
        Fixture.Customizations.Add(
            new RandomNumericSequenceGenerator(1,
                1000));
        Fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }
}