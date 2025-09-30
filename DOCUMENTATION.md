# ChurrasTech ERP - Sistema de Gestão para Churrascaria

## Visão Geral

O ChurrasTech ERP é um sistema completo de gestão desenvolvido em C# .NET 9.0, específico para churrascarias. O sistema oferece funcionalidades para gerenciamento de produtos, vendas, compras, estoque e relatórios.

## Arquitetura do Sistema

### Projetos

1. **ERP.Core** - Biblioteca base contendo modelos, serviços e contexto de dados
2. **ERP.MainApp** - Aplicação principal (ambiente de produção)
3. **ERP.TestApp** - Aplicação de teste (ambiente de teste)
4. **ERP.Tests** - Testes unitários
5. **ERP.Backup** - Sistema de backup e restauração

### Tecnologias Utilizadas

- **.NET 9.0** - Framework principal
- **Entity Framework Core 9.0** - ORM para acesso aos dados
- **Pomelo MySQL** - Provider para MySQL
- **xUnit** - Framework de testes
- **Microsoft.Extensions.Hosting** - Injeção de dependências e hosting

## Modelo de Dados

### Tabelas Principais

1. **TipoProduto** - Categorização dos produtos (Carnes, Acompanhamentos, Bebidas, Temperos)
2. **UnidadeMedida** - Unidades de medida (KG, UN, L, G)
3. **Produto** - Produtos da churrascaria
4. **FormaPagamento** - Formas de pagamento (Dinheiro, Cartão, PIX)
5. **Venda** - Registro de vendas
6. **ItemVenda** - Itens vendidos
7. **Compra** - Registro de compras
8. **ItemCompra** - Itens comprados
9. **Estoque** - Locais de estoque
10. **ItensUnitarioNoEstoque** - Controle unitário do estoque

### Relacionamentos

- Produto possui TipoProduto e UnidadeMedida
- Venda e Compra possuem FormaPagamento
- ItemVenda referencia Venda e Produto
- ItemCompra referencia Compra e Produto
- ItensUnitarioNoEstoque referencia Estoque e Produto

## Funcionalidades Implementadas

### 1. Gestão de Produtos
- Listagem de produtos
- Busca por código
- Cadastro de produtos
- Controle de estoque baixo
- Desativação de produtos

### 2. Sistema de Backup
- Backup automático do banco de dados
- Restauração de backups
- Suporte a agendamento

### 3. Ambientes Separados
- Ambiente de produção (ERP.MainApp)
- Ambiente de teste (ERP.TestApp)
- Bancos de dados independentes

### 4. Testes Unitários
- Testes para serviços principais
- Uso de banco em memória para testes
- Cobertura das funcionalidades críticas

## Como Executar

### Pré-requisitos
- .NET 9.0 SDK
- MySQL Server (opcional, usa in-memory para demo)

### Comandos

```bash
# Compilar solução
dotnet build

# Executar testes
dotnet test

# Executar aplicação principal
dotnet run --project src/ERP.MainApp/ERP.MainApp.csproj

# Executar aplicação de teste
dotnet run --project src/ERP.TestApp/ERP.TestApp.csproj

# Executar backup
dotnet run --project scripts/ERP.Backup/ERP.Backup.csproj backup
```

## Estrutura de Menus

### Menu Principal
1. Produtos
2. Compras (não implementado)
3. Vendas (não implementado)
4. Estoque (não implementado)
5. Relatórios (não implementado)
6. Formas de Pagamento (não implementado)

### Menu de Produtos
1. Listar Produtos
2. Buscar Produto por Código
3. Cadastrar Produto
4. Editar Produto (não implementado)
5. Desativar Produto
6. Produtos com Estoque Baixo

## Dados Iniciais (Seed)

O sistema vem pré-configurado com:
- 4 tipos de produto (Carnes, Acompanhamentos, Bebidas, Temperos)
- 4 unidades de medida (KG, UN, L, G)
- 4 formas de pagamento (Dinheiro, Débito, Crédito, PIX)
- 1 estoque principal

## Configuração de Banco

### MySQL (Produção)
```
Server=localhost
Database=churrastech_prod
UID=root
PWD=
```

### MySQL (Teste)
```
Server=localhost
Database=churrastech_test
UID=root
PWD=
```

### In-Memory (Demo)
Para demonstração, o sistema usa banco em memória quando MySQL não está disponível.

# ChurrasTech ERP - Documentação Completa

## Regras de Negócio (RN)
___________________________________________
• RN01: Todo Produto deve ser cadastrado com nome, descrição, tipo, unidade de medida, valor unitário e se é congelado ou não.
• RN02: O sistema deve registrar a quantidade em estoque de cada produto. A cada compra, o estoque é aumentado. A cada venda, o estoque é reduzido automaticamente. O sistema deve impedir a venda de produto com estoque insuficiente.
• RN03: O sistema deve aplicar automaticamente a taxa de rendimento após preparo: frango, linguiça e carne suína → 50%; carne bovina → 40%; costela → 50%. Exemplo: se o cliente pedir 1 kg de coxa assada, o sistema deve abater 2 kg crus do estoque.
• RN05: Em uma Venda, o sistema deve registrar data, valor total e forma de pagamento obrigatoriamente.
• RN06: O desconto em um item de venda (valorDesconto) não pode ser maior que o valor total sem desconto.
• RN07: Cada Compra deve registrar data, produto e quantidade adquirida obrigatoriamente.
• RN08: Cada ItemCompra deve estar vinculado a uma Compra e a um Produto válidos.
• RN09: A inclusão de um ItemCompra deve gerar aumento automático no Estoque.
• RN10: O sistema deve impedir vendas de produtos com estoque insuficiente.
• RN11: Caso um produto tenha sido comprado em lotes diferentes (ItemCompra), o sistema deve dar baixa do estoque pelo critério FIFO (primeiro que entra, primeiro que sai).
• RN12: A tabela ItensUnitarioNoEstoque deve permitir rastrear de qual compra veio cada produto no estoque.

---

## Regra para Alterações de Estrutura do Banco
- **Toda alteração de estrutura do banco (criação de tabelas, colunas, etc.) deve ser feita em scripts SQL únicos.**
- O script deve ter nome fácil de entender, por exemplo: `CriaColCongelado.sql`.
- Adicione o script na pasta `scripts/ERP.Banco/` e faça commit no repositório.
- Assim, todos os desenvolvedores podem aplicar as alterações no banco local facilmente.

---

## Instalação e Configuração

### Requisitos
- Windows 10 ou superior
- Visual Studio 2022 (com .NET 9 e Windows Forms)
- .NET 9 SDK
- MySQL Server (recomendado: MySQL Workbench)

### Passo a Passo do Banco de Dados
1. Instale o MySQL Server e o MySQL Workbench.
2. Crie os bancos de dados:
   ```sql
   CREATE DATABASE ERP_teste;
   CREATE DATABASE churrastech_prod;
   ```
3. Crie o usuário e permissões:
   ```sql
   CREATE USER 'DEV'@'localhost' IDENTIFIED BY 'unicid2025';
   GRANT ALL PRIVILEGES ON ERP_teste.* TO 'DEV'@'localhost';
   GRANT ALL PRIVILEGES ON churrastech_prod.* TO 'DEV'@'localhost';
   FLUSH PRIVILEGES;
   ```

### Compartilhamento de Dados do Banco
- **Não é recomendado** compartilhar arquivos do diretório `C:\Program Files\MySQL` diretamente pelo GitHub, pois são arquivos binários do servidor e não funcionam para replicação entre ambientes.
- Para compartilhar dados e estrutura, utilize **migrações do Entity Framework Core** e scripts SQL de backup.
- Gere e compartilhe backups SQL usando o projeto `ERP.Backup` ou o comando `mysqldump`.
- Inclua arquivos de migração (`Migrations` do EF Core) no repositório para que outros desenvolvedores possam atualizar o banco local com `dotnet ef database update`.

### Como Restaurar Dados
- Para restaurar um backup SQL, use o MySQL Workbench ou o comando:
  ```bash
  mysql -u DEV -p ERP_teste < backup.sql
  ```
- Para aplicar migrações do EF Core:
  ```bash
  dotnet ef database update -p src/ERP.Core/ERP.Core.csproj -s src/ERP.Launcher/ERP.Launcher.csproj
  ```

---

## Interface Visual
- Para editar a interface visual, abra o arquivo de formulário (`TelaPrincipal.cs`) no Visual Studio e use o designer do Windows Forms.
- Adicione componentes arrastando da Toolbox para o formulário.
- Edite propriedades e eventos conforme necessário.

---

## Estrutura do Projeto
- `ERP.Launcher`: Interface gráfica inicial
- `ERP.MainApp`: Menu principal (console)
- `ERP.Core`: Modelos, contexto de dados, serviços
- `ERP.TestApp`: Ambiente de testes
- `ERP.Tests`: Testes unitários
- `ERP.Backup`: Backup e restauração do banco

---

## Replicação de Alterações do Banco para DEV
- Sempre que alterar modelos, crie uma nova migração do EF Core e faça commit da pasta `Migrations`.
- Compartilhe scripts de backup SQL se necessário.
- Não compartilhe arquivos binários do MySQL Server pelo repositório.

---

## Suporte
- Consulte o README.md para instruções detalhadas.
- Para dúvidas, abra uma issue no repositório.

ChurrasTech ERP - Documentação atualizada por GitHub Copilot