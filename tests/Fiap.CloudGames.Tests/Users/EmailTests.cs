using Fiap.CloudGames.Domain.Users.ValueObjects;

namespace Fiap.CloudGames.Tests.Users;

public class EmailTests
{
    [Fact]
    public void Create_ValidEmail_ReturnsEmail()
    {
        var email = Email.Create("user@example.com");
        Assert.Equal("user@example.com", email.Address);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("invalid")]
    public void Create_InvalidEmail_Throws(string input)
    {
        Assert.Throws<ArgumentException>(() => Email.Create(input));
    }

    [Fact]
    public void ImplicitOperator_ReturnsAddress()
    {
        var email = Email.Create("a@b.com");
        string asString = email; // implicit conversion
        Assert.Equal("a@b.com", asString);
    }
}
