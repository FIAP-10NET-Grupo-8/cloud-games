# Tech Challenge - FIAP Cloud Games

## Configurar segredos em DEV

```bash
dotnet user-secrets set "Jwt:Secret" "<chave-aleatoria>" --project src/Fiap.CloudGames.Api/Fiap.CloudGames.Api.csproj
dotnet user-secrets set "AdminUser:Password" "<senha-temporaria>" --project src/Fiap.CloudGames.Api/Fiap.CloudGames.Api.csproj
```

## Exemplos de geração

**PowerShell:**
```powershell
-join ((33..126) | Get-Random -Count 64 | % {[char]$_})
```

**Linux/macOS:**
```bash
head -c 64 /dev/urandom | base64
```
