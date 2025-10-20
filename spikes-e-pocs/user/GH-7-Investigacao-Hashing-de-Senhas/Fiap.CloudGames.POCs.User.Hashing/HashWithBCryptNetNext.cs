using System;

namespace Fiap.CloudGames.POCs.User.Hashing;

public static class HashWithBCryptNetNext
{
    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public static bool Verify(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }

    public static void RunDemo()
    {
        Console.WriteLine("--- POC de Hashing de Senha com BCrypt.Net-Next ---");
        Console.WriteLine();

        // 1. Definir uma senha de exemplo
        string minhaSenha = "Senha@Forte123";
        Console.WriteLine($"Senha original: {minhaSenha}");
        Console.WriteLine();

        // 2. Gerar o Hash da senha
        string meuHash = HashPassword(minhaSenha);

        Console.WriteLine("O hash gerado (salt está incluído):");
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