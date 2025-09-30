# ChurrasTech
Projeto de ERP para uma churrascaria

DEV
unicid2025

---

# ChurrasTech ERP - Guia Completo de Instalação, Configuração, Uso e Regras de Negócio

## Requisitos
- Windows 10 ou superior
- Visual Studio 2022 (com suporte a .NET 9 e Windows Forms)
- .NET 9 SDK
- MySQL Server (recomendado: MySQL Workbench para administração)

---

## 2. Passo a Passo para Configurar o Banco de Dados

### 2.1 Instale o MySQL Server
- Baixe e instale o MySQL Server: https://dev.mysql.com/downloads/mysql/
- Instale o MySQL Workbench para facilitar a administração: https://dev.mysql.com/downloads/workbench/

### 2.2 Crie o banco de dados
Abra o MySQL Workbench e execute:
```sql
CREATE DATABASE ERP_teste;
CREATE DATABASE churrastech_prod;
```

### 2.3 Crie o usuário e dê permissões
```sql
CREATE USER 'DEV'@'localhost' IDENTIFIED BY 'unicid2025';
GRANT ALL PRIVILEGES ON ERP_teste.* TO 'DEV'@'localhost';
GRANT ALL PRIVILEGES ON churrastech_prod.* TO 'DEV'@'localhost';
FLUSH PRIVILEGES;
```

---

## 3. Configuração do Projeto

### 3.1 Clone o repositório
```bash
git clone https://github.com/EricDerre/ChurrasTech.git
```

### 3.2 Abra a solução no Visual Studio
- Abra o arquivo `ChurrasTech.sln`.
- Aguarde o Visual Studio restaurar os pacotes NuGet.

### 3.3 Configure o projeto de inicialização
- No Solution Explorer, clique com o botão direito em `ERP.Launcher`.
- Selecione "Definir como projeto de inicialização".

### 3.4 Execute o projeto
- Pressione F5 ou Ctrl+F5 para rodar.
- A tela inicial será exibida e mostrará o status da conexão com o banco.

---

## 4. Como Abrir e Editar a Interface Visual

### 4.1 Abrir o designer visual
- No Solution Explorer, localize `TelaPrincipal.cs` (ou `MainForm.cs`).
- Clique duas vezes para abrir o designer visual do Windows Forms.
- Se não abrir, clique com o botão direito e selecione "Exibir Designer".

### 4.2 Adicionar componentes visuais
- Abra a Toolbox (Exibir > Caixa de Ferramentas).
- Arraste botões, labels, campos de texto, etc. para o formulário.
- Configure propriedades na janela de propriedades.
- Clique duas vezes em um botão para criar o evento de clique.

### 4.3 Caminho das principais funções
- Tela inicial: `src/ERP.Launcher/TelaPrincipal.cs`
- Lógica de conexão: método `TestarConexaoBancoAsync` em `TelaPrincipal.cs`
- Menu principal (console): `src/ERP.MainApp/UI/MainMenu.cs`
- Gestão de produtos: `src/ERP.MainApp/UI/ProdutoMenu.cs`
- Contexto do banco: `src/ERP.Core/Data/ChurrasDbContext.cs`

---

## 5. Replicação de Alterações no Banco de Dados para DEV

### 5.1 Migrações do Entity Framework
Para garantir que todos os desenvolvedores tenham o mesmo modelo de banco:
- Use as migrações do Entity Framework Core.
- No terminal do Visual Studio, execute:
```bash
dotnet ef migrations add NomeDaMigracao -p src/ERP.Core/ERP.Core.csproj -s src/ERP.Launcher/ERP.Launcher.csproj
```
- Para aplicar as migrações no banco:
```bash
dotnet ef database update -p src/ERP.Core/ERP.Core.csproj -s src/ERP.Launcher/ERP.Launcher.csproj
```
- Compartilhe a pasta `Migrations` do projeto `ERP.Core` no repositório.
- Assim, qualquer DEV pode rodar `dotnet ef database update` para atualizar o banco local.

### 5.2 Recomendações
- Sempre que alterar modelos, crie uma nova migração e faça commit da pasta `Migrations`.
- Documente as alterações no README.

---

## 6. Estrutura dos Projetos
- `ERP.Launcher`: Interface gráfica inicial (Windows Forms)
- `ERP.MainApp`: Menu principal e lógica de negócio (console)
- `ERP.Core`: Modelos, contexto de dados e serviços
- `ERP.TestApp`: Ambiente de testes
- `ERP.Tests`: Testes unitários
- `ERP.Backup`: Sistema de backup do banco

---

## 7. Dicas Gerais
- Para criar novos formulários visuais, adicione um novo item do tipo "Formulário do Windows Forms".
- Para editar menus e lógica, altere os arquivos em `src/ERP.MainApp/UI/`.
- Para alterar modelos de dados, edite os arquivos em `src/ERP.Core/Models/` e crie migrações.

---

## 8. Suporte
- Para dúvidas, consulte este README ou abra uma issue no repositório.
- Para problemas com o banco, verifique permissões do usuário e se o MySQL está rodando.

---

## Regras de Negócio (RN)
___________________________________________
• RN01: Todo Produto deve ser cadastrado com nome, descrição, tipo, unidade de medida, valor unitário e se é congelado ou não.
• RN02: O sistema deve registrar a quantidade em estoque de cada produto. A cada compra, o estoque é aumentado. A cada venda, o estoque é reduzido automaticamente. O sistema deve impedir a venda de produto com estoque insuficiente.
• RN03: O sistema deve aplicar automaticamente a taxa de rendimento após preparo: frango, linguiça e carne suína ? 50%; carne bovina ? 40%; costela ? 50%. Exemplo: se o cliente pedir 1 kg de coxa assada, o sistema deve abater 2 kg crus do estoque.
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

## Instalação, Configuração e Uso

(continua com o restante do conteúdo já presente)
