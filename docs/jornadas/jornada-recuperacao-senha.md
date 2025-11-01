# Jornada — Esqueci a Senha / Redefinição

## Objetivo
Permitir que usuários recuperem o acesso quando esquecerem a senha, através de geração de token de redefinição enviado por e-mail.

## Atores
- Usuário (esqueceu a senha)
- API: `src/Fiap.CloudGames.Api`
- Serviço de e-mail (inicialmente via Console): `src/Fiap.CloudGames.Infrastructure/Email/ConsoleEmailService.cs`
- Banco: `AppDbContext`

## Pré-condições
- Usuário com e-mail cadastrado.

## Fluxo principal
1. Usuário solicita recuperação em `POST /api/users/forgot-password` informando e-mail.
2. Sistema verifica existência do e-mail e gera token de redefinição.
3. Envia e-mail com link para redefinir senha ao usuário.
4. Usuário acessa link e chama `POST /api/users/reset-password` com token e nova senha.
5. Sistema valida token, atualiza a senha (BCrypt) e invalida token.

## Fluxos alternativos
- E-mail não encontrado: retornar `200` sem indicar inexistência (pra evitar enumeração) ou retornar erro amigável.
- Token inválido/expirado: solicitar novo link.

## Pós-condição
- Senha atualizada; usuário pode autenticar com nova senha.

## Endpoints
- `POST /api/users/forgot-password`
- `POST /api/users/reset-password`

## Componentes do código
- Controller: `src/Fiap.CloudGames.Api/Controllers/UsersController.cs`
- Service: `src/Fiap.CloudGames.Application/Users/Services/UserService.cs`
- Infra e-mail: `src/Fiap.CloudGames.Infrastructure/Email/ConsoleEmailService.cs`

## Observações
- Considerar limitar tentativas de geração de token para evitar abuso.
