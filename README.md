# Sistema para Gerenciamento de Pedidos e Mesas

## Descrição

Este sistema foi desenvolvido para facilitar o gerenciamento de pedidos em estabelecimentos como hamburguerias e pizzarias. Ele permite o controle de mesas, clientes, produtos, categorias, métodos de pagamento, além de integrar com impressoras térmicas para impressão de pedidos. O sistema também oferece controle de acesso baseado em roles, com permissões configuráveis para diferentes tipos de usuários.

## Passos para Começar a Usar o Sistema

### 1. Configuração Inicial

Após clonar o repositório, siga os passos abaixo:

1. Execute o comando `dotnet restore` na raiz do projeto para restaurar as dependências.
2. Verifique as credenciais de conexão com o banco de dados no arquivo `appsettings.json`. Insira as informações corretas para a conexão com o seu banco.
3. Suba as migrações no banco de dados com o seguinte comando:
   ```bash
   dotnet ef database update -s System.Pdv.Web/System.Pdv.Web.csproj -p System.Pdv.Infrastructure/System.Pdv.Infrastructure.csproj
   ```
4. O sistema já estará pronto para uso.

### 2. Roles e Usuário Padrão

Ao iniciar o sistema, serão criadas duas roles por padrão: `ADMIN` e `GARCOM`.

Um usuário ADMIN será criado automaticamente com as seguintes credenciais:
- **Email**: `admin@pdv.com`
- **Senha**: `admin@123`

### 3. Permissões Padrão

As permissões pré-definidas no sistema são associadas às roles criadas:

#### Role ADMIN

A role ADMIN possui permissões completas para gerenciar todos os recursos do sistema, como:

- Adicionais
  - Criar, atualizar, excluir e visualizar.
- Categorias
  - Criar, atualizar, excluir e visualizar.
- Clientes
  - Criar, atualizar, excluir e visualizar.
- Mesas
  - Criar, atualizar, excluir e visualizar.
- Métodos de Pagamento
  - Criar, atualizar, excluir e visualizar.
- Pedidos
  - Criar, atualizar, excluir e visualizar.
- Produtos
  - Criar, atualizar, excluir e visualizar.
- Roles e Permissões
  - Gerenciar roles e suas permissões.
- Status de Pedido
  - Criar, atualizar, excluir e visualizar.
- Usuários
  - Gerenciar usuários do sistema.

#### Role GARCOM

A role GARCOM possui permissões restritas, focando em pedidos e visualização de dados:

- Adicionais, Categorias, Mesas, Métodos de Pagamento, Produtos, Status de Pedido
  - Apenas visualizar.
- Pedidos
  - Criar, atualizar, excluir e visualizar.

### 4. Gerenciamento de Permissões

O ADMIN pode atribuir ou remover permissões das roles existentes através das seguintes rotas:

- **Atribuir Permissões a uma Role** (POST): `api/PermissaoHasRole/atribuir`
  - Exemplo de payload:
    ```json
    {
      "roleIds": ["role-id"],
      "permissaoIds": ["permissao-id"]
    }
    ```

- **Remover Permissões de uma Role** (DELETE): `api/PermissaoHasRole/remover`

### 5. Configuração de Impressoras

Para gerenciar as impressoras do sistema:

- **Listar Impressoras Disponíveis** (GET): `api/Printer/disponiveis`.
- **Selecionar uma Impressora** (POST): `api/Printer/selecionar`.
- **Verificar Impressora Selecionada** (GET): `api/Printer/selecionada`.

### 6. Cadastrando Usuários e Recursos

- Após configurar o sistema, cadastre os usuários, associando-os a roles com os IDs correspondentes.
- Cadastrar itens como adicionais, categorias, mesas, métodos de pagamento e produtos (associando-os a uma categoria), além de status de pedidos e novas roles, se necessário.

### 7. Realizando Pedidos

Para criar um pedido, utilize a rota `api/Pedido`:

- **Criar Pedido** (POST): 
  - Para **pedidos internos** (mesa física), defina o campo `tipoPedido` como `0` e informe o `mesaId`.
  - Para **pedidos externos** (cliente vai buscar), defina o `tipoPedido` como `1` e não informe o `mesaId`.

Exemplo de payload para um pedido interno:
```json
{
  "nomeCliente": "string",
  "telefoneCliente": "string",
  "mesaId": "mesa-id",
  "tipoPedido": 0,
  "metodoPagamentoId": "pagamento-id",
  "statusPedidoId": "status-id",
  "itens": [
    {
      "produtoId": "produto-id",
      "quantidade": 1,
      "observacoes": "string",
      "adicionalId": ["adicional-id"]
    }
  ]
}
```

### 8. Impressão de Pedidos

- **Reimprimir Pedido** (POST): `api/Pedido/imprimir`, passando os IDs dos pedidos a serem impressos:
  ```json
  ["pedido-id"]
  ```

### 9. Atualização de Pedidos

- **Atualizar Pedido** (PATCH): Para atualizar um pedido, utilize a rota `api/Pedido` com o método `PATCH`. O JSON enviado substituirá o pedido original, mas apenas os itens enviados serão alterados ou removidos.

--- 

# Agora vamos detalhas as rotas da api

## **Autenticação**

### **1. Login**

- **Método**: `POST`
- **Rota**: `/api/Autenticacao/login`

#### Corpo da Requisição (JSON)
```json
{
  "email": "user@example.com",
  "password": "string"
}
```

#### Respostas
- **200**: OK (Autenticação bem-sucedida, retorna o token de acesso)
- **401**: Unauthorized (Credenciais inválidas)
- **500**: Internal Server Error (Erro inesperado no servidor)

---

## **Adicional**

### **1. Listar Adicionais**

- **Método**: `GET`
- **Rota**: `/api/Adicional`

#### Parâmetros (Query)
- `pageNumber`: Número da página. Valor padrão: `1`
- `pageSize`: Tamanho da página. Valor padrão: `10`

**Exemplo de URL**:
```
/api/Adicional?pageNumber=1&pageSize=10
```

#### Respostas
- **200**: OK
- **401**: Unauthorized
- **403**: Forbidden
- **404**: Not Found
- **500**: Internal Server Error

---

### **2. Criar Adicional**

- **Método**: `POST`
- **Rota**: `/api/Adicional`

#### Corpo da Requisição (JSON)
```json
{
  "nome": "string",
  "preco": 0.01
}
```

#### Respostas
- **200**: OK
- **401**: Unauthorized
- **403**: Forbidden
- **409**: Conflict
- **500**: Internal Server Error

---

### **3. Obter Adicional por ID**

- **Método**: `GET`
- **Rota**: `/api/Adicional/{id}`

#### Parâmetros (Path)
- `id` (string, UUID): ID do adicional a ser buscado.

**Exemplo de URL**:
```
/api/Adicional/3fa85f64-5717-4562-b3fc-2c963f66afa6
```

#### Respostas
- **200**: OK
- **401**: Unauthorized
- **403**: Forbidden
- **404**: Not Found
- **500**: Internal Server Error

---

### **4. Atualizar Adicional por ID**

- **Método**: `PATCH`
- **Rota**: `/api/Adicional/{id}`

#### Parâmetros (Path)
- `id` (string, UUID): ID do adicional a ser atualizado.

#### Corpo da Requisição (JSON)
```json
{
  "nome": "string",
  "preco": 0.01
}
```

#### Respostas
- **200**: OK
- **401**: Unauthorized
- **403**: Forbidden
- **404**: Not Found
- **500**: Internal Server Error

---

### **5. Deletar Adicional por ID**

- **Método**: `DELETE`
- **Rota**: `/api/Adicional/{id}`

#### Parâmetros (Path)
- `id` (string, UUID): ID do adicional a ser deletado.

**Exemplo de URL**:
```
/api/Adicional/3fa85f64-5717-4562-b3fc-2c963f66afa6
```

#### Respostas
- **200**: OK
- **401**: Unauthorized
- **403**: Forbidden
- **404**: Not Found
- **500**: Internal Server Error

---

## **Categoria**

### **1. Listar Categorias**

- **Método**: `GET`
- **Rota**: `/api/Categoria`

#### Parâmetros:
- Nenhum

#### Respostas:
- **200**: OK (Retorna uma lista de categorias)
- **401**: Unauthorized (Usuário não autenticado)
- **403**: Forbidden (Usuário não autorizado)
- **404**: Not Found (Nenhuma categoria encontrada)
- **500**: Internal Server Error (Erro no servidor)

---

### **2. Criar Categoria**

- **Método**: `POST`
- **Rota**: `/api/Categoria`

#### Corpo da Requisição (JSON):
```json
{
  "nome": "string"
}
```

#### Respostas:
- **200**: OK (Categoria criada com sucesso)
- **401**: Unauthorized (Usuário não autenticado)
- **403**: Forbidden (Usuário não autorizado)
- **409**: Conflict (Conflito, categoria já existente)
- **500**: Internal Server Error (Erro no servidor)

---

### **3. Buscar Categoria por ID**

- **Método**: `GET`
- **Rota**: `/api/Categoria/{id}`

#### Parâmetros:
- **id** (obrigatório): UUID da categoria

#### Respostas:
- **200**: OK (Retorna os detalhes da categoria)
- **401**: Unauthorized (Usuário não autenticado)
- **403**: Forbidden (Usuário não autorizado)
- **404**: Not Found (Categoria não encontrada)
- **500**: Internal Server Error (Erro no servidor)

---

### **4. Atualizar Categoria**

- **Método**: `PUT`
- **Rota**: `/api/Categoria/{id}`

#### Parâmetros:
- **id** (obrigatório): UUID da categoria

#### Corpo da Requisição (JSON):
```json
{
  "nome": "string"
}
```

#### Respostas:
- **200**: OK (Categoria atualizada com sucesso)
- **401**: Unauthorized (Usuário não autenticado)
- **403**: Forbidden (Usuário não autorizado)
- **404**: Not Found (Categoria não encontrada)
- **500**: Internal Server Error (Erro no servidor)

---

### **5. Deletar Categoria**

- **Método**: `DELETE`
- **Rota**: `/api/Categoria/{id}`

#### Parâmetros:
- **id** (obrigatório): UUID da categoria

#### Respostas:
- **200**: OK (Categoria deletada com sucesso)
- **401**: Unauthorized (Usuário não autenticado)
- **403**: Forbidden (Usuário não autorizado)
- **404**: Not Found (Categoria não encontrada)
- **500**: Internal Server Error (Erro no servidor)

---

## **Cliente**

### **1. Listar Clientes (Paginação)**

- **Método**: `GET`
- **Rota**: `/api/Cliente`

#### Parâmetros (Query):
- **pageNumber** (opcional): Número da página (padrão: 1)
- **pageSize** (opcional): Quantidade de itens por página (padrão: 10)

#### Respostas:
- **200**: OK (Retorna uma lista de clientes com paginação)
- **401**: Unauthorized (Usuário não autenticado)
- **403**: Forbidden (Usuário não autorizado)
- **404**: Not Found (Nenhum cliente encontrado)
- **500**: Internal Server Error (Erro no servidor)

---

### **2. Buscar Cliente por Nome**

- **Método**: `GET`
- **Rota**: `/api/Cliente/nome`

#### Parâmetros (Query):
- **nome** (obrigatório): Nome do cliente

#### Respostas:
- **200**: OK (Retorna os detalhes do cliente com o nome fornecido)
- **401**: Unauthorized (Usuário não autenticado)
- **403**: Forbidden (Usuário não autorizado)
- **404**: Not Found (Cliente não encontrado)
- **500**: Internal Server Error (Erro no servidor)

---

### **3. Buscar Cliente por ID**

- **Método**: `GET`
- **Rota**: `/api/Cliente/{id}`

#### Parâmetros:
- **id** (obrigatório): UUID do cliente

#### Respostas:
- **200**: OK (Retorna os detalhes do cliente com o ID fornecido)
- **401**: Unauthorized (Usuário não autenticado)
- **403**: Forbidden (Usuário não autorizado)
- **404**: Not Found (Cliente não encontrado)
- **500**: Internal Server Error (Erro no servidor)

---

## **Mesa**

### **1. Listar Mesas**

- **Método**: `GET`
- **Rota**: `/api/Mesa`

#### Parâmetros:
- Nenhum parâmetro

#### Respostas:
- **200**: OK (Retorna a lista de mesas)
- **401**: Unauthorized (Usuário não autenticado)
- **403**: Forbidden (Usuário não autorizado)
- **404**: Not Found (Nenhuma mesa encontrada)
- **500**: Internal Server Error (Erro no servidor)

---

### **2. Criar Nova Mesa**

- **Método**: `POST`
- **Rota**: `/api/Mesa`

#### Corpo da Requisição (JSON):
```json
{
  "numero": 2147483647,
  "status": 0
}
```
- **numero**: Número da mesa (inteiro)
- **status**: Status da mesa (0 - Livre, 1 - Ocupada, etc.)

#### Respostas:
- **200**: OK (Mesa criada com sucesso)
- **401**: Unauthorized (Usuário não autenticado)
- **403**: Forbidden (Usuário não autorizado)
- **409**: Conflict (Conflito ao tentar criar a mesa)
- **500**: Internal Server Error (Erro no servidor)

---

### **3. Buscar Mesa por ID**

- **Método**: `GET`
- **Rota**: `/api/Mesa/{id}`

#### Parâmetros:
- **id** (obrigatório): UUID da mesa

#### Respostas:
- **200**: OK (Retorna os detalhes da mesa com o ID fornecido)
- **401**: Unauthorized (Usuário não autenticado)
- **403**: Forbidden (Usuário não autorizado)
- **404**: Not Found (Mesa não encontrada)
- **500**: Internal Server Error (Erro no servidor)

---

### **4. Atualizar Mesa (Parcial)**

- **Método**: `PATCH`
- **Rota**: `/api/Mesa/{id}`

#### Parâmetros:
- **id** (obrigatório): UUID da mesa

#### Corpo da Requisição (JSON):
```json
{
  "numero": 2147483647,
  "status": 0
}
```
- **numero**: Número da mesa (inteiro)
- **status**: Status da mesa (0 - Livre, 1 - Ocupada, etc.)

#### Respostas:
- **200**: OK (Mesa atualizada com sucesso)
- **401**: Unauthorized (Usuário não autenticado)
- **403**: Forbidden (Usuário não autorizado)
- **404**: Not Found (Mesa não encontrada)
- **500**: Internal Server Error (Erro no servidor)

---

### **5. Excluir Mesa**

- **Método**: `DELETE`
- **Rota**: `/api/Mesa/{id}`

#### Parâmetros:
- **id** (obrigatório): UUID da mesa

#### Respostas:
- **200**: OK (Mesa excluída com sucesso)
- **401**: Unauthorized (Usuário não autenticado)
- **403**: Forbidden (Usuário não autorizado)
- **404**: Not Found (Mesa não encontrada)
- **500**: Internal Server Error (Erro no servidor)

---

## **Método de Pagamento**

### **1. Listar Métodos de Pagamento**

- **Método**: `GET`
- **Rota**: `/api/MetodoPagamento`

#### Parâmetros:
- Nenhum parâmetro

#### Respostas:
- **200**: OK (Retorna a lista de métodos de pagamento)
- **401**: Unauthorized (Usuário não autenticado)
- **403**: Forbidden (Usuário não autorizado)
- **404**: Not Found (Nenhum método de pagamento encontrado)
- **500**: Internal Server Error (Erro no servidor)

---

### **2. Criar Novo Método de Pagamento**

- **Método**: `POST`
- **Rota**: `/api/MetodoPagamento`

#### Corpo da Requisição (JSON):
```json
{
  "nome": "string"
}
```
- **nome**: Nome do método de pagamento (exemplo: "Cartão de Crédito")

#### Respostas:
- **200**: OK (Método de pagamento criado com sucesso)
- **401**: Unauthorized (Usuário não autenticado)
- **403**: Forbidden (Usuário não autorizado)
- **409**: Conflict (Conflito ao tentar criar o método de pagamento)
- **500**: Internal Server Error (Erro no servidor)

---

### **3. Buscar Método de Pagamento por ID**

- **Método**: `GET`
- **Rota**: `/api/MetodoPagamento/{id}`

#### Parâmetros:
- **id** (obrigatório): UUID do método de pagamento

#### Respostas:
- **200**: OK (Retorna os detalhes do método de pagamento com o ID fornecido)
- **401**: Unauthorized (Usuário não autenticado)
- **403**: Forbidden (Usuário não autorizado)
- **404**: Not Found (Método de pagamento não encontrado)
- **500**: Internal Server Error (Erro no servidor)

---

### **4. Atualizar Método de Pagamento (Parcial)**

- **Método**: `PATCH`
- **Rota**: `/api/MetodoPagamento/{id}`

#### Parâmetros:
- **id** (obrigatório): UUID do método de pagamento

#### Corpo da Requisição (JSON):
```json
{
  "nome": "string"
}
```
- **nome**: Nome do método de pagamento

#### Respostas:
- **200**: OK (Método de pagamento atualizado com sucesso)
- **401**: Unauthorized (Usuário não autenticado)
- **403**: Forbidden (Usuário não autorizado)
- **404**: Not Found (Método de pagamento não encontrado)
- **409**: Conflict (Conflito ao tentar atualizar o método de pagamento)
- **500**: Internal Server Error (Erro no servidor)

---

### **5. Excluir Método de Pagamento**

- **Método**: `DELETE`
- **Rota**: `/api/MetodoPagamento/{id}`

#### Parâmetros:
- **id** (obrigatório): UUID do método de pagamento

#### Respostas:
- **200**: OK (Método de pagamento excluído com sucesso)
- **401**: Unauthorized (Usuário não autenticado)
- **403**: Forbidden (Usuário não autorizado)
- **404**: Not Found (Método de pagamento não encontrado)
- **500**: Internal Server Error (Erro no servidor)

---

### **Pedido**

#### **1. Listar Pedidos**
- **Método**: `GET`
- **Rota**: `/api/Pedido`

##### Parâmetros de Consulta:
- **pageNumber** (integer, padrão: 1): Número da página a ser retornada.
- **pageSize** (integer, padrão: 10): Número de itens por página.
- **tipoPedido** (string): Filtrar por tipo de pedido.
- **statusPedido** (string): Filtrar por status de pedido.

##### Respostas:
- **200**: OK
- **401**: Unauthorized
- **403**: Forbidden
- **404**: Not Found
- **500**: Internal Server Error

---

#### **2. Criar Novo Pedido**
- **Método**: `POST`
- **Rota**: `/api/Pedido`

##### Observações:
- Para pedidos internos (mesa física), defina o campo `tipoPedido` como **0** e informe o `mesaId`.
- Para pedidos externos (cliente vai buscar), defina o campo `tipoPedido` como **1** e não informe o `mesaId`.

##### Corpo da Requisição (JSON):
```json
{
  "nomeCliente": "string",
  "telefoneCliente": "string",
  "mesaId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", // Apenas para pedidos internos
  "tipoPedido": 0, // 0 para interno, 1 para externo
  "metodoPagamentoId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "statusPedidoId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "itens": [
    {
      "produtoId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "quantidade": 0,
      "observacoes": "string",
      "adicionalId": [
        "3fa85f64-5717-4562-b3fc-2c963f66afa6"
      ]
    }
  ]
}
```

##### Respostas:
- **200**: OK (Pedido criado com sucesso)
- **400**: Bad Request (Dados inválidos)
- **401**: Unauthorized (Usuário não autenticado)
- **403**: Forbidden (Usuário não autorizado)
- **409**: Conflict (Conflito ao criar o pedido)
- **500**: Internal Server Error (Erro no servidor)

---

#### **3. Obter Pedido por Número da Mesa**
- **Método**: `GET`
- **Rota**: `/api/Pedido/{numeroMesa}`

##### Parâmetros de Caminho:
- **numeroMesa** (integer): Número da mesa.

##### Parâmetros de Consulta:
- **statusPedido** (string): Filtrar por status de pedido.

##### Respostas:
- **200**: OK
- **401**: Unauthorized
- **403**: Forbidden
- **404**: Not Found
- **500**: Internal Server Error

---

#### **4. Obter Pedido por ID**
- **Método**: `GET`
- **Rota**: `/api/Pedido/{id}`

##### Parâmetros de Caminho:
- **id** (string, uuid): ID do pedido.

##### Respostas:
- **200**: OK
- **401**: Unauthorized
- **403**: Forbidden
- **404**: Not Found
- **500**: Internal Server Error

---

#### **5. Atualizar Pedido**
- **Método**: `PATCH`
- **Rota**: `/api/Pedido/{id}`

##### Parâmetros de Caminho:
- **id** (string, uuid): ID do pedido.

##### Corpo da Requisição (JSON):
```json
{
  "nomeCliente": "string",
  "telefoneCliente": "string",
  "mesaId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", // Apenas para pedidos internos
  "tipoPedido": 0, // 0 para interno, 1 para externo
  "metodoPagamentoId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "statusPedidoId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "itens": [
    {
      "produtoId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "quantidade": 0,
      "observacoes": "string",
      "adicionalId": [
        "3fa85f64-5717-4562-b3fc-2c963f66afa6"
      ]
    }
  ]
}
```

##### Respostas:
- **200**: OK
- **401**: Unauthorized
- **403**: Forbidden
- **404**: Not Found
- **500**: Internal Server Error

---

### **Permissão**

#### **1. Listar Permissões**
- **Método**: `GET`
- **Rota**: `/api/Permissao`

##### Parâmetros de Consulta:
- **pageNumber** (integer, padrão: 1): Número da página a ser retornada.
- **pageSize** (integer, padrão: 10): Número de itens por página.
- **recurso** (string): Filtrar por recurso.
- **acao** (string): Filtrar por ação.

##### Respostas:
- **200**: OK
- **401**: Unauthorized
- **403**: Forbidden
- **404**: Not Found
- **500**: Internal Server Error

---

#### **2. Listar Permissões com Roles**
- **Método**: `GET`
- **Rota**: `/api/Permissao/com/roles`

##### Parâmetros de Consulta:
- **pageNumber** (integer, padrão: 1): Número da página a ser retornada.
- **pageSize** (integer, padrão: 10): Número de itens por página.
- **recurso** (string): Filtrar por recurso.
- **acao** (string): Filtrar por ação.

##### Respostas:
- **200**: OK
- **401**: Unauthorized
- **403**: Forbidden
- **404**: Not Found
- **500**: Internal Server Error

---

#### **3. Obter Permissões por Role**
- **Método**: `GET`
- **Rota**: `/api/Permissao/role/{roleId}`

##### Parâmetros de Caminho:
- **roleId** (string, uuid): ID da role.

##### Respostas:
- **200**: OK
- **401**: Unauthorized
- **403**: Forbidden
- **404**: Not Found
- **500**: Internal Server Error

---

### **Vínculo entre Role e Permissão**

#### **1. Atribuir Permissões a Roles**
- **Método**: `POST`
- **Rota**: `/api/PermissaoHasRole/atribuir`

##### Parâmetros:
- **Nenhum**

##### Corpo da Requisição:
```json
{
  "roleIds": [
    "3fa85f64-5717-4562-b3fc-2c963f66afa6"
  ],
  "permissaoIds": [
    "3fa85f64-5717-4562-b3fc-2c963f66afa6"
  ]
}
```

##### Respostas:
- **200**: OK
- **401**: Unauthorized
- **403**: Forbidden
- **404**: Not Found
- **500**: Internal Server Error

---

#### **2. Remover Permissões de Roles**
- **Método**: `DELETE`
- **Rota**: `/api/PermissaoHasRole/remover`

##### Parâmetros:
- **Nenhum**

##### Corpo da Requisição:
```json
{
  "roleIds": [
    "3fa85f64-5717-4562-b3fc-2c963f66afa6"
  ],
  "permissaoIds": [
    "3fa85f64-5717-4562-b3fc-2c963f66afa6"
  ]
}
```

##### Respostas:
- **200**: OK
- **401**: Unauthorized
- **403**: Forbidden
- **404**: Not Found
- **500**: Internal Server Error

---

### **Gerenciamento de Impressoras**

#### **1. Listar Impressoras Disponíveis**
- **Método**: `GET`
- **Rota**: `/api/Printer/disponiveis`

##### Parâmetros:
- **Nenhum**

##### Respostas:
- **200**: OK
- **401**: Unauthorized
- **403**: Forbidden

---

#### **2. Selecionar Impressora**
- **Método**: `POST`
- **Rota**: `/api/Printer/selecionar`

##### Parâmetros:
- **Nenhum**

##### Corpo da Requisição:
```json
"string"
```
*(onde "string" representa o nome ou ID da impressora selecionada)*

##### Respostas:
- **200**: OK
- **401**: Unauthorized
- **403**: Forbidden

---

#### **3. Obter Impressora Selecionada**
- **Método**: `GET`
- **Rota**: `/api/Printer/selecionada`

##### Parâmetros:
- **Nenhum**

##### Respostas:
- **200**: OK
- **401**: Unauthorized
- **403**: Forbidden

---

### **Gerenciamento de Produtos**

#### **1. Listar Produtos**
- **Método**: `GET`
- **Rota**: `/api/Produto`

##### Parâmetros:
- **pageNumber**: `integer($int32)` (query, padrão: 1)
- **pageSize**: `integer($int32)` (query, padrão: 10)

##### Respostas:
- **200**: OK
- **401**: Unauthorized
- **403**: Forbidden
- **404**: Not Found
- **500**: Internal Server Error

---

#### **2. Criar Produto**
- **Método**: `POST`
- **Rota**: `/api/Produto`

##### Parâmetros:
- **Nenhum**

##### Corpo da Requisição:
```json
{
  "nome": "string",
  "descricao": "string",
  "preco": 0.01,
  "disponivel": true,
  "categoriaId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

##### Respostas:
- **200**: OK
- **401**: Unauthorized
- **403**: Forbidden
- **404**: Not Found
- **500**: Internal Server Error

---

#### **3. Obter Produto por ID**
- **Método**: `GET`
- **Rota**: `/api/Produto/{id}`

##### Parâmetros:
- **id**: `string($uuid)` (path)

##### Respostas:
- **200**: OK
- **401**: Unauthorized
- **403**: Forbidden
- **404**: Not Found
- **500**: Internal Server Error

---

#### **4. Atualizar Produto**
- **Método**: `PATCH`
- **Rota**: `/api/Produto/{id}`

##### Parâmetros:
- **id**: `string($uuid)` (path)

##### Corpo da Requisição:
```json
{
  "nome": "string",
  "descricao": "string",
  "preco": 0.01,
  "disponivel": true,
  "categoriaId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

##### Respostas:
- **200**: OK
- **401**: Unauthorized
- **403**: Forbidden
- **404**: Not Found
- **500**: Internal Server Error

---

#### **5. Deletar Produto**
- **Método**: `DELETE`
- **Rota**: `/api/Produto/{id}`

##### Parâmetros:
- **id**: `string($uuid)` (path)

##### Respostas:
- **200**: OK
- **401**: Unauthorized
- **403**: Forbidden
- **404**: Not Found
- **500**: Internal Server Error

---

#### **6. Listar Produtos por Categoria**
- **Método**: `GET`
- **Rota**: `/api/Produto/categoria/{categoriaId}`

##### Parâmetros:
- **categoriaId**: `string($uuid)` (path)
- **pageNumber**: `integer($int32)` (query, padrão: 1)
- **pageSize**: `integer($int32)` (query, padrão: 10)

##### Respostas:
- **200**: OK
- **401**: Unauthorized
- **403**: Forbidden
- **404**: Not Found
- **500**: Internal Server Error

---

### **Gerenciamento de Roles**

#### **1. Listar Roles**
- **Método**: `GET`
- **Rota**: `/api/Role`

##### Parâmetros:
- **Nenhum**

##### Respostas:
- **200**: OK
- **401**: Unauthorized
- **403**: Forbidden
- **404**: Not Found
- **500**: Internal Server Error

---

#### **2. Criar Role**
- **Método**: `POST`
- **Rota**: `/api/Role`

##### Parâmetros:
- **Nenhum**

##### Corpo da Requisição:
```json
{
  "nome": "string",
  "descricao": "string"
}
```

##### Respostas:
- **200**: OK
- **401**: Unauthorized
- **403**: Forbidden
- **409**: Conflict
- **500**: Internal Server Error

---

#### **3. Obter Role por ID**
- **Método**: `GET`
- **Rota**: `/api/Role/{id}`

##### Parâmetros:
- **id**: `string($uuid)` (path)

##### Respostas:
- **200**: OK
- **401**: Unauthorized
- **403**: Forbidden
- **404**: Not Found
- **500**: Internal Server Error

---

#### **4. Atualizar Role**
- **Método**: `PATCH`
- **Rota**: `/api/Role/{id}`

##### Parâmetros:
- **id**: `string($uuid)` (path)

##### Corpo da Requisição:
```json
{
  "nome": "string",
  "descricao": "string"
}
```

##### Respostas:
- **200**: OK
- **400**: Bad Request
- **401**: Unauthorized
- **403**: Forbidden
- **404**: Not Found
- **500**: Internal Server Error

---

#### **5. Deletar Role**
- **Método**: `DELETE`
- **Rota**: `/api/Role/{id}`

##### Parâmetros:
- **id**: `string($uuid)` (path)

##### Respostas:
- **200**: OK
- **400**: Bad Request
- **401**: Unauthorized
- **403**: Forbidden
- **404**: Not Found
- **500**: Internal Server Error

---

### **Gerenciamento de Status dos Pedidos**

#### **1. Listar Status dos Pedidos**
- **Método**: `GET`
- **Rota**: `/api/StatusPedido`

##### Parâmetros:
- **Nenhum**

##### Respostas:
- **200**: OK
- **401**: Unauthorized
- **403**: Forbidden
- **404**: Not Found
- **500**: Internal Server Error

---

#### **2. Criar Status do Pedido**
- **Método**: `POST`
- **Rota**: `/api/StatusPedido`

##### Parâmetros:
- **Nenhum**

##### Corpo da Requisição:
```json
{
  "status": "string"
}
```

##### Respostas:
- **200**: OK
- **401**: Unauthorized
- **403**: Forbidden
- **409**: Conflict
- **500**: Internal Server Error

---

#### **3. Obter Status do Pedido por ID**
- **Método**: `GET`
- **Rota**: `/api/StatusPedido/{id}`

##### Parâmetros:
- **id**: `string($uuid)` (path)

##### Respostas:
- **200**: OK
- **401**: Unauthorized
- **403**: Forbidden
- **404**: Not Found
- **500**: Internal Server Error

---

#### **4. Atualizar Status do Pedido**
- **Método**: `PUT`
- **Rota**: `/api/StatusPedido/{id}`

##### Parâmetros:
- **id**: `string($uuid)` (path)

##### Corpo da Requisição:
```json
{
  "status": "string"
}
```

##### Respostas:
- **200**: OK
- **401**: Unauthorized
- **403**: Forbidden
- **404**: Not Found
- **409**: Conflict
- **500**: Internal Server Error

---

#### **5. Deletar Status do Pedido**
- **Método**: `DELETE`
- **Rota**: `/api/StatusPedido/{id}`

##### Parâmetros:
- **id**: `string($uuid)` (path)

##### Respostas:
- **200**: OK
- **401**: Unauthorized
- **403**: Forbidden
- **404**: Not Found
- **500**: Internal Server Error

---

### **Gerenciamento de Usuários**

#### **1. Listar Usuários**
- **Método**: `GET`
- **Rota**: `/api/Usuario`

##### Parâmetros:
- **pageNumber**: `integer($int32)` (query) - Valor padrão: `1`
- **pageSize**: `integer($int32)` (query) - Valor padrão: `10`

##### Respostas:
- **200**: OK
- **401**: Unauthorized
- **403**: Forbidden
- **404**: Not Found
- **500**: Internal Server Error

---

#### **2. Criar Usuário**
- **Método**: `POST`
- **Rota**: `/api/Usuario`

##### Parâmetros:
- **Nenhum**

##### Corpo da Requisição:
```json
{
  "nome": "string",
  "email": "user@example.com",
  "password": "stringst",
  "roleId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

##### Respostas:
- **200**: OK
- **401**: Unauthorized
- **403**: Forbidden
- **404**: Not Found
- **409**: Conflict
- **500**: Internal Server Error

---

#### **3. Obter Usuário por ID**
- **Método**: `GET`
- **Rota**: `/api/Usuario/{id}`

##### Parâmetros:
- **id**: `string($uuid)` (path)

##### Respostas:
- **200**: OK
- **401**: Unauthorized
- **403**: Forbidden
- **404**: Not Found
- **500**: Internal Server Error

---

#### **4. Atualizar Usuário**
- **Método**: `PATCH`
- **Rota**: `/api/Usuario/{id}`

##### Parâmetros:
- **id**: `string($uuid)` (path)

##### Corpo da Requisição:
```json
{
  "nome": "string",
  "email": "user@example.com",
  "password": "stringst",
  "roleId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

##### Respostas:
- **200**: OK
- **401**: Unauthorized
- **403**: Forbidden
- **404**: Not Found
- **409**: Conflict
- **500**: Internal Server Error

---

#### **5. Deletar Usuário**
- **Método**: `DELETE`
- **Rota**: `/api/Usuario/{id}`

##### Parâmetros:
- **id**: `string($uuid)` (path)

##### Respostas:
- **200**: OK
- **401**: Unauthorized
- **403**: Forbidden
- **404**: Not Found
- **500**: Internal Server Error

---
