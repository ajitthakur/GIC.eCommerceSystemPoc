using System;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

public class UserControllerTests 
{
    private readonly HttpClient _client;

    public UserControllerTests()
    {        
        _client = new HttpClient
        {
            BaseAddress = new System.Uri("https://localhost:5001")//User API            
        };
    }


    [Fact]
    public async Task CreateUser_Should_Return_Created_User()
    {
        // Arrange
        var newUser = new UserDto { Name = $"ajit{Guid.NewGuid()}", Email = $"ajit{Guid.NewGuid()}@testemail.com" };

        // Act
        var response = await _client.PostAsJsonAsync("User", newUser);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdUser = await response.Content.ReadFromJsonAsync<UserDto>();
        createdUser.Should().NotBeNull();
        createdUser!.Name.Should().Be(newUser.Name);
        createdUser.Id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public async Task GetUserById_Should_Return_User_If_Exists()
    {
        // First create a user
        var newUser = new UserDto { Name = $"ajit{Guid.NewGuid()}", Email = $"ajit{Guid.NewGuid()}@testemail.com" };
        var createResponse = await _client.PostAsJsonAsync("User", newUser);
        var createdUser = await createResponse.Content.ReadFromJsonAsync<UserDto>();

        // Act: fetch by ID
        var getResponse = await _client.GetAsync($"User/{createdUser!.Id}");

        // Assert
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var fetchedUserJson = await getResponse.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(fetchedUserJson);
        var root = doc.RootElement;        
        var id = root.GetProperty("userDetail").GetProperty("result").GetProperty("id");                   
        id.ToString().Should().Be(createdUser.Id.ToString());
    }
}

public class UserDetailPartialDto
{
    public UserDetailDto userDetail { get; set; }
}

public class UserDetailDto
{
    public UserDto result { get; set; }
}

public class UserDtoPartial
{
    public string id { get; set; }
    public string name { get; set; }
    public string email { get; set; }
}

public class UserDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}
