# Jornada — Primeiro Acesso (Admin created user)

## Objetivo
Descrever o fluxo quando um Administrador cria um usuário e o usuário precisa concluir o primeiro acesso definindo sua senha.

## Atores
- Administrador
- Novo Usuário
- API: `src/Fiap.CloudGames.Api`
- Serviço de e-mail (inicialmente via Console): `src/Fiap.CloudGames.Infrastructure/Email/ConsoleEmailService.cs`
- Banco: `AppDbContext`

## Pré-condições
- Administrador autenticado com role `Administrator`.

## Fluxo principal
1. Admin acessa painel e cria usuário por `POST /api/users` (informa nome, e-mail, role, status `pending`).
2. Sistema valida e cria usuário com status `pending` ou `inactive`, gerando token de primeiro acesso.
3. Sistema envia e-mail para novo usuário com link de primeiro acesso (contendo token).
4. Usuário clica no link e acessa `POST /api/users/first-access` com token e define nova senha.
5. Sistema valida token, grava senha hasheada (BCrypt) e ativa conta (`active`).

## Fluxos alternativos
- E-mail já cadastrado: Admin recebe erro e pode editar usuário existente.
- Token inválido/expirado: usuário solicita novo primeiro acesso via Admin ou fluxo de reenvio.

## Pós-condição
- Usuário criado e com senha definida; pode autenticar.

## Endpoints
- `POST /api/users`
- `POST /api/users/first-access`
- `POST /api/users/resend-first-access` (a fazer)

## Componentes do código
- Controller: `src/Fiap.CloudGames.Api/Controllers/UsersController.cs`
- Service: `src/Fiap.CloudGames.Application/Users/Services/UserService.cs`
- Infra e-mail: `src/Fiap.CloudGames.Infrastructure/Email/ConsoleEmailService.cs`

## Critérios de aceitação
- Admin consegue criar usuário que recebe link de primeiro acesso.
- Usuário define senha e ativa conta.

## Observações
- Recomenda-se registrar audit log da criação de usuário por Admin.
