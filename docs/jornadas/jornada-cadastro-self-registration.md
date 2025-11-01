# Jornada — Cadastro (Self-registration)

## Objetivo
Permitir que qualquer usuário finalize um cadastro de forma autônoma (self-registration) e confirme seu e-mail antes de obter acesso.

## Atores
- Usuário
- API: `src/Fiap.CloudGames.Api`
- Serviço de e-mail (inicialmente via Console): `src/Fiap.CloudGames.Infrastructure/Email/ConsoleEmailService.cs`
- Banco de dados: `AppDbContext`

## Pré-condições
- API operando e banco de dados criado.
- Configurações de e-mail e JWT definidas.

## Fluxo principal (caminho feliz)
1. Usuário preenche formulário de registro (nome, e-mail, senha) e envia `POST /api/users/register`.
2. Sistema valida dados (FluentValidation) e verifica duplicidade de e-mail.
3. Se válido, cria usuário com status `inactive` e persiste (senha hasheada com BCrypt).
4. Gera token de confirmação e envia e-mail para o usuário com link/token de ativação.
5. Usuário clica no link / chama `POST /api/users/confirm` com token.
6. Sistema valida token e ativa a conta (`active`).

## Fluxos alternativos
- E-mail já cadastrado: retornar `400` com orientação para recuperar senha ou login.
- Dados inválidos (senha fraca, e-mail inválido): retornar validação.
- Falha no envio de e-mail: registrar erro, mostrar mensagem ao usuário e tentar retry/alert.

## Pós-condição
- Conta ativa após confirmação por e-mail; usuário apto a autenticar.

## Endpoints
- `POST /api/users/register`
- `POST /api/users/confirm`

## Componentes do código
- Controller: `src/Fiap.CloudGames.Api/Controllers/UsersController.cs`
- Service: `src/Fiap.CloudGames.Application/Users/Services/UserService.cs`
- Infra e-mail: `src/Fiap.CloudGames.Infrastructure/Email/ConsoleEmailService.cs`
- Domain: `src/Fiap.CloudGames.Domain/Users/Entities/User.cs`

## Critérios de aceitação
- Usuário consegue criar conta e receber e-mail de confirmação.
- Conta permanece `inactive` até confirmação.

## Observações
- A autenticação (login) é uma jornada separada; aqui apenas o término cria a condição para autenticar.
