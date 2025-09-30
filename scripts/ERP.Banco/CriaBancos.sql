-- ================================
-- LIMPEZA (recriar do zero)
-- ================================
DROP SCHEMA IF EXISTS `ERP_Teste`;
DROP SCHEMA IF EXISTS `ERP_Producao`;

-- Para evitar erro de modes
SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- ================
-- SCHEMA: ERP_Teste
-- ================
CREATE SCHEMA IF NOT EXISTS `ERP_Teste` DEFAULT CHARACTER SET utf8 ;
USE `ERP_Teste` ;

-- Tabelas do ERP

CREATE TABLE `UnidadeMedida` (
  `idUnidadeMedida` INT NOT NULL AUTO_INCREMENT,
  `descricaoUnidadeMedida` VARCHAR(150) NOT NULL,
  `siglaUnidadeMedida` CHAR(2) NOT NULL,
  PRIMARY KEY (`idUnidadeMedida`),
  UNIQUE INDEX `idUnidadeMedida_UNIQUE` (`idUnidadeMedida` ASC) VISIBLE)
ENGINE = InnoDB;

CREATE TABLE `TipoProduto` (
  `idTipoProduto` INT NOT NULL AUTO_INCREMENT,
  `descricaoTipoProtudo` VARCHAR(100) NOT NULL,
  `siglaTipoProduto` CHAR(2) NOT NULL,
  PRIMARY KEY (`idTipoProduto`),
  UNIQUE INDEX `siglaTipoProduto_UNIQUE` (`siglaTipoProduto` ASC) VISIBLE,
  UNIQUE INDEX `idTipoProduto_UNIQUE` (`idTipoProduto` ASC) VISIBLE)
ENGINE = InnoDB;

CREATE TABLE `Produto` (
  `idProduto` INT NOT NULL AUTO_INCREMENT,
  `nomeProduto` VARCHAR(100) NOT NULL,
  `descricaoProduto` VARCHAR(255) NULL,
  `idTipoProduto` INT NOT NULL,
  `idUnidadeMedida` INT NOT NULL,
  `valorUnitarioProduto` DECIMAL(10,4) NOT NULL,
  `congelado` TINYINT NOT NULL,
  PRIMARY KEY (`idProduto`),
  UNIQUE INDEX `idProduto_UNIQUE` (`idProduto` ASC) VISIBLE,
  INDEX `idUnidadeMedida_idx` (`idUnidadeMedida` ASC) VISIBLE,
  INDEX `idTipoProduto_idx` (`idTipoProduto` ASC) VISIBLE,
  CONSTRAINT `fk_produto_unidademedida`
    FOREIGN KEY (`idUnidadeMedida`)
    REFERENCES `UnidadeMedida` (`idUnidadeMedida`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_produto_tipoproduto`
    FOREIGN KEY (`idTipoProduto`)
    REFERENCES `TipoProduto` (`idTipoProduto`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE TABLE `FormaPagamento` (
  `idFormaPagamento` INT NOT NULL AUTO_INCREMENT,
  `descricaoFormaPagamento` VARCHAR(100) NOT NULL,
  `siglaFormaPagamento` CHAR(3) NOT NULL,
  PRIMARY KEY (`idFormaPagamento`),
  UNIQUE INDEX `idFormaPagamento_UNIQUE` (`idFormaPagamento` ASC) VISIBLE,
  UNIQUE INDEX `siglaFormaPagamento_UNIQUE` (`siglaFormaPagamento` ASC) VISIBLE)
ENGINE = InnoDB;

CREATE TABLE `Venda` (
  `idVenda` INT NOT NULL AUTO_INCREMENT,
  `dataVenda` DATETIME NOT NULL,
  `valorTotalVenda` DECIMAL(10,4) NOT NULL,
  `idFormaPagamento` INT NOT NULL,
  PRIMARY KEY (`idVenda`),
  UNIQUE INDEX `idVenda_UNIQUE` (`idVenda` ASC) VISIBLE,
  INDEX `idFormaPagamento_idx` (`idFormaPagamento` ASC) VISIBLE,
  CONSTRAINT `fk_venda_formapagamento`
    FOREIGN KEY (`idFormaPagamento`)
    REFERENCES `FormaPagamento` (`idFormaPagamento`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE TABLE `ItemVenda` (
  `iditemVenda` INT NOT NULL AUTO_INCREMENT,
  `idVenda` INT NOT NULL,
  `idProduto` INT NOT NULL,
  `quantidadeVendida` FLOAT(10,2) NOT NULL,
  `idUnidadeMedida` INT NOT NULL,
  `valorUnitarioProdutoVenda` DECIMAL(10,4) NOT NULL,
  `valorTotalSemDesconto` DECIMAL(10,4) NOT NULL,
  `valorTotalComDesconto` DECIMAL(10,4) NOT NULL,
  `valorDesconto` DECIMAL(10,4) NULL,
  PRIMARY KEY (`iditemVenda`),
  UNIQUE INDEX `iditemVenda_UNIQUE` (`iditemVenda` ASC) VISIBLE,
  INDEX `idProduto_idx` (`idProduto` ASC) VISIBLE,
  INDEX `idUnidadeMedida_idx` (`idUnidadeMedida` ASC) VISIBLE,
  INDEX `idVenda_idx` (`idVenda` ASC) VISIBLE,
  CONSTRAINT `fk_itemvenda_produto`
    FOREIGN KEY (`idProduto`)
    REFERENCES `Produto` (`idProduto`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT,
  CONSTRAINT `fk_itemvenda_venda`
    FOREIGN KEY (`idVenda`)
    REFERENCES `Venda` (`idVenda`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_itemvenda_unidademedida`
    FOREIGN KEY (`idUnidadeMedida`)
    REFERENCES `UnidadeMedida` (`idUnidadeMedida`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB;

CREATE TABLE `Compra` (
  `idCompra` INT NOT NULL AUTO_INCREMENT,
  `idProduto` INT NOT NULL,
  `quantidadeCompraProduto` FLOAT(10,2) NOT NULL,
  `dataCompra` DATETIME NOT NULL,
  PRIMARY KEY (`idCompra`),
  UNIQUE INDEX `idCompra_UNIQUE` (`idCompra` ASC) VISIBLE,
  INDEX `idProduto_idx` (`idProduto` ASC) VISIBLE,
  CONSTRAINT `fk_compra_produto`
    FOREIGN KEY (`idProduto`)
    REFERENCES `Produto` (`idProduto`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE TABLE `ItemCompra` (
  `idItemCompra` INT NOT NULL AUTO_INCREMENT,
  `idCompra` INT NOT NULL,
  `idProduto` INT NOT NULL,
  `quantidadeCompra` FLOAT(10,2) NOT NULL,
  `idUnidadeMedida` INT NOT NULL,
  `valorUnitarioProdutoCompra` DECIMAL(10,4) NOT NULL,
  `valorTotalCompra` DECIMAL(10,4) NOT NULL,
  PRIMARY KEY (`idItemCompra`),
  UNIQUE INDEX `idItemPedidoCompra_UNIQUE` (`idItemCompra` ASC) VISIBLE,
  INDEX `idCompra_idx` (`idCompra` ASC) VISIBLE,
  INDEX `idProduto_idx` (`idProduto` ASC) VISIBLE,
  INDEX `idUnidadeMedida_idx` (`idUnidadeMedida` ASC) VISIBLE,
  CONSTRAINT `fk_itemcompra_compra`
    FOREIGN KEY (`idCompra`)
    REFERENCES `Compra` (`idCompra`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_itemcompra_produto`
    FOREIGN KEY (`idProduto`)
    REFERENCES `Produto` (`idProduto`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_itemcompra_unidademedida`
    FOREIGN KEY (`idUnidadeMedida`)
    REFERENCES `UnidadeMedida` (`idUnidadeMedida`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE TABLE `Estoque` (
  `idEstoque` INT NOT NULL AUTO_INCREMENT,
  `quantidadeEstoqueAtual` FLOAT(10,2) NOT NULL,
  `idProduto` INT NOT NULL,
  PRIMARY KEY (`idEstoque`),
  UNIQUE INDEX `idEstoque_UNIQUE` (`idEstoque` ASC) VISIBLE,
  INDEX `idProduto_idx` (`idProduto` ASC) VISIBLE,
  CONSTRAINT `fk_estoque_produto`
    FOREIGN KEY (`idProduto`)
    REFERENCES `Produto` (`idProduto`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE TABLE `ItensUnitarioNoEstoque` (
  `idItensNoEstoque` INT NOT NULL AUTO_INCREMENT,
  `idCompra` INT NOT NULL,
  `idItemCompra` INT NOT NULL,
  PRIMARY KEY (`idItensNoEstoque`),
  UNIQUE INDEX `idItensNoEstoque_UNIQUE` (`idItensNoEstoque` ASC) VISIBLE,
  INDEX `idCompra_idx` (`idCompra` ASC) VISIBLE,
  INDEX `idItemCompra_idx` (`idItemCompra` ASC) VISIBLE,
  CONSTRAINT `fk_itensunitario_compra`
    FOREIGN KEY (`idCompra`)
    REFERENCES `Compra` (`idCompra`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_itensunitario_itemcompra`
    FOREIGN KEY (`idItemCompra`)
    REFERENCES `ItemCompra` (`idItemCompra`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

-- ================
-- SCHEMA: ERP_Producao
-- ================
CREATE SCHEMA IF NOT EXISTS `ERP_Producao` DEFAULT CHARACTER SET utf8;

USE `ERP_Producao`;

-- Tabelas do ERP
CREATE TABLE `UnidadeMedida` (
  `idUnidadeMedida` INT NOT NULL AUTO_INCREMENT,
  `descricaoUnidadeMedida` VARCHAR(150) NOT NULL,
  `siglaUnidadeMedida` CHAR(2) NOT NULL,
  PRIMARY KEY (`idUnidadeMedida`),
  UNIQUE INDEX `idUnidadeMedida_UNIQUE` (`idUnidadeMedida` ASC) VISIBLE)
ENGINE = InnoDB;

CREATE TABLE `TipoProduto` (
  `idTipoProduto` INT NOT NULL AUTO_INCREMENT,
  `descricaoTipoProtudo` VARCHAR(100) NOT NULL,
  `siglaTipoProduto` CHAR(2) NOT NULL,
  PRIMARY KEY (`idTipoProduto`),
  UNIQUE INDEX `siglaTipoProduto_UNIQUE` (`siglaTipoProduto` ASC) VISIBLE,
  UNIQUE INDEX `idTipoProduto_UNIQUE` (`idTipoProduto` ASC) VISIBLE)
ENGINE = InnoDB;

CREATE TABLE `Produto` (
  `idProduto` INT NOT NULL AUTO_INCREMENT,
  `nomeProduto` VARCHAR(100) NOT NULL,
  `descricaoProduto` VARCHAR(255) NULL,
  `idTipoProduto` INT NOT NULL,
  `idUnidadeMedida` INT NOT NULL,
  `valorUnitarioProduto` DECIMAL(10,4) NOT NULL,
  `congelado` TINYINT NOT NULL,
  PRIMARY KEY (`idProduto`),
  UNIQUE INDEX `idProduto_UNIQUE` (`idProduto` ASC) VISIBLE,
  INDEX `idUnidadeMedida_idx` (`idUnidadeMedida` ASC) VISIBLE,
  INDEX `idTipoProduto_idx` (`idTipoProduto` ASC) VISIBLE,
  CONSTRAINT `fk_produto_unidademedida`
    FOREIGN KEY (`idUnidadeMedida`)
    REFERENCES `UnidadeMedida` (`idUnidadeMedida`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_produto_tipoproduto`
    FOREIGN KEY (`idTipoProduto`)
    REFERENCES `TipoProduto` (`idTipoProduto`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE TABLE `FormaPagamento` (
  `idFormaPagamento` INT NOT NULL AUTO_INCREMENT,
  `descricaoFormaPagamento` VARCHAR(100) NOT NULL,
  `siglaFormaPagamento` CHAR(3) NOT NULL,
  PRIMARY KEY (`idFormaPagamento`),
  UNIQUE INDEX `idFormaPagamento_UNIQUE` (`idFormaPagamento` ASC) VISIBLE,
  UNIQUE INDEX `siglaFormaPagamento_UNIQUE` (`siglaFormaPagamento` ASC) VISIBLE)
ENGINE = InnoDB;

CREATE TABLE `Venda` (
  `idVenda` INT NOT NULL AUTO_INCREMENT,
  `dataVenda` DATETIME NOT NULL,
  `valorTotalVenda` DECIMAL(10,4) NOT NULL,
  `idFormaPagamento` INT NOT NULL,
  PRIMARY KEY (`idVenda`),
  UNIQUE INDEX `idVenda_UNIQUE` (`idVenda` ASC) VISIBLE,
  INDEX `idFormaPagamento_idx` (`idFormaPagamento` ASC) VISIBLE,
  CONSTRAINT `fk_venda_formapagamento`
    FOREIGN KEY (`idFormaPagamento`)
    REFERENCES `FormaPagamento` (`idFormaPagamento`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE TABLE `ItemVenda` (
  `iditemVenda` INT NOT NULL AUTO_INCREMENT,
  `idVenda` INT NOT NULL,
  `idProduto` INT NOT NULL,
  `quantidadeVendida` FLOAT(10,2) NOT NULL,
  `idUnidadeMedida` INT NOT NULL,
  `valorUnitarioProdutoVenda` DECIMAL(10,4) NOT NULL,
  `valorTotalSemDesconto` DECIMAL(10,4) NOT NULL,
  `valorTotalComDesconto` DECIMAL(10,4) NOT NULL,
  `valorDesconto` DECIMAL(10,4) NULL,
  PRIMARY KEY (`iditemVenda`),
  UNIQUE INDEX `iditemVenda_UNIQUE` (`iditemVenda` ASC) VISIBLE,
  INDEX `idProduto_idx` (`idProduto` ASC) VISIBLE,
  INDEX `idUnidadeMedida_idx` (`idUnidadeMedida` ASC) VISIBLE,
  INDEX `idVenda_idx` (`idVenda` ASC) VISIBLE,
  CONSTRAINT `fk_itemvenda_produto`
    FOREIGN KEY (`idProduto`)
    REFERENCES `Produto` (`idProduto`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT,
  CONSTRAINT `fk_itemvenda_venda`
    FOREIGN KEY (`idVenda`)
    REFERENCES `Venda` (`idVenda`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_itemvenda_unidademedida`
    FOREIGN KEY (`idUnidadeMedida`)
    REFERENCES `UnidadeMedida` (`idUnidadeMedida`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB;

CREATE TABLE `Compra` (
  `idCompra` INT NOT NULL AUTO_INCREMENT,
  `idProduto` INT NOT NULL,
  `quantidadeCompraProduto` FLOAT(10,2) NOT NULL,
  `dataCompra` DATETIME NOT NULL,
  PRIMARY KEY (`idCompra`),
  UNIQUE INDEX `idCompra_UNIQUE` (`idCompra` ASC) VISIBLE,
  INDEX `idProduto_idx` (`idProduto` ASC) VISIBLE,
  CONSTRAINT `fk_compra_produto`
    FOREIGN KEY (`idProduto`)
    REFERENCES `Produto` (`idProduto`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE TABLE `ItemCompra` (
  `idItemCompra` INT NOT NULL AUTO_INCREMENT,
  `idCompra` INT NOT NULL,
  `idProduto` INT NOT NULL,
  `quantidadeCompra` FLOAT(10,2) NOT NULL,
  `idUnidadeMedida` INT NOT NULL,
  `valorUnitarioProdutoCompra` DECIMAL(10,4) NOT NULL,
  `valorTotalCompra` DECIMAL(10,4) NOT NULL,
  PRIMARY KEY (`idItemCompra`),
  UNIQUE INDEX `idItemPedidoCompra_UNIQUE` (`idItemCompra` ASC) VISIBLE,
  INDEX `idCompra_idx` (`idCompra` ASC) VISIBLE,
  INDEX `idProduto_idx` (`idProduto` ASC) VISIBLE,
  INDEX `idUnidadeMedida_idx` (`idUnidadeMedida` ASC) VISIBLE,
  CONSTRAINT `fk_itemcompra_compra`
    FOREIGN KEY (`idCompra`)
    REFERENCES `Compra` (`idCompra`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_itemcompra_produto`
    FOREIGN KEY (`idProduto`)
    REFERENCES `Produto` (`idProduto`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_itemcompra_unidademedida`
    FOREIGN KEY (`idUnidadeMedida`)
    REFERENCES `UnidadeMedida` (`idUnidadeMedida`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE TABLE `Estoque` (
  `idEstoque` INT NOT NULL AUTO_INCREMENT,
  `quantidadeEstoqueAtual` FLOAT(10,2) NOT NULL,
  `idProduto` INT NOT NULL,
  PRIMARY KEY (`idEstoque`),
  UNIQUE INDEX `idEstoque_UNIQUE` (`idEstoque` ASC) VISIBLE,
  INDEX `idProduto_idx` (`idProduto` ASC) VISIBLE,
  CONSTRAINT `fk_estoque_produto`
    FOREIGN KEY (`idProduto`)
    REFERENCES `Produto` (`idProduto`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE TABLE `ItensUnitarioNoEstoque` (
  `idItensNoEstoque` INT NOT NULL AUTO_INCREMENT,
  `idCompra` INT NOT NULL,
  `idItemCompra` INT NOT NULL,
  PRIMARY KEY (`idItensNoEstoque`),
  UNIQUE INDEX `idItensNoEstoque_UNIQUE` (`idItensNoEstoque` ASC) VISIBLE,
  INDEX `idCompra_idx` (`idCompra` ASC) VISIBLE,
  INDEX `idItemCompra_idx` (`idItemCompra` ASC) VISIBLE,
  CONSTRAINT `fk_itensunitario_compra`
    FOREIGN KEY (`idCompra`)
    REFERENCES `Compra` (`idCompra`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_itensunitario_itemcompra`
    FOREIGN KEY (`idItemCompra`)
    REFERENCES `ItemCompra` (`idItemCompra`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- ================================
-- USUÁRIOS
-- ================================
CREATE USER IF NOT EXISTS 'DEV'@'localhost' IDENTIFIED BY 'unicid2025';
CREATE USER IF NOT EXISTS 'USER'@'localhost' IDENTIFIED BY 'churrasTech2025';

-- DEV tem acesso total
GRANT ALL PRIVILEGES ON ERP_Teste.* TO 'DEV'@'localhost';
GRANT ALL PRIVILEGES ON ERP_Producao.* TO 'DEV'@'localhost';

-- USER pode apenas manipular dados
GRANT SELECT, INSERT, UPDATE, DELETE ON ERP_Teste.* TO 'USER'@'localhost';
GRANT SELECT, INSERT, UPDATE, DELETE ON ERP_Producao.* TO 'USER'@'localhost';

FLUSH PRIVILEGES;

-- ================================
-- RESTAURA MODES
-- ================================
SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
