using AutoFixture;
using Contracts;
using FluentAssertions;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using RatingService.Data.Entities;
using RatingService.Models;

namespace RatingService.IntegrationTests;

public class RatingServiceTests : BaseIntegrationTest
{
    private readonly Services.RatingService _ratingService;
    private readonly Mock<IPublishEndpoint> _publishEndpointMock;

    public RatingServiceTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
        var loggerMock = new Mock<ILogger<Services.RatingService>>();
        _publishEndpointMock = new Mock<IPublishEndpoint>();

        _ratingService = new Services.RatingService(DbContext, loggerMock.Object, _publishEndpointMock.Object);
    }

    [Fact]
    public async Task CreateRatingAsync_Should_Create_Rating_Successfully()
    {
        // Arrange
        var providerId = Fixture.Create<long>();
        await CreateProviderAsync(providerId);

        var createRatingRequest = GetCreateRatingRequest(providerId);

        var totalRatingCount = await DbContext.Ratings.CountAsync();

        // Act
        var act = () => _ratingService.CreateRatingAsync(createRatingRequest);

        // Assert
        await act.Should().NotThrowAsync();
        var newTotalRatingCount = await DbContext.Ratings.CountAsync();
        newTotalRatingCount.Should().Be(totalRatingCount + 1);

        _publishEndpointMock.Verify(pe => pe.Publish(It.IsAny<RatingCreated>(), default), Times.Once);
    }

    [Fact]
    public async Task CreateRatingAsync_Should_Throw_Exception_When_SaveChanges_Fails()
    {
        // Arrange
        var providerId = Fixture.Create<long>();
        await CreateProviderAsync(providerId);

        var createRatingRequest = GetCreateRatingRequest(providerId);

        await DbContext.DisposeAsync();

        // Act
        var act = async () => await _ratingService.CreateRatingAsync(createRatingRequest);

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }

    private async Task CreateProviderAsync(long providerId)
    {
        var provider = Fixture
            .Build<Provider>()
            .Without(x => x.Ratings)
            .With(x => x.Id, providerId)
            .Create();

        await DbContext.Providers.AddAsync(provider);
        await DbContext.SaveChangesAsync();
    }

    private CreateRatingRequestModel GetCreateRatingRequest(long providerId)
    {
        return Fixture
            .Build<CreateRatingRequestModel>()
            .With(x => x.Point, 5)
            .With(x => x.CustomerId, 5)
            .With(x => x.ProviderId, providerId)
            .Create();
    }
}