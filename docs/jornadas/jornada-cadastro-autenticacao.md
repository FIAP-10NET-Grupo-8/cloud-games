# Jornada — Cadastro e Autenticação

## Objetivo
Permitir que um novo usuário crie conta, confirme e-mail e faça login recebendo JWT.

## Atores
- Usuário (web/mobile)
- API: `src/Fiap.CloudGames.Api`
- Serviço de e-mail: `src/Fiap.CloudGames.Infrastructure/Email/ConsoleEmailService.cs` (ou similar)
- Banco de Dados: `AppDbContext` (`src/Fiap.CloudGames.Infrastructure`)

## Pré-condições
- Banco criado e migrations aplicadas.
- Configurações JWT definidas via `dotnet user-secrets` ou `appsettings`.

## Fluxo Principal
1. `POST /api/auth/register` com payload (nome, email, senha).
2. Validação (FluentValidation).
3. Serviço cria `User` no domínio; senha hasheada com BCrypt.
4. Persiste usuário via repositório (EF Core) e envia e-mail de confirmação via `ConsoleEmailService`.
5. Usuário confirma e-mail (`POST /api/auth/confirm`) e conta é ativada.
6. `POST /api/auth/login` com email/senha; API valida e retorna JWT.

## Fluxos Alternativos
- Email já cadastrado: 400 com mensagem.
- Senha fraca: 400 com validação.
- Falha no envio de e-mail: gravar evento e tentar envio novamente, alertar.

## Endpoints / Componentes
- Controllers: `src/Fiap.CloudGames.Api/Controllers/AuthController.cs`
- Services: `src/Fiap.CloudGames.Application/Services/AuthService.cs`
- Domain: `src/Fiap.CloudGames.Domain/Entities/User.cs`
- Infra: `src/Fiap.CloudGames.Infrastructure/Email/ConsoleEmailService.cs`
- Config: `src/Fiap.CloudGames.Infrastructure/Auth/JwtOptions.cs`

## Critérios de Aceitação
- Usuário pode registrar e logar; senha armazenada com BCrypt.
- JWT gerado de acordo com `JwtOptions`.
- Email de confirmação é disparado (ver logs/console).

## Métricas / Riscos
- Latência da criação de conta.
- Falha no envio de e-mail.

## Responsáveis
- Dono da jornada: (nome)
- Implementação: (equipe)
