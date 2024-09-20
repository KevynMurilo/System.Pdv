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
4. Para rodar o backend é só colocar esse comando:
   ```bash
   dotnet run -p System.Pdv.Web/System.Pdv.Web.csproj
   ```
   
5. O sistema já estará pronto para uso.

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
- Cadastrar itens como adicionais, categorias, mesas (Livre = 0, Ocupada = 1, Reservada = 2), métodos de pagamento e produtos (associando-os a uma categoria), além de status de pedidos e novas roles, se necessário.

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


## **Documentação Completa da API**

Para obter mais detalhes sobre todos os endpoints disponíveis e testar as funcionalidades da API, acesse a documentação interativa via Swagger. Nele, você pode explorar as rotas, ver exemplos de requisições e respostas, além de testar os endpoints diretamente:

- **HTTPS**:  
  [localhost:7166/swagger/index.html](https://localhost:7166/swagger/index.html)

- **HTTP**:  
  [localhost:5119/swagger/index.html](http://localhost:5119/swagger/index.html)

---
