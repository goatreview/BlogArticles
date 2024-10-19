using System.Net;
using System.Text;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ExposeCommand.Tests;

public class UnitTest1
{
    private readonly HttpClient _client;

    public UnitTest1()
    {
        var factory = new MyWebApplicationFactory();
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task ShouldGetGoat()
    {
        var response = await _client.GetAsync("/goats/2fe43769-1c63-46c7-8449-60a490462411");
        response.StatusCode.Should().Be(HttpStatusCode.OK, await response.Content.ReadAsStringAsync());
    }
    
    [Fact]
    public async Task ShouldReturnBadRequest()
    {
        var response = await _client.GetAsync("/goats/1");
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest, await response.Content.ReadAsStringAsync());
    }
    
    [Fact]
    public async Task ShouldCreateGoat()
    {
        var command = "{\"Name\":\"Goat\",\"BirthDate\":\"2024-10-16T09:19:47.6898565+02:00\",\"Address\":{\"Street\":\"Street\",\"City\":\"City\"}}";
        var response = await _client.PostAsync("/goats", new StringContent(command, Encoding.UTF8, "application/json"));
        response.StatusCode.Should().Be(HttpStatusCode.Created, await response.Content.ReadAsStringAsync());
    }
    
    [Fact]
    public async Task ShouldReturnBadRequest_WhenBirthDateIsInvalid()
    {
        var command = "{\"Name\":\"Goat\",\"BirthDate\":\"999-999-999T09:19:47.6898565+02:00\",\"Address\":{\"Street\":\"Street\",\"City\":\"City\"}}";
        var response = await _client.PostAsync("/goats", new StringContent(command, Encoding.UTF8, "application/json"));
        response.StatusCode.Should().Be(HttpStatusCode.Created, await response.Content.ReadAsStringAsync());
    }
    
    [Fact]
    public async Task ShouldReturnBadRequest_WhenBirthDateIsInvalid_ClassicApi()
    {
        var command = "{\"Name\":\"Goat\",\"BirthDate\":\"999-999-999T09:19:47.6898565+02:00\",\"Address\":{\"Street\":\"Street\",\"City\":\"City\"}}";
        var response = await _client.PostAsync("/goatsclassic", new StringContent(command, Encoding.UTF8, "application/json"));
        response.StatusCode.Should().Be(HttpStatusCode.Created, await response.Content.ReadAsStringAsync());
    }
    
    [Fact]
    public async Task ShouldReturnBadRequest_WhenBirthDateIsInvalid_WithInput()
    {
        var command = "{\"Name\":\"Goat\",\"BirthDate\":\"999-999-999T09:19:47.6898565+02:00\",\"Address\":{\"Street\":\"Street\",\"City\":\"City\"}}";
        var response = await _client.PostAsync("/goatswithinput", new StringContent(command, Encoding.UTF8, "application/json"));
        response.StatusCode.Should().Be(HttpStatusCode.Created, await response.Content.ReadAsStringAsync());
    }
}


public class MyWebApplicationFactory : WebApplicationFactory<Program>;
