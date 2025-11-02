using Fiap.CloudGames.Domain.Promotions.Entities;

namespace Fiap.CloudGames.Domain.Promotions.Repositories;

public interface IPromotionRepository
{
	/// <summary>
	/// Method to get all promotions including their related entities.
	/// </summary>
	/// <param name="ct"></param>
	/// <returns></returns>
	Task<IReadOnlyCollection<Promotion>> GetAllAsync(CancellationToken ct);

	/// <summary>
	/// Method to get a promotion by its Id including its related entities.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="ct"></param>
	/// <returns></returns>
	Task<Promotion?> GetByIdAsync(Guid id, CancellationToken ct);

	/// <summary>
	/// Method to find a promotion by its Id for update scenarios.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="ct"></param>
	/// <returns></returns>
	Task<Promotion?> FindByIdAsync(Guid id, CancellationToken ct);

	/// <summary>
	/// Method to add a new promotion to the repository.
	/// </summary>
	/// <param name="promotion"></param>
	/// <param name="ct"></param>
	/// <returns></returns>
	Task AddAsync(Promotion promotion, CancellationToken ct);

	/// <summary>
	/// Method to update an existing promotion in the repository.
	/// </summary>
	/// <param name="promotion"></param>
	/// <param name="ct"></param>
	/// <returns></returns>
	Task UpdateAsync(Promotion promotion, CancellationToken ct);

	/// <summary>
	/// Method to delete a promotion by its Id from the repository.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="ct"></param>
	/// <returns></returns>
	Task DeleteAsync(Guid id, CancellationToken ct);
}
