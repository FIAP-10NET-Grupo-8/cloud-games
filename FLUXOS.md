# Fluxos do Sistema

Documento com os principais fluxos do sistema, descrevendo as etapas, caminhos felizes e alternativos, e os resultados esperados.

---

## Sumário

- [Autenticação e Autorização](#autenticacao-e-autorizacao)
	- [Login](#fluxo-de-login)
	- [Registro](#fluxo-de-registro)
	- [Confirmação de E-mail](#fluxo-de-confirmacao-de-e-mail)
	- [Esqueci a Senha](#fluxo-de-esqueci-a-senha)
	- [Redefinição de Senha](#fluxo-de-redefinicao-de-senha)
	- [Visualização de Perfil do Usuário](#fluxo-de-visualizacao-de-perfil-do-usuario)
- [Administração de Usuários](#administracao-de-usuarios)
	- [Criação de Usuário (Admin)](#fluxo-de-criacao-de-usuario-admin)
	- [Primeiro Acesso do Usuário](#fluxo-de-primeiro-acesso-do-usuario)
	- [Edição de Usuário (Admin)](#fluxo-de-edicao-de-usuario-admin)
	- [Exclusão de Usuário (Admin)](#fluxo-de-exclusao-de-usuario-admin)
	- [Restauração de Conta (Admin)](#fluxo-de-restauracao-de-conta-admin)
	- [Bloqueio/Desbloqueio de Conta (Admin)](#fluxo-de-bloqueio-desbloqueio-de-conta-admin)
	- [Listagem de Usuários (Admin)](#fluxo-de-listagem-de-usuarios-admin)
- [Gerenciamento de Pedidos](#gerenciamento-de-pedidos)
  - [Criação de Pedido](#fluxo-de-criacao-de-pedido)
  - [Consulta de Pedidos](#fluxo-de-consulta-de-pedidos)
  - [Marcar Pedido como Pago](#fluxo-de-marcar-pedido-pago)
  - [Solicitar Estorno](#fluxo-de-solicitar-estorno)
  - [Marcar Pedido como Estornado](#fluxo-de-marcar-estornado)
  - [Cancelamento de Pedido](#fluxo-de-cancelamento-de-pedido)

---

## Diretriz rápida para adição de novas categorias

- Mantenha cada categoria em uma seção separada com o nome claro.
- Adicione novos fluxos dentro da categoria correspondente e crie uma nova seção de categoria/fluxo no Sumário.
- Use o template de fluxo (abaixo) para consistência.

---

### Nome do fluxo {#id-do-fluxo}

#### Caminho feliz - Descrição curta

1. Passo 1.
1. Passo 2.
1. etc.

**Resultado esperado:** Descrição curta do resultado. (opcional)

#### Caminhos alternativos

- Situação/Falha 1: Descrição curta da situação/falha.
- Situação/Falha 2: Descrição curta da situação/falha.

> Copie este template para adicionar novos fluxos ou categorias.

---

## Autenticação e Autorização {#autenticacao-e-autorizacao}

### Login {#fluxo-de-login}

#### Caminho feliz — Login bem-sucedido

1. Usuário abre a página de login.
1. Insere e-mail e senha.
1. Sistema valida credenciais.
1. Se válidas, gera um token de autenticação (JWT) contendo: `ID do usuário`, `Nome`, `Papel`.
1. Retorna o token ao cliente.
1. Cliente armazena o token (ex.: `localStorage`, cookie seguro).
1. Usuário acessa áreas protegidas usando o token.

**Resultado esperado:** usuário autenticado com token válido.

#### Caminhos alternativos

- Credenciais inválidas: retorna erro informando e-mail ou senha incorretos.
- Conta inativa: retorna orientação para ativação por e-mail.
- Conta bloqueada: retorna instrução para contatar suporte.
- Conta excluída: informa que conta foi excluída e não pode acessar; sugere reativação ou novo cadastro.

---

### Registro {#fluxo-de-registro}

#### Caminho feliz — Registro bem-sucedido

1. Usuário abre página de registro.
1. Preenche nome, e-mail e senha.
1. Sistema valida dados.
1. Se válidos, cria conta e gera token de confirmação.
1. Envia e-mail de confirmação.
1. Usuário é orientado a ativar a conta pelo e-mail.

**Resultado esperado:** conta criada em estado `inactive` até confirmação por e-mail.

#### Caminhos alternativos

- E-mail já cadastrado: retorna erro e opções (login, recuperar senha, usar outro e-mail).
- Dados inválidos: retorna campos com problemas (formato de e-mail, senha fraca, etc.).

---

### Confirmação de E-mail {#fluxo-de-confirmacao-de-e-mail}

#### Caminho feliz — Confirmação bem-sucedida

1. Usuário clica no link de confirmação.
1. Sistema valida token.
1. Se válido, ativa a conta e remove token.
1. Redireciona para login com mensagem de sucesso.

**Resultado esperado:** conta ativada e pronta para login.

#### Caminhos alternativos

- Token inválido/expirado: orientar a solicitar novo link.
- Conta já ativada: informar que já está confirmada.

---

### Esqueci a Senha {#fluxo-de-esqueci-a-senha}

#### Caminho feliz — Recuperação bem-sucedida

1. Usuário solicita recuperação informando o e-mail.
1. Sistema verifica existência do e-mail.
1. Se cadastrado, gera token de redefinição e envia link por e-mail.
1. Usuário é orientado a verificar o e-mail para redefinir a senha.

**Resultado esperado:** usuário recebe link seguro para redefinir senha.

#### Caminhos alternativos

- E-mail não cadastrado: informar que e-mail não foi encontrado.
- Formato de e-mail inválido: informar erro de validação.

---

### Visualização de Perfil do Usuário {#fluxo-de-visualizacao-de-perfil-do-usuario}

#### Caminho feliz — Visualização bem-sucedida

1. Usuário autenticado abre seção de perfil.
1. Sistema recupera e exibe informações do perfil.

#### Caminho alternativo — Usuário não autenticado

- Redirecionar para a página de login.

---

### Redefinição de Senha {#fluxo-de-redefinicao-de-senha}

#### Caminho feliz — Redefinição bem-sucedida

1. Usuário acessa link de redefinição.
1. Sistema valida token.
1. Se válido, exibe formulário para nova senha.
1. Usuário fornece e confirma a nova senha.
1. Sistema valida e atualiza a senha, removendo o token.
1. Redireciona para login com mensagem de sucesso.

#### Caminhos alternativos

- Token inválido/expirado: solicitar novo link.
- Senha inválida: exibir critérios não atendidos.

---

## Administração de Usuários {#administracao-de-usuarios}

### Criação de Usuário (Admin) {#fluxo-de-criacao-de-usuario-admin}

#### Caminho feliz — Criação bem-sucedida

1. Admin abre painel de administração → gerenciamento de usuários.
1. Seleciona criar usuário e preenche dados (nome, e-mail, papel, status).
1. Sistema valida dados e cria conta; gera token de primeiro acesso.
1. Envia e-mail ao novo usuário com instruções.

**Resultado esperado:** usuário criado em estado que exige definir a senha/primeiro acesso.

#### Caminhos alternativos

- E-mail já cadastrado: retornar erro; permitir editar usuário existente.
- Dados inválidos: retornar campos a corrigir.

---

### Primeiro Acesso do Usuário {#fluxo-de-primeiro-acesso-do-usuario}

#### Caminho feliz — Primeiro acesso bem-sucedido

1. Usuário clica no link de primeiro acesso e sistema valida token.
1. Se válido, exibe formulário para definir senha.
1. Usuário define senha; sistema valida e salva, removendo o token.
1. Redireciona para login.

#### Caminhos alternativos

- Token inválido/expirado: solicitar novo link.
- Primeiro acesso já realizado: informar que a senha já foi definida.

---

### Edição de Usuário (Admin) {#fluxo-de-edicao-de-usuario-admin}

#### Caminho feliz — Edição bem-sucedida

1. Admin seleciona usuário para editar.
1. Altera nome, e-mail, papel ou status.
1. Sistema valida e salva alterações.
1. Se e-mail alterado, enviar confirmação para o novo e-mail.

#### Caminhos alternativos

- E-mail já cadastrado: retornar erro.
- Dados inválidos: retornar validações a corrigir.

---

### Exclusão de Usuário (Admin) {#fluxo-de-exclusao-de-usuario-admin}

#### Caminho feliz — Exclusão bem-sucedida

1. Admin seleciona usuário e confirma exclusão.
1. Sistema marca status como `deleted`.
1. Impedir login do usuário excluído e enviar notificação por e-mail.

#### Caminhos alternativos

- Usuário não encontrado: retornar erro.
- Usuário já excluído: informar que não é possível excluir novamente.

---

### Restauração de Conta (Admin) {#fluxo-de-restauracao-de-conta-admin}

#### Caminho feliz — Restauração bem-sucedida

1. Admin seleciona usuário excluído e confirma restauração.
1. Sistema altera status para `active` e notifica o usuário.

#### Caminhos alternativos

- Usuário não encontrado: retornar erro.
- Usuário já ativo: informar que não há necessidade de restauração.

---

### Bloqueio/Desbloqueio de Conta (Admin) {#fluxo-de-bloqueio-desbloqueio-de-conta-admin}

#### Caminho feliz — Bloqueio/Desbloqueio bem-sucedido

1. Admin seleciona usuário e escolhe bloquear ou desbloquear.
1. Sistema atualiza status para `blocked` ou `active`.
1. Notificar usuário sobre a ação.

#### Caminhos alternativos

- Usuário não encontrado: retornar erro.
- Status incompatível (ex.: `deleted`): informar ação inválida.

---

### Listagem de Usuários (Admin) {#fluxo-de-listagem-de-usuarios-admin}

#### Caminho feliz — Listagem bem-sucedida

1. Admin acessa gerenciamento de usuários.
1. Sistema recupera e apresenta lista de usuários do banco de dados.

---

## Gerenciamento de Pedidos {#gerenciamento-de-pedidos}

### Criação de Pedido {#fluxo-de-criacao-de-pedido}

#### Caminho feliz — Pedido criado com sucesso
1. Sistema recebe solicitação com dados do usuário e itens, podendo incluir um header `Idempotency-Key`.
1. Sistema valida se o usuário autenticado é o dono do pedido.
1. Se válidos, cria o pedido, calcula o total e define o status como `PendingPayment`.
1. Retorna o pedido criado com seus detalhes.

**Resultado esperado:** pedido criado com status `PendingPayment`.

#### Caminhos alternativos

- Dados inválidos: retornar campos a corrigir.
- Usuário não autenticado ou incorreto: retornar 403.
- Requisição duplicada (mesma `Idempotency-Key`): retornar o mesmo pedido criado.
- Itens indisponíveis ou erro no total: retornar erro informando o problema.

---

### Consulta de Pedidos {#fluxo-de-consulta-de-pedidos}

#### Caminho feliz — Consulta bem-sucedida
1. Usuário autenticado (dono) ou administrador requisita seus pedidos.
1. Sistema aplica filtros (`status`, `page`, `pageSize`) e retorna lista paginada ou pedido por ID.

**Resultado esperado:** lista ou detalhe de pedidos conforme permissão do usuário.

#### Caminhos alternativos
- Pedido não encontrado: retornar 404.
- Acesso não autorizado: retornar 403.

---

### Solicitar Estorno {#fluxo-de-solicitar-estorno}

#### Caminho feliz — Solicitação registrada
1. Dono (ou admin) solicita estorno de um pedido `Paid`.
1. Sistema valida a permissão, status e prazo.
1. Atualiza o pedido para `RefundRequested` e registra motivo e data.

**Resultado esperado:** estorno solicitado com sucesso.

#### Caminhos alternativos
- Pedido não encontrado ou fora do prazo: retornar erro.
- Já estornado, cancelado ou com solicitação prévia: retornar estado incompatível.

---

> Observação: o documento está propositalmente focado nas categorias já modeladas. Quando for adicionar novas categorias, copie o "Template compacto para novo fluxo" e crie uma nova seção de categoria no Sumário.
