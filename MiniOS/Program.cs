using MiniOS.Controllers;
using MiniOS.Hardware;
using MiniOS.Services;
using MiniOS.Views;

class Program
{
    static void Main()
    {
        // Hardware
        var cpu = new CPU();
        var ram = new RAM();
        var disk = new HardDisk();

        // Services
        var processManager = new ProcessManager(cpu);
        var memoryManager = new MemoryManager(ram);
        var fileSystemManager = new FileSystemManager(disk);

        var kernel = new Kernel(processManager, memoryManager, fileSystemManager);

        // Controller
        var controller = new SystemController(kernel);

        // View
        var view = new ConsoleView(controller);
        view.Start();
    }
}