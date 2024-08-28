### **Rotas de Pedidos**

1. **POST `/api/pedidos`**
   - **Descrição**: Cria um novo pedido. Se o pedido for do tipo "externo" (delivery), um novo cliente é criado e o ID do cliente é associado ao pedido. Se o pedido for do tipo "interno" (consumo no local), o ID da mesa é associado ao pedido.
   - **Corpo da Requisição**:
     ```json
     {
         "tipo_pedido": "string",  // "interno" ou "externo"
         "mesa_id": "int",         // Obrigatório se tipo_pedido = "interno"
         "cliente": {              // Obrigatório se tipo_pedido = "externo"
             "nome": "string",
             "telefone": "string"
         },
         "garcom_id": "int",
         "metodo_pagamento_id": "int",
         "status_pedido_id": "int",
         "data_pedido": "DateTime",
         "itens": [
             {
                 "produto_id": "int",
                 "quantidade": "int",
                 "observacoes": "string",
                 "adicionais": [
                     {"adicional_id": "int"}
                 ]
             }
         ]
     }
     ```
   - **Resposta**:
     - **201 Created**: Retorna os detalhes do pedido criado, incluindo informações sobre o cliente ou mesa, garçom, método de pagamento, status do pedido e itens.

2. **GET `/api/pedidos/{id}`**
   - **Descrição**: Retorna os detalhes de um pedido específico.
   - **Resposta**: 
     - **200 OK**: Retorna os detalhes do pedido, incluindo informações sobre o cliente ou mesa, garçom, método de pagamento, status do pedido e itens.
     - **404 Not Found**: Caso o pedido não seja encontrado.

3. **GET `/api/pedidos`**
   - **Descrição**: Retorna a lista de todos os pedidos.
   - **Resposta**:
     - **200 OK**: Lista de pedidos, incluindo detalhes como ID, cliente/mesa, garçom, status, data e itens.

### **Rotas de Mesas**

1. **GET `/api/mesas`**
   - **Descrição**: Retorna a lista de mesas disponíveis.
   - **Resposta**:
     - **200 OK**: Lista de mesas, incluindo ID, número e status.

2. **GET `/api/mesas/{id}`**
   - **Descrição**: Retorna os detalhes de uma mesa específica.
   - **Resposta**:
     - **200 OK**: Detalhes da mesa, incluindo ID, número e status.
     - **404 Not Found**: Caso a mesa não seja encontrada.

3. **POST `/api/mesas`**
   - **Descrição**: Cria uma nova mesa.
   - **Corpo da Requisição**:
     ```json
     {
         "numero": "int",
         "status": "string"  // Ex: "disponível", "ocupada", etc.
     }
     ```
   - **Resposta**:
     - **201 Created**: Retorna os detalhes da mesa criada.

4. **PUT `/api/mesas/{id}`**
   - **Descrição**: Atualiza os detalhes de uma mesa específica.
   - **Corpo da Requisição**:
     ```json
     {
         "numero": "int",
         "status": "string"
     }
     ```
   - **Resposta**:
     - **200 OK**: Retorna os detalhes da mesa atualizada.
     - **404 Not Found**: Caso a mesa não seja encontrada.

5. **DELETE `/api/mesas/{id}`**
   - **Descrição**: Exclui uma mesa específica.
   - **Resposta**:
     - **204 No Content**: A mesa foi excluída com sucesso.
     - **404 Not Found**: Caso a mesa não seja encontrada.

### **Rotas de Clientes**

1. **GET `/api/clientes`**
   - **Descrição**: Retorna a lista de clientes cadastrados.
   - **Resposta**:
     - **200 OK**: Lista de clientes, incluindo ID, nome e telefone.

2. **GET `/api/clientes/{id}`**
   - **Descrição**: Retorna os detalhes de um cliente específico.
   - **Resposta**:
     - **200 OK**: Detalhes do cliente, incluindo ID, nome e telefone.
     - **404 Not Found**: Caso o cliente não seja encontrado.

3. **POST `/api/clientes`**
   - **Descrição**: Cria um novo cliente.
   - **Corpo da Requisição**:
     ```json
     {
         "nome": "string",
         "telefone": "string"
     }
     ```
   - **Resposta**:
     - **201 Created**: Retorna os detalhes do cliente criado.

4. **PUT `/api/clientes/{id}`**
   - **Descrição**: Atualiza os detalhes de um cliente específico.
   - **Corpo da Requisição**:
     ```json
     {
         "nome": "string",
         "telefone": "string"
     }
     ```
   - **Resposta**:
     - **200 OK**: Retorna os detalhes do cliente atualizado.
     - **404 Not Found**: Caso o cliente não seja encontrado.

5. **DELETE `/api/clientes/{id}`**
   - **Descrição**: Exclui um cliente específico.
   - **Resposta**:
     - **204 No Content**: O cliente foi excluído com sucesso.
     - **404 Not Found**: Caso o cliente não seja encontrado.

### **Rotas de Garçons**

1. **GET `/api/garcons`**
   - **Descrição**: Retorna a lista de garçons cadastrados.
   - **Resposta**:
     - **200 OK**: Lista de garçons, incluindo ID, nome e email.

2. **GET `/api/garcons/{id}`**
   - **Descrição**: Retorna os detalhes de um garçom específico.
   - **Resposta**:
     - **200 OK**: Detalhes do garçom, incluindo ID, nome e email.
     - **404 Not Found**: Caso o garçom não seja encontrado.

3. **POST `/api/garcons`**
   - **Descrição**: Cria um novo garçom.
   - **Corpo da Requisição**:
     ```json
     {
         "nome": "string",
         "email": "string",
         "password": "string"
     }
     ```
   - **Resposta**:
     - **201 Created**: Retorna os detalhes do garçom criado.

4. **PUT `/api/garcons/{id}`**
   - **Descrição**: Atualiza os detalhes de um garçom específico.
   - **Corpo da Requisição**:
     ```json
     {
         "nome": "string",
         "email": "string",
         "password": "string"
     }
     ```
   - **Resposta**:
     - **200 OK**: Retorna os detalhes do garçom atualizado.
     - **404 Not Found**: Caso o garçom não seja encontrado.

5. **DELETE `/api/garcons/{id}`**
   - **Descrição**: Exclui um garçom específico.
   - **Resposta**:
     - **204 No Content**: O garçom foi excluído com sucesso.
     - **404 Not Found**: Caso o garçom não seja encontrado.

### **Rotas de Produtos**

1. **GET `/api/produtos`**
   - **Descrição**: Retorna a lista de produtos disponíveis.
   - **Resposta**:
     - **200 OK**: Lista de produtos, incluindo ID, nome, descrição, preço, disponibilidade e categoria.

2. **GET `/api/produtos/{id}`**
   - **Descrição**: Retorna os detalhes de um produto específico.
   - **Resposta**:
     - **200 OK**: Detalhes do produto, incluindo ID, nome, descrição, preço, disponibilidade e categoria.
     - **404 Not Found**: Caso o produto não seja encontrado.

3. **POST `/api/produtos`**
   - **Descrição**: Cria um novo produto.
   - **Corpo da Requisição**:
     ```json
     {
         "nome": "string",
         "descricao": "string",
         "preco": "float",
         "disponivel": "boolean",
         "categoria_id": "int"
     }
     ```
   - **Resposta**:
     - **201 Created**: Retorna os detalhes do produto criado.

4. **PUT `/api/produtos/{id}`**
   - **Descrição**: Atualiza os detalhes de um produto específico.
   - **Corpo da Requisição**:
     ```json
     {
         "nome": "string",
         "descricao": "string",
         "preco": "float",
         "disponivel": "boolean",
         "categoria_id": "int"
     }
     ```
   - **Resposta**:
     - **200 OK**: Retorna os detalhes do produto atualizado.
     - **404 Not Found**: Caso o produto não seja encontrado.

5. **DELETE `/api/produtos/{id}`**
   - **Descrição**: Exclui um produto específico.
   - **Resposta**:
     - **204 No Content**

: O produto foi excluído com sucesso.
     - **404 Not Found**: Caso o produto não seja encontrado.

### **Rotas de Categorias**

1. **GET `/api/categorias`**
   - **Descrição**: Retorna a lista de categorias disponíveis.
   - **Resposta**:
     - **200 OK**: Lista de categorias, incluindo ID e nome.

2. **GET `/api/categorias/{id}`**
   - **Descrição**: Retorna os detalhes de uma categoria específica.
   - **Resposta**:
     - **200 OK**: Detalhes da categoria, incluindo ID e nome.
     - **404 Not Found**: Caso a categoria não seja encontrada.

3. **POST `/api/categorias`**
   - **Descrição**: Cria uma nova categoria.
   - **Corpo da Requisição**:
     ```json
     {
         "nome": "string"
     }
     ```
   - **Resposta**:
     - **201 Created**: Retorna os detalhes da categoria criada.

4. **PUT `/api/categorias/{id}`**
   - **Descrição**: Atualiza os detalhes de uma categoria específica.
   - **Corpo da Requisição**:
     ```json
     {
         "nome": "string"
     }
     ```
   - **Resposta**:
     - **200 OK**: Retorna os detalhes da categoria atualizada.
     - **404 Not Found**: Caso a categoria não seja encontrada.

5. **DELETE `/api/categorias/{id}`**
   - **Descrição**: Exclui uma categoria específica.
   - **Resposta**:
     - **204 No Content**: A categoria foi excluída com sucesso.
     - **404 Not Found**: Caso a categoria não seja encontrada.

### **Rotas de Métodos de Pagamento**

1. **GET `/api/metodos-pagamento`**
   - **Descrição**: Retorna a lista de métodos de pagamento disponíveis.
   - **Resposta**:
     - **200 OK**: Lista de métodos de pagamento, incluindo ID e nome.

2. **GET `/api/metodos-pagamento/{id}`**
   - **Descrição**: Retorna os detalhes de um método de pagamento específico.
   - **Resposta**:
     - **200 OK**: Detalhes do método de pagamento, incluindo ID e nome.
     - **404 Not Found**: Caso o método de pagamento não seja encontrado.

3. **POST `/api/metodos-pagamento`**
   - **Descrição**: Cria um novo método de pagamento.
   - **Corpo da Requisição**:
     ```json
     {
         "nome": "string"
     }
     ```
   - **Resposta**:
     - **201 Created**: Retorna os detalhes do método de pagamento criado.

4. **PUT `/api/metodos-pagamento/{id}`**
   - **Descrição**: Atualiza os detalhes de um método de pagamento específico.
   - **Corpo da Requisição**:
     ```json
     {
         "nome": "string"
     }
     ```
   - **Resposta**:
     - **200 OK**: Retorna os detalhes do método de pagamento atualizado.
     - **404 Not Found**: Caso o método de pagamento não seja encontrado.

5. **DELETE `/api/metodos-pagamento/{id}`**
   - **Descrição**: Exclui um método de pagamento específico.
   - **Resposta**:
     - **204 No Content**: O método de pagamento foi excluído com sucesso.
     - **404 Not Found**: Caso o método de pagamento não seja encontrado.

### **Rotas de Status de Pedido**

1. **GET `/api/status-pedido`**
   - **Descrição**: Retorna a lista de status possíveis para um pedido.
   - **Resposta**:
     - **200 OK**: Lista de status de pedidos, incluindo ID e nome.

2. **GET `/api/status-pedido/{id}`**
   - **Descrição**: Retorna os detalhes de um status de pedido específico.
   - **Resposta**:
     - **200 OK**: Detalhes do status de pedido, incluindo ID e nome.
     - **404 Not Found**: Caso o status de pedido não seja encontrado.

3. **POST `/api/status-pedido`**
   - **Descrição**: Cria um novo status de pedido.
   - **Corpo da Requisição**:
     ```json
     {
         "nome": "string"
     }
     ```
   - **Resposta**:
     - **201 Created**: Retorna os detalhes do status de pedido criado.

4. **PUT `/api/status-pedido/{id}`**
   - **Descrição**: Atualiza os detalhes de um status de pedido específico.
   - **Corpo da Requisição**:
     ```json
     {
         "nome": "string"
     }
     ```
   - **Resposta**:
     - **200 OK**: Retorna os detalhes do status de pedido atualizado.
     - **404 Not Found**: Caso o status de pedido não seja encontrado.

5. **DELETE `/api/status-pedido/{id}`**
   - **Descrição**: Exclui um status de pedido específico.
   - **Resposta**:
     - **204 No Content**: O status de pedido foi excluído com sucesso.
     - **404 Not Found**: Caso o status de pedido não seja encontrado.

### **Rotas de Adicionais**

1. **GET `/api/adicionais`**
   - **Descrição**: Retorna a lista de adicionais disponíveis.
   - **Resposta**:
     - **200 OK**: Lista de adicionais, incluindo ID, nome e preço.

2. **GET `/api/adicionais/{id}`**
   - **Descrição**: Retorna os detalhes de um adicional específico.
   - **Resposta**:
     - **200 OK**: Detalhes do adicional, incluindo ID, nome e preço.
     - **404 Not Found**: Caso o adicional não seja encontrado.

3. **POST `/api/adicionais`**
   - **Descrição**: Cria um novo adicional.
   - **Corpo da Requisição**:
     ```json
     {
         "nome": "string",
         "preco": "float"
     }
     ```
   - **Resposta**:
     - **201 Created**: Retorna os detalhes do adicional criado.

4. **PUT `/api/adicionais/{id}`**
   - **Descrição**: Atualiza os detalhes de um adicional específico.
   - **Corpo da Requisição**:
     ```json
     {
         "nome": "string",
         "preco": "float"
     }
     ```
   - **Resposta**:
     - **200 OK**: Retorna os detalhes do adicional atualizado.
     - **404 Not Found**: Caso o adicional não seja encontrado.

5. **DELETE `/api/adicionais/{id}`**
   - **Descrição**: Exclui um adicional específico.
   - **Resposta**:
     - **204 No Content**: O adicional foi excluído com sucesso.
     - **404 Not Found**: Caso o adicional não seja encontrado.
