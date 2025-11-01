# Jornada — Autenticação (Login / JWT)

## Objetivo
Descrever o fluxo de autenticação dos usuários via JWT, usado após cadastro, primeiro acesso ou seed.

## Atores
- Usuário autenticando
- API: `src/Fiap.CloudGames.Api`
- Serviço de autenticação: `src/Fiap.CloudGames.Application/Users/UserService`

## Pré-condições
- Conta ativa e/ou email confirmado.
- Configurações JWT definidas no `appsettings` ou user-secrets.

## Fluxo principal
1. Usuário envia `POST /api/users/login` com e-mail e senha.
2. Sistema valida credenciais verificando senha com BCrypt.
3. Se válidas, gera JWT com claims (UserId, Name, Role) e retorna ao cliente.
4. Cliente armazena token e usa nas requisições subsequentes no header `Authorization: Bearer <token>`.

## Fluxos alternativos
- Credenciais inválidas: retorna `401`.
- Conta inativa/excluída/bloqueada: retorna `403` com instruções.

## Pós-condição
- Cliente possui token JWT válido para acessar recursos protegidos.

## Endpoints
- `POST /api/users/login`

## Componentes do código
- Controller: `src/Fiap.CloudGames.Api/Controllers/UsersController.cs`
- Services: `src/Fiap.CloudGames.Application/Users/Services/UserService.cs` e `src/Fiap.CloudGames.Infrastructure/Auth/Services/JwtService.cs`
- Config: `src/Fiap.CloudGames.Infrastructure/Auth/JwtOptions.cs`

## Observações
- A autenticação é usada por praticamente todas as jornadas; pode ser descrita separadamente e referenciada quando necessário.
