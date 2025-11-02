using Fiap.CloudGames.Domain.Promotions.Entities;
using Fiap.CloudGames.Domain.Promotions.Enums;
using Fiap.CloudGames.Domain.Promotions.ValueObjects;

namespace Fiap.CloudGames.Tests.Promotions;

public class PromotionTests
{
    [Fact]
    public void Discount_Create_Valid_RoundsAndSetsPercentage()
    {
        var d = Discount.Create(12.345m);
        Assert.Equal(decimal.Round(12.345m, 2), d.Percentage);

        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => Discount.Create(0m));
        Assert.Contains("percentage", ex.ParamName);

        Assert.Throws<ArgumentOutOfRangeException>(() => Discount.Create(100.1m));
    }

    [Fact]
    public void PromotionPeriod_Create_And_IsActive_Works()
    {
        var now = DateTime.UtcNow;
        var start = now.AddMinutes(-1);
        var end = now.AddMinutes(1);
        var period = PromotionPeriod.Create(start, end);
        Assert.True(period.IsActive(now));

        Assert.Throws<ArgumentException>(() => PromotionPeriod.Create(now, now));
    }

    [Fact]
    public void Create_Promotion_With_Games_And_Status()
    {
        var now = DateTime.UtcNow;
        var start = now.AddMinutes(-1);
        var end = now.AddMinutes(1);
        var gameId = Guid.NewGuid();
        var promo = Promotion.Create(" Test Promo ", start, end, 10m, [gameId]);

        Assert.Equal("Test Promo", promo.Name);
        Assert.Single(promo.EligibleGames);
        Assert.Equal(10m, promo.Discount.Percentage);
        Assert.Equal(PromotionStatus.Active, promo.Status);
    }

    [Fact]
    public void Create_Promotion_Scheduled_When_Start_InFuture()
    {
        var now = DateTime.UtcNow;
        var start = now.AddMinutes(5);
        var end = now.AddMinutes(10);
        var promo = Promotion.Create("P", start, end, 5m);
        Assert.Equal(PromotionStatus.Scheduled, promo.Status);
    }

    [Fact]
    public void UpdateName_Throws_On_Invalid()
    {
        var now = DateTime.UtcNow;
        var promo = Promotion.Create("P", now.AddMinutes(-1), now.AddMinutes(1), 5m);
        Assert.Throws<ArgumentException>(() => promo.UpdateName(" "));
    }

    [Fact]
    public void UpdatePromotionDates_And_UpdateStatus_Work()
    {
        var now = DateTime.UtcNow;
        var promo = Promotion.Create("P", now.AddMinutes(5), now.AddMinutes(10), 5m);
        // initially scheduled
        Assert.Equal(PromotionStatus.Scheduled, promo.Status);

        // update to include now
        promo.UpdatePromotionDates(now.AddMinutes(-1), now.AddMinutes(1));
        Assert.Equal(PromotionStatus.Active, promo.Status);
        Assert.NotNull(promo.UpdatedAt);
        Assert.True(promo.UpdatedAt > promo.CreatedAt);
    }

    [Fact]
    public void AddGame_Adds_And_Prevents_Duplicates()
    {
        var now = DateTime.UtcNow;
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();
        var promo = Promotion.Create("P", now.AddMinutes(-1), now.AddMinutes(1), 5m, [id1]);

        promo.AddGame(new PromotionItem(id2));
        Assert.Equal(2, promo.EligibleGames.Count);

        promo.AddGame(new PromotionItem(id2));
        Assert.Equal(2, promo.EligibleGames.Count); // no duplicate
    }

    [Fact]
    public void RemoveGame_Behavior()
    {
        var now = DateTime.UtcNow;
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();
        var promo = Promotion.Create("P", now.AddMinutes(-1), now.AddMinutes(1), 5m, [id1, id2]);

        promo.RemoveGame(new PromotionItem(id1));
        Assert.Single(promo.EligibleGames);

        // removing when only one left should throw
        Assert.Throws<InvalidOperationException>(() => promo.RemoveGame(new PromotionItem(id2)));
    }

    [Fact]
    public void UpdateApplicableGames_Validates()
    {
        var now = DateTime.UtcNow;
        var promo = Promotion.Create("P", now.AddMinutes(-1), now.AddMinutes(1), 5m, [Guid.NewGuid()]);
        Assert.Throws<ArgumentException>(() => promo.UpdateApplicableGames(null));
        Assert.Throws<ArgumentException>(() => promo.UpdateApplicableGames([]));

        var newIds = new[] { Guid.NewGuid(), Guid.NewGuid() };
        promo.UpdateApplicableGames(newIds);
        Assert.Equal(2, promo.EligibleGames.Count);
    }

    [Fact]
    public void Activate_Deactivate_Expire_Workflow()
    {
        var now = DateTime.UtcNow;
        var activePromo = Promotion.Create("P", now.AddMinutes(-1), now.AddMinutes(1), 5m);
        // Activate should succeed (already active period)
        activePromo.Activate();
        Assert.Equal(PromotionStatus.Active, activePromo.Status);

        // Deactivate from Active
        activePromo.Deactivate();
        Assert.Equal(PromotionStatus.Inactive, activePromo.Status);

        // Deactivate when not allowed
        var expiredPromo = Promotion.Create("X", now.AddMinutes(-10), now.AddMinutes(-5), 5m);
        Assert.Equal(PromotionStatus.Expired, expiredPromo.Status);
        Assert.Throws<InvalidOperationException>(() => expiredPromo.Deactivate());

        // Expire should throw when end date in future
        var futurePromo = Promotion.Create("F", now.AddMinutes(1), now.AddMinutes(10), 5m);
        Assert.Throws<InvalidOperationException>(() => futurePromo.Expire());

        // Expire allowed when end date passed
        var alreadyEnded = Promotion.Create("E", now.AddMinutes(-10), now.AddMinutes(-1), 5m);
        alreadyEnded.Expire();
        Assert.Equal(PromotionStatus.Expired, alreadyEnded.Status);
    }

    [Fact]
    public void ComputeStatus_Returns_Expected()
    {
        var now = DateTime.UtcNow;
        Assert.Equal(PromotionStatus.Scheduled, Promotion.ComputeStatus(now.AddMinutes(1), now.AddMinutes(2)));
        Assert.Equal(PromotionStatus.Active, Promotion.ComputeStatus(now.AddMinutes(-1), now.AddMinutes(1)));
        Assert.Equal(PromotionStatus.Expired, Promotion.ComputeStatus(now.AddMinutes(-10), now.AddMinutes(-5)));
    }
}
