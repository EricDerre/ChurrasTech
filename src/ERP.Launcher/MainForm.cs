using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using ERP.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace ERP.Launcher
    //teste Kennedy
{
    // Formul�rio principal da aplica��o
    public class TelaPrincipal : Form
    {
        // Label para mostrar o status da conex�o com o banco
        private Label rotuloStatus;

        // Construtor do formul�rio
        public TelaPrincipal()
        {
            // Configura��es iniciais do formul�rio
            this.Text = "ChurrasTech Launcher";
            this.Size = new Size(400, 220);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Label de boas-vindas
            var rotuloBoasVindas = new Label()
            {
                Text = "Bem vindo ao ChurrasTech",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(80, 30)
            };
            this.Controls.Add(rotuloBoasVindas);

            // Label para status da conex�o
            rotuloStatus = new Label()
            {
                Text = "Testando conex�o com o banco...",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                AutoSize = true,
                Location = new Point(80, 70),
                ForeColor = Color.DarkGray
            };
            this.Controls.Add(rotuloStatus);

            // Bot�o "Entrar" (ainda n�o implementado)
            var botaoEntrar = new Button()
            {
                Text = "Entrar",
                Location = new Point(80, 130),
                Size = new Size(100, 40)
            };
            // botaoEntrar.Click += (s, e) => { /* Aqui iniciar� o ERP */ };
            this.Controls.Add(botaoEntrar);

            // Bot�o "Sair" para fechar o programa
            var botaoSair = new Button()
            {
                Text = "Sair",
                Location = new Point(220, 130),
                Size = new Size(100, 40)
            };
            botaoSair.Click += (s, e) => { this.Close(); };
            this.Controls.Add(botaoSair);

            // Evento de carregamento do formul�rio
            this.Load += TelaPrincipal_Load;
        }

        // Evento chamado ao carregar o formul�rio
        private async void TelaPrincipal_Load(object? sender, EventArgs e)
        {
            await TestarConexaoBancoAsync();
        }

        private void InitializeComponent()
        {

        }

        // M�todo para testar a conex�o com o banco de dados MySQL
        private async Task TestarConexaoBancoAsync()
        {
            try
            {
                // Configura as op��es do DbContext para usar MySQL
                var opcoes = new DbContextOptionsBuilder<ChurrasDbContext>()
                    .UseMySql("server=localhost;database=ERP_teste;uid=DEV;pwd=unicid2025;", new MySqlServerVersion(new Version(8, 0, 36)))
                    .Options;
                // Cria o contexto do banco
                using var contexto = new ChurrasDbContext(opcoes);
                // Tenta abrir e fechar a conex�o
                await contexto.Database.OpenConnectionAsync();
                await contexto.Database.CloseConnectionAsync();
                // Se sucesso, mostra mensagem positiva
                rotuloStatus.Text = "Conex�o com o banco de dados OK";
                rotuloStatus.ForeColor = Color.Green;
            }
            catch (Exception excecao)
            {
                // Se ocorrer erro, mostra mensagem de erro
                rotuloStatus.Text = $"Erro ao conectar: {excecao.Message}";
                rotuloStatus.ForeColor = Color.Red;
            }
        }
    }
}
