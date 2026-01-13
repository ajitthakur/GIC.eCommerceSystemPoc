//using System;
//using System.Net;
//using System.Net.Http.Json;
//using System.Threading.Tasks;
//using FluentAssertions;
//using Microsoft.AspNetCore.Mvc.Testing;
//using Xunit;

//public class UsersControllerTests : IClassFixture<WebApplicationFactory<Program>>
//{
//    private readonly HttpClient _client;

//    public UsersControllerTests(WebApplicationFactory<Program> factory)
//    {
//        _client = factory.CreateClient();
//    }

//    [Fact]
//    public async Task CreateUser_ShouldReturnCreatedUser()
//    {
//        // Arrange
//        var newUser = new { Username = "ajit", Password = "pass123" };

//        // Act
//        var response = await _client.PostAsJsonAsync("/api/users", newUser);

//        // Assert
//        response.StatusCode.Should().Be(HttpStatusCode.Created);

//        var createdUser = await response.Content.ReadFromJsonAsync<UserDto>();
//        createdUser.Should().NotBeNull();
//        createdUser!.Username.Should().Be("ajit");
//        createdUser.Id.Should().NotBe(Guid.Empty);
//    }

//    [Fact]
//    public async Task GetUserById_ShouldReturnUser_WhenExists()
//    {
//        // First create a user
//        var newUser = new { Username = "ajit", Password = "pass123" };
//        var createResponse = await _client.PostAsJsonAsync("/api/users", newUser);
//        var createdUser = await createResponse.Content.ReadFromJsonAsync<UserDto>();

//        // Act: fetch by ID
//        var getResponse = await _client.GetAsync($"/api/users/{createdUser!.Id}");

//        // Assert
//        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
//        var fetchedUser = await getResponse.Content.ReadFromJsonAsync<UserDto>();
//        fetchedUser!.Username.Should().Be("ajit");
//        fetchedUser.Id.Should().Be(createdUser.Id);
//    }

//    [Fact]
//    public async Task GetUserById_ShouldReturnNotFound_WhenUserDoesNotExist()
//    {
//        var randomId = Guid.NewGuid();

//        var response = await _client.GetAsync($"/api/users/{randomId}");

//        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
//    }
//}
