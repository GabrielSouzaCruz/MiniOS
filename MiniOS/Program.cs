using System;
using System.Windows;
using MiniOS.Controllers;
using MiniOS.Hardware;
using MiniOS.Services;
using MiniOS.Services.Scheduling;
using MiniOS.Views;

namespace MiniOS
{
    class Program
    {
        [STAThread] 
        static void Main()
        {
            // 1. Hardware
            var cpu = new CPU();
            var ram = new RAM(1024); 
            var disk = new HardDisk();

            // 2. Services & Strategy
            var strategy = new RoundRobinStrategy();
            var processManager = new ProcessManager(cpu, strategy);
            var memoryManager = new MemoryManager(ram);
            var fileSystemManager = new FileSystemManager(disk);


            var kernel = new Kernel(processManager, memoryManager, fileSystemManager);

            // 3. Controller
            var controller = new SystemController(kernel);

            // 4. Iniciar a Interface Gráfica (WPF) em vez do ConsoleView!
            var app = new Application();

            // Vamos criar esta janela no próximo passo
            var mainWindow = new MainWindow(controller);

            app.Run(mainWindow);
        }
    }
}