# Template de Jornada

## Título
- Nome da jornada:

## Objetivo
- O que se busca resolver/entregar com esta jornada

## Atores
- Ex.: Usuário não autenticado, Usuário autenticado, Administrador, Sistema de e-mail, Banco de Dados

## Pré-condições
- Estado do sistema antes do início

## Fluxo Principal (passo a passo)
1. ...
2. ...

## Fluxos Alternativos / Erros
- Caso A: ...
- Caso B: ...

## Regras de Negócio
- Regras e validações

## Endpoints / APIs envolvidos
- `POST /api/auth/register` — Controller / Método
- `POST /api/auth/login` — Controller / Método

## Componentes do código
- API: `src/Fiap.CloudGames.Api` (Controllers, Middlewares)
- Application: `src/Fiap.CloudGames.Application` (Serviços / Casos de uso)
- Domain: `src/Fiap.CloudGames.Domain` (Entidades / VO)
- Infrastructure: `src/Fiap.CloudGames.Infrastructure` (EF Core, Seeders, `AppDbContext`)

## Migrations / Dados
- Tabelas afetadas, seeders (ex.: `UserSeeder`), comandos:
 - `dotnet user-secrets set ...`
 - `Add-Migration` / `Update-Database`

## Critérios de Aceitação
- Condições que validam sucesso

## Métricas / Telemetria
- Tempo de resposta, taxa de erro, logs específicos

## Riscos e Observações
- Possíveis falhas e mitigação
