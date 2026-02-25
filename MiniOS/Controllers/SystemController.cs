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
            int ramNeeded = 50;

            // CORRIGIDO: Agora chama "Allocate" em vez de "AllocateMemory"
            int memId = _kernel.MemoryManager.Allocate(ramNeeded);

            if (memId != -1)
            {
                _kernel.ProcessManager.CreateProcess(name, executionTime, memId);
            }
            else
            {
                Console.WriteLine($"[Sistema] BLOQUEADO: Sem memória RAM suficiente para o processo {name}!");
            }
        }

        public void ExecuteNextProcess()
        {
            var finishedProcess = _kernel.ProcessManager.ExecuteNextProcess();

            if (finishedProcess != null)
            {
                // CORRIGIDO: Agora chama "Free" em vez de "FreeMemory"
                _kernel.MemoryManager.Free(finishedProcess.MemoryBlockId);
                Console.WriteLine($"[Sistema] A Memória do {finishedProcess.Name} foi libertada com sucesso!");
            }
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

        public void CreateFile(string directory, string name, string content)
        {
            _kernel.FileSystemManager.CreateFile(directory, name, content);
        }

        public void ReadFile(string name)
        {
            _kernel.FileSystemManager.ReadFile(name);
        }

        public void DeleteFile(string name)
        {
            _kernel.FileSystemManager.DeleteFile(name);
        }

        public System.Collections.Generic.IReadOnlyList<MiniOS.Models.FileEntry> GetAllFiles()
        {
            return _kernel.FileSystemManager.GetAllFiles();
        }
        public int GetReadyQueueCount()
        {
            return _kernel.ProcessManager.GetReadyQueueCount();
        }

    }
}