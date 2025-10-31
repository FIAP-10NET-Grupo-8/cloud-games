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
- [Gerenciamento de Jogos](#gerenciamento-de-jogos)
	- [Criação de Jogo (Admin)](#fluxo-de-criacao-de-jogo-admin)
	- [Edição de Jogo (Admin)](#fluxo-de-edicao-de-jogo-admin)
	- [Exclusão de Jogo (Admin)](#fluxo-de-exclusao-de-jogo-admin)
	- [Listagem de Jogos (Admin)](#fluxo-de-listagem-de-jogos-admin)
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

<a id="id-da-categoria"></a>
## Categoria de Exemplo (opcional)

<a id="id-do-fluxo"></a>
### Nome do fluxo

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

<a id="autenticacao-e-autorizacao"></a>
## Autenticação e Autorização

<a id="fluxo-de-login"></a>
### Login

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

<a id="fluxo-de-registro"></a>
### Registro

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

<a id="fluxo-de-confirmacao-de-e-mail"></a>
### Confirmação de E-mail

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

<a id="fluxo-de-esqueci-a-senha"></a>
### Esqueci a Senha

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

<a id="fluxo-de-visualizacao-de-perfil-do-usuario"></a>
### Visualização de Perfil do Usuário

#### Caminho feliz — Visualização bem-sucedida

1. Usuário autenticado abre seção de perfil.
1. Sistema recupera e exibe informações do perfil.

#### Caminho alternativo — Usuário não autenticado

- Redirecionar para a página de login.

---

<a id="fluxo-de-redefinicao-de-senha"></a>
### Redefinição de Senha

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

<a id="administracao-de-usuarios"></a>
## Administração de Usuários

<a id="fluxo-de-criacao-de-usuario-admin"></a>
### Criação de Usuário (Admin)

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

<a id="fluxo-de-primeiro-acesso-do-usuario"></a>
### Primeiro Acesso do Usuário

#### Caminho feliz — Primeiro acesso bem-sucedido

1. Usuário clica no link de primeiro acesso e sistema valida token.
1. Se válido, exibe formulário para definir senha.
1. Usuário define senha; sistema valida e salva, removendo o token.
1. Redireciona para login.

#### Caminhos alternativos

- Token inválido/expirado: solicitar novo link.
- Primeiro acesso já realizado: informar que a senha já foi definida.

---

<a id="fluxo-de-edicao-de-usuario-admin"></a>
### Edição de Usuário (Admin)

#### Caminho feliz — Edição bem-sucedida

1. Admin seleciona usuário para editar.
1. Altera nome, e-mail, papel ou status.
1. Sistema valida e salva alterações.
1. Se e-mail alterado, enviar confirmação para o novo e-mail.

#### Caminhos alternativos

- E-mail já cadastrado: retornar erro.
- Dados inválidos: retornar validações a corrigir.

---

<a id="fluxo-de-exclusao-de-usuario-admin"></a>
### Exclusão de Usuário (Admin)

#### Caminho feliz — Exclusão bem-sucedida

1. Admin seleciona usuário e confirma exclusão.
1. Sistema marca status como `deleted`.
1. Impedir login do usuário excluído e enviar notificação por e-mail.

#### Caminhos alternativos

- Usuário não encontrado: retornar erro.
- Usuário já excluído: informar que não é possível excluir novamente.

---

<a id="fluxo-de-restauracao-de-conta-admin"></a>
### Restauração de Conta (Admin)

#### Caminho feliz — Restauração bem-sucedida

1. Admin seleciona usuário excluído e confirma restauração.
1. Sistema altera status para `active` e notifica o usuário.

#### Caminhos alternativos

- Usuário não encontrado: retornar erro.
- Usuário já ativo: informar que não há necessidade de restauração.

---

<a id="fluxo-de-bloqueio-desbloqueio-de-conta-admin"></a>
### Bloqueio/Desbloqueio de Conta (Admin)

#### Caminho feliz — Bloqueio/Desbloqueio bem-sucedido

1. Admin seleciona usuário e escolhe bloquear ou desbloquear.
1. Sistema atualiza status para `blocked` ou `active`.
1. Notificar usuário sobre a ação.

#### Caminhos alternativos

- Usuário não encontrado: retornar erro.
- Status incompatível (ex.: `deleted`): informar ação inválida.

---

<a id="fluxo-de-listagem-de-usuarios-admin"></a>
### Listagem de Usuários (Admin)

#### Caminho feliz — Listagem bem-sucedida

1. Admin acessa gerenciamento de usuários.
1. Sistema recupera e apresenta lista de usuários do banco de dados.

---

<a id="gerenciamento-de-jogos"></a>
## Gerenciamento de Jogos

<a id="fluxo-de-criacao-de-jogo-admin"></a>
### Criação de Jogo (Admin)

#### Caminho feliz - Criação bem-sucedida

1. Admin acessa painel de administração → gerenciamento de jogos.
1. Seleciona criar novo jogo e preenche detalhes (nome, descrição, categoria, etc.).
1. Sistema valida dados e salva o novo jogo no banco de dados.
1. Confirmação de criação é exibida.

**Resultado esperado:** novo jogo criado e disponível no sistema.

#### Caminhos alternativos

- Dados inválidos: retornar campos a corrigir.
- Jogo já existente: retornar erro informando duplicidade.


---

<a id="fluxo-de-edicao-de-jogo-admin"></a>
### Edição de Jogo (Admin)

#### Caminho feliz - Edição bem-sucedida

1. Admin seleciona jogo existente para editar.
1. Altera detalhes do jogo conforme necessário.
1. Sistema valida e salva as alterações.
1. Confirmação de edição é exibida.

#### Caminhos alternativos

- Jogo não encontrado: retornar erro.
- Dados inválidos: retornar campos a corrigir.
- Jogo com nome duplicado: retornar erro informando duplicidade.

---

<a id="fluxo-de-exclusao-de-jogo-admin"></a>
### Exclusão de Jogo (Admin)

#### Caminho feliz - Exclusão bem-sucedida
1. Admin seleciona jogo para exclusão e confirma a ação.
1. Sistema remove o jogo do banco de dados.
1. Confirmação de exclusão é exibida.

#### Caminhos alternativos

- Jogo não encontrado: retornar erro.

---

<a id="fluxo-de-listagem-de-jogos-admin"></a>
### Listagem de Jogos (Admin)

#### Caminho feliz - Listagem bem-sucedida

1. Admin acessa gerenciamento de jogos.
1. Sistema recupera e apresenta lista de jogos do banco de dados.

---

<a id="gerenciamento-de-pedidos"></a>
## Gerenciamento de Pedidos

<a id="fluxo-de-criacao-de-pedido"></a>
### Criação de Pedido

#### Caminho feliz — Pedido criado com sucesso

1. Sistema recebe solicitação de criação de pedido com detalhes do usuário e itens (pode incluir o header `Idempotency-Key`).
1. Sistema valida dados do usuário e lista de itens.
1. Se válidos, cria o pedido, calcula o total e define o status como `PendingPayment`.
1. Retorna confirmação com detalhes do pedido.

**Resultado esperado:** pedido criado com status `PendingPayment`.

#### Caminhos alternativos

- Dados inválidos: retornar campos a corrigir.
- Itens indisponíveis: retornar erro informando quais itens não estão disponíveis.
- Usuário não encontrado: retornar erro informando que o usuário não existe.
- Erro no cálculo do total: retornar erro informando falha no processamento do pedido.
- Requisição duplicada (mesma `Idempotency-Key`): retornar o mesmo pedido previamente processado.

---

<a id="fluxo-de-marcar-pedido-pago"></a>
### Marcar Pedido como Pago

#### Caminho feliz — Pagamento confirmado

1. Sistema de pagamento notifica o sistema sobre o pagamento bem-sucedido.
1. Sistema valida a notificação e atualiza o status do pedido para `paid`.
1. Se válido, envia confirmação ao usuário.

**Resultado esperado:** pedido atualizado para status `paid`.

#### Caminhos alternativos

- Notificação inválida: retornar erro informando falha na validação.
- Pedido não encontrado: retornar erro informando que o pedido não existe.
- Pedido já pago: informar que o pedido já foi pago.
- Pedido cancelado: informar que o pedido foi cancelado e não pode ser marcado como pago.
- Pedido estornado: informar que o pedido foi estornado e não pode ser marcado como pago.

---

<a id="fluxo-de-solicitar-estorno"></a>
### Solicitar Estorno

#### Caminho feliz — Solicitação de estorno registrada

1. Usuário solicita estorno para um pedido pago.
1. Sistema valida a solicitação e o status do pedido.
1. Se válido, registra a solicitação de estorno e notifica o sistema de pagamentos.
1. Sistema notifica time de pagamentos para processamento.
1. Confirmação é enviada ao usuário.

**Resultado esperado:** solicitação de estorno registrada com sucesso.

#### Caminhos alternativos

- Pedido não encontrado: retornar erro informando que o pedido não existe.
- Pedido não pago: retornar erro informando que apenas pedidos pagos podem ser estornados.
- Pedido já estornado: informar que o pedido já foi estornado.
- Pedido cancelado: informar que pedidos cancelados não podem ser estornados.
- Solicitação duplicada: informar que já existe uma solicitação de estorno em andamento para este pedido.

---

<a id="fluxo-de-marcar-estornado"></a>
### Marcar Pedido como Estornado

#### Caminho feliz — Estorno concluído

1. Sistema de pagamento notifica o sistema sobre o estorno bem-sucedido.
1. Sistema valida a notificação.
1. Se válido, atualiza o status do pedido para `refunded`.
1. Notifica o usuário sobre o estorno concluído.

**Resultado esperado:** pedido atualizado para status `refunded`.

#### Caminhos alternativos

- Notificação inválida: retornar erro informando falha na validação.
- Pedido não encontrado: retornar erro informando que o pedido não existe.
- Pedido não pago: retornar erro informando que apenas pedidos pagos podem ser estornados.
- Pedido já estornado: informar que o pedido já foi estornado.
- Pedido cancelado: informar que pedidos cancelados não podem ser estornados.
- Estorno não solicitado: informar que não há solicitação de estorno para este pedido.

---

<a id="fluxo-de-cancelamento-de-pedido"></a>
### Cancelamento de Pedido

#### Caminho feliz — Cancelamento bem-sucedido

1. Usuário solicita cancelamento de um pedido pendente.
1. Sistema valida a solicitação e o status do pedido.
1. Se válido, atualiza o status do pedido para `Cancelled`.
1. Notifica o usuário sobre o cancelamento.

**Resultado esperado:** pedido atualizado para status `Cancelled`.

#### Caminhos alternativos

- Pedido não encontrado: retornar erro informando que o pedido não existe.
- Pedido já cancelado: informar que o pedido já foi cancelado.
- Pedido pago: informar que pedidos pagos não podem ser cancelados.
- Pedido estornado: informar que pedidos estornados não podem ser cancelados.
- Solicitação duplicada: informar que o pedido já está em processo de cancelamento.

---

> Observação: o documento está propositalmente focado nas categorias já modeladas. Quando for adicionar novas categorias, copie o "Template compacto para novo fluxo" e crie uma nova seção de categoria no Sumário.
