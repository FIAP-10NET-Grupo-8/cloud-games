using System;
using Fiap.CloudGames.Domain.Users.UserObjects;
using Xunit;

namespace Fiap.CloudGames.Tests.Users;

public class PasswordTests
{
    [Fact]
    public void Create_ValidPassword_HashesPassword()
    {
        string plain = "Strong@Password123";
        var pw = Password.Create(plain);
        Assert.NotNull(pw.Hash);
        Assert.NotEqual(plain, pw.Hash);
        Assert.True(pw.Verify(plain));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("short1!")]
    [InlineData("allletters!")]
    [InlineData("12345678!")]
    public void Create_InvalidPassword_Throws(string input)
    {
        Assert.Throws<ArgumentException>(() => Password.Create(input));
    }

    [Fact]
    public void Verify_WrongPassword_ReturnsFalse()
    {
        var pw = Password.Create("Strong@Password123");
        Assert.False(pw.Verify("wrongPass1!"));
    }
}
