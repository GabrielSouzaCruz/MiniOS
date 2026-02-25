using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using MiniOS.Controllers;

namespace MiniOS.Views
{
    public partial class MainWindow : Window
    {
        private readonly SystemController _controller;

        public MainWindow(SystemController controller)
        {
            InitializeComponent();
            _controller = controller;

            // Redireciona os "Console.WriteLine" do Kernel para o TextBox do WPF
            Console.SetOut(new ControlWriter(txtTerminal));

            Console.WriteLine("========================================");
            Console.WriteLine("    MiniOS Kernel Iniciado com Sucesso  ");
            Console.WriteLine("========================================\n");

            // Inicializa as interfaces visuais no arranque
            UpdateMemoryUI();
            UpdateFileSystemUI();
        }

        // ==========================================
        // MÉTODOS DE ATUALIZAÇÃO DA INTERFACE (UI)
        // ==========================================
        private void UpdateUI()
        {
            // 1. Controla o botão do escalonador
            btnRunScheduler.IsEnabled = _controller.GetReadyQueueCount() > 0;

            // 2. Atualiza a tabela do Gestor de Tarefas
            gridProcesses.ItemsSource = null; // Limpa a referência antiga
            gridProcesses.ItemsSource = _controller.GetReadyQueue(); // Injeta a nova lista
        }

        private void UpdateMemoryUI()
        {
            int total = _controller.GetTotalMemory();
            int used = _controller.GetUsedMemory();

            pbRamUsage.Maximum = total;
            pbRamUsage.Value = used;
            lblMemoryStatus.Text = $"Uso da RAM: {used} MB / {total} MB";

            // Lógica para mudar a cor da barra (verde -> amarelo -> vermelho)
            double percentage = (double)used / total;

            if (percentage > 0.85)
                pbRamUsage.Foreground = System.Windows.Media.Brushes.Red; // Quase a estourar!
            else if (percentage > 0.60)
                pbRamUsage.Foreground = System.Windows.Media.Brushes.Yellow; // Começou a pesar
            else
                pbRamUsage.Foreground = System.Windows.Media.Brushes.LimeGreen; // Tá leve
        }

        private void UpdateFileSystemUI()
        {
            gridFiles.ItemsSource = null;
            gridFiles.ItemsSource = _controller.GetAllFiles();
        }

        // ==========================================
        // EVENTOS DOS BOTÕES (CLICKS)
        // ==========================================

        // --- Gestão de Processos ---
        private void BtnCreateProcess_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtProcessTime.Text, out int time))
            {
                _controller.CreateProcess(txtProcessName.Text, time);
                UpdateUI();
            }
            else
            {
                Console.WriteLine("[Erro UI] Tempo de execução inválido.");
            }
        }

        private void BtnRunScheduler_Click(object sender, RoutedEventArgs e)
        {
            _controller.ExecuteNextProcess();
            UpdateUI();
        }

        // --- Gestão de Memória ---
        private void BtnAllocateMemory_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtMemSize.Text, out int size))
            {
                _controller.AllocateMemory(size);
                UpdateMemoryUI();
            }
            else
            {
                Console.WriteLine("[Erro UI] Tamanho de memória inválido.");
            }
        }

        private void BtnFreeMemory_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtMemId.Text, out int id))
            {
                _controller.FreeMemory(id);
                UpdateMemoryUI();
            }
            else
            {
                Console.WriteLine("[Erro UI] ID de bloco inválido.");
            }
        }

        private void BtnStatusRAM_Click(object sender, RoutedEventArgs e)
        {
            _controller.ShowMemoryStatus();
        }
        // --- Sistema de Ficheiros ---
        private void BtnCreateFile_Click(object sender, RoutedEventArgs e)
        {
            string fileName = txtFileName.Text.Trim();
            string content = txtFileContent.Text.Trim();

            // O segurança: bloqueia nomes vazios ou o texto de exemplo
            if (string.IsNullOrWhiteSpace(fileName) || fileName == "Nome do Ficheiro")
            {
                Console.WriteLine("[Erro UI] Por favor, digite um nome válido para criar o ficheiro.");
                return; // Pára a execução aqui!
            }

            _controller.CreateFile(fileName, content);
            UpdateFileSystemUI();
        }

        private void BtnReadFile_Click(object sender, RoutedEventArgs e)
        {
            string fileName = txtFileName.Text.Trim();

            // O segurança: bloqueia pesquisas vazias ou com o texto de exemplo
            if (string.IsNullOrWhiteSpace(fileName) || fileName == "Nome do Ficheiro")
            {
                Console.WriteLine("[Erro UI] Por favor, digite o nome do ficheiro que quer ler.");
                return; // Pára a execução aqui!
            }

            _controller.ReadFile(fileName);
        }

        // ==========================================
        // MÉTODOS AUXILIARES
        // ==========================================
        private void RemoveText_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Foreground == System.Windows.Media.Brushes.Gray)
            {
                textBox.Text = string.Empty;
                textBox.Foreground = System.Windows.Media.Brushes.Black;
            }
        }
    } // FIM DA CLASSE MAINWINDOW


    // =======================================================
    // CLASSE MÁGICA DE REDIRECIONAMENTO DO CONSOLE
    // Agora vive do lado de fora da MainWindow para não misturar!
    // =======================================================
    public class ControlWriter : TextWriter
    {
        private readonly TextBox _textbox;

        public ControlWriter(TextBox textbox)
        {
            _textbox = textbox;
        }

        public override void Write(char value)
        {
            _textbox.Dispatcher.Invoke(() =>
            {
                _textbox.AppendText(value.ToString());
                _textbox.ScrollToEnd(); // Faz o scroll automático para o fundo
            });
        }

        public override void Write(string value)
        {
            _textbox.Dispatcher.Invoke(() =>
            {
                _textbox.AppendText(value);
                _textbox.ScrollToEnd();
            });
        }

        public override Encoding Encoding => Encoding.UTF8;
    }
}