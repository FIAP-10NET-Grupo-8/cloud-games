using Fiap.CloudGames.Domain.Promotions.Entities;
using Fiap.CloudGames.Domain.Promotions.Repositories;
using Fiap.CloudGames.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fiap.CloudGames.Infrastructure.Promotions.Repositories;

public sealed class PromotionRepository(AppDbContext db) : IPromotionRepository
{
	private readonly AppDbContext _db = db;

	public async Task<IReadOnlyCollection<Promotion>> GetAllAsync(CancellationToken ct)
	{
		return await _db.Promotions
			.AsNoTracking()
			.Include(p => p.EligibleGames)
			.OrderBy(p => p.Period.StartDate)
			.ToListAsync(ct);
	}

	public async Task<Promotion?> GetByIdAsync(Guid id, CancellationToken ct)
	{
		return await _db.Promotions
			.AsNoTracking()
			.Include(p => p.EligibleGames)
			.FirstOrDefaultAsync(p => p.Id == id, ct);
	}

	public async Task<Promotion?> FindByIdAsync(Guid id, CancellationToken ct)
	{
		return await _db.Promotions
			.Include(p => p.EligibleGames)
			.FirstOrDefaultAsync(p => p.Id == id, ct);
	}

	public async Task AddAsync(Promotion promotion, CancellationToken ct)
	{
		_db.Promotions.Add(promotion);
		await _db.SaveChangesAsync(ct);
	}

	public async Task UpdateAsync(Promotion promotion, CancellationToken ct)
	{
		_db.Promotions.Update(promotion);
		await _db.SaveChangesAsync(ct);
	}

	public async Task DeleteAsync(Guid id, CancellationToken ct)
	{
		var e = await _db.Promotions.FindAsync(new object[] { id }, ct);
		if (e is null) return;
		_db.Promotions.Remove(e);
		await _db.SaveChangesAsync(ct);
	}
}
