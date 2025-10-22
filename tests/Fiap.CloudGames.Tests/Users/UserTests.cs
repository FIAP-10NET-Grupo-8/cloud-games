using Fiap.CloudGames.Domain.Users.Entities;
using Fiap.CloudGames.Domain.Users.Enums;

namespace Fiap.CloudGames.Tests.Users;

public class UserTests
{
    #region Creation / Name
    [Fact]
    public void Create_ValidUser_SetsProperties()
    {
        var user = User.Create("Name", "name@example.com", "Strong@Password123", UserRole.User, UserStatus.Active);
        Assert.NotEqual(Guid.Empty, user.Id);
        Assert.Equal("Name", user.Name);
        Assert.True(user.VerifyPassword("Strong@Password123"));
        Assert.True(user.IsActive);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void Create_InvalidName_Throws(string name)
    {
        Assert.Throws<ArgumentException>(() => User.Create(name, "a@b.com", "Strong@Password123", UserRole.User, UserStatus.Active));
    }

    [Fact]
    public void Create_WithActiveStatus_IsActiveTrue()
    {
        var user = User.Create("Name", "name@example.com", "Strong@Password123", UserRole.User, UserStatus.Active);
        Assert.True(user.IsActive);
    }

    [Fact]
    public void Create_WithInactiveStatus_IsActiveFalse()
    {
        var user = User.Create("Name", "name@example.com", "Strong@Password123", UserRole.User, UserStatus.Inactive);
        Assert.False(user.IsActive);
    }

    [Fact]
    public void UpdateName_Invalid_Throws()
    {
        var user = User.Create("Name", "name@example.com", "Strong@Password123", UserRole.User, UserStatus.Active);
        Assert.Throws<ArgumentException>(() => user.UpdateName("   "));
    }
    #endregion

    #region Email
    [Fact]
    public void UpdateEmail_ResetsEmailConfirmed()
    {
        var user = User.Create("Name", "name@example.com", "Strong@Password123", UserRole.User, UserStatus.Active);
        user.MarkEmailConfirmed();
        Assert.True(user.EmailConfirmed);

        user.UpdateEmail("other@example.com");
        Assert.False(user.EmailConfirmed);
    }

    [Fact]
    public void ConfirmEmail_WrongToken_ReturnsFalse()
    {
        var user = User.Create("Name", "name@example.com", "Strong@Password123", UserRole.User, UserStatus.Inactive);
        var token = user.GenerateEmailConfirmationToken();
        Assert.False(string.IsNullOrWhiteSpace(token));

        // Wrong token
        Assert.False(user.ConfirmEmail("wrong"));
        // Ensure still not confirmed
        Assert.False(user.EmailConfirmed);
    }

    [Fact]
    public void ConfirmEmail_CorrectToken_ConfirmsAndActivates()
    {
        var user = User.Create("Name", "name@example.com", "Strong@Password123", UserRole.User, UserStatus.Inactive);
        var token = user.GenerateEmailConfirmationToken();
        Assert.False(string.IsNullOrWhiteSpace(token));

        // Correct token
        Assert.True(user.ConfirmEmail(token));
        Assert.True(user.EmailConfirmed);
        Assert.Equal(UserStatus.Active, user.Status);

        // Confirming again should be idempotent
        Assert.True(user.ConfirmEmail(token));
    }
    #endregion

    #region Password / Reset
    [Fact]
    public void ResetPassword_WrongToken_ReturnsFalse()
    {
        var user = User.Create("Name", "name@example.com", "Strong@Password123", UserRole.User, UserStatus.Active);
        var token = user.GeneratePasswordResetToken(TimeSpan.FromHours(1));
        Assert.False(string.IsNullOrWhiteSpace(token));

        // Wrong token
        Assert.False(user.ResetPassword("wrong", "New@Password123"));
    }

    [Fact]
    public void ResetPassword_CorrectToken_ResetsPassword()
    {
        var user = User.Create("Name", "name@example.com", "Strong@Password123", UserRole.User, UserStatus.Active);
        var oldPassword = "Strong@Password123";
        Assert.True(user.VerifyPassword(oldPassword));

        var token = user.GeneratePasswordResetToken(TimeSpan.FromHours(1));
        Assert.False(string.IsNullOrWhiteSpace(token));

        // Correct token
        Assert.True(user.ResetPassword(token, "New@Password123"));
        Assert.True(user.VerifyPassword("New@Password123"));
    }

    [Fact]
    public void ResetPassword_ExpiredToken_ReturnsFalse()
    {
        var user = User.Create("User2", "u2@example.com", "Strong@Password123", UserRole.User, UserStatus.Active);
        var token = user.GeneratePasswordResetToken(TimeSpan.FromSeconds(-1));
        Assert.False(user.ResetPassword(token, "Another@123"));
    }
    #endregion

    #region Soft delete / Restore
    [Fact]
    public void SoftDelete_SetsDeleted()
    {
        var user = User.Create("Name", "name@example.com", "Strong@Password123", UserRole.User, UserStatus.Active);
        user.SoftDelete();
        Assert.Equal(UserStatus.Deleted, user.Status);
    }

    [Fact]
    public void Restore_WhenEmailConfirmed_RestoresToActive()
    {
        var user = User.Create("Name", "name@example.com", "Strong@Password123", UserRole.User, UserStatus.Active);
        user.SoftDelete();
        user.MarkEmailConfirmed();
        user.Restore();
        Assert.Equal(UserStatus.Active, user.Status);
    }

    [Fact]
    public void Restore_WhenEmailNotConfirmed_RestoresToInactive()
    {
        var user2 = User.Create("Name2", "name2@example.com", "Strong@Password123", UserRole.User, UserStatus.Active);
        user2.SoftDelete();
        user2.Restore();
        Assert.Equal(UserStatus.Inactive, user2.Status);
    }
    #endregion

    #region Role / Status setters
    [Fact]
    public void SetRole_Work()
    {
        var user = User.Create("Name", "name@example.com", "Strong@Password123", UserRole.User, UserStatus.Active);
        user.SetRole(UserRole.Administrator);
        Assert.Equal(UserRole.Administrator, user.Role);
    }

    [Fact]
    public void SetStatus_Work()
    {
        var user = User.Create("Name", "name@example.com", "Strong@Password123", UserRole.User, UserStatus.Active);
        user.SetStatus(UserStatus.Blocked);
        Assert.Equal(UserStatus.Blocked, user.Status);
    }
    #endregion
}
