using System;
using Fiap.CloudGames.POCs.User.Hashing;

Console.WriteLine("--- POC de Hashing de Senha ---");
Console.WriteLine();

// 2) Demo usando System.Security.Cryptography (PBKDF2)
Console.WriteLine("Executando demo 1: System.Security.Cryptography (PBKDF2)");
Console.WriteLine();
HashWithNativeCSharp.RunDemo();

Console.WriteLine();
Console.WriteLine("--- Retornando ao programa principal ---");
Console.WriteLine();

// 1) Demo usando BCrypt.Net-Next
Console.WriteLine("Executando demo 2: BCrypt.Net-Next");
Console.WriteLine();
HashWithBCryptNetNext.RunDemo();