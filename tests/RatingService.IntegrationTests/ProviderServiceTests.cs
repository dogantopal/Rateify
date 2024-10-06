using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using RatingService.Data.Entities;
using RatingService.Models;
using RatingService.Services;

namespace RatingService.IntegrationTests;

public class ProviderServiceTests : BaseIntegrationTest
{
    private readonly ProviderService _providerService;

    public ProviderServiceTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
         var loggerMock = new Mock<ILogger<ProviderService>>();
        _providerService = new ProviderService(DbContext, loggerMock.Object);
    }

    [Fact]
    public async Task GetAverageRatingByIdAsync_Should_Return_AverageRating_When_Provider_Exists()
    {
        // Arrange
        var provider = Fixture
            .Build<Provider>()
            .Without(x => x.Ratings)
            .Create();
        await DbContext.Providers.AddAsync(provider);
        await DbContext.SaveChangesAsync();

        var responseModel = new GetAverageRatingResponseModel(provider.Id, provider.Name, provider.AverageRating);

        // Act
        var result = await _providerService.GetAverageRatingByIdAsync(provider.Id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(responseModel);
    }
    
    [Fact]
    public async Task GetAverageRatingByIdAsync_Should_Throw_Exception_When_Provider_Not_Found()
    {
        // Arrange
        const int nonExistProviderId = 9999;

        // Act
        var act = async () => await _providerService.GetAverageRatingByIdAsync(nonExistProviderId);

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
    
    [Fact]
    public async Task UpdateAverageRatingAsync_Should_Update_AverageRating_When_Provider_Exists()
    {
        // Arrange
        var provider = new Provider
        {
            Id = 999,
            Name = "Test Provider",
            AverageRating = 4.0,
            TotalRating = 20.0,
            RatingCount = 5
        };

        await DbContext.Providers.AddAsync(provider);
        await DbContext.SaveChangesAsync();

        const double newRatingPoint = 5.0;

        // Act
        await _providerService.UpdateAverageRatingAsync(provider.Id, newRatingPoint);

        // Assert
        var updatedProvider = await DbContext.Providers.FindAsync(provider.Id);
        updatedProvider.Should().NotBeNull();
        updatedProvider!.RatingCount.Should().Be(6);
        updatedProvider.TotalRating.Should().Be(25.0);
        updatedProvider.AverageRating.Should().BeApproximately(4.17, 0.01); // 25 / 6
    }
    
    [Fact]
    public async Task UpdateAverageRatingAsync_Should_Throw_Exception_When_Provider_Not_Found()
    {
        // Arrange
        const int nonExistentProviderId = 9999;
        const double newRatingPoint = 4.5;

        // Act
        var act = async () => await _providerService.UpdateAverageRatingAsync(nonExistentProviderId, newRatingPoint);

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
}