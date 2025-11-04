# Documentação da API Fiap Cloud Games - Fase 1

Esta documentação detalha os fluxos, endpoints, exemplos de requisições e as respectivas respostas de sucesso (códigos 2xx) para a API Fiap Cloud Games.

## 1. Configuração e Variáveis

| Variável | Descrição | Valor Padrão (Exemplo) | Fonte |
| :--- | :--- | :--- | :--- |
| `{{baseUrl}}` | URL base da API | `/` | |
| `{{bearerToken}}` | Token JWT usado para autenticação Bearer | `""` | |

A autenticação é feita via **Bearer Token** (JWT) na maioria dos endpoints.

---

## 2. Fluxo: Usuários e Autenticação (Self-Service)

Caminho base: `{{baseUrl}}/api/Users`

### **2.1. Login**
Autentica um usuário e retorna um JWT (JSON Web Token).

| Método | URL |
| :--- | :--- |
| `POST` | `{{baseUrl}}/api/Users/login` |

#### Exemplo de Request Body (application/json)
```json
{
 "email": "string",
 "password": "string"
}
```

#### Resposta de Sucesso (200 OK)
```json
{
 "token": "string",
 "expiresAt": "1958-11-05T12:52:17.388Z"
}
```

### **2.2. Auto-registro**
Registra um novo usuário (self-signup).

| Método | URL |
| :--- | :--- |
| `POST` | `{{baseUrl}}/api/Users/register` |

#### Exemplo de Request Body (application/json)
```json
{
 "name": "string",
 "email": "string",
 "password": "string"
}
```

#### Resposta de Sucesso (201 Created)
```json
{
 "id": "ef2ca168-b9d2-ab9a-ce5b-87543388d33d",
 "name": "string",
 "email": "string",
 "role": "Administrator",
 "emailConfirmed": false,
 "createdAt": "1997-02-06T23:49:21.003Z"
}
```

### **2.3. Confirmação de Email**
Confirma o email de um usuário com token.

| Método | URL |
| :--- | :--- |
| `POST` | `{{baseUrl}}/api/Users/confirm` |

#### Exemplo de Request Body (application/json)
```json
{
 "token": "string"
}
```

#### Resposta de Sucesso (200 OK)
```json
{
 "statusCode": 1645,
 "message": "string"
}
```

### **2.4. Primeiro Acesso**
Define a senha usando um token recebido por email.

| Método | URL |
| :--- | :--- |
| `POST` | `{{baseUrl}}/api/Users/first-access` |

#### Exemplo de Request Body (application/json)
```json
{
 "token": "string",
 "newPassword": "string"
}
```

#### Resposta de Sucesso (200 OK)
```json
{
 "statusCode": 1645,
 "message": "string"
}
```

### **2.5. Esqueceu a Senha (Solicitação)**
Solicita um token de redefinição de senha para o email informado.

| Método | URL |
| :--- | :--- |
| `POST` | `{{baseUrl}}/api/Users/forgot-password` |

#### Exemplo de Request Body (application/json)
```json
{
 "email": "string"
}
```

#### Resposta de Sucesso (200 OK)
```json
{
 "resetToken": "string"
}
```

### **2.6. Redefinição de Senha**
Redefine a senha utilizando token enviado por email.

| Método | URL |
| :--- | :--- |
| `POST` | `{{baseUrl}}/api/Users/reset-password` |

#### Exemplo de Request Body (application/json)
```json
{
 "token": "string",
 "newPassword": "string"
}
```

#### Resposta de Sucesso (200 OK)
```json
{
 "statusCode": 1645,
 "message": "string"
}
```

### **2.7. Obter Dados do Usuário Autenticado**
Obtém os dados do usuário a partir do token JWT.

| Método | URL |
| :--- | :--- |
| `GET` | `{{baseUrl}}/api/Users/me` |

#### Resposta de Sucesso (200 OK)
```json
{
 "id": "ef2ca168-b9d2-ab9a-ce5b-87543388d33d",
 "name": "string",
 "email": "string",
 "role": "Administrator",
 "emailConfirmed": false,
 "createdAt": "1997-02-06T23:49:21.003Z"
}
```

---

## 3. Fluxo: Gerenciamento de Usuários (Administrativo)

Caminho base: `{{baseUrl}}/api/Users`. Requer autenticação com **role Administrator**.

### **3.1. Listar Todos os Usuários**
Lista todos os usuários (necessário role Administrator).

| Método | URL |
| :--- | :--- |
| `GET` | `{{baseUrl}}/api/Users` |

#### Resposta de Sucesso (200 OK)
```json
[
 {
  "id": "f2426c9e-80dc-c58d-68d1-1ec62fcef81d",
  "name": "string",
  "email": "string",
  "role": "User",
  "emailConfirmed": true,
  "createdAt": "1990-05-31T22:23:37.385Z"
 },
 {
  "id": "274792c4-949b-4880-144b-6d150e8933d0",
  "name": "string",
  "email": "string",
  "role": "User",
  "emailConfirmed": true,
  "createdAt": "2017-09-11T11:04:23.086Z"
 }
]
```

### **3.2. Obter Usuário por ID**
Obtém um usuário pelo identificador (necessário role Administrator).

| Método | URL | Parâmetro de Path |
| :--- | :--- | :--- |
| `GET` | `{{baseUrl}}/api/Users/:id` | `id` (GUID) |

#### Resposta de Sucesso (200 OK)
```json
{
 "id": "ef2ca168-b9d2-ab9a-ce5b-87543388d33d",
 "name": "string",
 "email": "string",
 "role": "Administrator",
 "emailConfirmed": false,
 "createdAt": "1997-02-06T23:49:21.003Z"
}
```

### **3.3. Criar Novo Usuário (Administrativo)**
Cria um usuário com privilégios administrativos (necessário role Administrator).

| Método | URL |
| :--- | :--- |
| `POST` | `{{baseUrl}}/api/Users` |

#### Exemplo de Request Body (application/json)
```json
{
 "name": "string",
 "email": "string",
 "role": "User"
}
```

#### Resposta de Sucesso (201 Created)
```json
{
 "id": "ef2ca168-b9d2-ab9a-ce5b-87543388d33d",
 "name": "string",
 "email": "string",
 "role": "Administrator",
 "emailConfirmed": false,
 "createdAt": "1997-02-06T23:49:21.003Z"
}
```

### **3.4. Atualizar Usuário**
Atualiza um usuário (necessário role Administrator).

| Método | URL |
| :--- | :--- |
| `PUT` | `{{baseUrl}}/api/Users` |

#### Exemplo de Request Body (application/json)
```json
{
 "id": "83ce6d97-3499-8396-f2b3-90bf22a97daa",
 "name": "string",
 "email": "string",
 "role": "Administrator",
 "status": "Deleted",
 "emailConfirmed": false
}
```

#### Resposta de Sucesso (200 OK)
```json
{
 "id": "ef2ca168-b9d2-ab9a-ce5b-87543388d33d",
 "name": "string",
 "email": "string",
 "role": "Administrator",
 "emailConfirmed": false,
 "createdAt": "1997-02-06T23:49:21.003Z"
}
```

### **3.5. Soft-Delete (Exclusão Lógica)**
Marca usuário como `Deleted` (necessário role Administrator).

| Método | URL | Parâmetro de Path |
| :--- | :--- | :--- |
| `DELETE` | `{{baseUrl}}/api/Users/:id` | `id` (GUID) |

#### Resposta de Sucesso (204 No Content)
*Corpo da resposta vazio (indica sucesso sem conteúdo para retornar).*

### **3.6. Restaurar Usuário**
Restaura um usuário deletado (necessário role Administrator).

| Método | URL | Parâmetro de Path |
| :--- | :--- | :--- |
| `POST` | `{{baseUrl}}/api/Users/:id/restore` | `id` (GUID) |

#### Resposta de Sucesso (200 OK)
```json
{
 "statusCode": 1645,
 "message": "string"
}
```

---

## 4. Fluxo: Catálogo de Jogos (Games)

Caminho base: `{{baseUrl}}/api/Games`

### **4.1. Listar Todos os Jogos**
Lista todos os jogos.

| Método | URL |
| :--- | :--- |
| `GET` | `{{baseUrl}}/api/Games` |

#### Resposta de Sucesso (200 OK)
```json
[
 {
  "id": "e31507cc-51e3-6970-7ef1-0ee6c401bf39",
  "title": "string",
  "price": 739.52177097937,
  "active": false
 },
 {
  "id": "205f495e-ad6b-d555-f4de-cc9ddd5f74a6",
  "title": "string",
  "price": 5275.296682040443,
  "active": true
 }
]
```

### **4.2. Buscar Jogo por ID**
Busca um jogo pelo identificador.

| Método | URL | Parâmetro de Path |
| :--- | :--- | :--- |
| `GET` | `{{baseUrl}}/api/Games/:id` | `id` (GUID) |

#### Resposta de Sucesso (200 OK)
```json
{
 "id": "09018011-17ba-453a-5d1a-31736e85a16e",
 "title": "string",
 "description": "string",
 "price": 1634.6581052557108,
 "releaseDate": "1953-10-12T21:30:20.317Z",
 "developer": "string",
 "publisher": "string",
 "genre": "string",
 "platforms": "string",
 "active": false,
 "createdAt": "1971-09-17T17:25:08.209Z",
 "updatedAt": "1994-10-30T21:10:28.056Z"
}
```

### **4.3. Cadastrar Novo Jogo (Administrativo)**
Cadastra um novo jogo.

| Método | URL |
| :--- | :--- |
| `POST` | `{{baseUrl}}/api/Games` |

#### Exemplo de Request Body (application/json)
```json
{
 "title": "string",
 "description": "string",
 "price": 9336.219931661553,
 "releaseDate": "1970-09-26T03:40:24.299Z",
 "developer": "string",
 "publisher": "string",
 "genre": "string",
 "platforms": "string"
}
```

#### Resposta de Sucesso (201 Created)
```json
{
 "id": "09018011-17ba-453a-5d1a-31736e85a16e",
 "title": "string",
 "description": "string",
 "price": 1634.6581052557108,
 "releaseDate": "1953-10-12T21:30:20.317Z",
 "developer": "string",
 "publisher": "string",
 "genre": "string",
 "platforms": "string",
 "active": false,
 "createdAt": "1971-09-17T17:25:08.209Z",
 "updatedAt": "1994-10-30T21:10:28.056Z"
}
```

### **4.4. Atualizar Jogo Existente**
Atualiza um jogo existente.

| Método | URL | Parâmetro de Path |
| :--- | :--- | :--- |
| `PUT` | `{{baseUrl}}/api/Games/:id` | `id` (GUID) |

#### Exemplo de Request Body (application/json)
```json
{
 "title": "string",
 "description": "string",
 "price": 2516.9256925941254,
 "releaseDate": "2021-09-26T12:01:04.820Z",
 "developer": "string",
 "publisher": "string",
 "genre": "string",
 "platforms": "string",
 "active": false
}
```

#### Resposta de Sucesso (204 No Content)
*Corpo da resposta vazio (indica sucesso sem conteúdo para retornar).*

### **4.5. Excluir Jogo**
Exclui um jogo.

| Método | URL | Parâmetro de Path |
| :--- | :--- | :--- |
| `DELETE` | `{{baseUrl}}/api/Games/:id` | `id` (GUID) |

#### Resposta de Sucesso (204 No Content)
*Corpo da resposta vazio.*

---

## 5. Fluxo: Promoções (Promotions)

Caminho base: `{{baseUrl}}/api/Promotions`

### **5.1. Listar Todas as Promoções**
Lista todas as promoções.

| Método | URL |
| :--- | :--- |
| `GET` | `{{baseUrl}}/api/Promotions` |

#### Resposta de Sucesso (200 OK)
```json
[
 {
  "id": "8392b48e-51b1-a8c8-cb47-c3f7808bfbf7",
  "name": "string",
  "startDate": "2005-11-21T10:40:43.961Z",
  "endDate": "1994-04-22T17:45:59.139Z",
  "discount": 7125.400927544101,
  "applicableGames": [
   {
    "id": "dc876516-6809-516e-e000-59ac9407c417",
    "title": "string",
    "price": 6526.598877957111,
    "active": true
   }
  ],
  "status": "Scheduled"
 },
 {
  "id": "99f40a58-27b8-fd92-8ac2-783661ca4d22",
  "name": "string",
  "startDate": "2009-10-23T21:38:34.328Z",
  "endDate": "2018-06-15T12:31:46.752Z",
  "discount": 7736.875232240303,
  "applicableGames": [
   {
    "id": "9a5d583b-9bc7-e21b-b6e1-cac9de749ac9",
    "title": "string",
    "price": 7935.091033604877,
    "active": false
   }
  ],
  "status": "Inactive"
 }
]
```

### **5.2. Obter Promoção por ID**
Obtém uma promoção pelo identificador.

| Método | URL | Parâmetro de Path |
| :--- | :--- | :--- |
| `GET` | `{{baseUrl}}/api/Promotions/:id` | `id` (GUID) |

#### Resposta de Sucesso (200 OK)
```json
{
 "id": "f0102c1b-7229-886d-2c1f-3aa308ee1639",
 "name": "string",
 "startDate": "1962-06-04T23:13:16.193Z",
 "endDate": "1955-11-25T01:38:05.565Z",
 "discount": 6108.333319899743,
 "applicableGames": [
  {
   "id": "9a5dd661-54f3-c279-e3c5-824875cc74a8",
   "title": "string",
   "price": 9973.45136196008,
   "active": false
  },
  {
   "id": "2bf68300-1cd6-b97e-21b4-517e350bd08e",
   "title": "string",
   "price": 581.9086507277071,
   "active": true
  }
 ],
 "status": "Inactive"
}
```

### **5.3. Criar Nova Promoção**
Cria uma nova promoção.

| Método | URL |
| :--- | :--- |
| `POST` | `{{baseUrl}}/api/Promotions` |

#### Exemplo de Request Body (application/json)
```json
{
 "name": "string",
 "startDate": "1991-11-25T15:05:28.643Z",
 "endDate": "1968-10-29T20:13:27.012Z",
 "discount": 8601.910065434051,
 "elligibleGames": [
  "b60a00d8-9621-529a-1190-1a7e69d98cf0",
  "0ef0aed0-efd1-7c48-53c3-721c8acc10a7"
 ]
}
```

#### Resposta de Sucesso (201 Created)
```json
{
 "id": "f0102c1b-7229-886d-2c1f-3aa308ee1639",
 "name": "string",
 "startDate": "1962-06-04T23:13:16.193Z",
 "endDate": "1955-11-25T01:38:05.565Z",
 "discount": 6108.333319899743,
 "applicableGames": [
  {
   "id": "9a5dd661-54f3-c279-e3c5-824875cc74a8",
   "title": "string",
   "price": 9973.45136196008,
   "active": false
  },
  {
   "id": "2bf68300-1cd6-b97e-21b4-517e350bd08e",
   "title": "string",
   "price": 581.9086507277071,
   "active": true
  }
 ],
 "status": "Inactive"
}
```

### **5.4. Atualizar Promoção Existente**
Atualiza uma promoção existente.

| Método | URL | Parâmetro de Path |
| :--- | :--- | :--- |
| `PUT` | `{{baseUrl}}/api/Promotions/:id` | `id` (GUID) |

#### Exemplo de Request Body (application/json)
```json
{
 "name": "string",
 "startDate": "1974-04-17T00:30:51.919Z",
 "endDate": "1967-08-25T15:39:04.116Z",
 "discount": 1129.0288395161108,
 "elligibleGames": [
  "dc89a53d-4081-7358-7951-e0e91e0e7454",
  "94e5c613-e6a7-b073-37f0-df4e86ad747a"
 ],
 "status": "Active"
}
```

#### Resposta de Sucesso (204 No Content)
*Corpo da resposta vazio.*

### **5.5. Desativar Promoção**
Desativa uma promoção.

| Método | URL | Parâmetro de Path |
| :--- | :--- | :--- |
| `DELETE` | `{{baseUrl}}/api/Promotions/:id` | `id` (GUID) |

#### Resposta de Sucesso (204 No Content)
*Corpo da resposta vazio.*

---

## 6. Fluxo: Carrinhos de Compra (Carts)

Caminho base: `{{baseUrl}}/api/Carts`

### **6.1. Listar Todos os Carrinhos (Auditoria)**
Lista todos os carrinhos (apenas Administrador). Suporta paginação.

| Método | URL | Parâmetros de Query |
| :--- | :--- | :--- |
| `GET` | `{{baseUrl}}/api/Carts?page=1&pageSize=50` | `page`, `pageSize` |

#### Resposta de Sucesso (200 OK)
```json
[
 {
  "cartId": "9536c159-596a-47e7-1bb1-3787bb1aa534",
  "userEmail": "string",
  "itemsCount": 9792,
  "total": 556.1047615301873,
  "updatedAt": "1973-09-24T14:28:22.611Z"
 },
 {
  "cartId": "e365ea9e-0dfa-fe12-c47f-48ef601dd09f",
  "userEmail": "string",
  "itemsCount": 6928,
  "total": 8447.942038902534,
  "updatedAt": "1980-08-18T10:30:22.215Z"
 }
]
```

### **6.2. Obter Meu Carrinho**
Obtém o carrinho do usuário autenticado (cria se não existir).

| Método | URL |
| :--- | :--- |
| `GET` | `{{baseUrl}}/api/Carts/mine` |

#### Resposta de Sucesso (200 OK)
```json
{
 "cartId": "877ea9e1-3ec1-29ec-fd5d-810bb15ee662",
 "userId": "02183db4-6064-fd7a-e787-ca5dc9b5dcc0",
 "userEmail": "string",
 "total": 813.397165061851,
 "updatedAt": "2008-07-18T12:31:26.009Z",
 "items": [
  {
   "gameId": "ee576288-cca5-57a6-4208-24c1094ec3db",
   "title": "string",
   "unitPrice": 8447.682729859798,
   "discount": 2502.153983159867,
   "finalPrice": 1100.8496873886031
  }
 ]
}
```

### **6.3. Adicionar Jogo ao Carrinho**
Adiciona um jogo ao carrinho do usuário autenticado.

| Método | URL |
| :--- | :--- |
| `POST` | `{{baseUrl}}/api/Carts/mine/items` |

#### Exemplo de Request Body (application/json)
```json
{
 "gameId": "f863bd05-230a-cdf1-78d2-33a9da272096"
}
```

#### Resposta de Sucesso (200 OK)
Retorna o objeto do carrinho atualizado, contendo os itens.

### **6.4. Remover Jogo do Carrinho**
Remove um jogo do carrinho do usuário autenticado.

| Método | URL | Parâmetro de Path |
| :--- | :--- | :--- |
| `DELETE` | `{{baseUrl}}/api/Carts/mine/items/:gameId` | `gameId` (GUID) |

#### Resposta de Sucesso (200 OK)
Retorna o objeto do carrinho atualizado.

### **6.5. Limpar Meu Carrinho**
Limpa todos os itens do carrinho do usuário autenticado.

| Método | URL |
| :--- | :--- |
| `DELETE` | `{{baseUrl}}/api/Carts/mine` |

#### Resposta de Sucesso (200 OK)
Retorna o objeto do carrinho, presumivelmente sem itens ou com os totais zerados.

---

## 7. Fluxo: Pedidos (Orders)

Caminho base: `{{baseUrl}}/api/Orders`

### **7.1. Criar Novo Pedido**
Cria um novo pedido a partir do carrinho/itens informados.

| Método | URL |
| :--- | :--- |
| `POST` | `{{baseUrl}}/api/Orders` |

#### Exemplo de Request Body (application/json)
```json
{
 "userId": "df454c34-8c0f-9707-58c4-019d7e54eb41",
 "items": [
  {
   "gameId": "19057e2c-7927-0abb-183c-d1ceaeefa553",
   "quantity": 8147,
   "unitPrice": 9351.824193492688
  }
 ],
 "paymentMethod": "string",
 "paymentTransactionId": "string"
}
```

#### Resposta de Sucesso (201 Created)
```json
{
 "id": "5d69d797-85a3-a54c-eda3-e2b685d70223",
 "userId": "bbd6108f-980c-f736-b067-7f8a2ee2d48d",
 "customerEmail": "string",
 "createdAt": "2019-07-28T07:48:46.735Z",
 "totalValue": 7935.64540353043,
 "status": "Refunded",
 "refundRequested": true,
 "paymentTransactionId": "string",
 "items": [
  {
   "gameId": "6f38bfe4-2abe-38bf-b229-2d48a610a229",
   "title": "string",
   "quantity": 3307,
   "unitPrice": 3769.5273928011306,
   "lineTotal": 6898.420484835854
  }
 ]
}
```

### **7.2. Listar Todos os Pedidos (Administrador)**
Lista todos os pedidos. Requer autenticação e `Administrator` role.

| Método | URL | Parâmetros de Query |
| :--- | :--- | :--- |
| `GET` | `{{baseUrl}}/api/Orders?startDate=...&endDate=...&status=string&page=1&pageSize=50` | `startDate`, `endDate`, `status`, `page`, `pageSize` |

#### Resposta de Sucesso (200 OK)
Retorna uma lista paginada de pedidos.

### **7.3. Listar Meus Pedidos**
Lista pedidos do usuário autenticado (dono).

| Método | URL | Parâmetros de Query |
| :--- | :--- | :--- |
| `GET` | `{{baseUrl}}/api/Orders/mine?status=string&page=1&pageSize=20` | `status`, `page`, `pageSize` |

#### Resposta de Sucesso (200 OK)
Retorna uma lista paginada de pedidos.

### **7.4. Obter Pedido por ID**
Obtém um pedido por identificador (Admin ou dono).

| Método | URL | Parâmetro de Path |
| :--- | :--- | :--- |
| `GET` | `{{baseUrl}}/api/Orders/:id` | `id` (GUID) |

#### Resposta de Sucesso (200 OK)
Retorna os detalhes do pedido.

### **7.5. Solicitar Estorno**
Solicita estorno do pedido (Admin ou dono).

| Método | URL | Parâmetro de Path |
| :--- | :--- | :--- |
| `POST` | `{{baseUrl}}/api/Orders/:id/refund` | `id` (GUID) |

#### Exemplo de Request Body (application/json)
```json
{
 "reason": "string"
}
```

#### Resposta de Sucesso (202 Accepted)
*Corpo da resposta vazio.*

---

## 8. Fluxo: Biblioteca (Library)

Caminho base: `{{baseUrl}}/api/Library`

### **8.1. Consultar Jogos da Biblioteca**
Consultar Jogos da Biblioteca com filtros e paginação.

| Método | URL | Parâmetros de Query |
| :--- | :--- | :--- |
| `GET` | `{{baseUrl}}/api/Library/my-games?Search=...` | `Search`, `Genre`, `Developer`, `Publisher`, `StartDate`, `EndDate`, `SortBy`, `Desc`, `Page`, `PageSize` |

#### Resposta de Sucesso (200 OK)
Retorna uma lista paginada dos jogos na biblioteca.

### **8.2. Simular Compra / Liberar Jogo**
Simula a compra/liberação de um jogo na biblioteca do usuário autenticado.

| Método | URL | Parâmetro de Path |
| :--- | :--- | :--- |
| `POST` | `{{baseUrl}}/api/Library/buy/:gameId` | `gameId` (GUID) |

#### Resposta de Sucesso (200 OK)
*Corpo da resposta vazio.*

### **8.3. Remover Jogo da Biblioteca (Estorno)**
Fluxo: "Estorno Realizado" / "Remover Jogo" da biblioteca.

| Método | URL | Parâmetro de Path |
| :--- | :--- | :--- |
| `DELETE` | `{{baseUrl}}/api/Library/remove/:gameId` | `gameId` (GUID) |

#### Resposta de Sucesso (204 No Content)
*Corpo da resposta vazio.*
