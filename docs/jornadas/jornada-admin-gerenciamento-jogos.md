# Jornada — Manutenção de Jogos (Admin)

## Objetivo
Permitir que administradores façam manutenção dos jogos no catálogo.

## Atores
- Administrador
- API: `src/Fiap.CloudGames.Api`
- Banco: `AppDbContext`

## Pré-condições
- Usuário autenticado com role `Administrator`.

## Fluxo Principal
- `POST /api/games` para criar novo jogo.
- `PUT /api/games/{id}` para editar dados do jogo.
- `DELETE /api/games/{id}` para remover jogo.

## Fluxos Alternativos
- Dados inválidos: retornar `400` com erros.
- Jogo duplicado: retornar `409` ou erro de validação.

## Endpoints
- `POST /api/games`
- `PUT /api/games/{id}`
- `DELETE /api/games/{id}`

## Componentes do código
- Controller: `src/Fiap.CloudGames.Api/Controllers/GamesController.cs`
- Service: `src/Fiap.CloudGames.Application/Games/Services/GameService.cs`
- Domain: `src/Fiap.CloudGames.Domain/Games/Entities/Game.cs`

## Critérios de Aceitação
- Admin consegue criar e editar jogos.
- Validações aplicadas e erros retornados corretamente.

## Riscos
- Conflito de concorrência em edições simultâneas.
