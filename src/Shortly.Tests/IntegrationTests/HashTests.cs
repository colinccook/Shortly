using NUnit.Framework;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Shortly.Tests;

[TestFixture]
public class HashTests {

    [Test]
    public async Task RedirectsToRoot_When_HashIsInvalid()
    {
        // Arrange
        using var app = new CustomWebApplicationFactory();
        var httpClient = app.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false});

        // Act
        var response = await httpClient.GetAsync("/nonexistent-hash-code");

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Moved);
        response.Headers.Location.Should().Be("/");
    }

    [Test]
    public async Task RedirectsToShortenedUrl_When_HashMatches()
    {
        // Arrange
        using var app = new CustomWebApplicationFactory();
        var httpClient = app.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false});
        var expectedUrl = "https://www.example.com";

        var postResponse = await httpClient.PostAsJsonAsync("/shorten", expectedUrl);
        var jsonHash = await postResponse.Content.ReadAsStringAsync();
        var hash = System.Text.Json.JsonSerializer.Deserialize<string>(jsonHash);

        // Act
        var response = await httpClient.GetAsync($"/{hash}");
        var responseText = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Moved);
        response.Headers.Location.Should().Be(expectedUrl);
    }

    [Test]
    public async Task CreatesUniqueHashes_When_SameUrlIsGiven()
    {
        // Arrange
        using var app = new CustomWebApplicationFactory();
        var httpClient = app.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false});
        var url = "https://www.same-url.com";

        // Act
        var postResponse1 = await httpClient.PostAsJsonAsync("/shorten", url);
        var hash1 = await postResponse1.Content.ReadAsStringAsync();

        var postResponse2 = await httpClient.PostAsJsonAsync("/shorten", url);
        var hash2 = await postResponse2.Content.ReadAsStringAsync();

        // Assert
        hash1.Should().NotBe(hash2);
    }
}