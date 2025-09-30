-- ChurrasTech ERP - Script de Atualiza��o de Dados do Banco
-- Execute este script no MySQL Workbench ou via linha de comando para atualizar os dados do banco
-- ESSE ARQUIVO EXISTE APENAS PARA TER UM CONTROLE DE VERS�O ARCAICO DENTRO DO BANCO, N�O � NECESS�RIO RODAR TODA A VEZ, APENAS SE PRECISAR DE ALGUMA ALTERA��O EM ESPECIFICO
-- TODAS AS ALTERA��ES DO TIPO DE CRIA��O DE TABELEAS, COLUNAS E COISAS QUE ALTEREM A ESTRUTURA DO BANCO, DEVEM SER FEITAS EM SCRIPTS, OU SEJA, ARQUIVOS �NICOS COM ESSA ALTERA��O


-- Exemplo de atualiza��o de dados
-- Adicione seus INSERTs, UPDATEs ou DELETEs abaixo

-- Exemplo: Inserir novo produto
INSERT INTO Produto (Codigo, Nome, Descricao, PrecoCompra, PrecoVenda, QuantidadeMinima, QuantidadeAtual, TipoProdutoId, UnidadeMedidaId, Ativo, DataCriacao)
VALUES ('P006', 'Frango', 'Frango para churrasco', 20.00, 35.00, 5, 15, 1, 1, true, NOW());

-- Exemplo: Atualizar pre�o de produto
UPDATE Produto SET PrecoVenda = 90.00 WHERE Codigo = 'P001';

-- Exemplo: Desativar produto
UPDATE Produto SET Ativo = false WHERE Codigo = 'P005';

-- Adicione mais comandos conforme necess�rio


