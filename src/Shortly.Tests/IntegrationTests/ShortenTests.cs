using NUnit.Framework;
using FluentAssertions;

namespace Shortly.Tests;

[TestFixture]
public class ShortenTests {

    [TestCase("")]
    [TestCase(" ")]
    [TestCase("www.example.com")]
    [TestCase("/about-us")]
    public async Task ReturnsBadRequest_When_UrlIsInvalid(string url)
    {
        // Arrange
        using var app = new CustomWebApplicationFactory();
        var httpClient = app.CreateClient();

        // Act
        var response = await httpClient.PostAsJsonAsync("/shorten", url);
        var responseText = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        responseText.Should().Contain($"The url {url} provided is not valid");
    }

    [TestCase("https://www.example.com")]
    public async Task ReturnsHash_When_UrlIsValid(string url)
    {
        // Arrange
        using var app = new CustomWebApplicationFactory();
        var httpClient = app.CreateClient();

        // Act
        var response = await httpClient.PostAsJsonAsync("/shorten", url);
        var responseText = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        responseText.Should().Contain($"lw", "the salt is always the same and it should be hashing id 1"); 
    }
}