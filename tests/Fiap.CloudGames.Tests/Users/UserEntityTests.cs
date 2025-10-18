using System;
using Fiap.CloudGames.Domain.Users.Entities;
using Fiap.CloudGames.Domain.Users.Enums;
using Xunit;

namespace Fiap.CloudGames.Tests.Users;

public class UserEntityTests
{
    [Fact]
    public void Create_ValidUser_SetsProperties()
    {
        var user = User.Create("Name", "name@example.com", "Strong@Password123", UserRole.User, default);
        Assert.NotEqual(Guid.Empty, user.Id);
        Assert.Equal("Name", user.Name);
        Assert.True(user.VerifyPassword("Strong@Password123"));
        Assert.True(user.IsActive);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Create_InvalidName_Throws(string name)
    {
        Assert.Throws<ArgumentException>(() => User.Create(name, "a@b.com", "Strong@Password123", UserRole.User, default));
    }
}
