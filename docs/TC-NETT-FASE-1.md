## Tech Challenge - Fase 1

Tech Challenge é o projeto da fase que engloba os conhecimentos obtidos em todas as disciplinas dela.
Esta é uma atividade que, a princípio, deve ser desenvolvida em grupo.
É importante atentar-se ao prazo de entrega, uma vez que essa atividade é obrigatória e vale 90% da nota de todas as disciplinas da fase.

### O problema

A **FIAP Cloud Games (FCG)** será uma plataforma de venda de jogos digitais e gestão de servidores para partidas online.
Nesta primeira fase, você desenvolverá um serviço de cadastro de usuários e biblioteca de jogos adquiridos que servirá de base para as próximas fases do projeto.
Este desafio foi estruturado para aplicar os conhecimentos adquiridos nas disciplinas da fase.

### Desafio

A FIAP decidiu lançar uma plataforma de games voltados para a educação de tecnologia.
Ela possui a ideia de como o projeto deve funcionar e decidiu quebrá-lo em quatro fases para que o lançamento da FCG seja gradual e melhorado durante todo o processo de construção.
O objetivo desta fase é criar uma API REST em .NET 8 para gerenciar usuários e seus jogos adquiridos.
O projeto precisa garantir **persistência de dados, qualidade do software e boas práticas de desenvolvimento**, preparando a base para futuras funcionalidades como matchmaking e gerenciamento de servidores.
Com esse MVP, a FIAP conseguirá seguir com o projeto avaliando o que deve ser melhorado e o que pode ser acrescentado para que o serviço seja robusto e suporte todos os alunos e alunas da FIAP, Alura e PM3.

### Funcionalidades obrigatórias

**Cadastro de usuários:**

- Identificação do cliente por nome, e-mail e senha.
- Validar formato de e-mail e senha segura (mínimo de 8 caracteres com números, letras e caracteres especiais).

**Autenticação e Autorização:**

- Implementar autenticação via token JWT.
- Ter dois níveis de acesso:
	1. **Usuário** - acesso a plataforma e biblioteca de jogos.
	1. **Administrador** - é possível cadastrar jogos, administrar usuários e criar promoções.

**Arquitetura:**

- Para essa fase, como se trata de um MVP, é necessário utilizar um monolito para facilitar o desenvolvimento ágil.

### Requisitos técnicos

**Persistência de Dados (Entity Framework Core/MongoDB):**

- Utilizar Entity Framework Core para gerenciar os modelos de usuários e jogos.
- Aplicar migrations para a criação do banco de dados.
- **(Opcional)** Utilizar MongoDB para persistência dos dados.
- **(Opcional)** Utilizar Dapper para consultas de alta performance, caso necessário.

**Desenvolvimento de API com .NET 8:**

- Criar a API seguindo o padrão Minimal API ou Controllers MVC.
- Implementar Middleware para tratamento de erros e logs estruturados.
- Adicionar documentação com Swagger para expor os endpoints da API.
- **(Opcional)** Utilizar GraphQL para consulta avançada de jogos, permitindo filtragens dinâmicas.

**Qualidade de Software:**

- Criar testes unitários para validar as principais regras de negócio.
- Aplicar Test-Driven Development (TDD) ou Behavior-Driven Development (BDD) em pelo menos um dos módulos do projeto.

**Domain-Driven Design (DDD):**

- Modelar o domínio do projeto utilizando Event Storming para mapear os fluxos de usuários e jogos.
- **(Opcional)** Aplicar Domain Storytelling para representar cenários de interação com a API.
- Seguir os princípios de DDD na organização das entidades e regras de negócio.

### Entregáveis da Fase 1

- **Vídeo de até 15 minutos** demonstrando todos os requisitos. Ele pode ser em grupo ou individual (um integrante do grupo grava ou é possível se dividir entre si e apresentar).
	- O projeto pode rodar localmente, apresentando os requisitos anteriores.
	- Se o requisito técnico estiver com a flag **(Opcional)**, isso significa que caso ele não seja implementado não descontaremos pontos.
	- **(Opcional)** Se o MongoDB for utilizado, será necessário mostrar a estrutura do banco.
- **Documentação DDD** (Miro ou equivalente), com:
	- Event Storming dos fluxos:
		- Criação de jogos.
		- Criação de usuários.
	- Diagramas conforme apresentado na disciplina de DDD.
- **Código-fonte** no repositório (público ou privado), incluindo:
	- APIs conforme requisitos.
	- Testes escritos.
	- README.md completo com instruções de uso e objetivos.
- **Relatório de entrega (PDF ou TXT)** - esse arquivo deve ser postado na data da entrega, contendo:
	- Nome do grupo.
	- Participantes e usernames no Discord.
	- Link da documentação.
	- Link do(s) repositório(s).
	- Link do vídeo salvo no Youtube ou lugar de sua preferência.

> Lembramos que caso você tenha qualquer dúvida, é só nos chamar no Discord!
