using FluentAssertions;
using NotificationService.Infrastructure;
using StackExchange.Redis;

namespace NotificationService.IntegrationTests;

public class RedisServiceTests : BaseIntegrationTest
{
    private readonly RedisService _redisService;

    public RedisServiceTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
        var options = ConfigurationOptions.Parse($"{factory.RedisContainer.Hostname}:{factory.RedisContainer.GetMappedPublicPort(6379)}");
        IConnectionMultiplexer redis = ConnectionMultiplexer.Connect(options);

        _redisService = new RedisService(redis);
    }
    
    [Fact]
    public async Task UpdateValueAsync_Should_SetValue_And_Return_UpdatedValue()
    {
        // Arrange
        const string key = "test-key";
        var value = new TestModel { Id = 1, Name = "Test Name" };

        // Act
        var updatedValue = await _redisService.UpdateValueAsync(key, value);

        // Assert
        updatedValue.Should().BeEquivalentTo(value);
    }

    [Fact]
    public async Task GetValueAsync_Should_Return_CorrectValue_If_KeyExists()
    {
        // Arrange
        const string key = "existing-key";
        var value = new TestModel { Id = 2, Name = "Existing Test" };
        await _redisService.UpdateValueAsync(key, value);

        // Act
        var storedValue = await _redisService.GetValueAsync<TestModel>(key);

        // Assert
        storedValue.Should().BeEquivalentTo(value);
    }

    [Fact]
    public async Task GetValueAsync_Should_Return_Default_If_KeyDoesNotExist()
    {
        // Act
        var storedValue = await _redisService.GetValueAsync<TestModel>("non-existing-key");

        // Assert
        storedValue.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_Should_Remove_Key_Successfully()
    {
        // Arrange
        const string key = "key-to-delete";
        var value = new TestModel { Id = 3, Name = "Delete Me" };
        await _redisService.UpdateValueAsync(key, value);

        // Act
        var deleteResult = await _redisService.DeleteAsync(key);
        var storedValue = await _redisService.GetValueAsync<TestModel>(key);

        // Assert
        deleteResult.Should().BeTrue();
        storedValue.Should().BeNull();
    }

    private class TestModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}