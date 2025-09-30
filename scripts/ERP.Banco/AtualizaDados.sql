-- ChurrasTech ERP - Script de Atualização de Dados do Banco
-- Execute este script no MySQL Workbench ou via linha de comando para atualizar os dados do banco
-- ESSE ARQUIVO EXISTE APENAS PARA TER UM CONTROLE DE VERSÃO ARCAICO DENTRO DO BANCO, NÃO É NECESSÁRIO RODAR TODA A VEZ, APENAS SE PRECISAR DE ALGUMA ALTERAÇÃO EM ESPECIFICO
-- TODAS AS ALTERAÇÕES DO TIPO DE CRIAÇÃO DE TABELEAS, COLUNAS E COISAS QUE ALTEREM A ESTRUTURA DO BANCO, DEVEM SER FEITAS EM SCRIPTS, OU SEJA, ARQUIVOS ÚNICOS COM ESSA ALTERAÇÃO


-- Exemplo de atualização de dados
-- Adicione seus INSERTs, UPDATEs ou DELETEs abaixo

-- Exemplo: Inserir novo produto
INSERT INTO Produto (Codigo, Nome, Descricao, PrecoCompra, PrecoVenda, QuantidadeMinima, QuantidadeAtual, TipoProdutoId, UnidadeMedidaId, Ativo, DataCriacao)
VALUES ('P006', 'Frango', 'Frango para churrasco', 20.00, 35.00, 5, 15, 1, 1, true, NOW());

-- Exemplo: Atualizar preço de produto
UPDATE Produto SET PrecoVenda = 90.00 WHERE Codigo = 'P001';

-- Exemplo: Desativar produto
UPDATE Produto SET Ativo = false WHERE Codigo = 'P005';

-- Adicione mais comandos conforme necessário


