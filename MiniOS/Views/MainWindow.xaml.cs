using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using MiniOS.Controllers;
using System.Windows.Threading; 

namespace MiniOS.Views
{
    public partial class MainWindow : Window
    {
        private readonly SystemController _controller;
        private readonly DispatcherTimer _cpuClock;
        private bool _isClockRunning = false;

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
            // Configura o "Coração" do Sistema (1 batida por segundo)
            _cpuClock = new DispatcherTimer();
            _cpuClock.Interval = TimeSpan.FromSeconds(10);
            _cpuClock.Tick += CpuClock_Tick;
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
                UpdateMemoryUI();
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
            UpdateMemoryUI();
        }
        // Este é o método que o relógio chama sozinho a cada 1 segundo!
        private void CpuClock_Tick(object sender, EventArgs e)
        {
            // Só manda executar se houver alguém na fila
            if (_controller.GetReadyQueueCount() > 0)
            {
                _controller.ExecuteNextProcess();
                UpdateUI();
                UpdateMemoryUI();
            }
        }

        // Este é o botão que liga e desliga o motor
        private void BtnToggleClock_Click(object sender, RoutedEventArgs e)
        {
            _isClockRunning = !_isClockRunning; // Inverte o estado

            if (_isClockRunning)
            {
                _cpuClock.Start();
                btnToggleClock.Content = "Desligar Clock";
                btnToggleClock.Background = System.Windows.Media.Brushes.Firebrick; // Fica vermelho
                Console.WriteLine("\n[HARDWARE] Clock da CPU LIGADO! Modo Automático ativado.");
            }
            else
            {
                _cpuClock.Stop();
                btnToggleClock.Content = "Ligar Clock (Auto)";
                btnToggleClock.Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#28A745"); // Volta ao verde
                Console.WriteLine("\n[HARDWARE] Clock da CPU DESLIGADO.");
            }
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
            string directory = txtDirectory.Text.Trim();
            string fileName = txtFileName.Text.Trim();
            string content = txtFileContent.Text.Trim();

            if (string.IsNullOrWhiteSpace(fileName) || fileName == "Nome do Ficheiro")
            {
                Console.WriteLine("[Erro UI] Por favor, digite um nome válido para o ficheiro.");
                return;
            }

            // Se o utilizador apagar o texto da pasta, guardamos na raiz ("Root")
            if (string.IsNullOrWhiteSpace(directory) || directory == "Root") directory = "Root";

            _controller.CreateFile(directory, fileName, content);
            UpdateFileSystemUI();
        }

        private void BtnReadFile_Click(object sender, RoutedEventArgs e)
        {
            string directory = txtDirectory.Text.Trim() == "Root" ? "Root" : txtDirectory.Text.Trim();
            string fileName = txtFileName.Text.Trim();

            if (string.IsNullOrWhiteSpace(fileName) || fileName == "Nome do Ficheiro") return;

            // O nosso "GPS" junta a pasta e o nome para procurar a chave certa na Árvore B+!
            string fullPath = $"{directory}/{fileName}";
            _controller.ReadFile(fullPath);
        }

        private void BtnDeleteFile_Click(object sender, RoutedEventArgs e)
        {
            string directory = txtDirectory.Text.Trim() == "Root" ? "Root" : txtDirectory.Text.Trim();
            string fileName = txtFileName.Text.Trim();

            if (string.IsNullOrWhiteSpace(fileName) || fileName == "Nome do Ficheiro") return;

            string fullPath = $"{directory}/{fileName}";
            _controller.DeleteFile(fullPath);
            UpdateFileSystemUI();
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