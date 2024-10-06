using Contracts;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using NotificationService.Infrastructure;
using NotificationService.Models;

namespace NotificationService.IntegrationTests;

public class NotificationServiceTests : BaseIntegrationTest
{
    private readonly Services.NotificationService _notificationService;
    private readonly Mock<IRedisService> _redisServiceMock;


    public NotificationServiceTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
        _redisServiceMock = new Mock<IRedisService>();

        var inMemorySettings = new Dictionary<string, string>
        {
            { "NotificationStackRedisKey", "NotificationStackRedisKey" }
        };

        IConfiguration config = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();

        _notificationService = new Services.NotificationService(config, _redisServiceMock.Object);
    }

    [Fact]
    public async Task GetNotificationsAsync_Should_Return_EmptyList_When_No_Data_In_Redis()
    {
        // Arrange
        const string redisKey = "NotificationStackRedisKey";
        _redisServiceMock.Setup(r => r.GetValueAsync<List<RatingCreated>>(redisKey)).ReturnsAsync((List<RatingCreated>)null!);

        // Act
        var result = await _notificationService.GetNotificationsAsync();

        // Assert
        result.Should().BeEmpty();
        _redisServiceMock.Verify(r => r.DeleteAsync(redisKey), Times.Never);
    }

    [Fact]
    public async Task GetNotificationsAsync_Should_Return_Notifications_When_Data_Exists_In_Redis()
    {
        // Arrange
        const string redisKey = "NotificationStackRedisKey";
        var ratingCreatedList = new List<RatingCreated>
        {
            new() { Point = 5, ProviderId = 456 },
            new() { Point = 4, ProviderId = 457 }
        };

        var notifications = new List<Notification>
        {
            new(5, 456),
            new(4, 457)
        };

        _redisServiceMock.Setup(r => r.GetValueAsync<List<RatingCreated>>(redisKey)).ReturnsAsync(ratingCreatedList);

        // Act
        var result = await _notificationService.GetNotificationsAsync();

        // Assert
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(notifications);
        _redisServiceMock.Verify(r => r.DeleteAsync(redisKey), Times.Once);
    }
}