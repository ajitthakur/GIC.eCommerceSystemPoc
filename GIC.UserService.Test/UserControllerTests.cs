using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

public class UserDto 
{
    public Guid Id { get; set; }    
    public string Name { get; set; }    
    public string Email { get; set; }
}

public class UserControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public UserControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient(); ;
    }

    [Fact]
    public async Task CreateUser_Should_ReturnCreatedUser()
    {
        // Arrange
        var newUser = new UserDto { Name = $"ajit{Guid.NewGuid()}", Email = $"ajit{Guid.NewGuid()}@testemail.com" };

        // Act
        var response = await _client.PostAsJsonAsync("http://localhost:5001/User", newUser);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdUser = await response.Content.ReadFromJsonAsync<UserDto>();
        createdUser.Should().NotBeNull();
        createdUser!.Name.Should().Be("ajit");
        createdUser.Id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public async Task GetUserById_Should_ReturnUser_WhenExists()
    {
        // First create a user
        var newUser = new UserDto { Name = $"ajit{Guid.NewGuid()}", Email = $"ajit{Guid.NewGuid()}@testemail.com" };
        var createResponse = await _client.PostAsJsonAsync("http://localhost:5001/User", newUser);
        var createdUser = await createResponse.Content.ReadFromJsonAsync<UserDto>();

        // Act: fetch by ID
        var getResponse = await _client.GetAsync($"http://localhost:5001/User/{createdUser!.Id}");

        // Assert
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var fetchedUser = await getResponse.Content.ReadFromJsonAsync<UserDto>();
        fetchedUser!.Name.Should().Be("ajit");
        fetchedUser.Id.Should().Be(createdUser.Id);
    }
}

