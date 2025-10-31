# Jornada — Primeiro Acesso / Seed do Admin

## Objetivo
Garantir que exista um usuário administrador inicial configurado via seed ou `dotnet user-secrets` para permitir administração do sistema.

## Atores
- Administrador (usuário inicial)
- API: `src/Fiap.CloudGames.Api`
- Seed: `src/Fiap.CloudGames.Infrastructure/Seeders/UserSeeder.cs`
- Banco: `AppDbContext`

## Pré-condições
- Projeto construído e migrations aplicadas.
- `dotnet user-secrets` configurado (opcional para senha temporária).

## Fluxo Principal
1. Ao aplicar seeds, se não existir admin, `UserSeeder` cria usuário com role `Administrator`.
2. Usuário recebe e-mail com instruções de primeiro acesso (token de primeiro acesso).
3. Usuario define senha via `POST /api/auth/first-access`.

## Fluxos Alternativos
- Admin já existe: não duplicar.
- Falha ao criar seed: log e retry manual.

## Endpoints / Componentes
- Seeder: `src/Fiap.CloudGames.Infrastructure/Seeders/UserSeeder.cs`
- Controller: `src/Fiap.CloudGames.Api/Controllers/AdminController.cs` (se existir)

## Critérios de Aceitação
- Admin criado e capaz de logar.
- Token de primeiro acesso válido e utilizável.

## Métricas / Riscos
- Exposição de senha temporária em logs.

## Responsáveis
- Dono da jornada: (nome)
- Implementação: (equipe)
