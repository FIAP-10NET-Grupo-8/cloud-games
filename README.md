# Tech Challenge - FIAP Cloud Games

## Configurar segredos em DEV

**Configurar JWT:**
```bash
dotnet user-secrets set "Jwt:Secret" "<chave-secreta>" --project src/Fiap.CloudGames.Api/Fiap.CloudGames.Api.csproj
```

> Obs: Exemplos de geração de chave aleatória:
> - PowerShell:
> ```powershell
> $chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()_-+=<>?"; $secret = -join (1..64 | ForEach-Object { $chars[(Get-Random -Maximum $chars.Length)] }); Write-Output $secret
> ```
> - Linux/macOS:
> ```bash
> head -c 64 /dev/urandom | base64
> ```

**Configurar Senha do Administrador:**
```bash
dotnet user-secrets set "AdminUser:Password" "<senha-temporaria>" --project src/Fiap.CloudGames.Api/Fiap.CloudGames.Api.csproj
```
> Obs: A senha temporária deverá obedecer as regras de complexidade definidas no sistema:
> - Mínimo de 8 caracteres
> - Pelo menos uma letra maiúscula
> - Pelo menos uma letra minúscula
> - Pelo menos um número
> - Pelo menos um caractere especial

## Adicionar migrações e atualizar banco de dados
Abrir o Package Manager Console com o projeto Fiap.CloudGames.Infrastructure selecionado como projeto de inicialização e executar o comando:
```bash
Add-Migration <MigrationName> -Context AppDbContext -OutputDir "Persistence/Migrations" -StartupProject Fiap.CloudGames.Api
```

Ao finalizar, atualizar o banco de dados com o comando:
```bash
Update-Database -Context AppDbContext -StartupProject Fiap.CloudGames.Api
```

> Obs: Se preferir rodar no CLI do .NET, vai ser necessário utilizar as ferramentas (e suas versões) listadas no manifesto, então rode os comandos abaixo:
> ```bash
> dotnet tool restore               # restaura ferramentas listadas no manifest
> dotnet tool run dotnet-ef -- migrations add <MigrationName> --project src/Fiap.CloudGames.Infrastructure --startup-project src/Fiap.CloudGames.Api --context AppDbContext --output-dir "Persistence/Migrations"
> dotnet tool run dotnet-ef -- database update --project src/Fiap.CloudGames.Infrastructure --startup-project src/Fiap.CloudGames.Api --context AppDbContext
> ```
