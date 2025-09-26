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

## Extensões Futuras

### Módulos a Implementar
- Sistema completo de vendas
- Gestão de compras
- Controle de estoque avançado
- Relatórios gerenciais
- Sistema de clientes
- Interface web (Blazor)
- API REST

### Melhorias Técnicas
- Windows Forms/WPF interface
- Autenticação e autorização
- Logs estruturados
- Cache de dados
- Notificações push
- Integração com PDV

## Testes

O sistema inclui testes unitários abrangentes para:
- Criação de produtos
- Validação de códigos únicos
- Busca de produtos
- Controle de estoque baixo

Execute `dotnet test` para rodar todos os testes.

## Contribuição

Este sistema foi desenvolvido seguindo as melhores práticas de:
- Clean Architecture
- SOLID Principles
- Dependency Injection
- Unit Testing
- Code Documentation

## Licença

Sistema desenvolvido para uso interno da churrascaria.