using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using FontAwesome.Sharp;

namespace ERPLauncher
{
    public partial class TelaHub : Form
    {
        // Lista para rastrear telas abertas
        private List<Form> telasAbertas = new List<Form>();

        // Controle do botão atualmente ativo (menu lateral)
        private IconButton currentButton;

        // Cores
        private readonly Color ColorDefault = Color.FromArgb(30, 30, 30);
        private readonly Color ColorHover = Color.FromArgb(45, 45, 45);
        private readonly Color ColorSelected = Color.FromArgb(249, 115, 22); // laranja

        // Tamanhos dinâmicos para botões da sidebar
        private readonly int SidebarButtonMinHeight = 48; // mínimo aceitável
        private readonly int SidebarButtonMaxHeight = 240; // limite superior (ajuste se quiser)

        // Host panel para carregar forms dentro da aba Estoque
        private Panel pnlEstoqueHost;

        public TelaHub()
        {
            InitializeComponent();

            // Configura TabControl e menu após o designer inicializar os controles
            ConfigureTabControlVisual();
            ConfigureSidebarButtons();

            // Cria host para as telas do módulo Estoque (parte superior da aba)
            EnsureEstoqueHostPanel();

            // Wire botões de dentro da aba Estoque para abrir forms
            if (btEntradaEstoque != null)
                btEntradaEstoque.Click += (s, e) => ShowFormInEstoqueHost(new TelaEntradaEstoque());
            if (btBaixaEstoque != null)
                btBaixaEstoque.Click += (s, e) => ShowFormInEstoqueHost(new TelaBaixaEstoque());

            // Seleciona aba Estoque por padrão (ativa botão e seleciona aba)
            if (btEstoque != null)
                SidebarButton_Click(btEstoque, EventArgs.Empty);

            // Atualiza tamanhos dinâmicos dos botões agora e quando a janela/painel for redimensionado
            UpdateSidebarButtonSizes();
            UpdateEstoqueButtonsSizes();
            this.Resize += (s, e) => { UpdateSidebarButtonSizes(); UpdateEstoqueButtonsSizes(); };
            if (pnlSidebar != null)
                pnlSidebar.SizeChanged += (s, e) => UpdateSidebarButtonSizes();
            if (tabEstoque != null)
                tabEstoque.SizeChanged += (s, e) => UpdateEstoqueButtonsSizes();
            if (tabVendas != null)
            {
                tabVendas.SizeChanged += (s, e) => UpdateVendasButtonSize();
                // também atualiza imediatamente
                UpdateVendasButtonSize();
            }

            // subscribe to tabCompras size changes
            if (tabCompras != null)
            {
                tabCompras.SizeChanged += (s, e) => UpdateComprasButtonSize();
                UpdateComprasButtonSize();
            }

            // subscribe to tabProdutos size changes
            if (tabProdutos != null)
            {
                tabProdutos.SizeChanged += (s, e) => UpdateProdutosButtonsSizes();
                UpdateProdutosButtonsSizes();
            }
        }

        // Configura o TabControl para "esconder" as abas mas manter edição via Designer
        private void ConfigureTabControlVisual()
        {
            if (tabControlMain == null)
                return;

            // Oculta visualmente as abas: tamanho mínimo e owner-draw vazio
            tabControlMain.Appearance = TabAppearance.FlatButtons;
            tabControlMain.SizeMode = TabSizeMode.Fixed;
            tabControlMain.ItemSize = new Size(0, 1);
            tabControlMain.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControlMain.DrawItem -= TabControlMain_DrawItem; // evitar duplo registro
            tabControlMain.DrawItem += TabControlMain_DrawItem;
        }

        // Evento DrawItem vazio para não desenhar cabeçalho
        private void TabControlMain_DrawItem(object sender, DrawItemEventArgs e)
        {
            // Intencionalmente vazio: isso evita desenhar as abas
        }

        // Configura visual/handlers dos botões do menu lateral
        private void ConfigureSidebarButtons()
        {
            if (pnlSidebar == null)
                return;

            // Mapeie os botões existentes no painel (IconButton)
            var buttons = pnlSidebar.Controls.OfType<IconButton>().ToList();

            foreach (var btn in buttons)
            {
                // Visual base: ícone à esquerda e texto à direita por padrão
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.BackColor = ColorDefault;
                btn.ForeColor = Color.WhiteSmoke;
                btn.Cursor = Cursors.Hand;
                btn.AutoSize = false;

                btn.IconColor = Color.White;
                btn.TextImageRelation = TextImageRelation.ImageBeforeText;
                btn.ImageAlign = ContentAlignment.MiddleLeft;
                btn.TextAlign = ContentAlignment.MiddleLeft;
                btn.Padding = new Padding(16, 0, 0, 0);
                btn.Dock = DockStyle.Top;
                btn.Margin = new Padding(0);

                // Eventos
                btn.Click -= SidebarButton_Click;
                btn.Click += SidebarButton_Click;

                btn.MouseEnter -= SidebarButton_MouseEnter;
                btn.MouseEnter += SidebarButton_MouseEnter;

                btn.MouseLeave -= SidebarButton_MouseLeave;
                btn.MouseLeave += SidebarButton_MouseLeave;
            }

            // Força clique no botão de Estoque para selecionar aba correspondente (se existir)
            if (btEstoque != null)
                btEstoque.PerformClick();
        }

        // Handler de clique comum: altera aba e estado visual
        private void SidebarButton_Click(object sender, EventArgs e)
        {
            if (sender is not IconButton btn)
                return;

            ActivateButton(btn);

            if (tabControlMain == null)
                return;

            // Seleciona TabPage diretamente por referência
            if (btn == btEstoque && tabEstoque != null) tabControlMain.SelectedTab = tabEstoque;
            else if (btn == btVendas && tabVendas != null) tabControlMain.SelectedTab = tabVendas;
            else if (btn == btCompras && tabCompras != null) tabControlMain.SelectedTab = tabCompras;
            else if (btn == btProdutos && tabProdutos != null) tabControlMain.SelectedTab = tabProdutos;
            else
            {
                // Fallback: usa índice relativo
                var idx = pnlSidebar.Controls.OfType<IconButton>().ToList().IndexOf(btn);
                if (idx >= 0 && idx < tabControlMain.TabCount)
                    tabControlMain.SelectedIndex = idx;
            }
        }

        private void SidebarButton_MouseEnter(object sender, EventArgs e)
        {
            if (sender is IconButton btn && btn != currentButton)
                btn.BackColor = ColorHover;
        }

        private void SidebarButton_MouseLeave(object sender, EventArgs e)
        {
            if (sender is IconButton btn && btn != currentButton)
                btn.BackColor = ColorDefault;
        }

        // Ativa visualmente um botão (comportamento radio)
        private void ActivateButton(IconButton button)
        {
            if (button == null)
                return;

            if (currentButton == button)
                return;

            // Reset anterior
            if (currentButton != null)
            {
                currentButton.BackColor = ColorDefault;
                currentButton.ForeColor = Color.WhiteSmoke;
                currentButton.IconColor = Color.White;
            }

            // Set novo
            currentButton = button;
            currentButton.BackColor = ColorSelected;
            currentButton.ForeColor = Color.White;
            currentButton.IconColor = Color.White;
        }

        // Exemplo de método para abrir uma tela
        private void AbrirTela(Form tela)
        {
            tela.FormClosed += (s, e) => telasAbertas.Remove(tela);
            telasAbertas.Add(tela);
            tela.Show();
        }

        // Atualiza as alturas dos botões da sidebar proporcionalmente ao tamanho do painel
        private void UpdateSidebarButtonSizes()
        {
            if (pnlSidebar == null)
                return;

            var buttons = pnlSidebar.Controls.OfType<IconButton>().Where(b => b.Visible).ToList();
            if (!buttons.Any())
                return;

            // Altura disponível para os botões (cliente do painel)
            int available = pnlSidebar.ClientSize.Height;

            // Reserve um pouco de espaço superior/inferior se houver (opcional)
            int reserved = 0; // ajuste se tiver header dentro do sidebar
            available = Math.Max(0, available - reserved);

            // Calcule altura por botão
            int per = available / buttons.Count;

            // garantir espaço para ícone + texto
            int maxIcon = buttons.Max(b => b.IconSize);
            int minForIcon = maxIcon + 28; // espaço para texto e padding
            if (per < minForIcon) per = Math.Max(per, minForIcon);

            if (per < SidebarButtonMinHeight) per = SidebarButtonMinHeight;
            if (SidebarButtonMaxHeight > 0 && per > SidebarButtonMaxHeight) per = SidebarButtonMaxHeight;

            // Aplica a altura a cada botão
            foreach (var btn in buttons)
            {
                btn.Height = per;

                // ícone padrão proporcional, sem diferenciação por botão
                int iconSz = Math.Clamp(per / 4, 16, 48);
                btn.IconSize = iconSz;
                btn.Font = new Font(btn.Font.FontFamily, Math.Max(9, iconSz / 3), FontStyle.Bold);
            }
        }

        // Atualiza tamanhos dos botões dentro da aba Estoque
        private void UpdateEstoqueButtonsSizes()
        {
            if (tabEstoque == null) return;

            // Tenta pegar os botões na ordem desejada: quantidade, entrada, baixa
            var names = new[] { "btQuantidadeEstoque", "btEntradaEstoque", "btBaixaEstoque" };
            var buttons = new List<IconButton>();
            foreach (var n in names)
            {
                var found = tabEstoque.Controls.Find(n, true).FirstOrDefault() as IconButton;
                if (found != null) buttons.Add(found);
            }

            // Se nenhum dos nomes foi encontrado, tenta pegar pelos campos existentes
            if (!buttons.Any())
            {
                if (btEntradaEstoque != null) buttons.Add(btEntradaEstoque);
                if (btBaixaEstoque != null) buttons.Add(btBaixaEstoque);
            }

            if (buttons.Count == 0) return;

            int available = tabEstoque.ClientSize.Height;

            // dividir igualmente entre os botões para preencher toda a aba
            int per = Math.Max(1, available / buttons.Count);

            // Vamos posicionar e ordenar os botões para preencher a aba de cima para baixo
            // Para garantir a ordem (quantidade, entrada, baixa), ajustamos o ChildIndex em ordem reversa
            foreach (var btn in buttons)
            {
                // remove temporariamente para evitar duplicatas na reordenação
                if (tabEstoque.Controls.Contains(btn))
                    tabEstoque.Controls.Remove(btn);
            }

            // Adiciona em ordem e posiciona
            foreach (var btn in buttons)
            {
                tabEstoque.Controls.Add(btn);
                btn.Dock = DockStyle.Top;
                btn.Margin = new Padding(0);
                btn.Padding = new Padding(0, 8, 0, 8);
                btn.Height = per;

                // Ajuste dinâmico do tamanho do ícone e da fonte
                int iconSz = Math.Clamp(per / 3, 24, 96);
                btn.IconSize = iconSz;
                btn.Font = new Font(btn.Font.FontFamily, Math.Max(9, iconSz / 3), FontStyle.Bold);

                // centralizar imagem/texto verticalmente
                btn.TextImageRelation = TextImageRelation.ImageAboveText;
                btn.ImageAlign = ContentAlignment.TopCenter;
                btn.TextAlign = ContentAlignment.MiddleCenter;
            }

            // Re-adiciona o host por último e envia para atrás
            if (pnlEstoqueHost != null)
            {
                if (tabEstoque.Controls.Contains(pnlEstoqueHost))
                    tabEstoque.Controls.Remove(pnlEstoqueHost);
                tabEstoque.Controls.Add(pnlEstoqueHost);
                pnlEstoqueHost.SendToBack();
            }
        }

        // Ensure host panel exists inside tabEstoque to load forms
        private void EnsureEstoqueHostPanel()
        {
            if (tabEstoque == null) return;
            if (pnlEstoqueHost != null && tabEstoque.Controls.Contains(pnlEstoqueHost)) return;

            pnlEstoqueHost = new Panel();
            pnlEstoqueHost.Name = "pnlEstoqueHost";
            pnlEstoqueHost.Dock = DockStyle.Fill;
            tabEstoque.Controls.Add(pnlEstoqueHost);
            // send to back so buttons docked bottom remain visible
            pnlEstoqueHost.SendToBack();
        }

        // Carrega um Form dentro do host da aba Estoque
        private void ShowFormInEstoqueHost(Form form)
        {
            if (pnlEstoqueHost == null)
                EnsureEstoqueHostPanel();
            if (pnlEstoqueHost == null) return;

            pnlEstoqueHost.Controls.Clear();
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            pnlEstoqueHost.Controls.Add(form);
            form.Show();
        }

        // Evento de fechamento do Hub
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (telasAbertas.Count > 0)
            {
                MessageBox.Show("Feche todas as telas abertas antes de fechar o Hub.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }
            base.OnFormClosing(e);
        }

        private void TelaHub_Load(object sender, EventArgs e)
        {
            // Garantir que os botões recebam tamanho correto após a janela ser mostrada
            // Em alguns casos as dimensões não estão corretas no construtor, então também atualizamos em Shown
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            // Atualiza os tamanhos dos botões depois que a janela estiver visível e com tamanho final
            UpdateSidebarButtonSizes();
            UpdateEstoqueButtonsSizes();
            // Força redraw/relayout se necessário
            this.Refresh();
        }

        private void labelChurras_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void btEstoque_Click(object sender, EventArgs e)
        {
            // For compatibility: if designer attached this handler, forward to common handler
            SidebarButton_Click(btEstoque, EventArgs.Empty);
        }

        private void pnlHeader_Paint(object sender, PaintEventArgs e)
        {
            // Evento de Paint vazio para evitar erro de compilação
        }

        private void btEstoque_Click_1(object sender, EventArgs e)
        {
            SidebarButton_Click(btEstoque, EventArgs.Empty);
        }

        private void btVendas_Click(object sender, EventArgs e)
        {

        }

        private void btCompras_Click(object sender, EventArgs e)
        {

        }

        private void btProdutos_Click(object sender, EventArgs e)
        {

        }

        private void iconButton1_Click(object sender, EventArgs e)
        {

        }

        private void tabEstoque_Click(object sender, EventArgs e)
        {

        }

        private void UpdateVendasButtonSize()
        {
            if (tabVendas == null || btTelaVendas == null) return;

            // Garantir que o botão preencha toda a área da aba
            btTelaVendas.Dock = DockStyle.Fill;
            btTelaVendas.Margin = new Padding(0);
            btTelaVendas.Padding = new Padding(0, 8, 0, 8);

            // Ajuste dinâmico do tamanho do ícone e da fonte baseado na altura disponível
            int available = tabVendas.ClientSize.Height;
            int iconSz = Math.Clamp(Math.Max(24, available / 6), 24, 120); // heurística
            btTelaVendas.IconSize = iconSz;
            btTelaVendas.TextImageRelation = TextImageRelation.ImageAboveText;
            btTelaVendas.ImageAlign = ContentAlignment.TopCenter;
            btTelaVendas.TextAlign = ContentAlignment.MiddleCenter;
            btTelaVendas.Font = new Font(btTelaVendas.Font.FontFamily, Math.Max(10, iconSz / 3), FontStyle.Bold);
        }

        private void UpdateComprasButtonSize()
        {
            if (tabCompras == null || btCadastroCompras == null) return;
            btCadastroCompras.Dock = DockStyle.Fill;
            btCadastroCompras.Margin = new Padding(0);
            btCadastroCompras.Padding = new Padding(0, 8, 0, 8);

            int available = tabCompras.ClientSize.Height;
            int iconSz = Math.Clamp(Math.Max(24, available / 6), 24, 120);
            btCadastroCompras.IconSize = iconSz;
            btCadastroCompras.TextImageRelation = TextImageRelation.ImageAboveText;
            btCadastroCompras.ImageAlign = ContentAlignment.TopCenter;
            btCadastroCompras.TextAlign = ContentAlignment.MiddleCenter;
            btCadastroCompras.Font = new Font(btCadastroCompras.Font.FontFamily, Math.Max(10, iconSz / 3), FontStyle.Bold);
        }

        private void UpdateProdutosButtonsSizes()
        {
            if (tabProdutos == null) return;
            var names = new[] { "btCadastroFormaPagamento", "btCadastroProduto", "btCadastroTipoProduto", "btCadastroUnMedida" };
            var buttons = new List<IconButton>();
            foreach (var n in names)
            {
                var found = tabProdutos.Controls.Find(n, true).FirstOrDefault() as IconButton;
                if (found != null) buttons.Add(found);
            }
            if (!buttons.Any())
            {
                if (btCadastroFormaPagamento != null) buttons.Add(btCadastroFormaPagamento);
                if (btCadastroProduto != null) buttons.Add(btCadastroProduto);
                if (btCadastroTipoProduto != null) buttons.Add(btCadastroTipoProduto);
                if (btCadastroUnMedida != null) buttons.Add(btCadastroUnMedida);
            }
            if (buttons.Count == 0) return;

            int available = tabProdutos.ClientSize.Height;
            int per = Math.Max(1, available / buttons.Count);

            // remove and re-add in order to ensure stacking top-down
            foreach (var btn in buttons)
            {
                if (tabProdutos.Controls.Contains(btn))
                    tabProdutos.Controls.Remove(btn);
            }
            foreach (var btn in buttons)
            {
                tabProdutos.Controls.Add(btn);
                btn.Dock = DockStyle.Top;
                btn.Margin = new Padding(0);
                btn.Padding = new Padding(0, 8, 0, 8);
                btn.Height = per;

                int iconSz = Math.Clamp(per / 3, 24, 96);
                btn.IconSize = iconSz;
                btn.Font = new Font(btn.Font.FontFamily, Math.Max(9, iconSz / 3), FontStyle.Bold);
                btn.TextImageRelation = TextImageRelation.ImageAboveText;
                btn.ImageAlign = ContentAlignment.TopCenter;
                btn.TextAlign = ContentAlignment.MiddleCenter;
            }
        }
    }
}
