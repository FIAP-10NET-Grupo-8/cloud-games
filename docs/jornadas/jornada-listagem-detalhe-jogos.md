# Jornada — Listagem e Detalhe de Jogos

## Objetivo
Permitir que usuários encontrem e visualizem detalhes de jogos disponíveis.

## Atores
- Usuário (qualquer)
- API: `src/Fiap.CloudGames.Api`
- Banco: `AppDbContext`

## Pré-condições
- Jogos cadastrados no banco.

## Fluxo Principal
1. `GET /api/games` retorna lista de jogos.
2. Usuário aplica filtros/ordenacao.
3. `GET /api/games/{id}` retorna detalhes do jogo selecionado.

## Fluxos Alternativos
- Nenhum jogo encontrado: retornar lista vazia com `200`.
- Jogo não encontrado por id: retornar `404`.

## Endpoints
- `GET /api/games`
- `GET /api/games/{id}`

## Componentes do código
- Controllers: `src/Fiap.CloudGames.Api/Controllers/GamesController.cs`
- Services: `src/Fiap.CloudGames.Application/Games/Services/GameService.cs`
- Domain: `src/Fiap.CloudGames.Domain/Games/Entities/Game.cs`

## Critérios de Aceitação
- Listagem paginada com filtros funcionais.
- Detalhe por id retorna todas informações relevantes.

## Métricas / Riscos
- Performance em consultas complexas; adicionar índices se necessário.
