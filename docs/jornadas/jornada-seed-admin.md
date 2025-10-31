# Jornada — Seed do Admin (startup seeding)

## Objetivo
Garantir que um usuário administrador exista após a primeira inicialização da API, criando-o automaticamente caso não exista.

## Atores
- Sistema (seeder)
- Administrador inicial
- API: `src/Fiap.CloudGames.Api`
- Banco: `AppDbContext`

## Pré-condições
- Aplicação sendo executada sem admin existente.
- `dotnet user-secrets` ou variáveis de ambiente configurados com credenciais iniciais (nome, e-mail, senha).

## Fluxo principal
1. No startup, `UserSeeder` verifica existência de um Usuário com os dados com as credenciais iniciais.
2. Se nenhum usuário for encontrado, cria um usuário admin com dados fornecidos por configuração e senha definida (hash com BCrypt).
3. Admin criado com `active` e `EmailConfirmed = true`.

## Fluxos alternativos
- Admin já existe: seed não faz alteração.
- Credenciais inválidas/no secrets: não cria admin, loga erro e interrompe startup (falha crítica).

## Pós-condição
- Admin capaz de autenticar imediatamente.

## Componentes
- Seeder: `src/Fiap.CloudGames.Infrastructure/Users/Seeders/UserSeeder.cs`
- Config: `src/Fiap.CloudGames.Domain/Shared/Options/AdminUserOptions.cs`

## Critérios de aceitação
- Ao iniciar com DB vazio, existe ao menos um admin funcional.

## Observações
- Tratar com cuidado a distribuição de senhas iniciais; preferir secrets e não commitar credenciais no repositório.
