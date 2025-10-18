using System;
using System.Security.Cryptography;
using System.Text;

namespace Fiap.CloudGames.POCs.User.Hashing;

public static class HashWithNativeCSharp
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 100_000;

    // Formato armazenado: pbkdf2_sha256$<iterations>$<salt-base64>$<hash-base64>
    public static string HashPassword(string password)
    {
        byte[] salt = new byte[SaltSize];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        byte[] hash;
        using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
        {
            hash = pbkdf2.GetBytes(HashSize);
        }

        string saltB64 = Convert.ToBase64String(salt);
        string hashB64 = Convert.ToBase64String(hash);
        return $"pbkdf2_sha256${Iterations}${saltB64}${hashB64}";
    }

    public static bool Verify(string password, string storedHash)
    {
        try
        {
            var parts = storedHash.Split('$');

            if (parts.Length != 4) return false;
            if (!parts[0].Equals("pbkdf2_sha256", StringComparison.OrdinalIgnoreCase)) return false;
            if (!int.TryParse(parts[1], out int iterations)) return false;

            byte[] salt = Convert.FromBase64String(parts[2]);
            byte[] hash = Convert.FromBase64String(parts[3]);

            byte[] computed;
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
            {
                computed = pbkdf2.GetBytes(hash.Length);
            }

            // Comparação em tempo fixo para evitar ataques de timing
            return CryptographicOperations.FixedTimeEquals(computed, hash);
        }
        catch
        {
            return false;
        }
    }

    public static void RunDemo()
    {
        Console.WriteLine("--- POC de Hashing de Senha com System.Security.Cryptography (PBKDF2) ---");
        Console.WriteLine();

        // 1. Definir uma senha de exemplo
        string minhaSenha = "Senha@Forte123";
        Console.WriteLine($"Senha original: {minhaSenha}");
        Console.WriteLine();

        // 2. Gerar o Hash da senha
        string meuHash = HashPassword(minhaSenha);

        Console.WriteLine("O hash gerado (contém salt e iterações):");
        Console.WriteLine(meuHash);
        Console.WriteLine();

        // --- Verificação ---
        Console.WriteLine("--- Verificando a Senha ---");

        // 3. Verificar a senha CORRETA
        bool senhaCorretaValida = Verify(minhaSenha, meuHash);

        Console.WriteLine($"Tentando verificar com a senha correta ('{minhaSenha}')...");
        Console.WriteLine($"O resultado é: {senhaCorretaValida}"); // Deve ser True
        Console.WriteLine();

        // 4. Verificar uma senha INCORRETA
        string senhaErrada = "senhaErrada";
        bool senhaIncorretaValida = Verify(senhaErrada, meuHash);

        Console.WriteLine($"Tentando verificar com a senha incorreta ('{senhaErrada}')...");
        Console.WriteLine($"O resultado é: {senhaIncorretaValida}"); // Deve ser False
        Console.WriteLine();

        Console.WriteLine("--- POC Concluída ---");
        Console.WriteLine("Pressione uma tecla para continuar...");
        Console.ReadKey();
    }
}