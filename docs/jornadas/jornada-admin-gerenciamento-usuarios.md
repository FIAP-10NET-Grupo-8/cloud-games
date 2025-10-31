# Jornada — Manutenção de Cadastros de Usuário (Admin)

## Objetivo
Operações de administração sobre contas de usuários: criar, editar, bloquear, desbloquear, excluir, restaurar e listar.

## Atores
- Administrador
- API: `src/Fiap.CloudGames.Api`
- Banco: `AppDbContext`

## Pré-condições
- Administrador autenticado com role `Administrator`.

## Fluxo principal
- Listagem: `GET /api/users`.
- Detalhamento: `GET /api/users/{id}`.
- Criação: `POST /api/users` (gera primeiro-acesso).
- Edição: `PUT /api/users/{id}` atualiza dados e opcionalmente revalida e-mail.
- Exclusão: `DELETE /api/users/{id}` (marca `deleted`).
- Restauração: `POST /api/users/{id}/restore` (altera status para `active`).
- Bloqueio/Desbloqueio: `POST /api/users/{id}/block` / `unblock`. (a fazer)

## Fluxos alternativos
- Ações em usuário não encontrado: retornar `404`.
- Tentativa de ação inválida (ex.: bloquear já `deleted`): retornar erro apropriado.

## Pós-condição
- Alterações refletidas no cadastro de usuário e audit logs atualizados.

## Endpoints
- `GET /api/users`
- `GET /api/users/{id}`
- `POST /api/users`
- `PUT /api/users/{id}`
- `DELETE /api/users/{id}`
- `POST /api/users/{id}/restore`
- `POST /api/users/{id}/block` (a fazer)

## Componentes do código
- Controller: `src/Fiap.CloudGames.Api/Controllers/Admin/UsersAdminController.cs`
- Services: `src/Fiap.CloudGames.Application/Services/UserService.cs`

## Observações
- Registrar auditoria para alterações críticas.
