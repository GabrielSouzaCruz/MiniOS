using MiniOS.Services;
using System.Diagnostics;
using MiniOS.Models;

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
        public IReadOnlyList<Models.Process> GetReadyQueue()
        {
            return _kernel.ProcessManager.GetReadyQueue();
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
        public int GetTotalMemory()
        {
            return _kernel.MemoryManager.GetTotalMemory();
        }

        public int GetUsedMemory()
        {
            return _kernel.MemoryManager.GetUsedMemory();
        }

        public void CreateFile(string name, string content)
        {
            _kernel.FileSystemManager.CreateFile(name, content);
        }
        public void ReadFile(string name)
        {
            _kernel.FileSystemManager.ReadFile(name);
        }
        public System.Collections.Generic.IReadOnlyList<FileEntry> GetAllFiles()
        {
            return _kernel.FileSystemManager.GetAllFiles();
        }
        public int GetReadyQueueCount()
        {
            return _kernel.ProcessManager.GetReadyQueueCount();
        }

    }
}