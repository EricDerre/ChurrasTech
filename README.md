# ChurrasTech
Projeto de ERP para uma churrascaria

DEV
unicid2025

---

# ChurrasTech ERP - Guia Completo de Instala��o, Configura��o, Uso e Regras de Neg�cio

## Requisitos
- Windows 10 ou superior
- Visual Studio 2022 (com suporte a .NET 9 e Windows Forms)
- .NET 9 SDK
- MySQL Server (recomendado: MySQL Workbench para administra��o)

---

## 2. Passo a Passo para Configurar o Banco de Dados

### 2.1 Instale o MySQL Server
- Baixe e instale o MySQL Server: https://dev.mysql.com/downloads/mysql/
- Instale o MySQL Workbench para facilitar a administra��o: https://dev.mysql.com/downloads/workbench/

### 2.2 Crie o banco de dados
Abra o MySQL Workbench e execute:
```sql
CREATE DATABASE ERP_teste;
CREATE DATABASE churrastech_prod;
```

### 2.3 Crie o usu�rio e d� permiss�es
```sql
CREATE USER 'DEV'@'localhost' IDENTIFIED BY 'unicid2025';
GRANT ALL PRIVILEGES ON ERP_teste.* TO 'DEV'@'localhost';
GRANT ALL PRIVILEGES ON churrastech_prod.* TO 'DEV'@'localhost';
FLUSH PRIVILEGES;
```

---

## 3. Configura��o do Projeto

### 3.1 Clone o reposit�rio
```bash
git clone https://github.com/EricDerre/ChurrasTech.git
```

### 3.2 Abra a solu��o no Visual Studio
- Abra o arquivo `ChurrasTech.sln`.
- Aguarde o Visual Studio restaurar os pacotes NuGet.

### 3.3 Configure o projeto de inicializa��o
- No Solution Explorer, clique com o bot�o direito em `ERP.Launcher`.
- Selecione "Definir como projeto de inicializa��o".

### 3.4 Execute o projeto
- Pressione F5 ou Ctrl+F5 para rodar.
- A tela inicial ser� exibida e mostrar� o status da conex�o com o banco.

---

## 4. Como Abrir e Editar a Interface Visual

### 4.1 Abrir o designer visual
- No Solution Explorer, localize `TelaPrincipal.cs` (ou `MainForm.cs`).
- Clique duas vezes para abrir o designer visual do Windows Forms.
- Se n�o abrir, clique com o bot�o direito e selecione "Exibir Designer".

### 4.2 Adicionar componentes visuais
- Abra a Toolbox (Exibir > Caixa de Ferramentas).
- Arraste bot�es, labels, campos de texto, etc. para o formul�rio.
- Configure propriedades na janela de propriedades.
- Clique duas vezes em um bot�o para criar o evento de clique.

### 4.3 Caminho das principais fun��es
- Tela inicial: `src/ERP.Launcher/TelaPrincipal.cs`
- L�gica de conex�o: m�todo `TestarConexaoBancoAsync` em `TelaPrincipal.cs`
- Menu principal (console): `src/ERP.MainApp/UI/MainMenu.cs`
- Gest�o de produtos: `src/ERP.MainApp/UI/ProdutoMenu.cs`
- Contexto do banco: `src/ERP.Core/Data/ChurrasDbContext.cs`

---

## 5. Replica��o de Altera��es no Banco de Dados para DEV

### 5.1 Migra��es do Entity Framework
Para garantir que todos os desenvolvedores tenham o mesmo modelo de banco:
- Use as migra��es do Entity Framework Core.
- No terminal do Visual Studio, execute:
```bash
dotnet ef migrations add NomeDaMigracao -p src/ERP.Core/ERP.Core.csproj -s src/ERP.Launcher/ERP.Launcher.csproj
```
- Para aplicar as migra��es no banco:
```bash
dotnet ef database update -p src/ERP.Core/ERP.Core.csproj -s src/ERP.Launcher/ERP.Launcher.csproj
```
- Compartilhe a pasta `Migrations` do projeto `ERP.Core` no reposit�rio.
- Assim, qualquer DEV pode rodar `dotnet ef database update` para atualizar o banco local.

### 5.2 Recomenda��es
- Sempre que alterar modelos, crie uma nova migra��o e fa�a commit da pasta `Migrations`.
- Documente as altera��es no README.

---

## 6. Estrutura dos Projetos
- `ERP.Launcher`: Interface gr�fica inicial (Windows Forms)
- `ERP.MainApp`: Menu principal e l�gica de neg�cio (console)
- `ERP.Core`: Modelos, contexto de dados e servi�os
- `ERP.TestApp`: Ambiente de testes
- `ERP.Tests`: Testes unit�rios
- `ERP.Backup`: Sistema de backup do banco

---

## 7. Dicas Gerais
- Para criar novos formul�rios visuais, adicione um novo item do tipo "Formul�rio do Windows Forms".
- Para editar menus e l�gica, altere os arquivos em `src/ERP.MainApp/UI/`.
- Para alterar modelos de dados, edite os arquivos em `src/ERP.Core/Models/` e crie migra��es.

---

## 8. Suporte
- Para d�vidas, consulte este README ou abra uma issue no reposit�rio.
- Para problemas com o banco, verifique permiss�es do usu�rio e se o MySQL est� rodando.

---

## Regras de Neg�cio (RN)
___________________________________________
� RN01: Todo Produto deve ser cadastrado com nome, descri��o, tipo, unidade de medida, valor unit�rio e se � congelado ou n�o.
� RN02: O sistema deve registrar a quantidade em estoque de cada produto. A cada compra, o estoque � aumentado. A cada venda, o estoque � reduzido automaticamente. O sistema deve impedir a venda de produto com estoque insuficiente.
� RN03: O sistema deve aplicar automaticamente a taxa de rendimento ap�s preparo: frango, lingui�a e carne su�na ? 50%; carne bovina ? 40%; costela ? 50%. Exemplo: se o cliente pedir 1 kg de coxa assada, o sistema deve abater 2 kg crus do estoque.
� RN05: Em uma Venda, o sistema deve registrar data, valor total e forma de pagamento obrigatoriamente.
� RN06: O desconto em um item de venda (valorDesconto) n�o pode ser maior que o valor total sem desconto.
� RN07: Cada Compra deve registrar data, produto e quantidade adquirida obrigatoriamente.
� RN08: Cada ItemCompra deve estar vinculado a uma Compra e a um Produto v�lidos.
� RN09: A inclus�o de um ItemCompra deve gerar aumento autom�tico no Estoque.
� RN10: O sistema deve impedir vendas de produtos com estoque insuficiente.
� RN11: Caso um produto tenha sido comprado em lotes diferentes (ItemCompra), o sistema deve dar baixa do estoque pelo crit�rio FIFO (primeiro que entra, primeiro que sai).
� RN12: A tabela ItensUnitarioNoEstoque deve permitir rastrear de qual compra veio cada produto no estoque.

---

## Regra para Altera��es de Estrutura do Banco
- **Toda altera��o de estrutura do banco (cria��o de tabelas, colunas, etc.) deve ser feita em scripts SQL �nicos.**
- O script deve ter nome f�cil de entender, por exemplo: `CriaColCongelado.sql`.
- Adicione o script na pasta `scripts/ERP.Banco/` e fa�a commit no reposit�rio.
- Assim, todos os desenvolvedores podem aplicar as altera��es no banco local facilmente.

---

## Instala��o, Configura��o e Uso

(continua com o restante do conte�do j� presente)
