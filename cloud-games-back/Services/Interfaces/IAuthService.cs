namespace cloud_games_back.Services.Interfaces
{
    public interface IAuthService
    {
        public Task<string?> LoginAsync(string email, string senha);

        public Task SeedUserAsync();


    }
}
