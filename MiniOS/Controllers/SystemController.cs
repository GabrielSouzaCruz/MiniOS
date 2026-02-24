using MiniOS.Services;

namespace MiniOS.Controllers
{
    public class SystemController
    {
        private readonly Kernel _kernel;

        public SystemController(Kernel kernel)
        {
            _kernel = kernel;
        }

        public void CreateProcess(string name, int executionTime)
        {
            _kernel.ProcessManager.CreateProcess(name, executionTime);
        }
        public void ExecuteNextProcess()
        {
            _kernel.ProcessManager.ExecuteNextProcess();
        }

        public void AllocateMemory(int size)
        {
            _kernel.MemoryManager.Allocate(size);
        }
        public void FreeMemory(int blockId)
        {
            _kernel.MemoryManager.Free(blockId);
        }

        public void ShowMemoryStatus()
        {
            _kernel.MemoryManager.ShowStatus();
        }

        public void CreateFile(string name, string content)
        {
            _kernel.FileSystemManager.CreateFile(name, content);
        }
        public void ReadFile(string name)
        {
            _kernel.FileSystemManager.ReadFile(name);
        }

    }
}